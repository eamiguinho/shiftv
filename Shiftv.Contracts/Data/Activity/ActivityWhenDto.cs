using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Activity
{
    public class ActivityWhenDto
    {
        [JsonProperty(PropertyName = "day")]
        public string Day { get; set; }

        [JsonProperty(PropertyName = "time")]
        public string Time { get; set; }
    }
}