using Autofac;
using Shiftv.Contracts.Data.Media;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class SubtitlesInfoDtoFactory
    {
        public static ISubtitlesInfo Create(SubtitlesInfoDto subtitlesInfoDto)
        {
            var x = Ioc.Container.Resolve<ISubtitlesInfo>();
            x.Language = subtitlesInfoDto.Language;
            x.SubtitleFileName = subtitlesInfoDto.SubtitleFileName;
            x.SubtitlesLink = subtitlesInfoDto.SubtitlesLink;
            x.LanguageId = subtitlesInfoDto.LanguageId;
            return x;
        }
    }
}