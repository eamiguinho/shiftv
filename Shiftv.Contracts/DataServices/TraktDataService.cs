using Autofac;
using Shiftv.Contracts.DataServices.Calendars;
using Shiftv.Contracts.DataServices.Comments;
using Shiftv.Contracts.DataServices.Login;
using Shiftv.Contracts.DataServices.Movies;
using Shiftv.Contracts.DataServices.Shows;
using Shiftv.Global;

namespace Shiftv.Contracts.DataServices
{
    public class TraktDataService
    {
        private static IShowTraktDataService _show;
        private static ILoginTraktDataService _login;
        private static IMovieTraktDataService _movie;
        private static ICalendarTraktDataService _calendar;
        private static ICommentTraktDataService _comment;
        public static IShowTraktDataService Show { get { return _show ?? (_show = Ioc.Container.Resolve<IShowTraktDataService>()); } }
        public static ILoginTraktDataService Login { get { return _login ?? (_login = Ioc.Container.Resolve<ILoginTraktDataService>()); } }
        public static IMovieTraktDataService Movie { get { return _movie ?? (_movie = Ioc.Container.Resolve<IMovieTraktDataService>()); } }
        public static ICalendarTraktDataService Calendar { get { return _calendar ?? (_calendar = Ioc.Container.Resolve<ICalendarTraktDataService>()); } }
        public static ICommentTraktDataService Comment { get { return _comment ?? (_comment = Ioc.Container.Resolve<ICommentTraktDataService>()); } }
    }
}
