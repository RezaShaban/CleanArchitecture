using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Entities.Commands
{
    public class UpdateProductSkuCommand: IRequest<ProductSku>, IUpdateCommand
    {
        public long Id { get; set; }
        public string Sku { get; set; }
        public decimal SalePrice { get; set; }
    }
}
