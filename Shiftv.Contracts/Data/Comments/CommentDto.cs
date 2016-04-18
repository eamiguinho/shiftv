using Newtonsoft.Json;
using Shiftv.Contracts.Data.Images;
using Shiftv.Contracts.Data.Users;

namespace Shiftv.Contracts.Data.Comments
{
    public class CommentDto
    {
        //[JsonProperty(PropertyName = "poster")]
        //public string Poster { get; set; }

        //[JsonProperty(PropertyName = "id")]
        //public int Id { get; set; }

        //[JsonProperty(PropertyName = "inserted")]
        //public long Inserted { get; set; }

        //[JsonProperty(PropertyName = "text")]
        //public string Text { get; set; }

        //[JsonProperty(PropertyName = "text_html")]
        //public string TextHtml { get; set; }

        //[JsonProperty(PropertyName = "spoiler")]
        //public bool IsSpoiler { get; set; }

        //[JsonProperty(PropertyName = "type")]
        //public string Type { get; set; }

        //[JsonProperty(PropertyName = "likes")]
        //public int Likes { get; set; }

        //[JsonProperty(PropertyName = "replies")]
        //public int Replies { get; set; }

        //[JsonProperty(PropertyName = "user")]
        //public UserProfileDto User { get; set; }

        //[JsonProperty(PropertyName = "imdbid")]
        //public string ImdbId { get; set; }

        //[JsonProperty(PropertyName = "episode")]
        //public int? Episode { get; set; }
        //[JsonProperty(PropertyName = "season")]
        //public int? Season { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "parent_id")]
        public int? ParentId { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty(PropertyName = "comment")]
        public string CommentText { get; set; }
        [JsonProperty(PropertyName = "spoiler")]

        public bool Spoiler { get; set; }
        [JsonProperty(PropertyName = "review")]

        public bool Review { get; set; }
        [JsonProperty(PropertyName = "replies")]

        public int Replies { get; set; }
        [JsonProperty(PropertyName = "likes")]
        public int Likes { get; set; }

        [JsonProperty(PropertyName = "user")]
        public UserDto User { get; set; }
    }

    public class UserDto
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "@private")]
        public bool Private { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "vip")]
        public bool Vip { get; set; }

        [JsonProperty(PropertyName = "joined_at")]
        public string JoinedAt { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "about")]
        public string About { get; set; }

        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }

        [JsonProperty(PropertyName = "age")]
        public int? Age { get; set; }

        [JsonProperty(PropertyName = "images")]
        public ImageDto Images { get; set; }
    }
}
