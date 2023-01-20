namespace Domain.Events;
public class EntityEvent<TEntity> : DomainEvent
{
    public EntityEvent(TEntity item)
    {
        Item = item;
    }

    public TEntity Item { get; }
}
