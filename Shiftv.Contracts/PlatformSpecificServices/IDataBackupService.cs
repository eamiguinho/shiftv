        using System;
        using System.Threading.Tasks;

namespace Shiftv.Contracts.PlatformSpecificServices
{
    public interface IDataBackupService
    {
        void SaveFileToAzure(string jsonData, string fileName, BackupContainerTypes container, bool isToFastCache);
        Task<string> GetFileFromAzure(string fileName, BackupContainerTypes containerType);
        void SaveLocalFile(string jsonData, string fileName);
        Task<string> ReadLocalFile(string fileName);
        void SaveFileDirectToAzure(string json, string fileName, BackupContainerTypes type);
        Task<string> GetFileFromAzureWithTime(string fileName, BackupContainerTypes containerType, TimeSpan maxDateFile);
    }

    public enum BackupContainerTypes
    {
        GlobalData,
        Movies,
        TvShows,
        Seasons,
        Episodes,
        UserSpecificData,
        Subtitles,
        StreamData
    }
}   
