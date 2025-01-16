namespace Domain.Primitives.Interfaces
{
  public interface ICreatableFromDto<out TEntity, in TDto>
    where TEntity : IEntity
    where TDto : IDto
  {
    static abstract TEntity Create(TDto dto);
  }
}
