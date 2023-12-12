using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.EventArgs
{
    /// <summary>
    /// FontNameSelectedIndexChangedEventArgsクラス
    /// </summary>
    public class FontNameSelectedIndexChangedEventArgs : System.EventArgs
    {
        public FontFamily FontFamily { get; set; } = null;
    }
}
