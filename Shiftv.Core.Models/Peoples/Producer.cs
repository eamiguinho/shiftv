using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Peoples;

namespace Shiftv.Core.Models.Peoples
{
    public class Producer : IProducer
    {
        public string Name { get; set; }
        public bool Executive { get; set; }
        public IPeopleImage Image { get; set; }
    }
}