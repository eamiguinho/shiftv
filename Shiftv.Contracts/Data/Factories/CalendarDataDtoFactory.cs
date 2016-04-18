using Autofac;
using Shiftv.Contracts.Data.Calendars;
using Shiftv.Contracts.Domain.Calendars;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class CalendarDataDtoFactory
    {
        public static ICalendarData  Create(CalendarDataDto dto)
        {
            var calendarData = Ioc.Container.Resolve<ICalendarData>();
            calendarData.Show = ShowDtoFactory.CreateShow(dto.Show);
            calendarData.Episode = EpisodeDtoFactory.Create(dto.Episode,dto.Show.Title);
            return calendarData;
        }
    }
}