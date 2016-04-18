using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Comments;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.Results;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Comments;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Contracts.Services.Accounts;
using Shiftv.Global;

namespace Shiftv.DesignServices.Implementation
{
    public class UserDesignService : IUserService
    {
        public Task<LoginUserResult> LoginToTrakt(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public IUserToken GetCurrentUser()
        {
            var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetCurrentUser.json");
            var streamReader = new StreamReader(manifestResourceStream);
            var jsonString = streamReader.ReadToEnd();
            var tracksCollection = JsonConvert.DeserializeObject<UserTokenDto>(jsonString);
            return UserTokenDtoFactory.Create(tracksCollection);
        }

        public void SetUser(IUserToken userAccount)
        {
            throw new System.NotImplementedException();
        }

        Task<DataResult<List<IUser>>> IUserService.GetFollowingByUsername(string username)
        {
            throw new System.NotImplementedException();
        }

        Task<DataResult<List<IUser>>> IUserService.GetFollowersByUsername(string username)
        {
            throw new System.NotImplementedException();
        }

        Task<DataResult<List<IUser>>> IUserService.GetFriendsByUsername(string username)
        {
            throw new System.NotImplementedException();
        }

        Task<DataResult<IUser>> IUserService.GetUserProfileByUsername(string username)
        {
            throw new System.NotImplementedException();
        }


        public Task<DataResult<List<IUser>>> GetFollowingByUsername(string username)
        {
            return new Task<DataResult<List<IUser>>>(() => new DataResult<List<IUser>>(StandardResults.Ok));
        }

        public Task<DataResult<List<IUser>>> GetFollowersByUsername(string username)
        {
            return new Task<DataResult<List<IUser>>>(() => new DataResult<List<IUser>>(StandardResults.Ok));
        }

        public Task<DataResult<List<IUser>>> GetFriendsByUsername(string username)
        {
            return new Task<DataResult<List<IUser>>>(() => new DataResult<List<IUser>>(StandardResults.Ok));
        }

        public Task<DataResult<IUser>> GetUserProfileByUsername(string username)
        {
            return Task.Run(() =>
             {
                 var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetUserProfile.json");
                 var streamReader = new StreamReader(manifestResourceStream);
                 var jsonString = streamReader.ReadToEnd();
                 var tracksCollection = JsonConvert.DeserializeObject<UserDto>(jsonString);
                 return new DataResult<IUser>(UserDtoFactory.Create(tracksCollection));
             });
        }

        public Task<DataResult<List<IUser>>> SearchUserByKey(string queryText)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetUserByKey.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<UserDto>>(jsonString);
                return new DataResult<List<IUser>>(tracksCollection.Select(UserDtoFactory.Create).ToList());
            });
        }

        public void SetUserAsSilverBadge()
        {
            throw new System.NotImplementedException();
        }

        public void SetUserAsGoldBadge(string text)
        {
            throw new System.NotImplementedException();
        }

        public Task<DataResult<IShiftvUserStats>> GetUserStats(string username)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetUserStats.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<ShiftvUserStatsDto>>(jsonString);
                return new DataResult<IShiftvUserStats>(tracksCollection.Select(ShiftvUserStatsDtoFactory.Create).First());
            });
        }

        Task<DataResult<IUserToken>> IUserService.GetToken(string authCode)
        {
            throw new System.NotImplementedException();
        }

        public Task GetToken(string authCode)
        {
            throw new System.NotImplementedException();
        }
    }
}