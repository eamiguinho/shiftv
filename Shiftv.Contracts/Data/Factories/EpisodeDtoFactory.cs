using Autofac;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class EpisodeDtoFactory
    {
        public static IEpisode Create(EpisodeDto dto, string showName)
        {
            if (dto == null) return null;
            var episode = Ioc.Container.Resolve<IEpisode>();
            episode.FirstAired = dto.FirstAired;
            episode.Number = dto.Number;
            episode.Overview = dto.Overview;
            episode.Season = dto.Season;
            episode.Title = dto.Title;
            episode.Images = ImageDtoFactory.Create(dto.Images);
            episode.Rating = dto.Rating;
            episode.Ids = IdsDtoFactory.Create(dto.Ids);
            episode.NumberAbs = dto.NumberAbs;
            episode.UpdatedAt = dto.UpdatedAt;
            episode.Votes = dto.Votes;
            episode.ShowName = showName;
            episode.Watched = dto.Watched;
            episode.RatedValue = dto.UserRating;
            return episode;
        }

        public static EpisodeDto GetDto(IEpisode episode)
        {
            var dto = new EpisodeDto();
            dto.FirstAired = episode.FirstAired;
            dto.Number = episode.Number;
            dto.Overview = episode.Overview;
            dto.Season = episode.Season;
            dto.Title = episode.Title;
            dto.Images = ImageDtoFactory.GetDto(episode.Images);
            dto.Rating = episode.Rating;
            dto.Ids = IdsDtoFactory.GetDto(episode.Ids);
            dto.NumberAbs = episode.NumberAbs;
            dto.UpdatedAt = episode.UpdatedAt;
            dto.Votes = episode.Votes;
            dto.Watched = episode.Watched;
            dto.UserRating = episode.RatedValue;

            return dto;
        }

        public static IEpisode CreateClean(EpisodeDto dto, string title)
        {
            var res = Create(dto, title);
            //if (res != null)
            //{
            //    res.IsLoveOrHate = false;
            //    res.InCollection = false;
            //    res.InWatchlist = false;
            //    res.Watched = false;
            //    res.UserRating = false;
            //    res.InWatchlist = false;
            //}
            return res;
        }
    }
}