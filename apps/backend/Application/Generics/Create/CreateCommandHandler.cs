using Application.Generics.Messaging.Commands;
using Domain.Primitives;
using Domain.Primitives.Interfaces;
using Domain.Shared;

namespace Application.Generics.Create
{
  public class CreateCommandHandler<TDto, TEntity> : ICommandHandler<CreateCommand<TDto, TEntity>>
        where TDto : IDto
        where TEntity : class, IEntity, ICreatableFromDto<TEntity, TDto>
  {

        private readonly ICreateService<TDto, TEntity> _createService;

        public CreateCommandHandler(ICreateService<TDto, TEntity> createService)
        {
            _createService = createService;
        }

        public async Task<Result> Handle(CreateCommand<TDto, TEntity> request, CancellationToken cancellationToken)
        {
            await _createService.ExecuteAsync(request, cancellationToken);

            return Result.Success();
        }
    }
}
