using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Users;

namespace Shiftv.Contracts.DataServices.Networks
{
    public interface INetworkTraktDataService
    {
        Task<List<IUser>> GetFollowRequests(UserTokenDto userAccount);
        Task<INetworkApproveDenyResult> ApproveFollowerRequest(UserTokenDto userAccount, string username, bool followBack);
        Task<INetworkApproveDenyResult> DenyFollowerRequest(UserTokenDto userAccount, string username);
        Task<INetworkFollowResult> Follow(UserTokenDto userAccount, string username);
        Task<INetworkFollowResult> Unfollow(UserTokenDto userAccount, string username);
    }
}