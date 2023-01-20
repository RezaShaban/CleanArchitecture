namespace Application.Common.Interfaces
{
    public interface IQueryService<TEntity>
    {
        Task<TEntity?> GetById(TEntity entity, CancellationToken token);
        IQueryable<TEntity> GetAll(CancellationToken token);
    }
}
