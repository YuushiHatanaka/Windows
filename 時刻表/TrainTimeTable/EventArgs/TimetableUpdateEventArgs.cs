using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Common;
using TrainTimeTable.Property;

namespace TrainTimeTable.EventArgs
{
    public class TimetableUpdateEventArgs : System.EventArgs
    {
        /// <summary>
        /// 更新種別
        /// </summary>
        public Type UpdateType { get; set; } = default(Type);

        /// <summary>
        /// ダイヤグラム名
        /// </summary>
        public string DiagramName { get; set; } = string.Empty;

        /// <summary>
        /// 方向種別
        /// </summary>
        public DirectionType DirectionType { get; set; } = DirectionType.None;

        /// <summary>
        /// RouteFilePropertyオブジェクト
        /// </summary>
        public RouteFileProperty RouteFileProperty { get; set; } = null;

        /// <summary>
        /// 更新オブジェクト
        /// </summary>
        public object UpdateObject { get; set; } = null;
    }
}
