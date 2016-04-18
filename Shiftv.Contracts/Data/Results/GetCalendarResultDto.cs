using System.Collections.Generic;
using Shiftv.Contracts.Data.Calendars;

namespace Shiftv.Contracts.Domain.Results
{
    public class GetCalendarResultDto : ResultBase
    {
        public List<CalendarDto> Data { get; set; }

        public static GetCalendarResultDto Ok(List<CalendarDto> calendar)
        {
            return new GetCalendarResultDto { Result = Results.Ok, Data = calendar };
        }

        public static GetCalendarResultDto Error()
        {
            return new GetCalendarResultDto { Result = Results.Error };
        }
    }
}