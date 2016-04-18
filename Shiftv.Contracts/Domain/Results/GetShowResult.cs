using System.Collections.Generic;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Contracts.Domain.Results
{
    public class GetShowResult : ResultBase
    {
        public List<IShow> ListData { get; set; }
        public IShow Data { get; set; }

        public static GetShowResult Ok(List<IShow> calendar)
        {
            return new GetShowResult { Result = Results.Ok, ListData = calendar };
        }

        public static GetShowResult Ok(IShow calendar)
        {
            return new GetShowResult { Result = Results.Ok, Data = calendar };
        }

        public static GetShowResult Error()
        {
            return new GetShowResult { Result = Results.Error };
        }
    }
}