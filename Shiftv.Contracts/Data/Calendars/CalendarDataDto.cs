using Newtonsoft.Json;
using Shiftv.Contracts.Data.Shows;

namespace Shiftv.Contracts.Data.Calendars
{
    public class CalendarDataDto
    {
        [JsonProperty(PropertyName = "show", ItemTypeNameHandling = TypeNameHandling.Auto)]
        public ShowDto Show { get; set; }

        [JsonProperty(PropertyName = "episode", ItemTypeNameHandling = TypeNameHandling.Auto)]
        public EpisodeDto Episode { get; set; }
    }

}
