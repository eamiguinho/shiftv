using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using Shiftv.Contracts.Domain.Images;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Helpers;

namespace Shiftv.DataModel
{
    public class SeasonDataModel : ViewModelBase
    {
        private ISeason _model;
        private bool _isSelected;

        public SeasonDataModel(ISeason season, int year)
        {
            _model = season;
            ShowYear = year;
        }

        public SeasonDataModel(ISeason seasonData)
        {
            _model = seasonData;
        }

        public int ShowYear { get; set; }
        public int Number
        {
            get
            {
                return _model.Number != null ? _model.Number.Value : 0;
            }
        }
        public List<IEpisode> Episodes { get { return _model.Episodes ?? new List<IEpisode>() ; } }
        public string Poster { get { return _model.Images.Poster.Full; } }
        public IImage Image { get { return _model.Images; } }
        public int TotalWatched { get { return _model.Episodes.Count(x => x.Watched); } }
        public string TotalEpisodes { get { return string.Format("{0}", _model.EpisodeCount); } }

        public string SeasonNumber
        {
            get { return string.Format("{1} {0}", Number, ShiftvHelpers.GetTranslation("Season_Upper")); }
        }

        public string SeasonYear
        {
            get { return string.Format("{0}", ShowYear + (Number - 1)); }
        }

  

        public bool IsSeasonSeen
        {
            get { return TotalWatched == _model.Episodes.Count; }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { Set(ref _isSelected, value); }
        }
    }
}