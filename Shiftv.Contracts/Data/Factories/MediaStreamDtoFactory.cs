using System.Linq;
using Autofac;
using Shiftv.Contracts.Data.Media;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class MediaStreamDtoFactory
    {
        public static IMediaStream Create(MediaStreamDto mediaStreamDto)
        {
            var x = Ioc.Container.Resolve<IMediaStream>();
            if (mediaStreamDto == null) return x;
            x.ImdbId = mediaStreamDto.ImdbId;
            if (mediaStreamDto.Links != null) x.Links = mediaStreamDto.Links.Select(LinkInfoDtoFactory.Create).ToList();
            if (mediaStreamDto.Subtitles != null) x.Subtitles = mediaStreamDto.Subtitles.Select(SubtitlesInfoDtoFactory.Create).ToList();
            return x;
        }
    }
}