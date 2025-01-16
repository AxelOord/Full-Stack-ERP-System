using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Domain.Primitives.Interfaces;
using Domain.Warehouse.Article.Dto;
using Domain.Warehouse.Article;

namespace Domain.Primitives
{
  public abstract class Entity<TEntity, TDto> : IEntity, ICreatableFromDto<TEntity, TDto>
    where TEntity : class, IEntity, ICreatableFromDto<TEntity, TDto>
    where TDto : IDto
  {
    public Guid Id { get; private init; }
    public DateTime InsertDate { get; private init; }
    public DateTime UpdateDate { get; private init; }

    protected Entity(Guid id)
    {
          Id = id;
          InsertDate = DateTime.Now;
          UpdateDate = DateTime.Now;
    }

    protected Entity() { }

    static TEntity ICreatableFromDto<TEntity, TDto>.Create(TDto dto)
    {
      throw new NotImplementedException();
    }
  }
}
