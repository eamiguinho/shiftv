using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Core.Models.Shows
{
    class Airs : IAirs
    {
        public string Day { get; set; }
        public string Time { get; set; }
        public string Timezone { get; set; }
    }
}