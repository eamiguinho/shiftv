using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices.Episodes.Fakes;
using Shiftv.Contracts.DataServices.Stats.Fakes;
using Shiftv.Core.Models;
using Shiftv.Infrastucture.Trakt.Implementation.Episodes;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;
using Shiftv.Infrastucture.Trakt.Implementation.Stats;

namespace Shiftv.Infrastucture.Trakt.Test.Stats
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class StatisticsTraktDatServiceTest
    {

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            CoreModelsIoc.RegisterTypes();
        }
        [TestMethod]
        public async Task StatisticsTraktDatServiceTest_GetShowStats_QueryString()
        {
            var ctx = new StatisticsTraktDataService(null);
            var x = await ctx.GetShowStats(-1);
            Assert.IsNull(x);

            var stub = new StubIStatisticsTraktQueryService
            {
                GetShowStatsInt32 = (a) => Task.Run(() => null as string)
            };

            ctx = new StatisticsTraktDataService(stub);
            x = await ctx.GetShowStats(1);
            Assert.IsNull(x);

            stub = new StubIStatisticsTraktQueryService
            {
                GetShowStatsInt32 = (a) => Task.Run(() => "adgasdasfasda")
            };
            ctx = new StatisticsTraktDataService(stub);
            x = await ctx.GetShowStats(1);
            Assert.IsNull(x);
        }

        [TestMethod]
        public async Task StatisticsTraktDatServiceTest_GetShowStats_DataTest()
        {
            var url = "http://api.trakt.tv/show/stats.json/" + TraktConstants.TraktKey + "/258823/";
            var ctx = new StatisticsTraktDataService(null);
            var x = await ctx.GetShowStats(-1);
            Assert.IsNull(x);

            var stub = new StubIStatisticsTraktQueryService
            {
                GetShowStatsInt32 = (a) => Task.Run(() => url)
            };

            ctx = new StatisticsTraktDataService(stub);
            x = await ctx.GetShowStats(-1);
            Assert.IsNull(x);

            ctx = new StatisticsTraktDataService(stub);
            x = await ctx.GetShowStats(258823);
            Assert.IsNotNull(x);
        }

        [TestMethod]
        public async Task StatisticsTraktDatServiceTest_GetEpisodeStats_QueryString()
        {
            var ctx = new StatisticsTraktDataService(null);
            var x = await ctx.GetEpisodeStats(123123, 1, 1);
            Assert.IsNull(x);

            var stub = new StubIStatisticsTraktQueryService
            {
                GetEpisodeStatsInt32Int32Int32 = (a, b, c) => Task.Run(() => null as string)
            };

            var u = new UserTokenDto();
            u.AccessToken = "ttt";

            ctx = new StatisticsTraktDataService(stub);
            x = await ctx.GetEpisodeStats(123123, 1, 1);
            Assert.IsNull(x);

            stub = new StubIStatisticsTraktQueryService
            {
                GetEpisodeStatsInt32Int32Int32 = (a, b, c) => Task.Run(() => "asdfasdasfasd")
            };
            ctx = new StatisticsTraktDataService(stub);
            x = await ctx.GetEpisodeStats(123123, 1, 1);
            Assert.IsNull(x);
        }

        [TestMethod]
        public async Task StatisticsTraktDatServiceTest_GetEpisodeStats_DataTest()
        {
            var url = "http://api.trakt.tv/show/episode/stats.json/" + TraktConstants.TraktKey + "/258823/1/1/";
            var ctx = new StatisticsTraktDataService(null);
            var x = await ctx.GetEpisodeStats(258823, -1, -1);
            Assert.IsNull(x);

            var stub = new StubIStatisticsTraktQueryService
            {
                GetEpisodeStatsInt32Int32Int32 = (a, b, c) => Task.Run(() => url)
            };

            ctx = new StatisticsTraktDataService(stub);
            x = await ctx.GetEpisodeStats(258823, -1, -1);
            Assert.IsNull(x);

            ctx = new StatisticsTraktDataService(stub);
            x = await ctx.GetEpisodeStats(258823, 1, 1);
            Assert.IsNotNull(x);
        }

        [TestMethod]
        public async Task StatisticsTraktDatServiceTest_GetMovieStats_QueryString()
        {
            var ctx = new StatisticsTraktDataService(null);
            var x = await ctx.GetShowStats(-1);
            Assert.IsNull(x);

            var stub = new StubIStatisticsTraktQueryService
            {
                GetMovieStatsString = (a) => Task.Run(() => null as string)
            };

            ctx = new StatisticsTraktDataService(stub);
            x = await ctx.GetMovieStats("tt2294629");
            Assert.IsNull(x);

            stub = new StubIStatisticsTraktQueryService
            {
                GetMovieStatsString = (a) => Task.Run(() => "adgasdasfasda")
            };
            ctx = new StatisticsTraktDataService(stub);
            x = await ctx.GetMovieStats("tt2294629");
            Assert.IsNull(x);
        }

        [TestMethod]
        public async Task StatisticsTraktDatServiceTest_GetMovieStats_DataTest()
        {
            var url = "http://api.trakt.tv/movie/stats.json/" + TraktConstants.TraktKey + "/tt2294629/";
            var ctx = new StatisticsTraktDataService(null);
            var x = await ctx.GetMovieStats(null);
            Assert.IsNull(x);

            var stub = new StubIStatisticsTraktQueryService
            {
                GetMovieStatsString = (a) => Task.Run(() => url)
            };

            ctx = new StatisticsTraktDataService(stub);
            x = await ctx.GetMovieStats(null);
            Assert.IsNull(x);

            ctx = new StatisticsTraktDataService(stub);
            x = await ctx.GetMovieStats("tt2294629");
            Assert.IsNotNull(x);
        }
    }
}
