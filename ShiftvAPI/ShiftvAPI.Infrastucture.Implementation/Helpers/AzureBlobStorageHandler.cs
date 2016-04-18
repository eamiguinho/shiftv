using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace ShiftvAPI.Infrastucture.Implementation.Helpers
{
    public static class AzureBlobStorageHandler
    {
        public async static Task<string> SaveFileToAzure(string jsonData, string fileName, BackupContainerTypes containerType)
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

                if (x == null) return null;
                if (await x.ExistsAsync())
                {
                    await x.FetchAttributesAsync();
                }
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);
                var blockBlob = container.GetBlockBlobReference(fileName);
                await blockBlob.UploadFromByteArrayAsync(byteArray, 0, byteArray.Length);
                return fileName;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static async Task<string> GetFileFromAzure(string fileName, BackupContainerTypes containerType)
        {
            try
            {
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

    }
}