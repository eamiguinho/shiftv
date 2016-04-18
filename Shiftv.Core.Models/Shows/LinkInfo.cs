using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Core.Models.Shows
{
    class LinkInfo : ILinkInfo
    {
        public string StreamLink { get; set; }
        public string OriginalLink { get; set; }
        public StreamQuality Quality { get; set; }
        public StreamVelocity Velocity { get; set; }
        public string FileSizeFormatted { get; set; }
        public string EmbbedLink { get; set; }
        public string ReportLink { get; set; }
        public bool IsCached { get; set; }
        public double FileSize { get; set; }
    }
}