using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shiftv.Contracts.Domain.Results;

namespace Shiftv.Core.Models.Results
{
    class GenericPostResult : IGenericPostResult
    {
        public RequestResults Status { get; set; }
        public string Message { get; set; }
    }
}
