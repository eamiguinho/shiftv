namespace Shiftv.Contracts.Domain.Users
{
    public interface IUserAccount
    {
        string Status { get; set; }
        string Message { get; set; }
        IUserProfile UserProfile { get; set; }
        IAccountDetails AccountDetails { get; set; }
        IUserSharingText UserSharingText { get; set; }
        string Username { get; set; }
        string PasswordEnc { get; set; }
    }
}
