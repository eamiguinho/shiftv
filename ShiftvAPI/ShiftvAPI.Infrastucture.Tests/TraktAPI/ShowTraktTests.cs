using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ShiftvAPI.Contracts.Infrastucture.Trakt.Shows.Fakes;
using ShiftvAPI.Infrastucture.Trakt.Implementation.Shows;

namespace ShiftvAPI.Infrastucture.Tests
{
    [TestClass]
    public class ShowTraktTests
    {
        [TestMethod]
        public async Task GetShowById()
        {
            var stub = new StubIShowTraktQueryService
            {
                GetShowByIdInt32 = (s) => Task.Run(() => "https://api.trakt.tv/shows/161511?extended=full,images")
            };
            var ctx = new ShowTraktDataService(stub);
            var a = await ctx.GetShowById(161511);
            Assert.IsNotNull(a);
        }

        [TestMethod]
        public async Task GetPopular()
        {
            var stub = new StubIShowTraktQueryService
            {
                GetPopularInt32Int32 = (p, i) => Task.Run(() => "https://api.trakt.tv/shows/popular?page=1&limit=25")
            };
            var ctx = new ShowTraktDataService(stub);
            var a = await ctx.GetPopular(1, 25);
            Assert.IsNotNull(a);
            Assert.AreEqual(25, a.Count);
        }

        [TestMethod]
        public async Task GetTrending()
        {
            var stub = new StubIShowTraktQueryService
            {
                GetTrendingInt32Int32 = (p, i) => Task.Run(() => "https://api.trakt.tv/shows/trending?page=1&limit=25")
            };
            var ctx = new ShowTraktDataService(stub);
            var a = await ctx.GetTrending(1, 25);
            Assert.IsNotNull(a);
            Assert.AreEqual(25, a.Count);
        }
        [TestMethod]
        public async Task GetUpdates()
        {
            var stub = new StubIShowTraktQueryService
            {
                GetUpdatesDateTimeInt32Int32 = (d, p, i) => Task.Run(() => "https://api.trakt.tv/shows/updates/2015-01-12?page=1&limit=25")
            };
            var ctx = new ShowTraktDataService(stub);
            var a = await ctx.GetUpdates(1, 25,DateTime.Now);
            Assert.IsNotNull(a);
            Assert.AreEqual(25, a.Count);
        }

        [TestMethod]
        public async Task GetPeople()
        {
            var stub = new StubIShowTraktQueryService
            {
                GetPeopleInt32 = (i) => Task.Run(() => "https://api.trakt.tv/shows/161511/people?extended=full,images")
            };
            var ctx = new ShowTraktDataService(stub);
            var a = await ctx.GetPeople(161511);
            Assert.IsNotNull(a);
        }

        [TestMethod]
        public async Task GetComments()
        {
            var stub = new StubIShowTraktQueryService
            {
                GetCommentsInt32Int32Int32 = (i, p, n) => Task.Run(() => "https://api.trakt.tv/shows/161511/comments?extended=full,images&page=1&limit=25")
            };
            var ctx = new ShowTraktDataService(stub);
            var a = await ctx.GetComments(1, 25, 161511);
            Assert.IsNotNull(a);
            Assert.AreEqual(25, a.Count);
        }

        [TestMethod]
        public async Task GetSeasons()
        {
            var stub = new StubIShowTraktQueryService
            {
                GetSeasonsInt32 = (i) => Task.Run(() => "https://api.trakt.tv/shows/161511/seasons?extended=full,images")
            };
            var ctx = new ShowTraktDataService(stub);
            var a = await ctx.GetSeasons(161511);
            Assert.IsNotNull(a);
        }


        [TestMethod]
        public async Task GetEpisodes()
        {
            var stub = new StubIShowTraktQueryService
            {
                GetEpisodesBySeasonInt32Int32 = (i, s) => Task.Run(() => "https://api.trakt.tv/shows/161511/seasons/1?extended=full,images")
            };
            var ctx = new ShowTraktDataService(stub);
            var a = await ctx.GetEpisodesBySeason(161511, 1);
            Assert.IsNotNull(a);
        }
    }
}
