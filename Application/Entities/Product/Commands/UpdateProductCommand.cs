using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Entities.Commands
{
    public class UpdateProductCommand: IRequest<Product>, IUpdateCommand
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string Code { get; set; }
        public ProductStatus Status { get; set; }
    }
}
