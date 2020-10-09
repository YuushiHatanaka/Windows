using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.Net
{
    /// <summary>
    /// Telnetテキストボックスクラス
    /// </summary>
    public class TelnetTextBox : TextBox
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TelnetTextBox()
            : base()
        {
            // 初期設定
            this.Multiline = true;
            this.ScrollBars = ScrollBars.Both;
        }
    }
}
