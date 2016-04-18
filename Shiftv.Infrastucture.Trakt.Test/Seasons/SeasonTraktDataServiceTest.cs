using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices.Seasons.Fakes;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Core.Models;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;
using Shiftv.Infrastucture.Trakt.Implementation.Seasons;

namespace Shiftv.Infrastucture.Trakt.Test.Seasons
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SeasonTraktDataServiceTest
    {
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            CoreModelsIoc.RegisterTypes();
        }
        [TestMethod]
        public async Task SeasonTraktDataServiceTest_SetSeasonAsSeen_QueryString()
        {
            var ctx = new SeasonTraktDataService(null);
            var x = await ctx.SetSeasonAsSeen(null,-1, null, null, -1, -1);
            Assert.IsNull(x);

            var stub = new StubISeasonTraktQueryService
            {
                SetSeasonAsSeen = () => Task.Run(() => null as string)
            };

            var u = new UserTokenDto();
            u.AccessToken = "ttt";

            ctx = new SeasonTraktDataService(stub);
            x = await ctx.SetSeasonAsSeen(u, 123123, "tt123141t", "The cenas", 2000, 1);
            Assert.IsNull(x);

            stub = new StubISeasonTraktQueryService
            {
                SetSeasonAsSeen = () => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new SeasonTraktDataService(stub);
            x = await ctx.SetSeasonAsSeen(u, 123123, "tt123141t", "The cenas", 2000, 1);
            Assert.IsNull(x);
        }

        [TestMethod]
        public async Task SeasonTraktDataServiceTest_SetSeasonAsSeen_DataTest()
        {
            var url = "http://api.trakt.tv/show/season/seen/" + TraktConstants.TraktKey;
            var ctx = new SeasonTraktDataService(null);
            var x = await ctx.SetSeasonAsSeen(null, -1, null, null, -1, -1);
            Assert.IsNull(x);

            var stub = new StubISeasonTraktQueryService
            {
                SetSeasonAsSeen = () => Task.Run(() => url)
            };

            const int tvDbId = 258823;
            const string imdbId = "tt2070791";
            const int year = 2012;
            const string title = "Revolution";

            var u = new UserTokenDto();
            u.AccessToken = "ttt";

            ctx = new SeasonTraktDataService(stub);
            x = await ctx.SetSeasonAsSeen(u, tvDbId, imdbId, title,year, 1);
            Assert.AreEqual(RequestResults.Success,x.Status);
        }

        [TestMethod]
        public Task SeasonTraktDataServiceTest_SetSeasonAsUnSeen_DataTest()
        {
            Assert.Inconclusive("TODO SEND EMAIL TO TRAKT!");
            //var url = "http://api.trakt.tv/show/season/unseen/" + TraktConstants.TraktKey;
            //var ctx = new SeasonTraktDataService(null);
            //var x = await ctx.SetSeasonAsSeen(null, -1, null, null, -1, -1);
            //Assert.IsNull(x);

            //var stub = new StubISeasonTraktQueryService
            //{
            //    SetSeasonAsUnSeen = () => Task.Run(() => url)
            //};

            //const int tvDbId = 258823;
            //const string imdbId = "tt2070791";
            //const int year = 2012;
            //const string title = "Revolution";

            //var u = new UserAccountDto();
            //u.Username = "amiguinho";
            //u.PasswordEnc = "25713423e";

            //ctx = new SeasonTraktDataService(stub);
            //x = await ctx.SetSeasonAsUnSeen(u, tvDbId, imdbId, title, year, 1);
            //Assert.AreEqual(RequestResults.Success, x.Status);
            return null;
        }   
    }
}
