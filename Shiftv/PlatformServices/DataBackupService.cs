using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Shiftv.Contracts.PlatformSpecificServices;
using Buffer = Windows.Storage.Streams.Buffer;

namespace Shiftv.PlatformServices
{
    class DataBackupService : IDataBackupService
    {
        public async void SaveFileToAzure(string jsonData, string fileName, BackupContainerTypes containerType, bool isToFastCache)
        {
            try
            {
                return;
                fileName = fileName + ".json";
                var storageAccount = new CloudStorageAccount(new StorageCredentials("shiftv", "UO8e3zOGo/ue7WY4xrnFBs/Jnu9U7tagSduFr+sMvchTt5JmjYSABShG++zWIGA5ImcwFaC/0V/+7ku4rTedQQ=="), false);
                // Create the blob client.
                var blobClient = storageAccount.CreateCloudBlobClient();
                // Retrieve reference to a previously created container.
                var container = blobClient.GetContainerReference(containerType.ToString().ToLower());
                await container.CreateIfNotExistsAsync();
                var x = container.GetBlockBlobReference(fileName);

                if (x == null) return;
                if (await x.ExistsAsync())
                {
                    await x.FetchAttributesAsync();
                }
                var ts = new TimeSpan(1,0,0,0);
                if (isToFastCache)
                {
                    ts = new TimeSpan(0, 1, 0, 0);
                }
                if (x.Properties.LastModified == null || x.Properties.LastModified != null && x.Properties.LastModified.Value.ToUniversalTime() < DateTime.Now.Subtract(ts).ToUniversalTime())
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);
                    //byte[] byteArray = Encoding.ASCII.GetBytes(contents);

                    // convert stream to string

                    var blockBlob = container.GetBlockBlobReference(fileName);
                    await blockBlob.UploadFromByteArrayAsync(byteArray, 0, byteArray.Length);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }


        public async void SaveLocalFile(string jsonData, string fileName)
        {
            try
            {
                fileName = fileName + ".json";
                var folder = ApplicationData.Current.LocalFolder;
                using (var fileStream = await folder.OpenStreamForWriteAsync(fileName, CreationCollisionOption.ReplaceExisting))
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);
                    await fileStream.WriteAsync(byteArray, 0, byteArray.Length);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public async Task<string> ReadLocalFile(string fileName)
        {
            try
            {
                fileName = fileName + ".json";
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile localData = await storageFolder.GetFileAsync(fileName);
                return await FileIO.ReadTextAsync(localData);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async void SaveFileDirectToAzure(string json, string fileName, BackupContainerTypes containerType)
        {
            try
            {
                fileName = fileName + ".json";
                var storageAccount = new CloudStorageAccount(new StorageCredentials("shiftv", "UO8e3zOGo/ue7WY4xrnFBs/Jnu9U7tagSduFr+sMvchTt5JmjYSABShG++zWIGA5ImcwFaC/0V/+7ku4rTedQQ=="), false);
                // Create the blob client.
                var blobClient = storageAccount.CreateCloudBlobClient();
                // Retrieve reference to a previously created container.
                var container = blobClient.GetContainerReference(containerType.ToString().ToLower());
                await container.CreateIfNotExistsAsync();
                var x = container.GetBlockBlobReference(fileName);

                if (x == null) return;
                if (await x.ExistsAsync())
                {
                    await x.FetchAttributesAsync();
                }

                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                //byte[] byteArray = Encoding.ASCII.GetBytes(contents);

                // convert stream to string

                var blockBlob = container.GetBlockBlobReference(fileName);
                await blockBlob.UploadFromByteArrayAsync(byteArray, 0, byteArray.Length);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }


        public async Task<string> GetFileFromAzure(string fileName, BackupContainerTypes containerType)
        {
            return null;
            try
            {
                fileName = fileName + ".json";
                var storageAccount = new CloudStorageAccount(new StorageCredentials("shiftv", "UO8e3zOGo/ue7WY4xrnFBs/Jnu9U7tagSduFr+sMvchTt5JmjYSABShG++zWIGA5ImcwFaC/0V/+7ku4rTedQQ=="), false);
                // Create the blob client.
                var blobClient = storageAccount.CreateCloudBlobClient();
                // Retrieve reference to a previously created container.
                var container = blobClient.GetContainerReference(containerType.ToString().ToLower());
                await container.CreateIfNotExistsAsync();
                var x = container.GetBlockBlobReference(fileName);
                if (await x.ExistsAsync())
                {
                    if (x.Properties.LastModified == null ||
                        x.Properties.LastModified != null &&
                        x.Properties.LastModified.Value.ToUniversalTime().AddDays(10) > DateTime.Now.ToUniversalTime())
                    {
                        await x.FetchAttributesAsync();
                        var a = new byte[x.Properties.Length];
                        await x.DownloadToByteArrayAsync(a, 0);
                        var text = Encoding.UTF8.GetString(a, 0, a.Length);
                        return text;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<string> GetFileFromAzureWithTime(string fileName, BackupContainerTypes containerType, TimeSpan maxDateFile)
        {
            try
            {
                fileName = fileName + ".json";
                var storageAccount = new CloudStorageAccount(new StorageCredentials("shiftv", "UO8e3zOGo/ue7WY4xrnFBs/Jnu9U7tagSduFr+sMvchTt5JmjYSABShG++zWIGA5ImcwFaC/0V/+7ku4rTedQQ=="), false);
                // Create the blob client.
                var blobClient = storageAccount.CreateCloudBlobClient();
                // Retrieve reference to a previously created container.
                var container = blobClient.GetContainerReference(containerType.ToString().ToLower());
                await container.CreateIfNotExistsAsync();
                var x = container.GetBlockBlobReference(fileName);
                if (await x.ExistsAsync())
                {
                    await x.FetchAttributesAsync();
                    if (x.Properties.LastModified != null && x.Properties.LastModified.Value.ToUniversalTime().Add(maxDateFile) >
                        DateTime.Now.ToUniversalTime())
                    {
                        var a = new byte[x.Properties.Length];
                        await x.DownloadToByteArrayAsync(a, 0);
                        var text = Encoding.UTF8.GetString(a, 0, a.Length);
                        return text;
                    }
                    return null;
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
