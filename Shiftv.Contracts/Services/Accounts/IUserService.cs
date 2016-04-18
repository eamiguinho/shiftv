using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Global;
using LoginUserResult = Shiftv.Contracts.Data.Results.LoginUserResult;

namespace Shiftv.Contracts.Services.Accounts
{
    public interface IUserService
    {
        Task<LoginUserResult> LoginToTrakt(string username, string password);
        //Task<DataResult<IUserAccount>> GetUser(string username, string calculateHashForString);

        IUserToken GetCurrentUser();
        void SetUser(IUserToken userAccount);

        Task<DataResult<List<IUser>>> GetFollowingByUsername(string username);
        Task<DataResult<List<IUser>>> GetFollowersByUsername(string username);
        Task<DataResult<List<IUser>>> GetFriendsByUsername(string username);

        Task<DataResult<IUser>> GetUserProfileByUsername(string username);
        //Task<DataResult<List<IUserProfile>>> SearchUserByKey(string queryText);
        void SetUserAsSilverBadge();
        void SetUserAsGoldBadge(string text);
        Task<DataResult<IShiftvUserStats>> GetUserStats(string username);
        Task<DataResult<IUserToken>> GetToken(string authCode);
    }
}