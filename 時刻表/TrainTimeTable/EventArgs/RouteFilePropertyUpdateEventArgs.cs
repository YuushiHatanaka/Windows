using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Property;

namespace TrainTimeTable.EventArgs
{
    /// <summary>
    /// RouteFilePropertyUpdateEventArgsクラス
    /// </summary>
    public class RouteFilePropertyUpdateEventArgs : System.EventArgs
    {
        /// <summary>
        /// 更新プロパティ
        /// </summary>
        public RouteFileProperty Property { get; set; } = null;
    }
}
