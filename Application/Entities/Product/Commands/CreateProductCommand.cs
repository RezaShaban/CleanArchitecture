using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Entities.Commands
{
    public class CreateProductCommand : IRequest<Product>, ICreateCommand
    {
        public string? Name { get; set; }
        public string Code { get; set; }
        public ProductStatus Status { get; set; }
    }

}
