using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Results
{
    public class NetworkFollowResultDto
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "pending")]
        public bool IsPending { get; set; }
    }
}