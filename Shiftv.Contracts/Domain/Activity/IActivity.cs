using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shiftv.Contracts.Domain.Activity
{
    public interface IActivity
    {
        IActivityTimestamps Timestamps { get; set; }
        List<IActivityItem> ActivityItems { get; set; }
    }
}
