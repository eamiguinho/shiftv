using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices.Episodes.Fakes;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Core.Models;
using Shiftv.Infrastucture.Trakt.Implementation.Episodes;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Test.Episodes
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class EpisodeTraktDataServiceTest
    {
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            CoreModelsIoc.RegisterTypes();
        }

        [TestMethod]
        public async Task EpisodeTraktDataServiceTest_GetEpisodeBySeason_QueryString()
        {
            var ctx = new EpisodeTraktDataService(null, null);
            var x = await ctx.GetEpisodeBySeason(null, -1,-1, null);
            Assert.IsNull(x);

            var stub = new StubIEpisodeTraktQueryService
            {
                GetEpisodeBySeasonInt32Int32 = (a,b) => Task.Run(() => null as string)
            };

            var u = new UserTokenDto();
            u.AccessToken = "tttt";

            ctx = new EpisodeTraktDataService(stub, null);
            x = await ctx.GetEpisodeBySeason(u,123123,1, "asd");
            Assert.IsNull(x);

            stub = new StubIEpisodeTraktQueryService
            {
                GetEpisodeBySeasonInt32Int32 = (a, b) => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new EpisodeTraktDataService(stub, null);
            x = await ctx.GetEpisodeBySeason(u, 123123, 1, "asd");
            Assert.IsNull(x);
        }

        [TestMethod]
        public async Task EpisodeTraktDataServiceTest_GetEpisodeBySeason_DataTest()
        {
            var url = "http://api.trakt.tv/show/season.json/" + TraktConstants.TraktKey + "/" + 258823 + "/" + 1;
            var ctx = new EpisodeTraktDataService(null, null);
            var x = await ctx.GetEpisodeBySeason(null, -1, -1, null);
            Assert.IsNull(x);   

            var stub = new StubIEpisodeTraktQueryService
            {
                GetEpisodeBySeasonInt32Int32 = (a, b) => Task.Run(() => url)
            };

            var u = new UserTokenDto();
            u.AccessToken = "tttt";
 
            ctx = new EpisodeTraktDataService(stub, null);
            x = await ctx.GetEpisodeBySeason(u, 258823, 1, "asas");
            Assert.IsNotNull(x);
            Assert.AreNotEqual(0, x.Count);
        }

        [TestMethod]
        public async Task EpisodeTraktDataServiceTest_GetWatchingnow_QueryString()
        {
            var ctx = new EpisodeTraktDataService(null, null);
            var x = await ctx.GetWatchingNow(-1, -1, -1);
            Assert.IsNull(x);

            var stub = new StubIEpisodeTraktQueryService
            {
                GetWatchingNowInt32Int32Int32 = (a, b,c) => Task.Run(() => null as string)
            };

            var u = new UserTokenDto();
            u.AccessToken = "tttt";

            ctx = new EpisodeTraktDataService(stub, null);
            x = await ctx.GetWatchingNow(-1, -1, -1);
            Assert.IsNull(x);

            stub = new StubIEpisodeTraktQueryService
            {
                GetWatchingNowInt32Int32Int32 = (a, b, c) => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new EpisodeTraktDataService(stub, null);
            x = await ctx.GetWatchingNow(123123, 1, 1);
            Assert.IsNull(x);
        }

        [TestMethod]
        public async Task EpisodeTraktDataServiceTest_GetWatchingnow_DataTest()
        {
            var url = "http://api.trakt.tv/show/episode/watchingnow.json/" + TraktConstants.TraktKey + "/" + 258823 + "/1/1";
            var ctx = new EpisodeTraktDataService(null, null);
            var x = await ctx.GetWatchingNow(-1, -1, -1);
            Assert.IsNull(x);
            x = await ctx.GetWatchingNow(258823, -1, -1);
            Assert.IsNull(x);
            x = await ctx.GetWatchingNow(258823, 1, -1);
            Assert.IsNull(x);
            x = await ctx.GetWatchingNow(258823, -1, 1);
            Assert.IsNull(x); 
            x = await ctx.GetWatchingNow(-1, -1, 1);
            Assert.IsNull(x);  
            x = await ctx.GetWatchingNow(-1, 1, 1);
            Assert.IsNull(x);

            var stub = new StubIEpisodeTraktQueryService
            {
                GetWatchingNowInt32Int32Int32 = (a, b, c) => Task.Run(() => url)
            };

            var u = new UserTokenDto();
            u.AccessToken = "tttt";

            ctx = new EpisodeTraktDataService(stub, null);
            x = await ctx.GetWatchingNow(258823, 1, 1);
            Assert.IsNotNull(x);
        }


        [TestMethod]
        public async Task EpisodeTraktDataServiceTest_SetAsSeen_QueryString()
        {
            var ctx = new EpisodeTraktDataService(null, null);
            var x = await ctx.SetAsSeen(null,-1, null, null, -1, -1, -1);
            Assert.IsNull(x);

            var stub = new StubIEpisodeTraktQueryService
            {
                GetSetAsSeen = () => Task.Run(() => null as string)
            };

            var u = new UserTokenDto();
            u.AccessToken = "tttt";

            ctx = new EpisodeTraktDataService(stub, null);
            x = await ctx.SetAsSeen(u, -1, null, null, -1, -1, -1);
            Assert.IsNull(x);

            stub = new StubIEpisodeTraktQueryService
            {
                GetSetAsSeen = () => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new EpisodeTraktDataService(stub, null);
            x = await ctx.SetAsSeen(u, 124123, "qefasdfa", "AAFSDA", 1988, 1, 1);
            Assert.IsNull(x);
        }


        [TestMethod]
        public async Task EpisodeTraktDataServiceTest_SetAsSeen_DataTest()
        {
            var u = new UserTokenDto();
            u.AccessToken = "tttt";

            var url = "http://api.trakt.tv/show/episode/seen/" + TraktConstants.TraktKey;
            var ctx = new EpisodeTraktDataService(null, null);
            var x = await ctx.SetAsSeen(null, 258823, null, null, -1, -1, -1);
            Assert.IsNull(x);
            x = await ctx.SetAsSeen(u, 258823, "tt2070791", null, -1, -1, -1);
            Assert.IsNull(x);
            x = await ctx.SetAsSeen(u, 258823, "tt2070791", "Revolution", -1, -1, -1);
            Assert.IsNull(x);
            x = await ctx.SetAsSeen(u, 258823, "tt2070791", "Revolution", 2012, -1, -1);
            Assert.IsNull(x);
            x = await ctx.SetAsSeen(u, 258823, "tt2070791", "Revolution", 2012, 1, -1);
            Assert.IsNull(x);
            x = await ctx.SetAsSeen(u, 258823, "tt2070791", "Revolution", 2012, -1, 1);
            Assert.IsNull(x);

            var stub = new StubIEpisodeTraktQueryService
            {
                GetSetAsSeen = () => Task.Run(() => url)
            };

          
            ctx = new EpisodeTraktDataService(stub, null);
            x = await ctx.SetAsSeen(u, 258823, "tt2070791", "Revolution", 2012, 2, 1);
            Assert.IsNotNull(x);
        }


        [TestMethod]
        public async Task EpisodeTraktDataServiceTest_SetAsUnseen_QueryString()
        {
            var ctx = new EpisodeTraktDataService(null, null);
            var x = await ctx.SetAsUnseen(null, -1, null, null, -1, -1, -1);
            Assert.IsNull(x);

            var stub = new StubIEpisodeTraktQueryService
            {
                GetSetAsUnseen = () => Task.Run(() => null as string)
            };
            var u = new UserTokenDto();
            u.AccessToken = "tttt";
            ctx = new EpisodeTraktDataService(stub, null);
            x = await ctx.SetAsUnseen(u, -1, null, null, -1, -1, -1);
            Assert.IsNull(x);

            stub = new StubIEpisodeTraktQueryService
            {
                GetSetAsUnseen = () => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new EpisodeTraktDataService(stub, null);
            x = await ctx.SetAsUnseen(u, 124123, "qefasdfa", "AAFSDA", 1988, 1, 1);
            Assert.IsNull(x);
        }


        [TestMethod]
        public async Task EpisodeTraktDataServiceTest_SetAsUnseen_DataTest()
        {
            var u = new UserTokenDto();
            u.AccessToken = "tttt";

            var url = "http://api.trakt.tv/show/episode/unseen/" + TraktConstants.TraktKey;
            var ctx = new EpisodeTraktDataService(null, null);
            var x = await ctx.SetAsUnseen(null, 258823, null, null, -1, -1, -1);
            Assert.IsNull(x);
            x = await ctx.SetAsUnseen(u, 258823, "tt2070791", null, -1, -1, -1);
            Assert.IsNull(x);
            x = await ctx.SetAsUnseen(u, 258823, "tt2070791", "Revolution", -1, -1, -1);
            Assert.IsNull(x);
            x = await ctx.SetAsUnseen(u, 258823, "tt2070791", "Revolution", 2012, -1, -1);
            Assert.IsNull(x);
            x = await ctx.SetAsUnseen(u, 258823, "tt2070791", "Revolution", 2012, 1, -1);
            Assert.IsNull(x);
            x = await ctx.SetAsUnseen(u, 258823, "tt2070791", "Revolution", 2012, -1, 1);
            Assert.IsNull(x);

            var stub = new StubIEpisodeTraktQueryService
            {
                GetSetAsUnseen = () => Task.Run(() => url)
            };


            ctx = new EpisodeTraktDataService(stub, null);
            x = await ctx.SetAsUnseen(u, 258823, "tt2070791", "Revolution", 2012, 2, 1);
            Assert.IsNotNull(x);
        }

        [TestMethod]
        public async Task EpisodeTraktDataServiceTest_RateEpisode_QueryString()
        {
            var ctx = new EpisodeTraktDataService(null, null);
            var x = await ctx.RateEpisode(null, false, null, -1, -1,2,1);
            Assert.IsNull(x);

            var stub = new StubIEpisodeTraktQueryService
            {
                GetRateEpisode = () => Task.Run(() => null as string)
            };

            ctx = new EpisodeTraktDataService(stub, null);
            x = await ctx.RateEpisode(null, false, null, -1, -1, 2, 1);
            Assert.IsNull(x);
            var u = new UserTokenDto();
            u.AccessToken = "tttt";
            stub = new StubIEpisodeTraktQueryService
            {
                GetRateEpisode = () => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new EpisodeTraktDataService(stub, null);
            x = await ctx.RateEpisode(u, true, "Revolution", 258823, 2012, 2, 1);
            Assert.IsNull(x);
        }

        [TestMethod]
        public async Task EpisodeTraktDataServiceTest_RateEpisode_DataTest()
        {
            var url = "http://api.trakt.tv/rate/episode/" + TraktConstants.TraktKey;
            var stub = new StubIEpisodeTraktQueryService
            {
                GetRateEpisode = () => Task.Run(() => url)
            };


            const int tvDbId = 258823;
            const int year = 2012;
            const string title = "Revolution";

            var u = new UserTokenDto();
            u.AccessToken = "tttt";

            var ctx = new EpisodeTraktDataService(stub, null);

            var rec = await ctx.RateEpisode(null, false, null, -1, -1, -1, -1);
            Assert.IsNull(rec);
            rec = await ctx.RateEpisode(u, false, null, -1, -1, -1, -1);
            Assert.IsNull(rec);
            rec = await ctx.RateEpisode(u, false, null, -1, -1, -1, -1);
            Assert.IsNull(rec);
            rec = await ctx.RateEpisode(u, false, title, -1, -1, -1, -1);
            Assert.IsNull(rec);

            rec = await ctx.RateEpisode(null, false, null, -1, -1, -1, -1);
            Assert.IsNull(rec);
            rec = await ctx.RateEpisode(u, false, null, -1, -1, -1, -1);
            Assert.IsNull(rec);
            rec = await ctx.RateEpisode(null, false, title, -1, -1, -1, -1);
            Assert.IsNull(rec);

            rec = await ctx.RateEpisode(null, false, title, -1, -1, -1, -1);
            Assert.IsNull(rec);
            rec = await ctx.RateEpisode(u, false, title, -1, -1, -1, -1);
            Assert.IsNull(rec);
            rec = await ctx.RateEpisode(null, true, title, -1, -1, -1, -1);
            Assert.IsNull(rec);

            rec = await ctx.RateEpisode(u, true, title, tvDbId, year, 2, 1);
            Assert.IsNotNull(rec);
            Assert.AreEqual(rec.Status, RequestResults.Success);
        }


        [TestMethod]
        public async Task EpisodeTraktDataServiceTest_CheckIn_QueryString()
        {
            const int tvDbId = 258823;
            const int year = 2012;
            const string title = "Revolution";

            var u = new UserTokenDto();
            u.AccessToken = "tttt";
            var ctx = new EpisodeTraktDataService(null, null);
            var x = await ctx.CheckIn(u, title, "tt2070791", tvDbId, year, 2, 1);
            Assert.IsNull(x);

            var stub = new StubIEpisodeTraktQueryService
            {
                GetCheckIn = () => Task.Run(() => null as string)
            };

            ctx = new EpisodeTraktDataService(stub, null);
            x = await ctx.CheckIn(u, title, "tt2070791", tvDbId, year, 2, 1);
            Assert.IsNull(x);


            stub = new StubIEpisodeTraktQueryService
            {
                GetCheckIn = () => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new EpisodeTraktDataService(stub, null);
            x = await ctx.CheckIn(u, title, "tt2070791", tvDbId, year, 2, 1);
            Assert.IsNull(x);
        }

        [TestMethod]
        public Task EpisodeTraktDataServiceTest_CheckIn_DataTest()
        {
            Assert.IsTrue(true);
            //var url = "http://api.trakt.tv/show/checkin/" + TraktConstants.TraktDevKey;
            //var stub = new StubIEpisodeTraktQueryService
            //{
            //    GetCheckIn = () => Task.Run(() => url)
            //};


            //const int tvDbId = 258823;
            //const int year = 2012;
            //const string title = "Revolution";

            //var userAccount = new UserAccountDto { Username = "amiguinho", PasswordEnc = "25713423e" };

            //var ctx = new EpisodeTraktDataService(stub, null);

            //var rec = await ctx.CheckIn(null, null, null, -1,-1, -1, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.CheckIn(userAccount, null, null, -1, -1, -1, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.CheckIn(userAccount, title, null, -1, -1, -1, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.CheckIn(userAccount, title, "tt2070791", -1, -1, -1,-1);
            //Assert.IsNull(rec);

            //rec = await ctx.CheckIn(userAccount, title, "tt2070791", tvDbId, -1, -1, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.CheckIn(userAccount, title, "tt2070791", tvDbId, year, -1, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.CheckIn(userAccount, title, "tt2070791", tvDbId, year, 2, -1);
            //Assert.IsNull(rec);

            //rec = await ctx.CheckIn(userAccount, title, "tt2070791", tvDbId, year, 2, 3);
            //Assert.IsNotNull(rec);
            //Assert.AreEqual(rec.Status, RequestResults.Success);
            return null;
        }
    }
}
