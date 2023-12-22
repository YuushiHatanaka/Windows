using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Property;

namespace TrainTimeTable.EventArgs
{
    /// <summary>
    /// TrainPropertyUpdateEventArgsクラス
    /// </summary>
    public class TrainPropertyUpdateEventArgs : System.EventArgs
    {
        /// <summary>
        /// TrainPropertyオブジェクト
        /// </summary>
        public TrainProperty Property { get; set; } = null;
    }
}
