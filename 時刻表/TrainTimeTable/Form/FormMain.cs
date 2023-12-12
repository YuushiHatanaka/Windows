using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Common;
using TrainTimeTable.Component;
using TrainTimeTable.Database;
using TrainTimeTable.Dialog;
using TrainTimeTable.Property;

namespace TrainTimeTable
{
    /// <summary>
    /// FormMainクラス
    /// </summary>
    public partial class FormMain : Form
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region FormRoute辞書
        /// <summary>
        /// FormRoute辞書
        /// </summary>
        private DictionaryFormRoute m_FormRoutes = new DictionaryFormRoute();
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormMain()
        {
            // ロギング
            Logger.Debug("=>>>> FormMain::FormMain()");

            // コンポーネント初期化
            InitializeComponent();

            // ロギング
            Logger.Debug("<<<<= FormMain::FormMain()");
        }
        #endregion

        #region イベント
        #region FormMainイベント
        /// <summary>
        /// FormMain
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Load(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormMain::FormMain_Load(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // コントコール設定
            MinimumSize = Size;

            // ロギング
            Logger.Debug("<<<<= FormMain::FormMain_Load(object, EventArgs)");
        }

        /// <summary>
        /// FormMain_Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Closing(object sender, FormClosingEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormMain::FormMain_Closing(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 終了確認
            if (MessageBox.Show("アプリケーションを終了しますか？", "終了確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                // ロギング
                Logger.Debug("終了キャンセル");

                // 終了キャンセル
                e.Cancel = true;
            }

            // ロギング
            Logger.Debug("<<<<= FormMain::FormMain_Closing(object, EventArgs)");
        }
        #endregion
        #endregion

        #region メニューイベント
        #region toolStripMenuItemFileメニューイベント
        /// <summary>
        /// toolStripMenuItemFileNewCreate_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemFileNewCreate_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormMain::toolStripMenuItemFileNewCreate_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 時刻表ファイルを選択する
            string dbFileName = SelectOpenFile(false);

            // ファイル名判定
            if (dbFileName == string.Empty)
            {
                // ロギング
                Logger.Debug("<<<<= FormMain::toolStripMenuItemFileNewCreate_Click(object, EventArgs)");

                // 正常以外なので何もしない
                return;
            }

            // 登録判定
            if (m_FormRoutes.IsExist(dbFileName))
            {
                // メッセージ表示
                MessageBox.Show(
                    "同一ファイルの同時編集はできません。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // ロギング
                Logger.WarnFormat("同一ファイルの同時編集はできません:[{0}]", dbFileName);
                Logger.Debug("<<<<= FormMain::toolStripMenuItemFileCreateNew_Click(object, EventArgs)");

                // 正常以外なので何もしない
                return;
            }

            // 上書き確認
            if (!m_FormRoutes.OverwriteConfirmation(dbFileName))
            {
                // ロギング
                Logger.Debug("<<<<= FormMain::toolStripMenuItemFileCreateNew_Click(object, EventArgs)");

                // 正常以外なので何もしない
                return;
            }

            // FormRouteフォーム生成
            FormRoute formRoute = m_FormRoutes.Create(this);

            // DialogProgressオブジェクト生成
            using (DialogProgress dialogProgress = new DialogProgress("新規ファイル作成", doWorkFileNewCreate, formRoute, dbFileName))
            {
                // 進行状況ダイアログを表示する
                DialogResult result = dialogProgress.ShowDialog(this);

                // 結果判定
                if (result == DialogResult.Cancel)
                {
                    // メッセージ表示
                    MessageBox.Show("新規ファイル作成をキャンセルしました。", "キャンセル", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                else if (result == DialogResult.Abort)
                {
                    // メッセージ表示
                    MessageBox.Show("新規ファイル作成が異常終了しました。", "異常終了", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (result == DialogResult.OK)
                {
                    // RoutePropertiesオブジェクト取得
                    RouteFileProperty routeProperties = (RouteFileProperty)dialogProgress.Result;

                    // FormRouteフォーム更新
                    formRoute.Update(routeProperties);

                    // FormRouteフォーム登録
                    m_FormRoutes.Regston(dbFileName, formRoute);

                    // FormRouteフォーム表示
                    formRoute.Show();
                }
            }

            // ロギング
            Logger.Debug("<<<<= FormMain::toolStripMenuItemFileNewCreate_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripMenuItemFileOpen_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemFileOpen_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormMain::toolStripMenuItemFileOpen_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 時刻表ファイルを選択する
            string dbFileName = SelectOpenFile(true);

            // ファイル名判定
            if (dbFileName == string.Empty)
            {
                // ロギング
                Logger.Debug("<<<<= FormMain::toolStripMenuItemFileOpen_Click(object, EventArgs)");

                // 正常以外なので何もしない
                return;
            }

            // 登録判定
            if (m_FormRoutes.IsExist(dbFileName))
            {
                // メッセージ表示
                MessageBox.Show(
                    "同一ファイルの同時編集はできません。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // ロギング
                Logger.WarnFormat("同一ファイルの同時編集はできません:[{0}]", dbFileName);
                Logger.Debug("<<<<= FormMain::toolStripMenuItemFileOpen_Click(object, EventArgs)");

                // 正常以外なので何もしない
                return;
            }

            // FormRouteフォーム生成
            FormRoute formRoute = m_FormRoutes.Create(this);

            // DialogProgressオブジェクト生成
            using (DialogProgress dialogProgress = new DialogProgress("ファイルオープン", doWorkFileOpen, formRoute, dbFileName))
            {
                // 進行状況ダイアログを表示する
                DialogResult result = dialogProgress.ShowDialog(this);

                // 結果判定
                if (result == DialogResult.Cancel)
                {
                    // メッセージ表示
                    MessageBox.Show("ファイルオープンをキャンセルしました。", "キャンセル", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                else if (result == DialogResult.Abort)
                {
                    // メッセージ表示
                    MessageBox.Show("ファイルオープンが異常終了しました。", "異常終了", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (result == DialogResult.OK)
                {
                    // RoutePropertiesオブジェクト取得
                    RouteFileProperty routeProperties = (RouteFileProperty)dialogProgress.Result;

                    // FormRouteフォーム更新
                    formRoute.Update(routeProperties);

                    // FormRouteフォーム登録
                    m_FormRoutes.Regston(dbFileName, formRoute);

                    // FormRouteフォーム表示
                    formRoute.Show();
                }
            }

            // ロギング
            Logger.Debug("<<<<= FormMain::toolStripMenuItemFileOpen_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripMenuItemFileImportWinDIA_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemFileImportWinDIA_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormMain::toolStripMenuItemFileImportWinDIA_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // インポートファイルを選択する
            string fileName = SelectImportFile("WinDIA 路線ファイル(*.dia)|*.dia");

            // ファイル名判定
            if (fileName == string.Empty)
            {
                // ロギング
                Logger.Debug("<<<<= FormMain::toolStripMenuItemFileImportWinDIA_Click(object, EventArgs)");

                // 正常以外なので何もしない
                return;
            }

            // データベースファイル名作成
            string dbFileName = Path.ChangeExtension(fileName, "db");

            // 登録判定
            if (m_FormRoutes.IsExist(dbFileName))
            {
                // メッセージ表示
                MessageBox.Show(
                    "同一ファイルの同時編集はできません。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // ロギング
                Logger.WarnFormat("同一ファイルの同時編集はできません:[{0}]", fileName);
                Logger.Debug("<<<<= FormMain::toolStripMenuItemFileCreateNew_Click(object, EventArgs)");

                // 正常以外なので何もしない
                return;
            }

            // 上書き確認
            if (!m_FormRoutes.OverwriteConfirmation(dbFileName))
            {
                // ロギング
                Logger.Debug("<<<<= FormMain::toolStripMenuItemFileImportWinDIA_Click(object, EventArgs)");

                // 正常以外なので何もしない
                return;
            }

            // FormRouteフォーム生成
            FormRoute formRoute = m_FormRoutes.Create(this);

            // DialogProgressオブジェクト生成
            using (DialogProgress dialogProgress = new DialogProgress("ファイルインポート(WinDIA)", doWorkFileImportWinDIA, formRoute, dbFileName, fileName))
            {
                // 進行状況ダイアログを表示する
                DialogResult result = dialogProgress.ShowDialog(this);

                // 結果判定
                if (result == DialogResult.Cancel)
                {
                    // メッセージ表示
                    MessageBox.Show("ファイルインポート(WinDIA)をキャンセルしました。", "キャンセル", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                else if (result == DialogResult.Abort)
                {
                    // メッセージ表示
                    MessageBox.Show("ファイルインポート(WinDIA)が異常終了しました。", "異常終了", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (result == DialogResult.OK)
                {
                    // RoutePropertiesオブジェクト取得
                    RouteFileProperty routeProperties = (RouteFileProperty)dialogProgress.Result;

                    // FormRouteフォーム更新
                    formRoute.Update(routeProperties);

                    // FormRouteフォーム登録
                    m_FormRoutes.Regston(dbFileName, formRoute);

                    // FormRouteフォーム表示
                    formRoute.Show();
                }
            }

            // ロギング
            Logger.Debug("<<<<= FormMain::toolStripMenuItemFileImportWinDIA_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripMenuItemFileImportOudia_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemFileImportOudia_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormMain::toolStripMenuItemFileImportOudia_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // インポートファイルを選択する
            string fileName = SelectImportFile("OuDia 路線ファイル(*.oud)|*.oud");

            // ファイル名判定
            if (fileName == string.Empty)
            {
                // ロギング
                Logger.Debug("<<<<= FormMain::toolStripMenuItemFileImportOudia_Click(object, EventArgs)");

                // 正常以外なので何もしない
                return;
            }

            // データベースファイル名作成
            string dbFileName = Path.ChangeExtension(fileName, "db");

            // 登録判定
            if (m_FormRoutes.IsExist(dbFileName))
            {
                // メッセージ表示
                MessageBox.Show(
                    "同一ファイルの同時編集はできません。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // ロギング
                Logger.WarnFormat("同一ファイルの同時編集はできません:[{0}]", fileName);
                Logger.Debug("<<<<= FormMain::toolStripMenuItemFileCreateNew_Click(object, EventArgs)");

                // 正常以外なので何もしない
                return;
            }

            // 上書き確認
            if (!m_FormRoutes.OverwriteConfirmation(dbFileName))
            {
                // ロギング
                Logger.Debug("<<<<= FormMain::toolStripMenuItemFileImportOudia_Click(object, EventArgs)");

                // 正常以外なので何もしない
                return;
            }

            // FormRouteフォーム生成
            FormRoute formRoute = m_FormRoutes.Create(this);

            // DialogProgressオブジェクト生成
            using (DialogProgress dialogProgress = new DialogProgress("ファイルインポート(Oudia)", doWorkFileImportOudia, formRoute, dbFileName, fileName))
            {
                // 進行状況ダイアログを表示する
                DialogResult result = dialogProgress.ShowDialog(this);

                // 結果判定
                if (result == DialogResult.Cancel)
                {
                    // メッセージ表示
                    MessageBox.Show("ファイルインポート(Oudia)をキャンセルしました。", "キャンセル", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                else if (result == DialogResult.Abort)
                {
                    // メッセージ表示
                    MessageBox.Show("ファイルインポート(Oudia)が異常終了しました。", "異常終了", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (result == DialogResult.OK)
                {
                    // RoutePropertiesオブジェクト取得
                    RouteFileProperty routeProperties = (RouteFileProperty)dialogProgress.Result;

                    // FormRouteフォーム更新
                    formRoute.Update(routeProperties);

                    // FormRouteフォーム登録
                    m_FormRoutes.Regston(dbFileName, formRoute);

                    // FormRouteフォーム表示
                    formRoute.Show();
                }
            }

            // ロギング
            Logger.Debug("<<<<= FormMain::toolStripMenuItemFileImportOudia_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripMenuItemFileImportOudia2_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemFileImportOudia2_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormMain::toolStripMenuItemFileImportOudia2_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // インポートファイルを選択する
            string fileName = SelectImportFile("OuDiaSecondV2 路線ファイル(*.oud2)|*.oud2");

            // ファイル名判定
            if (fileName == string.Empty)
            {
                // ロギング
                Logger.Debug("<<<<= FormMain::toolStripMenuItemFileImportOudia2_Click(object, EventArgs)");

                // 正常以外なので何もしない
                return;
            }

            // データベースファイル名作成
            string dbFileName = Path.ChangeExtension(fileName, "db");

            // 登録判定
            if (m_FormRoutes.IsExist(dbFileName))
            {
                // メッセージ表示
                MessageBox.Show(
                    "同一ファイルの同時編集はできません。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // ロギング
                Logger.WarnFormat("同一ファイルの同時編集はできません:[{0}]", fileName);
                Logger.Debug("<<<<= FormMain::toolStripMenuItemFileImportOudia2_Click(object, EventArgs)");

                // 正常以外なので何もしない
                return;
            }

            // 上書き確認
            if (!m_FormRoutes.OverwriteConfirmation(dbFileName))
            {
                // ロギング
                Logger.Debug("<<<<= FormMain::toolStripMenuItemFileImportOudia2_Click(object, EventArgs)");

                // 正常以外なので何もしない
                return;
            }

            // FormRouteフォーム生成
            FormRoute formRoute = m_FormRoutes.Create(this);

            // DialogProgressオブジェクト生成
            using (DialogProgress dialogProgress = new DialogProgress("ファイルインポート(Oudia2)", doWorkFileImportOudia2, formRoute, dbFileName, fileName))
            {
                // 進行状況ダイアログを表示する
                DialogResult result = dialogProgress.ShowDialog(this);

                // 結果判定
                if (result == DialogResult.Cancel)
                {
                    // メッセージ表示
                    MessageBox.Show("ファイルインポート(Oudia2)をキャンセルしました。", "キャンセル", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                else if (result == DialogResult.Abort)
                {
                    // メッセージ表示
                    MessageBox.Show("ファイルインポート(Oudia2)が異常終了しました。", "異常終了", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (result == DialogResult.OK)
                {
                    // RoutePropertiesオブジェクト取得
                    RouteFileProperty routeProperties = (RouteFileProperty)dialogProgress.Result;

                    // FormRouteフォーム更新
                    formRoute.Update(routeProperties);

                    // FormRouteフォーム登録
                    m_FormRoutes.Regston(dbFileName, formRoute);

                    // FormRouteフォーム表示
                    formRoute.Show();
                }
            }

            // ロギング
            Logger.Debug("<<<<= FormMain::toolStripMenuItemFileImportOudia2_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripMenuItemFileExit_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemFileExit_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormMain::toolStripMenuItemFileExit_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // アプリケーション終了
            Application.Exit();

            // ロギング
            Logger.Debug("<<<<= FormMain::toolStripMenuItemFileExit_Click(object, EventArgs)");
        }
        #endregion

        #region toolStripMenuItemHelpAboutメニューイベント
        /// <summary>
        /// toolStripMenuItemHelpAbout_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemHelpAbout_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormMain::toolStripMenuItemHelpAbout_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // バージョン情報表示
            DialogAbout dialogAbout = new DialogAbout(this);
            dialogAbout.ShowDialog();

            // ロギング
            Logger.Debug("<<<<= FormMain::toolStripMenuItemHelpAbout_Click(object, EventArgs)");
        }
        #endregion

        #region BackgroundWorkerイベント
        /// <summary>
        /// doWorkFileNewCreate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void doWorkFileNewCreate(object sender, DoWorkEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormMain::doWorkFileNewCreate(object, DoWorkEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // BackgroundWorkerオブジェクト取得
            BackgroundWorker backgroundWorker = (BackgroundWorker)sender;

            // 経過メッセージ表示
            backgroundWorker.ReportProgress(0, "0% 終了しました");

            // オブジェクト変換
            FormRoute formRoute = (FormRoute)((object[])e.Argument)[0];
            string dbFileName = (string)((object[])e.Argument)[1];

            // RouteFileDatabaseオブジェクト生成
            using (RouteFileDatabase routeFileDatabase = new RouteFileDatabase(dbFileName))
            {
                // 経過メッセージ表示
                backgroundWorker.ReportProgress(25, "25% 終了しました");

                // データベース作成
                routeFileDatabase.Create();

                // 経過メッセージ表示
                backgroundWorker.ReportProgress(50, "50% 終了しました");

                // データ読込
                RouteFileProperty routeFileProperty = routeFileDatabase.Load();

                // 経過メッセージ表示
                backgroundWorker.ReportProgress(75, "75% 終了しました");

                // 更新
                formRoute.Update(routeFileProperty);

                // 結果設定
                e.Result = routeFileProperty;

                // 完了メッセージ表示
                backgroundWorker.ReportProgress(100, "100% 終了しました");
            }

            // ロギング
            Logger.Debug("<<<<= FormMain::doWorkFileNewCreate(object, DoWorkEventArgs)");
        }

        /// <summary>
        /// doWorkFileOpen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void doWorkFileOpen(object sender, DoWorkEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormMain::doWorkFileOpen(object, DoWorkEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // BackgroundWorkerオブジェクト取得
            BackgroundWorker backgroundWorker = (BackgroundWorker)sender;

            // 経過メッセージ表示
            backgroundWorker.ReportProgress(0, "0% 終了しました");

            // オブジェクト変換
            FormRoute formRoute = (FormRoute)((object[])e.Argument)[0];
            string dbFileName = (string)((object[])e.Argument)[1];

            // RouteFileDatabaseオブジェクト生成
            using (RouteFileDatabase routeFileDatabase = new RouteFileDatabase(dbFileName))
            {
                // 経過メッセージ表示
                backgroundWorker.ReportProgress(33, "33% 終了しました");

                // データ読込
                RouteFileProperty routeFileProperty = routeFileDatabase.Load();

                // 経過メッセージ表示
                backgroundWorker.ReportProgress(66, "66% 終了しました");

                // 更新
                formRoute.Update(routeFileProperty);

                // 結果設定
                e.Result = routeFileProperty;

                // 経過メッセージ表示
                backgroundWorker.ReportProgress(100, "100% 終了しました");
            }

            // ロギング
            Logger.Debug("<<<<= FormMain::doWorkFileOpen(object, DoWorkEventArgs)");
        }

        /// <summary>
        /// doWorkFileImportWinDIA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void doWorkFileImportWinDIA(object sender, DoWorkEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormMain::doWorkFileImportWinDIA(object, DoWorkEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // BackgroundWorkerオブジェクト取得
            BackgroundWorker backgroundWorker = (BackgroundWorker)sender;

            // 経過メッセージ表示
            backgroundWorker.ReportProgress(0, "0% 終了しました");

            // オブジェクト変換
            FormRoute formRoute = (FormRoute)((object[])e.Argument)[0];
            string dbFileName = (string)((object[])e.Argument)[1];
            string fileName = (string)((object[])e.Argument)[2];

            // 経過メッセージ表示
            backgroundWorker.ReportProgress(20, "20% 終了しました");

            // RouteFileDatabaseオブジェクト生成
            using (RouteFileDatabase RouteFileDatabase = new RouteFileDatabase(dbFileName))
            {
                // 経過メッセージ表示
                backgroundWorker.ReportProgress(40, "40% 終了しました");

                // データベース作成
                RouteFileDatabase.Create();

                // 経過メッセージ表示
                backgroundWorker.ReportProgress(60, "60% 終了しました");

                // インポート
                RouteFileDatabase.Import(ImportFileType.WinDIA, fileName);

                // 経過メッセージ表示
                backgroundWorker.ReportProgress(80, "80% 終了しました");

                // 読込
                RouteFileProperty routeProperties = RouteFileDatabase.Load();

                // 経過メッセージ表示
                backgroundWorker.ReportProgress(100, "100% 終了しました");

                // 結果設定
                e.Result = routeProperties;
            }

            // ロギング
            Logger.DebugFormat("e.Result:[{0}]", e.Result);
            Logger.Debug("<<<<= FormMain::doWorkFileImportWinDIA(object, DoWorkEventArgs)");
        }

        /// <summary>
        /// doWorkFileImportOudia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void doWorkFileImportOudia(object sender, DoWorkEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormMain::doWorkFileImportOudia(object, DoWorkEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // BackgroundWorkerオブジェクト取得
            BackgroundWorker backgroundWorker = (BackgroundWorker)sender;

            // 経過メッセージ表示
            backgroundWorker.ReportProgress(0, "0% 終了しました");

            // オブジェクト変換
            FormRoute formRoute = (FormRoute)((object[])e.Argument)[0];
            string dbFileName = (string)((object[])e.Argument)[1];
            string fileName = (string)((object[])e.Argument)[2];

            // 経過メッセージ表示
            backgroundWorker.ReportProgress(20, "20% 終了しました");

            // RouteFileDatabaseオブジェクト生成
            using (RouteFileDatabase RouteFileDatabase = new RouteFileDatabase(dbFileName))
            {
                // 経過メッセージ表示
                backgroundWorker.ReportProgress(40, "40% 終了しました");

                // データベース作成
                RouteFileDatabase.Create();

                // 経過メッセージ表示
                backgroundWorker.ReportProgress(60, "60% 終了しました");

                // インポート
                RouteFileDatabase.Import(ImportFileType.OuDia, fileName);

                // 経過メッセージ表示
                backgroundWorker.ReportProgress(80, "80% 終了しました");

                // 読込
                RouteFileProperty routeProperties = RouteFileDatabase.Load();

                // 経過メッセージ表示
                backgroundWorker.ReportProgress(100, "100% 終了しました");

                // 結果設定
                e.Result = routeProperties;
            }

            // ロギング
            Logger.DebugFormat("e.Result:[{0}]", e.Result);
            Logger.Debug("<<<<= FormMain::doWorkFileImportOudia(object, DoWorkEventArgs)");
        }

        /// <summary>
        /// doWorkFileImportOudia2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void doWorkFileImportOudia2(object sender, DoWorkEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormMain::doWorkFileImportOudia2(object, DoWorkEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // BackgroundWorkerオブジェクト取得
            BackgroundWorker backgroundWorker = (BackgroundWorker)sender;

            // 経過メッセージ表示
            backgroundWorker.ReportProgress(0, "0% 終了しました");

            // オブジェクト変換
            FormRoute formRoute = (FormRoute)((object[])e.Argument)[0];
            string dbFileName = (string)((object[])e.Argument)[1];
            string fileName = (string)((object[])e.Argument)[2];

            // 経過メッセージ表示
            backgroundWorker.ReportProgress(20, "20% 終了しました");

            // RouteFileDatabaseオブジェクト生成
            using (RouteFileDatabase RouteFileDatabase = new RouteFileDatabase(dbFileName))
            {
                // 経過メッセージ表示
                backgroundWorker.ReportProgress(40, "40% 終了しました");

                // データベース作成
                RouteFileDatabase.Create();

                // 経過メッセージ表示
                backgroundWorker.ReportProgress(60, "60% 終了しました");

                // インポート
                RouteFileDatabase.Import(ImportFileType.OuDia2, fileName);

                // 経過メッセージ表示
                backgroundWorker.ReportProgress(80, "80% 終了しました");

                // 読込
                RouteFileProperty routeProperties = RouteFileDatabase.Load();

                // 経過メッセージ表示
                backgroundWorker.ReportProgress(100, "100% 終了しました");

                // 結果設定
                e.Result = routeProperties;
            }

            // ロギング
            Logger.DebugFormat("e.Result:[{0}]", e.Result);
            Logger.Debug("<<<<= FormMain::doWorkFileImportOudia2(object, DoWorkEventArgs)");
        }
        #endregion
        #endregion

        #region privateメソッド
        #region ファイル選択
        /// <summary>
        /// オープンファイル選択
        /// </summary>
        /// <param name="checkFileExists"></param>
        /// <returns></returns>
        private string SelectOpenFile(bool checkFileExists)
        {
            // ロギング
            Logger.Debug("=>>>> FormMain::SelectOpenFile(bool)");
            Logger.DebugFormat("checkFileExists:{0}", checkFileExists);

            // OpenFileDialogクラスのインスタンスを作成
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // はじめのファイル名を指定する
            // はじめに「ファイル名」で表示される文字列を指定する
            openFileDialog.FileName = Const.DefaultFileName;

            // [ファイルの種類]に表示される選択肢を指定する
            // 指定しないとすべてのファイルが表示される
            openFileDialog.Filter = "時刻表ファイル(*.db)|*.db";

            // タイトルを設定する
            openFileDialog.Title = "時刻表ファイルを選択してください";

            // ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            openFileDialog.RestoreDirectory = true;

            // 存在しないファイルの名前が指定されたとき警告を表示する
            // デフォルトでTrueなので指定する必要はない
            openFileDialog.CheckFileExists = checkFileExists;

            // 存在しないパスが指定されたとき警告を表示する
            // デフォルトでTrueなので指定する必要はない
            openFileDialog.CheckPathExists = true;

            // ダイアログを表示する
            DialogResult result = openFileDialog.ShowDialog();

            // ダイアログ表示結果判定
            if (result != DialogResult.OK)
            {
                // ロギング
                Logger.DebugFormat("result:{0}", result);
                Logger.Debug("<<<<= FormMain::SelectOpenFile(bool)");

                // 空のファイル名を返却
                return string.Empty;
            }

            // ロギング
            Logger.DebugFormat("FileName:{0}", openFileDialog.FileName);
            Logger.Debug("<<<<= FormMain::SelectOpenFile(bool)");

            // ファイル名を返却
            return openFileDialog.FileName;
        }

        /// <summary>
        /// インポートファイル選択
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        private string SelectImportFile(string filter)
        {
            // ロギング
            Logger.Debug("=>>>> FormMain::SelectImportFile(string)");
            Logger.DebugFormat("filter:{0}", filter);

            // OpenFileDialogクラスのインスタンスを作成
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // [ファイルの種類]に表示される選択肢を指定する
            // 指定しないとすべてのファイルが表示される
            openFileDialog.Filter = filter;

            // タイトルを設定する
            openFileDialog.Title = "路線ファイルを選択してください";
            // ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            openFileDialog.RestoreDirectory = true;

            // 存在しないファイルの名前が指定されたとき警告を表示する
            // デフォルトでTrueなので指定する必要はない
            openFileDialog.CheckFileExists = true;

            // 存在しないパスが指定されたとき警告を表示する
            // デフォルトでTrueなので指定する必要はない
            openFileDialog.CheckPathExists = true;

            // ダイアログを表示する
            DialogResult result = openFileDialog.ShowDialog();

            // ダイアログ表示結果判定
            if (result != DialogResult.OK)
            {
                // ロギング
                Logger.DebugFormat("result:{0}", result);
                Logger.Debug("<<<<= FormMain::SelectImportFile(string)");

                // 空のファイル名を返却
                return string.Empty;
            }

            // ロギング
            Logger.DebugFormat("FileName:{0}", openFileDialog.FileName);
            Logger.Debug("<<<<= FormMain::SelectImportFile(string)");

            // ファイル名を返却
            return openFileDialog.FileName;
        }
        #endregion
        #endregion
    }
}
