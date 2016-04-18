using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShiftvAPI.Contracts.Data
{
    public class TraktList
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "privacy")]
        public string Privacy { get; set; }

        [JsonProperty(PropertyName = "display_numbers")]
        public bool DisplayNumbers { get; set; }

        [JsonProperty(PropertyName = "allow_comments")]
        public bool AllowComments { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "item_count")]
        public int? ItemCount { get; set; }

        [JsonProperty(PropertyName = "likes")]
        public int? Likes { get; set; }

        [JsonProperty(PropertyName = "ids")]
        public Ids Ids { get; set; }

        [JsonProperty(PropertyName = "items")]
        public List<TraktListItemMini> Items { get; set; }
    }


    public class TraktListItem
    {
        [JsonProperty(PropertyName = "listed_at")]
        public string ListedAt { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "show")]
        public Show Show { get; set; }

        [JsonProperty(PropertyName = "movie")]
        public Movie Movie { get; set; }
    }

    public class TraktListItemMini
    {
        [JsonProperty(PropertyName = "listed_at")]
        public string ListedAt { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "show")]
        public MiniShow Show { get; set; }

        [JsonProperty(PropertyName = "movie")]
        public MiniMovie Movie { get; set; }
    }
}
