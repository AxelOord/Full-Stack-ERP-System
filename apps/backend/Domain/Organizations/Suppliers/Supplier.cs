using Domain.Organizations.Suppliers.Dtos;
using Domain.Primitives;
using Domain.Primitives.Interfaces;

namespace Domain.Organizations.Suppliers
{
  public class Supplier : Entity<Supplier, SupplierDto>
  {
    public string Name { get; private set; }
    public int SbnId { get; private set; }
    public bool IsActive { get; private set; }

    private Supplier(Guid id, string name, int sbnId, bool isActive)
      : base(id)
    {
      Name = name;
      SbnId = sbnId;
      IsActive = isActive;
    }

    public static Supplier Create(SupplierDto Dto)
    {
      return new Supplier(Guid.NewGuid(), Dto.Name, Dto.SbnId, Dto.IsActive);
    }
  }
}
