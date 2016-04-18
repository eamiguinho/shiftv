using System;
using System.Threading.Tasks;

namespace Shiftv.Contracts.DataServices.Calendars
{
    public interface ICalendarTraktQueryService
    {
        Task<string> GetCalendarQuery();
    }
}
