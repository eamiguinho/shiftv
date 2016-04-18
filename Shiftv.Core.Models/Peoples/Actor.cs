using Shiftv.Contracts.Domain.Peoples;

namespace Shiftv.Core.Models.Peoples
{
    public class Actor : IActor
    {
        public string Name { get; set; }
        public string Character { get; set; }
        public IPeopleImage Image { get; set; }
    }
}