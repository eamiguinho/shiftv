using Shiftv.Contracts.Domain.Activity;

namespace Shiftv.Core.Models.Activities
{
    class ActivityWhen : IActivityWhen
    {
        public string Day { get; set; }
        public string Time { get; set; }
    }
}