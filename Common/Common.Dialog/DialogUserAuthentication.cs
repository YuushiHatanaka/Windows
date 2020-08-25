using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.Dialog
{
    /// <summary>
    /// ユーザ認証ダイアログクラス
    /// </summary>
    public partial class DialogUserAuthentication : Dialog
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DialogUserAuthentication()
            : base()
        {
            // コンポーネント初期化
            InitializeComponent();
        }
    }
}
