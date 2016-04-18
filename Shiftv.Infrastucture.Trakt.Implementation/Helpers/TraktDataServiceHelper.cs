using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.JsonTrakt;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.PlatformSpecificServices;

namespace Shiftv.Infrastucture.Trakt.Implementation.Helpers
{
    public class TraktDataServiceHelper
    {
        public static HttpClient GetHttpClient(int? i = null)
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip |
                                                 DecompressionMethods.Deflate;
            }
            var timeout = i ?? 10;
            return new HttpClient(handler) { Timeout = new TimeSpan(0, 5, 0) };
        }
        private static HttpClient GetHttpClientV2()
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip |
                                                 DecompressionMethods.Deflate;
            }
            var client = new HttpClient(handler) { Timeout = new TimeSpan(0, 0, 5) };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("trakt-api-key", "185527aca7982be00bf0ff2b9f481d31be539e096c1451a1fd174b211948af19");
            client.DefaultRequestHeaders.Add("trakt-api-version", "2");
            return client;
        }

        //public static async Task<T> GetObjectWithCredentials<T>(string url, string username, string password) 
        //{
        //    var loginReq = new LoginRequestJsonDto { Username = username.Trim(), Password = password };
        //    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(loginReq));
        //    var httpClient = GetHttpClient();
        //    var responseBodyAsText = await httpClient.PostAsync(url, myContent);
        //    if (responseBodyAsText.IsSuccessStatusCode)
        //    {
        //        var res = await responseBodyAsText.Content.ReadAsStringAsync();
        //        var objectReceived = JsonConvert.DeserializeObject<T>(res);
        //        return objectReceived;
        //    }
        //    return default(T);
        //}

        public static async Task<T> GetObjectWithCredentialsToken<T>(string url, string token)
        {
            var httpClient = GetHttpClient();
            httpClient.DefaultRequestHeaders.Add("authorization", string.Format("Bearer {0}", token));
            var responseBodyAsText = await httpClient.GetStringAsync(url);
            var objectReceived = JsonConvert.DeserializeObject<T>(responseBodyAsText);
            return objectReceived;
        }

        public static async Task<T> GetObjectWithCredentials<T>(string url, UserTokenDto token)
        {
            if (token == null) return await GetObjectWithoutCredentials<T>(url);
            return await GetObjectWithCredentialsToken<T>(url, token.AccessToken);
        }



        public static async Task<T> GetObjectWithoutCredentials<T>(string url, bool isCritical = false)
        {
            var httpClient = isCritical ? GetHttpClient(60) : GetHttpClient();
            var responseBodyAsText = await httpClient.GetStringAsync(url);
            var objectReceived = JsonConvert.DeserializeObject<T>(responseBodyAsText);
            return objectReceived;
        }

        public static async Task<T> GetObjectWithoutCredentialsV2<T>(string url)
        {
            var httpClient = GetHttpClientV2();
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, "relativeAddress");

            var responseBodyAsText = await httpClient.GetStringAsync(url);
            var objectReceived = JsonConvert.DeserializeObject<T>(responseBodyAsText);
            return objectReceived;
        }

        public static async Task<T> PostWithCredentials<T>(string url, HttpContent myContent, string accessToken = null)
        {
            var httpClient = GetHttpClient(30);
            httpClient.DefaultRequestHeaders.Add("authorization", string.Format("Bearer {0}", accessToken));
            var responseBodyAsText = await httpClient.PostAsync(url, myContent);
            if (responseBodyAsText.IsSuccessStatusCode)
            {
                var res = await responseBodyAsText.Content.ReadAsStringAsync();
                var objectReceived = JsonConvert.DeserializeObject<T>(res);
                return objectReceived;
            }
            return default(T);
        }

        public static async Task<HttpStatusCode> PostWithCredentials(string url, HttpContent myContent, string accessToken = null)
        {
            var httpClient = GetHttpClient(30);
            httpClient.DefaultRequestHeaders.Add("authorization", string.Format("Bearer {0}", accessToken));
            var responseBodyAsText = await httpClient.PostAsync(url, myContent);
            return responseBodyAsText.StatusCode;
        }

        public static async Task<string> GetStremString(string url)
        {
            var httpClient = GetHttpClient();
            return await httpClient.GetStringAsync(url);
        }

        public static async void SaveToAzure(object dto, string fileName, BackupContainerTypes type, IDataBackupService backupService, bool isToFastCache = false)
        {
            await Task.Run(() =>
            {
                var json = JsonConvert.SerializeObject(dto);
                backupService.SaveFileToAzure(json, fileName, type, isToFastCache);
            });
        }


        public static async void SaveToLocal(object dto, string fileName, IDataBackupService backupService)
        {
            await Task.Run(() =>
            {
                var json = JsonConvert.SerializeObject(dto);
                backupService.SaveLocalFile(json, fileName);
            });
        }

        public static async Task<T> GetObjectWithoutCredentials<T>(string url, int i)
        {
            var httpClient = GetHttpClient(i);
            var responseBodyAsText = await httpClient.GetStringAsync(url);
            var objectReceived = JsonConvert.DeserializeObject<T>(responseBodyAsText);
            return objectReceived;
        }

        public static async void SaveDirectToAzure(object dto, string fileName, BackupContainerTypes type,
            IDataBackupService backupService)
        {
            await Task.Run(() =>
            {
                var json = JsonConvert.SerializeObject(dto);
                backupService.SaveFileDirectToAzure(json, fileName, type);
            });
        }
    }
}