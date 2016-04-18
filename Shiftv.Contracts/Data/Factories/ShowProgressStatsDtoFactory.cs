using System.Linq;
using Autofac;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class ShowProgressStatsDtoFactory
    {
        public static IShowProgressStats Create(ProgressStatsDto progress)
        {
            var prog = Ioc.Container.Resolve<IShowProgressStats>();
            prog.Aired = progress.Aired;
            prog.Completed = progress.Completed;
            prog.Left = progress.Left;
            prog.Percentage = progress.Percentage;
            return prog;
        }

        //public static IShowProgressStats CreateEmptyForShow(IShow show)
        //{
        //    var prog = Ioc.Container.Resolve<IShowProgressStats>();
        //    prog.Aired = show.Seasons.Where(x=>x.Number!=0).Sum(season => season.Episodes.Count);
        //    prog.Completed = 0;
        //    prog.Left = show.Seasons.Where(x => x.Number != 0).Sum(season => season.Episodes.Count);
        //    prog.Percentage = 0;
        //    return prog;
        //}

    
    }
}