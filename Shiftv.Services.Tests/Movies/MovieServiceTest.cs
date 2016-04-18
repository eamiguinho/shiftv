using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shiftv.Contracts.DataServices.Movies.Fakes;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Contracts.Services.Accounts.Fakes;
using Shiftv.Core.Models;
using Shiftv.Global;
using Shiftv.Services.Implementation.Movies;
using Shiftv.Services.Tests.Helpers;

namespace Shiftv.Services.Tests.Movies
{
    [TestClass]
    public class MovieServiceTest
    {
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            CoreModelsIoc.RegisterTypes();
        }

        [TestMethod]
        public async Task MovieServiceTest_GetTrending()
        {
            var list = new List<IMovie>();
            var user = Ioc.Container.Resolve<IUserAccount>();
            user.Username = "amiguinho";
            user.PasswordEnc = "25713423e";

            var show = Ioc.Container.Resolve<IMovie>();
            show.Year = 2010;
            show.Title = "The Social Network";
            show.ImdbId = "tt1285016";
            list.Add(show);

            try
            {
                var ctx2 = new MovieService(null, null, null);
                var res2 = await ctx2.GetTrending();
                Assert.AreEqual(StandardResults.Error, res2.Result);
                Assert.Fail();
            }
            catch (Exception)
            {

            }

            var stub = new StubIMovieTraktDataService()
            {
                GetTrendingUserAccountDto = (ac) => Task.Run(() => null as List<IMovie>)
            };

            var stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => null;

            var ctx = new MovieService(stub, stubUser, null);
            var res = await ctx.GetTrending();
            Assert.AreEqual(StandardResults.Ok, res.Result);

            stubUser.MethodCalled("GetCurrentUser", 1);

            stub = new StubIMovieTraktDataService()
            {
                GetTrendingUserAccountDto = (ac) => Task.Run(() => list),
            };

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;


            ctx = new MovieService(stub, stubUser, null);
            res = await ctx.GetTrending();
            Assert.AreEqual(StandardResults.Ok, res.Result);
            Assert.AreEqual(1, res.Data.Count);
            stubUser.MethodCalled("GetCurrentUser", 2);

        }

        [TestMethod]
        public async Task MovieServiceTest_GetRecommendation()
        {
            var list = new List<IMovie>();
            var user = Ioc.Container.Resolve<IUserAccount>();
            user.Username = "amiguinho";
            user.PasswordEnc = "25713423e";

            var show = Ioc.Container.Resolve<IMovie>();
            show.Year = 2010;
            show.Title = "The Social Network";
            show.ImdbId = "tt1285016";
            list.Add(show);

            try
            {
                var ctx2 = new MovieService(null, null, null);
                var res2 = await ctx2.GetRecommendations();
                Assert.AreEqual(StandardResults.Error, res2.Result);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            var stub = new StubIMovieTraktDataService()
            {
                GetRecommendationsUserAccountDto = (ac) => Task.Run(() => null as List<IMovie>)
            };

            var stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => null;

            var ctx = new MovieService(stub, stubUser, null);
            var res = await ctx.GetRecommendations();
            Assert.AreEqual(StandardResults.Error, res.Result);

            stubUser.MethodCalled("GetCurrentUser", 1);

            stub = new StubIMovieTraktDataService()
            {
                GetRecommendationsUserAccountDto = (ac) => Task.Run(() => list),
            };

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;


            ctx = new MovieService(stub, stubUser, null);
            res = await ctx.GetRecommendations();
            Assert.AreEqual(StandardResults.Ok, res.Result);
            Assert.AreEqual(1, res.Data.Count);
            stubUser.MethodCalled("GetCurrentUser", 1);

        }


        [TestMethod]
        public async Task MovieServiceTest_SearchMoviesByKey()
        {
            var list = new List<IMovie>();
            var user = Ioc.Container.Resolve<IUserAccount>();
            user.Username = "amiguinho";
            user.PasswordEnc = "25713423e";

            var show = Ioc.Container.Resolve<IMovie>();
            show.Year = 2010;
            show.Title = "The Social Network";
            show.ImdbId = "tt1285016";
            list.Add(show);

            try
            {
                var ctx2 = new MovieService(null, null, null);
                var res2 = await ctx2.SearchMoviesByKey(null);
                Assert.AreEqual(StandardResults.Error, res2.Result);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            var stub = new StubIMovieTraktDataService()
            {
                SearchMoviesByKeyString = (ac) => Task.Run(() => null as List<IMovie>)
            };

            var stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => null;

            var ctx = new MovieService(stub, stubUser, null);
            var res = await ctx.SearchMoviesByKey(null);
            Assert.AreEqual(StandardResults.Error, res.Result);


            stub = new StubIMovieTraktDataService()
            {
                SearchMoviesByKeyString = (ac) => Task.Run(() => list),
            };

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;

            ctx = new MovieService(stub, stubUser, null);
            res = await ctx.SearchMoviesByKey("test");
            Assert.AreEqual(StandardResults.Ok, res.Result);
            Assert.AreEqual(show.Title, res.Data.First().Title);
        }



        [TestMethod]
        public async Task MovieServiceTest_CheckinEpisode()
        {
            var user = Ioc.Container.Resolve<IUserAccount>();
            user.Username = "amiguinho";
            user.PasswordEnc = "25713423e";

            var movie = Ioc.Container.Resolve<IMovie>();
            movie.Year = 2013;
            movie.Title = "Frozen";
            movie.ImdbId = "tt2294629";

            var result = Ioc.Container.Resolve<ICheckinResult>();
            result.Status = RequestResults.Success;
            result.Message = "check in complete!";


            try
            {
                var ctx1 = new MovieService(null, null, null);
                var res1 = await ctx1.CheckIn();
                Assert.AreEqual(res1.Result, StandardResults.Error);
            }
            catch (Exception)
            {
            }

            var stub = new StubIMovieTraktDataService()
            {
                CheckInUserAccountDtoStringStringInt32 = (ac, i1, s2, s3) => Task.Run(() => null as ICheckinResult),
            };

            var stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => null;

            var ctx = new MovieService(stub, stubUser, null);
            var res = await ctx.CheckIn();
            Assert.AreEqual(res.Result, StandardResults.Error);

            stubUser.MethodCalled("GetCurrentUser", 1);

            stub = new StubIMovieTraktDataService()
            {
                CheckInUserAccountDtoStringStringInt32 = (ac, i1, s2, s3) => Task.Run(() => result),
                GetByImdbIdUserAccountDtoString = (ac, value) => Task.Run(() => movie)
            };

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;

            ctx = new MovieService(stub, stubUser, null);
            await ctx.SetCurrent(movie);
            res = await ctx.CheckIn();
            Assert.AreEqual(StandardResults.Ok, res.Result);
            Assert.AreEqual(RequestResults.Success, res.Data.Status);
            stubUser.MethodCalled("GetCurrentUser", 2);
            Assert.AreEqual(StandardResults.Ok, res.Result);

        }


    }
}
