using Autofac;
using ShiftvAPI.Contracts.Helpers;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Lists;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Login;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Movies;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Shows;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Sync;
using ShiftvAPI.Infrastucture.Trakt.Implementation.Lists;
using ShiftvAPI.Infrastucture.Trakt.Implementation.Login;
using ShiftvAPI.Infrastucture.Trakt.Implementation.Movies;
using ShiftvAPI.Infrastucture.Trakt.Implementation.Shows;
using ShiftvAPI.Infrastucture.Trakt.Implementation.Sync;

namespace ShiftvAPI.Infrastucture.Trakt.Implementation
{
    public static class TraktDataServicesIoc
    {
        public static void Register()
        {
            Ioc.Instance.RegisterType<ShowTraktDataService>().As<IShowTraktDataService>().SingleInstance();
            Ioc.Instance.RegisterType<ShowTraktQueryService>().As<IShowTraktQueryService>().SingleInstance();
            Ioc.Instance.RegisterType<MovieTraktDataService>().As<IMovieTraktDataService>().SingleInstance();
            Ioc.Instance.RegisterType<MovieTraktQueryService>().As<IMovieTraktQueryService>().SingleInstance(); Ioc.Instance.RegisterType<ListTraktDataService>().As<IListTraktDataService>().SingleInstance();
            Ioc.Instance.RegisterType<ListTraktQueryService>().As<IListTraktQueryService>().SingleInstance();
            Ioc.Instance.RegisterType<LoginTraktDataService>().As<ILoginTraktDataService>().SingleInstance();
            Ioc.Instance.RegisterType<LoginTraktQueryService>().As<ILoginTraktQueryService>().SingleInstance();
            Ioc.Instance.RegisterType<SyncTraktDataService>().As<ISyncTraktDataService>().SingleInstance();
            Ioc.Instance.RegisterType<SyncTraktQueryService>().As<ISyncTraktQueryService>().SingleInstance();
        }

        public static void RegisterDesignMode()
        {
            //Ioc.Instance.RegisterType<ShowDesignService>().As<IShowService>().SingleInstance();
            //Ioc.Instance.RegisterType<EpisodeDesignService>().As<IEpisodeService>().SingleInstance();
            //Ioc.Instance.RegisterType<UserDesignService>().As<IUserService>().SingleInstance();
            //Ioc.Instance.RegisterType<MovieDesignService>().As<IMovieService>().SingleInstance();
            //Ioc.Instance.RegisterType<CalendarDesignService>().As<ICalendarService>().SingleInstance();
            ////Ioc.Instance.RegisterType<SeasonService>().As<ISeasonService>().SingleInstance();
            //Ioc.Instance.RegisterType<CommentDesignService>().As<ICommentService>().SingleInstance();
            //Ioc.Instance.RegisterType<StatisticsDesignService>().As<IStatisticsService>().SingleInstance();
            ////Ioc.Instance.RegisterType<CategoryService>().As<ICategoryService>().SingleInstance();
            //Ioc.Instance.RegisterType<ActivityDesignService>().As<IActivityService>().SingleInstance();
            //Ioc.Instance.RegisterType<NetworkDesignService>().As<INetworkService>().SingleInstance();
            ////Ioc.Instance.RegisterType<CrawlerService>().As<ICrawlerService>().SingleInstance();
            ////Ioc.Instance.RegisterType<AnimeRealmCrawler>().As<IAnimeRealmCrawler>().SingleInstance();
            ////Ioc.Instance.RegisterType<CrawlerHelper>().As<ICrawlerHelper>().SingleInstance();
            ////Ioc.Instance.RegisterType<StreamTvCrawler>().As<IStreamTvCrawler>().SingleInstance();

        }
    }

}
