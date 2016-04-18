using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BackgroundTasks
{
    public sealed class CalendarDto
    {
        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "episodes", ItemTypeNameHandling = TypeNameHandling.Auto)]
        public List<CalendarDataDto> CalendarItems { get; set; }
    }

    public sealed class CalendarDataDto
    {
        [JsonProperty(PropertyName = "show", ItemTypeNameHandling = TypeNameHandling.Auto)]
        public ShowDto Show { get; set; }

        [JsonProperty(PropertyName = "episode", ItemTypeNameHandling = TypeNameHandling.Auto)]
        public EpisodeDto Episode { get; set; }
    }

}
