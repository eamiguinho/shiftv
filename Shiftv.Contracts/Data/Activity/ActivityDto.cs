using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Activity
{
    public class ActivityDto
    {
        [JsonProperty(PropertyName = "timestamps")]
        public ActivityTimestampsDto Timestamps { get; set; }

        [JsonProperty(PropertyName = "activity")]
        public List<ActivityItemDto> ActivityItems { get; set; }
    }
}
