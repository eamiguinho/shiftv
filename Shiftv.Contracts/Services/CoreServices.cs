using Autofac;
using Shiftv.Contracts.Services.Accounts;
using Shiftv.Contracts.Services.Activities;
using Shiftv.Contracts.Services.Calendars;
using Shiftv.Contracts.Services.Comments;
using Shiftv.Contracts.Services.Crawler;
using Shiftv.Contracts.Services.Episodes;
using Shiftv.Contracts.Services.Movies;
using Shiftv.Contracts.Services.Networks;
using Shiftv.Contracts.Services.Seasons;
using Shiftv.Contracts.Services.Shows;
using Shiftv.Contracts.Services.Stats;
using Shiftv.Global;

namespace Shiftv.Contracts.Services
{
    public static class CoreServices
    {
        private static IShowService _show;
        private static IEpisodeService _episode;
        private static IUserService _user;
        private static IMovieService _movie;
        private static ICalendarService _calendar;
        private static IStatisticsService _stats;
        private static ICommentService _comments;
        private static IActivityService _activity;
        private static INetworkService _network;
        private static ISeasonService _season;
        private static ICrawlerService _crawler;

        public static IShowService Show
        {
            get { return _show ?? (_show = Ioc.Container.Resolve<IShowService>()); }
        }

        public static IEpisodeService Episode { get { return _episode ?? (_episode = Ioc.Container.Resolve<IEpisodeService>()); } }
        public static IMovieService Movie { get { return _movie ?? (_movie = Ioc.Container.Resolve<IMovieService>()); } }
        public static ICalendarService Calendar
        {
            get
            {
                return _calendar ?? (_calendar = Ioc.Container.Resolve<ICalendarService>());
            }
        }

        public static IUserService User
        {
            get
            {
                return _user ?? (_user = Ioc.Container.Resolve<IUserService>());
            }
        }

        public static IStatisticsService Stats
        {
            get
            {
                return _stats ?? (_stats = Ioc.Container.Resolve<IStatisticsService>());
            }
        }

        public static ICommentService Comments
        {
            get
            {
                return _comments ?? (_comments = Ioc.Container.Resolve<ICommentService>());
            }
        }   
        
        public static IActivityService Activity
        {
            get
            {
                return _activity ?? (_activity = Ioc.Container.Resolve<IActivityService>());
            }
        }

        public static INetworkService Network
        {
            get
            {
                return _network ?? (_network = Ioc.Container.Resolve<INetworkService>());
            }
        }
        public static ISeasonService Season
        {
            get
            {
                return _season ?? (_season = Ioc.Container.Resolve<ISeasonService>());
            }
        }    
        
        public static ICrawlerService Crawler
        {
            get
            {
                return _crawler ?? (_crawler = Ioc.Container.Resolve<ICrawlerService>());
            }
        }


    }
}
