using Newtonsoft.Json;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Global;

namespace Shiftv.Core.Models.Users
{
    public class UserAccount : IUserAccount
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public IUserProfile UserProfile { get; set; }
        public IAccountDetails AccountDetails { get; set; }
        public IUserSharingText UserSharingText { get; set; }
        public string Username { get; set; }
        public string PasswordEnc { get; set; }
    }
}
