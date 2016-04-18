using System;
using Windows.Networking.BackgroundTransfer;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.DataModel
{
    public class DownloadEpisodeStatus : ViewModelBase
    {
        private double _percentage;
        private bool _isInternetDown;
        private bool _isSelected;
        private bool _isPausedByUser;

        public DownloadEpisodeStatus(IEpisode episode, DownloadOperation download)
        {
            Episode = new EpisodeDataModel(episode);
            try
            {
                Percentage = download.Progress.BytesReceived * 100 / download.Progress.TotalBytesToReceive;
            }
            catch (Exception)
            {
                Percentage = 0;
            }
            DownloadId = download.Guid;
            IsInternetDown = download.Progress.Status == BackgroundTransferStatus.PausedNoNetwork;
            IsPauseByUser = download.Progress.Status == BackgroundTransferStatus.PausedByApplication;
        }

        public EpisodeDataModel Episode { get; set; }
        public double Percentage { get { return _percentage; } set { SetProperty(ref _percentage, value); OnPropertyChanged("PercentageAsString"); OnPropertyChanged("IsInternetDown"); } }
        public string PercentageAsString { get { return string.Format("{0}%", Percentage); } }
        public Guid DownloadId { get; set; }

        public bool IsInternetDown { get { return _isInternetDown; } set { SetProperty(ref _isInternetDown, value); } }
        public bool IsPauseByUser { get { return _isPausedByUser; } set { SetProperty(ref _isPausedByUser, value); } }


        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }
    }
}