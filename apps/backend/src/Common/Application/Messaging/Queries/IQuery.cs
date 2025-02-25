using MediatR;
using Shared.Results;

namespace Application.Messaging.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{

}
