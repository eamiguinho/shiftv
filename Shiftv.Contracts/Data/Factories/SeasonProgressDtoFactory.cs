using System.Collections.Generic;
using System.Linq;
using Autofac;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class SeasonProgressDtoFactory
    {
        public static ISeasonProgress Create(SeasonProgressDto dto)
        {
            var prog = Ioc.Container.Resolve<ISeasonProgress>();
            prog.Aired = dto.Aired;
            prog.Completed = dto.Completed;
            prog.Left = dto.Left;
            prog.Season = dto.Season;
            prog.Percentage = dto.Percentage;
            prog.EpisodesSeen = dto.Episodes != null ? EpisodesProgressDtoFactory.Create(dto.Episodes, true) : null;
            prog.EpisodesToSee = dto.Episodes != null ? EpisodesProgressDtoFactory.Create(dto.Episodes, false) : null;
            return prog;
        }

        //public static List<ISeasonProgress> CreateEmptyForShow(IShow show)
        //{
        //    var list = new List<ISeasonProgress>();
        //    foreach (var season in show.Seasons.Where(x=>x.Number != 0))
        //    {
        //        var prog = Ioc.Container.Resolve<ISeasonProgress>();
        //        prog.Aired = season.Episodes.Count;
        //        prog.Completed = 0;
        //        prog.Left = season.Episodes.Count;
        //        prog.Percentage = 0;
        //        prog.Season = season.Number;
        //        prog.EpisodesToSee = season.Episodes.Select(episode => episode.Number).ToList();
        //        list.Add(prog);
        //    }
        //    return list;
        //}
    }
}