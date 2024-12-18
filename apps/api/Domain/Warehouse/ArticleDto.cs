using Domain.Primitives;
using Domain.Shared.ApiResponse;
using System.ComponentModel.DataAnnotations;

namespace Domain.Articles
{
    public class ArticleDto : Dto
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
    }
}
