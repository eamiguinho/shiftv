namespace Shiftv.Contracts.Domain.Activity
{
    public interface IActivityTimestamps
    {
        int Start { get; set; }
        int End { get; set; }
        int Current { get; set; }
    }
}