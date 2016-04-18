using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml.Data;
using Windows.Web.Http;
using Microsoft.Media.WebVTT;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services;
using Shiftv.DataModel;
using HttpStatusCode = Windows.Web.Http.HttpStatusCode;

namespace Shiftv.Helpers
{
    public static class ShiftvHelpers
    {
        public static DateTime GetDateTimeFromUtcTicks(long utcOffsetTicks)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date = epoch.AddSeconds(utcOffsetTicks);
            return date.ToLocalTime();
        }

        public static string IntToString(int val)
        {
            return val < 1000 ? val.ToString("D") : ((double)val / 1000).ToString("0.#k");
        }

        public static string LargeTextPlaceHolder
        {
            get { return "LOADING DATA"; }
        }

        public static string TextPlaceHolder
        {
            get { return "LOADING DATA"; }
        }

        public static string NumberPlaceHolder
        {
            get { return "?"; }
        }

        public static string UriImagePlaceHolder
        {
            get { return "ms-appx:///Assets/background.jpg"; }
        }

        public static string UriAvatarPlaceHolder
        {
            get { return "ms-appx:///Assets/avatar-large.jpg"; }
        }


        public static async Task<List<ILinkInfo>> CheckFileSizesAndQuality(IEnumerable<ILinkInfo> res, int runtime)
        {
            var newList = new List<ILinkInfo>();
            var streamLinks = res as IList<ILinkInfo> ?? res.ToList();
            foreach (var linkInfo in streamLinks)
            {
           
                    linkInfo.Velocity = GetVelocityByWebsite(string.IsNullOrEmpty(linkInfo.EmbbedLink) ? linkInfo.StreamLink : linkInfo.EmbbedLink);
                    linkInfo.ReportLink =
                        GetReportByWebsite(string.IsNullOrEmpty(linkInfo.EmbbedLink)
                            ? linkInfo.StreamLink
                            : linkInfo.EmbbedLink);
                    newList.Add(linkInfo);
                
            }
            var linksFiltered = new List<ILinkInfo>();
            foreach (var streamLink in newList.OrderBy(x => x.Velocity))
            {
                try
                {
                    Debug.WriteLine("testing " + streamLink.StreamLink);
                    HttpClient client = new HttpClient();
                    var cancellationTokenSource = new CancellationTokenSource(7000); //timeout
                    try
                    {
                        using (var request = new HttpRequestMessage()
                        {
                            RequestUri = new Uri(streamLink.StreamLink),
                            Method = HttpMethod.Head
                        })
                        {
                            using (
                                var response =
                                    await
                                        client.SendRequestAsync(request, HttpCompletionOption.ResponseHeadersRead)
                                            .AsTask(cancellationTokenSource.Token))
                            {
                                var restatus = response.StatusCode == HttpStatusCode.Ok;
                                if (restatus)
                                {
                                    var contentLength = response.Content.Headers.ContentLength;
                                    CalculateStreamQuality(contentLength, streamLink, runtime);
                                }
                                else
                                {
                                    streamLink.Quality = StreamQuality.ND;
                                }
                            }
                        }
                    }
                    catch (TaskCanceledException ex)
                    {
                        Random r = new Random();
                        long rInt = r.Next(291222420, 324522480); //for ints
                        if (streamLink.StreamLink.Contains("77.81.98.147"))
                        {
                            Debug.WriteLine("tried vidzi.tv " + streamLink.StreamLink);
                            CalculateStreamQuality((ulong?)rInt, streamLink, runtime);
                        }
                        else if (streamLink.StreamLink.Contains("vodlocker"))
                        {
                            rInt = r.Next(191222420, 224522480); //for ints
                            Debug.WriteLine("tried vodlocker " + streamLink.StreamLink);
                            CalculateStreamQuality((ulong?)rInt, streamLink, runtime);
                        }
                        else streamLink.Quality = StreamQuality.ND;
                    }

                }
                catch (Exception)
                {
                    CalculateStreamQuality(260, streamLink, runtime);
                }
                linksFiltered.Add(streamLink);
                //if (linksFiltered.Count > 8 && linksFiltered.Count(x => x.Quality == StreamQuality.MD) > 5)
                //{
                //    return linksFiltered;
                //}
            }
            return linksFiltered;
        }

        private static void CalculateStreamQuality(ulong? filesizeKb, ILinkInfo streamLink, int runtime)
        {
            if (filesizeKb == null)
            {
                streamLink.Quality = StreamQuality.ND;
                return;
            }
            var filesize = ConvertBytesToMegabytes(filesizeKb);
            if (filesize < 10)
            {
                streamLink.Quality = StreamQuality.ND;
                return;
            }
            if (runtime < 40)
            {
                if (filesize < 100) streamLink.Quality = StreamQuality.SD;
                else if (filesize >= 100 && filesize <= 260) streamLink.Quality = StreamQuality.MD;
                else streamLink.Quality = StreamQuality.HD;
            }
            else if (runtime >= 40 && runtime <= 70)
            {
                if (filesize < 200) streamLink.Quality = StreamQuality.SD;
                else if (filesize >= 200 && filesize <= 400) streamLink.Quality = StreamQuality.MD;
                else streamLink.Quality = StreamQuality.HD;
            }
            else
            {
                if (filesize < 400) streamLink.Quality = StreamQuality.SD;
                else if (filesize >= 400 && filesize <= 800) streamLink.Quality = StreamQuality.MD;
                else streamLink.Quality = StreamQuality.HD;
            }
            streamLink.FileSizeFormatted = Math.Round(filesize, 2) + " MB";
            streamLink.FileSize = Math.Round(filesize, 2);
            //streamLink.Velocity = GetVelocityByWebsite(streamLink.EmbbedLink);
        }



        private static StreamVelocity GetVelocityByWebsite(string streamLink)
        {
            if (streamLink.Contains("allmyvideos"))
            {
                return StreamVelocity.Slow;
            }
            if (streamLink.Contains("exashare"))
            {
                return StreamVelocity.Fast;
            }
            if (streamLink.Contains("fileshow"))
            {
                return StreamVelocity.Fast;
            }
            if (streamLink.Contains("vidzi"))
            {
                return StreamVelocity.Fast;
            }
            if (streamLink.Contains("filehoot"))
            {
                return StreamVelocity.Fast;
            }
            if (streamLink.Contains("cinemalive"))
            {
                return StreamVelocity.Fast;
            } if (streamLink.Contains("videomega"))
            {
                return StreamVelocity.Fast;
            }
            if (streamLink.Contains("vidspot"))
            {
                return StreamVelocity.Slow;
            }
            if (streamLink.Contains("vodlocker"))
            {
                return StreamVelocity.Slow;
            }
            if (streamLink.Contains("moviesmaze"))
            {
                return StreamVelocity.Fast;
            }
            if (streamLink.Contains("videowood"))
            {
                return StreamVelocity.Fast;
            }
            if (streamLink.Contains("openload"))
            {
                return StreamVelocity.Fast;
            }
            if (streamLink.Contains("thevideo"))
            {
                return StreamVelocity.Fast;
            }
            return StreamVelocity.Normal;
        }

        private static string GetReportByWebsite(string streamLink)
        {
            if (streamLink.Contains("allmyvideos"))
            {
                return "dmca@allmyvideos.net";
            }
            if (streamLink.Contains("exashare"))
            {
                return "abuse@exashare.com";
            }

            if (streamLink.Contains("vidzi"))
            {
                return "abuse@vidzi.tv";
            }
            if (streamLink.Contains("filehoot"))
            {
                return "abuse@filehoot.com";
            }
            if (streamLink.Contains("cinemalive"))
            {
                return "cinemalive@icloud.com";
            } if (streamLink.Contains("videomega"))
            {
                return null;
            }
            if (streamLink.Contains("vidspot"))
            {
                return "support@vidspot.net";
            }
            if (streamLink.Contains("vodlocker"))
            {
                return "dmca@VodLocker.com";
            }
            return null;
        }

        private static double ConvertBytesToMegabytes(ulong? bytes)
        {
            if (bytes == null) return 0;
            return (bytes.Value / 1024f) / 1024f;
        }

        public static async Task<string> ConvertAndUpload(StorageFile sFilePath)
        {
            var test = "";
            Stream originalFileStream = await sFilePath.OpenStreamForReadAsync();
            //const int offsetTicks = 0;
            //const int nOffsetDirection = 0;
            //var ci1 = new CultureInfo("pt-PT");
            using (var strReader = new StreamReader(originalFileStream, Encoding.GetEncoding("Windows-1252")))
            {
                var ignoreUntilNextCue = false;
                var rgxDialogNumber = new Regex(@"^\d+$");
                var rgxTimeFrame = new Regex(@"(\d\d:\d\d:\d\d,\d\d\d) --> (\d\d:\d\d:\d\d,\d\d\d)");
                var rgxTimeFrameError = new Regex(@"(\d\d:\d\d:\d\d,\d\d\d)--> (\d\d:\d\d:\d\d,\d\d\d)");
                //Write mandatory starting line for the WebVTT file
                //strWriter.WriteLine("WEBVTT FILE");
                //strWriter.WriteLine("");
                test += "WEBVTT FILE\r\n\r\n";
                //Handle each line of the SRT file
                string sLine;
                while ((sLine = strReader.ReadLine()) != null)
                {
                    //We only care about lines that aren't just an integer (aka ignore dialog id number lines)
                    if (rgxDialogNumber.IsMatch(sLine))
                        continue;

                    //If the line is a time frame line, reformat and output the time frame
                    Match myMatch = rgxTimeFrameError.Match(sLine);
                    if (myMatch.Success)
                    {
                        sLine = sLine.Replace("-->", " -->");
                    }
                    Match match = rgxTimeFrame.Match(sLine);
                    if (match.Success)
                    {
                        ignoreUntilNextCue = false;
                        sLine = sLine.Replace(',', '.'); //Simply replace the comma in the time with a period
                        var slines = Regex.Split(sLine, " --> ");
                        var date1 = DateTime.Parse(slines[0]);
                        var date2 = DateTime.Parse(slines[1]);
                        if (date1 == date2)
                        {
                            date2 = date2.AddMilliseconds(2);
                            sLine = slines[0] + " --> " + date2.ToString(@"hh\:mm\:ss\.fff");
                        }
                    }
                    else
                    {
                        Regex reg = new Regex(@"^(\d\d:\d\d)");
                        if (reg.IsMatch(sLine))
                        {
                            ignoreUntilNextCue = true;
                        }
                        sLine = Regex.Replace(sLine, "<.*?>", string.Empty);
                    }

                    //  strWriter.WriteLine(sLine); //Write out the line
                    if (!ignoreUntilNextCue) test += sLine + "\r\n";
                    else
                    {
                        test += "\r\n";
                    }
                }
                return test;
                //   return await Upload(fileN);
            }
        }

        //private static async Task<Uri> Upload(StorageFile fileN)
        //{

        //    CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials("shiftv", "UO8e3zOGo/ue7WY4xrnFBs/Jnu9U7tagSduFr+sMvchTt5JmjYSABShG++zWIGA5ImcwFaC/0V/+7ku4rTedQQ=="), false);

        //    // Create the blob client.
        //    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

        //    // Retrieve reference to a previously created container.
        //    CloudBlobContainer container = blobClient.GetContainerReference("shiftvdata");

        //    // Retrieve reference to a blob named "myblob".
        //    CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileN.Name);


        //    await blockBlob.UploadFromFileAsync(fileN);
        //    return blockBlob.StorageUri.PrimaryUri;
        //}



        public static string SHA1Converter(string original)
        {
            if (string.IsNullOrEmpty(original)) return null;
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(original, BinaryStringEncoding.Utf8);
            HashAlgorithmProvider hashAlgorithm = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);
            IBuffer hashBuffer = hashAlgorithm.HashData(buffer);

            var strHashBase64 = CryptographicBuffer.EncodeToHexString(hashBuffer);
            return strHashBase64;
        }

        public static string GetTranslation(string key)
        {
            var local = new LocalizedStrings();
            return local[key];
        }

        public static string GetTimeZone()
        {
            var s = TimeZoneInfo.Local.DisplayName;
            string output = s.Split(new char[] { '(', ')' })[1];
            return output;
        }

        public static string Capitalize(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        private static IAsyncOperation<IUICommand> messageDialogCommand = null;
        public async static Task<bool> ShowDialog(MessageDialog dlg)
        {

            // Close the previous one out
            if (messageDialogCommand != null)
            {
                messageDialogCommand.Cancel();
                messageDialogCommand = null;
            }

            messageDialogCommand = dlg.ShowAsync();
            await messageDialogCommand;
            return true;
        }

        public static bool CheckSubtitle(string subWtt)
        {
            try
            {
                var x = WebVTTParser.ParseDocument(subWtt, new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 0));
                return x != null;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public static string Percentage(double round)
        {
            return string.Format("{0}%", round*100);
        }
    }

    public class LocalizedStrings
    {
        public string this[string key]
        {
            get
            {
                try
                {
                    var data = key.Split(Convert.ToChar("_"));
                    var res = ResourceLoader.GetForViewIndependentUse().GetString(data[0]);
                    if (string.IsNullOrEmpty(res))
                    {
                        throw new Exception(data[0] + " dont exist in dictionary");
                    }
                    switch (data.Count())
                    {
                        case 1:
                            return res.ToLower();
                        case 2:
                            if (data[1] == "Upper") return res.ToUpper();
                            if (data[1] == "DoubleDots") return string.Format("{0}:", res.ToLower());
                            if (data[1] == "Bracket") return string.Format("({0})", res.ToLower());
                            if (data[1] == "Capital") return string.Format("{0}", ShiftvHelpers.Capitalize(res));
                            return res.ToLower();
                        case 3:
                            if (data[1] == "Upper" && data[2] == "DoubleDots")
                            {
                                return string.Format("{0}:", res.ToUpper());
                            }
                            if (data[1] == "Upper" && data[2] == "Bracket")
                            {
                                return string.Format("({0})", res.ToUpper());
                            }
                            return res.ToLower();
                    }
                    return res.ToLower();
                }
                catch (Exception)
                {
                    return ResourceLoader.GetForViewIndependentUse().GetString(key);
                }
            }
        }
    }



    public class MultiObservableCollection<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        public MultiObservableCollection(IEnumerable<T> partialCollecion, IEnumerable<T> fullCollection)
            : base(partialCollecion)
        {
            HasMoreItems = true;
            _fullCollection = fullCollection.ToList();
            CollectionNumber = 10;
        }

        public bool HasMoreItems { get; set; }
        private bool _isRunning = false;
        private readonly List<T> _fullCollection;
        private int CollectionNumber { get; set; }
        private const int PageSize = 10;

        public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            if (_isRunning) //not thread safe
            {
                return null;
            }
            _isRunning = true;

            if (_fullCollection.Count() < CollectionNumber)
            {
                HasMoreItems = false;
                //return null;
            }
            return AsyncInfo.Run(async c =>
            {
                for (int i = CollectionNumber; i < CollectionNumber + PageSize; i++)
                {
                    if (i < _fullCollection.Count())
                        Add(_fullCollection[i]);
                }
                CollectionNumber += PageSize;

                HasMoreItems = _fullCollection.Count() > CollectionNumber;
                _isRunning = false;
                return new LoadMoreItemsResult()
                {
                    Count = (uint)CollectionNumber
                };

            });
        }
    }
    public static class EnumerableExtension
    {
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
    }
}
