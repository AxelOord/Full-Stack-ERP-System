using Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;
using Warehouse.Domain.Articles.Dtos;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Domain.Articles
{
  public class Article : Entity
  {

    public string ArticleNumber { get; private set; }
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public Guid SupplierId { get; set; }

    [ForeignKey("SupplierId")]
    public Supplier Supplier { get; private set; }

    public Article() { }

    private Article(Guid id, string articleNumber, string name, bool isActive, Supplier supplier)
      : base(id)
    {
      ArticleNumber = articleNumber;
      Name = name;
      IsActive = isActive;
      Supplier = supplier;
      SupplierId = supplier.Id;
    }

    public static Article Create(CreateArticleDto Dto, Supplier supplier)
    {
      return new Article(Guid.NewGuid(), Dto.ArticleNumber, Dto.Name, Dto.IsActive, supplier);
    }
  }
}
