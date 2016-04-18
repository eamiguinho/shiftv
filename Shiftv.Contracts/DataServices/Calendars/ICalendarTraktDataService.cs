using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Data.Calendars;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Calendars;

namespace Shiftv.Contracts.DataServices.Calendars
{
    public interface ICalendarTraktDataService
    {
        Task<List<ICalendar>> GetCalendar(UserTokenDto token);
    }
}