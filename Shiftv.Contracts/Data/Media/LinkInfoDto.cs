using Newtonsoft.Json;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Contracts.Data.Media
{
    public class LinkInfoDto
    {
        [JsonProperty(PropertyName = "StreamLink")]
        public string StreamLink { get; set; }

        [JsonProperty(PropertyName = "OriginalLink")]
        public string OriginalLink { get; set; }

        [JsonProperty(PropertyName = "Quality")]
        public StreamQuality Quality { get; set; }

        [JsonProperty(PropertyName = "Velocity")]
        public StreamVelocity Velocity { get; set; }
        [JsonProperty(PropertyName = "FileSizeFormatted")]
        public string FileSize { get; set; }
        [JsonProperty(PropertyName = "EmbbedLink")]
        public string EmbbedLink { get; set; }
                [JsonProperty(PropertyName = "IsCached")]

        public bool IsCached { get; set; }
    }
}