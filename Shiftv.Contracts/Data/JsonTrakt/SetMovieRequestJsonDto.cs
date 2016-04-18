using System.Collections.Generic;
using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.JsonTrakt
{
    public class SetMovieRequestJsonDto
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "movies")]
        public List<SetMovieInfoRequestJsonDto> Movies { get; set; }
    }
}