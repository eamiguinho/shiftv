using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices.Shows.Fakes;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Core.Models;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;
using Shiftv.Infrastucture.Trakt.Implementation.Shows;

namespace Shiftv.Infrastucture.Trakt.Test.Shows
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ShowTraktDataServiceTest
    {
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            CoreModelsIoc.RegisterTypes();
        }

        [TestMethod]
        public async Task ShowTraktDataServiceTest_GetTrending_QueryString()
        {
            var stub = new StubIShowTraktQueryService
            {
                GetTrending = () => Task.Run(() => null as string)
            };
            var u = new UserTokenDto();
            u.AccessToken = "ttt";
            var ctx = new ShowTraktDataService(stub, null);
            var x = await ctx.GetRecommendationsByUser(u);
            Assert.IsNull(x);

            stub = new StubIShowTraktQueryService
            {
                GetTrending = () => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new ShowTraktDataService(stub, null);
            x = await ctx.GetRecommendationsByUser(u);
            Assert.IsNull(x);
        }
        [TestMethod]
        public async Task ShowTraktDataServiceTest_GetTrending_DataTest()
        {
            var url = "http://shiftvapi.azurewebsites.net/shows/trending";
            var stub = new StubIShowTraktQueryService
            {
                GetTrending = () => Task.Run(() => url)
            };
            var ctx = new ShowTraktDataService(stub, null);
            var u = new UserTokenDto();
            u.AccessToken = "ttt";
            var rec = await ctx.GetTrending(null);
            Assert.IsNotNull(rec);
            Assert.AreNotEqual(0, rec.Count);
            //u.PasswordEnc = "25713423e";
            //rec = await ctx.GetTrending(u);
            //Assert.IsNotNull(rec);
            //Assert.AreNotEqual(0, rec.Count);
        }

        [TestMethod]
        public async Task ShowTraktDataServiceTest_GetRecommendations_QueryString()
        {
            var stub = new StubIShowTraktQueryService
            {
                GetRecommendations = () => Task.Run(() => null as string)
            };
            var u = new UserTokenDto();
            u.AccessToken = "ttt";
            var ctx = new ShowTraktDataService(stub, null);
            var x = await ctx.GetRecommendationsByUser(u);
            Assert.IsNull(x);

            stub = new StubIShowTraktQueryService
            {
                GetRecommendations = () => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new ShowTraktDataService(stub, null);
            x = await ctx.GetRecommendationsByUser(u);
            Assert.IsNull(x);
        }
        [TestMethod]
        public async Task ShowTraktDataServiceTest_GetRecommendations_DataTest()
        {
            var url = "http://api.trakt.tv/recommendations/shows/" + TraktConstants.TraktKey;
            var stub = new StubIShowTraktQueryService
            {
                GetRecommendations = () => Task.Run(() => url)
            };
            var ctx = new ShowTraktDataService(stub, null);

            var rec = await ctx.GetRecommendationsByUser(null);
            Assert.IsNull(rec);
            var u = new UserTokenDto();
            u.AccessToken = "ttt";
            rec = await ctx.GetRecommendationsByUser(u);
            Assert.IsNull(rec);
           // u.PasswordEnc = "25713423e";
            rec = await ctx.GetRecommendationsByUser(u);
            Assert.AreNotEqual(rec.Count, 0);
        }
        [TestMethod]
        public async Task ShowTraktDataServiceTest_SearchShowsByKey_QueryString()
        {
            var stub = new StubIShowTraktQueryService
            {
                GetSearchByKeyString = a => Task.Run(() => null as string)
            };
            var ctx = new ShowTraktDataService(stub, null);
            var x = await ctx.SearchShowsByKey("aa");
            Assert.IsNull(x);

            stub = new StubIShowTraktQueryService
            {
                GetSearchByKeyString = b => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new ShowTraktDataService(stub, null);
            x = await ctx.SearchShowsByKey("aa");
            Assert.IsNull(x);
        }

        [TestMethod]
        public async Task ShowTraktDataServiceTest_SearchShowsByKey_DataTest()
        {
            var url = "http://api.trakt.tv/search/shows.json/" + TraktConstants.TraktKey + "?query=American";
            var stub = new StubIShowTraktQueryService
            {
                GetSearchByKeyString = a => Task.Run(() => url)
            };
            var ctx = new ShowTraktDataService(stub, null);

            var rec = await ctx.SearchShowsByKey(null);
            Assert.IsNull(rec);
            rec = await ctx.SearchShowsByKey("test");
            Assert.IsNotNull(rec);
            Assert.AreNotEqual(rec.Count, 0);
        }
        [TestMethod]
        public async Task ShowTraktDataServiceTest_GetByImdbId_QueryString()
        {
            var ctx = new ShowTraktDataService(null, null);
            var x = await ctx.GetByImdbId(null, null);
            Assert.IsNull(x);

            var stub = new StubIShowTraktQueryService
            {
                GetByImdbIdInt32= a => Task.Run(() => null as string)
            };

            ctx = new ShowTraktDataService(stub, null);
            x = await ctx.GetByImdbId(null, new IdsDto());
            Assert.IsNull(x);

            stub = new StubIShowTraktQueryService
            {
                GetByImdbIdInt32 = b => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new ShowTraktDataService(stub, null);
            x = await ctx.GetByImdbId(null, new IdsDto());
            Assert.IsNull(x);
        }


        [TestMethod]
        public async Task ShowTraktDataServiceTest_GetByImdbId_DataTest()
        {
            var url = "http://shiftvapi.azurewebsites.net/shows/tt1119644/";
            var stub = new StubIShowTraktQueryService
            {
                GetByImdbIdInt32 = a => Task.Run(() => url)
            };
            var ctx = new ShowTraktDataService(stub, null);

            var rec = await ctx.GetByImdbId(null, null);
            Assert.IsNull(rec);
            rec = await ctx.GetByImdbId(null, new IdsDto());
            Assert.IsNotNull(rec);
            Assert.AreNotEqual(rec.Ids.TvDbId, new IdsDto());

            //var u = new UserAccountDto();
            //u.Username = "amiguinho";
            //u.PasswordEnc = "25713423e";

            //rec = await ctx.GetByImdbId(u, "tt1119644");
            //Assert.IsNotNull(rec);
            //Assert.AreNotEqual(rec.Ids.TvDbId, "tt1119644");
        }



        [TestMethod]
        public async Task ShowTraktDataServiceTest_RateShow_QueryString()
        {
            var ctx = new ShowTraktDataService(null, null);
           // var x = await ctx.RateShow(null, false, null, -1, -1);
          //  Assert.IsNull(x);

            var stub = new StubIShowTraktQueryService
            {
                RateShow = () => Task.Run(() => null as string)
            };

            ctx = new ShowTraktDataService(stub, null);
            //x = await ctx.RateShow(null, false, null, -1, -1);
         //   Assert.IsNull(x);
            var u = new UserTokenDto();
            u.AccessToken = "ttt";
            stub = new StubIShowTraktQueryService
            {
                RateShow = () => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new ShowTraktDataService(stub, null);
          //  x = await ctx.RateShow(u, true, "Revolution", 258823, 2012);
         //   Assert.IsNull(x);
        }

        [TestMethod]
        public async Task ShowTraktDataServiceTest_RateShow_DataTest()
        {
            var url = "http://api.trakt.tv/rate/show/" + TraktConstants.TraktKey;
            var stub = new StubIShowTraktQueryService
            {
                RateShow = () => Task.Run(() => url)
            };

          
            const int tvDbId = 258823;
            const int year = 2012;
            const string title = "Revolution";

            var userAccount = new UserTokenDto();
            userAccount.AccessToken = "ttt";

            var ctx = new ShowTraktDataService(stub, null);

            //var rec = await ctx.RateShow(null, false, null, -1, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.RateShow(userAccount, false, null, -1, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.RateShow(userAccount, true, null, -1, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.RateShow(userAccount, false, title, -1, -1);
            //Assert.IsNull(rec);

            //rec = await ctx.RateShow(null, true, null, -1, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.RateShow(userAccount, true, null, -1, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.RateShow(null, true, title, -1, -1);
            //Assert.IsNull(rec);

            //rec = await ctx.RateShow(null, false, title, -1, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.RateShow(userAccount, false, title, -1, -1);
            //Assert.IsNull(rec);
            //rec = await ctx.RateShow(null, true, title, -1, -1);
            //Assert.IsNull(rec);

            //rec = await ctx.RateShow(userAccount, true, title, tvDbId, year);
            //Assert.IsNotNull(rec);
            //Assert.AreEqual(rec.Status, RequestResults.Success);
        }

        [TestMethod]
        public async Task ShowTraktDataServiceTest_AddToWatchList_QueryString()
        {
            var ctx = new ShowTraktDataService(null, null);
            var x = await ctx.AddShowToWatchlist(null, -1, null, -1);
            Assert.IsNull(x);

            var stub = new StubIShowTraktQueryService
            {
                AddShowToWatchList = () => Task.Run(() => null as string)
            };

            ctx = new ShowTraktDataService(stub, null);
            x = await ctx.AddShowToWatchlist(null, -1, null, -1);
            Assert.IsNull(x);
            var u = new UserTokenDto();
            u.AccessToken = "ttt";
            stub = new StubIShowTraktQueryService
            {
                AddShowToWatchList = () => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new ShowTraktDataService(stub, null);
            x = await ctx.AddShowToWatchlist(u, 258823, "Revolution", 2012);
            Assert.IsNull(x);
        }
    

         [TestMethod]
        public async Task ShowTraktDataServiceTest_AddToWatchList_DataTest()
        {
            var url = "http://api.trakt.tv/show/watchlist/" + TraktConstants.TraktKey;
            var stub = new StubIShowTraktQueryService
            {
                AddShowToWatchList = () => Task.Run(() => url)
            };  


            var tvDbId = 80348;
            var year = 2007;
            var title = "Chuck";

            var u = new UserTokenDto();
            u.AccessToken = "ttt";

            var ctx = new ShowTraktDataService(stub, null);

            var rec = await ctx.AddShowToWatchlist(null, -1, null, -1);
            Assert.IsNull(rec);
            rec = await ctx.AddShowToWatchlist(u, -1, null, -1);
            Assert.IsNull(rec);
            rec = await ctx.AddShowToWatchlist(u, 5, null, -1);
            Assert.IsNull(rec);
            rec = await ctx.AddShowToWatchlist(u, -1, title, -1);
            Assert.IsNull(rec);

            rec = await ctx.AddShowToWatchlist(null, 10, null, -1);
            Assert.IsNull(rec);
            rec = await ctx.AddShowToWatchlist(u, 10, null, -1);
            Assert.IsNull(rec);
            rec = await ctx.AddShowToWatchlist(null, 10, title, -1);
            Assert.IsNull(rec);

            rec = await ctx.AddShowToWatchlist(null, -1, title, -1);
            Assert.IsNull(rec);
            rec = await ctx.AddShowToWatchlist(u, -1, title, -1);
            Assert.IsNull(rec);
            rec = await ctx.AddShowToWatchlist(null, 10, title, -1);
            Assert.IsNull(rec);

            rec = await ctx.AddShowToWatchlist(u, tvDbId, title, year);
            Assert.IsNotNull(rec);
            Assert.AreEqual(rec.Status, RequestResults.Success);
        }


         [TestMethod]
         public async Task ShowTraktDataServiceTest_RemoveFromWatchList_QueryString()
         {
             var ctx = new ShowTraktDataService(null, null);
             var x = await ctx.RemoveShowFromWatchlist(null, -1, null, -1);
             Assert.IsNull(x);

             var stub = new StubIShowTraktQueryService
             {
                 RemoveShowFromWatchList = () => Task.Run(() => null as string)
             };

             ctx = new ShowTraktDataService(stub, null);
             x = await ctx.AddShowToWatchlist(null, -1, null, -1);
             Assert.IsNull(x);
             var u = new UserTokenDto();
             u.AccessToken = "ttt";
             stub = new StubIShowTraktQueryService
             {
                 RemoveShowFromWatchList = () => Task.Run(() => "asagsdfsgdsqweqw")
             };
             ctx = new ShowTraktDataService(stub, null);
             x = await ctx.RemoveShowFromWatchlist(u, 258823, "Revolution", 2012);
             Assert.IsNull(x);
         }

        [TestMethod]
         public async Task ShowTraktDataServiceTest_RemoveFromWatchList_DataTest()
         {
             var url = "http://api.trakt.tv/show/unwatchlist/" + TraktConstants.TraktKey;
             var stub = new StubIShowTraktQueryService
             {
                 RemoveShowFromWatchList = () => Task.Run(() => url)
             };


             var tvDbId = 80348;
             var year = 2007;
             var title = "Chuck";

             var u = new UserTokenDto();
             u.AccessToken = "ttt";

             var ctx = new ShowTraktDataService(stub, null);

             var rec = await ctx.RemoveShowFromWatchlist(null, -1, null, -1);
             Assert.IsNull(rec);
             rec = await ctx.RemoveShowFromWatchlist(u, -1, null, -1);
             Assert.IsNull(rec);
             rec = await ctx.RemoveShowFromWatchlist(u, 5, null, -1);
             Assert.IsNull(rec);
             rec = await ctx.RemoveShowFromWatchlist(u, -1, title, -1);
             Assert.IsNull(rec);

             rec = await ctx.RemoveShowFromWatchlist(null, 10, null, -1);
             Assert.IsNull(rec);
             rec = await ctx.RemoveShowFromWatchlist(u, 10, null, -1);
             Assert.IsNull(rec);
             rec = await ctx.RemoveShowFromWatchlist(null, 10, title, -1);
             Assert.IsNull(rec);

             rec = await ctx.RemoveShowFromWatchlist(null, -1, title, -1);
             Assert.IsNull(rec);
             rec = await ctx.RemoveShowFromWatchlist(u, -1, title, -1);
             Assert.IsNull(rec);
             rec = await ctx.RemoveShowFromWatchlist(null, 10, title, -1);
             Assert.IsNull(rec);

             rec = await ctx.RemoveShowFromWatchlist(u, tvDbId, title, year);
             Assert.IsNotNull(rec);
             Assert.AreEqual(rec.Status, RequestResults.Success);
         }


        [TestMethod]
        public async Task ShowTraktDataServiceTest_GetShowsWatchlistByUser_QueryString()
        {
            var ctx = new ShowTraktDataService(null, null);
            var x = await ctx.GetShowsWatchlistByUser(null);
            Assert.IsNull(x);

            var stub = new StubIShowTraktQueryService
            {
                GetShowsWatchlistByUserString = a => Task.Run(() => null as string)
            };

            ctx = new ShowTraktDataService(stub, null);
            x = await ctx.GetShowsWatchlistByUser(new UserTokenDto());
            Assert.IsNull(x);

            stub = new StubIShowTraktQueryService
            {
                GetShowsWatchlistByUserString = b => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new ShowTraktDataService(stub, null);
            x = await ctx.GetShowsWatchlistByUser(new UserTokenDto());
            Assert.IsNull(x);
        }


        [TestMethod]
        public async Task ShowTraktDataServiceTest_GetShowsWatchlistByUser_DataTest()
        {
            var url = "http://api.trakt.tv/user/watchlist/shows.json/" + TraktConstants.TraktKey + "/amiguinho/";
            var stub = new StubIShowTraktQueryService
            {
                GetShowsWatchlistByUserString = a => Task.Run(() => url)
            };
            var ctx = new ShowTraktDataService(stub, null);

            var rec = await ctx.GetShowsWatchlistByUser(null);
            Assert.IsNull(rec);

            rec = await ctx.GetShowsWatchlistByUser(new UserTokenDto());
            Assert.IsNotNull(rec);
            Assert.AreNotEqual(rec.Count, 0);
        }
        
        [TestMethod]
        public async Task ShowTraktDataServiceTest_GetShowsWithEpisodesWatchlistByUserString_QueryString()
        {
            var ctx = new ShowTraktDataService(null, null);
            var x = await ctx.GetShowsWithEpisodesWatchlistByUser(null);
            Assert.IsNull(x);

            var stub = new StubIShowTraktQueryService
            {
                GetShowsWithEpisodesWatchlistByUserString = a => Task.Run(() => null as string)
            };

            ctx = new ShowTraktDataService(stub, null);
            x = await ctx.GetShowsWithEpisodesWatchlistByUser("asfas");
            Assert.IsNull(x);

            stub = new StubIShowTraktQueryService
            {
                GetShowsWithEpisodesWatchlistByUserString = b => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new ShowTraktDataService(stub, null);
            x = await ctx.GetShowsWithEpisodesWatchlistByUser("aa");
            Assert.IsNull(x);
        }


        [TestMethod]
        public async Task ShowTraktDataServiceTest_GetShowsWithEpisodesWatchlistByUserString_DataTest()
        {
            var url = "http://api.trakt.tv/user/watchlist/episodes.json/" + TraktConstants.TraktKey + "/amiguinho/";
            var stub = new StubIShowTraktQueryService
            {
                GetShowsWithEpisodesWatchlistByUserString = a => Task.Run(() => url)
            };
            var ctx = new ShowTraktDataService(stub, null);

            var rec = await ctx.GetShowsWithEpisodesWatchlistByUser(null);
            Assert.IsNull(rec);

            rec = await ctx.GetShowsWithEpisodesWatchlistByUser("amiguinho");
            Assert.IsNotNull(rec);
            Assert.AreNotEqual(rec.Count, 0);
        }
    }
}
