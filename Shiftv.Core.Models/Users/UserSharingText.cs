using Newtonsoft.Json;
using Shiftv.Contracts.Domain.Users;

namespace Shiftv.Core.Models.Users
{
    public class UserSharingText : IUserSharingText
    {
        public string Watching { get; set; }
        public string Watched { get; set; }
    }
}