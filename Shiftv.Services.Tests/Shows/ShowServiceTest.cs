using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shiftv.Contracts.DataServices.Shows.Fakes;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Contracts.Services.Accounts.Fakes;
using Shiftv.Core.Models;
using Shiftv.Global;
using Shiftv.Services.Implementation.Shows;
using Shiftv.Services.Tests.Helpers;

namespace Shiftv.Services.Tests.Shows
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ShowServiceTest
    {
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            CoreModelsIoc.RegisterTypes();
        }

        [TestMethod]
        public async Task ShowServiceTest_GetByImdbId()
        {
            var imdbId = "tt1520211";
            var show = Ioc.Container.Resolve<IShow>();
            show.Title = "The Walking Dead";
            show.ImdbId = imdbId;


            var user = Ioc.Container.Resolve<IUserAccount>();
            user.Username = "amiguinho";
            user.PasswordEnc = "25713423e";

            var ctx = new ShowService(null, null);
            var res = await ctx.GetByImdbId(null);
            Assert.AreEqual(res.Result, StandardResults.Error);

            res = await ctx.GetByImdbId("");
            Assert.AreEqual(res.Result, StandardResults.Error);


            var stub = new StubIShowTraktDataService()
            {
                GetByImdbIdUserAccountDtoString = (ac, value) => Task.Run(() => null as IShow)
            };

            var stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => null;
         

            try
            {
                ctx = new ShowService(stub, stubUser);
                res = await ctx.GetByImdbId(imdbId);
                stubUser.MethodCalled("GetCurrentUser", 1);
                Assert.AreEqual(res.Result, StandardResults.Error);
                Assert.Fail();
            }
            catch (Exception)
            {
                
            }


          

            stub = new StubIShowTraktDataService()
            {
                GetByImdbIdUserAccountDtoString = (ac, value) => Task.Run(() => show)
            };

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;

            ctx = new ShowService(stub, stubUser);
            res = await ctx.GetByImdbId(imdbId);
            Assert.AreEqual(res.Result, StandardResults.Ok);
            Assert.AreEqual(res.Data.ImdbId, imdbId);
            stubUser.MethodCalled("GetCurrentUser", 1);
        }


        [TestMethod]
        public async Task ShowServiceTest_RateShow()
        {
            var generalPost = Ioc.Container.Resolve<IRateResult>();
            generalPost.Status = RequestResults.Success;
            generalPost.Message = "test";
            generalPost.Type = RateTypes.Show;

            var user = Ioc.Container.Resolve<IUserAccount>();
            user.Username = "amiguinho";
            user.PasswordEnc = "25713423e";

            var show = Ioc.Container.Resolve<IShow>();
            show.TvDbId = 258823;
            show.Year = 2012;
            show.Title = "Revolution";
            show.ImdbId = "tt2070791";


            var ctx = new ShowService(null, null);
            var res = await ctx.RateShow(false);
            Assert.AreEqual(res.Result, StandardResults.Error);

            var stub = new StubIShowTraktDataService()
            {
                RateShowUserAccountDtoBooleanStringInt32Int32 = (ac, i1, s2, i3, i4) => Task.Run(() => null as IRateResult)
            };

            var stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => null;

            ctx = new ShowService(stub, stubUser);
            res = await ctx.RateShow(true);
            Assert.AreEqual(res.Result, StandardResults.Error);

            stubUser.MethodCalled("GetCurrentUser", 1);

            stub = new StubIShowTraktDataService()
            {
                RateShowUserAccountDtoBooleanStringInt32Int32 = (ac, i1, s2, i3, i4) => Task.Run(() => generalPost),
                GetByImdbIdUserAccountDtoString = (ac, value) => Task.Run(() => show)
            };

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;


            ctx = new ShowService(stub, stubUser);
            res = await ctx.RateShow(true);
            Assert.AreEqual(res.Result, StandardResults.Error);

            ctx = new ShowService(stub, stubUser);
            await ctx.SetCurrent(show);
            res = await ctx.RateShow(true);
            Assert.AreEqual(StandardResults.Ok, res.Result);
            Assert.AreEqual(RateTypes.Show, res.Data.Type);

        }


        [TestMethod]
        public async Task ShowServiceTest_AddToWatchlist()
        {
            var generalPost = Ioc.Container.Resolve<IGenericPostResult>();
            generalPost.Status = RequestResults.Success;
            generalPost.Message = "test";

            var user = Ioc.Container.Resolve<IUserAccount>();
            user.Username = "amiguinho";
            user.PasswordEnc = "25713423e";

            var show = Ioc.Container.Resolve<IShow>();
            show.TvDbId = 258823;
            show.Year = 2012;
            show.Title = "Revolution";
            show.ImdbId = "tt2070791";


            var ctx = new ShowService(null, null);
            var res = await ctx.AddToWatchlist();
            Assert.AreEqual(StandardResults.Error, res.Result);

            var stub = new StubIShowTraktDataService()
            {
                AddShowToWatchlistUserAccountDtoInt32StringInt32 = (ac, i1, s2, i3) => Task.Run(() => null as IGenericPostResult)
            };

            var stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => null;

            ctx = new ShowService(stub, stubUser);
            res = await ctx.AddToWatchlist();
            Assert.AreEqual(StandardResults.Error, res.Result);

            stubUser.MethodCalled("GetCurrentUser", 1);

            stub = new StubIShowTraktDataService()
            {
                AddShowToWatchlistUserAccountDtoInt32StringInt32 = (ac, i1, s2, i3) => Task.Run(() => generalPost),
            };

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;


            ctx = new ShowService(stub, stubUser);
            res = await ctx.AddToWatchlist();
            Assert.AreEqual(StandardResults.Ok, res.Result);
            Assert.AreEqual(RequestResults.Success, res.Data.Status);

        }

        [TestMethod]
        public async Task ShowServiceTest_RemoveFromWatchlist()
        {
            var generalPost = Ioc.Container.Resolve<IGenericPostResult>();
            generalPost.Status = RequestResults.Success;
            generalPost.Message = "test";

            var user = Ioc.Container.Resolve<IUserAccount>();
            user.Username = "amiguinho";
            user.PasswordEnc = "25713423e";

            var show = Ioc.Container.Resolve<IShow>();
            show.TvDbId = 258823;
            show.Year = 2012;
            show.Title = "Revolution";
            show.ImdbId = "tt2070791";


            var ctx = new ShowService(null, null);
            var res = await ctx.RemoveFromWatchlist();
            Assert.AreEqual(StandardResults.Error, res.Result);

            var stub = new StubIShowTraktDataService()
            {
                RemoveShowFromWatchlistUserAccountDtoInt32StringInt32 = (ac, i1, s2, i3) => Task.Run(() => null as IGenericPostResult)
            };

            var stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => null;

            ctx = new ShowService(stub, stubUser);
            res = await ctx.RemoveFromWatchlist();
            Assert.AreEqual(StandardResults.Error, res.Result);

            stubUser.MethodCalled("GetCurrentUser", 1);

            stub = new StubIShowTraktDataService()
            {
                RemoveShowFromWatchlistUserAccountDtoInt32StringInt32 = (ac, i1, s2, i3) => Task.Run(() => generalPost),
            };

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;


            ctx = new ShowService(stub, stubUser);
            res = await ctx.RemoveFromWatchlist();
            Assert.AreEqual(StandardResults.Ok, res.Result);
            Assert.AreEqual(RequestResults.Success, res.Data.Status);

        }


        [TestMethod]
        public async Task ShowServiceTest_SearchShowsByKey()
        {
            var list = new List<IShow>();
            var show = Ioc.Container.Resolve<IShow>();
            show.TvDbId = 258823;
            show.Year = 2012;
            show.Title = "Revolution";
            show.ImdbId = "tt2070791";
            list.Add(show);

            var ctx = new ShowService(null, null);
            var res = await ctx.SearchShowsByKey(null);
            Assert.AreEqual(StandardResults.Error, res.Result);

            var stub = new StubIShowTraktDataService()
            {
                SearchShowsByKeyString = (s1) => Task.Run(() => null as List<IShow>)
            };

            ctx = new ShowService(stub, null);
            res = await ctx.SearchShowsByKey("rev");
            Assert.AreEqual(StandardResults.Error, res.Result);

            stub = new StubIShowTraktDataService()
            {
                SearchShowsByKeyString = (s1) => Task.Run(() => list),
            };


            ctx = new ShowService(stub, null);
            res = await ctx.SearchShowsByKey("rev");
            Assert.AreEqual(StandardResults.Ok, res.Result);
            Assert.AreEqual(1, res.Data.Count);

        }

        [TestMethod]
        public async Task ShowServiceTest_GetRecommentation()
        {
            var list = new List<IShow>();
            var user = Ioc.Container.Resolve<IUserAccount>();
            user.Username = "amiguinho";
            user.PasswordEnc = "25713423e";

            var show = Ioc.Container.Resolve<IShow>();
            show.TvDbId = 258823;
            show.Year = 2012;
            show.Title = "Revolution";
            show.ImdbId = "tt2070791";
            list.Add(show);

            var stub = new StubIShowTraktDataService()
            {
                GetRecommendationsByUserUserAccountDto = (ac) => Task.Run(() => null as List<IShow>)
            };

            var stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => null;


           var ctx = new ShowService(stub, stubUser);
            var res = await ctx.GetRecommendations();
            Assert.AreEqual(StandardResults.Error, res.Result);

            stubUser.MethodCalled("GetCurrentUser", 1);

            stub = new StubIShowTraktDataService()
            {
                GetRecommendationsByUserUserAccountDto = (ac) => Task.Run(() => list),
            };

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;


            ctx = new ShowService(stub, stubUser);
            res = await ctx.GetRecommendations();
            Assert.AreEqual(StandardResults.Ok, res.Result);
            Assert.AreEqual(1, res.Data.Count);

        }


        [TestMethod]
        public async Task ShowServiceTest_GetTrending()
        {
            var list = new List<IShow>();
            var user = Ioc.Container.Resolve<IUserAccount>();
            user.Username = "amiguinho";
            user.PasswordEnc = "25713423e";

            var show = Ioc.Container.Resolve<IShow>();
            show.TvDbId = 258823;
            show.Year = 2012;
            show.Title = "Revolution";
            show.ImdbId = "tt2070791";
            list.Add(show);

            try
            {
                var ctx2 = new ShowService(null, null);
                var res2 = await ctx2.GetTrending();
                Assert.AreEqual(StandardResults.Error, res2.Result);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            var stub = new StubIShowTraktDataService()
            {
                GetTrendingUserAccountDto = (ac) => Task.Run(() => null as List<IShow>)
            };

            var stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => null;

            var ctx = new ShowService(stub, stubUser);
           var res = await ctx.GetTrending();
            Assert.AreEqual(StandardResults.Ok, res.Result);

            stubUser.MethodCalled("GetCurrentUser", 1);

            stub = new StubIShowTraktDataService()
            {
                GetTrendingUserAccountDto = (ac) => Task.Run(() => list),
            };

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;


            ctx = new ShowService(stub, stubUser);
            res = await ctx.GetTrending();
            Assert.AreEqual(StandardResults.Ok, res.Result);
            Assert.AreEqual(1, res.Data.Count);
            stubUser.MethodCalled("GetCurrentUser", 2);

        }


    }
}
