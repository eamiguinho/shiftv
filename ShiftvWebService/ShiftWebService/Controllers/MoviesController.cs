using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OSDBnet;
using ShiftWebService.Models;

namespace ShiftWebService.Controllers
{
    public class MoviesController : ApiController
    {
        public IHttpActionResult GetMovie(string id, string param1)
        {
            var show = new Show();
            show.Subtitles = new List<SubtitlesInfo>();
            var x = Osdb.Login("ShiftvApp");
            var idCorrect = id.Replace("tt", "");
            var res = x.SearchSubtitlesFromImdb(param1.Replace("+", ","), idCorrect);
            foreach (var subtitle in res)
            {
                show.Subtitles.Add(new SubtitlesInfo { SubtitlesLink = subtitle.SubTitleDownloadLink.OriginalString, Language = subtitle.LanguageName, LanguageId = subtitle.LanguageId, SubtitleFileName = subtitle.SubtitleFileName });
            }
            return Ok(show);
        }   
    }
}
