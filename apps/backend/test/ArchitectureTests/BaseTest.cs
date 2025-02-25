using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using Assembly = System.Reflection.Assembly;

namespace ArchitectureTests;
public abstract class BaseTest
{
    protected static readonly Assembly WarehouseDomainAssembly = Warehouse.Domain.AssemblyReference.Assembly;

    protected static readonly Architecture Architecture = new ArchLoader()
      .LoadAssemblies(WarehouseDomainAssembly)
      .Build();

    protected static readonly IObjectProvider<IType> ObjectProvider =
      ArchRuleDefinition.Types().That().ResideInAssembly(WarehouseDomainAssembly).As("Warehouse Domain layer");
}
