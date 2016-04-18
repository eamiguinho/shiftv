using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.IO.Extensions;

namespace Shiftv.Helpers
{
    public class ImageHelper
    {
   
        public static async Task<BitmapImage> GetOtherImageAsync(Uri uri)
        {
            try
            {
                if (uri == null) return new BitmapImage();
                var filename = GetFileName(uri);
                var image = new BitmapImage();
                var x = await IsFilePresent(filename, ImageType.Other);
                if (x != null && await x.GetSize() != 0)
                {
                    image.UriSource = new Uri(x.Path);
                }
                else
                {
                    var a = await DownloadFileAsync(uri, filename, ImageType.Other);
                    image.UriSource = new Uri(a);
                }
                return image;
            }
            catch (Exception)
            {
                return new BitmapImage();
            }

        }

        public static async Task<BitmapImage> GetShowImageAsync(Uri uri)
        {
            try
            {
                var filename = GetFileName(uri);
                var image = new BitmapImage();
                var x = await IsFilePresent(filename, ImageType.Show);
                if (x != null && await x.GetSize() != 0)
                {
                    image.UriSource = new Uri(x.Path);
                }
                else
                {
                    var a = await DownloadFileAsync(uri, filename, ImageType.Show);
                    image.UriSource = new Uri(a);
                }
                return image;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static async Task<BitmapImage> GetMovieImageAsync(Uri uri)
        {
            try
            {
                var filename = GetFileName(uri);
                var image = new BitmapImage();
                var x = await IsFilePresent(filename, ImageType.Movie);
                if (x != null && await x.GetSize() != 0)
                {
                    image.UriSource = new Uri(x.Path);
                }
                else
                {
                    var a = await DownloadFileAsync(uri, filename, ImageType.Movie);
                    image.UriSource = new Uri(a);
                }
                return image;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string GetFileName(Uri uri)
        {
            try
            {
                return Path.GetFileName(uri.LocalPath);
            }
            catch (Exception)
            {
                return Path.GetRandomFileName();
            }
        }

        public static async Task<BitmapImage> GetMovieImageInCache()
        {
            try
            {
                var folder = await GetFolder(ImageType.Movie, false);
                if (folder == null) return null;
                var files = await folder.GetFilesAsync();
                Random rand = new Random();
                int size = files.Count;
                var bitmap = new BitmapImage(new Uri(files[rand.Next(size)].Path));
                return bitmap;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static async Task<BitmapImage> GetShowImageInCache()
        {
            try
            {
                var folder = await GetFolder(ImageType.Show, false);
                if (folder == null) return null;
                var files = await folder.GetFilesAsync();
                Random rand = new Random();
                int size = files.Count;
                var bitmap = new BitmapImage(new Uri(files[rand.Next(size)].Path));
                return bitmap;
            }
            catch (Exception)
            {
                return null;
            }
       
        }

        public static async Task<IStorageItem> IsFilePresent(string fileName, ImageType type)
        {
            try
            {
                var folder = await GetFolder(type, false);
                var item = await folder.TryGetItemAsync(fileName);
                return item;
            }
            catch (Exception)
            {
                return null;
            }


        }

        private static async Task<StorageFolder> GetFolder(ImageType type, bool toCreate)
        {
            var folder = await ApplicationData.Current.LocalFolder.TryGetItemAsync(type.ToString());
            if (folder == null)
            {
                if (toCreate)
                {
                    return await ApplicationData.Current.LocalFolder.CreateFolderAsync(type.ToString());
                }
                return null;
            }
            if (folder.IsOfType(StorageItemTypes.Folder))
            {
                return await ApplicationData.Current.LocalFolder.GetFolderAsync(type.ToString());
            }
            return null;
        }

        public static async Task<string> DownloadFileAsync(Uri uri, string filename, ImageType type)
        {

            var folder = await GetFolder(type, true);
            using (var fileStream = await folder.OpenStreamForWriteAsync(filename, CreationCollisionOption.ReplaceExisting))
            {
                var webStream = await new HttpClient().GetStreamAsync(uri);
                await webStream.CopyToAsync(fileStream);
                webStream.Dispose();
            }
            return (await folder.GetFileAsync(filename)).Path;
        }

  
    }

    public enum ImageType
    {
        Movie,
        Show,
        Other
    }
}
