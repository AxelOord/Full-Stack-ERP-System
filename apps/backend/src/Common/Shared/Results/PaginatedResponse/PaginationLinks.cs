using Shared.Results.Response;
using System.Text.Json.Serialization;

namespace Shared.Results.PaginatedResponse;

public class PaginationLinks
{
    [JsonPropertyName("self")]
    [JsonRequired]
    public required Link Self { get; set; }

    [JsonPropertyName("next")]
    [NullableProp]
    public Link? Next { get; set; }

    [JsonPropertyName("prev")]
    [NullableProp]
    public Link? Prev { get; set; }
}
