using System.Threading.Tasks;
using Shiftv.Contracts.Domain.Activity;
using Shiftv.Global;

namespace Shiftv.Contracts.Services.Activities
{
    public interface IActivityService
    {
        Task<DataResult<IActivity>> GetCommunityActivities();
        Task<DataResult<IActivity>> GetFriendsActivity();
        Task<DataResult<IActivity>> GetUserActivity(string username);
    }
}