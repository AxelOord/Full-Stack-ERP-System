using Domain.Organizations.Suppliers.Dtos;
using Domain.Primitives.Interfaces;
using Domain.Shared.ApiResponse;
using System.ComponentModel.DataAnnotations;

namespace Domain.Warehouse.Article.Dto
{
  public class ArticleDto : IDto
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

    [TranslationKey("COLUMN_NAME_SUPPLIER")]
    [Sortable]
    public SupplierDto? Supplier { get; set; }
  }
}
