namespace Application.Common.Interfaces
{
    public interface ICommandService<TEntity>
    {
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken token);
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken token);
        Task<int> DeleteAsync(TEntity entity, CancellationToken token);
    }
}
