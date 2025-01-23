using Application.Generics.Delete;
using Application.Generics.GetAll;
using Application.Generics.GetById;
using Domain.Primitives.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
  public class ServiceFactory(IServiceProvider serviceProvider) : IServiceFactory
  {
    public IGetByIdService<TEntity> GetGetByIdService<TEntity>() where TEntity : IEntity
        => serviceProvider.GetRequiredService<IGetByIdService<TEntity>>();

    public IGetAllService<TEntity> GetGetAllService<TEntity>() where TEntity : IEntity
        => serviceProvider.GetRequiredService<IGetAllService<TEntity>>();

    public IDeleteService<TEntity> GetDeleteService<TEntity>() where TEntity : IEntity
        => serviceProvider.GetRequiredService<IDeleteService<TEntity>>();
  }
}
