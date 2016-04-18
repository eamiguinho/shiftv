using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shiftv.Contracts.DataServices.Calendars;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Calendars
{
    public class CalendarTraktQueryService : ICalendarTraktQueryService
    {
        public Task<string> GetCalendarQuery()
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}", 
                TraktConstants.ShiftvBaseApiUrl,
                TraktConstants.CalendarResource,
                TraktConstants.MyResource));
        }

       
    }
}
