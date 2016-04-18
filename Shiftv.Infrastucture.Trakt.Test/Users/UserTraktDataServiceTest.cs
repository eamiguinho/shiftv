using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shiftv.Contracts.DataServices.Users.Fakes;
using Shiftv.Core.Models;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;
using Shiftv.Infrastucture.Trakt.Implementation.Users;

namespace Shiftv.Infrastucture.Trakt.Test.Users
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class UserTraktDataServiceTest
    {
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            CoreModelsIoc.RegisterTypes();
        }

        [TestMethod]
        public async Task UserTraktDataServiceTest_GetFollowingByUsername_QueryString()
        {
            var ctx = new UserTraktDataService(null, null);
            var x = await ctx.GetFollowingByUsername(null, null);
            Assert.IsNull(x);

            var stub = new StubIUserTraktQueryService
            {
                GetFollowingByUsernameString = (a) => Task.Run(() => null as string)
            };

            ctx = new UserTraktDataService(stub, null);
            x = await ctx.GetFollowingByUsername("afsdasfa", null);
            Assert.IsNull(x);

            stub = new StubIUserTraktQueryService
            {
                GetFollowingByUsernameString = (a) => Task.Run(() => "adgasdasfasda")
            };
            ctx = new UserTraktDataService(stub, null);
            x = await ctx.GetFollowingByUsername("amiguinho", null);
            Assert.IsNull(x);
        } 
        
        [TestMethod]
        public async Task UserTraktDataServiceTest_GetFollowingByUsername_DataTest()
        {
            var url = "http://api.trakt.tv/user/network/following.json/" + TraktConstants.TraktKey + "/goldie75/";
            var ctx = new UserTraktDataService(null, null);
            var x = await ctx.GetFollowingByUsername(null, null);
            Assert.IsNull(x);

            var stub = new StubIUserTraktQueryService
            {
                GetFollowingByUsernameString = (a) => Task.Run(() => url)
            };

            ctx = new UserTraktDataService(stub, null);
            x = await ctx.GetFollowingByUsername(null, null);
            Assert.IsNull(x);

            ctx = new UserTraktDataService(stub, null);
            x = await ctx.GetFollowingByUsername("goldie75", null);
            Assert.IsNotNull(x);
        }  
        
        [TestMethod]
        public async Task UserTraktDataServiceTest_GetFollowersByUsername_QueryString()
        {
            var ctx = new UserTraktDataService(null, null);
            var x = await ctx.GetFollowersByUsername(null, null);
            Assert.IsNull(x);

            var stub = new StubIUserTraktQueryService
            {
                GetFollowersByUsernameString = (a) => Task.Run(() => null as string)
            };

            ctx = new UserTraktDataService(stub, null);
            x = await ctx.GetFollowingByUsername("afsdasfa", null);
            Assert.IsNull(x);

            stub = new StubIUserTraktQueryService
            {
                GetFollowersByUsernameString = (a) => Task.Run(() => "adgasdasfasda")
            };
            ctx = new UserTraktDataService(stub, null);
            x = await ctx.GetFollowersByUsername("amiguinho", null);
            Assert.IsNull(x);
        } 
        
        [TestMethod]
        public async Task UserTraktDataServiceTest_GetFollowersByUsername_DataTest()
        {
            var url = "http://api.trakt.tv/user/network/followers.json/" + TraktConstants.TraktKey + "/goldie75/";
            var ctx = new UserTraktDataService(null, null);
            var x = await ctx.GetFollowersByUsername(null, null);
            Assert.IsNull(x);

            var stub = new StubIUserTraktQueryService
            {
                GetFollowersByUsernameString = (a) => Task.Run(() => url)
            };

            ctx = new UserTraktDataService(stub, null);
            x = await ctx.GetFollowersByUsername(null, null);
            Assert.IsNull(x);

            ctx = new UserTraktDataService(stub, null);
            x = await ctx.GetFollowersByUsername("goldie75", null);
            Assert.IsNotNull(x);
        }


        [TestMethod]
        public async Task UserTraktDataServiceTest_GetFriendsByUsername_QueryString()
        {
            var ctx = new UserTraktDataService(null, null);
            var x = await ctx.GetFriendsByUsername(null, null);
            Assert.IsNull(x);

            var stub = new StubIUserTraktQueryService
            {
                GetFriendsByUsernameString = (a) => Task.Run(() => null as string)
            };

            ctx = new UserTraktDataService(stub, null);
            x = await ctx.GetFriendsByUsername("afsdasfa",null);
            Assert.IsNull(x);

            stub = new StubIUserTraktQueryService
            {
                GetFriendsByUsernameString = (a) => Task.Run(() => "adgasdasfasda")
            };
            ctx = new UserTraktDataService(stub, null);
            x = await ctx.GetFriendsByUsername("amiguinho", null);
            Assert.IsNull(x);
        }

        [TestMethod]
        public async Task UserTraktDataServiceTest_GetFriendsByUsername_DataTest()
        {
            var url = "http://api.trakt.tv/user/network/friends.json/" + TraktConstants.TraktKey + "/goldie75/";
            var ctx = new UserTraktDataService(null, null);
            var x = await ctx.GetFriendsByUsername(null, null);
            Assert.IsNull(x);

            var stub = new StubIUserTraktQueryService
            {
                GetFriendsByUsernameString = (a) => Task.Run(() => url)
            };

            ctx = new UserTraktDataService(stub, null);
            x = await ctx.GetFriendsByUsername(null, null);
            Assert.IsNull(x);

            ctx = new UserTraktDataService(stub, null);
            x = await ctx.GetFriendsByUsername("goldie75", null);
            Assert.IsNotNull(x);
        }
    }
}   
