using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// 方向種別
    /// </summary>
    public enum DirectionType
    {
        /// <summary>
        /// 不明
        /// </summary>
        [StringValue("不明")]
        None = -1,

        /// <summary>
        /// 下り
        /// </summary>
        [StringValue("下り")]
        Outbound = 0,

        /// <summary>
        /// 上り
        /// </summary>
        [StringValue("上り")]
        Inbound = 1,
    }
}
