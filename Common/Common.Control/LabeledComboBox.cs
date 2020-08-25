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

namespace Common.Control
{
    /// <summary>
    /// ラベル付きComboBox
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [Designer("System.Windows.Forms.Design.UserControlDocumentDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IRootDesigner))]
    [Designer("System.Windows.Forms.Design.ControlDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [DesignerCategory("LabeledControl")]
    public class LabeledComboBox : LabeledControl
    {
        #region プロパティ
        /// <summary>
        /// TextBox
        /// </summary>
        [Localizable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ComboBox ComboBox
        {
            get
            {
                return (ComboBox)this.Control;
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
        public LabeledComboBox()
            : base(new ComboBox())
        {
            Trace.WriteLine("=>>>> LabeledComboBox::LabeledComboBox()");

            // コンポーネント初期化
            InitializeComponent();

            // 初期化
            this.Initialization();

            Trace.WriteLine("<<<<= LabeledComboBox::LabeledComboBox()");
        }
        #endregion

        #region 初期化
        /// <summary>
        /// コンポーネント初期化
        /// </summary>
        private void InitializeComponent()
        {
            Trace.WriteLine("=>>>> LabeledComboBox::InitializeComponent()");

            Trace.WriteLine("<<<<= LabeledComboBox::InitializeComponent()");
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            Trace.WriteLine("=>>>> LabeledComboBox::Initialization()");

            // コントロール設定
            base.SetControl();

            Trace.WriteLine("<<<<= LabeledComboBox::Initialization()");
        }
        #endregion
    }
}