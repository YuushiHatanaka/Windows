using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Common;
using TrainTimeTable.Property;

namespace TrainTimeTable.EventArgs
{
    /// <summary>
    /// TrainPropertiesUpdateEventArgsクラス
    /// </summary>
    public class TrainPropertiesUpdateEventArgs : System.EventArgs
    {
        /// <summary>
        /// DirectionTypeオブジェクト
        /// </summary>
        public DirectionType DirectionType { get; set; } = DirectionType.None;

        /// <summary>
        /// TrainPropertiesオブジェクト
        /// </summary>
        public TrainProperties Properties { get; set; } = null;

        /// <summary>
        /// TrainSequencePropertiesオブジェクト
        /// </summary>
        public TrainSequenceProperties Sequences { get; set; } = null;
    }
}
