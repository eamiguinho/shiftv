using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Users;

namespace Shiftv.Contracts.DataServices.Users
{
    public interface IUserTraktDataService
    {
        Task<List<IUser>> GetFollowingByUsername(string username, UserTokenDto dto);
        Task<List<IUser>> GetFollowersByUsername(string username, UserTokenDto dto);
        Task<List<IUser>> GetFriendsByUsername(string username, UserTokenDto dto);
        Task<IUser> GetUserProfileByUsername(string username, UserTokenDto dto);
        void SetUserAsSilverBadge(string username);
        void SetUserAsGoldBadge(string toLower, string email);
        Task<IUserToken> GetToken(string authCode);
        Task<List<IShiftvUserStats>> GetUserStats();
    }
}