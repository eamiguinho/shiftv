using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Core.Models.Shows
{
    class SubtitlesInfo : ISubtitlesInfo
    {
        public string SubtitlesLink { get; set; }
        public string Language { get; set; }
        public string LanguageId { get; set; }
        public string SubtitleFileName { get; set; }
    }
}