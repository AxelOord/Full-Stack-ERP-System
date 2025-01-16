using Application.Generics.Messaging.Commands;
using Domain.Primitives.Interfaces;
using Domain.Shared;

namespace Application.Generics.Delete
{
    public class DeleteCommandHandler<TEntity> : ICommandHandler<DeleteCommand<TEntity>>
        where TEntity : IEntity, new()
    {

        private readonly IDeleteService<TEntity> _deleteService;

        public DeleteCommandHandler(IDeleteService<TEntity> deleteService)
        {
            _deleteService = deleteService;
        }

        public async Task<Result> Handle(DeleteCommand<TEntity> request, CancellationToken cancellationToken)
        {
            var result = await _deleteService.ExecuteAsync(request, cancellationToken);

            return result;
        }
    }
}
