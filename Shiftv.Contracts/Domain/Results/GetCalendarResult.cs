using System.Collections.Generic;
using Shiftv.Contracts.Domain.Calendars;

namespace Shiftv.Contracts.Domain.Results
{
    public class GetCalendarResult : ResultBase
    {
        public List<ICalendar> Data { get; set; }

        public static GetCalendarResult Ok(List<ICalendar> calendar)
        {
            return new GetCalendarResult { Result = Results.Ok, Data = calendar };
        }

        public static GetCalendarResult Error()
        {
            return new GetCalendarResult { Result = Results.Error };
        }
    }
}