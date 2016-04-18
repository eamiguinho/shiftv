using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ShiftvAPI.Contracts.Helpers;
using ShiftvAPI.Contracts.Services;
using ShiftvAPI.Contracts.Services.Calendar;
using ShiftvAPI.Contracts.Services.Lists;
using ShiftvAPI.Contracts.Services.Login;
using ShiftvAPI.Contracts.Services.Movies;
using ShiftvAPI.Contracts.Services.Shows;
using ShiftvAPI.Contracts.Services.Sync;
using ShiftvAPI.Services.Implementation.Calendar;
using ShiftvAPI.Services.Implementation.Lists;
using ShiftvAPI.Services.Implementation.Login;
using ShiftvAPI.Services.Implementation.Movies;
using ShiftvAPI.Services.Implementation.Shows;
using ShiftvAPI.Services.Implementation.Sync;

namespace ShiftvAPI.Services.Implementation
{
    public static class ServicesIoc
    {
        public static void Register()
        {
            Ioc.Instance.RegisterType<ShowService>().As<IShowService>().SingleInstance();
            Ioc.Instance.RegisterType<MovieService>().As<IMovieService>().SingleInstance();
            Ioc.Instance.RegisterType<ListService>().As<IListService>().SingleInstance();
            Ioc.Instance.RegisterType<LoginService>().As<ILoginService>().SingleInstance();
            Ioc.Instance.RegisterType<SyncService>().As<ISyncService>().SingleInstance();
            Ioc.Instance.RegisterType<CalendarService>().As<ICalendarService>().SingleInstance();
          
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
