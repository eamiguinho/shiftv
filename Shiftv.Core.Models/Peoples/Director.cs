using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Peoples;

namespace Shiftv.Core.Models.Peoples
{
    public class Director : IDirector
    {
        public string Name { get; set; }

        public IPeopleImage Image { get; set; }
    }
}