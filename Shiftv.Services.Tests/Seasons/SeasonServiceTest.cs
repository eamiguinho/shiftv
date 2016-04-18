using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shiftv.Contracts.DataServices.Seasons.Fakes;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Contracts.Services.Accounts.Fakes;
using Shiftv.Contracts.Services.Shows.Fakes;
using Shiftv.Core.Models;
using Shiftv.Global;
using Shiftv.Services.Implementation.Seasons;
using Shiftv.Services.Tests.Helpers;

namespace Shiftv.Services.Tests.Seasons
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SeasonServiceTest
    {
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            CoreModelsIoc.RegisterTypes();
        }

        [TestMethod]
        public async Task SeasonServiceTest_SetSeasonAsSeen()
        {
            var user = Ioc.Container.Resolve<IUserAccount>();
            user.Username = "amiguinho";
            user.PasswordEnc = "25713423e";

            var show = Ioc.Container.Resolve<IShow>();
            show.TvDbId = 258823;
            show.Year = 2012;
            show.Title = "Revolution";
            show.ImdbId = "tt2070791";

            var postRes = Ioc.Container.Resolve<IGenericPostResult>();
            postRes.Status = RequestResults.Success;
            postRes.Message = "blabla";

            var ctx = new SeasonService(null, null, null);
            var res = await ctx.SetSeasonAsSeen(-1);
            Assert.AreEqual(res.Result, StandardResults.Error);

            var stub = new StubISeasonTraktDataService()
            {
                SetSeasonAsSeenUserAccountDtoInt32StringStringInt32Int32 = (ac, i1, s2, p2, p3, p4) => Task.Run(() => null as IGenericPostResult)
            };

            var stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => null;
            try
            {
                ctx = new SeasonService(stub, stubUser, null);
                res = await ctx.SetSeasonAsSeen(1);
                Assert.AreEqual(res.Result, StandardResults.Error);
                stubUser.MethodCalled("GetCurrentUser", 1);
            }
            catch (Exception)
            {
                
            }

            stub = new StubISeasonTraktDataService()
            {
                SetSeasonAsSeenUserAccountDtoInt32StringStringInt32Int32 = (ac, i1, s2, p2, p3, p4) => Task.Run(() => postRes)
            };

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;

            try
            {
                ctx = new SeasonService(stub, stubUser, null);
                res = await ctx.SetSeasonAsSeen(1);
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

            ctx = new SeasonService(stub, stubUser, stubShow);
            res = await ctx.SetSeasonAsSeen(1);
            Assert.AreEqual(res.Result, StandardResults.Error);
            stubUser.MethodCalled("GetCurrentUser", 1);
            stubShow.MethodCalled("GetCurrentShow", 1);

            stubUser = FakesHelper.CreateNotImplementedStub<StubIUserService>();
            stubUser.GetCurrentUser = () => user;


            stubShow = FakesHelper.CreateNotImplementedStub<StubIShowService>();
            stubShow.GetCurrentShow = () => show;

            ctx = new SeasonService(stub, stubUser, stubShow);
            res = await ctx.SetSeasonAsSeen(1);
            Assert.AreEqual(StandardResults.Ok, res.Result);
            Assert.AreEqual(RequestResults.Success, res.Data.Status);
            stubUser.MethodCalled("GetCurrentUser", 1);
            stubShow.MethodCalled("GetCurrentShow", 1);
        }
    }
}   
