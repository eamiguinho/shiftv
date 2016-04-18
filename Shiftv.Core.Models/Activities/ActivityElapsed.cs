using Shiftv.Contracts.Domain.Activity;

namespace Shiftv.Core.Models.Activities
{
    public class ActivityElapsed : IActivityElapsed
    {
        public string Full { get; set; }
        public string Short { get; set; }
    }
}