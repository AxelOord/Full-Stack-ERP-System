using Autofac;
using Application.Generics.GetById;
using Application.Generics.GetAll;
using Application.Generics.Delete;
using Persistence.Generics.Queries;
using Persistence.Generics.Commands;

namespace Persistence
{
  public static class AutofacServiceRegistration
  {
    public static void ConfigureAutofacContainer(this ContainerBuilder containerBuilder)
    {
      // Register generic services because they cant be registered automatically by Autofac due to parity issues
      // Slow for the first call, but fast for the next calls. This is because the first call will create the service, but the next calls will just resolve it.
      // This is a workaround for the lack of support for generic services in Autofac
      // If a better solution is found, this should be removed
      containerBuilder.Register<Func<Type, Type, Type, object>>(c =>
      {
        var ctxFactory = c.Resolve<IComponentContext>();
        return (entityType, serviceType, DbContextType) =>
        {
          var dbContext = ctxFactory.Resolve(DbContextType);

          var resolvedServiceType = serviceType switch
          {
            Type t when t == typeof(IGetByIdService<>) => typeof(GetByIdService<,>).MakeGenericType(DbContextType, entityType),
            Type t when t == typeof(IGetAllService<>) => typeof(GetAllService<,>).MakeGenericType(DbContextType, entityType),
            Type t when t == typeof(IDeleteService<>) => typeof(DeleteService<,>).MakeGenericType(DbContextType, entityType),
            _ => throw new InvalidOperationException("Unknown service type")
          };

          return Activator.CreateInstance(resolvedServiceType, dbContext);
        };
      });
    }
  }
}
