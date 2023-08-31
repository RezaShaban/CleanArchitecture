using Application.Common.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (bool.Parse(configuration.GetSection("UseInMemoryDatabase").Value ?? "false"))
            {
                //TODO: enable inMemory mode
                //services.AddDbContext<CommandDbContext>(options =>
                //    options.UseInMemoryDatabase("CleanArchitectureDb"));
            }
            else
            {
                string connectionString = configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<CommandDbContext>(options =>
                    options.UseSqlServer(connectionString,
                        b => b.MigrationsAssembly(typeof(CommandDbContext).Assembly.FullName)));
            }

            services.AddScoped<IDbContext, CommandDbContext>(provider => provider.GetRequiredService<CommandDbContext>());

            services.AddScoped<IDomainEventService, DomainEventService>();

            services
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CommandDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, CommandDbContext>();

            services.AddTransient<ITimeService, TimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            //services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

            services.AddAuthentication().AddIdentityServerJwt();

            services.AddAuthorization(options =>
                options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

            return services;
        }
    }
}