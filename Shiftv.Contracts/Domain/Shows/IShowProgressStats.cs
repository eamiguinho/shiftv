namespace Shiftv.Contracts.Domain.Shows
{
    public interface IShowProgressStats
    {
        int Percentage { get; set; }
        int Aired { get; set; }
        int Completed { get; set; }
        int Left { get; set; }
    }
}