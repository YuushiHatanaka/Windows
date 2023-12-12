using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.Dialog
{
    /// <summary>
    /// ProgressInfoクラス
    /// </summary>
    public class ProgressInfo
    {
        /// <summary>
        /// プログレスバー表示位置
        /// </summary>
        public int Position = 0;

        /// <summary>
        /// 表示メッセージ
        /// </summary>
        public string Message = string.Empty;

        /// <summary>
        /// 処理結果
        /// </summary>
        public DialogResult Result = DialogResult.None;
    }
}
