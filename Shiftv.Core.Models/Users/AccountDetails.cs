using Newtonsoft.Json;
using Shiftv.Contracts.Domain.Users;

namespace Shiftv.Core.Models.Users
{
    public class AccountDetails : IAccountDetails
    {
        public string Timezone { get; set; }
        public bool Use24Hr { get; set; }
        public bool IsProtected { get; set; }
    }
}