using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.JsonTrakt
{
    public class EditCommentRequestJsonDto
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "comment_id")]
        public int CommentId { get; set; }

        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }

        [JsonProperty(PropertyName = "spoiler")]
        public bool Spoiler { get; set; }

        [JsonProperty(PropertyName = "review")]
        public bool Review { get; set; }
    }
}