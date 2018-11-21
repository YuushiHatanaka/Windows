using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Windows.Dialog;

namespace ServiceOperation
{
    /// <summary>
    /// ユーザ認証ダイアログクラス
    /// </summary>
    public partial class dialogUserAuthentication : Dialog
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public dialogUserAuthentication()
            : base()
        {
            // コンポーネント初期化
            InitializeComponent();
        }
    }
}
