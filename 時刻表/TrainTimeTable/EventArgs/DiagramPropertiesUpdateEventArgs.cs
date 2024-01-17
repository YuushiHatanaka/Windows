using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Property;

namespace TrainTimeTable.EventArgs
{
    /// <summary>
    /// DiagramPropertiesUpdateEventArgsクラス
    /// </summary>
    public class DiagramPropertiesUpdateEventArgs : System.EventArgs
    {
        /// <summary>
        /// DiagramPropertiesオブジェクト
        /// </summary>
        public DiagramSequenceProperties Sequences { get; set; } = null;

        /// <summary>
        /// DiagramPropertiesオブジェクト
        /// </summary>
        public DiagramProperties Properties { get; set; } = null;
    }
}
