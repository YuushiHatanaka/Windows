using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Property;

namespace TrainTimeTable.EventArgs
{
    /// <summary>
    /// StationPropertiesUpdateEventArgsクラス
    /// </summary>
    public class StationPropertiesUpdateEventArgs : System.EventArgs
    {
        /// <summary>
        /// 旧駅名
        /// </summary>
        public string OldStationName { get; set; } = string.Empty;

        /// <summary>
        /// 新駅名
        /// </summary>
        public string NewStationName { get; set; } = string.Empty;

        /// <summary>
        /// 旧StationPropertiesオブジェクト
        /// </summary>
        public StationProperties OldProperties { get; set; } = new StationProperties();

        /// <summary>
        /// StationPropertiesオブジェクト
        /// </summary>
        public StationProperties Properties { get; set; } = null;

        /// <summary>
        /// StationSequencePropertiesオブジェクト
        /// </summary>
        public StationSequenceProperties Sequences { get; set; } = null;
    }
}
