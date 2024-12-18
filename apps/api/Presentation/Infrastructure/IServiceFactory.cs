using Application.Generics.Create;
using Application.Generics.Delete;
using Application.Generics.GetAll;
using Application.Generics.GetById;
using Domain.Primitives;

namespace Api.Infrastructure
{
    public interface IServiceFactory
    {
        IGetByIdService<TEntity> GetGetByIdService<TEntity>() where TEntity : Entity, new();
        IGetAllService<TEntity> GetGetAllService<TEntity>() where TEntity : Entity, new();
        ICreateService<TDto, TEntity> GetCreateService<TDto, TEntity>() where TEntity : Entity, new() where TDto : Dto;
        IDeleteService<TEntity> GetDeleteService<TEntity>() where TEntity : Entity, new();
    }
}
