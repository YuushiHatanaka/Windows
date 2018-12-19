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
    /// 自動スクロールラベル
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [Designer("System.Windows.Forms.Design.UserControlDocumentDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IRootDesigner))]
    [Designer("System.Windows.Forms.Design.ControlDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [DesignerCategory("AutoScrollControl")]
    public class AutoScrollLabel : AutoScrollControl
    {
        #region プロパティ
        /// <summary>
        /// TextBox
        /// </summary>
        [Localizable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Label Label
        {
            get
            {
                return (Label)this.Control;
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
        public AutoScrollLabel()
            : base(new Label())
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
            Trace.WriteLine("=>>>> AutoScrollLabel::InitializeComponent()");

            Trace.WriteLine("<<<<= AutoScrollLabel::InitializeComponent()");
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            Trace.WriteLine("=>>>> AutoScrollLabel::Initialization()");

            Trace.WriteLine("<<<<= AutoScrollLabel::Initialization()");
        }
        #endregion
    }
}
