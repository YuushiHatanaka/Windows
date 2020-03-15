using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace Common.Windows.Forms
{
    /// <summary>
    /// ラベル付きTextBox
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [Designer("System.Windows.Forms.Design.UserControlDocumentDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IRootDesigner))]
    [Designer("System.Windows.Forms.Design.ControlDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [DesignerCategory("LabeledControl")]
    public class LabeledTextBox : LabeledControl
    {
        #region プロパティ
        /// <summary>
        /// TextBox
        /// </summary>
        [Localizable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TextBox TextBox
        {
            get
            {
                return (TextBox)this.Control;
            }
            set
            {
                this.Control = value;
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LabeledTextBox()
            : base(new TextBox())
        {
            Trace.WriteLine("=>>>> LabeledTextBox::LabeledTextBox()");

            // コンポーネント初期化
            InitializeComponent();

            // 初期化
            this.Initialization();

            Trace.WriteLine("<<<<= LabeledTextBox::LabeledTextBox()");
        }
        #endregion

        #region 初期化
        /// <summary>
        /// コンポーネント初期化
        /// </summary>
        private void InitializeComponent()
        {
            Trace.WriteLine("=>>>> LabeledTextBox::InitializeComponent()");

            Trace.WriteLine("<<<<= LabeledTextBox::InitializeComponent()");
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            Trace.WriteLine("=>>>> LabeledTextBox::Initialization()");

            // コントロール設定
            base.SetControl();

            // イベントハンドラ設定
            this.TextBox.MultilineChanged += this.TextBox_OnMultilineChanged;

            Trace.WriteLine("<<<<= LabeledTextBox::Initialization()");
        }
        #endregion

        #region イベント
        #region TextBox
        /// <summary>
        /// TextBox OnMultilineChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_OnMultilineChanged(object sender, EventArgs e)
        {
            Trace.WriteLine("=>>>> LabeledTextBox::TextBox_OnMultilineChanged(object, EventArgs)");

            // コントロール設定
            base.SetControl();

            Trace.WriteLine("<<<<= LabeledTextBox::TextBox_OnMultilineChanged(object, EventArgs)");
        }
        #endregion
        #endregion
    }
}