using Application.Generics.Delete;
using Application.Generics.GetAll;
using Application.Generics.GetById;
using Domain.Primitives.Interfaces;

namespace Infrastructure
{
  public interface IServiceFactory
  {
    IGetByIdService<TEntity> GetGetByIdService<TEntity>() where TEntity : IEntity;
    IGetAllService<TEntity> GetGetAllService<TEntity>() where TEntity : IEntity;
    IDeleteService<TEntity> GetDeleteService<TEntity>() where TEntity : IEntity;
  }
}
