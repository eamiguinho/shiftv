using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Core.Models.Shows
{
    class SubtitlesLanguage : ISubtitlesLanguage
    {
        public string Language { get; set; }
        public string LanguageId { get; set; }
    }
}