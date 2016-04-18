using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Shiftv.Contracts.Services.Crawler;

namespace Shiftv.Services.Implementation.Crawler
{
    internal class CrawlerHelper : ICrawlerHelper
    {
        public async Task<string> GetHtml(string url, bool b = false)
        {
           
            try
            {
                Debug.WriteLine(string.Format("Foi buscar: {0}", url));
                HttpClient client = new HttpClient();
                client.Timeout = b ? new TimeSpan(0,0,8) : new TimeSpan(0,0,5);
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    response.EnsureSuccessStatusCode();
                    string responseAsString = await response.Content.ReadAsStringAsync();
                    return responseAsString;
                }
            }
            catch (Exception )
            {
                return null;

            }
        }
        public async Task<string> GetHtmlUtf(string url, bool b = false)
        {

            try
            {
                Debug.WriteLine(string.Format("Foi buscar: {0}", url));
                HttpClient client = new HttpClient();
                client.Timeout = b ? new TimeSpan(0, 0, 8) : new TimeSpan(0, 0, 4);
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    response.EnsureSuccessStatusCode();
                    var raw_response = await response.Content.ReadAsByteArrayAsync();
                    var resResponse = Encoding.UTF8.GetString(raw_response, 0, raw_response.Length);
                    return resResponse;
                }
            }
            catch (Exception)
            {
                return null;

            }
        }

    
    }
}