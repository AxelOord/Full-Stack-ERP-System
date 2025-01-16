using Application.Generics.Create;
using AutoMapper;
using Domain.Primitives;
using Domain.Primitives.Interfaces;
using Domain.Shared;

namespace Infrastructure.Persistence.Generics.Commands
{
  public sealed class CreateService<TDto, TEntity> : ICreateService<TDto, TEntity>
      where TDto : IDto
      where TEntity : class, IEntity, ICreatableFromDto<TEntity, TDto>
  {
    private readonly ApplicationDbContext _context;

    public CreateService(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<Result> ExecuteAsync(CreateCommand<TDto, TEntity> request, CancellationToken cancellationToken)
    {
      var entity = TEntity.Create(request.Dto);

      _context.Set<TEntity>().Add(entity);
      await _context.SaveChangesAsync(cancellationToken);

      return Result.Success();
    }
  }
}
