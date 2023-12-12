using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// TreeNodeStationTimetableクラス
    /// </summary>
    public class TreeNodeStationTimetable : TreeNode
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TreeNodeStationTimetable()
            : base("駅時刻表")
        {
        }
    }
}
