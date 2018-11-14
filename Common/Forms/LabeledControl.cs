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

namespace Common.Windows.Forms
{
    /// <summary>
    /// ラベル付きControl
    /// </summary>
    public partial class LabeledControl : UserControl
    {
        #region プロパティ実態
        /// <summary>
        /// ラベル配置位置
        /// </summary>
        private ControlPositions m_LabelPosition = ControlPositions.Left;

        /// <summary>
        /// コントロール
        /// </summary>
        private Control m_Control = null;
        #endregion

        #region プロパティ
        /// <summary>
        /// ラベル配置位置
        /// </summary>
        public ControlPositions LabelPosition
        {
            get
            {
                return this.m_LabelPosition;
            }
            set
            {
                this.m_LabelPosition = value;

                // コントロール設定
                this.SetControl();
            }
        }

        /// <summary>
        /// Label
        /// </summary>
        [Localizable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Label Label
        {
            get
            {
                return this.m_Label;
            }
            set
            {
                this.m_Label = value;
            }
        }

        /// <summary>
        /// Control
        /// </summary>
        [Localizable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        protected Control Control
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
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LabeledControl(Control control)
        {
            Trace.WriteLine("=>>>> LabeledControl::LabeledControl()");

            // 初期設定
            this.m_Control = control;

            // コントロール追加
            this.Controls.Add(control);

            // コンポーネント初期化
            InitializeComponent();

            // 初期化
            this.Initialization();

            Trace.WriteLine("<<<<= LabeledControl::LabeledControl()");
        }
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            Trace.WriteLine("=>>>> LabeledControl::Initialization()");

            // コントロール設定
            this.SetControl();

            // イベントハンドラ設定
            this.Resize += this.OnResize;
            this.m_Label.AutoSizeChanged += this.Label_OnAutoSizeChanged;
            this.m_Label.SizeChanged += this.Label_OnSizeChanged;
            this.m_Label.TextChanged += this.Label_OnTextChanged;
            this.m_Control.SizeChanged += this.Control_OnSizeChanged;

            Trace.WriteLine("<<<<= LabeledControl::Initialization()");
        }
        #endregion

        #region コントロール設定
        /// <summary>
        /// コントロール設定
        /// </summary>
        public void SetControl()
        {
            Trace.WriteLine("=>>>> LabeledControl::SetControl()");

            // リサイズイベントキャンセル
            this.Resize -= this.OnResize;

            // ラベル配置位置で分岐
            switch (this.m_LabelPosition)
            {
                case ControlPositions.Top:
                    this.SetControl_Top();
                    break;
                case ControlPositions.Bottom:
                    this.SetControl_Bottom();
                    break;
                case ControlPositions.Right:
                    this.SetControl_Right();
                    break;
                default:
                    this.SetControl_Left();
                    break;
            }

            // リサイズイベント再設定
            this.Resize += this.OnResize;

            Trace.WriteLine("<<<<= LabeledControl::SetControl()");
        }

        /// <summary>
        /// コントロール設定(上)
        /// </summary>
        private void SetControl_Top()
        {
            Trace.WriteLine("=>>>> LabeledControl::SetControl_Top()");

            // ラベル位置設定
            this.m_Label.Location = new Point(1, 1);

            // 幅決定
            int _Width = this.m_Label.Width + 2;
            if (this.m_Control.Width + 2 > _Width)
            {
                _Width = this.m_Control.Width + 2;
            }
            this.Width = _Width;

            // 高さ決定
            this.Height = this.m_Label.Height + this.m_Control.Height + 3;

            // テキストボックス位置設定
            this.m_Control.Location = new Point(1, this.m_Label.Height + 2);

            Trace.WriteLine("<<<<= LabeledControl::SetControl_Top()");
        }

        /// <summary>
        /// コントロール設定(下)
        /// </summary>
        private void SetControl_Bottom()
        {
            Trace.WriteLine("=>>>> LabeledControl::SetControl_Bottom()");

            // テキストボックス位置設定
            this.m_Control.Location = new Point(1, 1);

            // 幅決定
            int _Width = this.m_Label.Width + 2;
            if (this.m_Control.Width + 2 > _Width)
            {
                _Width = this.m_Control.Width + 2;
            }
            this.Width = _Width;

            // 高さ決定
            this.Height = this.m_Label.Height + this.m_Control.Height + 3;

            // ラベル位置設定
            this.m_Label.Location = new Point(1, this.m_Control.Height + 2);

            Trace.WriteLine("<<<<= LabeledControl::SetControl_Bottom()");
        }

        /// <summary>
        /// コントロール設定(右)
        /// </summary>
        private void SetControl_Right()
        {
            Trace.WriteLine("=>>>> LabeledControl::SetControl_Right()");

            // テキストボックス位置設定
            this.m_Control.Location = new Point(1, 1);

            // 幅決定
            this.Width = this.m_Label.Width + this.m_Control.Width + 3;

            // 高さ決定
            int _Height = this.m_Label.Height + 2;
            if (this.m_Control.Height + 2 > _Height)
            {
                _Height = this.m_Control.Height + 2;
            }
            this.Height = _Height;

            // ラベル位置設定
            this.m_Label.Location = new Point(this.m_Control.Width + 2, 1);

            Trace.WriteLine("<<<<= LabeledControl::SetControl_Right()");
        }

        /// <summary>
        /// コントロール設定(左)
        /// </summary>
        private void SetControl_Left()
        {
            Trace.WriteLine("=>>>> LabeledControl::SetControl_Left()");

            // ラベル位置設定
            this.m_Label.Location = new Point(1, 1);

            // 幅決定
            this.Width = this.m_Label.Width + this.m_Control.Width + 3;

            // 高さ決定
            int _Height = this.m_Label.Height + 2;
            if (this.m_Control.Height + 2 > _Height)
            {
                _Height = this.m_Control.Height + 2;
            }
            this.Height = _Height;

            // テキストボックス位置設定
            this.m_Control.Location = new Point(this.m_Label.Width + 2, 1);

            Trace.WriteLine("<<<<= LabeledControl::SetControl_Left()");
        }
        #endregion

        #region イベント
        #region リサイズ
        /// <summary>
        /// リサイズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResize(object sender, EventArgs e)
        {
            Trace.WriteLine("=>>>> LabeledControl::OnResize(object, EventArgs)");

            // ラベル配置位置で分岐
            switch (this.m_LabelPosition)
            {
                case ControlPositions.Top:
                    this.OnResize_Top(sender, e);
                    break;
                case ControlPositions.Bottom:
                    this.OnResize_Bottom(sender, e);
                    break;
                case ControlPositions.Right:
                    this.OnResize_Right(sender, e);
                    break;
                default:
                    this.OnResize_Left(sender, e);
                    break;
            }

            Trace.WriteLine("<<<<= LabeledControl::OnResize(object, EventArgs)");
        }

        /// <summary>
        /// リサイズ(上)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResize_Top(object sender, EventArgs e)
        {
            Trace.WriteLine("=>>>> LabeledControl::OnResize_Top(object, EventArgs)");

            // ラベル位置設定
            this.m_Label.Location = new Point(1, 1);

            // テキストボックス位置設定
            this.m_Control.Location = new Point(1, this.m_Label.Height + 2);

            // テキストボックスサイズ設定
            this.m_Control.Size = new Size(this.Width - 2, this.Height - this.m_Label.Height - 3);

            Trace.WriteLine("<<<<= LabeledControl::OnResize_Top(object, EventArgs)");
        }

        /// <summary>
        /// リサイズ(下)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResize_Bottom(object sender, EventArgs e)
        {
            Trace.WriteLine("=>>>> LabeledControl::OnResize_Bottom(object, EventArgs)");

            // テキストボックス位置設定
            this.m_Control.Location = new Point(1, 1);

            // テキストボックスサイズ設定
            this.m_Control.Size = new Size(this.Width - 2, this.Height - this.m_Label.Height - 3);

            // ラベル位置設定
            this.m_Label.Location = new Point(1, this.m_Control.Height + 2);

            Trace.WriteLine("<<<<= LabeledControl::OnResize_Bottom(object, EventArgs)");
        }

        /// <summary>
        /// リサイズ(右)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResize_Right(object sender, EventArgs e)
        {
            Trace.WriteLine("=>>>> LabeledControl::OnResize_Right(object, EventArgs)");

            // テキストボックス位置設定
            this.m_Control.Location = new Point(1, 1);

            // テキストボックスサイズ設定
            this.m_Control.Size = new Size(this.Width - this.m_Label.Width - 2, this.Height - 2);

            // ラベル位置設定
            this.m_Label.Location = new Point(this.m_Control.Width + 2, 1);

            Trace.WriteLine("<<<<= LabeledControl::OnResize_Right(object, EventArgs)");
        }

        /// <summary>
        /// リサイズ(左)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResize_Left(object sender, EventArgs e)
        {
            Trace.WriteLine("=>>>> LabeledTextBox::OnResize_Left(object, EventArgs)");

            // ラベル位置設定
            this.m_Label.Location = new Point(1, 1);

            // テキストボックス位置設定
            this.m_Control.Location = new Point(this.m_Label.Width + 2, 1);

            // テキストボックスサイズ設定
            this.m_Control.Size = new Size(this.Width - this.m_Label.Width - 2, this.Height - 2);

            Trace.WriteLine("<<<<= LabeledTextBox::OnResize_Left(object, EventArgs)");
        }
        #endregion

        #region Label
        /// <summary>
        /// Label OnTextChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_OnTextChanged(object sender, EventArgs e)
        {
            Trace.WriteLine("=>>>> LabeledControl::Label_OnTextChanged(object, EventArgs)");

            // コントロール設定
            this.SetControl();

            // リサイズイベントキャンセル
            this.Resize -= this.OnResize;

            // リサイズイベント再設定
            this.Resize += this.OnResize;

            Trace.WriteLine("<<<<= LabeledControl::Label_OnTextChanged(object, EventArgs)");
        }

        /// <summary>
        /// Label OnAutoSizeChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_OnAutoSizeChanged(object sender, EventArgs e)
        {
            Trace.WriteLine("=>>>> LabeledControl::Label_OnAutoSizeChanged(object, EventArgs)");

            // コントロール設定
            this.SetControl();

            Trace.WriteLine("<<<<= LabeledControl::Label_OnAutoSizeChanged(object, EventArgs)");
        }

        /// <summary>
        /// Label OnSizeChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_OnSizeChanged(object sender, EventArgs e)
        {
            Trace.WriteLine("=>>>> LabeledControl::Label_OnSizeChanged(object, EventArgs)");

            // コントロール設定
            this.SetControl();

            Trace.WriteLine("<<<<= LabeledControl::Label_OnSizeChanged(object, EventArgs)");
        }
        #endregion

        #region Control
        /// <summary>
        /// Control OnSizeChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_OnSizeChanged(object sender, EventArgs e)
        {
            Trace.WriteLine("=>>>> LabeledControl::Control_OnSizeChanged(object, EventArgs)");

            // コントロール設定
            this.SetControl();

            Trace.WriteLine("<<<<= LabeledControl::Control_OnSizeChanged(object, EventArgs)");
        }
        #endregion
        #endregion
    }
}
