using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Property;

namespace TrainTimeTable.EventArgs
{
    /// <summary>
    /// StationPropertyUpdateEventArgsクラス
    /// </summary>
    public class StationPropertyUpdateEventArgs : System.EventArgs
    {
        /// <summary>
        /// 更新プロパティ
        /// </summary>
        public StationProperty Property { get; set; } = null;
    }
}
