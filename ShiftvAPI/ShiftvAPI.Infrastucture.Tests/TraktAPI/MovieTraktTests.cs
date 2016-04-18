using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Movies.Fakes;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Shows.Fakes;
using ShiftvAPI.Infrastucture.Trakt.Implementation.Movies;
using ShiftvAPI.Infrastucture.Trakt.Implementation.Shows;

namespace ShiftvAPI.Infrastucture.Tests
{
    [TestClass]
    public class MovieTraktTests
    {
        [TestMethod]
        public async Task GetMovie()
        {
            var stub = new StubIMovieTraktQueryService
            {
                GetMoviebyIdInt32 = (s) => Task.Run(() => "https://api.trakt.tv/movies/122917?extended=full,images")
            };
            var ctx = new MovieTraktDataService(stub);
            var a = await ctx.GetMovieById(122917);
            Assert.IsNotNull(a);
        }

        [TestMethod]
        public async Task GetPopular()
        {
            var stub = new StubIMovieTraktQueryService
            {
                GetPopularInt32Int32 = (p, i) => Task.Run(() => "https://api.trakt.tv/movies/popular?page=1&limit=25")
            };
            var ctx = new MovieTraktDataService(stub);
            var a = await ctx.GetPopular(1, 25);
            Assert.IsNotNull(a);
            Assert.AreEqual(25, a.Count);
        }

        [TestMethod]
        public async Task GetTrending()
        {
            var stub = new StubIMovieTraktQueryService
            {
                GetTrendingInt32Int32 = (p, i) => Task.Run(() => "https://api.trakt.tv/movies/trending?page=1&limit=25")
            };
            var ctx = new MovieTraktDataService(stub);
            var a = await ctx.GetTrending(1, 25);
            Assert.IsNotNull(a);
            Assert.AreEqual(25, a.Count);
        }
        [TestMethod]
        public async Task GetUpdates()
        {
            var stub = new StubIMovieTraktQueryService
            {
                GetUpdatesDateTimeInt32Int32 = (d, p, i) => Task.Run(() => "https://api.trakt.tv/movies/updates/2015-01-12?page=1&limit=25")
            };
            var ctx = new MovieTraktDataService(stub);
            var a = await ctx.GetUpdates(1, 25,DateTime.Now);
            Assert.IsNotNull(a);
            Assert.AreEqual(25, a.Count);
        }

        [TestMethod]
        public async Task GetPeople()
        {
            var stub = new StubIMovieTraktQueryService
            {
                GetPeopleInt32 = (i) => Task.Run(() => "https://api.trakt.tv/movies/122917/people?extended=full,images")
            };
            var ctx = new MovieTraktDataService(stub);
            var a = await ctx.GetPeople(122917);
            Assert.IsNotNull(a);
        }

        [TestMethod]
        public async Task GetComments()
        {
            var stub = new StubIMovieTraktQueryService
            {
                GetCommentsInt32Int32Int32 = (i, p, n) => Task.Run(() => "https://api.trakt.tv/movies/122917/comments?extended=full,images&page=1&limit=25")
            };
            var ctx = new MovieTraktDataService(stub);
            var a = await ctx.GetComments(1, 25, 122917);
            Assert.IsNotNull(a);
        }

     
    }
}
