namespace Shiftv.Contracts.Domain.Users
{
    public interface IAccount
    {
        string Timezone { get; set; }

        string CoverImage { get; set; }
    }
}