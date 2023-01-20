using Application.Common.Interfaces;

namespace Application.Common.Services
{
    public class CommandService<TEntity> : ICommandService<TEntity> where TEntity : class
    {
        private readonly IDbContext dbContext;
        public CommandService(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken token)
        {
            await dbContext.Set<TEntity>().AddAsync(entity);
            await dbContext.SaveChangesAsync(token);
            return entity;
        }

        public async Task<int> DeleteAsync(TEntity entity, CancellationToken token)
        {
            dbContext.Set<TEntity>().Remove(entity);
            var result = await dbContext.SaveChangesAsync(token);
            return result;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken token)
        {
            dbContext.Set<TEntity>().Update(entity);
            await dbContext.SaveChangesAsync(token);
            return entity;
        }
    }
}
