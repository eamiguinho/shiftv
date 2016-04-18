using System.Threading.Tasks;

namespace Shiftv.Contracts.DataServices.Networks
{
    public interface INetworkTraktQueryService
    {
        Task<string> GetFollowRequests();
        Task<string> ApproveFollowerRequest();
        Task<string> DenyFollowerRequest();
        Task<string> Follow();
        Task<string> Unfollow();
    }
}