using Shiftv.Contracts.Domain.Peoples;

namespace Shiftv.Core.Models.Peoples
{
    class Art : IArt
    {
        public string Job { get; set; }
        public IPerson Person { get; set; }
    }
}