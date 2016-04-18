using System.Collections.Generic;
using Newtonsoft.Json;
using Shiftv.Contracts.Domain.Users;

namespace Shiftv.Core.Models.Users
{
    public class UserProfile : IUserProfile
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string Location { get; set; }
        public string About { get; set; }
        public long? Joined { get; set; }
        public long? LastLogin { get; set; }
        public string Avatar { get; set; }
        public string Url { get; set; }
        public bool IsVip { get; set; }
        public bool Protected { get; set; }
        public IUserProfileStats Stats { get; set; }
        public List<IUserProfileWatching> Watching { get; set; }
        public List<IUserProfileWatched> Watched { get; set; }
        public bool IsGold { get; set; }
        public bool IsSilver { get; set; }
    }


    public class ShiftvUserStats : IShiftvUserStats
    {
        public bool IsGold { get; set; }
        public bool IsSilver { get; set; }
        public string Username { get; set; }
    }
}