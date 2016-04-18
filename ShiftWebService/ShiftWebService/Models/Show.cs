using System.Collections.Generic;

namespace ShiftWebService.Models
{
    public class Show
    {
        public string ImdbId { get; set; }
        public List<LinkInfo> Links { get; set; }
        public List<SubtitlesInfo> Subtitles { get; set; } 
    }

    public class LinkInfo
    {
        public string StreamLink { get; set; }
        public string OriginalLink { get; set; }
        public StreamQuality Quality { get; set; }
    }

    public enum StreamQuality
    {
        HD,
        MD,
        SD
    }
        
    public class SubtitlesInfo
    {
        public string SubtitlesLink { get; set; }
        public string Language { get; set; }
        public string SubtitleFileName { get; set; }
    }
}