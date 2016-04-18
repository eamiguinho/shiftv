using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Shiftv.Contracts.Data.Crawler;
using Shiftv.Contracts.Domain.Crawler;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class NameMapDtoFactory
    {
        public static INameMap Create(NameMapDto nameMapDto)
        {
            var x = Ioc.Container.Resolve<INameMap>();
            x.ImdbId = nameMapDto.ImdbId;
            x.IsCountRestart = nameMapDto.IsCountRestart;
            x.NameMapped = nameMapDto.NameMapped;
            x.RestartAt = nameMapDto.RestartAt;
            x.SourceCrawler = nameMapDto.SourceCrawler;
            return x;
        }
    }
}
