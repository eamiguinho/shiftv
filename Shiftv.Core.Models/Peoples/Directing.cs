using Shiftv.Contracts.Domain.Peoples;

namespace Shiftv.Core.Models.Peoples
{
    class Directing : IDirecting
    {
        public string Job { get; set; }
        public IPerson Person { get; set; }
    }
}