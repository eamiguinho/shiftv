using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Calendars;
using Shiftv.Contracts.Data.Comments;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Calendars;
using Shiftv.Contracts.Domain.Comments;
using Shiftv.Contracts.Services.Calendars;
using Shiftv.Global;

namespace Shiftv.DesignServices.Implementation
{
    public class CalendarDesignService : ICalendarService
    {
        public Task<DataResult<List<ICalendar>>> GetCalendar(DateTime date, string username)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetCalendar.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<CalendarDto>>(jsonString);
                return new DataResult<List<ICalendar>>(tracksCollection.Select(CalendarDtoFactory.Create).ToList());
            });
        }

        public Task<DataResult<List<ICalendar>>> GetCalendar(DateTime date, UserTokenDto token)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<List<ICalendar>>> GetCalendarFull(DateTime date)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetCalendar.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<CalendarDto>>(jsonString);
                return new DataResult<List<ICalendar>>(tracksCollection.Select(CalendarDtoFactory.Create).ToList());
            });
        }

        public Task<DataResult<List<ICalendar>>> GetCalendar(bool date)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetCalendar.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<CalendarDto>>(jsonString);
                return new DataResult<List<ICalendar>>(tracksCollection.Select(CalendarDtoFactory.Create).ToList());
            });
        }

        public Task<DataResult<List<ICalendar>>> GetFullCalendar()
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetCalendar.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<CalendarDto>>(jsonString);
                return new DataResult<List<ICalendar>>(tracksCollection.Select(CalendarDtoFactory.Create).ToList());
            });
        }

        public Task<DataResult<List<ICalendar>>> GetCalendar()
        {
            throw new NotImplementedException();
        }
    }
}