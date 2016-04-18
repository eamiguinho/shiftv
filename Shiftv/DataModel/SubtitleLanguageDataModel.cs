using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.DataModel
{
    public class SubtitleLanguageDataModel
    {
        private ISubtitlesLanguage _language;

        public SubtitleLanguageDataModel(ISubtitlesLanguage language)
        {
            _language = language;
        }
        public string Language { get { return _language.Language; }}
        public string LanguageId { get { return _language.LanguageId; } }
    }
}