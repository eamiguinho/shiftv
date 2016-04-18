using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShiftvAPI.Contracts.Data
{


    public class User
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
        public Images Images { get; set; }
    }

    public class Comment
    {
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
        public User User { get; set; }
    }
}
