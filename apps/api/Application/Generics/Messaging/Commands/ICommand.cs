using Domain.Shared;
using MediatR;

namespace Application.Generics.Messaging.Commands
{
    public interface ICommand : IRequest<Result>
    {
    }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
