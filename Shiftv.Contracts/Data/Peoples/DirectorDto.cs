using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Peoples
{
    public class DirectorDto
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "images")]
        public PeopleImageDto Image { get; set; }
    }
}