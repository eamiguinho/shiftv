using System.Collections.Generic;
using Shiftv.Contracts.Domain.Peoples;

namespace Shiftv.Core.Models.Peoples
{
    class People : IPeople
    {
        public List<ICast> Cast { get; set; }
        public ITeam Crew { get; set; }
    }
}