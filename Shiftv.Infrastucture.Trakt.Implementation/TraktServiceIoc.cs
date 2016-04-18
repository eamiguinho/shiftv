using Autofac;
using Shiftv.Contracts.DataServices.Activities;
using Shiftv.Contracts.DataServices.Calendars;
using Shiftv.Contracts.DataServices.Categories;
using Shiftv.Contracts.DataServices.Comments;
using Shiftv.Contracts.DataServices.Crawler;
using Shiftv.Contracts.DataServices.Episodes;
using Shiftv.Contracts.DataServices.Login;
using Shiftv.Contracts.DataServices.Movies;
using Shiftv.Contracts.DataServices.Networks;
using Shiftv.Contracts.DataServices.Seasons;
using Shiftv.Contracts.DataServices.Shows;
using Shiftv.Contracts.DataServices.Stats;
using Shiftv.Contracts.DataServices.Sync;
using Shiftv.Contracts.DataServices.Users;
using Shiftv.Global;
using Shiftv.Infrastucture.Trakt.Implementation.Activities;
using Shiftv.Infrastucture.Trakt.Implementation.Calendars;
using Shiftv.Infrastucture.Trakt.Implementation.Categories;
using Shiftv.Infrastucture.Trakt.Implementation.Comments;
using Shiftv.Infrastucture.Trakt.Implementation.Crawler;
using Shiftv.Infrastucture.Trakt.Implementation.Episodes;
using Shiftv.Infrastucture.Trakt.Implementation.Login;
using Shiftv.Infrastucture.Trakt.Implementation.Movies;
using Shiftv.Infrastucture.Trakt.Implementation.Networks;
using Shiftv.Infrastucture.Trakt.Implementation.Seasons;
using Shiftv.Infrastucture.Trakt.Implementation.Shows;
using Shiftv.Infrastucture.Trakt.Implementation.Stats;
using Shiftv.Infrastucture.Trakt.Implementation.Sync;
using Shiftv.Infrastucture.Trakt.Implementation.Users;

namespace Shiftv.Infrastucture.Trakt.Implementation
{
    public static class TraktServiceIoc
    {
        public static void Register()
        {
            //Show
            Ioc.Instance.RegisterType<ShowTraktDataService>().As<IShowTraktDataService>().SingleInstance();
            Ioc.Instance.RegisterType<ShowTraktQueryService>().As<IShowTraktQueryService>().SingleInstance();
            //Login
            Ioc.Instance.RegisterType<LoginTraktDataService>().As<ILoginTraktDataService>().SingleInstance();
            Ioc.Instance.RegisterType<LoginTraktQueryService>().As<ILoginTraktQueryService>().SingleInstance();
            //Movie
            Ioc.Instance.RegisterType<MovieTraktDataService>().As<IMovieTraktDataService>().SingleInstance();
            Ioc.Instance.RegisterType<MovieTraktQueryService>().As<IMovieTraktQueryService>().SingleInstance();
            //Calendar
            Ioc.Instance.RegisterType<CalendarTraktDataService>().As<ICalendarTraktDataService>().SingleInstance();
            Ioc.Instance.RegisterType<CalendarTraktQueryService>().As<ICalendarTraktQueryService>().SingleInstance();
            //Comments
            Ioc.Instance.RegisterType<CommentTraktDataService>().As<ICommentTraktDataService>().SingleInstance();
            Ioc.Instance.RegisterType<CommentTraktQueryService>().As<ICommentTraktQueryService>().SingleInstance();
            //Episodes
            Ioc.Instance.RegisterType<EpisodeTraktDataService>().As<IEpisodeTraktDataService>().SingleInstance();
            Ioc.Instance.RegisterType<EpisodeTraktQueryService>().As<IEpisodeTraktQueryService>().SingleInstance();
            //Seasons
            Ioc.Instance.RegisterType<SeasonTraktDataService>().As<ISeasonTraktDataService>().SingleInstance();
            Ioc.Instance.RegisterType<SeasonTraktQueryService>().As<ISeasonTraktQueryService>().SingleInstance();

            //Stats
            Ioc.Instance.RegisterType<StatisticsTraktDataService>().As<IStatisticsTraktDataService>().SingleInstance();
            Ioc.Instance.RegisterType<StatisticsTraktQueryService>().As<IStatisticsTraktQueryService>().SingleInstance();

            //Categories
            Ioc.Instance.RegisterType<CategoryTraktDataService>().As<ICategoryTraktDataService>().SingleInstance();
            Ioc.Instance.RegisterType<CategoryTraktQueryService>().As<ICategoryTraktQueryService>().SingleInstance();    
            
            //Activities
            Ioc.Instance.RegisterType<ActivityTraktDataService>().As<IActivityTraktDataService>().SingleInstance();
            Ioc.Instance.RegisterType<ActivityTraktQueryService>().As<IActivityTraktQueryService>().SingleInstance();  
            
            //Activities
            Ioc.Instance.RegisterType<UserTraktDataService>().As<IUserTraktDataService>().SingleInstance();
            Ioc.Instance.RegisterType<UserTraktQueryService>().As<IUserTraktQueryService>().SingleInstance();  
            
            //Activities
            Ioc.Instance.RegisterType<NetworkTraktDataService>().As<INetworkTraktDataService>().SingleInstance();
            Ioc.Instance.RegisterType<NetworkTraktQueryService>().As<INetworkTraktQueryService>().SingleInstance();

            Ioc.Instance.RegisterType<CrawlerShiftvDataService>().As<ICrawlerShiftvDataService>().SingleInstance();
            Ioc.Instance.RegisterType<CrawlerShiftvQueryService>().As<ICrawlerShiftvQueryService>().SingleInstance();

            Ioc.Instance.RegisterType<SyncTraktDataService>().As<ISyncTraktDataService>().SingleInstance();
            Ioc.Instance.RegisterType<SyncTraktQueryService>().As<ISyncTraktQueryService>().SingleInstance(); 
        }
    }
}
