using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Common.Control
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
        /// ラベル
        /// </summary>
        private Label m_Label = null;

        /// <summary>
        /// Labelマージン
        /// </summary>
        private ControlMargin m_LabelMargin = new ControlMargin(1, 1, 1, 1);

        /// <summary>
        /// コントロール
        /// </summary>
        private System.Windows.Forms.Control m_Control = null;

        /// <summary>
        /// Controlマージン
        /// </summary>
        private ControlMargin m_ControlMargin = new ControlMargin(1, 1, 1, 1);
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
        /// Labelマージン
        /// </summary>
        //[Localizable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlMargin LabelMargin
        {
            get
            {
                return this.m_LabelMargin;
            }
            set
            {
                this.m_LabelMargin = value;

                // コントロール設定
                this.SetControl();
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
        #endregion

        private TableLayoutPanel m_Layout = new TableLayoutPanel();
        private Panel m_LabelPanel = new Panel();
        private Panel m_ControlPanel = new Panel();
        private float m_LabelPercent = 50;
        public float LabelPercent
        {
            get
            {
                return this.m_LabelPercent;
            }
            set
            {
                if (value > 100)
                {
                    return;
                }

                this.m_LabelPercent = value;
                this.m_ControlPercent = 100F - value;

                // コントロール設定
                this.SetControl();
            }
        }
        private float m_ControlPercent = 50;
        public float ControlPercent
        {
            get
            {
                return this.m_ControlPercent;
            }
            set
            {
                if (value > 100)
                {
                    return;
                }

                this.m_LabelPercent = 100F - value;
                this.m_ControlPercent = value;

                // コントロール設定
                this.SetControl();
            }
        }

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LabeledControl(System.Windows.Forms.Control control)
        {
            // コンポーネント初期化
            InitializeComponent();

            // 初期設定
            this.m_Label = new Label() { Text = "Label" };
            this.m_Control = control;

            // コントロール追加
            this.Controls.Add(this.m_Layout);
            this.m_LabelPanel.Controls.Add(this.m_Label);
            this.m_ControlPanel.Controls.Add(this.m_Control);
            this.m_LabelPanel.Dock = DockStyle.Fill;
            this.m_ControlPanel.Dock = DockStyle.Fill;
            this.m_Label.Dock = DockStyle.Fill;
            this.m_Control.Dock = DockStyle.Fill;
            this.m_Layout.Dock = DockStyle.Fill;

            // コントロール設定
            this.SetControl();


            // イベントハンドラ設定
            this.Resize += this.OnResize;
            //this.m_Label.AutoSizeChanged += this.Label_OnAutoSizeChanged;
            //this.m_Label.SizeChanged += this.Label_OnSizeChanged;
            //this.m_Label.TextChanged += this.Label_OnTextChanged;
            //this.m_Control.SizeChanged += this.Control_OnSizeChanged;
        }
        #endregion

        #region コントロール設定
        /// <summary>
        /// コントロール設定
        /// </summary>
        public void SetControl()
        {
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
        }

        /// <summary>
        /// コントロール設定(上)
        /// </summary>
        private void SetControl_Top()
        {
            Trace.WriteLine("=>>>> LabeledControl::SetControl_Top()");

            // レイアウト一旦停止
            this.m_Layout.SuspendLayout();

            // コントロールクリア
            this.m_Layout.Controls.Clear();
            this.m_Layout.RowStyles.Clear();
            this.m_Layout.ColumnStyles.Clear();

            // サイズ設定
            this.Height = this.GetHeightTotal();
            this.Width = this.GetWidthMax();

            // コントロール追加
            this.m_Layout.RowCount = 2;
            this.m_Layout.ColumnCount = 1;
            this.m_Layout.Controls.Add(this.m_Label, 0, 0);
            this.m_Layout.Controls.Add(this.m_Control, 0, 1);

            // コントロール設定
            RowStyle labelRowStyle = new RowStyle(SizeType.Percent, this.m_LabelPercent);
            RowStyle controlRowStyle = new RowStyle(SizeType.Percent, this.m_ControlPercent);
            ColumnStyle columnStyle = new ColumnStyle(SizeType.Absolute, this.GetWidthMax());
            this.m_Layout.RowStyles.Add(labelRowStyle);
            this.m_Layout.RowStyles.Add(controlRowStyle);
            this.m_Layout.ColumnStyles.Add(columnStyle);

            // レイアウト再開
            //this.m_SplitContainer.ResumeLayout();
            this.m_Layout.ResumeLayout();

            Trace.WriteLine("<<<<= LabeledControl::SetControl_Top()");
        }

        /// <summary>
        /// コントロール設定(下)
        /// </summary>
        private void SetControl_Bottom()
        {
            Trace.WriteLine("=>>>> LabeledControl::SetControl_Bottom()");

            // レイアウト一旦停止
            this.m_Layout.SuspendLayout();

            // コントロールクリア
            this.m_Layout.Controls.Clear();
            this.m_Layout.RowStyles.Clear();
            this.m_Layout.ColumnStyles.Clear();

            // サイズ設定
            this.Height = this.GetHeightTotal();
            this.Width = this.GetWidthMax();

            // コントロール追加
            this.m_Layout.RowCount = 2;
            this.m_Layout.ColumnCount = 1;
            this.m_Layout.Controls.Add(this.m_Control, 0, 0);
            this.m_Layout.Controls.Add(this.m_Label, 0, 1);

            // コントロール設定
            RowStyle labelRowStyle = new RowStyle(SizeType.Percent, this.m_LabelPercent);
            RowStyle controlRowStyle = new RowStyle(SizeType.Percent, this.m_ControlPercent);
            ColumnStyle columnStyle = new ColumnStyle(SizeType.Absolute, this.GetWidthMax());
            this.m_Layout.RowStyles.Add(controlRowStyle);
            this.m_Layout.RowStyles.Add(labelRowStyle);
            this.m_Layout.ColumnStyles.Add(columnStyle);

            // レイアウト再開
            this.m_Layout.ResumeLayout();

            Trace.WriteLine("<<<<= LabeledControl::SetControl_Bottom()");
        }

        /// <summary>
        /// コントロール設定(右)
        /// </summary>
        private void SetControl_Right()
        {
            Trace.WriteLine("=>>>> LabeledControl::SetControl_Right()");
            Trace.WriteLine("=>>>> LabeledControl::SetControl_Left()");

            // レイアウト一旦停止
            this.m_Layout.SuspendLayout();

            // コントロールクリア
            this.m_Layout.Controls.Clear();
            this.m_Layout.RowStyles.Clear();
            this.m_Layout.ColumnStyles.Clear();

            // サイズ設定
            this.Height = this.GetHeightMax();
            this.Width = this.GetWidthTotal();

            // コントロール追加
            this.m_Layout.RowCount = 1;
            this.m_Layout.ColumnCount = 2;
            this.m_Layout.Controls.Add(this.m_Control, 0, 0);
            this.m_Layout.Controls.Add(this.m_Label, 1, 0);

            // コントロール設定
            RowStyle rowStyle = new RowStyle(SizeType.Absolute, this.GetHeightMax());
            ColumnStyle labelColumnStyle = new ColumnStyle(SizeType.Percent, this.m_LabelPercent);
            ColumnStyle controlColumnStyle = new ColumnStyle(SizeType.Percent, this.m_ControlPercent);
            this.m_Layout.RowStyles.Add(rowStyle);
            this.m_Layout.ColumnStyles.Add(controlColumnStyle);
            this.m_Layout.ColumnStyles.Add(labelColumnStyle);

            // レイアウト再開
            this.m_Layout.ResumeLayout();

            Trace.WriteLine("<<<<= LabeledControl::SetControl_Left()");
            Trace.WriteLine("<<<<= LabeledControl::SetControl_Right()");
        }

        /// <summary>
        /// コントロール設定(左)
        /// </summary>
        private void SetControl_Left()
        {
            Trace.WriteLine("=>>>> LabeledControl::SetControl_Left()");

            // レイアウト一旦停止
            this.m_Layout.SuspendLayout();

            // コントロールクリア
            this.m_Layout.Controls.Clear();
            this.m_Layout.RowStyles.Clear();
            this.m_Layout.ColumnStyles.Clear();

            // サイズ設定
            this.Height = this.GetHeightMax();
            this.Width = this.GetWidthTotal();

            // コントロール追加
            this.m_Layout.RowCount = 1;
            this.m_Layout.ColumnCount = 2;
            this.m_Layout.Controls.Add(this.m_Label, 0, 0);
            this.m_Layout.Controls.Add(this.m_Control, 1, 0);

            // コントロール設定
            RowStyle rowStyle = new RowStyle(SizeType.Absolute, this.GetHeightMax());
            ColumnStyle labelColumnStyle = new ColumnStyle(SizeType.Percent, this.m_LabelPercent);
            ColumnStyle controlColumnStyle = new ColumnStyle(SizeType.Percent, this.m_ControlPercent);
            this.m_Layout.RowStyles.Add(rowStyle);
            this.m_Layout.ColumnStyles.Add(labelColumnStyle);
            this.m_Layout.ColumnStyles.Add(controlColumnStyle);

            // レイアウト再開
            this.m_Layout.ResumeLayout();

            Trace.WriteLine("<<<<= LabeledControl::SetControl_Left()");
        }

        #region サイズ取得
        /// <summary>
        /// 幅（Label）取得
        /// </summary>
        /// <returns></returns>
        private int GetLabelWidth()
        {
            int _Width = this.m_LabelMargin.Left + this.m_LabelMargin.Right + this.m_Label.Width;
            return _Width;
        }

        /// <summary>
        /// 幅（Control）取得
        /// </summary>
        /// <returns></returns>
        private int GetControlWidth()
        {
            int _Width = this.m_ControlMargin.Left + this.m_ControlMargin.Right + this.m_Control.Width;
            return _Width;
        }

        /// <summary>
        /// 高さ（Label）取得
        /// </summary>
        /// <returns></returns>
        private int GetLabelHeight()
        {
            int _Height = this.m_LabelMargin.Top + this.m_LabelMargin.Bottom + this.m_Label.Height;
            return _Height;
        }

        /// <summary>
        /// 高さ（Control）取得
        /// </summary>
        /// <returns></returns>
        private int GetControlHeight()
        {
            int _Height = this.m_ControlMargin.Top + this.m_ControlMargin.Bottom + this.m_Control.Height;
            return _Height;
        }
        /// <summary>
        /// 高さ（合計）取得
        /// </summary>
        /// <returns></returns>
        private int GetWidthMax()
        {
            // 幅決定
            int _LabelWidth = this.GetLabelWidth();
            int _ControlWidth = this.GetControlWidth();

            // 幅返却
            if (_LabelWidth > _ControlWidth)
            {
                return _LabelWidth;
            }
            else
            {
                return _ControlWidth;
            }
        }

        /// <summary>
        /// 高さ（最大）取得
        /// </summary>
        /// <returns></returns>
        private int GetHeightMax()
        {
            // 高さ決定
            int _LabelHeight = this.GetLabelHeight();
            int _ControlHeight = this.GetControlHeight();

            // 高さ返却
            if (_LabelHeight > _ControlHeight)
            {
                return _LabelHeight;
            }
            else
            {
                return _ControlHeight;
            }
        }

        /// <summary>
        /// 高さ（幅）取得
        /// </summary>
        /// <returns></returns>
        private int GetWidthTotal()
        {
            // 幅決定
            int _Width = this.GetLabelWidth() + this.GetControlWidth();

            // 幅返却
            return _Width;
        }
        private int GetHeightTotal()
        {
            // 高さ決定
            int _Height = this.GetLabelHeight() + this.GetControlHeight();

            // 高さ返却
            return _Height;
        }
        #endregion
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
#if __DELETE__
            // コントロールサイズ決定
            this.m_Control.Width = this.Width - this.ControlMargin.Right - this.ControlMargin.Left;
            this.m_Control.Height = this.Height - this.m_ControlMargin.Top - this.m_Label.Height - this.m_SeparateSize;

            // コントロール設定
            this.SetControl();
#endif
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
#if __DELETE__
            // コントロールサイズ決定
            this.m_Control.Width = this.Width - this.ControlMargin.Right - this.ControlMargin.Left;
            this.m_Control.Height = this.Height - this.m_SeparateSize - this.m_Label.Height - this.m_ControlMargin.Bottom;

            // コントロール設定
            this.SetControl();
#endif
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
#if __DELETE__
            // コントロールサイズ決定
            this.m_Control.Width = this.Width - this.m_Label.Width - this.m_SeparateSize - this.ControlMargin.Right;
            this.m_Control.Height = this.Height - this.ControlMargin.Top - this.ControlMargin.Bottom;

            // コントロール設定
            this.SetControl();
#endif
            Trace.WriteLine("<<<<= LabeledControl::OnResize_Right(object, EventArgs)");
        }

        /// <summary>
        /// リサイズ(左)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResize_Left(object sender, EventArgs e)
        {
            Trace.WriteLine("=>>>> LabeledControl::OnResize_Left(object, EventArgs)");
#if __DELETE__
            // コントロールサイズ決定
            this.m_Control.Width = this.Width - this.ControlMargin.Left - this.m_Label.Width - this.m_SeparateSize;
            this.m_Control.Height = this.Height - this.ControlMargin.Top - this.ControlMargin.Bottom;

            // コントロール設定
            this.SetControl();
#endif
            Trace.WriteLine("<<<<= LabeledControl::OnResize_Left(object, EventArgs)");
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