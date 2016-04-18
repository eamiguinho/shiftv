using Newtonsoft.Json;
using Shiftv.Contracts.Data.JsonTrakt;
namespace Shiftv.Contracts.Data.Results
{
    public class SetAsSeenResultJsonDto
    {
        [JsonProperty(PropertyName = "request")]
        public SetAsSeenJsonDto Request { get; set; }
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }
    }
}