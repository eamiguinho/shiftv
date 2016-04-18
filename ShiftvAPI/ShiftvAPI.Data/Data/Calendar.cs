using Newtonsoft.Json;

namespace ShiftvAPI.Contracts.Data
{
    public class Calendar
    {
        [JsonProperty(PropertyName = "airs_at")]
        public string AirsAt { get; set; }

        [JsonProperty(PropertyName = "episode")]
        public Episode Episode { get; set; }

        [JsonProperty(PropertyName = "show_ids")]
        public Ids ShowIds { get; set; }  

        [JsonProperty(PropertyName = "show_name")]  
        public string ShowName { get; set; }

        [JsonProperty(PropertyName = "show")]
        public Show Show { get; set; }

        [JsonProperty(PropertyName = "groupKey")]
        public string GroupKey { get; set; }
    }
}