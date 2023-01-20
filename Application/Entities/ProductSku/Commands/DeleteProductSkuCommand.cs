using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Entities.Commands
{
    public class DeleteProductSkuCommand: IRequest<ProductSku>, IDeleteCommand
    {
        public long Id { get; set; }
    }
}
