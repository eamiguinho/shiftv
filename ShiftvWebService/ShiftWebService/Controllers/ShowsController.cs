using System;
using System.Collections.Generic;
using System.Web.Http;
using OSDBnet;
using ShiftWebService.Models;

namespace ShiftWebService.Controllers
{
    public class ShowsController : ApiController
    {
        public IHttpActionResult GetShow(string id, string param1, string param2, string param3)
        {
            try
            {
                var show = new Show();
                show.Subtitles = new List<SubtitlesInfo>();
                var x = Osdb.Login("ShiftvApp");
                var idCorrect = id.Replace("tt", "");
                var res = x.SearchSubtitlesTsShowFromImdb(param3.Replace("+", ","), idCorrect, param1, param2, "");
                foreach (var subtitle in res)
                {
                    show.Subtitles.Add(new SubtitlesInfo{ SubtitlesLink = subtitle.SubTitleDownloadLink.OriginalString, Language = subtitle.LanguageName, LanguageId = subtitle.LanguageId, SubtitleFileName = subtitle.SubtitleFileName});
                }
                return Ok(show);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + Environment.NewLine + e.StackTrace);
            }
        }   
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }
}
