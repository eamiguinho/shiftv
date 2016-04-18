using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Shows
{
    public class ImdbRatingDto
    {
        [JsonProperty(PropertyName = "imdbRating")]
        public double ImdbRating { get; set; }
    }
}