using System.Collections.Generic;
using Newtonsoft.Json;

namespace Shiftv.Contracts.Domain.Peoples
{
    public interface ITeam
    {
        [JsonProperty(PropertyName = "production")]
        List<IProduction> Production { get; set; }

        [JsonProperty(PropertyName = "camera")]
        List<ICamera> Camera { get; set; }

        [JsonProperty(PropertyName = "art")]
        List<IArt> Art { get; set; }

        [JsonProperty(PropertyName = "crew")]
        List<ICrew> Crew { get; set; }

        [JsonProperty(PropertyName = "costume & make-up")]
        List<ICostumeMakeUp> CostumeMakeUp { get; set; }

        [JsonProperty(PropertyName = "directing")]
        List<IDirecting> Directing { get; set; }

        [JsonProperty(PropertyName = "writing")]
        List<IWriting> Writing { get; set; }

        [JsonProperty(PropertyName = "sound")]
        List<ISound> Sound { get; set; }
    }
}