using Autofac;
using Shiftv.Contracts.Data.Movies;
using Shiftv.Contracts.Domain.Activity;
using Shiftv.Contracts.Domain.Calendars;
using Shiftv.Contracts.Domain.Categories;
using Shiftv.Contracts.Domain.Comments;
using Shiftv.Contracts.Domain.Crawler;
using Shiftv.Contracts.Domain.Images;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Peoples;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Stats;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Core.Models.Activities;
using Shiftv.Core.Models.Calendars;
using Shiftv.Core.Models.Categories;
using Shiftv.Core.Models.Comments;
using Shiftv.Core.Models.Crawler;
using Shiftv.Core.Models.Global;
using Shiftv.Core.Models.Images;
using Shiftv.Core.Models.Movies;
using Shiftv.Core.Models.Peoples;
using Shiftv.Core.Models.Results;
using Shiftv.Core.Models.Shows;
using Shiftv.Core.Models.Stats;
using Shiftv.Core.Models.Users;
using Shiftv.Global;

namespace Shiftv.Core.Models
{
    public static class CoreModelsIoc
    {
        public static void RegisterTypes()
        {
            Ioc.Instance.RegisterType<Comment>().As<IComment>();
            Ioc.Instance.RegisterType<Show>().As<IShow>();
            Ioc.Instance.RegisterType<Movie>().As<IMovie>();
            Ioc.Instance.RegisterType<Actor>().As<IActor>();
            Ioc.Instance.RegisterType<Episode>().As<IEpisode>();
            Ioc.Instance.RegisterType<Image>().As<IImage>();
            Ioc.Instance.RegisterType<People>().As<IPeople>();
            Ioc.Instance.RegisterType<PeopleImage>().As<IPeopleImage>();
            Ioc.Instance.RegisterType<Rating>().As<IRating>();
            Ioc.Instance.RegisterType<GeneralStats>().As<IGeneralStats>();
            Ioc.Instance.RegisterType<Season>().As<ISeason>();
            Ioc.Instance.RegisterType<Calendar>().As<ICalendar>();
            Ioc.Instance.RegisterType<CalendarData>().As<ICalendarData>();
            Ioc.Instance.RegisterType<RateResult>().As<IRateResult>();
            Ioc.Instance.RegisterType<CommentResult>().As<ICommentResult>();
            Ioc.Instance.RegisterType<GenericPostResult>().As<IGenericPostResult>();
            Ioc.Instance.RegisterType<Statistics>().As<IStatistics>();
            Ioc.Instance.RegisterType<StatsCheckins>().As<IStatsCheckins>();
            Ioc.Instance.RegisterType<StatsCollection>().As<IStatsCollection>();
            Ioc.Instance.RegisterType<StatsLists>().As<IStatsLists>();
            Ioc.Instance.RegisterType<StatsScrobbles>().As<IStatsScrobbles>();
            Ioc.Instance.RegisterType<StatsComments>().As<IStatsComments>();
            Ioc.Instance.RegisterType<Distribution>().As<IDistribution>();
            Ioc.Instance.RegisterType<CheckinResult>().As<ICheckinResult>();
            Ioc.Instance.RegisterType<CheckInTimestampsResult>().As<ICheckInTimestampsResult>();
            Ioc.Instance.RegisterType<CheckInShowResult>().As<ICheckInShowResult>();
            Ioc.Instance.RegisterType<Category>().As<ICategory>();
            Ioc.Instance.RegisterType<Activity>().As<IActivity>();
            Ioc.Instance.RegisterType<ActivityItem>().As<IActivityItem>();
            Ioc.Instance.RegisterType<ActivityElapsed>().As<IActivityElapsed>();
            Ioc.Instance.RegisterType<ActivityTimestamps>().As<IActivityTimestamps>();
            Ioc.Instance.RegisterType<ActivityWhen>().As<IActivityWhen>();
            Ioc.Instance.RegisterType<NetworkApproveDenyResult>().As<INetworkApproveDenyResult>();
            Ioc.Instance.RegisterType<NetworkFollowResult>().As<INetworkFollowResult>();
            Ioc.Instance.RegisterType<ShowProgress>().As<IShowProgress>();

            Ioc.Instance.RegisterType<MediaStream>().As<IMediaStream>();
            Ioc.Instance.RegisterType<LinkInfo>().As<ILinkInfo>();
            Ioc.Instance.RegisterType<SubtitlesInfo>().As<ISubtitlesInfo>();
            Ioc.Instance.RegisterType<SubtitlesLanguage>().As<ISubtitlesLanguage>();
            Ioc.Instance.RegisterType<NameMap>().As<INameMap>();
            Ioc.Instance.RegisterType<SeasonProgress>().As<ISeasonProgress>();
            Ioc.Instance.RegisterType<ShowProgressStats>().As<IShowProgressStats>();
            Ioc.Instance.RegisterType<EpisodesProgress>().As<IEpisodesProgress>();




            Ioc.Instance.RegisterType<Ids>().As<IIds>();
            Ioc.Instance.RegisterType<Airs>().As<IAirs>();
            Ioc.Instance.RegisterType<Fanart>().As<IFanart>();
            Ioc.Instance.RegisterType<Banner>().As<IBanner>();
            Ioc.Instance.RegisterType<Poster>().As<IPoster>();
            Ioc.Instance.RegisterType<Headshot>().As<IHeadshot>();
            Ioc.Instance.RegisterType<Headshot>().As<IHeadshot>();
            Ioc.Instance.RegisterType<ThumbImage>().As<IThumb>();
            Ioc.Instance.RegisterType<Logo>().As<ILogo>();
            Ioc.Instance.RegisterType<Clearart>().As<IClearart>();
            Ioc.Instance.RegisterType<Avatar>().As<IAvatar>();

            Ioc.Instance.RegisterType<Person>().As<IPerson>();
            Ioc.Instance.RegisterType<Camera>().As<ICamera>();
            Ioc.Instance.RegisterType<Art>().As<IArt>();
            Ioc.Instance.RegisterType<Cast>().As<ICast>();
            Ioc.Instance.RegisterType<CostumeMakeUp>().As<ICostumeMakeUp>();
            Ioc.Instance.RegisterType<Directing>().As<IDirecting>();
            Ioc.Instance.RegisterType<People>().As<IPeople>();
            Ioc.Instance.RegisterType<Team>().As<ITeam>();
            Ioc.Instance.RegisterType<Production>().As<IProduction>();
            Ioc.Instance.RegisterType<Sound>().As<ISound>();
            Ioc.Instance.RegisterType<Writing>().As<IWriting>();
            Ioc.Instance.RegisterType<MiniShow>().As<IMiniShow>();
            Ioc.Instance.RegisterType<MiniMovie>().As<IMiniMovie>();
            Ioc.Instance.RegisterType<Screenshot>().As<IScreenshot>();
            Ioc.Instance.RegisterType<User>().As<IUser>();
            Ioc.Instance.RegisterType<Crew>().As<ICrew>();

            Ioc.Instance.RegisterType<UserToken>().As<IUserToken>();
            Ioc.Instance.RegisterType<UserSettings>().As<IUserSettings>();
            Ioc.Instance.RegisterType<Account>().As<IAccount>();

            Ioc.Instance.RegisterType<SetAsSeenResult>().As<ISetAsSeenResult>();
        }    
    }
}
