using System;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Lists;
using ShiftvAPI.Infrastucture.Trakt.Implementation.Helpers.Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace ShiftvAPI.Infrastucture.Trakt.Implementation.Lists
{
    class ListTraktQueryService : IListTraktQueryService
    {
        public Task<string> GetListInfo(string user, string id)
        {
            //https://api.trakt.tv/users/username/lists/id
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}/{4}",
              TraktConstants.BaseApiUrl,
              TraktConstants.UsersAction,
              user,
              TraktConstants.Lists,id));
        }

        public Task<string> GetListItems(string user, string id)
        {
            //https://api.trakt.tv/users/username/lists/id
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}/{4}/{5}?{6}",
              TraktConstants.BaseApiUrl,
              TraktConstants.UsersAction,
              user,
              TraktConstants.Lists,
              id,
              TraktConstants.Items, TraktConstants.ExtendedImagesData));
        }
    }
}
