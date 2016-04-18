using Autofac;
using Shiftv.Contracts.Data.Activity;
using Shiftv.Contracts.Domain.Activity;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class ActivityTimestampsDtoFactory
    {
        public static IActivityTimestamps Create(ActivityTimestampsDto timestampsDto)
        {
            var timestamps = Ioc.Container.Resolve<IActivityTimestamps>();
            timestamps.Current = timestampsDto.Current.Value;
            timestamps.End = timestampsDto.End.Value;
            timestamps.Start = timestampsDto.Start.Value;
            return timestamps;
        }
    }
}