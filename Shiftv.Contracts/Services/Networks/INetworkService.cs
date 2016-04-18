using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Global;

namespace Shiftv.Contracts.Services.Networks
{
    public interface INetworkService
    {
        Task<DataResult<List<IUser>>> GetRequests();
        Task<DataResult<INetworkFollowResult>> Follow(string username);
         Task<DataResult<INetworkFollowResult>> Unfollow(string username);
    }
}
