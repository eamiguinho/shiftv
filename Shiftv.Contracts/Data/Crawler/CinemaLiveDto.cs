using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Crawler
{
    public class CinemaLiveDto
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "filename")]
        public string Filename { get; set; }
        [JsonProperty(PropertyName = "player_type")]
        public string PlayerType { get; set; }
        [JsonProperty(PropertyName = "hostel")]
        public string Hostel { get; set; }
    }

    public class CinemaLiveVk
    {
        [JsonProperty(PropertyName = "url240")]
        public string url240 { get; set; }

        [JsonProperty(PropertyName = "url360")]
        public string url360 { get; set; }
        [JsonProperty(PropertyName = "url480")]
        public string url480 { get; set; }
        [JsonProperty(PropertyName = "url720")]
        public string url720 { get; set; }
    }
}
