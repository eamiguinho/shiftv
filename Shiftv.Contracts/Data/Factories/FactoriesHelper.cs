using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shiftv.Contracts.Data.Factories
{
    public static class FactoriesHelper
    {
        public static DateTime GetDateTimeFromUtcTicks(long utcOffsetTicks)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date = epoch.AddSeconds(utcOffsetTicks);
            return date.ToLocalTime();
        }
    }
}
