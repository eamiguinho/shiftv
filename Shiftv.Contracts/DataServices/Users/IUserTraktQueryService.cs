using System.Threading.Tasks;

namespace Shiftv.Contracts.DataServices.Users
{
    public interface IUserTraktQueryService
    {
        Task<string> GetFollowingByUsername(string username);
        Task<string> GetFollowersByUsername(string username);
        Task<string> GetFriendsByUsername(string username);
        Task<string> GetUserProfileByUsername(string username);
        Task<string> SearchUserByKey(string queryText);
        Task<string> GetToken(string authCode);
    }
}