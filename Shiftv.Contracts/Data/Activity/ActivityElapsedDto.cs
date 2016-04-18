using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Activity
{
    public class ActivityElapsedDto
    {
        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }

        [JsonProperty(PropertyName = "short")]
        public string Short { get; set; }
    }
}