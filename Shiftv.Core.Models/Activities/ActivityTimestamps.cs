using Shiftv.Contracts.Domain.Activity;

namespace Shiftv.Core.Models.Activities
{
    class ActivityTimestamps : IActivityTimestamps
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int Current { get; set; }
    }
}