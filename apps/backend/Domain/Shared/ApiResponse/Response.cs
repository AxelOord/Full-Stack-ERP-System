using Domain.Primitives.Interfaces;
using System.Text.Json.Serialization;

namespace Domain.Shared.ApiResponse
{
  public class Response<T> : IResponse<T>
    where T : IDto
  {
    [JsonPropertyName("data")]
    public List<ApiData<T>>? Data { get; set; }

    [JsonPropertyName("metadata")]
    public Metadata? Metadata { get; set; }
    public string? Message { get; set; }
  }
}
