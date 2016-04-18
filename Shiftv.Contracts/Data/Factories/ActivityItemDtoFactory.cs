using Autofac;
using Shiftv.Contracts.Data.Activity;
using Shiftv.Contracts.Domain.Activity;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class ActivityItemDtoFactory
    {
        public static IActivityItem Create(ActivityItemDto dto)
        {
            if (dto == null) return null;
            var activityItem = Ioc.Container.Resolve<IActivityItem>();
            switch (dto.Action)
            {
                case "watching":
                    activityItem.Action = ActivityActions.Watching;
                    break;
                case "checkin":
                    activityItem.Action = ActivityActions.Checkin;
                    break;
                case "collection":
                    activityItem.Action = ActivityActions.Collection;
                    break;
                case "rating":
                    activityItem.Action = ActivityActions.Rating;
                    break;
                case "review":
                    activityItem.Action = ActivityActions.Review;
                    break;
                case "scrobble":
                    activityItem.Action = ActivityActions.Scrobble;
                    break;
                case "seen":
                    activityItem.Action = ActivityActions.Seen;
                    break;
                case "shout":
                    activityItem.Action = ActivityActions.Shout;
                    break;
                case "watchlist":
                    activityItem.Action = ActivityActions.Watchlist;
                    break;
                default:
                    activityItem.Action = ActivityActions.Unknown;
                    break;
            }
            switch (dto.Type)
            {
                case "episode":
                    activityItem.Type = ActivityTypes.Episode;
                    activityItem.Show = ShowDtoFactory.CreateShow(dto.Show);
                    activityItem.Episode = EpisodeDtoFactory.Create(dto.Episode, dto.Show.Title);
                    break;
                case "movie":
                    activityItem.Type = ActivityTypes.Movie;
                    activityItem.Movie = MovieDtoFactory.Create(dto.Movie);
                    break;
                case "show":
                    activityItem.Show = ShowDtoFactory.CreateShow(dto.Show);
                    activityItem.Type = ActivityTypes.Show;
                    break;
                default:
                    activityItem.Type = ActivityTypes.Unknown;
                    break;  
            }
            activityItem.Id = dto.Id;
            activityItem.Elapsed = ActivityElapsedDtoFactory.Create(dto.Elapsed);
            activityItem.IsRatingAdvanced = dto.IsRatingAdvanced;
            activityItem.Rating = dto.Rating;
            activityItem.RatingAdvanced = dto.RatingAdvanced;
            activityItem.Timestamp = dto.Timestamp;
            //activityItem.User = UserProfileDtoFactory.Create(dto.User);
            activityItem.When = ActivityWhenDtoFactory.Create(dto.When);
            return activityItem;
        }
    }
}