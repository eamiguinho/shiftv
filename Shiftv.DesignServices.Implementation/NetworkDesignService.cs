using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Comments;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Contracts.Services.Networks;
using Shiftv.Global;

namespace Shiftv.DesignServices.Implementation
{
    public class NetworkDesignService : INetworkService
    {
        public Task<DataResult<List<IUser>>> GetRequests()
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.FollowRequests.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<UserDto>>(jsonString);
                return new DataResult<List<IUser>>(tracksCollection.Select(UserDtoFactory.Create).ToList());
            });
        }

        public Task<DataResult<INetworkFollowResult>> Follow(string username)
        {
            return null;
        }

        public Task<DataResult<INetworkFollowResult>> Unfollow(string username)
        {
            return null;
        }
    }
}
