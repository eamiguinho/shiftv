using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Stats
{
    public class RatingDto
    {
        [JsonProperty(PropertyName = "percentage")]
        public float Percentage { get; set; }

        [JsonProperty(PropertyName = "votes")]
        public int Votes { get; set; }

        [JsonProperty(PropertyName = "loved")]
        public int Loved { get; set; }

        [JsonProperty(PropertyName = "hated")]
        public int Hated { get; set; }

        [JsonProperty(PropertyName = "distribution")]
        public DistributionDto Distribution { get; set; }
    }

    public class DistributionDto
    {
        [JsonProperty(PropertyName = "1")]
        public int Star1 { get; set; }
        [JsonProperty(PropertyName = "2")]
        public int Star2 { get; set; }
        [JsonProperty(PropertyName = "3")]
        public int Star3 { get; set; }
        [JsonProperty(PropertyName = "4")]
        public int Star4 { get; set; }
        [JsonProperty(PropertyName = "5")]
        public int Star5 { get; set; }
        [JsonProperty(PropertyName = "6")]
        public int Star6 { get; set; }
        [JsonProperty(PropertyName = "7")]
        public int Star7 { get; set; }
        [JsonProperty(PropertyName = "8")]
        public int Star8 { get; set; }
        [JsonProperty(PropertyName = "9")]
        public int Star9 { get; set; }
        [JsonProperty(PropertyName = "10")]
        public int Star10 { get; set; }
    }
}