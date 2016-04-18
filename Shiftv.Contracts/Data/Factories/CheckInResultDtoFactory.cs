using Autofac;
using Shiftv.Contracts.Data.Results;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class CheckInResultDtoFactory
    {
        public static ICheckinResult Create(CheckInResultDto dto)
        {
            var checkIn = Ioc.Container.Resolve<ICheckinResult>();
            checkIn.Error = dto.Error;
            checkIn.Facebook = dto.Facebook;
            checkIn.Message = dto.Message;
            checkIn.Path = dto.Path;
            checkIn.Show = dto.Show != null ? CheckInShowResultDtoFactory.Create(dto.Show) : null;
            switch (dto.Status)
            {
                case "success":
                    checkIn.Status = RequestResults.Success;
                    break;
                default:
                    checkIn.Status = RequestResults.Failure;
                    break;
            }
            checkIn.TimeStamps = dto.Timestamps != null ? CheckInTimeStampsResultDtoFactory.Create(dto.Timestamps) : null;
            checkIn.Tumblr = dto.Tumblr;
            checkIn.Twitter = dto.Twitter;
            checkIn.Wait = dto.Wait;
            return checkIn;
        }


        public class CheckInTimeStampsResultDtoFactory
        {
            public static ICheckInTimestampsResult Create(CheckInTimeStampsResultDto dto)
            {
                var timestamps = Ioc.Container.Resolve<ICheckInTimestampsResult>();
                timestamps.ActiveFor = dto.ActiveFor;
                timestamps.End = dto.End;
                timestamps.Start = dto.Start;
                return timestamps;
            }
        }

        public class CheckInShowResultDtoFactory
        {
            public static ICheckInShowResult Create(CheckInShowResultDto dto)
            {
                var show = Ioc.Container.Resolve<ICheckInShowResult>();
                show.ImdbId = dto.ImdbId;
                show.Title = dto.Title;
                show.TvdbId = dto.TvdbId;
                show.Year = dto.Year;
                return show;
            }
        }
    }
}