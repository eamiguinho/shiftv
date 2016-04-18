using System;
using System.Threading.Tasks;
using Shiftv.Contracts.Data.Activity;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices.Activities;
using Shiftv.Contracts.Domain.Activity;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Activities
{
    public class ActivityTraktDataService : IActivityTraktDataService
    {
        private IActivityTraktQueryService _queryService;

        public ActivityTraktDataService(IActivityTraktQueryService queryService)
        {
            _queryService = queryService;
        }

        public Task<IActivity> GetCommunityActivities()
        {
            return Task.Run(async () =>
            {   
                try
                {
                    var url = await _queryService.GetCommunityActivities();
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<ActivityDto>(url);
                    return ActivityDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<IActivity> GetFriendsActivities(UserTokenDto account)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (account == null) return null;
                    var url = await _queryService.GetFriendsActivities();
                    var x = await TraktDataServiceHelper.GetObjectWithCredentials<ActivityDto>(url, account);
                    return ActivityDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<IActivity> GetUserActivities(string username)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(username)) return null;
                    var url = await _queryService.GetUserActivities(username);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<ActivityDto>(url);
                    return ActivityDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }
    }
}
