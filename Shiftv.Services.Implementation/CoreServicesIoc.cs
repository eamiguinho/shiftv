using Autofac;
using Shiftv.Contracts.Services.Accounts;
using Shiftv.Contracts.Services.Activities;
using Shiftv.Contracts.Services.Calendars;
using Shiftv.Contracts.Services.Categories;
using Shiftv.Contracts.Services.Comments;
using Shiftv.Contracts.Services.Crawler;
using Shiftv.Contracts.Services.Episodes;
using Shiftv.Contracts.Services.Movies;
using Shiftv.Contracts.Services.Networks;
using Shiftv.Contracts.Services.Seasons;
using Shiftv.Contracts.Services.Shows;
using Shiftv.Contracts.Services.Stats;
using Shiftv.Contracts.Services.Sync;
using Shiftv.DesignServices.Implementation;
using Shiftv.Global;
using Shiftv.Services.Implementation.Activities;
using Shiftv.Services.Implementation.Calendars;
using Shiftv.Services.Implementation.Categories;
using Shiftv.Services.Implementation.Comments;
using Shiftv.Services.Implementation.Crawler;
using Shiftv.Services.Implementation.Episodes;
using Shiftv.Services.Implementation.Movies;
using Shiftv.Services.Implementation.Networks;
using Shiftv.Services.Implementation.Seasons;
using Shiftv.Services.Implementation.Shows;
using Shiftv.Services.Implementation.Stats;
using Shiftv.Services.Implementation.Sync;
using Shiftv.Services.Implementation.Users;

namespace Shiftv.Services.Implementation
{
    public static class CoreServicesIoc
    {
        public static void Register()
        {
            Ioc.Instance.RegisterType<ShowService>().As<IShowService>().SingleInstance();
            Ioc.Instance.RegisterType<EpisodeService>().As<IEpisodeService>().SingleInstance();
            Ioc.Instance.RegisterType<UserService>().As<IUserService>().SingleInstance();
            Ioc.Instance.RegisterType<MovieService>().As<IMovieService>().SingleInstance();
            Ioc.Instance.RegisterType<CalendarService>().As<ICalendarService>().SingleInstance();
            Ioc.Instance.RegisterType<SeasonService>().As<ISeasonService>().SingleInstance();
            Ioc.Instance.RegisterType<CommentService>().As<ICommentService>().SingleInstance();
            Ioc.Instance.RegisterType<StatisticsService>().As<IStatisticsService>().SingleInstance();
            Ioc.Instance.RegisterType<CategoryService>().As<ICategoryService>().SingleInstance();
            Ioc.Instance.RegisterType<ActivityService>().As<IActivityService>().SingleInstance();
            Ioc.Instance.RegisterType<NetworkService>().As<INetworkService>().SingleInstance();
            Ioc.Instance.RegisterType<CrawlerService>().As<ICrawlerService>().SingleInstance();
            Ioc.Instance.RegisterType<AnimeRealmCrawler>().As<IAnimeRealmCrawler>().SingleInstance();
            Ioc.Instance.RegisterType<CrawlerHelper>().As<ICrawlerHelper>().SingleInstance();
            Ioc.Instance.RegisterType<OnlineMoviesProCrawler>().As<IOnlineMoviesProCrawler>().SingleInstance();
            Ioc.Instance.RegisterType<MoviesHdCoCrawler>().As<IMoviesHdCoCrawler>().SingleInstance();
            Ioc.Instance.RegisterType<AnimeTwistCrawler>().As<IAnimeTwistCrawler>().SingleInstance();
            Ioc.Instance.RegisterType<WatchMoviesCrawler>().As<IWatchMoviesCrawler>().SingleInstance();
            Ioc.Instance.RegisterType<OnlineHdMoviesCrawler>().As<IOnlineHdMoviesCrawler>().SingleInstance();
            Ioc.Instance.RegisterType<MoviesMazeCrawler>().As<IMoviesMazeCrawler>().SingleInstance();
            Ioc.Instance.RegisterType<WatchSeriesCrawler>().As<IWatchSeriesCrawler>().SingleInstance();
            Ioc.Instance.RegisterType<SyncService>().As<ISyncService>().SingleInstance();
            Ioc.Instance.RegisterType<CouchtunerSeriesCrawler>().As<ICouchtunerSeriesCrawler>().SingleInstance();
        }

        public static void RegisterDesignMode()
        {
            Ioc.Instance.RegisterType<ShowDesignService>().As<IShowService>().SingleInstance();
            Ioc.Instance.RegisterType<EpisodeDesignService>().As<IEpisodeService>().SingleInstance();
            Ioc.Instance.RegisterType<UserDesignService>().As<IUserService>().SingleInstance();
            Ioc.Instance.RegisterType<MovieDesignService>().As<IMovieService>().SingleInstance();
            Ioc.Instance.RegisterType<CalendarDesignService>().As<ICalendarService>().SingleInstance();
            //Ioc.Instance.RegisterType<SeasonService>().As<ISeasonService>().SingleInstance();
            Ioc.Instance.RegisterType<CommentDesignService>().As<ICommentService>().SingleInstance();
            Ioc.Instance.RegisterType<StatisticsDesignService>().As<IStatisticsService>().SingleInstance();
            //Ioc.Instance.RegisterType<CategoryService>().As<ICategoryService>().SingleInstance();
            Ioc.Instance.RegisterType<ActivityDesignService>().As<IActivityService>().SingleInstance();
            Ioc.Instance.RegisterType<NetworkDesignService>().As<INetworkService>().SingleInstance();
            //Ioc.Instance.RegisterType<CrawlerService>().As<ICrawlerService>().SingleInstance();
            //Ioc.Instance.RegisterType<AnimeRealmCrawler>().As<IAnimeRealmCrawler>().SingleInstance();
            //Ioc.Instance.RegisterType<CrawlerHelper>().As<ICrawlerHelper>().SingleInstance();
            //Ioc.Instance.RegisterType<StreamTvCrawler>().As<IStreamTvCrawler>().SingleInstance();

        }
    }


}
