using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shiftv.Contracts.Domain.Crawler;

namespace Shiftv.Core.Models.Crawler
{
    class NameMap : INameMap    
    {
        public string ImdbId { get; set; }
        public string SourceCrawler { get; set; }
        public string NameMapped { get; set; }
        public bool IsCountRestart { get; set; }
        public int RestartAt { get; set; }
    }
}
