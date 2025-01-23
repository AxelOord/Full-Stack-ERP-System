using Domain.Organizations.Suppliers;
using Domain.Primitives;
using Domain.Primitives.Interfaces;
using Domain.Warehouse.Article.Dto;

namespace Domain.Warehouse.Article
{
  public class Article : Entity<Article, ArticleDto>
  {

    public string ArticleNumber { get; private set; }
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public Guid SupplierId { get; set; }
    public Supplier Supplier { get; }

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

    public static Article Create(ArticleDto Dto, Supplier supplier)
    {
      return new Article(Guid.NewGuid(), Dto.ArticleNumber, Dto.Name, Dto.IsActive, supplier);
    }
  }
}
