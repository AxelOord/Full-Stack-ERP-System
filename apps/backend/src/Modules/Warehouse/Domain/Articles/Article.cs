using Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;
using Warehouse.Domain.Articles.Request;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Domain.Articles;

public sealed class Article : Entity
{
    public string ArticleNumber { get; private set; }
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public Guid SupplierId { get; private set; }

    [ForeignKey("SupplierId")]
    public Supplier Supplier { get; private set; }

    private readonly List<ArticleVariant> _variants = new();
    public IReadOnlyCollection<ArticleVariant> Variants => _variants.AsReadOnly();

    private Article() { }

    private Article(Guid id, string articleNumber, string name, bool isActive, Supplier supplier)
            : base(id)
    {
        ArticleNumber = articleNumber;
        Name = name;
        IsActive = isActive;
        Supplier = supplier;
        SupplierId = supplier.Id;
    }

    public static Article Create(CreateArticleRequest Dto, Supplier supplier)
    {
        return new Article(Guid.NewGuid(), Dto.ArticleNumber, Dto.Name, Dto.IsActive, supplier);
    }
}
