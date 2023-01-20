using MediatR;

namespace Application.Common.Interfaces
{
    public interface IDeleteCommand
    {
        public long Id { get; set; }
    }
    public interface ICreateCommand { }
    public interface IUpdateCommand { }
}