using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Activity;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Domain.Activity;
using Shiftv.Contracts.Services.Activities;
using Shiftv.Global;

namespace Shiftv.DesignServices.Implementation
{
    public class ActivityDesignService : IActivityService
    {
        public Task<DataResult<IActivity>> GetCommunityActivities()
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.Activities.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<ActivityDto>(jsonString);
                return new DataResult<IActivity>(ActivityDtoFactory.Create(tracksCollection));
            });
        }

        public Task<DataResult<IActivity>> GetFriendsActivity()
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.Activities.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<ActivityDto>(jsonString);
                return new DataResult<IActivity>(ActivityDtoFactory.Create(tracksCollection));
            });
        }

        public Task<DataResult<IActivity>> GetUserActivity(string username)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.Activities.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<ActivityDto>(jsonString);
                return new DataResult<IActivity>(ActivityDtoFactory.Create(tracksCollection));
            });
        }
    }
}