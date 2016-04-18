namespace Shiftv.Contracts.Domain.Shows
{
    public interface ILinkInfo
    {
        string StreamLink { get; set; }
        string OriginalLink { get; set; }
        StreamQuality Quality { get; set; }
        StreamVelocity Velocity { get; set; }
        string FileSizeFormatted { get; set; }
        string EmbbedLink { get; set; }
        string ReportLink { get; set; }
        bool IsCached { get; set; }
        double FileSize { get; set; }
    }
}