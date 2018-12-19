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
        private Size m_ScrollSize = new Size();
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

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~AutoScrollControl()
        {
            Trace.WriteLine("=>>>> AutoScrollControl::~AutoScrollControl()");

            // タイマー停止
            this.Stop();

            Trace.WriteLine("<<<<= AutoScrollControl::~AutoScrollControl()");
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
            this.m_ScrollPanel.AutoScroll = false;
            this.m_ScrollPanel.AutoSize = false;
            this.Controls.Add(this.m_ScrollPanel);
            this.Size.ToString();

            //　タイマーイベント登録
            this.m_IntervalTimer.Tick += this.OnIntervalTimer;
            this.m_WaitTimer.Tick += this.OnWaitTimer;

            Trace.WriteLine("<<<<= AutoScrollControl::Initialization()");
        }
        #endregion

        /// <summary>
        /// スクロール開始
        /// </summary>
        public void Start()
        {
            Trace.WriteLine("=>>>> AutoScrollControl::Start()");

            // スクロールされているか？
            if (!this.m_IntervalTimer.Enabled)
            {
                // スクロール開始
                this.m_IntervalTimer.Start();
            }

            Trace.WriteLine("<<<<= AutoScrollControl::Start()");
        }

        /// <summary>
        /// スクロール停止
        /// </summary>
        public void Stop()
        {
            Trace.WriteLine("=>>>> AutoScrollControl::Stop()");

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

            Trace.WriteLine("<<<<= AutoScrollControl::Stop()");
        }

        /// <summary>
        /// スクロールイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIntervalTimer(object sender, EventArgs e)
        {
            Trace.WriteLine("=>>>> AutoScrollControl::OnIntervalTimer(object, EventArgs)");
            Debug.WriteLine("タイマ間隔：{0}", this.m_IntervalTimer.Interval);
            Debug.WriteLine(DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss.fff"));

            // 休止タイマー動作中は動作させない
            if (this.m_WaitTimer.Enabled)
            {
                return;
            }

            // 横スクロール有効か？
            if ((this.m_AllowScroll & AllowScroll.SideWay) == AllowScroll.SideWay)
            {
                // 横スクロール実行
                this.SideWayScroll();
            }

            // 縦スクロール有効か？
            if ((this.m_AllowScroll & AllowScroll.Vietical) == AllowScroll.Vietical)
            {
                // 縦スクロール実行
                this.VirticalScroll();

            }

            Trace.WriteLine("<<<<= AutoScrollControl::OnIntervalTimer(object, EventArgs)");
        }

        /// <summary>
        /// 横スクロール実行
        /// </summary>
        private void SideWayScroll()
        {
            Trace.WriteLine("=>>>> AutoScrollControl::SideWayScroll()");
            Debug.WriteLine("タイマ間隔：{0}", this.m_IntervalTimer.Interval);
            Debug.WriteLine(DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss.fff"));

            // コントロール移動
            this.m_Control.Left += this.m_ScrollSize.Width;

            // 右方向スクロールの場合
            if (this.m_Control.Left > this.Width)
            {
                // 休止タイマー発動
                this.m_WaitTimer.Start();
                return;
            }
            // TODO:左方向スクロールの場合

            Trace.WriteLine("<<<<= AutoScrollControl::SideWayScroll()");
        }

        /// <summary>
        /// 縦スクロール実行
        /// </summary>
        private void VirticalScroll()
        {
            Trace.WriteLine("=>>>> AutoScrollControl::VirticalScroll()");
            Debug.WriteLine("タイマ間隔：{0}", this.m_IntervalTimer.Interval);
            Debug.WriteLine(DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss.fff"));

            // TODO:未実装

            Trace.WriteLine("<<<<= AutoScrollControl::VirticalScroll()");
        }

        /// <summary>
        /// スクロール休止イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWaitTimer(object sender, EventArgs e)
        {
            Trace.WriteLine("=>>>> AutoScrollControl::OnWaitTimer(object, EventArgs)");
            Debug.WriteLine("タイマ間隔：{0}", this.m_WaitTimer.Interval);
            Debug.WriteLine(DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss.fff"));

            // ウェイト完了なのでタイマー停止
            this.m_WaitTimer.Stop();

            // 横スクロール有効か？
            if ((this.m_AllowScroll & AllowScroll.SideWay) == AllowScroll.SideWay)
            {
                // 横スクロールリセット
                this.ResetSideWayScroll();
            }

            // 縦スクロール有効か？
            if ((this.m_AllowScroll & AllowScroll.Vietical) == AllowScroll.Vietical)
            {
                // 縦スクロールリセット
                this.ResetVirticalScroll();
            }

            Trace.WriteLine("<<<<= AutoScrollControl::OnWaitTimer(object, EventArgs)");
        }

        /// <summary>
        /// 横スクロールリセット
        /// </summary>
        private void ResetSideWayScroll()
        {
            Trace.WriteLine("=>>>> AutoScrollControl::ResetSideWayScroll()");

            // 右方向スクロールの場合
            if (this.m_Control.Left > this.Width)
            {
                // 休止タイマー発動
                this.m_Control.Left = 0 - this.m_Control.Width;
                return;
            }
            // TODO:左方向スクロールの場合

            Trace.WriteLine("<<<<= AutoScrollControl::ResetSideWayScroll()");
        }

        /// <summary>
        /// 縦スクロールリセット
        /// </summary>
        private void ResetVirticalScroll()
        {
            Trace.WriteLine("=>>>> AutoScrollControl::ResetVirticalScroll()");

            // TODO:未実装

            Trace.WriteLine("<<<<= AutoScrollControl::ResetVirticalScroll()");
        }
    }
}