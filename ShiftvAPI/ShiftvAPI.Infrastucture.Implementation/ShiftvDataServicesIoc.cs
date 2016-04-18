using Autofac;
using ShiftvAPI.Contracts.Helpers;
using ShiftvAPI.Contracts.Infrastucture.Shiftv;
using ShiftvAPI.Infrastucture.Implementation.Calendar;
using ShiftvAPI.Infrastucture.Implementation.Lists;
using ShiftvAPI.Infrastucture.Implementation.Login;
using ShiftvAPI.Infrastucture.Implementation.Movies;
using ShiftvAPI.Infrastucture.Implementation.Shows;
using ShiftvAPI.Infrastucture.Implementation.Sync;

namespace ShiftvAPI.Infrastucture.Implementation
{
    public static class ShiftvDataServicesIoc
    {
        public static void Register()
        {
            Ioc.Instance.RegisterType<ShowShiftvDataService>().As<IShowShiftvDataService>().SingleInstance();
            Ioc.Instance.RegisterType<MovieShiftvDataService>().As<IMovieShiftvDataService>().SingleInstance();
            Ioc.Instance.RegisterType<ListShiftvDataService>().As<IListShiftvDataService>().SingleInstance();
            Ioc.Instance.RegisterType<LoginShiftvDataService>().As<ILoginShiftvDataService>().SingleInstance();
            Ioc.Instance.RegisterType<SyncShiftvDataService>().As<ISyncShiftvDataService>().SingleInstance();
            Ioc.Instance.RegisterType<CalendarShiftvDataService>().As<ICalendarShiftvDataService>().SingleInstance();
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
