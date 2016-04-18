using System.Collections.Generic;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Results;

namespace Shiftv.Contracts.Data.Media
{
    public class MediaStreamDto
    {
        [JsonProperty(PropertyName = "ImdbId")]
        public string ImdbId { get; set; }

        [JsonProperty(PropertyName = "Links")]
        public List<LinkInfoDto> Links { get; set; }

        [JsonProperty(PropertyName = "Subtitles")]
        public List<SubtitlesInfoDto> Subtitles { get; set; }
    }
}