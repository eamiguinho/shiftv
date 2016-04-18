using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.DataModel
{
    public class ShowProgressDataModel
    {
        private IShowProgress _model;
        private IShow _show;
        private int _totalEpisodes;

        public ShowProgressDataModel(IShowProgress progress, IShow data)
        {
            _model = progress;
            _show = data;
           _totalEpisodes = _show != null ? _show.Seasons.Sum(x => x.Episodes.Count) : 0;
        }

        //public int Percentage
        //{
        //    get { return _model !=null ? _model.Percentage : 0; }
        //}

        //int Aired { get { return _model != null ? _model.Aired : _totalEpisodes; } }
        //int Completed { get { return _model != null ? _model.Completed : 0; } }
        //int Left
        //{
        //    get
        //    {
        //        return _model != null ? _model.Left : 0;
        //    }
        //}

        //public string PercentageFormated
        //{
        //    get { return string.Format("{0}%", Percentage == 0 ? "0" : Percentage.ToString()); }
        //}

        //public string LeftFormated
        //{
        //    get { return string.Format("{0}", Left == 0 ? _totalEpisodes.ToString() : Left.ToString()); }
        //}

        //public string Watched
        //{
        //    get { return string.Format("{0} / {1}", Completed == 0 ?  "0" : Completed.ToString(), Aired); }
        //}
    }
}
