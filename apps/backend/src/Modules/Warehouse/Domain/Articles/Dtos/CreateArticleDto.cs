using Domain.Primitives.Interfaces;
using Shared.Results.Response;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Domain.Articles.Dtos
{
  public class CreateArticleDto : IDto
  {
    [Required]
    [TranslationKey("COLUMN_NAME_ARTICLE_NUMBER")]
    [Sortable]
    public required string ArticleNumber { get; set; }

    [Required]
    [TranslationKey("COLUMN_NAME_NAME")]
    [Sortable(false)]
    public required string Name { get; set; }

    [TranslationKey("COLUMN_NAME_IS_ACTIVE")]
    [Sortable]
    public bool IsActive { get; set; }

    [TranslationKey("COLUMN_NAME_SUPPLIER_ID")]
    [Sortable]
    public Guid SupplierId { get; set; }
  }
}
