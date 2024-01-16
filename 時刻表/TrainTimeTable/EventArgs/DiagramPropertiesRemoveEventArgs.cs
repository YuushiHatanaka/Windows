using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Property;

namespace TrainTimeTable.EventArgs
{
    /// <summary>
    /// DiagramPropertiesRemoveEventArgsクラス
    /// </summary>
    public class DiagramPropertiesRemoveEventArgs
    {
        /// <summary>
        /// DiagramPropertiesオブジェクト
        /// </summary>
        public DiagramProperties Properties { get; set; } = null;
    }
}
