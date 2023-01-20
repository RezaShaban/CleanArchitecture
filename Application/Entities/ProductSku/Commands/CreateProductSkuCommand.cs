using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Entities.Commands
{
    public class CreateProductSkuCommand: IRequest<ProductSku>, ICreateCommand
    {
        public string Sku { get; set; }
        public decimal SalePrice { get; set; }
        public long ProductId { get; set; }
    }
}
