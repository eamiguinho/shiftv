using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Results
{
    public class NetworkApproveDenyResultDto
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}