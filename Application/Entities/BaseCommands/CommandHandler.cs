using Application.Common.Interfaces;
using Domain.Common;
using Domain.Enums;
using Domain.Events;
using MediatR;

namespace Application.Entities.BaseCommands;
public class CommandHandler<TCommand, TEntity> : IRequestHandler<TCommand, TEntity>
    where TCommand : IRequest<TEntity>
{
    private readonly ICommandService<TEntity> crudService;

    public CommandHandler(ICommandService<TEntity> crudService)
    {
        this.crudService = crudService;
    }

    public async Task<TEntity> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var entity = Activator.CreateInstance(typeof(TEntity));
        request.GetType().GetProperties().Select(x =>
        {
            entity?.GetType().GetProperty(x.Name)?.SetValue(entity, x.GetValue(request));
            return x;
        }).ToList();

        TEntity result = default(TEntity);

        if (typeof(TEntity).IsAssignableTo(typeof(IHasDomainEvent)))
            ((IHasDomainEvent)entity).DomainEvents.Add(new EntityEvent<TEntity>((TEntity)entity));

        if (typeof(TCommand).IsAssignableTo(typeof(ICreateCommand)))
            result = await crudService.CreateAsync((TEntity)entity, cancellationToken);

        if (typeof(TCommand).IsAssignableTo(typeof(IUpdateCommand)))
            result = await crudService.UpdateAsync((TEntity)entity, cancellationToken);

        if (typeof(TCommand).IsAssignableTo(typeof(IDeleteCommand)))
            await crudService.DeleteAsync((TEntity)entity, cancellationToken);

        return result;
    }
}
