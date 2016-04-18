using Newtonsoft.Json;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Peoples
{
    public class ProducerDto
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "executive")]
        public bool Executive { get; set; }

        [JsonProperty(PropertyName = "images")]
        public PeopleImageDto Image { get; set; }
    }
}