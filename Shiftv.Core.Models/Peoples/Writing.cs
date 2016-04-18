using Shiftv.Contracts.Domain.Peoples;

namespace Shiftv.Core.Models.Peoples
{
    class Writing : IWriting
    {
        public string Job { get; set; }
        public IPerson Person { get; set; }
    }
}