using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Comments
{
    public class CommentResultDto
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
