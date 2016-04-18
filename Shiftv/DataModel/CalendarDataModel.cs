using System;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Calendars;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.DataModel
{
    public class CalendarDataModel : ViewModelBase
    {
        private bool _isLoadingData;
        private double _imageOpacity;

        public CalendarDataModel(IEpisode episode, DateTime date, IMiniShow show)
        {
            Episode = episode != null ? new EpisodeDataModel(episode, true, show.Fanart) : null;
            Show = show != null ? new MiniShowDataModel(show) : null;
            ImageOpacity = 1;
            Date = date;
        }

        public EpisodeDataModel Episode { get; set; }
        public MiniShowDataModel Show { get; set; }
        public DateTime Date { get; set; }

        public bool IsLoadingData
        {
            get { return _isLoadingData; }
            set
            {
                SetProperty(ref _isLoadingData, value);
                ImageOpacity = 0.5;
            }
        }

        public double ImageOpacity
        {
            get { return _imageOpacity; }
            set { SetProperty(ref _imageOpacity, value); }
        }
    }
}

