using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Property;
using static log4net.Appender.ColoredConsoleAppender;
using TrainTimeTable.Component;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// ListBoxFontクラス
    /// </summary>
    public class ListBoxFont : ListBox
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>

        public ListBoxFont(FontProperties properties)
        {
            Items.Clear();
            foreach (var property in properties)
            {
                Items.Add(property.Key);
            }
        }
    }
}
