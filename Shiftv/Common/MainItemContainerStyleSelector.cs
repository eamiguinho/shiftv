using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Shiftv.Common
{
    public class MainItemContainerStyleSelector : StyleSelector
    {
        public Style NoSelectStyle { get; set; }


        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            return NoSelectStyle;
        }
    }
}
