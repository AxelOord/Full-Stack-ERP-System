using Domain.Primitives;
using Domain.Shared.ApiResponse;
using System.ComponentModel.DataAnnotations;

namespace Domain.Organizations
{
    public class SupplierDto : Dto
    {
        [Required]
        [TranslationKey("COLUMN_NAME_SBN_ID")]
        [Sortable]
        public required string SbnId { get; set; }

        [Required]
        [TranslationKey("COLUMN_NAME_NAME")]
        [Sortable(false)]
        public required string Name { get; set; }

        [TranslationKey("COLUMN_NAME_IS_ACTIVE")]
        [Sortable]
        public bool IsActive { get; set; }
    }
}
