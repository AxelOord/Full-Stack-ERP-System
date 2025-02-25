using System.Text.Json.Serialization;

namespace Shared.Results.Response;

public class AttributeLinks
{
    [JsonPropertyName("self")]
    public required string Self { get; set; }
}
