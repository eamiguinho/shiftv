using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices.Activities.Fakes;
using Shiftv.Contracts.DataServices.Shows.Fakes;
using Shiftv.Core.Models;
using Shiftv.Infrastucture.Trakt.Implementation.Activities;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;
using Shiftv.Infrastucture.Trakt.Implementation.Shows;

namespace Shiftv.Infrastucture.Trakt.Test.Activities
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ActivityTraktDataServiceTest
    {
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            CoreModelsIoc.RegisterTypes();
        }

        [TestMethod]
        public async Task ActivityTraktDataServiceTest_GetCommunityActivity_QueryString()
        {
            var stub = new StubIActivityTraktQueryService
            {
                GetCommunityActivities = () => Task.Run(() => null as string)
            };
        
            var ctx = new ActivityTraktDataService(stub);
            var x = await ctx.GetCommunityActivities();
            Assert.IsNull(x);

            stub = new StubIActivityTraktQueryService
            {
                GetCommunityActivities = () => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new ActivityTraktDataService(stub);
            x = await ctx.GetCommunityActivities();
            Assert.IsNull(x);
        }

        [TestMethod]
        public async Task ShowTraktDataServiceTest_GetCommunityActivity_DataTest()
        {
            var url = "http://api.trakt.tv/activity/community.json/" + TraktConstants.TraktKey;
            var stub = new StubIActivityTraktQueryService
            {
                GetCommunityActivities = () => Task.Run(() => url)
            };
            var ctx = new ActivityTraktDataService(stub);
            var rec = await ctx.GetCommunityActivities();
            Assert.IsNotNull(rec);
            Assert.AreNotEqual(0, rec.ActivityItems.Count);
        }

        [TestMethod]
        public async Task ActivityTraktDataServiceTest_GetFriendsActivity_QueryString()
        {
            var userAccount = new UserTokenDto();
            userAccount.AccessToken = "tt";
            var stub = new StubIActivityTraktQueryService
            {
                GetFriendsActivities = () => Task.Run(() => null as string)
            };
        
            var ctx = new ActivityTraktDataService(stub);
            var x = await ctx.GetFriendsActivities(null);
            Assert.IsNull(x);

            stub = new StubIActivityTraktQueryService
            {
                GetFriendsActivities = () => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new ActivityTraktDataService(stub);
            x = await ctx.GetFriendsActivities(userAccount);
            Assert.IsNull(x);
        }

        [TestMethod]
        public async Task ShowTraktDataServiceTest_GetFriendsActivity_DataTest()
        {

            var userAccount = new UserTokenDto();
            userAccount.AccessToken = "tt";
            var url = "http://api.trakt.tv/activity/friends.json/" + TraktConstants.TraktKey;
            var stub = new StubIActivityTraktQueryService
            {
                GetFriendsActivities = () => Task.Run(() => url)
            };
            var ctx = new ActivityTraktDataService(stub);
            var rec = await ctx.GetFriendsActivities(userAccount);
            Assert.IsNotNull(rec);
            Assert.AreNotEqual(0, rec.ActivityItems.Count);
        }


        [TestMethod]
        public async Task ActivityTraktDataServiceTest_GetUserActivity_QueryString()
        {
            var u = "amiguinho";
            var stub = new StubIActivityTraktQueryService
            {
                GetUserActivitiesString = (s) => Task.Run(() => null as string)
            };

            var ctx = new ActivityTraktDataService(stub);
            var x = await ctx.GetFriendsActivities(null);
            Assert.IsNull(x);

            stub = new StubIActivityTraktQueryService
            {
                GetUserActivitiesString = (s) => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new ActivityTraktDataService(stub);
            x = await ctx.GetUserActivities(u);
            Assert.IsNull(x);
        }

        [TestMethod]
        public async Task ShowTraktDataServiceTest_GetUserActivity_DataTest()
        {
            var u = "amiguinho";
            var url = "http://api.trakt.tv/activity/user.json/" + TraktConstants.TraktKey + "/" +u;
            var stub = new StubIActivityTraktQueryService
            {
                GetUserActivitiesString = (s) => Task.Run(() => url)
            };
            var ctx = new ActivityTraktDataService(stub);
            var rec = await ctx.GetUserActivities(u);
            Assert.IsNotNull(rec);
            Assert.AreNotEqual(0, rec.ActivityItems.Count);
        }
    }
}
