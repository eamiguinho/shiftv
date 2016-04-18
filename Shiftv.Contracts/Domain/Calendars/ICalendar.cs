using System.Collections.Generic;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Contracts.Domain.Calendars
{
    public interface ICalendar
    {
        IMiniShow Show { get; set; }

        List<IEpisode> Episodes { get; set; }
    }
}
