using System.Collections.Generic;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Shows;

namespace Shiftv.Contracts.Data.Calendars
{
    public class CalendarDto    
    {
        [JsonProperty(PropertyName = "show")]
        public MiniShowDto Show { get; set; }

        [JsonProperty(PropertyName = "episodes")]
        public List<EpisodeDto> Episodes { get; set; }
    }
}   
