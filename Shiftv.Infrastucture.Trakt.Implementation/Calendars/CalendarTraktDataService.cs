using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Shiftv.Contracts.Data.Calendars;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices.Calendars;
using Shiftv.Contracts.Domain.Calendars;
using Shiftv.Core.Models.Calendars;
using Shiftv.Global;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Calendars
{
    public class CalendarTraktDataService : ICalendarTraktDataService
    {
        private ICalendarTraktQueryService _queryService;

        private ICalendarTraktQueryService QueryService { get
        {
            return _queryService ?? (_queryService = Ioc.Container.Resolve<ICalendarTraktQueryService>());
        } }
        public Task<List<ICalendar>> GetCalendar(UserTokenDto token)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var req = await QueryService.GetCalendarQuery();
                    var x = await TraktDataServiceHelper.GetObjectWithCredentials<List<CalendarDto>>(req, token);
                    return x.Select(CalendarDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

      
    }

   
}
