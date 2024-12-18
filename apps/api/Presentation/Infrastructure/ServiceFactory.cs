using Application.Generics.Create;
using Application.Generics.Delete;
using Application.Generics.GetAll;
using Application.Generics.GetById;
using Domain.Primitives;

namespace Api.Infrastructure
{
    public class ServiceFactory(IServiceProvider serviceProvider) : IServiceFactory
    {
        public IGetByIdService<TEntity> GetGetByIdService<TEntity>() where TEntity : Entity, new()
            => serviceProvider.GetRequiredService<IGetByIdService<TEntity>>();

        public IGetAllService<TEntity> GetGetAllService<TEntity>() where TEntity : Entity, new()
            => serviceProvider.GetRequiredService<IGetAllService<TEntity>>();

        public ICreateService<TDto, TEntity> GetCreateService<TDto, TEntity>() where TEntity : Entity, new() where TDto : Dto
            => serviceProvider.GetRequiredService<ICreateService<TDto, TEntity>>();

        public IDeleteService<TEntity> GetDeleteService<TEntity>() where TEntity : Entity, new()
            => serviceProvider.GetRequiredService<IDeleteService<TEntity>>();
    }
}
