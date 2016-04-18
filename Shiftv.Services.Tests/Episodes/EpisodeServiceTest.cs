using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shiftv.Contracts.DataServices.Episodes.Fakes;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Contracts.Services.Accounts.Fakes;
using Shiftv.Contracts.Services.Shows.Fakes;
using Shiftv.Core.Models;
using Shiftv.Global;
using Shiftv.Services.Implementation.Episodes;
using Shiftv.Services.Tests.Helpers;

namespace Shiftv.Services.Tests.Episodes
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class EpisodeServiceTest
    {
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            CoreModelsIoc.RegisterTypes();
        }

        [TestMethod]
        public async Task EpisodeServiceTest_GetEpisodesBySeason()
        {
            var user = Ioc.Container.Resolve<IUserAccount>();
            user.Username = "amiguinho";
            user.PasswordEnc = "25713423e";

            var show = Ioc.Container.Resolve<IShow>();
            show.TvDbId = 258823;
            show.Year = 2012;
            show.Title = "Revolution";
            show.ImdbId = "tt2070791";

            var episode = Ioc.Container.Resolve<IEpisode>();
            episode.Season = 1;
            episode.Number = 1;
            episode.Title = "Pilot";
            var list = new List<IEpisode>();
            list.Add(episode);

            var ctx = new EpisodeService(null, null, null, null);
            var res = await ctx.GetEpisodesBySeason(-1);
            Assert.AreEqual(res.Result, StandardResults.Error);

            var stub = new StubIEpisodeTraktDataService()
            {
                GetEpisodeBySeasonUserAccountDtoInt32Int32String = (ac, i1, s2, s3) => Task.Run(() => null as List<IEpisode>)
            };

            var stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => null;

            ctx = new EpisodeService(stub, stubUser, null,null);
            res = await ctx.GetEpisodesBySeason(1);
            Assert.AreEqual(res.Result, StandardResults.Error);

            stubUser.MethodCalled("GetCurrentUser", 1);

            stub = new StubIEpisodeTraktDataService()
            {
                GetEpisodeBySeasonUserAccountDtoInt32Int32String = (ac, i1, s2, s3) => Task.Run(() => list)
            };

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;

            try
            {
                ctx = new EpisodeService(stub, stubUser, null,null);
                res = await ctx.GetEpisodesBySeason(1);
                Assert.AreEqual(res.Result, StandardResults.Error);
                stubUser.MethodCalled("GetCurrentUser", 1);
                Assert.Fail();
            }
            catch (Exception)
            {

            }

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;

            var stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => null;

            ctx = new EpisodeService(stub, stubUser, stubShow,null);
            res = await ctx.GetEpisodesBySeason(1);
            Assert.AreEqual(res.Result, StandardResults.Error);
            stubUser.MethodCalled("GetCurrentUser", 1);
            stubShow.MethodCalled("GetCurrentShow", 1);

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;


            stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => show;

            ctx = new EpisodeService(stub, stubUser, stubShow,null);
            res = await ctx.GetEpisodesBySeason(1);
            Assert.AreEqual(StandardResults.Ok, res.Result);
            Assert.AreEqual(1, res.Data.Count);
            stubUser.MethodCalled("GetCurrentUser", 1);
            stubShow.MethodCalled("GetCurrentShow", 1);

        }


        [TestMethod]
        public async Task EpisodeServiceTest_GetWatchingNow()
        {
            var show = Ioc.Container.Resolve<IShow>();
            show.TvDbId = 258823;
            show.Year = 2012;
            show.Title = "Revolution";
            show.ImdbId = "tt2070791";

            var x = Ioc.Container.Resolve<IUserProfile>();
            x.FullName = "Emanuel Amiguinho";
            var list = new List<IUserProfile>();
            list.Add(x);

            var ctx = new EpisodeService(null, null, null, null);
            var res = await ctx.GetWatchingNow(-1, -1);
            Assert.AreEqual(res.Result, StandardResults.Error);


            ctx = new EpisodeService(null, null, null, null);
            res = await ctx.GetWatchingNow(1, -1);
            Assert.AreEqual(res.Result, StandardResults.Error);


            ctx = new EpisodeService(null, null, null, null);
            res = await ctx.GetWatchingNow(-1, 1);
            Assert.AreEqual(res.Result, StandardResults.Error);

            var stub = new StubIEpisodeTraktDataService()
            {
                GetWatchingNowInt32Int32Int32 = (a1, a2,a3) => Task.Run(() => null as List<IUserProfile>)
            };


            var stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => null;

            ctx = new EpisodeService(stub, null, stubShow, null);
            res = await ctx.GetWatchingNow(1, 1);
            Assert.AreEqual(res.Result, StandardResults.Error);

            stub = new StubIEpisodeTraktDataService()
            {
                GetWatchingNowInt32Int32Int32 = (a1, a2, a3) => Task.Run(() => list)
            };

            stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => null;

            ctx = new EpisodeService(stub, null, stubShow, null);
            res = await ctx.GetWatchingNow(1,1);
            Assert.AreEqual(res.Result, StandardResults.Error);
            stubShow.MethodCalled("GetCurrentShow", 1);



            stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => show;

            ctx = new EpisodeService(stub, null, stubShow, null);
            res = await ctx.GetWatchingNow(1,1);
            Assert.AreEqual(StandardResults.Ok, res.Result);
            Assert.AreEqual(1, res.Data.Count);
            stubShow.MethodCalled("GetCurrentShow", 1);
        }



        [TestMethod]
        public async Task EpisodeServiceTest_SetAsSeen()
        {
            var user = Ioc.Container.Resolve<IUserAccount>();
            user.Username = "amiguinho";
            user.PasswordEnc = "25713423e";

            var show = Ioc.Container.Resolve<IShow>();
            show.TvDbId = 258823;
            show.Year = 2012;
            show.Title = "Revolution";
            show.ImdbId = "tt2070791";

            var episode = Ioc.Container.Resolve<IEpisode>();
            episode.Season = 2;
            episode.Number = 1;

            var result = Ioc.Container.Resolve<IGenericPostResult>();
            result.Status = RequestResults.Success;
            result.Message = "Set as seen complete!";

            var ctx = new EpisodeService(null, null, null, null);
            var res = await ctx.SetAsSeen(-1,-1);
            Assert.AreEqual(res.Result, StandardResults.Error);

            var stub = new StubIEpisodeTraktDataService()
            {
                SetAsSeenUserAccountDtoInt32StringStringInt32Int32Int32 = (ac, i1, s2,s3,i4,i5,i6) => Task.Run(() => null as IGenericPostResult)
            };

            var stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => null;

            ctx = new EpisodeService(stub, stubUser, null,null);
            res = await ctx.SetAsSeen(2,1);
            Assert.AreEqual(res.Result, StandardResults.Error);

            stubUser.MethodCalled("GetCurrentUser", 1);

            stub = new StubIEpisodeTraktDataService()
            {
                SetAsSeenUserAccountDtoInt32StringStringInt32Int32Int32 = (ac, i1, s2, s3, i4, i5, i6) => Task.Run(() => result)
            };

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;

            try
            {
                ctx = new EpisodeService(stub, stubUser, null,null);
                res = await ctx.SetAsSeen(2, 1);
                Assert.AreEqual(res.Result, StandardResults.Error);
                stubUser.MethodCalled("GetCurrentUser", 1);
                Assert.Fail();
            }
            catch (Exception)
            {

            }

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;

            var stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => null;

            ctx = new EpisodeService(stub, stubUser, stubShow,null);
            res = await ctx.SetAsSeen(2, 1);
            Assert.AreEqual(res.Result, StandardResults.Error);
            stubUser.MethodCalled("GetCurrentUser", 1);
            stubShow.MethodCalled("GetCurrentShow", 1);

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;


            stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => show;

            ctx = new EpisodeService(stub, stubUser, stubShow,null);
            res = await ctx.SetAsSeen(2, 1);
            Assert.AreEqual(StandardResults.Ok, res.Result);
            Assert.AreEqual(RequestResults.Success, res.Data.Status);
            stubUser.MethodCalled("GetCurrentUser", 1);
            stubShow.MethodCalled("GetCurrentShow", 1);

        }

        [TestMethod]
        public async Task EpisodeServiceTest_SetAsUnseen()
        {
            var user = Ioc.Container.Resolve<IUserAccount>();
            user.Username = "amiguinho";
            user.PasswordEnc = "25713423e";

            var show = Ioc.Container.Resolve<IShow>();
            show.TvDbId = 258823;
            show.Year = 2012;
            show.Title = "Revolution";
            show.ImdbId = "tt2070791";

            var episode = Ioc.Container.Resolve<IEpisode>();
            episode.Season = 2;
            episode.Number = 1;

            var result = Ioc.Container.Resolve<IGenericPostResult>();
            result.Status = RequestResults.Success;
            result.Message = "Set as unseen complete!";

            var ctx = new EpisodeService(null, null, null, null);
            var res = await ctx.SetAsUnseen(-1, -1);
            Assert.AreEqual(res.Result, StandardResults.Error);

            var stub = new StubIEpisodeTraktDataService()
            {
                SetAsUnseenUserAccountDtoInt32StringStringInt32Int32Int32 = (ac, i1, s2, s3, i4, i5, i6) => Task.Run(() => null as IGenericPostResult)
            };

            var stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => null;

            ctx = new EpisodeService(stub, stubUser, null,null);
            res = await ctx.SetAsUnseen(2, 1);
            Assert.AreEqual(res.Result, StandardResults.Error);

            stubUser.MethodCalled("GetCurrentUser", 1);

            stub = new StubIEpisodeTraktDataService()
            {
                SetAsUnseenUserAccountDtoInt32StringStringInt32Int32Int32 = (ac, i1, s2, s3, i4, i5, i6) => Task.Run(() => result)
            };

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;

            try
            {
                ctx = new EpisodeService(stub, stubUser, null,null);
                res = await ctx.SetAsUnseen(2, 1);
                Assert.AreEqual(res.Result, StandardResults.Error);
                stubUser.MethodCalled("GetCurrentUser", 1);
                Assert.Fail();
            }
            catch (Exception)
            {

            }

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;

            var stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => null;

            ctx = new EpisodeService(stub, stubUser, stubShow,null);
            res = await ctx.SetAsUnseen(2, 1);
            Assert.AreEqual(res.Result, StandardResults.Error);
            stubUser.MethodCalled("GetCurrentUser", 1);
            stubShow.MethodCalled("GetCurrentShow", 1);

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;


            stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => show;

            ctx = new EpisodeService(stub, stubUser, stubShow,null);
            res = await ctx.SetAsUnseen(2, 1);
            Assert.AreEqual(StandardResults.Ok, res.Result);
            Assert.AreEqual(RequestResults.Success, res.Data.Status);
            stubUser.MethodCalled("GetCurrentUser", 1);
            stubShow.MethodCalled("GetCurrentShow", 1);

        }

        [TestMethod]
        public async Task EpisodeServiceTest_RateEpisode()
        {
            var user = Ioc.Container.Resolve<IUserAccount>();
            user.Username = "amiguinho";
            user.PasswordEnc = "25713423e";

            var show = Ioc.Container.Resolve<IShow>();
            show.TvDbId = 258823;
            show.Year = 2012;
            show.Title = "Revolution";
            show.ImdbId = "tt2070791";

            var episode = Ioc.Container.Resolve<IEpisode>();
            episode.Season = 2;
            episode.Number = 1;

            var result = Ioc.Container.Resolve<IRateResult>();
            result.Status = RequestResults.Success;
            result.Type = RateTypes.Episode;
            result.Message = "Rate episode complete!";

            var ctx = new EpisodeService(null, null, null, null);
            var res = await ctx.RateEpisode(false, -1, -1);
            Assert.AreEqual(res.Result, StandardResults.Error);

            var stub = new StubIEpisodeTraktDataService()
            {
                RateEpisodeUserAccountDtoBooleanStringInt32Int32Int32Int32 = (ac, i1, s2, s3, i4, i5, i6) => Task.Run(() => null as IRateResult)
            };

            var stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => null;

            ctx = new EpisodeService(stub, stubUser, null,null);
            res = await ctx.RateEpisode(true, 2, 1);
            Assert.AreEqual(res.Result, StandardResults.Error);

            stubUser.MethodCalled("GetCurrentUser", 1);

            stub = new StubIEpisodeTraktDataService()
            {
                RateEpisodeUserAccountDtoBooleanStringInt32Int32Int32Int32 = (ac, i1, s2, s3, i4, i5, i6) => Task.Run(() => result)
            };

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;

            try
            {
                ctx = new EpisodeService(stub, stubUser, null,null);
                res = await ctx.RateEpisode(true, 2, 1);
                Assert.AreEqual(res.Result, StandardResults.Error);
                stubUser.MethodCalled("GetCurrentUser", 1);
                Assert.Fail();
            }
            catch (Exception)
            {

            }

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;

            var stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => null;

            ctx = new EpisodeService(stub, stubUser, stubShow,null);
            res = await ctx.RateEpisode(true, 2, 1);
            Assert.AreEqual(res.Result, StandardResults.Error);
            stubUser.MethodCalled("GetCurrentUser", 1);
            stubShow.MethodCalled("GetCurrentShow", 1);

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;


            stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => show;

            ctx = new EpisodeService(stub, stubUser, stubShow,null);
            res = await ctx.RateEpisode(true, 2, 1);
            Assert.AreEqual(StandardResults.Ok, res.Result);
            Assert.AreEqual(RequestResults.Success, res.Data.Status);
            stubUser.MethodCalled("GetCurrentUser", 1);
            stubShow.MethodCalled("GetCurrentShow", 1);
            Assert.AreEqual(StandardResults.Ok, res.Result);
            Assert.AreEqual(RateTypes.Episode, res.Data.Type);

        }


        [TestMethod]
        public async Task EpisodeServiceTest_CheckinEpisode()
        {
            var user = Ioc.Container.Resolve<IUserAccount>();
            user.Username = "amiguinho";
            user.PasswordEnc = "25713423e";

            var show = Ioc.Container.Resolve<IShow>();
            show.TvDbId = 258823;
            show.Year = 2012;
            show.Title = "Revolution";
            show.ImdbId = "tt2070791";

            var episode = Ioc.Container.Resolve<IEpisode>();
            episode.Season = 2;
            episode.Number = 1;

            var result = Ioc.Container.Resolve<ICheckinResult>();
            result.Status = RequestResults.Success;
            result.Message = "check in complete!";

            var ctx = new EpisodeService(null, null, null, null);
            var res = await ctx.CheckIn(-1, -1);
            Assert.AreEqual(res.Result, StandardResults.Error);

            var stub = new StubIEpisodeTraktDataService()
            {
                CheckInUserAccountDtoStringStringInt32Int32Int32Int32 = (ac, i1, s2, s3, i4, i5, i6) => Task.Run(() => null as ICheckinResult)
            };

            var stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => null;

            ctx = new EpisodeService(stub, stubUser, null,null);
            res = await ctx.CheckIn(2, 1);
            Assert.AreEqual(res.Result, StandardResults.Error);

            stubUser.MethodCalled("GetCurrentUser", 1);

            stub = new StubIEpisodeTraktDataService()
            {
                CheckInUserAccountDtoStringStringInt32Int32Int32Int32 = (ac, i1, s2, s3, i4, i5, i6) => Task.Run(() => result)
            };

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;

            try
            {
                ctx = new EpisodeService(stub, stubUser, null,null);
                res = await ctx.CheckIn(2, 1);
                Assert.AreEqual(res.Result, StandardResults.Error);
                stubUser.MethodCalled("GetCurrentUser", 1);
                Assert.Fail();
            }
            catch (Exception)
            {

            }

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;

            var stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => null;

            ctx = new EpisodeService(stub, stubUser, stubShow,null);
            res = await ctx.CheckIn(2, 1);
            Assert.AreEqual(res.Result, StandardResults.Error);
            stubUser.MethodCalled("GetCurrentUser", 1);
            stubShow.MethodCalled("GetCurrentShow", 1);

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;


            stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => show;

            ctx = new EpisodeService(stub, stubUser, stubShow,null);
            res = await ctx.CheckIn(2, 1);
            Assert.AreEqual(StandardResults.Ok, res.Result);
            Assert.AreEqual(RequestResults.Success, res.Data.Status);
            stubUser.MethodCalled("GetCurrentUser", 1);
            stubShow.MethodCalled("GetCurrentShow", 1);
            Assert.AreEqual(StandardResults.Ok, res.Result);

        }
    }
}   
