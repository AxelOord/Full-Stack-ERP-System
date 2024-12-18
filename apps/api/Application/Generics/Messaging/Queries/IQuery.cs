using Domain.Shared;
using MediatR;

namespace Application.Generics.Messaging.Queries
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {

    }
}
