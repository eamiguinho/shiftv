using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Results
{
    public class CheckInResultDto
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "timestamps")]
        public CheckInTimeStampsResultDto Timestamps { get; set; }

        [JsonProperty(PropertyName = "show")]
        public CheckInShowResultDto Show { get; set; }

        [JsonProperty(PropertyName = "facebook")]
        public bool Facebook { get; set; }

        [JsonProperty(PropertyName = "twitter")]
        public bool Twitter { get; set; }

        [JsonProperty(PropertyName = "tumblr")]
        public bool Tumblr { get; set; }

        [JsonProperty(PropertyName = "path")]
        public bool Path { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "wait")]
        public int Wait { get; set; }

    }

    public class CheckInTimeStampsResultDto
    {
        [JsonProperty(PropertyName = "start")]

        public int Start { get; set; }
        [JsonProperty(PropertyName = "end")]

        public int End { get; set; }
        [JsonProperty(PropertyName = "active_for")]

        public int ActiveFor { get; set; }
    }

    public class CheckInShowResultDto
    {
        [JsonProperty(PropertyName = "title")]

        public string Title { get; set; }
        [JsonProperty(PropertyName = "year")]

        public int Year { get; set; }
        [JsonProperty(PropertyName = "imdb_id")]

        public string ImdbId { get; set; }
        [JsonProperty(PropertyName = "tvdb_id")]

        public int TvdbId { get; set; }
    }
}