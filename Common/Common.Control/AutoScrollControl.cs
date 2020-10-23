using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Common.Control
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

        /// <summary>
        /// スクロール間隔タイマー
        /// </summary>
        private System.Windows.Forms.Timer m_IntervalTimer = new Timer();

        /// <summary>
        /// スクロール休止タイマー
        /// </summary>
        private System.Windows.Forms.Timer m_WaitTimer = new Timer();

        /// <summary>
        /// スクロール方向
        /// </summary>
        private AllowScroll m_AllowScroll = AllowScroll.SideWay;

        /// <summary>
        /// スクロールサイズ
        /// </summary>
        private Size m_ScrollSize = new Size(1, 1);
        #endregion

        #region プロパティ
        /// <summary>
        /// コントロール
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
        /// スクロールパネル
        /// </summary>
        [Localizable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public System.Windows.Forms.Panel ScrollPanel
        {
            get
            {
                return this.m_ScrollPanel;
            }
        }

        /// <summary>
        /// スクロール間隔タイマー
        /// </summary>
        [Localizable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public System.Windows.Forms.Timer IntervalTimer
        {
            get
            {
                return this.m_IntervalTimer;
            }
            set
            {
                this.m_IntervalTimer = value;
            }
        }

        /// <summary>
        /// スクロール休止タイマー
        /// </summary>
        [Localizable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public System.Windows.Forms.Timer WaitTimer
        {
            get
            {
                return this.m_WaitTimer;
            }
            set
            {
                this.m_WaitTimer = value;
            }
        }

        /// <summary>
        /// スクロール方向
        /// </summary>
        [Localizable(true)]
        public AllowScroll AllowScroll
        {
            get
            {
                return this.m_AllowScroll;
            }
            set
            {
                this.m_AllowScroll = value;
            }
        }

        /// <summary>
        /// スクロールサイズ
        /// </summary>
        [Localizable(true)]
        public Size ScrollSize
        {
            get
            {
                return this.m_ScrollSize;
            }
            set
            {
                this.m_ScrollSize = value;
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="control"></param>
        public AutoScrollControl(System.Windows.Forms.Control control)
        {
            // 初期設定
            this.m_Control = control;
            this.Width = control.Width;
            this.Height = control.Height;

            // コントロール追加
            this.Controls.Add(control);

            // コンポーネント初期化
            InitializeComponent();

            // 初期化
            this.Initialization();

            // スクロールリセット
            this.Reset();
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~AutoScrollControl()
        {
            // タイマー停止
            this.Stop();
        }
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            // スクロールパネル生成
            this.m_ScrollPanel = new Panel();
            this.m_ScrollPanel.AutoScroll = false;
            this.m_ScrollPanel.AutoSize = false;
            this.Controls.Add(this.m_ScrollPanel);

            //　タイマーイベント登録
            this.m_IntervalTimer.Tick += this.OnIntervalTimer;
            this.m_WaitTimer.Tick += this.OnWaitTimer;

            // リセット
            this.Reset();
        }
        #endregion

        /// <summary>
        /// スクロール開始
        /// </summary>
        public void Start()
        {
            // スクロールされているか？
            if (!this.m_IntervalTimer.Enabled)
            {
                // スクロール開始
                this.m_IntervalTimer.Start();
            }
        }

        /// <summary>
        /// リセット
        /// </summary>
        public void Reset()
        {
            // 横スクロール有効か？
            if (this.m_AllowScroll.HasFlag(AllowScroll.SideWay))
            {
                // 横スクロールリセット
                this.ResetSideWayScroll();
            }

            // 縦スクロール有効か？
            if (this.m_AllowScroll.HasFlag(AllowScroll.Vertical))
            {
                // 縦スクロールリセット
                this.ResetVirticalScroll();
            }
        }


        /// <summary>
        /// 横スクロールリセット
        /// </summary>
        private void ResetSideWayScroll()
        {
            // 右方向スクロールの場合
            if (this.m_ScrollSize.Width > 0)
            {
                // ポジションリセット
                this.m_Control.Left = 0;
            }
            // 左方向スクロールの場合
            else if (this.m_ScrollSize.Width < 0)
            {
                // ポジションリセット
                this.m_Control.Left = this.Width;
            }
        }

        /// <summary>
        /// 縦スクロールリセット
        /// </summary>
        private void ResetVirticalScroll()
        {
            // 下方向スクロールの場合
            if (this.m_ScrollSize.Height > 0)
            {
                // ポジションリセット
                this.m_Control.Top = 0;
            }
            // 上方向スクロールの場合
            else if (this.m_ScrollSize.Height < 0)
            {
                // ポジションリセット
                this.m_Control.Top = this.Height;
            }
        }

        /// <summary>
        /// スクロール停止
        /// </summary>
        public void Stop()
        {
            // 休止タイマー動作中？
            if (this.m_WaitTimer.Enabled)
            {
                // 休止タイマー停止
                this.m_WaitTimer.Stop();
            }

            // スクロールされているか？
            if (this.m_IntervalTimer.Enabled)
            {
                // スクロール停止
                this.m_IntervalTimer.Stop();
            }
        }

        /// <summary>
        /// スクロールイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIntervalTimer(object sender, EventArgs e)
        {
            // 休止タイマー動作中は動作させない
            if (this.m_WaitTimer.Enabled)
            {
                return;
            }

            // 横スクロール有効か？
            if(this.m_AllowScroll.HasFlag(AllowScroll.SideWay))
            {
                // 横スクロール実行
                this.SideWayScroll();
            }

            // 縦スクロール有効か？
            if (this.m_AllowScroll.HasFlag(AllowScroll.Vertical))
            {
                // 縦スクロール実行
                this.VirticalScroll();
            }
        }

        /// <summary>
        /// 横スクロール実行
        /// </summary>
        private void SideWayScroll()
        {
            // コントロール移動
            this.m_Control.Left += this.m_ScrollSize.Width;

            // 右方向スクロールの場合
            if (this.m_ScrollSize.Width > 0)
            {
                if (this.m_Control.Left > this.Width)
                {
                    // 休止タイマー発動
                    this.m_WaitTimer.Start();
                }
            }
            // 左方向スクロールの場合
            else if (this.m_ScrollSize.Width < 0)
            {
                if (this.m_Control.Left + this.m_Control.Width <= 0)
                {
                    // 休止タイマー発動
                    this.m_WaitTimer.Start();
                }
            }
        }

        /// <summary>
        /// 縦スクロール実行
        /// </summary>
        private void VirticalScroll()
        {
            // コントロール移動
            this.m_Control.Top += this.m_ScrollSize.Height;

            // 下方向スクロールの場合
            if (this.m_ScrollSize.Height > 0)
            {
                if (this.m_Control.Top > this.Height)
                {
                    // 休止タイマー発動
                    this.m_WaitTimer.Start();
                }
            }
            // 上方向スクロールの場合
            else if (this.m_ScrollSize.Height < 0)
            {
                if (this.m_Control.Top + this.m_Control.Height <= 0)
                {
                    // 休止タイマー発動
                    this.m_WaitTimer.Start();
                }
            }
        }

        /// <summary>
        /// スクロール休止イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWaitTimer(object sender, EventArgs e)
        {
            // ウェイト完了なのでタイマー停止
            this.m_WaitTimer.Stop();

            // リセット
            this.Reset();
        }
    }
}