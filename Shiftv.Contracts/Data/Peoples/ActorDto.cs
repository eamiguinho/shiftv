using Newtonsoft.Json;
using Shiftv.Contracts.Domain.Peoples;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Peoples
{
    public class ActorDto   
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "character")]
        public string Character { get; set; }

        [JsonProperty(PropertyName = "images")]
        public PeopleImageDto Image { get; set; }
    }
}