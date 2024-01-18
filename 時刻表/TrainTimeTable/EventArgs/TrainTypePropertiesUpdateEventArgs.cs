using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Property;

namespace TrainTimeTable.EventArgs
{
    /// <summary>
    /// TrainTypePropertiesUpdateEventArgsクラス
    /// </summary>
    public class TrainTypePropertiesUpdateEventArgs : System.EventArgs
    {
        /// <summary>
        /// 旧列車種別名
        /// </summary>
        public string OldTrainTypeName { get; set; } = string.Empty;

        /// <summary>
        /// 新列車種別名
        /// </summary>
        public string NewTrainTypeName { get; set; } = string.Empty;

        /// <summary>
        /// 旧TrainTypePropertiesオブジェクト
        /// </summary>
        public TrainTypeProperties OldProperties { get; set; } = new TrainTypeProperties();

        /// <summary>
        /// TrainTypePropertiesオブジェクト
        /// </summary>
        public TrainTypeProperties Properties { get; set; } = null;

        /// <summary>
        /// TrainTypeSequencePropertiesオブジェクト
        /// </summary>
        public TrainTypeSequenceProperties Sequences { get; set; } = null;
    }
}
