using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using ShiftvAPI.Contracts.Data;

namespace ShiftvAPI.Infrastucture.Trakt.Implementation.Helpers
{
    public class TraktDataServiceHelper
    {
        private static HttpClient GetHttpClient(bool isFastForget = false)
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip |
                                                 DecompressionMethods.Deflate;
            }
            var client = new HttpClient(handler) { Timeout = !isFastForget ? new TimeSpan(0, 0, 30) : new TimeSpan(0,0,10)};
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("trakt-api-key", "233fcb9838282957f4d5b6f4fdd7d0167bb8344bcd2463eaaa9cfc4a659da9b5");
            client.DefaultRequestHeaders.Add("trakt-api-version", "2");
            return client;
        }

        public static async Task<T> GetObjectWithoutCredentials<T>(string url, bool isCritical = false)
        {
            var httpClient = GetHttpClient();
            var responseBodyAsText = await httpClient.GetStringAsync(url);
            var objectReceived = JsonConvert.DeserializeObject<T>(responseBodyAsText);
            return objectReceived;
        }

        public static async Task<T> PostObjectWithoutCredentials<T>(string url, object serializeObject)
        {
            var httpClient = GetHttpClient();
            HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(serializeObject), Encoding.UTF8,
                "application/json");

            var responseBodyAsText = await httpClient.PostAsync(url, contentPost);
            if (responseBodyAsText.IsSuccessStatusCode)
            {
                var res = await responseBodyAsText.Content.ReadAsStringAsync();
                var objectReceived = JsonConvert.DeserializeObject<T>(res);
                return objectReceived;
            }
            return default(T);
        }

        public static async Task<T> GetObjectWithCredentials<T>(string url, string accessToken)
        {
            var httpClient = GetHttpClient();
            httpClient.DefaultRequestHeaders.Add("authorization", string.Format("Bearer {0}",accessToken));
            var responseBodyAsText = await httpClient.GetStringAsync(url);
            var objectReceived = JsonConvert.DeserializeObject<T>(responseBodyAsText);
            return objectReceived;
        }

        public static async Task<Dictionary<string,List<Calendar>>> GetObjectWithCredentialsTest(string url, string accessToken)
        {
            var httpClient = GetHttpClient();
            httpClient.DefaultRequestHeaders.Add("authorization", string.Format("Bearer {0}", accessToken));
            var responseBodyAsText = await httpClient.GetStringAsync(url);
            var objectReceived = JsonConvert.DeserializeObject<Dictionary<string, List<Calendar>>>(responseBodyAsText);
            return objectReceived;
        }

        public static async Task PostObjectWithCredentials(string url, string token, string serializeObject, bool isFastForget = false)
        {
            var httpClient = GetHttpClient(isFastForget);
            httpClient.DefaultRequestHeaders.Add("authorization", string.Format("Bearer {0}", token));
            HttpContent contentPost = new StringContent(serializeObject, Encoding.UTF8,
                "application/json");

            var responseBodyAsText = await httpClient.PostAsync(url, contentPost);
            responseBodyAsText.EnsureSuccessStatusCode();
            if (responseBodyAsText.IsSuccessStatusCode)
            {
             var res =    await responseBodyAsText.Content.ReadAsStringAsync();
            }
        }
    }
}