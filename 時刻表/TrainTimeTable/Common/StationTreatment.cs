using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// 駅扱
    /// </summary>
    public enum StationTreatment
    {
        /// <summary>
        /// 不明
        /// </summary>
        [StringValue("不明")]
        None = -1,

        /// <summary>
        /// 運行なし
        /// </summary>
        [StringValue("運行なし")]
        NoService = 0,

        /// <summary>
        /// 停車
        /// </summary>
        [StringValue("停車")]
        Stop,

        /// <summary>
        /// 通過
        /// </summary>
        [StringValue("通過")]
        Passing,

        /// <summary>
        /// 経由なし
        /// </summary>
        [StringValue("経由なし")]
        NoRoute,
    }
}
