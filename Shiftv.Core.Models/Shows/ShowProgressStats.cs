using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Core.Models.Shows
{
    class ShowProgressStats : IShowProgressStats
    {
        public int Percentage { get; set; }
        public int Aired { get; set; }
        public int Completed { get; set; }
        public int Left { get; set; }
    }
}