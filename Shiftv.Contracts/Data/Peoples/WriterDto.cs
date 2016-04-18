using Newtonsoft.Json;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Peoples
{
    public class WriterDto 
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "job")]
        public string Job { get; set; }

        [JsonProperty(PropertyName = "images")]
        public PeopleImageDto Image { get; set; }
    }
}