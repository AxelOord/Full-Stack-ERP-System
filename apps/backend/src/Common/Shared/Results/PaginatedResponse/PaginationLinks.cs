using Newtonsoft.Json;
using Shared.Results.Response;

namespace Shared.Results.PaginatedResponse
{
    public class PaginationLinks
    {
        [JsonProperty("self")]
        [JsonRequired]
        public required Link Self { get; set; }

        [JsonProperty("next", NullValueHandling = NullValueHandling.Ignore)]
        [NullableProp]
        public Link? Next { get; set; }

        [JsonProperty("prev", NullValueHandling = NullValueHandling.Ignore)]
        [NullableProp]
        public Link? Prev { get; set; }
    }
}
