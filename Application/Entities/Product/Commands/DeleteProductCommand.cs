using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System.ComponentModel;

namespace Application.Entities.Commands
{
    public class DeleteProductCommand: IRequest<Product>, IDeleteCommand
    {
        public long Id { get; set; }
    }
}
