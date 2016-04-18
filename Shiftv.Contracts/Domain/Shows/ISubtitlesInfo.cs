namespace Shiftv.Contracts.Domain.Shows
{
    public interface ISubtitlesInfo
    {
        string SubtitlesLink { get; set; }
        string Language { get; set; }
        string LanguageId { get; set; }
        string SubtitleFileName { get; set; }
    }
}