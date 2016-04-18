using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Crawler
{
    public class NameMapDto
    {
        [JsonProperty(PropertyName = "ImdbId")]
        public string ImdbId { get; set; }

        [JsonProperty(PropertyName = "SourceCrawler")]
        public string SourceCrawler { get; set; }

        [JsonProperty(PropertyName = "NameMapped")]
        public string NameMapped { get; set; }

        [JsonProperty(PropertyName = "IsCountRestart")]
        public bool IsCountRestart { get; set; }

        [JsonProperty(PropertyName = "RestartAt")]
        public int RestartAt { get; set; }
    }
}
