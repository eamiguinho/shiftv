namespace Shiftv.Contracts.Domain.Users
{
    public interface IUserSettings
    {
        IUser User { get; set; }

        IAccount Account { get; set; }

    }
}