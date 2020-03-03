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
        private System.Windows.Forms.Control m_Control = null;

        /// <summary>
        /// Controlマージン
        /// </summary>
        private ControlMargin m_ControlMargin = new ControlMargin(1, 1, 1, 1);

        /// <summary>
        /// Controlセパレート
        /// </summary>
        private int m_SeparateSize = 1;
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
        /// Controlマージン
        /// </summary>
        //[Localizable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlMargin ControlMargin
        {
            get
            {
                return this.m_ControlMargin;
            }
            set
            {
                this.m_ControlMargin = value;

                // コントロール設定
                this.SetControl();
            }
        }

        /// <summary>
        /// Controlセパレート
        /// </summary>
        public int SeparateSize
        {
            get
            {
                return this.m_SeparateSize;
            }
            set
            {
                this.m_SeparateSize = value;

                // コントロール設定
                this.SetControl();
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LabeledControl(System.Windows.Forms.Control control)
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
            this.m_Label.Location = new Point(this.ControlMargin.Left, this.ControlMargin.Top);

            // 幅決定
            int _Width = this.m_Control.Width;
            if (this.m_Label.Width > this.m_Control.Width)
            {
                _Width = this.m_Label.Width;
            }
            this.Width = this.ControlMargin.Left + _Width +  this.ControlMargin.Right;

            // 高さ決定
            this.Height = this.m_ControlMargin.Top + this.m_Label.Height + this.m_SeparateSize + this.m_Control.Height +  this.m_ControlMargin.Bottom;

            // コントロール位置設定
            this.m_Control.Location = new Point(this.ControlMargin.Left, this.ControlMargin.Top + this.m_Label.Height + this.m_SeparateSize);

            Trace.WriteLine("<<<<= LabeledControl::SetControl_Top()");
        }

        /// <summary>
        /// コントロール設定(下)
        /// </summary>
        private void SetControl_Bottom()
        {
            Trace.WriteLine("=>>>> LabeledControl::SetControl_Bottom()");

            // コントロール位置設定
            this.m_Control.Location = new Point(this.ControlMargin.Left, this.ControlMargin.Top);

            // 幅決定
            int _Width = this.m_Control.Width;
            if (this.m_Label.Width > this.m_Control.Width)
            {
                _Width = this.m_Label.Width;
            }
            this.Width = this.ControlMargin.Left + _Width +  this.ControlMargin.Right;

            // 高さ決定
            this.Height = this.m_ControlMargin.Top + this.m_Control.Height + this.m_SeparateSize + this.m_Label.Height + this.m_ControlMargin.Bottom;

            // ラベル位置設定
            this.m_Label.Location = new Point(this.ControlMargin.Left, this.ControlMargin.Top + this.m_Control.Height + this.m_SeparateSize);

            Trace.WriteLine("<<<<= LabeledControl::SetControl_Bottom()");
        }

        /// <summary>
        /// コントロール設定(右)
        /// </summary>
        private void SetControl_Right()
        {
            Trace.WriteLine("=>>>> LabeledControl::SetControl_Right()");

            // コントロール位置設定
            this.m_Control.Location = new Point(this.ControlMargin.Left, this.ControlMargin.Top);

            // 幅決定
            this.Width = this.m_ControlMargin.Left + this.m_Control.Width + this.m_SeparateSize + this.m_Label.Width + this.m_ControlMargin.Right;

            // 高さ決定
            int _Height = this.m_Control.Height;
            if (this.m_Label.Height > this.m_Control.Height)
            {
                _Height = this.m_Label.Height;
            }
            this.Height = this.ControlMargin.Top + _Height +  this.ControlMargin.Bottom;

            // ラベル位置設定
            this.m_Label.Location = new Point(this.ControlMargin.Left + this.m_Control.Width + this.m_SeparateSize, this.ControlMargin.Top);

            Trace.WriteLine("<<<<= LabeledControl::SetControl_Right()");
        }

        /// <summary>
        /// コントロール設定(左)
        /// </summary>
        private void SetControl_Left()
        {
            Trace.WriteLine("=>>>> LabeledControl::SetControl_Left()");

            // ラベル位置設定
            this.m_Label.Location = new Point(this.ControlMargin.Left, this.ControlMargin.Top);

            // 幅決定
            this.Width = this.m_ControlMargin.Left + this.m_Label.Width + this.m_SeparateSize + this.m_Control.Width + this.m_ControlMargin.Right;

            // 高さ決定
            int _Height = this.m_Control.Height;
            if (this.m_Label.Height > this.m_Control.Height)
            {
                _Height = this.m_Label.Height;
            }
            this.Height = this.ControlMargin.Top + _Height +  this.ControlMargin.Bottom;

            // コントロール位置設定
            this.m_Control.Location = new Point(this.ControlMargin.Left + this.m_Label.Width + this.m_SeparateSize, this.ControlMargin.Top);

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

            // コントロールサイズ決定
            this.m_Control.Width = this.Width - this.ControlMargin.Right - this.ControlMargin.Left;
            this.m_Control.Height = this.Height - this.m_ControlMargin.Top - this.m_Label.Height - this.m_SeparateSize;

            // コントロール設定
            this.SetControl();

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

            // コントロールサイズ決定
            this.m_Control.Width = this.Width - this.ControlMargin.Right - this.ControlMargin.Left;
            this.m_Control.Height = this.Height - this.m_SeparateSize - this.m_Label.Height - this.m_ControlMargin.Bottom;

            // コントロール設定
            this.SetControl();

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

            // コントロールサイズ決定
            this.m_Control.Width = this.Width - this.m_Label.Width - this.m_SeparateSize - this.ControlMargin.Right;
            this.m_Control.Height = this.Height - this.ControlMargin.Top - this.ControlMargin.Bottom;

            // コントロール設定
            this.SetControl();

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

            // コントロールサイズ決定
            this.m_Control.Width = this.Width - this.ControlMargin.Left - this.m_Label.Width - this.m_SeparateSize;
            this.m_Control.Height = this.Height - this.ControlMargin.Top - this.ControlMargin.Bottom;

            // コントロール設定
            this.SetControl();

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