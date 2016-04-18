using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shiftv.Contracts.DataServices.Activities
{
    public interface IActivityTraktQueryService
    {
        Task<string> GetCommunityActivities();
        Task<string> GetFriendsActivities();
        Task<string> GetUserActivities(string username);
    }
}
