using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Shiftv.Contracts.Data.Calendars;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices;
using Shiftv.Contracts.Domain.Calendars;
using Shiftv.Contracts.Services;
using Shiftv.Contracts.Services.Accounts;
using Shiftv.Contracts.Services.Calendars;
using Shiftv.Global;

namespace Shiftv.Services.Implementation.Calendars
{
    class CalendarService : ServiceHelper, ICalendarService
    {
        private IUserService _userService;

        public CalendarService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<DataResult<List<ICalendar>>> GetCalendar()
        {
            //if (!await IsInternet()) return new DataResult<List<ICalendar>>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            var res = await TraktDataService.Calendar.GetCalendar(UserTokenDtoFactory.GetDto(user));
            if (res == null) return new DataResult<List<ICalendar>>(StandardResults.Error);
            return new DataResult<List<ICalendar>>(res);
        }
  
    }
}
