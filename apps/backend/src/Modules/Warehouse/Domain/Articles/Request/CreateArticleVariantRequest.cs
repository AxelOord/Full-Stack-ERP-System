using System.ComponentModel.DataAnnotations;

namespace Warehouse.Domain.Articles.Request;
public class CreateArticleVariantRequest
{
    [Required]
    public required string Color { get; set; }

    [Required]

    public double Width { get; set; }

    [Required]

    public bool IsActive { get; set; }

    [Required]
    public Guid ArticleId { get; set; }
}
