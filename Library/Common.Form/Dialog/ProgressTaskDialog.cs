using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Common.Dialog
{
    /// <summary>
    /// ProgressTaskDialogクラス
    /// </summary>
    public partial class ProgressTaskDialog : Form
    {
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
        public delegate Task<string> DoWorkEvent(IProgress<ProgressInfo> position, CancellationToken cancelToken, params object[] args);

        /// <summary>
        /// DoWork
        /// </summary>
        public DoWorkEvent DoWork = null;

        /// <summary>
        /// Task<bool>オブジェクト
        /// </summary>
        private Task<string> m_Task = null;

        /// <summary>
        /// 処理結果文字列
        /// </summary>
        private string m_Result = string.Empty;

        /// <summary>
        /// 引継オブジェクト
        /// </summary>
        private object[] m_Argument = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <param name="doWork"></param>
        /// <param name="startButtonVisible"></param>
        public ProgressTaskDialog(Icon icon, string title, string message, int minimum, int maximum, DoWorkEvent doWork, bool startButtonVisible, params object[] args)
        {
            // コンポーネント初期化
            InitializeComponent();

            // IconからBitmapを生成
            Bitmap bitmap = icon.ToBitmap();

            // 設定
            Icon = icon;
            Text = title;
            m_Argument = args;
            pictureBoxMain.Image = bitmap;
            labelMessage.Text = message;
            progressBarMain.Minimum = minimum;
            progressBarMain.Maximum = maximum;
            DoWork += doWork;
            buttonStart.Visible = startButtonVisible;
        }

        /// <summary>
        /// ProgressTaskDialog_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressTaskDialog_Load(object sender, EventArgs e)
        {
            // コントロール設定
            tableLayoutPanelMain.Dock = DockStyle.Fill;
            pictureBoxMain.Dock = DockStyle.Fill;
            labelMessage.Dock = DockStyle.Fill;
            panelProgressBar.Dock = DockStyle.Fill;
            progressBarMain.Dock = DockStyle.Fill;
            tableLayoutPanelButton.Dock = DockStyle.Fill;
            buttonStart.Dock = DockStyle.Fill;
            buttonCancel.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// FormProgress_Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormProgress_Shown(object sender, EventArgs e)
        {
            // 開始判定
            if (buttonStart.Visible)
            {
                // 開始ボタンが表示されているので自動実行しない
                return;
            }

            // 開始(開始ボタンを押下したことにする)
            buttonStart_Click(sender, e);
        }

        /// <summary>
        /// buttonStart - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStart_Click(object sender, EventArgs e)
        {
            // ボタン更新
            buttonStart.Enabled = false;

            // 開始
            Start();

            // ボタン更新
            buttonCancel.Enabled = true;
        }

        /// <summary>
        /// buttonCancel - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // ボタン更新
            buttonCancel.Enabled = false;

            // 停止
            Stop();

            // ボタン更新
            buttonStart.Enabled = true;
        }

        /// <summary>
        /// Task開始
        /// </summary>
        private void Start()
        {
            // Task実行判定
            if (DoWork != null)
            {
                // CancellationTokenSourceoオブジェクト生成
                m_CancellationTokenSource = new CancellationTokenSource();

                // CancellationTokenオブジェクト取得
                CancellationToken cancellationToken = m_CancellationTokenSource.Token;

                // 経過表示オブジェクト生成
                Progress<ProgressInfo> progress = new Progress<ProgressInfo>(ShowProgress);

                // Task実行
                m_Task = Task.Run(() => DoWork(progress, cancellationToken, m_Argument));
            }
        }

        /// <summary>
        /// Task停止
        /// </summary>
        private void Stop()
        {
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

                // フォームClose
                Close();
            }
        }

        /// <summary>
        /// 表示更新
        /// </summary>
        /// <param name="info"></param>
        private void ShowProgress(ProgressInfo info)
        {
            // 表示設定
            progressBarMain.Value = info.Position;
            labelMessage.Text = info.Message;

            // 表示更新
            progressBarMain.Update();
            labelMessage.Update();

            // 終了判定
            if ((info.Position >= progressBarMain.Maximum) || (info.Result != DialogResult.None))
            {
                // Task終了待ち
                m_Task.Wait();

                // 結果設定
                DialogResult = info.Result;

                // 処理結果設定
                m_Result = m_Task.Result;

                // フォームClose
                Close();
            }
        }

        /// <summary>
        /// 返り値取得
        /// </summary>
        /// <returns></returns>
        public string GetResult()
        {
            // 返却
            return m_Result;
        }
    }
}
