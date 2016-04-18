using Shiftv.Contracts.Domain.Users;

namespace Shiftv.Core.Models.Users
{
    public class Account : IAccount
    {
        public string Timezone { get; set; }
        public string CoverImage { get; set; }
    }
}