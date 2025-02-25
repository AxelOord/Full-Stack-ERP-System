using System.Reflection;

namespace Warehouse.Presentation;

/// <summary>
/// Represents the users module endpoints assembly reference.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// The assembly.
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
