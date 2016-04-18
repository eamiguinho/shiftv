using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Activity;

namespace Shiftv.Contracts.DataServices.Activities
{
    public interface IActivityTraktDataService
    {
        Task<IActivity> GetCommunityActivities();
        Task<IActivity> GetFriendsActivities(UserTokenDto account);
        Task<IActivity> GetUserActivities(string username);
    }
}
