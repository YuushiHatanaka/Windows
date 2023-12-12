using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.EventArgs
{
    /// <summary>
    /// CommentUpdateEventArgsクラス
    /// </summary>
    public class CommentUpdateEventArgs : System.EventArgs
    {
        /// <summary>
        /// コメント文字列
        /// </summary>
        public StringBuilder Comment { get; set; } = null;
    }
}
