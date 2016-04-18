using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShiftvAPI.Contracts.Data.PostObjects
{
    public class RateRequestJsonDto
    {
        [JsonProperty(PropertyName = "rating")]
        public int Rating { get; set; }

        [JsonProperty(PropertyName = "ids")]
        public Ids Ids { get; set; }
    }
}
