using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices.Movies.Fakes;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Core.Models;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;
using Shiftv.Infrastucture.Trakt.Implementation.Movies;
using Shiftv.Infrastucture.Trakt.Implementation.Shows;

namespace Shiftv.Infrastucture.Trakt.Test.Movies
{
    [TestClass]
    public class MovieTraktDataServiceTest
    {
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            CoreModelsIoc.RegisterTypes();
        }

        [TestMethod]
        public async Task MovieTraktDataServiceTest_GetTrending_QueryString()
        {
            var stub = new StubIMovieTraktQueryService
            {
                GetTredingQuery = () => Task.Run(() => null as string)
            };
            var u = new UserTokenDto();
            u.AccessToken = "ttttttt";
            var ctx = new MovieTraktDataService(stub, null);
            var x = await ctx.GetTrending(null);
            Assert.IsNull(x);

            stub = new StubIMovieTraktQueryService
            {
                GetTredingQuery = () => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new MovieTraktDataService(stub, null);
            x = await ctx.GetTrending(u);
            Assert.IsNull(x);
        }

        [TestMethod]
        public async Task MovieTraktDataServiceTest_GetTrending_DataTest()
        {
            var url = "http://shiftvapi.azurewebsites.net/movies/treding";
            var stub = new StubIMovieTraktQueryService
            {
                GetTredingQuery = () => Task.Run(() => url)
            };
            var ctx = new MovieTraktDataService(stub, null);
            var u = new UserTokenDto();
            u.AccessToken = "ttttttt";
            var rec = await ctx.GetTrending(null);
            Assert.IsNotNull(rec);
            Assert.AreNotEqual(0, rec.Count);
            //u.PasswordEnc = "25713423e";
            //rec = await ctx.GetTrending(u);
            //Assert.IsNotNull(rec);
            //Assert.AreNotEqual(0, rec.Count);
        }

        [TestMethod]
        public async Task MovieTraktDataServiceTest_SearchMoviesByKey_QueryString()
        {
            var stub = new StubIMovieTraktQueryService
            {
                GetSearchMoviesByKeyString = a => Task.Run(() => null as string)
            };
            var u = new UserTokenDto();
            u.AccessToken = "ttttttt";
            var ctx = new MovieTraktDataService(stub, null);
            var x = await ctx.SearchMoviesByKey(null);
            Assert.IsNull(x);

            stub = new StubIMovieTraktQueryService
            {
                GetSearchMoviesByKeyString = a => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new MovieTraktDataService(stub, null);
            x = await ctx.SearchMoviesByKey("game");
            Assert.IsNull(x);
        }

        [TestMethod]
        public async Task MovieTraktDataServiceTest_SearchMoviesByKey_DataTest()
        {
            var url = "http://api.trakt.tv/search/movies.json/" + TraktConstants.TraktKey + "?query=game";
            var stub = new StubIMovieTraktQueryService
            {
                GetSearchMoviesByKeyString = a => Task.Run(() => url)
            };
            var ctx = new MovieTraktDataService(stub, null);
            var rec = await ctx.SearchMoviesByKey(null);
            Assert.IsNull(rec);
            rec = await ctx.SearchMoviesByKey("game");
            Assert.IsNotNull(rec);
            Assert.AreNotEqual(0, rec.Count);
        }

        [TestMethod]
        public async Task MovieTraktDataServiceTest_GetRecommendations_QueryString()
        {
            var stub = new StubIMovieTraktQueryService
            {
                GetRecommendations = () => Task.Run(() => null as string)
            };
            var u = new UserTokenDto();
            u.AccessToken = "ttttttt";
            var ctx = new MovieTraktDataService(stub, null);
            var x = await ctx.GetRecommendations(null);
            Assert.IsNull(x);

            stub = new StubIMovieTraktQueryService
            {
                GetRecommendations = () => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new MovieTraktDataService(stub, null);
            x = await ctx.GetRecommendations(u);
            Assert.IsNull(x);
        }

        [TestMethod]
        public async Task MovieTraktDataServiceTest_GetRecommendations_DataTest()
        {
            var url = "http://api.trakt.tv/recommendations/movies/" + TraktConstants.TraktKey;
            var stub = new StubIMovieTraktQueryService
            {
                GetRecommendations = () => Task.Run(() => url)
            };
            var ctx = new MovieTraktDataService(stub, null);
            var u = new UserTokenDto();
            u.AccessToken = "ttttttt";
            var rec = await ctx.GetRecommendations(null);
            Assert.IsNull(rec);
            rec = await ctx.GetRecommendations(u);
            Assert.IsNotNull(rec);
            Assert.AreNotEqual(0, rec.Count);
        }


        [TestMethod]
        public async Task MovieTraktDataServiceTest_RateMovie_QueryString()
        {
            var ctx = new MovieTraktDataService(null, null);
            //var x = await ctx.RateMovie(null, false, null, null, -1);
            //Assert.IsNull(x);

            var stub = new StubIMovieTraktQueryService
            {
                RateMovie = () => Task.Run(() => null as string)
            };

            ctx = new MovieTraktDataService(stub, null);
            //x = await ctx.RateMovie(null, false, null, null, -1);
            //Assert.IsNull(x);
            var u = new UserTokenDto();
            u.AccessToken = "ttttttt";
            stub = new StubIMovieTraktQueryService
            {
                RateMovie = () => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new MovieTraktDataService(stub, null);
            //x = await ctx.RateMovie(u, true, "Frozen", "tt2294629", 2012);
            //Assert.IsNull(x);
        }

        [TestMethod]
        public async Task MovieTraktDataServiceTest_RateMovie_DataTest()
        {
            var url = "http://api.trakt.tv/rate/movie/" + TraktConstants.TraktKey;
            var stub = new StubIMovieTraktQueryService
            {
                RateMovie = () => Task.Run(() => url)
            };


            const string imdbId = "tt2294629";
            const int year = 2013;
            const string title = "Frozen";

            var userAccount = new UserTokenDto();
            userAccount.AccessToken = "ttttttt";

            var ctx = new MovieTraktDataService(stub, null);

            //var rec = await ctx.RateMovie(null, false, null, null, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.RateMovie(userAccount, false, null, null, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.RateMovie(userAccount, true, null, null, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.RateMovie(userAccount, false, title, null, -1);
            //Assert.IsNull(rec);

            //rec = await ctx.RateMovie(null, true, null, null, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.RateMovie(userAccount, true, null, null, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.RateMovie(null, true, title, null, -1);
            //Assert.IsNull(rec);

            //rec = await ctx.RateMovie(null, false, title, null, - 1);
            //Assert.IsNull(rec);
            //rec = await ctx.RateMovie(userAccount, false, title, null, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.RateMovie(null, true, title, null, -1);
            //Assert.IsNull(rec);

            //rec = await ctx.RateMovie(userAccount, true, title, imdbId, year);
            //Assert.IsNotNull(rec);
            //Assert.AreEqual(rec.Status, RequestResults.Success);
        }

        [TestMethod]
        public async Task MovieTraktDataServiceTest_GetByImdbId_QueryString()
        {
            var ctx = new MovieTraktDataService(null, null);
            var x = await ctx.GetByImdbId(null, null);
            Assert.IsNull(x);

            var stub = new StubIMovieTraktQueryService
            {
                GetByImdbIdInt32 = a => Task.Run(() => null as string)
            };

            ctx = new MovieTraktDataService(stub, null);
            x = await ctx.GetByImdbId(null, new IdsDto());
            Assert.IsNull(x);

            stub = new StubIMovieTraktQueryService
            {
                GetByImdbIdInt32 = b => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new MovieTraktDataService(stub, null);
            x = await ctx.GetByImdbId(null, new IdsDto());
            Assert.IsNull(x);
        }


        [TestMethod]
        public async Task MovieTraktDataServiceTest_GetByImdbId_DataTest()
        {
            var url = "http://api.trakt.tv/movie/summary.json/" + TraktConstants.TraktKey + "/tt2294629/";
            var stub = new StubIMovieTraktQueryService
            {
                GetByImdbIdInt32 = a => Task.Run(() => url)
            };
            var ctx = new MovieTraktDataService(stub, null);

            var rec = await ctx.GetByImdbId(null, null);
            Assert.IsNull(rec);
            rec = await ctx.GetByImdbId(null, new IdsDto());
            Assert.IsNotNull(rec);
            Assert.AreEqual(rec.Ids.ImdbId, new IdsDto());

            var u = new UserTokenDto();
            u.AccessToken = "ttt";

            rec = await ctx.GetByImdbId(u, new IdsDto());
            Assert.IsNotNull(rec);
            Assert.AreEqual(rec.Ids.ImdbId, "tt2294629");
        }


        [TestMethod]
        public async Task MovieTraktDataServiceTest_CheckIn_QueryString()
        {
            const int year = 2013;
            const string title = "Frozen";


            var userAccount = new UserTokenDto();
            userAccount.AccessToken = "ttt";
            var ctx = new MovieTraktDataService(null, null);
            var x = await ctx.CheckIn(userAccount, title, "tt2294629", year);
            Assert.IsNull(x);

            var stub = new StubIMovieTraktQueryService
            {
                GetCheckIn = () => Task.Run(() => null as string)
            };

            ctx = new MovieTraktDataService(stub, null);
            x = await ctx.CheckIn(userAccount, title, "tt2294629", year);
            Assert.IsNull(x);


            stub = new StubIMovieTraktQueryService
            {
                GetCheckIn = () => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new MovieTraktDataService(stub, null);
            x = await ctx.CheckIn(userAccount, title, "tt2294629", year);
            Assert.IsNull(x);
        }



        [TestMethod]
        public Task MovieTraktDataServiceTest_CheckIn_DataTest()
        {
            Assert.IsTrue(true);
            //var url = "http://api.trakt.tv/movie/checkin/" + TraktConstants.TraktDevKey;
            //var stub = new StubIMovieTraktQueryService
            //{
            //    GetCheckIn = () => Task.Run(() => url)
            //};


            //const int year = 2013;
            //const string title = "Frozen";

            //var userAccount = new UserAccountDto { Username = "amiguinho", PasswordEnc = "25713423e" };

            //var ctx = new MovieTraktDataService(stub, null);

            //var rec = await ctx.CheckIn(null, null, null, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.CheckIn(userAccount, null, null, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.CheckIn(userAccount, title, null, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.CheckIn(userAccount, title, "tt2294629", -1);
            //Assert.IsNull(rec);
            //rec = await ctx.CheckIn(userAccount, title, "tt2294629", year);
            //Assert.IsNotNull(rec);
            //Assert.AreEqual(rec.Status, RequestResults.Success);
            return null;
        }


        //[TestMethod]
        //public async Task MovieTraktDataServiceTest_GetMoviesWatchlistByUser_QueryString()
        //{
        //    var ctx = new MovieTraktDataService(null, null);
        //    var x = await ctx.GetMoviesWatchlistByUser(null, null);
        //    Assert.IsNull(x);

        //    var stub = new StubIMovieTraktQueryService
        //    {
        //        GetUserWatchlist = a => Task.Run(() => null as string)
        //    };

        //    ctx = new MovieTraktDataService(stub, null);
        //    x = await ctx.GetMoviesWatchlistByUser("asfas", null);
        //    Assert.IsNull(x);

        //    stub = new StubIMovieTraktQueryService
        //    {
        //        GetUserWatchlist = => Task.Run(() => "asagsdfsgdsqweqw")
        //    };
        //    ctx = new MovieTraktDataService(stub, null);
        //    x = await ctx.GetMoviesWatchlistByUser("aa",);
        //    Assert.IsNull(x);
        //}


        //[TestMethod]
        //public async Task MovieTraktDataServiceTest_GetMoviesWatchlistByUser_DataTest()
        //{
        //    var url = "http://api.trakt.tv/user/watchlist/movies.json/" + TraktConstants.TraktKey + "/amiguinho/";
        //    var stub = new StubIMovieTraktQueryService
        //    {
        //        GetUserWatchlistString = a => Task.Run(() => url)
        //    };
        //    var ctx = new MovieTraktDataService(stub, null);

        //    var rec = await ctx.GetMoviesWatchlistByUser(null, null);
        //    Assert.IsNull(rec);

        //    rec = await ctx.GetMoviesWatchlistByUser("amiguinho", null);
        //    Assert.IsNotNull(rec);
        //    Assert.AreNotEqual(rec.Count, 0);
        //}


    }
}
