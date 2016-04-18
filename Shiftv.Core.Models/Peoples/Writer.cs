using Newtonsoft.Json;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Peoples;
using Shiftv.Global;

namespace Shiftv.Core.Models.Peoples
{
    public class Writer : IWriter
    {
        public string Name { get; set; }
        public string Job { get; set; }
        public IPeopleImage Image { get; set; }
    }
}