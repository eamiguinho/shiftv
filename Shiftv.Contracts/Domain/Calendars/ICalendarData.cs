using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Contracts.Domain.Calendars
{
    public interface ICalendarData 
    {
        IShow Show { get; set; }
        IEpisode Episode { get; set; }
    }
}
