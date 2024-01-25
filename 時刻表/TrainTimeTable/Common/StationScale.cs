using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// 駅規模
    /// </summary>
    [Serializable]
    public enum StationScale
    {
        /// <summary>
        /// 不明
        /// </summary>
        [StringValue("不明")]
        None = 0,

        /// <summary>
        /// 一般駅
        /// </summary>
        [StringValue("一般駅")]
        GeneralStation,

        /// <summary>
        /// 主要駅
        /// </summary>
        [StringValue("主要駅")]
        MainStation,
    }
}
