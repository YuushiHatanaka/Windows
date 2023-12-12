using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Common;

namespace TrainTimeTable.EventArgs
{
    public class FontStyleSelectedIndexChangedEventArgs : System.EventArgs
    {
        public FontStyle Style { get; set; } = Const.DefaultFontStyle;
    }
}
