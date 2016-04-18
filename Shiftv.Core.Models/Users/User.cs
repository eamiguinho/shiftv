using Shiftv.Contracts.Domain.Images;
using Shiftv.Contracts.Domain.Users;

namespace Shiftv.Core.Models.Users
{
    class User : IUser
    {
        public string Username { get; set; }
        public bool Private { get; set; }
        public string Name { get; set; }
        public bool Vip { get; set; }
        public string JoinedAt { get; set; }
        public string Location { get; set; }
        public string About { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public IImage Images { get; set; }
    }
}