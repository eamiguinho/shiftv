using System.Collections.Generic;
using Shiftv.Contracts.Domain.Calendars;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Core.Models.Calendars
{
    public class Calendar : ICalendar
    {
        public IMiniShow Show { get; set; }

        public List<IEpisode> Episodes { get; set; }
    }
}   
