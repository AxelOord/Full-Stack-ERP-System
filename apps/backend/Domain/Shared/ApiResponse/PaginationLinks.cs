using System.Text.Json.Serialization;

namespace Domain.Shared.ApiResponse
{
    public class PaginationLinks
    {
        [JsonPropertyName("self")]
        public required string Self { get; set; }

        [JsonPropertyName("next")]
        public string? Next { get; set; }

        [JsonPropertyName("prev")]
        public string? Prev { get; set; }
    }
}
