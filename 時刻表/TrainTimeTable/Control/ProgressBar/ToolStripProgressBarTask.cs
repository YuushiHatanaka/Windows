using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// ToolStripProgressBarTaskクラス
    /// </summary>
    public class ToolStripProgressBarTask : ToolStripProgressBar
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
        public string Result { get { return m_Result; } }
        #endregion

        /// <summary>
        /// 引継オブジェクト
        /// </summary>
        private object[] m_Argument = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <param name="doWork"></param>
        /// <param name="startButtonVisible"></param>
        public ToolStripProgressBarTask(int minimum, int maximum, DoWorkEvent doWork, params object[] args)
            : base()
        {
            // ロギング
            Logger.Debug("=>>>> ToolStripProgressBarTask::ToolStripProgressBarTask(int, int, DoWorkEvent, params object[])");
            Logger.DebugFormat("minimum:[{0}]", minimum);
            Logger.DebugFormat("maximum:[{0}]", maximum);
            Logger.DebugFormat("doWork :[{0}]", doWork);
            Logger.DebugFormat("args   :[{0}]", args);

            // コンポーネント初期化
            InitializeComponent();

            // 設定
            m_Argument = args;
            Minimum = minimum;
            Maximum = maximum;
            m_DoWork += doWork;

            // 非表示にする
            Visible = false;

            // ロギング
            Logger.Debug("<<<<= ToolStripProgressBarTask::ToolStripProgressBarTask(int, int, DoWorkEvent, params object[])");
        }

        /// <summary>
        /// コンポーネント初期化
        /// </summary>
        private void InitializeComponent()
        {
            // ロギング
            Logger.Debug("=>>>> ToolStripProgressBarTask::InitializeComponent()");

            // ロギング
            Logger.Debug("<<<<= ToolStripProgressBarTask::InitializeComponent()");
        }

        /// <summary>
        /// Task開始
        /// </summary>
        public void Start()
        {
            // ロギング
            Logger.Debug("=>>>> ToolStripProgressBarTask::Start()");

            // Task実行判定
            if (m_DoWork != null)
            {
                // 初期化
                Value = 0;

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
            Logger.Debug("<<<<= ToolStripProgressBarTask::Start()");
        }

        /// <summary>
        /// Task停止
        /// </summary>
        private void Stop()
        {
            // ロギング
            Logger.Debug("=>>>> ToolStripProgressBarTask::Stop()");

            // CancellationTokenSourceオブジェクト判定」
            if (m_CancellationTokenSource != null)
            {
                // 初期化
                Value = 0;

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
            Logger.Debug("<<<<= ToolStripProgressBarTask::Stop()");
        }

        /// <summary>
        /// 表示更新
        /// </summary>
        /// <param name="infomation"></param>
        private void ShowProgressBar(ProgressBarInfomation infomation)
        {
            // ロギング
            Logger.Debug("=>>>> ToolStripProgressBarTask::ShowProgressBar(ProgressBarInfomation)");
            Logger.DebugFormat("infomation:[{0}]", infomation);

            // 表示設定
            Value = infomation.Position;

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
            Logger.Debug("<<<<= ToolStripProgressBarTask::ShowProgressBar(ProgressBarInfomation)");
        }
    }
}
