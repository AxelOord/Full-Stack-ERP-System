using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using Assembly = System.Reflection.Assembly;

namespace ArchitectureTests;
public abstract class BaseTest
{
    protected static readonly Assembly WarehouseDomainAssembly = Warehouse.Domain.AssemblyReference.Assembly;
    protected static readonly Assembly WarehouseApplicationAssembly = Warehouse.Application.AssemblyReference.Assembly;
    protected static readonly Assembly WarehouseInfrastructureAssembly = Warehouse.Infrastructure.AssemblyReference.Assembly;
    protected static readonly Assembly WarehousePersistenceAssembly = Warehouse.Persistence.AssemblyReference.Assembly;
    protected static readonly Assembly WarehousePresentationAssembly = Warehouse.Presentation.AssemblyReference.Assembly;

    protected static readonly Architecture Architecture = new ArchLoader()
      .LoadAssemblies(
        WarehouseDomainAssembly,
        WarehouseApplicationAssembly,
        WarehouseInfrastructureAssembly,
        WarehousePersistenceAssembly,
        WarehousePresentationAssembly
        )
      .Build();

    protected static readonly IObjectProvider<IType> DomainLayer =
      ArchRuleDefinition.Types().That().ResideInAssembly(WarehouseDomainAssembly).As("Warehouse Domain layer");

    protected static readonly IObjectProvider<IType> ApplicationLayer =
      ArchRuleDefinition.Interfaces().That().ResideInAssembly(WarehouseApplicationAssembly).As("Warehouse Application layer");

    protected static readonly IObjectProvider<IType> InfrastructureLayer =
        ArchRuleDefinition.Interfaces().That().ResideInAssembly(WarehouseInfrastructureAssembly).As("Warehouse Infrastructure layer");

    protected static readonly IObjectProvider<IType> PersistenceLayer =
        ArchRuleDefinition.Interfaces().That().ResideInAssembly(WarehousePersistenceAssembly).As("Warehouse Persistence layer");

    protected static readonly IObjectProvider<IType> PresentationLayer =
        ArchRuleDefinition.Interfaces().That().ResideInAssembly(WarehousePresentationAssembly).As("Warehouse Presentation layer");
}
