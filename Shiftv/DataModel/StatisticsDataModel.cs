using Shiftv.Contracts.Domain.Stats;

namespace Shiftv.DataModel
{
    public class StatisticsDataModel
    {
        private IStatistics _model;

        public StatisticsDataModel(IStatistics model)
        {
            _model = model;
        }

        public int Loved
        {
            get { return _model.Ratings.Loved; }
        }
        public int Hated
        {
            get { return _model.Ratings.Hated; }
        }

        public int Watchers
        {
            get { return _model.Watchers; }
        }
    }
}