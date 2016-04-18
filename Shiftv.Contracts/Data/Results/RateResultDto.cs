using Newtonsoft.Json;
using Shiftv.Contracts.Data.JsonTrakt;
using Shiftv.Contracts.Data.Stats;

namespace Shiftv.Contracts.Data.Results
{
    public class RateResultDto
    {
        [JsonProperty(PropertyName = "request")]
        public RateRequestJsonDto Request { get; set; }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; } 
    }
}
