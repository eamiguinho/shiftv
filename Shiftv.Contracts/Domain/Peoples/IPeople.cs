using System.Collections.Generic;
using Shiftv.Contracts.Data.Images;
using Shiftv.Contracts.Data.Peoples;
using Shiftv.Contracts.Data.Shows;

namespace Shiftv.Contracts.Domain.Peoples
{
    public interface IPeople
    {
        List<ICast> Cast { get; set; }

        ITeam Crew { get; set; }
    }

    public interface IGlobalDataPeople
    {
        string Job { get; set; }
        IPerson Person { get; set; }
    }
}