using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Lists.Fakes;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Movies.Fakes;
using ShiftvAPI.Infrastucture.Trakt.Implementation.Lists;
using ShiftvAPI.Infrastucture.Trakt.Implementation.Movies;

namespace ShiftvAPI.Infrastucture.Tests
{
    [TestClass]
    public class ListTraktTests
    {
        [TestMethod]
        public async Task GetListInfo()
        {
            var stub = new StubIListTraktQueryService
            {
                GetListInfoStringString = (u, i) => Task.Run(() => "https://api.trakt.tv/users/amiguinho/lists/anime")
            };
            var ctx = new ListTraktDataService(stub);
            var a = await ctx.GetListInfo("amiguinho","anime");
            Assert.IsNotNull(a);
        }

        [TestMethod]
        public async Task GetListItems()
        {
            var stub = new StubIListTraktQueryService
            {
                GetListItemsStringString = (p, i) => Task.Run(() => "https://api.trakt.tv/users/amiguinho/lists/anime/items")
            };
            var ctx = new ListTraktDataService(stub);
            var a = await ctx.GetListItems("amiguinho", "anime");
            Assert.IsNotNull(a);
        }

     
    }
}
