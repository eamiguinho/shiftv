namespace Shiftv.Contracts.Domain.Users
{
    public interface IAccountDetails
    {
        string Timezone { get; set; }
        bool Use24Hr { get; set; }
        bool IsProtected { get; set; }
    }
}