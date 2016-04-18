using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.Results;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices;
using Shiftv.Contracts.DataServices.Users;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Contracts.Services;
using Shiftv.Contracts.Services.Accounts;
using Shiftv.Global;

namespace Shiftv.Services.Implementation.Users
{
    class UserService : ServiceHelper, IUserService
    {
        private IUserTraktDataService _userDataService;
        private IUserToken _currentUser;
        private List<IShiftvUserStats> _userStats;
        public UserService()
        {

        }

        public UserService(IUserTraktDataService userDataService)
        {
            _userDataService = userDataService;
        }
        private string CalculateHashForString(string dataString)
        {
            //var data = new byte[dataString.Length * sizeof(char)];
            //var shaM = new SHA1Managed();
            //var result = shaM.ComputeHash(data);
            return dataString;
        }



        public async Task<LoginUserResult> LoginToTrakt(string username, string password)
        {
            //if (!await IsInternet()) return LoginUserResult.Offline();
            var passwordEnc = CalculateHashForString(password);
            var req = await TraktDataService.Login.LoginToTrakt(username, passwordEnc);
            if (!req.IsOk)
            {
                //var req2 = await TraktDataService.Login.GetUserLocalData(username, passwordEnc);
                //if (req2 != null)
                //{
                //    return new LoginUserResult {Result = ResultBase.Results.Ok};
                //}
            }
            return req;
        }

      

        public IUserToken GetCurrentUser()
        {
            return _currentUser;
        }

        public void SetUser(IUserToken userAccount)
        {
            _currentUser = userAccount;
            //CoreServices.Show.UpdateWatchlist();
        }

        //public async Task<CreateUserResult> CreateAccount(string createUsername, string password, string email)
        //{
        //    //if (!await IsInternet()) return CreateUserResult.Offline();
        //    var passwordEnc = CalculateHashForString(password);
        //    var req = await TraktDataService.Login.CreateAccount(createUsername, passwordEnc, email);
        //    return req;
        //}

        public async Task<DataResult<List<IUser>>> GetFollowingByUsername(string username)
        {
            if (string.IsNullOrEmpty(username)) return new DataResult<List<IUser>>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<List<IUserProfile>>(StandardResults.Offline);
            var req = await _userDataService.GetFollowingByUsername(username, UserTokenDtoFactory.GetDto(GetCurrentUser()));
            return req == null ? new DataResult<List<IUser>>(StandardResults.Error) : new DataResult<List<IUser>>(req);
        }
        public async Task<DataResult<List<IUser>>> GetFollowersByUsername(string username)
        {
            if (string.IsNullOrEmpty(username)) return new DataResult<List<IUser>>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<List<IUserProfile>>(StandardResults.Offline);
            var req = await _userDataService.GetFollowersByUsername(username, UserTokenDtoFactory.GetDto(GetCurrentUser()));
            return req == null ? new DataResult<List<IUser>>(StandardResults.Error) : new DataResult<List<IUser>>(req);
        }
        public async Task<DataResult<List<IUser>>> GetFriendsByUsername(string username)
        {
            if (string.IsNullOrEmpty(username)) return new DataResult<List<IUser>>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<List<IUserProfile>>(StandardResults.Offline);
            var req = await _userDataService.GetFriendsByUsername(username, UserTokenDtoFactory.GetDto(GetCurrentUser()));
            return req == null ? new DataResult<List<IUser>>(StandardResults.Error) : new DataResult<List<IUser>>(req);
        }

        public async Task<DataResult<IUser>> GetUserProfileByUsername(string username)
        {
            if (string.IsNullOrEmpty(username)) return new DataResult<IUser>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IUserProfile>(StandardResults.Offline);
            var req = await _userDataService.GetUserProfileByUsername(username, UserTokenDtoFactory.GetDto(GetCurrentUser()));
            return req == null ? new DataResult<IUser>(StandardResults.Error) : new DataResult<IUser>(req);
        }

        public void SetUserAsSilverBadge()
        {
            var user = GetCurrentUser();
            if(user == null) return;
            _userDataService.SetUserAsSilverBadge(user.UserSettings.User.Username.ToLower());
            _userStats = null;
        }

        public void SetUserAsGoldBadge(string email)
        {
            var user = GetCurrentUser();
            if (user == null) return;
            _userDataService.SetUserAsGoldBadge(user.UserSettings.User.Username.ToLower(), email);
            _userStats = null;
        }

        public async Task<DataResult<IShiftvUserStats>> GetUserStats(string username)
        {
           // return new DataResult<IShiftvUserStats>( StandardResults.Error);
            if (string.IsNullOrEmpty(username)) return new DataResult<IShiftvUserStats>(StandardResults.Error);
            if (_userStats == null) _userStats = await _userDataService.GetUserStats();
            if (_userStats == null) return new DataResult<IShiftvUserStats>(StandardResults.Error);
            if (_userStats.Any(x => String.Equals(x.Username, username, StringComparison.CurrentCultureIgnoreCase)))
            {
                return
                    new DataResult<IShiftvUserStats>(
                        _userStats.FirstOrDefault(
                            x => String.Equals(x.Username, username, StringComparison.CurrentCultureIgnoreCase)));
            }
            else
            {
                return new DataResult<IShiftvUserStats>(StandardResults.Error);
            }
        }

        public async Task<DataResult<IUserToken>>  GetToken(string authCode)
        {
            if (string.IsNullOrEmpty(authCode)) return new DataResult<IUserToken>(StandardResults.Error);
            var x = await _userDataService.GetToken(authCode);
            return new DataResult<IUserToken>(x);
        }
    }
}
