using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// TreeNodeOutboundTimetableクラス
    /// </summary>
    public class TreeNodeOutboundTimetable : TreeNode
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TreeNodeOutboundTimetable()
            : base("下り時刻表")
        {
        }
    }
}
