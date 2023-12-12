using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// 時刻形式
    /// </summary>
    public enum TimeFormat
    {
        /// <summary>
        /// 不明
        /// </summary>
        [StringValue("不明")]
        None = 0,

        /// <summary>
        /// 発時刻
        /// </summary>
        [StringValue("発時刻")]
        DepartureTime,

        /// <summary>
        /// 発着
        /// </summary>
        [StringValue("発着")]
        DepartureAndArrival,

        /// <summary>
        /// 下り着時刻
        /// </summary>
        [StringValue("下り着時刻")]
        OutboundArrivalTime,

        /// <summary>
        /// 上り着時刻
        /// </summary>
        [StringValue("上り着時刻")]
        InboundArrivalTime,

        /// <summary>
        /// 下り発着
        /// </summary>
        [StringValue("下り発着")]
        OutboundArrivalAndDeparture,

        /// <summary>
        /// 上り発着
        /// </summary>
        [StringValue("上り発着")]
        InboundDepartureAndArrival,
    }
}
