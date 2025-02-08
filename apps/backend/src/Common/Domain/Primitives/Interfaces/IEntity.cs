using System.Runtime.InteropServices;

namespace Domain.Primitives.Interfaces
{
  public interface IEntity
  {
    Guid Id { get; }
    DateTime InsertDate { get; }
    DateTime UpdateDate { get; }
  }
}
