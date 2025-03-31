using Domain.Primitives.Interfaces;
using Shared.Results.Response;
using System.ComponentModel.DataAnnotations;
using DataTypeAttribute = Shared.Results.Response.DataTypeAttribute;

namespace Warehouse.Domain.Articles.Dto;

public class ArticleDto : IDto
{
    [TranslationKey("COLUMN_NAME_SUPPLIER_NAME")]
    [FilterPath("Supplier[name]")]
    [Sortable]
    public required string SupplierName { get; set; }

    [Required]
    [TranslationKey("COLUMN_NAME_ARTICLE_NUMBER")]
    [Sortable]
    public required string ArticleNumber { get; set; }

    [Required]
    [TranslationKey("COLUMN_NAME_NAME")]
    [Sortable]
    public required string Name { get; set; }

    [TranslationKey("COLUMN_NAME_IS_ACTIVE")]
    [DataType("boolean")]
    [Sortable(false)]
    public bool IsActive { get; set; }

    [Expandable]
    public List<ApiData<ArticleVariantDto>> Variants { get; set; } = new();
}