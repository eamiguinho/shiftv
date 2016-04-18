using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices.Categories.Fakes;
using Shiftv.Core.Models;
using Shiftv.Infrastucture.Trakt.Implementation.Categories;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Test.Categories
{
    [TestClass]
    public class CategoryTraktDataServiceTest
    {
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            CoreModelsIoc.RegisterTypes();
        }

        [TestMethod]
        public async Task CategoryTraktDataServiceTest_GetAllForMovies_QueryString()
        {
            var ctx = new CategoryTraktDataService(null);
            var x = await ctx.GetAllForMovies();
            Assert.IsNull(x);

            var stub = new StubICategoryTraktQueryService
            {
                GetAllForMovies = () => Task.Run(() => null as string)
            };

            var u = new UserTokenDto();
            u.AccessToken = "tt";

            ctx = new CategoryTraktDataService(stub);
            x = await ctx.GetAllForMovies();
            Assert.IsNull(x);

            stub = new StubICategoryTraktQueryService
            {
                GetAllForMovies = () => Task.Run(() => "asagsdfsgdsqweqw")
            };
            ctx = new CategoryTraktDataService(stub);
            x = await ctx.GetAllForMovies();
            Assert.IsNull(x);
        }

        [TestMethod]
        public async Task CategoryTraktDataServiceTest_GetAllForMovies_DataTest()
        {
            var url = "http://api.trakt.tv/genres/movies.json/" + TraktConstants.TraktKey;
            var ctx = new CategoryTraktDataService(null);
            var x = await ctx.GetAllForMovies();
            Assert.IsNull(x);

            var stub = new StubICategoryTraktQueryService
            {
                GetAllForMovies = () => Task.Run(() => url)
            };

            var u = new UserTokenDto();
            u.AccessToken = "tt";


            ctx = new CategoryTraktDataService(stub);
            x = await ctx.GetAllForMovies();
            Assert.IsNotNull(x);
            Assert.AreNotEqual(0, x.Count);
        }
    }
}
