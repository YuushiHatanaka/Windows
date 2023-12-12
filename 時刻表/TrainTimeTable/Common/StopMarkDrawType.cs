using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// 停車駅明示
    /// </summary>
    public enum StopMarkDrawType
    {
        /// <summary>
        /// 不明
        /// </summary>
        [StringValue("不明")]
        None = 0,

        /// <summary>
        /// 明示しない
        /// </summary>
        [StringValue("明示しない")]
        Nothing,

        /// <summary>
        /// 停車駅を明示
        /// </summary>
        [StringValue("停車駅を明示")]
        DrawOnStop,
    }
}
