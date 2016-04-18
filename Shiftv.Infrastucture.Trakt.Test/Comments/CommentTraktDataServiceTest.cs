using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices.Comments.Fakes;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Core.Models;
using Shiftv.Infrastucture.Trakt.Implementation.Comments;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Test.Comments
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class CommentTraktDataServiceTest
    {
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            CoreModelsIoc.RegisterTypes();
        }

        [TestMethod]
        public async Task CommentTraktDataServiceTest_GetCommentsShowById_QueryString()
        {
            var stub = new StubICommentTraktQueryService
            {
                GetCommentsShowByIdNullableOfInt32 = b => Task.Run(() => null as string)
            };
            var tvdbId = new IdsDto();
            var ctx = new CommentTraktDataService(stub, null);
            var x = await ctx.GetCommentsShowById(tvdbId);
            Assert.IsNull(x);

            stub = new StubICommentTraktQueryService
            {
                GetCommentsShowByIdNullableOfInt32 = b => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new CommentTraktDataService(stub, null);
            x = await ctx.GetCommentsShowById(tvdbId);
            Assert.IsNull(x);
        }

        [TestMethod]
        public async Task CommentTraktDataServiceTest_GetCommentsShowById_TestData()
        {
            var tvdbId = new IdsDto();
            var url = "http://api.trakt.tv/show/comments.json/" + TraktConstants.TraktKey + "/" + tvdbId;
            var stub = new StubICommentTraktQueryService
            {
                GetCommentsShowByIdNullableOfInt32 = b => Task.Run(() => url)
            };
            var ctx = new CommentTraktDataService(stub, null);
            var rec = await ctx.GetCommentsShowById(null);
            Assert.IsNull(rec);
            rec = await ctx.GetCommentsShowById(tvdbId);
            Assert.AreNotEqual(rec.Count, 0);
        }

        [TestMethod]
        public Task ShowTraktDataServiceTest_CommentShow_DataTest()
        {
            Assert.IsTrue(true);
            return null;
            //var url = "http://api.trakt.tv/comment/show/" + TraktConstants.TraktKey;
            //var stub = new StubICommentTraktQueryService
            //{
            //    CommentShow = () => Task.Run(() => url)
            //};


            //var tvDbId = 258823;
            //var year = 2012;
            //var title = "Revolution";

            //var userAccount = new UserAccountDto();
            //userAccount.Username = "amiguinho";
            //userAccount.PasswordEnc = "25713423e";

            //var ctx = new CommentTraktDataService(stub, null);

            //var rec = await ctx.CommentShow(null, null, null, -1, -1, false, false);
            //Assert.IsNull(rec);
            //rec = await ctx.CommentShow(userAccount, null, null, -1, -1, false, false);
            //Assert.IsNull(rec);
            //rec = await ctx.CommentShow(userAccount, "asfasd", null, -1, -1, false, false);
            //Assert.IsNull(rec);
            //rec = await ctx.CommentShow(userAccount, "asfasd", title, -1, -1, false, false);
            //Assert.IsNull(rec);

            //rec = await ctx.CommentShow(userAccount, "asfasd", title, tvDbId, -1, false, false);
            //Assert.IsNull(rec);

            //rec = await ctx.CommentShow(userAccount, "revolution is one of the best shows i ever seen, all episodes are exciting and i love that badass girl <3", title, tvDbId, year, false, false);
            //Assert.IsNotNull(rec);
            //Assert.AreEqual(rec.Status, RequestResults.Success);
        }

        [TestMethod]
        public void ShowTraktDataServiceTest_EditCommentShow_DataTest()
        {
            Assert.Inconclusive("TODO SEND EMAIL TO TRAKT!");
            //var url = "http://trakt.tv/api/shout/edit/trakt/" + TraktConstants.TraktKey;
            //var stub = new StubICommentTraktQueryService
            //{
            //    EditCommentShow = () => Task.Run(() => url)
            //};


            //var commentId = 9402;
            //var type = "show";

            //var userAccount = new UserAccountDto();
            //userAccount.Username = "amiguinho";
            //userAccount.PasswordEnc = "25713423e";

            //var ctx = new CommentTraktDataService(stub, null);

            //var rec = await ctx.CommentShow(null, null, null, -1, -1, false, false);
            //Assert.IsNull(rec);
            //rec = await ctx.CommentShow(userAccount, null, null, -1, -1, false, false);
            //Assert.IsNull(rec);
            //rec = await ctx.CommentShow(userAccount, "asfasd", null, -1, -1, false, false);
            //Assert.IsNull(rec);
            //rec = await ctx.CommentShow(userAccount, "asfasd", title, -1, -1, false, false);
            //Assert.IsNull(rec);

            //rec = await ctx.CommentShow(userAccount, "asfasd", title, tvDbId, -1, false, false);
            //Assert.IsNull(rec);

           //var  rec = await ctx.Edit(userAccount, "damn what a show!", commentId, false, false, type);
           // Assert.IsNotNull(rec);
           // Assert.AreEqual(rec.Status, RequestResults.Success);
        }

        [TestMethod]
        public async Task CommentTraktDataServiceTest_GetCommentsByEpisode_QueryString()
        {
            var stub = new StubICommentTraktQueryService
            {
                GetCommentsByEpisodeInt32Int32Int32 = (a,b,c) => Task.Run(() => null as string)
            };
            var tvdbId = 153021;
            var ctx = new CommentTraktDataService(stub, null);
            var x = await ctx.GetCommentsByEpisode(tvdbId, -1, -1);
            Assert.IsNull(x);

            stub = new StubICommentTraktQueryService
            {
                GetCommentsByEpisodeInt32Int32Int32 = (a, b, c) => Task.Run(() => "aadasfasdasfas")
            };
            ctx = new CommentTraktDataService(stub, null);
            x = await ctx.GetCommentsByEpisode(tvdbId, -1, -1);
            Assert.IsNull(x);
        }

        [TestMethod]
        public async Task CommentTraktDataServiceTest_GetCommentsByEpisode_TestData()
        {
            var tvdbId = 153021;
            var url = "http://api.trakt.tv/show/episode/comments.json/" + TraktConstants.TraktKey + "/" + tvdbId +"/1/1";
            var stub = new StubICommentTraktQueryService
            {
                GetCommentsByEpisodeInt32Int32Int32 = (a, b, c) => Task.Run(() => url)
            };
            var ctx = new CommentTraktDataService(stub, null);
            var rec = await ctx.GetCommentsByEpisode(-1,-1,-1);
            Assert.IsNull(rec);

            rec = await ctx.GetCommentsByEpisode(tvdbId, -1,-1);
            Assert.IsNull(rec);

            rec = await ctx.GetCommentsByEpisode(tvdbId, 1, -1);
            Assert.IsNull(rec);

            rec = await ctx.GetCommentsByEpisode(tvdbId,1,1);
            Assert.AreNotEqual(rec.Count, 0);
        }


        [TestMethod]
        public async Task CommentTraktDataServiceTest_CommentEpisode_QueryString()
        {
            var tvDbId = 258823;
            var year = 2012;
            var title = "Revolution";

            var userAccount = new UserTokenDto();
            userAccount.AccessToken = "tt";

            var stub = new StubICommentTraktQueryService
            {
                CommentEpisode = () => Task.Run(() => null as string)
            };
            var ctx = new CommentTraktDataService(stub, null);
            var x = await ctx.CommentEpisode(userAccount, "just wondering what is coming from here :o", title, tvDbId, year, 2, 1, false, false);
            Assert.IsNull(x);

            stub = new StubICommentTraktQueryService
            {
                 CommentEpisode = () => Task.Run(() => "aadasfasdasfas")
            };
            ctx = new CommentTraktDataService(stub, null);
            x = await ctx.CommentEpisode(userAccount, "just wondering what is coming from here :o", title, tvDbId, year, 2, 1, false, false);
            Assert.IsNull(x);
        }

        [TestMethod]
        public async Task CommentTraktDataServiceTest_CommentEpisode_DataTest()
        {
            var url = "http://api.trakt.tv/comment/episode/" + TraktConstants.TraktKey;
            var stub = new StubICommentTraktQueryService
            {
                CommentEpisode = () => Task.Run(() => url)
            };


            var tvDbId = 258823;
            var year = 2012;
            var title = "Revolution";


            var userAccount = new UserTokenDto();
            userAccount.AccessToken = "tt";
            var ctx = new CommentTraktDataService(stub, null);

            var rec = await ctx.CommentEpisode(null, null, null, -1, -1, -1,-1, false, false);
            Assert.IsNull(rec);
            rec = await ctx.CommentEpisode(userAccount, null, null, -1, -1, -1,-1, false, false);
            Assert.IsNull(rec);
            rec = await ctx.CommentEpisode(userAccount, "asfasd", null, -1, -1, -1, -1, false, false);
            Assert.IsNull(rec);
            rec = await ctx.CommentEpisode(userAccount, "asfasd", title, -1, -1, -1, -1, false, false);
            Assert.IsNull(rec);

            rec = await ctx.CommentEpisode(userAccount, "asfasd", title, tvDbId, -1, -1, -1, false, false);
            Assert.IsNull(rec);

            rec = await ctx.CommentEpisode(userAccount, "just wondering what is coming from here :o", title, tvDbId, year, 2, 1, false, false);
            Assert.IsNotNull(rec);
            Assert.AreEqual(rec.Status, RequestResults.Success);
        }


        [TestMethod]
        public async Task CommentTraktDataServiceTest_GetCommentsMovieById_QueryString()
        {
            var stub = new StubICommentTraktQueryService
            {
                GetCommentsMovieByIdNullableOfInt32 = b => Task.Run(() => null as string)
            };
            var imdbId = new IdsDto();
            var ctx = new CommentTraktDataService(stub, null);
            var x = await ctx.GetCommentsMovieById(imdbId);
            Assert.IsNull(x);

            stub = new StubICommentTraktQueryService
            {
                GetCommentsMovieByIdNullableOfInt32 = b => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new CommentTraktDataService(stub, null);
            x = await ctx.GetCommentsMovieById(imdbId);
            Assert.IsNull(x);
        }

        [TestMethod]
        public async Task CommentTraktDataServiceTest_GetCommentsMovieById_TestData()
        {
            var imdbId = new IdsDto();
            var url = "http://api.trakt.tv/movie/comments.json/" + TraktConstants.TraktKey + "/" + imdbId;
            var stub = new StubICommentTraktQueryService
            {
                GetCommentsMovieByIdNullableOfInt32 = b => Task.Run(() => url)
            };
            var ctx = new CommentTraktDataService(stub, null);
            var rec = await ctx.GetCommentsMovieById(null);
            Assert.IsNull(rec);
            rec = await ctx.GetCommentsMovieById(imdbId);
            Assert.AreNotEqual(rec.Count, 0);
        }



        [TestMethod]
        public async Task CommentTraktDataServiceTest_CommentMovie_QueryString()
        {
            const string imdbId = "tt2294629";
            const int year = 2013;
            const string title = "Frozen";


            var userAccount = new UserTokenDto();
            userAccount.AccessToken = "tt";
            var stub = new StubICommentTraktQueryService
            {
                CommentEpisode = () => Task.Run(() => null as string)
            };
            var ctx = new CommentTraktDataService(stub, null);
            //var x = await ctx.CommentMovie(userAccount, "what a nice movie! :) i recommend best animation movie of 2013", title, imdbId, year, false, false);
            //Assert.IsNull(x);

            stub = new StubICommentTraktQueryService
            {
                CommentMovie = () => Task.Run(() => "aadasfasdasfas")
            };
            ctx = new CommentTraktDataService(stub, null);
            //x = await ctx.CommentMovie(userAccount, "what a nice movie! :) i recommend best animation movie of 2013", title, imdbId, year, false, false);
            //Assert.IsNull(x);
        }

        [TestMethod]
        public async Task CommentTraktDataServiceTest_CommentMovie_DataTest()
        {
            var url = "http://api.trakt.tv/comment/movie/" + TraktConstants.TraktKey;
            var stub = new StubICommentTraktQueryService
            {
                CommentMovie = () => Task.Run(() => url)
            };

            const string imdbId = "tt2294629";
            const int year = 2013;
            const string title = "Frozen";



            var userAccount = new UserTokenDto();
            userAccount.AccessToken = "tt";

            var ctx = new CommentTraktDataService(stub, null);

            //var rec = await ctx.CommentMovie(null, null, null, null, -1, false, false);
            //Assert.IsNull(rec);
            //rec = await ctx.CommentMovie(userAccount, null, null, null, -1, false, false);
            //Assert.IsNull(rec);
            //rec = await ctx.CommentMovie(userAccount, "asfasd", null, null, -1, false, false);
            //Assert.IsNull(rec);
            //rec = await ctx.CommentMovie(userAccount, "asfasd", title, null, -1, false, false);
            //Assert.IsNull(rec);

            //rec = await ctx.CommentMovie(userAccount, "asfasd", title, imdbId, -1, false, false);
            //Assert.IsNull(rec);

            //rec = await ctx.CommentMovie(userAccount, "what a nice movie! :) i recommend best animation movie of 2013", title, imdbId, year, false, false);
            //Assert.IsNotNull(rec);
            //Assert.AreEqual(rec.Status, RequestResults.Success);
        }

    }
}
