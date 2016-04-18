using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Comments;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices.Users;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Contracts.PlatformSpecificServices;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Users
{
    public class UserTraktDataService : IUserTraktDataService
    {
        private IUserTraktQueryService _queryService;
        private IDataBackupService _backupService;

        public UserTraktDataService(IUserTraktQueryService queryService, IDataBackupService backupService)
        {
            _queryService = queryService;
            _backupService = backupService;
        }

        public Task<List<IUser>> GetFollowingByUsername(string username, UserTokenDto dto)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(username)) return null;
                    var url = await _queryService.GetFollowingByUsername(username);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<UserDto>>(url);
                    return x.Select(UserDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<IUser>> GetFollowersByUsername(string username, UserTokenDto dto)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(username)) return null;
                    var url = await _queryService.GetFollowersByUsername(username);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<UserDto>>(url);
                    return x.Select(UserDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<IUser>> GetFriendsByUsername(string username, UserTokenDto dto)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(username)) return null;
                    var url = await _queryService.GetFriendsByUsername(username);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<UserDto>>(url);
                    return x.Select(UserDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }



        public Task<IUser> GetUserProfileByUsername(string username, UserTokenDto userAccount)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(username)) return null;
                    var url = await _queryService.GetUserProfileByUsername(username);
                    UserDto x;
                    if (userAccount != null) x = await TraktDataServiceHelper.GetObjectWithCredentials<UserDto>(url, userAccount);
                    else x = await TraktDataServiceHelper.GetObjectWithoutCredentials<UserDto>(url);
                    return UserDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }


        public async void SetUserAsSilverBadge(string username)
        {
            var trendingShows = await _backupService.GetFileFromAzure("userStats", BackupContainerTypes.GlobalData);
            var objectReceived = new List<ShiftvUserStatsDto>();
            if (trendingShows != null) objectReceived = JsonConvert.DeserializeObject<List<ShiftvUserStatsDto>>(trendingShows);
            objectReceived.Add(new ShiftvUserStatsDto { IsGold = false, IsSilver = true, Username = username });
            TraktDataServiceHelper.SaveDirectToAzure(objectReceived, "userStats", BackupContainerTypes.GlobalData, _backupService);
        }

   

        public async void SetUserAsGoldBadge(string username, string email)
        {
            var trendingShows = await _backupService.GetFileFromAzure("userStats", BackupContainerTypes.GlobalData);
            var objectReceived = new List<ShiftvUserStatsDto>();
            if (trendingShows != null) objectReceived = JsonConvert.DeserializeObject<List<ShiftvUserStatsDto>>(trendingShows);
            objectReceived.Add(new ShiftvUserStatsDto { IsGold = true, IsSilver = false, Username = username });
            TraktDataServiceHelper.SaveDirectToAzure(objectReceived, "userStats", BackupContainerTypes.GlobalData, _backupService);


            var emails = await _backupService.GetFileFromAzure("emailGoldUsers", BackupContainerTypes.GlobalData);
            var objectReceivedemails = "";
            if (emails != null) objectReceivedemails = JsonConvert.DeserializeObject<string>(emails);
            objectReceivedemails += email + "; ";
            TraktDataServiceHelper.SaveDirectToAzure(objectReceivedemails, "emailGoldUsers", BackupContainerTypes.GlobalData, _backupService);
        }

        public async Task<List<IShiftvUserStats>> GetUserStats()
        {
            var trendingShows = await _backupService.GetFileFromAzure("userStats", BackupContainerTypes.GlobalData);
            if (trendingShows == null) return null;
            var objectReceived = JsonConvert.DeserializeObject<List<ShiftvUserStatsDto>>(trendingShows);
            return objectReceived.Select(ShiftvUserStatsDtoFactory.Create).ToList();
        }

        public Task<IUserToken> GetToken(string authCode)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(authCode)) return null;
                    var url = await _queryService.GetToken(authCode);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<UserTokenDto>(url);
                    return UserTokenDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }
    }
}
