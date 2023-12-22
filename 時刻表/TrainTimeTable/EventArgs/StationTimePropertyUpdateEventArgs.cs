using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Property;

namespace TrainTimeTable.EventArgs
{
    /// <summary>
    /// StationTimePropertyUpdateEventArgsクラス
    /// </summary>
    public class StationTimePropertyUpdateEventArgs : System.EventArgs
    {
        /// <summary>
        /// StationTimePropertyオブジェクト
        /// </summary>
        public StationTimeProperty Property { get; set; } = null;
    }
}
