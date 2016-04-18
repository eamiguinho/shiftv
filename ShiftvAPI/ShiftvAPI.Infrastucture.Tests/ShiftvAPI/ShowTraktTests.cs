using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShiftvAPI.Infrastucture.Implementation.Shows;

namespace ShiftvAPI.Infrastucture.Tests.ShiftvAPI
{
    [TestClass]
    public class ShowShiftvTests
    {
        [TestMethod]
        public async Task GetShowById()
        {
            var ctx = new ShowShiftvDataService();
            var a = await ctx.GetShowById(161511);
            Assert.IsNotNull(a);
        }

        [TestMethod]
        public async Task SaveShow()
        {
            var ctx = new ShowShiftvDataService();
            var a = await ctx.GetShowById(161511);
            ctx.SaveShow(a);
            Assert.IsNotNull(a);
        }

    }
}
