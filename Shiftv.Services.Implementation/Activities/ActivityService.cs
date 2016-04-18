using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Activity;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.DataServices.Activities;
using Shiftv.Contracts.Domain.Activity;
using Shiftv.Contracts.Services.Accounts;
using Shiftv.Contracts.Services.Activities;
using Shiftv.Global;

namespace Shiftv.Services.Implementation.Activities
{
    public class ActivityService : ServiceHelper, IActivityService
    {
        private IUserService _userService;
        private IActivityTraktDataService _activityDataService;

        public ActivityService(IActivityTraktDataService activityTraktDataService, IUserService userService)
        {
            _userService = userService;
            _activityDataService = activityTraktDataService;
        }

        public async Task<DataResult<IActivity>> GetCommunityActivities()
        {
            //if (!await IsInternet()) return new DataResult<IActivity>(StandardResults.Offline);
            var res = await _activityDataService.GetCommunityActivities();
            return res == null ? new DataResult<IActivity>(StandardResults.Error) : new DataResult<IActivity>(res);
        }  
        
        public async Task<DataResult<IActivity>> GetFriendsActivity()
        {
            //if (!await IsInternet()) return new DataResult<IActivity>(StandardResults.Offline);
            var currentUser = _userService.GetCurrentUser();
            if (currentUser == null) return new DataResult<IActivity>(StandardResults.Error);
            var res = await _activityDataService.GetFriendsActivities(UserTokenDtoFactory.GetDto(currentUser));
            return res == null ? new DataResult<IActivity>(StandardResults.Error) : new DataResult<IActivity>(res);
        }   
        public async Task<DataResult<IActivity>> GetUserActivity(string username)
        {
            //if (!await IsInternet()) return new DataResult<IActivity>(StandardResults.Offline);
            if (string.IsNullOrEmpty(username)) return new DataResult<IActivity>(StandardResults.Error);
            var res = await _activityDataService.GetUserActivities(username);
            return res == null ? new DataResult<IActivity>(StandardResults.Error) : new DataResult<IActivity>(res);
        }
    }
}