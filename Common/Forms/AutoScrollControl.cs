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
using Common.Control;

namespace Common.Windows.Forms
{
    /// <summary>
    /// 自動スクロールControl
    /// </summary>
    public partial class AutoScrollControl : UserControl
    {
        #region プロパティ実態
        /// <summary>
        /// コントロール
        /// </summary>
        private System.Windows.Forms.Control m_Control = null;

        /// <summary>
        /// スクロールパネル
        /// </summary>
        private System.Windows.Forms.Panel m_ScrollPanel = null;
        #endregion

        #region プロパティ
        /// <summary>
        /// Control
        /// </summary>
        [Localizable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        protected System.Windows.Forms.Control Control
        {
            get
            {
                return this.m_Control;
            }
            set
            {
                this.m_Control = value;
            }
        }

        /// <summary>
        /// ScrollPanel
        /// </summary>
        [Localizable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public  System.Windows.Forms.Panel ScrollPanel
        {
            get
            {
                return this.m_ScrollPanel;
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AutoScrollControl(System.Windows.Forms.Control control)
        {
            Trace.WriteLine("=>>>> AutoScrollControl::AutoScrollControl()");

            // 初期設定
            this.m_Control = control;

            // コントロール追加
            this.Controls.Add(control);

            // コンポーネント初期化
            InitializeComponent();

            // 初期化
            this.Initialization();

            Trace.WriteLine("<<<<= AutoScrollControl::AutoScrollControl()");
        }
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            Trace.WriteLine("=>>>> AutoScrollControl::Initialization()");

            // スクロールパネル生成
            this.m_ScrollPanel = new Panel();
            this.Controls.Add(this.m_ScrollPanel);

            Trace.WriteLine("<<<<= AutoScrollControl::Initialization()");
        }
        #endregion
    }
}
