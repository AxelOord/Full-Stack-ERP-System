using Domain.Primitives;
using Warehouse.Domain.Articles;
using Warehouse.Domain.Suppliers.Request;

namespace Warehouse.Domain.Suppliers;

public sealed class Supplier : Entity
{
    public string Name { get; private set; }
    public int SbnId { get; private set; }
    public bool IsActive { get; private set; }

    public ICollection<Article> Articles { get; set; } = [];

    private Supplier() { }

    private Supplier(Guid id, string name, int sbnId, bool isActive)
      : base(id)
    {
        Name = name;
        SbnId = sbnId;
        IsActive = isActive;
    }

    public static Supplier Create(CreateSupplierRequest Dto)
    {
        return new Supplier(Guid.NewGuid(), Dto.Name, Dto.SbnId, Dto.IsActive);
    }
}
