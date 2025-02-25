using MediatR;
using Shared.Results;

namespace Application.Messaging.Queries;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
