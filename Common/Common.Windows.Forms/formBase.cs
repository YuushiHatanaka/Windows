using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.Windows.Forms
{
    /// <summary>
    /// フォーム基底クラス
    /// </summary>
    public partial class formBase : Form
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public formBase()
        {
            // コンポーネント初期化
            InitializeComponent();
        }

        /// <summary>
        /// FormClosing イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void formClosing(object sender, FormClosingEventArgs e)
        {
            // 終了確認
            if (MessageBox.Show(
                    "アプリケーションを終了しますか？", "終了確認", 
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                // 終了キャンセル
                e.Cancel = true;
            }
        }
    }
}
