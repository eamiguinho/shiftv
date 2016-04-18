using Autofac;
using Shiftv.Contracts.Data.Media;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class LinkInfoDtoFactory
    {
        public static ILinkInfo Create(LinkInfoDto linkInfoDto)
        {
            var x = Ioc.Container.Resolve<ILinkInfo>();
            x.OriginalLink = linkInfoDto.OriginalLink;
            x.Quality = linkInfoDto.Quality;
            x.StreamLink = linkInfoDto.StreamLink;
            x.EmbbedLink = linkInfoDto.EmbbedLink;
            x.FileSizeFormatted = linkInfoDto.FileSize;
            x.Velocity = linkInfoDto.Velocity;
            x.IsCached = linkInfoDto.IsCached;
            return x;
        }

        public static LinkInfoDto GetDto(ILinkInfo linkInfo)
        {
            var x = new LinkInfoDto();
            x.OriginalLink = linkInfo.OriginalLink;
            x.Quality = linkInfo.Quality;
            x.StreamLink = linkInfo.StreamLink;
            x.EmbbedLink = linkInfo.EmbbedLink;
            x.Velocity = linkInfo.Velocity;
            x.FileSize = linkInfo.FileSizeFormatted;
            x.IsCached = linkInfo.IsCached;
            return x;
        }
    }
}