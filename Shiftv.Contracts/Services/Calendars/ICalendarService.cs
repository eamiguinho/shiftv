using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Calendars;
using Shiftv.Global;

namespace Shiftv.Contracts.Services.Calendars
{
    public interface ICalendarService
    {
        Task<DataResult<List<ICalendar>>> GetCalendar();

    }
}   