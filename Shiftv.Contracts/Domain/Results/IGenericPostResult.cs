using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shiftv.Contracts.Domain.Results
{
    public interface IGenericPostResult
    {
        RequestResults Status { get; set; }
        string Message { get; set; }
    }
}
