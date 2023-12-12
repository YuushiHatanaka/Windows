using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Property;

namespace TrainTimeTable.EventArgs
{
    /// <summary>
    /// TrainTypePropertyUpdateEventArgsクラス
    /// </summary>
    public class TrainTypePropertyUpdateEventArgs : System.EventArgs
    {
        /// <summary>
        /// 更新プロパティ
        /// </summary>
        public TrainTypeProperty Property { get; set; } = null;
    }
}
