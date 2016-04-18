using Shiftv.Common;

namespace Shiftv.ViewModels.Shows.Player
{
    public class FastEpisodeViewerViewModel : ViewModelBase
    {
        private string _episodeTitle;
        private bool _isTopBarEnabled;


        public string EpisodeTitle
        {
           
            get { return _episodeTitle; }
            set { SetProperty(ref _episodeTitle, value); }
        }

        public bool IsTopBarEnabled
        {
            get { return _isTopBarEnabled; }
            set { SetProperty(ref _isTopBarEnabled, value); }
        }
    }
}
