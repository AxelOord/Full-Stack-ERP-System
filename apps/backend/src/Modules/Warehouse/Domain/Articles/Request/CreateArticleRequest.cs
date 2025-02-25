using System.ComponentModel.DataAnnotations;

namespace Warehouse.Domain.Articles.Request;

public class CreateArticleRequest
{
    [Required]
    public required string ArticleNumber { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public bool IsActive { get; set; }

    [Required]
    public Guid SupplierId { get; set; }
}
