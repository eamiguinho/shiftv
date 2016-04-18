using System.Linq;
using Autofac;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class MiniShowDtoFactory
    {
        public static IMiniShow CreateShow(MiniShowDto dto)
        {
            if (dto == null) return null;
            var show = Ioc.Container.Resolve<IMiniShow>();
            show.Network = dto.Network;
            show.Rating = dto.Rating;
            show.Title = dto.Title;
            show.Ids = IdsDtoFactory.Create(dto.Ids);
            show.Fanart = FanartDtoFactory.Create(dto.Fanart);
            show.Votes = dto.Votes;
            show.FirstAired = dto.FirstAired;
            show.UserRating = dto.UserRating;
            show.Year = dto.Year;
            show.Genres = dto.Genres;
            show.Status = dto.Status;
            return show;
        }

        public static MiniShowDto GetDto(IMiniShow miniShow)
        {
            if (miniShow == null) return null;
            var dto = new MiniShowDto
            {
                Title = miniShow.Title,
                Fanart = FanartDtoFactory.GetDto(miniShow.Fanart),
                FirstAired = miniShow.FirstAired,
                Genres = miniShow.Genres,
                Ids = IdsDtoFactory.GetDto(miniShow.Ids),
                Network = miniShow.Network,
                Rating = miniShow.Rating,
                Status = miniShow.Status,
                UserRating = miniShow.UserRating,
                Votes = miniShow.Votes,
                Year = miniShow.Year
            };
            return dto;
        }
    }

    public static class ShowDtoFactory
    {
        public static IShow CreateShow(ShowDto dto)
        {
            if (dto == null) return null;
            var show = Ioc.Container.Resolve<IShow>();
            show.Certification = dto.Certification;
            show.Country = dto.Country;
            show.FirstAired = dto.FirstAired;
            show.Genres = dto.Genres;
            show.Network = dto.Network;
            show.Overview = dto.Overview;
            show.Rating = dto.Rating;
            show.Runtime = dto.Runtime;
            show.Status = dto.Status;
            show.Title = dto.Title;
            show.Year = dto.Year;
            show.Ids = IdsDtoFactory.Create(dto.Ids);
            show.Airs = AirsDtoFactory.Create(dto.Airs);
            show.Images = ImageDtoFactory.Create(dto.Images);
            show.Votes = dto.Votes;
            show.UserRating = dto.UserRating;
            show.Seasons = dto.Seasons != null ? dto.Seasons.Select(x => SeasonDtoFactory.Create(x, show.Ids.ImdbId, show.Title)).ToList() : null;
            return show;
        }

        public static IShow CreateShowClean(ShowDto dto)
        {
            var show = CreateShow(dto);
           
            return show;
        }

        public static ShowDto GetDto(IShow show)
        {
            if (show == null) return null;
            var dto = new ShowDto();
            dto.Certification = show.Certification;
            dto.Country = show.Country;
            dto.FirstAired = show.FirstAired;
            dto.Genres = show.Genres;
            dto.Network = show.Network;
            dto.Overview = show.Overview;
            dto.Rating = show.Rating;
            dto.Runtime = show.Runtime;
            dto.Status = show.Status;
            dto.Title = show.Title;
            dto.Year = show.Year;
            dto.Ids = IdsDtoFactory.GetDto(show.Ids);
            dto.Airs = AirsDtoFactory.GetDto(show.Airs);
            dto.Images = ImageDtoFactory.GetDto(show.Images);
            dto.Votes = dto.Votes;
            dto.UserRating = dto.UserRating;
            //dto.Seasons = dto.Seasons.Select(x => SeasonDtoFactory.Create(x, show.Ids.ImdbId, show.Title)).ToList();
            return dto;
        }

        public static MiniShowDto GetMiniDto(IShow show)
        {
            if (show == null) return null;
            var dto = new MiniShowDto
            {
                Title = show.Title,
                Fanart = FanartDtoFactory.GetDto(show.Images.Fanart),
                FirstAired = show.FirstAired,
                Genres = show.Genres,
                Ids = IdsDtoFactory.GetDto(show.Ids),
                Network = show.Network,
                Rating = show.Rating,
                Status = show.Status,
                UserRating = show.UserRating,
                Votes = show.Votes,
                Year = show.Year
            };
            return dto;
        }
    }
}