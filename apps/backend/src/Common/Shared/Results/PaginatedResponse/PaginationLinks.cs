using System.Text.Json.Serialization;

namespace Shared.Results.PaginatedResponse
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
