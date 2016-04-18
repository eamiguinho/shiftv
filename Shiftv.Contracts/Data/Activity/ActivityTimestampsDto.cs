using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Activity
{
    public class ActivityTimestampsDto
    {
        [JsonProperty(PropertyName = "start")]
        public int? Start { get; set; }

        [JsonProperty(PropertyName = "end")]
        public int? End { get; set; }

        [JsonProperty(PropertyName = "current")]
        public int? Current { get; set; }
    }
}