using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Peoples
{
    public class PeopleImageDto
    {
        [JsonProperty(PropertyName = "headshot")]
        public string Headshot { get; set; }
    }
}