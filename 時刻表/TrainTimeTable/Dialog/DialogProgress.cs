using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainTimeTable.Dialog
{
    /// <summary>
    /// DialogProgressクラス
    /// </summary>
    /// <see cref="https://dobon.net/vb/dotnet/programing/progressdialogbw.html"/>
    public partial class DialogProgress : Form
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region DoWorkEventHandlerパラメータ
        /// <summary>
        /// DoWorkEventHandlerパラメータ
        /// </summary>
        private object[] m_WorkerArgument = null;
        #endregion

        #region 結果オブジェクト
        /// <summary>
        /// 結果オブジェクト
        /// </summary>
        private object m_Result = null;

        /// <summary>
        /// DoWorkイベントハンドラで設定された結果
        /// </summary>
        public object Result
        {
            get
            {
                return m_Result;
            }
        }
        #endregion

        #region 例外オブジェクト
        /// <summary>
        /// 例外オブジェクト
        /// </summary>
        private Exception m_Error = null;

        /// <summary>
        /// バックグラウンド処理中に発生したエラー
        /// </summary>
        public Exception Error
        {
            get
            {
                return m_Error;
            }
        }
        #endregion

        #region BackgroundWorkerオブジェクト
        /// <summary>
        /// 進行状況ダイアログで使用しているBackgroundWorkerクラス
        /// </summary>
        public BackgroundWorker BackgroundWorker
        {
            get
            {
                return backgroundWorkerMain;
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="title">タイトルバーに表示するテキスト</param>
        /// <param name="doWork">バックグラウンドで実行するメソッド</param>
        /// <param name="args">doWorkで取得できるパラメータ</param>
        public DialogProgress(string title, DoWorkEventHandler doWork, params object[] args)
        {
            // ロギング
            Logger.Debug("=>>>> DialogProgress::DialogProgress(string, DoWorkEventHandler, params object[])");
            Logger.DebugFormat("title :[{0}]", title);
            Logger.DebugFormat("doWork:[{0}]", doWork);
            Logger.DebugFormat("args  :[{0}]", args);

            // コンポーネント初期化
            InitializeComponent();

            // 初期設定
            m_WorkerArgument = args;
            Text = title;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            ControlBox = false;
            CancelButton = buttonCancel;
            labelMessage.Text = "";
            progressBarMain.Minimum = 0;
            progressBarMain.Maximum = 100;
            progressBarMain.Value = 0;
            buttonCancel.Text = "キャンセル";
            buttonCancel.Enabled = true;
            backgroundWorkerMain.WorkerReportsProgress = true;
            backgroundWorkerMain.WorkerSupportsCancellation = true;

            // イベント設定
            Shown += new EventHandler(ProgressDialog_Shown);
            buttonCancel.Click += new EventHandler(buttonCancel_Click);
            backgroundWorkerMain.DoWork += doWork;
            backgroundWorkerMain.ProgressChanged += new ProgressChangedEventHandler(backgroundWorkerMain_ProgressChanged);
            backgroundWorkerMain.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerMain_RunWorkerCompleted);

            // ロギング
            Logger.Debug("<<<<= DialogProgress::DialogProgress(string, DoWorkEventHandler, params object[])");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="title">タイトルバーに表示するテキスト</param>
        /// <param name="doWork">バックグラウンドで実行するメソッド</param>
        public DialogProgress(string title, DoWorkEventHandler doWork)
            : this(title, doWork, null)
        {
            // ロギング
            Logger.Debug("=>>>> DialogProgress::DialogProgress(string, DoWorkEventHandler)");
            Logger.DebugFormat("title :[{0}]", title);
            Logger.DebugFormat("doWork:[{0}]", doWork);

            // ロギング
            Logger.Debug("<<<<= DialogProgress::DialogProgress(string, DoWorkEventHandler)");
        }
        #endregion

        #region イベント
        #region DialogProgressイベント
        /// <summary>
        /// DialogProgress_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DialogProgress_Load(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DialogProgress::DialogProgress_Load(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);


            // ロギング
            Logger.Debug("<<<<= DialogProgress::DialogProgress_Load(object, EventArgs)");
        }

        /// <summary>
        /// ProgressDialog_Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressDialog_Shown(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DialogProgress::ProgressDialog_Shown(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // フォームが表示されたときにバックグラウンド処理を開始
            backgroundWorkerMain.RunWorkerAsync(m_WorkerArgument);

            // ロギング
            Logger.Debug("<<<<= DialogProgress::ProgressDialog_Shown(object, EventArgs)");
        }
        #endregion

        #region buttonCancelイベント
        /// <summary>
        /// buttonCancel_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DialogProgress::buttonCancel_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // キャンセルボタンが押されたとき
            buttonCancel.Enabled = false;
            backgroundWorkerMain.CancelAsync();

            // ロギング
            Logger.Debug("<<<<= DialogProgress::buttonCancel_Click(object, EventArgs)");
        }
        #endregion

        #region backgroundWorkerMainイベント
        /// <summary>
        /// backgroundWorkerMain_ProgressChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerMain_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DialogProgress::backgroundWorkerMain_ProgressChanged(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            //プログレスバーの値を変更する
            if (e.ProgressPercentage < progressBarMain.Minimum)
            {
                progressBarMain.Value = progressBarMain.Minimum;
            }
            else if (progressBarMain.Maximum < e.ProgressPercentage)
            {
                progressBarMain.Value = progressBarMain.Maximum;
            }
            else
            {
                progressBarMain.Value = e.ProgressPercentage;
            }

            //メッセージのテキストを変更する
            labelMessage.Text = (string)e.UserState;

            // ロギング
            Logger.Debug("<<<<= DialogProgress::backgroundWorkerMain_ProgressChanged(object, EventArgs)");
        }

        /// <summary>
        /// backgroundWorkerMain_RunWorkerCompleted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerMain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DialogProgress::backgroundWorkerMain_RunWorkerCompleted(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // エラー判定
            if (e.Error != null)
            {
                // 結果設定
                m_Error = e.Error;
                DialogResult = DialogResult.Abort;

                // ロギング
                Logger.ErrorFormat("m_Error     :[{0}]", m_Error);
                Logger.ErrorFormat(m_Error.Message);
                Logger.ErrorFormat("DialogResult:[{0}]", DialogResult);
            }
            else if (e.Cancelled)
            {
                // 結果設定
                DialogResult = DialogResult.Cancel;

                // ロギング
                Logger.WarnFormat("DialogResult:[{0}]", DialogResult);
            }
            else
            {
                // 結果設定
                m_Result = e.Result;
                DialogResult = DialogResult.OK;

                // ロギング
                Logger.DebugFormat("m_Result    :[{0}]", m_Result?.ToString());
                Logger.DebugFormat("DialogResult:[{0}]", DialogResult);
            }

            // フォームをクローズする
            Close();

            // ロギング
            Logger.Debug("<<<<= DialogProgress::backgroundWorkerMain_RunWorkerCompleted(object, EventArgs)");
        }
        #endregion
        #endregion
    }
}
