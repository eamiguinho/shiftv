using System.Linq;
using Autofac;
using Shiftv.Contracts.Data.Calendars;
using Shiftv.Contracts.Domain.Calendars;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class CalendarDtoFactory
    {
        public static ICalendar Create(CalendarDto dto)
        {
            var calendar = Ioc.Container.Resolve<ICalendar>();
            calendar.Episodes = dto.Episodes != null ? dto.Episodes.Select(x=>EpisodeDtoFactory.Create(x, dto.Show.Title)).ToList() : null;
            calendar.Show = MiniShowDtoFactory.CreateShow(dto.Show);
            return calendar;
        }
    }
}