using Shared.Results.Response;

namespace Warehouse.Domain.Articles.Dto;

public class ArticleVariantDto
{
    [TranslationKey("COLUMN_NAME_ARTICLE_COLOR")]
    [FilterPath("Supplier[name]")]
    [Sortable]
    public required string Color { get; set; }

    [TranslationKey("COLUMN_NAME_ARTICLE_WIDTH")]
    [FilterPath("Supplier[name]")]
    [Sortable]
    public double Width { get; set; }

    [TranslationKey("COLUMN_NAME_VARIANT_IS_ACTIVE")]
    [FilterPath("Supplier[name]")]
    [Sortable]
    public bool IsActive { get; set; }
}
