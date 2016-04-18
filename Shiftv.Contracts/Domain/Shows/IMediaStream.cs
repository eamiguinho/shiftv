using System.Collections.Generic;

namespace Shiftv.Contracts.Domain.Shows
{
    public interface IMediaStream
    {
        string ImdbId { get; set; }
        List<ILinkInfo> Links { get; set; }
        List<ISubtitlesInfo> Subtitles { get; set; } 
    }
}