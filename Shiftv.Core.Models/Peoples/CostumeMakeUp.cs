using Shiftv.Contracts.Domain.Peoples;

namespace Shiftv.Core.Models.Peoples
{
    class CostumeMakeUp : ICostumeMakeUp
    {
        public string Job { get; set; }
        public IPerson Person { get; set; }
    }
}