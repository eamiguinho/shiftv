using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShiftvAPI.Contracts.Data
{
    public class Episode
    {
        [JsonProperty(PropertyName = "season")]
        public int Season { get; set; }

        [JsonProperty(PropertyName = "number")]
        public int Number { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "ids")]
        public Ids Ids { get; set; }

        [JsonProperty(PropertyName = "number_abs")]
        public int? NumberAbs { get; set; }

        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [JsonProperty(PropertyName = "first_aired")]
        public string FirstAired { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public double? Rating { get; set; }

        [JsonProperty(PropertyName = "votes")]
        public int? Votes { get; set; }

        [JsonProperty(PropertyName = "available_translations")]
        public List<object> AvailableTranslations { get; set; }

        [JsonProperty(PropertyName = "images")]
        public Images Images { get; set; }

        [JsonProperty(PropertyName = "user_rating")]
        public int? UserRating { get; set; }


        [JsonProperty(PropertyName = "watched")]
        public bool Watched { get; set; }   
    }
}