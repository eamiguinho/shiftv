using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.DataServices.Networks;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Contracts.Services.Accounts;
using Shiftv.Contracts.Services.Networks;
using Shiftv.Global;

namespace Shiftv.Services.Implementation.Networks
{
    class NetworkService : ServiceHelper, INetworkService
    {
        private IUserService _userService;
        private INetworkTraktDataService _dataService;

        public NetworkService(INetworkTraktDataService dataService, IUserService userService)
        {
            _dataService = dataService;
            _userService = userService;
        }

        public async Task<DataResult<List<IUser>>> GetRequests()
        {
            //if (!await IsInternet()) return new DataResult<List<IUserProfile>>(StandardResults.Offline);
            var currentUser = _userService.GetCurrentUser();
            if (currentUser == null) return new DataResult<List<IUser>>(StandardResults.Error);
            var res = await _dataService.GetFollowRequests(UserTokenDtoFactory.GetDto(currentUser));
            return res == null ? new DataResult<List<IUser>>(StandardResults.Error) : new DataResult<List<IUser>>(res);
        }

        public async Task<DataResult<INetworkFollowResult>> Follow(string username)
        {
            //if (!await IsInternet()) return new DataResult<INetworkFollowResult>(StandardResults.Offline);
            var currentUser = _userService.GetCurrentUser();
            if (currentUser == null) return new DataResult<INetworkFollowResult>(StandardResults.Error);
            var res = await _dataService.Follow(UserTokenDtoFactory.GetDto(currentUser), username);
            return res == null ? new DataResult<INetworkFollowResult>(StandardResults.Error) : new DataResult<INetworkFollowResult>(res);
        }

        public async Task<DataResult<INetworkFollowResult>> Unfollow(string username)
        {
            //if (!await IsInternet()) return new DataResult<INetworkFollowResult>(StandardResults.Offline);
            var currentUser = _userService.GetCurrentUser();
            if (currentUser == null) return new DataResult<INetworkFollowResult>(StandardResults.Error);
            var res = await _dataService.Unfollow(UserTokenDtoFactory.GetDto(currentUser), username);
            return res == null ? new DataResult<INetworkFollowResult>(StandardResults.Error) : new DataResult<INetworkFollowResult>(res);
        }
    }
}