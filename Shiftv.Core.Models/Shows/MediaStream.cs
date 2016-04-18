using System.Collections.Generic;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Core.Models.Shows
{
    class MediaStream : IMediaStream
    {
        public string ImdbId { get; set; }
        public List<ILinkInfo> Links { get; set; }
        public List<ISubtitlesInfo> Subtitles { get; set; }
    }
}
