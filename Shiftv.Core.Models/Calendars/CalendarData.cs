using Shiftv.Contracts.Domain.Calendars;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Core.Models.Calendars
{
    class CalendarData : ICalendarData
    {
        public IShow Show { get; set; }

        public IEpisode Episode { get; set; }
    }
}
