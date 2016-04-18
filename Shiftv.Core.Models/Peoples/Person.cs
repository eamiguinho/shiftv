using Shiftv.Contracts.Domain.Images;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Peoples;

namespace Shiftv.Core.Models.Peoples
{
    class Person : IPerson
    {
        public string Name { get; set; }
        public IIds Ids { get; set; }
        public IImage Images { get; set; }
        public string Biography { get; set; }
        public string Birthday { get; set; }
        public string Death { get; set; }
        public string Birthplace { get; set; }
        public string Homepage { get; set; }
    }
}