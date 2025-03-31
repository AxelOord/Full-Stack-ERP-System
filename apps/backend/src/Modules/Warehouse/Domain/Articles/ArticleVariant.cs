using Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;
using Warehouse.Domain.Articles.Request;

namespace Warehouse.Domain.Articles;

public sealed class ArticleVariant : Entity
{
    public string Color { get; private set; }
    public double Width { get; private set; }
    public bool IsActive { get; private set; }
    public Guid ArticleId { get; private set; }

    [ForeignKey("ArticleId")]
    public Article Article { get; private set; }

    private ArticleVariant() { }

    private ArticleVariant(Guid id, string color, double width, bool isActive, Article article)
        : base(id)
    {
        Color = color;
        Width = width;
        IsActive = isActive;
        Article = article;
        ArticleId = article.Id;
    }

    public static ArticleVariant Create(CreateArticleVariantRequest request, Article article)
    {
        return new ArticleVariant(Guid.NewGuid(), request.Color, request.Width, request.IsActive, article);
    }
}
