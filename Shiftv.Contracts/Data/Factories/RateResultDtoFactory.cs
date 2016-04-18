using Autofac;
using Shiftv.Contracts.Data.Results;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class RateResultDtoFactory
    {
        public static IRateResult Create(RateResultDto rateResultDto)
        {
            if (rateResultDto == null) return null;
            var x = Ioc.Container.Resolve<IRateResult>();
            x.Status = rateResultDto.Success;
            if (rateResultDto.Request.Episode != null)
            {
                x.Episode = Ioc.Container.Resolve<IEpisode>();
                x.Episode.Ids = IdsDtoFactory.Create(rateResultDto.Request.Episode.Ids);
                x.Episode.Number = rateResultDto.Request.Episode.Number;
                x.Episode.Season = rateResultDto.Request.Episode.Season;
            }
            if (rateResultDto.Request.Show != null)
            {
                x.Show = Ioc.Container.Resolve<IShow>();
                x.Show.Ids = IdsDtoFactory.Create(rateResultDto.Request.Show.Ids);
                x.Show.Title = rateResultDto.Request.Show.Title;
                x.Show.Year = rateResultDto.Request.Show.Year;
            }
            if (rateResultDto.Request.Movie != null)
            {
                x.Movie = Ioc.Container.Resolve<IMovie>();
                x.Movie.Ids = IdsDtoFactory.Create(rateResultDto.Request.Movie.Ids);
                x.Movie.Title = rateResultDto.Request.Movie.Title;
                x.Movie.Year = rateResultDto.Request.Movie.Year;
            }
            return x;
        }


   
    }
}