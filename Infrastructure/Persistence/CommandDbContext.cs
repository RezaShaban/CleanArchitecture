using Application.Common.Interfaces;
using Domain.Common;
using Duende.IdentityServer.EntityFramework.Options;
using Infrastructure.Identity;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Infrastructure.Persistence
{
    public class CommandDbContext : ApiAuthorizationDbContext<ApplicationUser>, IDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ITimeService _timeService;
        private readonly IDomainEventService _domainEventService;

        public CommandDbContext(DbContextOptions<CommandDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            ICurrentUserService currentUserService,
            IDomainEventService domainEventService,
            ITimeService timeService
            ) : base(options, operationalStoreOptions)

        {
            _currentUserService = currentUserService;
            _domainEventService = domainEventService;
            _timeService = timeService;

            Migrate();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            long.TryParse(_currentUserService.UserId, out long userId);
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreateUserId = userId;
                        entry.Entity.CreateDate = _timeService.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastUpdateUserId = userId;
                        entry.Entity.LastUpdateDate = _timeService.Now;
                        break;
                }
            }

            var events = ChangeTracker.Entries<IHasDomainEvent>()
                    .Select(x => x.Entity.DomainEvents)
                    .SelectMany(x => x)
                    .Where(domainEvent => !domainEvent.IsPublished)
                    .ToArray();

            var result = await base.SaveChangesAsync(cancellationToken);

            await DispatchEvents(events);
            return result;
        }

        public void Migrate()
        {
            Database.EnsureCreated();
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            AddEntities(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        private async Task DispatchEvents(DomainEvent[] events)
        {
            foreach (var @event in events)
            {
                @event.IsPublished = true;
                await _domainEventService.Publish(@event);
            }
        }

        private void AddEntities(ModelBuilder modelBuilder)
        {
            var candidates = GetEntities();

            var entityMethod = typeof(ModelBuilder).GetMethod("Entity", new Type[] { });

            foreach (var item in candidates)
            {
                entityMethod.MakeGenericMethod(item)
                    .Invoke(modelBuilder, new object[] { });
            }
        }

        private IEnumerable<Type> GetEntities()
        {
            return typeof(AuditableEntity).Assembly.GetExportedTypes().Where(x => x.Namespace == ("Domain.Entities")); //&& !x.GetCustomAttributes(typeof(NoEntityAttribute), false).Any()
        }
    }
}
