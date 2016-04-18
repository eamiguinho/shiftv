using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shiftv.DataModel
{
    public class EpisodeViewerDataModel
    {
        public EpisodeViewerDataModel(SeasonDataModel season, int episode)
        {
            Season = season;
            SelectedEpisode = episode;
        }
        public SeasonDataModel Season { get; set; }
        public int SelectedEpisode { get; set; }
    }
    

    public class EpisodeViewerDataModelMini
    {
        public EpisodeViewerDataModelMini(int season, int episode, bool isEpisode = true, bool isLinkDownloaded = false, bool isNextOrPrevious = false)
        {
            Season = season;
            SelectedEpisode = episode;
            IsLinkDownloaded = isLinkDownloaded;
            IsEpisode = isEpisode;
            IsNextOrPrevious = isNextOrPrevious;
        }
        public int Season { get; set; }
        public int SelectedEpisode { get; set; }

        public bool IsLinkDownloaded { get; set; }
        public bool IsEpisode { get; set; }

        public bool IsNextOrPrevious { get; set; }

    }
   
}
