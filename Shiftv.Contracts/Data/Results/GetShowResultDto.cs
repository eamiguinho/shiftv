using System.Collections.Generic;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Domain.Results;

namespace Shiftv.Contracts.Data.Results
{
    public class GetShowResultDto : ResultBase
    {
        public List<ShowDto> ListData { get; set; }
        public ShowDto Data { get; set; }

        public static GetShowResultDto Ok(List<ShowDto> calendar)
        {
            return new GetShowResultDto { Result = Results.Ok, ListData = calendar };
        }

        public static GetShowResultDto Ok(ShowDto calendar)
        {
            return new GetShowResultDto { Result = Results.Ok, Data = calendar };
        }

        public static GetShowResultDto Error()
        {
            return new GetShowResultDto { Result = Results.Error };
        }
    }
}