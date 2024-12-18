using System.Text.Json.Serialization;

namespace Domain.Shared.ApiResponse
{
    public class ApiRelationship
    {
        [JsonPropertyName("links")]
        public ApiLinks Links { get; set; }

        [JsonPropertyName("data")]
        public object Data { get; set; }
    }
}
