using Newtonsoft.Json;
using Shiftv.Contracts.Data.Shows;

namespace Shiftv.Contracts.Data.JsonTrakt
{
    public class ShowRequestJsonDto
    {
        [JsonProperty(PropertyName = "ids")]
        public IdsDto Ids { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int? Year { get; set; }
    }

    public class MovieRequestJsonDto
    {
        [JsonProperty(PropertyName = "ids")]
        public IdsDto Ids { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int? Year { get; set; }
    }

    public class EpisodeRequestJsonDto
    {
        [JsonProperty(PropertyName = "ids")]
        public IdsDto Ids { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "number")]
        public int Season { get; set; }

        [JsonProperty(PropertyName = "season")]
        public int Number { get; set; }
    }

    public class CommentRequestJsonDto
    {
        [JsonProperty(PropertyName = "show")]
        public ShowRequestJsonDto Show { get; set; }

        [JsonProperty(PropertyName = "movie")]
        public MovieRequestJsonDto Movie { get; set; }

        [JsonProperty(PropertyName = "episode")]
        public EpisodeRequestJsonDto Episode { get; set; }  

        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }

        [JsonProperty(PropertyName = "spoiler")]
        public bool Spoiler { get; set; }

        [JsonProperty(PropertyName = "review")]
        public bool Review { get; set; }

    
    }
}
