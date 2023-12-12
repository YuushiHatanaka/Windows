using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// ProgressBarTaskクラス
    /// </summary>
    public class ProgressBarTask : ProgressBar
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// CancellationTokenSourceオブジェクト
        /// </summary>
        private CancellationTokenSource m_CancellationTokenSource;

        /// <summary>
        /// DoWork定義
        /// </summary>
        /// <param name="position"></param>
        /// <param name="cancelToken"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public delegate Task<string> DoWorkEvent(IProgress<ProgressBarInfomation> position, CancellationToken cancelToken, params object[] args);

        /// <summary>
        /// DoWork
        /// </summary>
        private DoWorkEvent m_DoWork = null;

        /// <summary>
        /// 処理結果
        /// </summary>
        public DialogResult DialogResult = DialogResult.None;

        /// <summary>
        /// Task<bool>オブジェクト
        /// </summary>
        private Task<string> m_Task = null;

        #region 処理結果文字列
        /// <summary>
        /// 処理結果文字列
        /// </summary>
        private string m_Result = string.Empty;

        /// <summary>
        /// 処理結果文字列
        /// </summary>
        public string Result {  get { return m_Result; } }
        #endregion

        /// <summary>
        /// 引継オブジェクト
        /// </summary>
        private object[] m_Argument = null;

        /// <summary>
        /// メッセージラベル
        /// </summary>
        private Label m_MessageLabel = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <param name="doWork"></param>
        /// <param name="startButtonVisible"></param>
        public ProgressBarTask(string message, int minimum, int maximum, DoWorkEvent doWork, params object[] args)
            : base()
        {
            // ロギング
            Logger.Debug("=>>>> ProgressBarTask::ProgressBarTask(string, int, int, DoWorkEvent, params object[])");
            Logger.DebugFormat("message:[{0}]", message);
            Logger.DebugFormat("minimum:[{0}]", minimum);
            Logger.DebugFormat("maximum:[{0}]", maximum);
            Logger.DebugFormat("doWork :[{0}]", doWork);
            Logger.DebugFormat("args   :[{0}]", args);

            // コンポーネント初期化
            InitializeComponent();

            // 設定
            m_Argument = args;
            m_MessageLabel.Text = message;
            Minimum = minimum;
            Maximum = maximum;
            m_DoWork += doWork;

            // 非表示にする
            Visible = false;

            // ロギング
            Logger.Debug("<<<<= ProgressBarTask::ProgressBarTask(string, int, int, DoWorkEvent, params object[])");
        }

        /// <summary>
        /// コンポーネント初期化
        /// </summary>
        private void InitializeComponent()
        {
            // ロギング
            Logger.Debug("=>>>> ProgressBarTask::InitializeComponent()");

            // メッセージLabel
            m_MessageLabel = new Label();
            Controls.Add(m_MessageLabel);
            m_MessageLabel.Dock = DockStyle.Fill;

            // ロギング
            Logger.Debug("<<<<= ProgressBarTask::InitializeComponent()");
        }

        /// <summary>
        /// Task開始
        /// </summary>
        public void Start()
        {
            // ロギング
            Logger.Debug("=>>>> ProgressBarTask::Start()");

            // Task実行判定
            if (m_DoWork != null)
            {
                // CancellationTokenSourceoオブジェクト生成
                m_CancellationTokenSource = new CancellationTokenSource();

                // CancellationTokenオブジェクト取得
                CancellationToken cancellationToken = m_CancellationTokenSource.Token;

                // 経過表示オブジェクト生成
                Progress<ProgressBarInfomation> progress = new Progress<ProgressBarInfomation>(ShowProgressBar);

                // Task実行
                m_Task = Task.Run(() => m_DoWork(progress, cancellationToken, m_Argument));

                // 表示にする
                Visible = true;
            }

            // ロギング
            Logger.Debug("<<<<= ProgressBarTask::Start()");
        }

        /// <summary>
        /// Task停止
        /// </summary>
        private void Stop()
        {
            // ロギング
            Logger.Debug("=>>>> ProgressBarTask::Stop()");

            // CancellationTokenSourceオブジェクト判定」
            if (m_CancellationTokenSource != null)
            {
                // Taskキャンセル
                m_CancellationTokenSource.Cancel();

                // Task終了待ち
                m_Task.Wait();

                // ダイアログリザルト
                DialogResult = DialogResult.Cancel;

                // 処理結果設定
                m_Result = m_Task.Result;

                // 非表示にする
                Visible = false;
            }

            // ロギング
            Logger.Debug("<<<<= ProgressBarTask::Stop()");
        }

        /// <summary>
        /// 表示更新
        /// </summary>
        /// <param name="infomation"></param>
        private void ShowProgressBar(ProgressBarInfomation infomation)
        {
            // ロギング
            Logger.Debug("=>>>> ProgressBarTask::ShowProgressBar(ProgressBarInfomation)");
            Logger.DebugFormat("infomation:[{0}]", infomation);

            // 表示設定
            Value = infomation.Position;
            m_MessageLabel.Text = infomation.Message;

            // 表示更新
            Update();
            m_MessageLabel.Update();

            // 終了判定
            if ((infomation.Position >= Maximum) || (infomation.Result != DialogResult.None))
            {
                // Task終了待ち
                m_Task.Wait();

                // 結果設定
                DialogResult = infomation.Result;

                // 処理結果設定
                m_Result = m_Task.Result;

                // 非表示にする
                Visible = false;
            }

            // ロギング
            Logger.Debug("<<<<= ProgressBarTask::ShowProgressBar(ProgressBarInfomation)");
        }
    }
}
