using System;
using System.Linq;
using Autofac;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class ShowProgressDtoFactory
    {
        public static IShowProgress Create(ShowProgressDto dto)
        {
            var prog = Ioc.Container.Resolve<IShowProgress>();
            prog.EpisodesLeft = dto.EpisodesLeft != null ? dto.EpisodesLeft.Select(x=>EpisodeDtoFactory.Create(x, dto.Show.Title)).ToList() : null;
            prog.Show = dto.Show != null ? MiniShowDtoFactory.CreateShow(dto.Show) : null;
            prog.TotalEpisodes = dto.TotalEpisodes;
            return prog;
        }

        //public static IShowProgress CreateEmptyForShow(IShow show)
        //{
        //    var prog = Ioc.Container.Resolve<IShowProgress>();
        //    prog.Progress = ShowProgressStatsDtoFactory.CreateEmptyForShow(show);
        //    prog.Show = show;
        //    prog.Seasons = SeasonProgressDtoFactory.CreateEmptyForShow(show);
        //    return prog;
        //}
    }
}