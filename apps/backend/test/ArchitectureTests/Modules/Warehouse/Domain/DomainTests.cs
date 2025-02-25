using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;
using Domain.Primitives;
using FluentAssertions;
using Xunit;

// these test should be run on all the modules that have a Domain layer
// but for now it is only run on the Warehouse module
// just something to keep in mind when more modules are added
namespace ArchitectureTests.Modules.Warehouse.Domain;
public class DomainTests : BaseTest
{

    [Fact]
    public void Entities_Should_HavePrivateParameterLessConstructors()
    {
        IEnumerable<Class> entityTypes = ArchRuleDefinition
          .Classes()
          .That()
          .AreAssignableTo(typeof(Entity))
          .GetObjects(Architecture);

        var failingTypes = new List<Class>();
        foreach (Class entityType in entityTypes)
        {
            IEnumerable<MethodMember>? constructors = entityType.GetConstructors();

            if (!constructors.Any(c => c.Visibility == Visibility.Private && !c.Parameters.Any()))
            {
                failingTypes.Add(entityType);
            }
        }

        failingTypes.Should().BeEmpty();
    }
}
