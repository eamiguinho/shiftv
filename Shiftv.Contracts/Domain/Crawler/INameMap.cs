using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shiftv.Contracts.Domain.Crawler
{
    public interface INameMap
    {
        string ImdbId { get; set; }
        string SourceCrawler { get; set; }
        string NameMapped { get; set; }
        bool IsCountRestart { get; set; }
        int RestartAt { get; set; }
    }
}
