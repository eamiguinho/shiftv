using Shiftv.Contracts.Domain.Users;

namespace Shiftv.Core.Models.Users
{
    public class UserSettings : IUserSettings
    {
        public IUser User { get; set; }
        public IAccount Account { get; set; }
    }
}