using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// 発着種別
    /// </summary>
    public enum DepartureArrivalType
    {
        /// <summary>
        /// 不明
        /// </summary>
        [StringValue("不明")]
        None = -1,

        /// <summary>
        /// 発
        /// </summary>
        [StringValue("発")]
        Departure = 0,

        /// <summary>
        /// 着
        /// </summary>
        [StringValue("着")]
        Arrival = 1,
    }
}
