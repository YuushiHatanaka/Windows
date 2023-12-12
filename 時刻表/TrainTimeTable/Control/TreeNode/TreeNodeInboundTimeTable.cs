using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// TreeNodeInboundTimeTableクラス
    /// </summary>
    public class TreeNodeInboundTimeTable : TreeNode
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TreeNodeInboundTimeTable()
            : base("上り時刻表")
        {
        }
    }
}
