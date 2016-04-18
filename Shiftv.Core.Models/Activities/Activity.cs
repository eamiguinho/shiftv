using System.Collections.Generic;
using Shiftv.Contracts.Domain.Activity;

namespace Shiftv.Core.Models.Activities
{
    public class Activity : IActivity
    {
        public IActivityTimestamps Timestamps { get; set; }
        public List<IActivityItem> ActivityItems { get; set; }
    }
}