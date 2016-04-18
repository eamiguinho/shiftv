using System.Collections.Generic;

namespace Shiftv.Contracts.Domain.Users
{
    public interface IUserProfile
    {
        string Username { get; set; }
        string FullName { get; set; }
        string Gender { get; set; }
        int? Age { get; set; }
        string Location { get; set; }
        string About { get; set; }
        long? Joined { get; set; }
        long? LastLogin { get; set; }
        string Avatar { get; set; }
        string Url { get; set; }
        bool IsVip { get; set; }
        bool Protected { get; set; }

        IUserProfileStats Stats { get; set; }
        List<IUserProfileWatching> Watching { get; set; }
        List<IUserProfileWatched> Watched { get; set; }
       
    }
}