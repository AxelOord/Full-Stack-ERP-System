using System.Reflection;

namespace Warehouse.Persistence;

/// <summary>
/// Represents the users module persistence assembly reference.
/// </summary>
public static class AssemblyReference
{
  /// <summary>
  /// The assembly.
  /// </summary>
  public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
