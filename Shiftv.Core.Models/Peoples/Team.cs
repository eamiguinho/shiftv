using System.Collections.Generic;
using Shiftv.Contracts.Domain.Peoples;

namespace Shiftv.Core.Models.Peoples
{
    class Team : ITeam
    {
        public List<IProduction> Production { get; set; }
        public List<ICamera> Camera { get; set; }
        public List<IArt> Art { get; set; }
        public List<ICrew> Crew { get; set; }
        public List<ICostumeMakeUp> CostumeMakeUp { get; set; }
        public List<IDirecting> Directing { get; set; }
        public List<IWriting> Writing { get; set; }
        public List<ISound> Sound { get; set; }
    }
}