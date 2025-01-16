using System.Text.Json.Serialization;

namespace Domain.Shared.ApiResponse
{
  public class AttributeLinks
  {
    [JsonPropertyName("self")]
    public required string Self { get; set; }
  }
}
