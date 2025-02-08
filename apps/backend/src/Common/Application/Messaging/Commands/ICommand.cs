using MediatR;
using Shared.Results;

namespace Application.Messaging.Commands
{
  public interface ICommand : IRequest<Result>
  {
  }

  public interface ICommand<TResponse> : IRequest<Result<TResponse>>
  {
  }
}
