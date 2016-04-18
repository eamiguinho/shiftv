using System.Collections.Generic;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Shows;

namespace Shiftv.Contracts.Data.JsonTrakt
{
    public class AddToWatchListRequestJsonDto
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "shows")]
        public List<ShowDto> Shows { get; set; }
    }
}