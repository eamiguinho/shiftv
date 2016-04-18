using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shiftv.Contracts.DataServices.Comments.Fakes;
using Shiftv.Contracts.Domain.Comments;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Contracts.Services.Accounts.Fakes;
using Shiftv.Contracts.Services.Movies.Fakes;
using Shiftv.Contracts.Services.Shows.Fakes;
using Shiftv.Core.Models;
using Shiftv.Global;
using Shiftv.Services.Implementation.Comments;
using Shiftv.Services.Tests.Helpers;

namespace Shiftv.Services.Tests.Comments
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class CommentServiceTest
    {
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            CoreModelsIoc.RegisterTypes();
        }

        [TestMethod]
        public async Task CommentServiceTest_GetCommentsShowById()
        {
            var show = Ioc.Container.Resolve<IShow>();
            show.TvDbId = -1;
            var list = new List<IComment>();
            var comment = Ioc.Container.Resolve<IComment>();
            comment.Text = "blabla";
            comment.Id = 1242312;
            comment.IsSpoiler = false;
            list.Add(comment);

            var ctx = new CommentService(null, null, null);
            var res = await ctx.GetCommentsShowById();
            Assert.AreEqual(res.Result, StandardResults.Error);

            var stub = new StubICommentTraktDataService()
            {
                GetCommentsShowByIdInt32 = value => Task.Run(() => null as List<IComment>)
            };

            var stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => null;

            try
            {
                ctx = new CommentService(stub, null, null);
                res = await ctx.GetCommentsShowById();
                Assert.AreEqual(res.Result, StandardResults.Error);
            }
            catch (Exception )
            {
            }

            show.TvDbId = 153021;
            stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => show;
            ctx = new CommentService(stub, null, stubShow);

            res = await ctx.GetCommentsShowById();
            Assert.AreEqual(res.Result, StandardResults.Error);
            stubShow.MethodCalled("GetCurrentShow", 1);

            stub = new StubICommentTraktDataService()
            {
                GetCommentsShowByIdInt32 = value => Task.Run(() => list)
            };


            stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => show;
            ctx = new CommentService(stub, null, stubShow);
            res = await ctx.GetCommentsShowById();
            Assert.IsNotNull(res);
            Assert.AreNotEqual(res.Data.Count, 0);
            stubShow.MethodCalled("GetCurrentShow", 1);
        }

        [TestMethod]
        public async Task CommentServiceTest_CommentShow()
        {
            var show = Ioc.Container.Resolve<IShow>();
            show.TvDbId = -1;
            var comment = Ioc.Container.Resolve<ICommentResult>();
            comment.Message = "blabla";
            comment.Status = RequestResults.Success;
            var user = Ioc.Container.Resolve<IUserAccount>();
            user.Username = "amiguinho";
            user.PasswordEnc = "25173423e";

            var ctx = new CommentService(null, null, null);
            var res = await ctx.CommentsShow(null, false, false);
            Assert.AreEqual(res.Result, StandardResults.Error);

            var stub = new StubICommentTraktDataService()
            {
                CommentShowUserAccountDtoStringStringInt32Int32BooleanBoolean = (p1, p2, p3, p4, p5, p6, p7) => Task.Run(() => null as ICommentResult)
            };

            ctx = new CommentService(stub, null, null);
            res = await ctx.CommentsShow(null, false, false);
            Assert.AreEqual(res.Result, StandardResults.Error);

            //show.TvDbId = 153021;
            //res = await ctx.CommentsShow(show, "my comment", false, false);
            //Assert.AreEqual(res.Result, StandardResults.Error);

            stub = new StubICommentTraktDataService()
            {
                CommentShowUserAccountDtoStringStringInt32Int32BooleanBoolean = (p1, p2, p3, p4, p5, p6, p7) => Task.Run(() => null as ICommentResult)
            };

            var stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => null;
            var stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => null;

            ctx = new CommentService(stub, stubUser, stubShow);
            res = await ctx.CommentsShow("my comment", false, false);
            Assert.IsNotNull(res);
            Assert.AreNotEqual(res.Result, StandardResults.Ok);
            stubUser.MethodCalled("GetCurrentUser", 1);

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;

            stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => show;

            ctx = new CommentService(stub, stubUser, stubShow);
            res = await ctx.CommentsShow("my comment", false, false);
            Assert.IsNotNull(res);
            Assert.AreNotEqual(res.Result, StandardResults.Ok);
            stubUser.MethodCalled("GetCurrentUser", 1);
            stubShow.MethodCalled("GetCurrentShow", 1);
        }

        [TestMethod]
        public async Task CommentServiceTest_GetEpisodeComments()
        {
            var show = Ioc.Container.Resolve<IShow>();
            show.TvDbId = -1;
            var list = new List<IComment>();
            var comment = Ioc.Container.Resolve<IComment>();
            comment.Text = "blabla";
            comment.Id = 1242312;
            comment.IsSpoiler = false;
            list.Add(comment);

            var ctx = new CommentService(null, null, null);
            var res = await ctx.GetEpisodeComments(null, -1, -1);
            Assert.AreEqual(res.Result, StandardResults.Error);

            var stub = new StubICommentTraktDataService()
            {
                GetCommentsByEpisodeInt32Int32Int32 = (a, b, c) => Task.Run(() => null as List<IComment>)
            };

            ctx = new CommentService(stub, null, null);
            res = await ctx.GetEpisodeComments(show, -1, -1);
            Assert.AreEqual(res.Result, StandardResults.Error);

            show.TvDbId = 153021;
            res = await ctx.GetEpisodeComments(show, 1, -1);
            Assert.AreEqual(res.Result, StandardResults.Error);

            res = await ctx.GetEpisodeComments(show, 1, 1);
            Assert.AreEqual(res.Result, StandardResults.Error);

            stub = new StubICommentTraktDataService()
            {
                GetCommentsByEpisodeInt32Int32Int32 = (a, b, c) => Task.Run(() => list)
            };

            ctx = new CommentService(stub, null, null);
            res = await ctx.GetEpisodeComments(show, 1, 1);
            Assert.IsNotNull(res);
            Assert.AreNotEqual(res.Data.Count, 0);
        }


        [TestMethod]
        public async Task CommentServiceTest_CommentEpisode()
        {
            var show = Ioc.Container.Resolve<IShow>();
            show.TvDbId = -1;
            var comment = Ioc.Container.Resolve<ICommentResult>();
            comment.Message = "blabla";
            comment.Status = RequestResults.Success;
            var user = Ioc.Container.Resolve<IUserAccount>();
            user.Username = "amiguinho";
            user.PasswordEnc = "25173423e";

            var ctx = new CommentService(null, null, null);
            var res = await ctx.CommentEpisode(null, -1, -1, false, false);
            Assert.AreEqual(res.Result, StandardResults.Error);

            var stub = new StubICommentTraktDataService()
            {
                CommentEpisodeUserAccountDtoStringStringInt32Int32Int32Int32BooleanBoolean = (p1, p2, p3, p4, p5, p6, p7, p8, p9) => Task.Run(() => null as ICommentResult)
            };

            ctx = new CommentService(stub, null, null);
            res = await ctx.CommentEpisode("blabla", -1, -1, false, false);
            Assert.AreEqual(StandardResults.Error, res.Result);

            stub = new StubICommentTraktDataService()
            {
                CommentEpisodeUserAccountDtoStringStringInt32Int32Int32Int32BooleanBoolean = (p1, p2, p3, p4, p5, p6, p7, p8, p9) => Task.Run(() => comment)
            };

            var stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => null;

            ctx = new CommentService(stub, stubUser, null);
            res = await ctx.CommentEpisode("my comment", 1, 1, false, false);
            Assert.IsNotNull(res);
            Assert.AreEqual(StandardResults.Error, res.Result);
            stubUser.MethodCalled("GetCurrentUser", 1);

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;

            var stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => null;

            ctx = new CommentService(stub, stubUser, stubShow);
            res = await ctx.CommentEpisode("my comment", 1, 1, false, false);
            Assert.IsNotNull(res);
            Assert.AreEqual(StandardResults.Error, res.Result);
            stubUser.MethodCalled("GetCurrentUser", 1);
            stubShow.MethodCalled("GetCurrentShow", 1);

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;

            stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => show;

            ctx = new CommentService(stub, stubUser, stubShow);
            res = await ctx.CommentEpisode("my comment", 1, 1, false, false);
            Assert.IsNotNull(res);
            Assert.AreEqual(StandardResults.Ok, res.Result);
            stubUser.MethodCalled("GetCurrentUser", 1);
            stubShow.MethodCalled("GetCurrentShow", 1);

        }

        [TestMethod]
        public async Task CommentServiceTest_GetCommentsMovieById()
        {
            var movie = Ioc.Container.Resolve<IMovie>();
            movie.ImdbId = null;
            var list = new List<IComment>();
            var comment = Ioc.Container.Resolve<IComment>();
            comment.Text = "blabla";
            comment.Id = 1242312;
            comment.IsSpoiler = false;
            list.Add(comment);
            try
            {
                var ctx1 = new CommentService(null, null, null, null);
                var res1 = await ctx1.GetCommentsMovie();
                Assert.AreEqual(res1.Result, StandardResults.Error);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            var stub = new StubICommentTraktDataService()
            {
                GetCommentsMovieByIdString = value => Task.Run(() => null as List<IComment>)
            };

            var stubMovie = FakesHelper.CreateNotImplementedStub<StubIMovieService>();
            stubMovie.GetCurrentMovie = () => null;

            try
            {
                var ctx2 = new CommentService(stub, null, null, stubMovie);
                var res2 = await ctx2.GetCommentsMovie();
                Assert.AreEqual(res2.Result, StandardResults.Error);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            movie.ImdbId = "153021";
            stubMovie = FakesHelper.CreateNotImplementedStub<StubIMovieService>();
            stubMovie.GetCurrentMovie = () => movie;
            var ctx = new CommentService(stub, null, null, stubMovie);

            var res = await ctx.GetCommentsMovie();
            Assert.AreEqual(res.Result, StandardResults.Error);
            stubMovie.MethodCalled("GetCurrentMovie", 1);

            stub = new StubICommentTraktDataService()
            {
                GetCommentsMovieByIdString = value => Task.Run(() => list)
            };


            stubMovie = FakesHelper.CreateNotImplementedStub<StubIMovieService>();
            stubMovie.GetCurrentMovie = () => movie;
            ctx = new CommentService(stub, null, null, stubMovie);
            res = await ctx.GetCommentsMovie();
            Assert.IsNotNull(res);
            Assert.AreNotEqual(res.Data.Count, 0);
            stubMovie.MethodCalled("GetCurrentMovie", 1);
        }


        [TestMethod]
        public async Task CommentServiceTest_CommentMovie()
        {
            var movie = Ioc.Container.Resolve<IMovie>();
            movie.ImdbId = null;
            var comment = Ioc.Container.Resolve<ICommentResult>();
            comment.Message = "blabla";
            comment.Status = RequestResults.Success;
            var user = Ioc.Container.Resolve<IUserAccount>();
            user.Username = "amiguinho";
            user.PasswordEnc = "25173423e";

            var ctx = new CommentService(null, null, null);
            var res = await ctx.CommentsMovie(null, false, false);
            Assert.AreEqual(res.Result, StandardResults.Error);

            var stub = new StubICommentTraktDataService()
            {
                CommentMovieUserAccountDtoStringStringStringInt32BooleanBoolean = (p1, p2, p3, p4, p5, p6, p7) => Task.Run(() => null as ICommentResult)
            };

            ctx = new CommentService(stub, null, null);
            res = await ctx.CommentsMovie(null, false, false);
            Assert.AreEqual(res.Result, StandardResults.Error);

            //show.TvDbId = 153021;
            //res = await ctx.CommentsShow(show, "my comment", false, false);
            //Assert.AreEqual(res.Result, StandardResults.Error);

            stub = new StubICommentTraktDataService()
            {
                CommentMovieUserAccountDtoStringStringStringInt32BooleanBoolean = (p1, p2, p3, p4, p5, p6, p7) => Task.Run(() => null as ICommentResult)
            };

            var stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => null;
            var stubMovie = FakesHelper.CreateNotImplementedStub<StubIMovieService>();
            stubMovie.GetCurrentMovie = () => null;

            ctx = new CommentService(stub, stubUser, null, stubMovie);
            res = await ctx.CommentsMovie("my comment", false, false);
            Assert.IsNotNull(res);
            Assert.AreNotEqual(res.Result, StandardResults.Ok);
            stubUser.MethodCalled("GetCurrentUser", 1);

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;

            stubMovie = FakesHelper.CreateNotImplementedStub<StubIMovieService>();
            stubMovie.GetCurrentMovie = () => movie;

            ctx = new CommentService(stub, stubUser, null, stubMovie);
            res = await ctx.CommentsMovie("my comment", false, false);
            Assert.IsNotNull(res);
            Assert.AreNotEqual(res.Result, StandardResults.Ok);
            stubUser.MethodCalled("GetCurrentUser", 1);
            stubMovie.MethodCalled("GetCurrentMovie", 1);
        }
    }
}
