using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Media
{
    public class SubtitlesInfoDto
    {
        [JsonProperty(PropertyName = "SubtitlesLink")]
        public string SubtitlesLink { get; set; }

        [JsonProperty(PropertyName = "Language")]
        public string Language { get; set; }    
        
        [JsonProperty(PropertyName = "LanguageId")]
        public string LanguageId { get; set; }

        [JsonProperty(PropertyName = "SubtitleFileName")]
        public string SubtitleFileName { get; set; }
    }
}