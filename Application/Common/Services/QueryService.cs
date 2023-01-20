using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Services
{
    public class QueryService<TEntity> : IQueryService<TEntity> where TEntity : class
    {
        private readonly IDbContext dbContext;
        public QueryService(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<TEntity> GetAll(CancellationToken token)
        {
            var result = dbContext.Set<TEntity>();
            return result.AsQueryable<TEntity>();
        }

        public Task<TEntity?> GetById(TEntity entity, CancellationToken token)
        {
            var result = dbContext.Set<TEntity>().FirstOrDefaultAsync(token); //TODO: set base entity => x => x.Id == entity.Id, 
            return result;
        }
    }
}
