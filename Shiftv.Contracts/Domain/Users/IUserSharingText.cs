namespace Shiftv.Contracts.Domain.Users
{
    public interface IUserSharingText
    {
        string Watching { get; set; }
        string Watched { get; set; }
    }
}