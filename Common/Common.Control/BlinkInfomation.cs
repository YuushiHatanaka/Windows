using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace Common.Control
{
    /// <summary>
    /// 点滅情報クラス
    /// </summary>
    public class BlinkInfomation
    {
        /// <summary>
        /// 前面色
        /// </summary>
        public Color ForeColor = SystemColors.ControlText;

        /// <summary>
        /// 背景色
        /// </summary>
        public Color BackColor = SystemColors.Control;

        /// <summary>
        /// 外観指定
        /// </summary>
        public BlinkAppearance FlatAppearance = new BlinkAppearance();

        /// <summary>
        /// 描写スタイル
        /// </summary>
        public FlatStyle FlatStyle = FlatStyle.Standard;

        /// <summary>
        /// Visial Style使用フラグ
        /// </summary>
        public bool UseVisualStyleBackColor = true;
    }
}
