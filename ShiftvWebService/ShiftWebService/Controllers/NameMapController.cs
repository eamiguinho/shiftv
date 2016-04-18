using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using OSDBnet;
using ShiftWebService.Models;

namespace ShiftWebService.Controllers
{
    public class NameMapController : ApiController
    {
        public IHttpActionResult GetNameMap(string id, string param1)
        {
            var mapped = new List<NameMap>();
            mapped.Add(new NameMap
            {
                ImdbId = "tt2098220",
                SourceCrawler = "animerealm",
                NameMapped = "Hunter x Hunter 2011"
            });
            mapped.Add(new NameMap
            {
                ImdbId = "tt1409055",
                SourceCrawler = "animerealm",
                NameMapped = "Dragon Ball Kai 2014",
                IsCountRestart = true,
                RestartAt = 99
            });
            mapped.Add(new NameMap
            {
                ImdbId = "tt1409055",
                SourceCrawler = "animerealm",
                NameMapped = "Dragon Ball Z Kai"
            });
            mapped.Add(new NameMap
            {
                ImdbId = "tt1528406",
                SourceCrawler = "animerealm",
                NameMapped = "Fairy Tail 2014",
                IsCountRestart = true,
                RestartAt = 176
            });
            mapped.Add(new NameMap
            {
                ImdbId = "tt1528406",
                SourceCrawler = "animetwist",
                NameMapped = "Fairy Tail 2014",
                IsCountRestart = true,
                RestartAt = 176
            });
            mapped.Add(new NameMap
            {
                ImdbId = "tt2256334",
                SourceCrawler = "animetwist",
                NameMapped = "swordartonlines2",
                IsCountRestart = true,
                RestartAt = 26
            }); 
            mapped.Add(new NameMap
            {
                ImdbId = "tt2256334",
                SourceCrawler = "animerealm",
                NameMapped = "Sword Art Online 2",
                IsCountRestart = true,
                RestartAt = 26
            });
            mapped.Add(new NameMap
            {
                ImdbId = "tt0213338",
                SourceCrawler = "animetwist",
                NameMapped = "cowbowbebopdub"
            });    
            mapped.Add(new NameMap
            {
                ImdbId = "tt3742982",
                SourceCrawler = "animetwist",
                NameMapped = "akamegakill"
            }); 
            mapped.Add(new NameMap
            {
                ImdbId = "tt3124992",
                SourceCrawler = "animerealm",
                NameMapped = "Bishoujo Senshi Sailor Moon Crystal"
            });  
            mapped.Add(new NameMap
            {
                ImdbId = "tt1355642",
                SourceCrawler = "animetwist",
                NameMapped = "fmabrotherhood"
            });  
            mapped.Add(new NameMap
            {
                ImdbId = "tt1355642",
                SourceCrawler = "animerealm",
                NameMapped = "Fullmetal Alchemist Brotherhood"
            });  
            
            mapped.Add(new NameMap
            {
                ImdbId = "tt0807832",
                SourceCrawler = "animetwist",
                NameMapped = "mushishis1"
            });      
            mapped.Add(new NameMap
            {
                ImdbId = "tt3105422",
                SourceCrawler = "animerealm",
                NameMapped = "Diamond no Ace"
            });

            mapped.Add(new NameMap
            {
                ImdbId = "tt1843230",
                SourceCrawler = "series-cravings",
                NameMapped = "Upon Time"
            });      
            
            mapped.Add(new NameMap
            {
                ImdbId = "tt2364582",
                SourceCrawler = "series-cravings",
                NameMapped = "marvels-agents-s-h-e-l-d"
            });

            if (mapped.Any(x => x.ImdbId == id && x.SourceCrawler == param1))
            {
                return Ok(mapped.Where(x => x.ImdbId == id && x.SourceCrawler == param1));
            }
            else
            {
                return NotFound();
            }
        }   
    }


    public class NameMap
    {
        public string ImdbId { get; set; }
        public string SourceCrawler { get; set; }
        public string NameMapped { get; set; }
        public bool IsCountRestart { get; set; }
        public int RestartAt { get; set; }
    }
}
