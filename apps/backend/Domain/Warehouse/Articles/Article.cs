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
    public Supplier? Supplier { get; }

    private Article(Guid id, string articleNumber, string name, bool isActive)
      : base(id)
    {
      ArticleNumber = articleNumber;
      Name = name;
      IsActive = isActive;
    }

    public static Article Create(ArticleDto Dto)
    {
      return new Article(Guid.NewGuid(), Dto.ArticleNumber, Dto.Name, Dto.IsActive);
    }
  }
}
