using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Property;
using TrainTimeTable.Component;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// ListBoxColorクラス
    /// </summary>
    public class ListBoxColor : ListBox
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public ListBoxColor(ColorProperties properties)
        {
            Items.Clear();
            foreach (var property in properties)
            {
                Items.Add(property.Key);
            }
        }
    }
}
