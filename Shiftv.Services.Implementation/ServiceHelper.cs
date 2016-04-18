using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shiftv.Services.Implementation
{
    public class ServiceHelper
    {
        private static bool _lastResult;
        private static DateTime _lastResultTime;

        public static async Task<bool> IsInternet()
        {
            try
            {
                if (DateTime.Now.Subtract(_lastResultTime).Seconds < 5) return _lastResult;
                const string req = "http://www.google.com";
                var httpClient = new HttpClient();
                await httpClient.GetStringAsync(req);
                _lastResult = true;
                _lastResultTime = DateTime.Now;
                return true;
            }
            catch (Exception)
            {
                _lastResult = false;
                _lastResultTime = DateTime.Now;
                return false;
            }
        }
    }
}