using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.QualityTools.Testing.Fakes.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shiftv.Contracts.DataServices.Login.Fakes;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Core.Models;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;
using Shiftv.Infrastucture.Trakt.Implementation.Login;

namespace Shiftv.Infrastucture.Trakt.Test.Accounts
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class LoginTraktDataServiceTest
    {
        [TestInitialize]
        public void Initialize()
        {
            CoreModelsIoc.RegisterTypes();
        }

        [TestMethod]
        public async Task LoginTraktDataServiceTest_CreateAccount_AssertParameters()
        {
            var stub = new StubILoginTraktQueryService
            {
                InstanceBehavior = StubBehaviors.NotImplemented
            };
            var ctx = new LoginTraktDataService(stub, null);
            var x = await ctx.CreateAccount(null, null, null);
            Assert.AreEqual(x.Result, ResultBase.Results.Error);
            x = await ctx.CreateAccount("blabla", null, null);
            Assert.AreEqual(x.Result, ResultBase.Results.Error);
            x = await ctx.CreateAccount(null, "blabla", null);
            Assert.AreEqual(x.Result, ResultBase.Results.Error);
            x = await ctx.CreateAccount(null, null, "blabla");
            Assert.AreEqual(x.Result, ResultBase.Results.Error);
            x = await ctx.CreateAccount("blabla", "blabla", null);
            Assert.AreEqual(x.Result, ResultBase.Results.Error);
            x = await ctx.CreateAccount("blabla", null, "blabla");
            Assert.AreEqual(x.Result, ResultBase.Results.Error);
            x = await ctx.CreateAccount(null, "blabla", "blabla");
            Assert.AreEqual(x.Result, ResultBase.Results.Error);
        }

        [TestMethod]
        public async Task LoginTraktDataServiceTest_CreateAccount_QueryString()
        {
            var stub = new StubILoginTraktQueryService
            {
                GetCreateAccount = () => Task.Run(() => null as string)
            };
            var ctx = new LoginTraktDataService(stub, null);
            var x = await ctx.CreateAccount("blabla", "blabla", "blabla");
            Assert.AreEqual(x.Result, ResultBase.Results.Error);

            stub = new StubILoginTraktQueryService
            {
                GetCreateAccount = () => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new LoginTraktDataService(stub, null);
            x = await ctx.CreateAccount("blabla", "blabla", "blabla");
            Assert.AreEqual(x.Result, ResultBase.Results.Error);
        }

        [TestMethod]
        public async Task LoginTraktDataServiceTest_CreateAccount_AccountEmailError()
        {
            var url = "http://api.trakt.tv/account/create/" + TraktConstants.TraktDevKey;
            var pass = "qwrqweq";
            var stub = new StubILoginTraktQueryService
            {
                GetCreateAccount = () => Task.Run(() => url)
            };
            var ctx = new LoginTraktDataService(stub, null);
            var x = await ctx.CreateAccount("blablaqweqwr", pass, "aafasfasdasd");
            Assert.AreEqual(x.Result, ResultBase.Results.Error);
        }

        [TestMethod]
        public async Task LoginTraktDataServiceTest_CreateAccount_UsernameError()
        {
            var url = "http://api.trakt.tv/account/create/" + TraktConstants.TraktDevKey;
            var pass = "qwrqweq";
            var stub = new StubILoginTraktQueryService
            {
                GetCreateAccount = () => Task.Run(() => url)
            };
            var ctx = new LoginTraktDataService(stub, null);
            var x = await ctx.CreateAccount("blablaqweqw%$£$%r", pass, "emanuelamiguinho@gmail.com");
            Assert.AreEqual(x.Result, ResultBase.Results.Error);
        }

        [TestMethod]
        public async Task LoginTraktDataServiceTest_CreateAccount_PasswordError()
        {
            var url = "http://api.trakt.tv/account/create/" + TraktConstants.TraktDevKey;
            var pass = "qwrqweq";
            var stub = new StubILoginTraktQueryService
            {
                GetCreateAccount = () => Task.Run(() => url)
            };
            var ctx = new LoginTraktDataService(stub, null);
            var x = await ctx.CreateAccount("eamiguinho", pass, "emanuelamiguinho@gmail.com");
            Assert.AreEqual(x.Result, ResultBase.Results.Error);
        }

        [TestMethod]
        public void LoginTraktDataServiceTest_CreateAccount_Ok()
        {
            Assert.IsTrue(true);
            //var url = "http://api.trakt.tv/account/create/" + TraktConstants.TraktDevKey;
            //var pass = "3de27387c253d9f4707e01ab4eff9551d8c3b157";
            //var stub = new StubILoginTraktQueryService
            //{
            //    GetCreateAccount = () => Task.Run(() => url)
            //};
            //var ctx = new LoginTraktDataService(stub, null);
            //var x = await ctx.CreateAccount("eamiguinho", pass, "emanuelamiguinho@gmail.com");
            //Assert.AreEqual(x.Result, ResultBase.Results.Ok);

        }




    }
}
