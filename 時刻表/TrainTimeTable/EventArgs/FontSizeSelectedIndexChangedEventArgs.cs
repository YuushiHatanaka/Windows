using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Common;

namespace TrainTimeTable.EventArgs
{
    public class FontSizeSelectedIndexChangedEventArgs : System.EventArgs
    {
        public float Size { get; set; } = Const.DefaultFontSize;
    }
}
