using Domain.Primitives;
using Warehouse.Domain.Articles;
using Warehouse.Domain.Suppliers.Dtos;

namespace Warehouse.Domain.Suppliers
{
  public class Supplier : Entity
  {
    public string Name { get; private set; }
    public int SbnId { get; private set; }
    public bool IsActive { get; private set; }

    public ICollection<Article> Articles { get; set; } = new List<Article>();

    private Supplier(Guid id, string name, int sbnId, bool isActive)
      : base(id)
    {
      Name = name;
      SbnId = sbnId;
      IsActive = isActive;
    }

    public static Supplier Create(CreateSupplierDto Dto)
    {
      return new Supplier(Guid.NewGuid(), Dto.Name, Dto.SbnId, Dto.IsActive);
    }
  }
}
