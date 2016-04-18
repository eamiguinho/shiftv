namespace Shiftv.Contracts.Domain.Users
{
    public interface IShiftvUserStats
    {
        bool IsGold { get; set; }
        bool IsSilver { get; set; }
        string Username { get; set; }
    }
}