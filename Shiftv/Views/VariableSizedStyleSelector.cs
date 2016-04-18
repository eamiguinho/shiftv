using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Shiftv.DataModel;

namespace Shiftv.Views
{
    public class VariableSizedStyleSelector : StyleSelector
    {
        public Style NormalStyle { get; set; }
        public Style DoubleHeightStyle { get; set; }
        public Style BigStyle { get; set; }

        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            var show = item as MiniShowDataModel;
            var movie = item as MiniMovieDataModel;
            var tileType = show == null ? (movie == null ? TileType.Normal : movie.TileType) : show.TileType;

            switch (tileType)
            {
                case TileType.Big:
                    return BigStyle;
                case TileType.Normal:
                    return NormalStyle;
                case TileType.DoubleHeight:
                    return DoubleHeightStyle;
            }

            //default style
            return NormalStyle;
        }
    }

}