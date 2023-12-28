using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Common;
using TrainTimeTable.Control;
using TrainTimeTable.Database;
using TrainTimeTable.Dialog;
using TrainTimeTable.EventArgs;
using TrainTimeTable.Property;

namespace TrainTimeTable
{
    /// <summary>
    /// FormRouteクラス
    /// </summary>
    public partial class FormRoute : Form
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region データベースファイル名
        /// <summary>
        /// データベースファイル名
        /// </summary>
        public string FileName { get; set; }
        #endregion

        #region RouteFilePropertyオブジェクト
        /// <summary>
        /// TimetablePropertyオブジェクト(旧データ)
        /// </summary>
        private RouteFileProperty m_OriginalRouteFileProperty = new RouteFileProperty();

        /// <summary>
        /// TimetablePropertyオブジェクト(現行データ)
        /// </summary>
        private RouteFileProperty m_CurrentRouteFileProperty = null;
        #endregion

        #region TreeViewRouteオブジェクト
        /// <summary>
        /// TreeViewRouteオブジェクト
        /// </summary>
        private TreeViewRoute m_TreeViewRoute = new TreeViewRoute();
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormRoute()
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::FormRoute()");

            // コンポーネント初期化
            InitializeComponent();

            // ロギング
            Logger.Debug("<<<<= FormRoute::FormRoute()");
        }
        #endregion

        #region イベント
        #region FormRouteイベント
        /// <summary>
        /// FormRoute_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormRoute_Load(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::FormRoute_Load(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 設定
            menuStripMain.Visible = false;
            splitContainerMain.Dock = DockStyle.Fill;
            splitContainerMain.Panel1.Controls.Add(m_TreeViewRoute);
            m_TreeViewRoute.Dock = DockStyle.Fill;
            m_TreeViewRoute.OnNodeRootMouseClick += TreeViewRoute_OnNodeRootMouseClick;
            m_TreeViewRoute.OnNodeStationMouseClick += TreeViewRoute_OnNodeStationMouseClick;
            m_TreeViewRoute.OnNodeTrainTypeMouseClick += TreeViewRoute_OnNodeTrainTypeMouseClick;
            m_TreeViewRoute.OnNodeDiaMouseDoubleClick += TreeViewRoute_OnNodeDiaMouseDoubleClick;
            m_TreeViewRoute.OnNodeCommentMouseClick += TreeViewRoute_OnNodeCommentMouseClick;
            m_TreeViewRoute.OnNodeOutboundTimetableMouseClick += TreeViewRoute_OnNodeOutboundTimetableMouseClick;
            m_TreeViewRoute.OnNodeInboundTimetableMouseClick += TreeViewRoute_OnNodeInboundTimetableMouseClick;
            m_TreeViewRoute.OnNodeDiagramMouseClick += TreeViewRoute_OnNodeDiagramMouseClick;
            m_TreeViewRoute.OnNodeStationTimetableMouseClick += TreeViewRoute_OnNodeStationTimetableMouseClick;

            // タイトル設定
            SetTitle();

            // ロギング
            Logger.Debug("<<<<= FormRoute::FormRoute_Load(object, EventArgs)");
        }

        /// <summary>
        /// FormRoute_Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormRoute_Closing(object sender, FormClosingEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::FormRoute_Closing(object, FormClosingEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 変更されているか判定
            if (m_CurrentRouteFileProperty != null)
            {
                // 変更判定
                if (!m_CurrentRouteFileProperty.Compare(m_OriginalRouteFileProperty))
                {
                    // 保存処理確認
                    if (MessageBox.Show(
                        string.Format("以下のデータが変更されています。保存しますか？\r\n\r\n路線:[{0}]", m_CurrentRouteFileProperty.Route.Name),
                        "保存確認",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                    {
                        // 保存しない
                        return;
                    }

                    // DialogProgressオブジェクト生成
                    using (DialogProgress dialogProgress = new DialogProgress("データベース書き込み", doWorkDatabaseWrite))
                    {
                        // 進行状況ダイアログを表示する
                        DialogResult result = dialogProgress.ShowDialog(this);

                        // 結果判定
                        if (result == DialogResult.Cancel)
                        {
                            // メッセージ表示
                            MessageBox.Show("データベース書き込みをキャンセルしました。", "キャンセル", MessageBoxButtons.OK, MessageBoxIcon.Question);
                        }
                        else if (result == DialogResult.Abort)
                        {
                            // メッセージ表示
                            MessageBox.Show("データベース書き込みが異常終了しました。", "異常終了", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (result == DialogResult.OK)
                        {
                            // 何もしない
                        }
                    }
                }
            }

            // ロギング
            Logger.Debug("<<<<= FormRoute::FormRoute_Closing(object, FormClosingEventArgs)");
        }

        /// <summary>
        /// FormRoute_Resize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormRoute_Resize(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::FormRoute_Resize(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= FormRoute::FormRoute_Resize(object, EventArgs)");
        }
        #endregion

        #region メニューイベント
        #region toolStripMenuItemFileメニューイベント
        /// <summary>
        /// toolStripMenuItemFileClose_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemFileClose_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::toolStripMenuItemFileClose_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // フォームクローズ
            Close();

            // ロギング
            Logger.Debug("<<<<= FormRoute::toolStripMenuItemFileClose_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripMenuItemFileSave_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemFileSave_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::toolStripMenuItemFileSave_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 時刻表ファイル保存
            SaveTimetableFile();

            // ロギング
            Logger.Debug("<<<<= FormRoute::toolStripMenuItemFileSave_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripMenuItemFileSaveAs_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemFileSaveAs_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::toolStripMenuItemFileSaveAs_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 時刻表ファイルを選択する
            string fileName = SelectSaveTimetableFile();

            // ファイル名判定
            if (fileName == string.Empty)
            {
                // ロギング
                Logger.Debug("<<<<= FormRoute::toolStripMenuItemFileSaveAs_Click(object, EventArgs)");

                // 正常以外なので何もしない
                return;
            }

            // ファイル名設定
            FileName = fileName;

            // 時刻表ファイル保存
            SaveTimetableFile();

            // ロギング
            Logger.Debug("<<<<= FormRoute::toolStripMenuItemFileSaveAs_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripMenuItemFileExport_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemFileExport_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::toolStripMenuItemFileExport_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= FormRoute::toolStripMenuItemFileExport_Click(object, EventArgs)");
        }
        #endregion

        #region toolStripMenuItemWindowメニューイベント
        /// <summary>
        /// toolStripMenuItemWindowDisplayOverlapping_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemWindowDisplayOverlapping_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::toolStripMenuItemWindowDisplayOverlapping_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // コントロール登録判定
            if (splitContainerMain.Panel2.Controls.Count == 0)
            {
                // ロギング
                Logger.Debug("<<<<= FormRoute::toolStripMenuItemWindowDisplayOverlapping_Click(object, EventArgs)");
                return;
            }

            // 登録されているコントロールを繰り返す
            int x = 0, y = 0;
            int titleBarHeight = WindowLibrary.GetTitleBarHeight();
            foreach (Form form in splitContainerMain.Panel2.Controls)
            {
                // Formオブジェクト設定
                form.WindowState = FormWindowState.Normal;
                form.Location = new Point(x, y);
                form.BringToFront();
                x += titleBarHeight;
                y += titleBarHeight;
            }

            // ロギング
            Logger.Debug("<<<<= FormRoute::toolStripMenuItemWindowDisplayOverlapping_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripMenuItemWindowDisplayVertically_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemWindowDisplayVertically_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::toolStripMenuItemWindowDisplayVertically_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // コントロール登録判定
            if (splitContainerMain.Panel2.Controls.Count == 0)
            {
                // ロギング
                Logger.Debug("<<<<= FormRoute::toolStripMenuItemWindowDisplayVertically_Click(object, EventArgs)");
                return;
            }

            // 列取得、行数取得
            int columns = 1;
            int rows = 1;
            if (splitContainerMain.Panel2.Controls.Count > 2)
            {
                columns = 2;
                rows = (int)Math.Ceiling((double)splitContainerMain.Panel2.Controls.Count / 2);
            }
            else
            {
                columns = 1;
                rows = splitContainerMain.Panel2.Controls.Count;
            }

            // 最大サイズ取得
            int maxWidth = splitContainerMain.Panel2.Width;
            int maxHeight = splitContainerMain.Panel2.Height - (WindowLibrary.GetTitleBarHeight() * rows);

            // 単位当たりの幅、高さを計算
            int widthUnit = maxWidth / columns;
            int heightUnit = maxHeight / rows;

            // ロギング
            Logger.DebugFormat("columns   :[{0}]", columns);
            Logger.DebugFormat("rows      :[{0}]", rows);
            Logger.DebugFormat("maxWidth  :[{0}]", maxWidth);
            Logger.DebugFormat("maxHeight :[{0}]", maxHeight);
            Logger.DebugFormat("widthUnit :[{0}]", widthUnit);
            Logger.DebugFormat("heightUnit:[{0}]", heightUnit);

            // 登録されているコントロールを繰り返す
            int columnsCount = 0, rowsCount = 0;
            foreach (Form form in splitContainerMain.Panel2.Controls)
            {
                // Formオブジェクト設定
                form.WindowState = FormWindowState.Normal;
                form.Location = new Point(columnsCount * widthUnit, rowsCount * heightUnit);
                form.Width = widthUnit;
                form.Height = heightUnit;
                form.BringToFront();

                // カウンタ計算
                rowsCount++;
                if (rowsCount >= rows)
                {
                    columnsCount++;
                    rowsCount = 0;
                }
            }

            // ロギング
            Logger.Debug("<<<<= FormRoute::toolStripMenuItemWindowDisplayVertically_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripMenuItemWindowDisplaySideBySide_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemWindowDisplaySideBySide_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::toolStripMenuItemWindowDisplaySideBySide_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // コントロール登録判定
            if (splitContainerMain.Panel2.Controls.Count == 0)
            {
                // ロギング
                Logger.Debug("<<<<= FormRoute::toolStripMenuItemWindowDisplaySideBySide_Click(object, EventArgs)");
                return;
            }

            // 列取得、行数取得
            int columns = 1;
            int rows = 1;
            if (splitContainerMain.Panel2.Controls.Count > 2)
            {
                rows = 2;
                columns = (int)Math.Ceiling((double)splitContainerMain.Panel2.Controls.Count / 2);
            }
            else
            {
                rows = 1;
                columns = splitContainerMain.Panel2.Controls.Count;
            }

            // 最大サイズ取得
            int maxWidth = splitContainerMain.Panel2.Width;
            int maxHeight = splitContainerMain.Panel2.Height - (WindowLibrary.GetTitleBarHeight() * rows);

            // 単位当たりの幅、高さを計算
            int widthUnit = maxWidth / columns;
            int heightUnit = maxHeight / rows;

            // ロギング
            Logger.DebugFormat("columns   :[{0}]", columns);
            Logger.DebugFormat("rows      :[{0}]", rows);
            Logger.DebugFormat("maxWidth  :[{0}]", maxWidth);
            Logger.DebugFormat("maxHeight :[{0}]", maxHeight);
            Logger.DebugFormat("widthUnit :[{0}]", widthUnit);
            Logger.DebugFormat("heightUnit:[{0}]", heightUnit);

            // 登録されているコントロールを繰り返す
            int columnsCount = 0, rowsCount = 0;
            foreach (Form form in splitContainerMain.Panel2.Controls)
            {
                // Formオブジェクト設定
                form.WindowState = FormWindowState.Normal;
                form.Location = new Point(columnsCount * widthUnit, rowsCount * heightUnit);
                form.Width = widthUnit;
                form.Height = heightUnit;
                form.BringToFront();

                // カウンタ計算
                columnsCount++;
                if (columnsCount >= columns)
                {
                    rowsCount++;
                    columnsCount = 0;
                }
            }

            // ロギング
            Logger.Debug("<<<<= FormRoute::toolStripMenuItemWindowDisplaySideBySide_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripMenuItemWindowArrangeIcons_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemWindowArrangeIcons_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::toolStripMenuItemWindowArrangeIcons_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // コントロール登録判定
            if (splitContainerMain.Panel2.Controls.Count == 0)
            {
                // ロギング
                Logger.Debug("<<<<= FormRoute::toolStripMenuItemWindowArrangeIcons_Click(object, EventArgs)");
                return;
            }

            // 登録されているコントロールを繰り返す
            foreach (Form form in splitContainerMain.Panel2.Controls)
            {
                // Formオブジェクト設定
                form.WindowState = FormWindowState.Maximized;
            }

            // ロギング
            Logger.Debug("<<<<= FormRoute::toolStripMenuItemWindowArrangeIcons_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripMenuItemWindowCloseAll_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemWindowCloseAll_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::toolStripMenuItemWindowCloseAll_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // コントロール登録判定
            if (splitContainerMain.Panel2.Controls.Count == 0)
            {
                // ロギング
                Logger.Debug("<<<<= FormRoute::toolStripMenuItemWindowCloseAll_Click(object, EventArgs)");
                return;
            }

            // 登録をすべて解除
            splitContainerMain.Panel2.Controls.Clear();

            // ロギング
            Logger.Debug("<<<<= FormRoute::toolStripMenuItemWindowCloseAll_Click(object, EventArgs)");
        }
        #endregion
        #endregion

        #region TreeViewRoutイベント
        #region MouseClick
        /// <summary>
        /// TreeViewRoute_OnNodeRootMouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void TreeViewRoute_OnNodeRootMouseClick(object sender, TreeNodeRoot e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::TreeViewRoute_OnNodeRootMouseClick(object, TreeNodeRoot)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // フォームオブジェクト生成
            FormRouteFileProperty form = new FormRouteFileProperty(m_CurrentRouteFileProperty);

            // イベント設定
            form.OnUpdate += FormRouteFileProperty_OnUpdate;

            // フォーム表示
            form.StartPosition = FormStartPosition.CenterParent;
            form.WindowState = FormWindowState.Normal;

            // フォーム表示結果判定
            if (form.ShowDialog() != DialogResult.OK)
            {
                // ロギング
                Logger.Debug("<<<<= FormRoute::TreeViewRoute_OnNodeRootMouseClick(object, TreeNodeRoot)");

                // 処理終了
                return;
            }

            // ロギング
            Logger.Debug("<<<<= FormRoute::TreeViewRoute_OnNodeRootMouseClick(object, TreeNodeRoot)");
        }

        /// <summary>
        /// TreeViewRoute_OnNodeStationMouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewRoute_OnNodeStationMouseClick(object sender, TreeNodeStation e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::TreeViewRoute_OnNodeStationMouseClick(object, TreeNodeStation)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 登録判定
            if (!IsMDIChildForm(typeof(FormStationProperties)))
            {
                // フォームオブジェクト生成
                FormStationProperties form = new FormStationProperties(m_CurrentRouteFileProperty);

                // イベント設定
                form.OnUpdate += FormStation_OnUpdate;

                // フォーム登録
                AddMDIChildForm(form);

                // フォーム表示
                form.Show();
            }
            else
            {
                // フォーム取得
                FormStationProperties form = GetMDIChildForm(typeof(FormStationProperties)) as FormStationProperties;

                // フォームを前面表示する
                form.BringToFront();
                form.WindowState = FormWindowState.Normal;
            }

            // ロギング
            Logger.Debug("<<<<= FormRoute::TreeViewRoute_OnNodeStationMouseClick(object, TreeNodeStation)");
        }

        /// <summary>
        /// TreeViewRoute_OnNodeTrainTypeMouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewRoute_OnNodeTrainTypeMouseClick(object sender, TreeNodeTrainType e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::TreeViewRoute_OnNodeTrainTypeMouseClick(object, TreeNodeTrainType)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 登録判定
            if (!IsMDIChildForm(typeof(FormTrainTypeProperties)))
            {
                // フォームオブジェクト生成
                FormTrainTypeProperties form = new FormTrainTypeProperties(m_CurrentRouteFileProperty.Fonts, m_CurrentRouteFileProperty.TrainTypes);

                // イベント設定
                form.OnUpdate += FormTrainType_OnUpdate;

                // フォーム登録
                AddMDIChildForm(form);

                // フォーム表示
                form.Show();
            }
            else
            {
                // フォーム取得
                FormTrainTypeProperties form = GetMDIChildForm(typeof(FormTrainTypeProperties)) as FormTrainTypeProperties;

                // フォームを前面表示する
                form.BringToFront();
                form.WindowState = FormWindowState.Normal;
            }

            // ロギング
            Logger.Debug("<<<<= FormRoute::TreeViewRoute_OnNodeTrainTypeMouseClick(object, TreeNodeTrainType)");
        }

        /// <summary>
        /// TreeViewRoute_OnNodeDiaMouseDoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewRoute_OnNodeDiaMouseDoubleClick(object sender, TreeNodeDiagram e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::TreeViewRoute_OnNodeDiaMouseDoubleClick(object, TreeNodeDia)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 登録判定
            if (!IsMDIChildForm(typeof(FormDiagramProperties)))
            {
                // フォームオブジェクト生成
                FormDiagramProperties form = new FormDiagramProperties(m_CurrentRouteFileProperty);

                // イベント設定
                form.OnUpdate += FormDiagramProperties_OnUpdate;

                // フォーム表示
                form.StartPosition = FormStartPosition.CenterParent;

                // フォーム表示結果判定
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // TODO:データ処理
                }
            }

            // ロギング
            Logger.Debug("<<<<= FormRoute::TreeViewRoute_OnNodeDiaMouseDoubleClick(object, TreeNodeDia)");
        }

        /// <summary>
        /// TreeViewRoute_OnNodeCommentMouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewRoute_OnNodeCommentMouseClick(object sender, TreeNodeComment e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::TreeViewRoute_OnNodeCommentMouseClick(object, TreeNodeComment)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 登録判定
            if (!IsMDIChildForm(typeof(FormComment)))
            {
                // フォームオブジェクト生成
                FormComment form = new FormComment(m_CurrentRouteFileProperty.Comment);

                // イベント設定
                form.OnUpdate += FormComment_OnUpdate;

                // フォーム登録
                AddMDIChildForm(form);

                // フォーム表示
                form.Show();
            }
            else
            {
                // フォーム取得
                FormComment form = GetMDIChildForm(typeof(FormComment)) as FormComment;

                // フォームを前面表示する
                form.BringToFront();
                form.WindowState = FormWindowState.Normal;
            }

            // ロギング
            Logger.Debug("<<<<= FormRoute::TreeViewRoute_OnNodeCommentMouseClick(object, TreeNodeComment)");
        }

        /// <summary>
        /// TreeViewRoute_OnNodeOutboundTimetableMouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewRoute_OnNodeOutboundTimetableMouseClick(object sender, TreeNodeOutboundTimetable e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::TreeViewRoute_OnNodeOutboundTimetableMouseClick(object, TreeNodeOutboundTimetable)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 登録名作成
            string regName = string.Format("{0} {1}", e.Parent.Text, e.Text);

            // 登録判定
            if (!IsMDIChildForm(typeof(FormTimetable), regName))
            {
                // フォームオブジェクト生成
                FormTimetable form = new FormTimetable(this, e.Parent.Text, regName, DirectionType.Outbound, m_CurrentRouteFileProperty);

                // イベント設定
                form.OnUpdate += FormOutboundTimetable_OnUpdate;

                // フォーム登録
                AddMDIChildForm(form);

                // フォーム表示
                form.Show();
            }
            else
            {
                // フォーム取得
                FormTimetable form = GetMDIChildForm(typeof(FormTimetable), regName) as FormTimetable;

                // フォームを前面表示する
                form.BringToFront();
                form.WindowState = FormWindowState.Normal;
            }

            // ロギング
            Logger.Debug("<<<<= FormRoute::TreeViewRoute_OnNodeOutboundTimetableMouseClick(object, TreeNodeOutboundTimetable)");
        }

        /// <summary>
        /// TreeViewRoute_OnNodeInboundTimetableMouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewRoute_OnNodeInboundTimetableMouseClick(object sender, TreeNodeInboundTimeTable e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::TreeViewRoute_OnNodeInboundTimetableMouseClick(object, TreeNodeInboundTimeTable)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 登録名作成
            string regName = string.Format("{0} {1}", e.Parent.Text, e.Text);

            // 登録判定
            if (!IsMDIChildForm(typeof(FormTimetable), regName))
            {
                // フォームオブジェクト生成
                FormTimetable form = new FormTimetable(this, e.Parent.Text, regName, DirectionType.Inbound, m_CurrentRouteFileProperty);

                // イベント設定
                form.OnUpdate += FormInboundTimetable_OnUpdate;

                // フォーム登録
                AddMDIChildForm(form);

                // フォーム表示
                form.Show();
            }
            else
            {
                // フォーム取得
                FormTimetable form = GetMDIChildForm(typeof(FormTimetable), regName) as FormTimetable;

                // フォームを前面表示する
                form.BringToFront();
                form.WindowState = FormWindowState.Normal;
            }

            // ロギング
            Logger.Debug("<<<<= FormRoute::TreeViewRoute_OnNodeInboundTimetableMouseClick(object, TreeNodeInboundTimeTable)");
        }

        /// <summary>
        /// TreeViewRoute_OnNodeDiagramMouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewRoute_OnNodeDiagramMouseClick(object sender, TreeNodeDiagramDraw e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::TreeViewRoute_OnNodeDiagramMouseClick(object, TreeNodeDiagram)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 登録名作成
            string regName = string.Format("{0} {1}", e.Parent.Text, e.Text);

            // 登録判定
            if (!IsMDIChildForm(typeof(FormDiagramDisplay), regName))
            {
                // フォームオブジェクト生成
                FormDiagramDisplay form = new FormDiagramDisplay(this, regName, e.Parent.Text, m_CurrentRouteFileProperty);

                // イベント設定
                form.OnUpdate += FormDiagramDraw_OnUpdate;

                // フォーム登録
                AddMDIChildForm(form);

                // フォーム表示
                form.Show();
            }
            else
            {
                // フォーム取得
                FormDiagramDisplay form = GetMDIChildForm(typeof(FormDiagramDisplay), regName) as FormDiagramDisplay;

                // フォームを前面表示する
                form.BringToFront();
                form.WindowState = FormWindowState.Normal;
            }

            // ロギング
            Logger.Debug("<<<<= FormRoute::TreeViewRoute_OnNodeDiagramMouseClick(object, TreeNodeDiagram)");
        }

        /// <summary>
        /// TreeViewRoute_OnNodeStationTimetableMouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewRoute_OnNodeStationTimetableMouseClick(object sender, TreeNodeStationTimetable e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::TreeViewRoute_OnNodeStationTimetableMouseClick(object, TreeNodeStationTimetable)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 登録名作成
            string regName = string.Format("{0} {1}", e.Parent.Text, e.Text);

            // ダイアグラムインデックス取得
            int diagramIndex = m_CurrentRouteFileProperty.Diagrams.GetIndex(e.Parent.Text);

            // 登録判定
            if (!IsMDIChildForm(typeof(FormStationTimetable), regName))
            {
                // フォームオブジェクト生成
                FormStationTimetable form = new FormStationTimetable(this, regName, diagramIndex, m_CurrentRouteFileProperty);

                // イベント設定
                form.OnUpdate += FormStationTimetable_OnUpdate;

                // フォーム登録
                AddMDIChildForm(form);

                // フォーム表示
                form.Show();
            }
            else
            {
                // フォーム取得
                FormStationTimetable form = GetMDIChildForm(typeof(FormStationTimetable), regName) as FormStationTimetable;

                // フォームを前面表示する
                form.BringToFront();
                form.WindowState = FormWindowState.Normal;
            }

            // ロギング
            Logger.Debug("<<<<= FormRoute::TreeViewRoute_OnNodeStationTimetableMouseClick(object, TreeNodeStationTimetable)");
        }
        #endregion
        #endregion

        #region データ更新イベント
        /// <summary>
        /// FormRouteFileProperty_OnUpdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void FormRouteFileProperty_OnUpdate(object sender, RouteFilePropertyUpdateEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::FormRouteFileProperty_OnUpdate(object, RouteFilePropertyUpdateEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // コピー
            m_CurrentRouteFileProperty.Copy(e.Property);

            // 更新通知
            UpdateNotification();

            // ロギング
            Logger.Debug("<<<<= FormRoute::FormRouteFileProperty_OnUpdate(object, RouteFilePropertyUpdateEventArgs)");
        }

        /// <summary>
        /// FormStation_OnUpdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormStation_OnUpdate(object sender, StationPropertiesUpdateEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::FormStation_OnUpdate(object, StationPropertiesUpdateEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // コピー
            m_CurrentRouteFileProperty.Stations.Copy(e.Properties);

            // 更新通知
            UpdateNotification();

            // ロギング
            Logger.Debug("<<<<= FormRoute::FormStation_OnUpdate(object, StationPropertiesUpdateEventArgs)");
        }

        /// <summary>
        /// FormTrainType_OnUpdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormTrainType_OnUpdate(object sender, TrainTypePropertiesUpdateEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::FormTrainType_OnUpdate(object, TrainTypePropertiesUpdateEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // コピー
            m_CurrentRouteFileProperty.TrainTypes.Copy(e.Property);

            // 更新通知
            UpdateNotification();

            // ロギング
            Logger.Debug("<<<<= FormRoute::FormTrainType_OnUpdate(object, TrainTypePropertiesUpdateEventArgs)");
        }

        /// <summary>
        /// FormDiagramProperties_OnUpdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormDiagramProperties_OnUpdate(object sender, DiagramPropertiesUpdateEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::FormDiagramProperties_OnUpdate(object, DiagramPropertiesUpdateEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= FormRoute::FormDiagramProperties_OnUpdate(object, DiagramPropertiesUpdateEventArgs)");
        }

        /// <summary>
        /// FormComment_OnUpdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormComment_OnUpdate(object sender, CommentUpdateEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::FormComment_OnUpdate(object, CommentUpdateEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // コピー
            m_CurrentRouteFileProperty.Comment.Clear();
            m_CurrentRouteFileProperty.Comment.Append(e.Comment);

            // 更新通知
            UpdateNotification();

            // ロギング
            Logger.Debug("<<<<= FormRoute::FormComment_OnUpdate(object, CommentUpdateEventArgs)");
        }

        /// <summary>
        /// FormOutboundTimetable_OnUpdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormOutboundTimetable_OnUpdate(object sender, TimetableUpdateEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::FormOutboundTimetable_OnUpdate(object, TimetableUpdateEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 更新通知
            UpdateNotification();

            // ロギング
            Logger.Debug("<<<<= FormRoute::FormOutboundTimetable_OnUpdate(object, TimetableUpdateEventArgs)");
        }

        /// <summary>
        /// FormInboundTimetable_OnUpdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormInboundTimetable_OnUpdate(object sender, TimetableUpdateEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::FormInboundTimetable_OnUpdate(object, TimetableUpdateEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 更新通知
            UpdateNotification();

            // ロギング
            Logger.Debug("<<<<= FormRoute::FormInboundTimetable_OnUpdate(object, TimetableUpdateEventArgs)");
        }

        /// <summary>
        /// FormDiagramDraw_OnUpdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormDiagramDraw_OnUpdate(object sender, TimetableUpdateEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::FormDiagramDraw_OnUpdate(object, TimetableUpdateEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= FormRoute::FormDiagramDraw_OnUpdate(object, TimetableUpdateEventArgs)");
        }

        /// <summary>
        /// FormStationTimetable_OnUpdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormStationTimetable_OnUpdate(object sender, StationTimetableUpdateEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::FormStationTimetable_OnUpdate(object, StationTimetableUpdateEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= FormRoute::FormStationTimetable_OnUpdate(object, StationTimetableUpdateEventArgs)");
        }
        #endregion
        #endregion

        #region publicメソッド
        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        public void Update(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::Update(RouteProperties)");
            Logger.DebugFormat("property:[{0}]", property);

            // 設定
            m_CurrentRouteFileProperty = property;

            // コピー
            m_OriginalRouteFileProperty.Copy(m_CurrentRouteFileProperty);

            // 更新通知
            UpdateNotification();

            // タイトル設定
            SetTitle();

            // 更新
            Update();

            // ロギング
            Logger.Debug("<<<<= FormRoute::Update(RouteProperties)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public bool Compare(FormRoute form)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::Compare(FormRoute)");
            Logger.DebugFormat("form:[{0}]", form);

            // 比較
            if (FileName != form.FileName)
            {
                // ロギング
                Logger.DebugFormat("FileName:[不一致][{0}][{1}]", FileName, form.FileName);
                Logger.Debug("<<<<= FormRoute::Compare(FormRoute)");

                // 不一致
                return false;
            }

            // ロギング
            Logger.DebugFormat("result:[一致]");
            Logger.Debug("<<<<= FormRoute::Compare(FormRoute)");

            // 一致
            return true;
        }
        #endregion

        #region データベース
        #region 読込
        /// <summary>
        /// データベース読込
        /// </summary>
        public void DatabaseRead(int min, int max, BackgroundWorker backgroundWorker)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::DatabaseRead(int, int, BackgroundWorker)");
            Logger.DebugFormat("min             :[{0}]", min);
            Logger.DebugFormat("max             :[{0}]", max);
            Logger.DebugFormat("backgroundWorker:[{0}]", backgroundWorker);

            // 初期化
            int progress = min;
            int progressStep = (max - min) / 3;

            // 経過メッセージ表示
            backgroundWorker.ReportProgress(progress, string.Format("{0}% 終了しました", progress));

            // RouteFileDatabaseオブジェクト生成
            using (RouteFileDatabase routeFileDatabase = new RouteFileDatabase(FileName))
            {
                // 経過メッセージ表示
                progress += progressStep;
                backgroundWorker.ReportProgress(progress, string.Format("{0}% 終了しました", progress));

                // 読込
                m_CurrentRouteFileProperty = routeFileDatabase.Load();

                // 経過メッセージ表示
                progress += progressStep;
                backgroundWorker.ReportProgress(progress, string.Format("{0}% 終了しました", progress));
            }

            // 旧データにコピー
            m_OriginalRouteFileProperty.Copy(m_CurrentRouteFileProperty);

            // 経過メッセージ表示
            backgroundWorker.ReportProgress(max, string.Format("{0}% 終了しました", max));

            // ロギング
            Logger.Debug("<<<<= FormRoute::Read(int, int, BackgroundWorker)");
        }
        #endregion

        #region 書込
        /// <summary>
        /// データベース書込DoWork
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void doWorkDatabaseWrite(object sender, DoWorkEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::doWorkDatabaseWrite(object, DoWorkEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // BackgroundWorkerオブジェクト取得
            BackgroundWorker backgroundWorker = (BackgroundWorker)sender;

            // データベース書込
            DatabaseWrite(0, 100, backgroundWorker);

            // ロギング
            Logger.Debug("<<<<= FormRoute::doWorkDatabaseWrite(object, DoWorkEventArgs)");
        }

        /// <summary>
        /// データベース書込
        /// </summary>
        public void DatabaseWrite(int min, int max, BackgroundWorker backgroundWorker)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::DatabaseWrite(int, int, BackgroundWorker)");
            Logger.DebugFormat("min             :[{0}]", min);
            Logger.DebugFormat("max             :[{0}]", max);
            Logger.DebugFormat("backgroundWorker:[{0}]", backgroundWorker);

            // 初期化
            int progress = min;
            int progressStep = (max - min) / 4;

            // 経過メッセージ表示
            backgroundWorker.ReportProgress(progress, string.Format("{0}% 終了しました", progress));

            // RouteFileDatabaseオブジェクト生成
            using (RouteFileDatabase routeFileDatabase = new RouteFileDatabase(FileName))
            {
                // 経過メッセージ表示
                progress += progressStep;
                backgroundWorker.ReportProgress(progress, string.Format("{0}% 終了しました", progress));

                // データベース作成
                routeFileDatabase.Create();

                // 経過メッセージ表示
                progress += progressStep;
                backgroundWorker.ReportProgress(progress, string.Format("{0}% 終了しました", progress));

                // データベース再構成(キーを含む更新)
                routeFileDatabase.Rebuilding(m_CurrentRouteFileProperty);
            }

            // 経過メッセージ表示
            progress += progressStep;
            backgroundWorker.ReportProgress(progress, string.Format("{0}% 終了しました", progress));

            // 旧データにコピー
            m_OriginalRouteFileProperty.Copy(m_CurrentRouteFileProperty);

            // 経過メッセージ表示
            backgroundWorker.ReportProgress(max, string.Format("{0}% 終了しました", max));

            // ロギング
            Logger.Debug("<<<<= FormRoute::DatabaseWrite(int, int, BackgroundWorker)");
        }
        #endregion
        #endregion
        #endregion

        #region privateメソッド
        #region タイトル設定
        /// <summary>
        /// タイトル設定
        /// </summary>
        private void SetTitle()
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::SetTitle()");

            // 設定
            Text = string.Format("路線 - {0}", Path.GetFileName(FileName));

            // ロギング
            Logger.DebugFormat("Text:[{0}]", Text);
            Logger.Debug("<<<<= FormRoute::SetTitle()");
        }
        #endregion

        #region 時刻表ファイル選択
        /// <summary>
        /// 時刻表ファイル選択
        /// </summary>
        /// <returns></returns>
        private string SelectSaveTimetableFile()
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::SelectSaveTimetableFile()");

            // SaveFileDialogクラスのインスタンスを作成
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // はじめのファイル名を指定する
            // はじめに「ファイル名」で表示される文字列を指定する
            saveFileDialog.FileName = Const.DefaultFileName;

            // [ファイルの種類]に表示される選択肢を指定する
            // 指定しないとすべてのファイルが表示される
            saveFileDialog.Filter = "時刻表ファイル(*.db)|*.db";

            // タイトルを設定する
            saveFileDialog.Title = "時刻表ファイルを指定してください";

            // ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            saveFileDialog.RestoreDirectory = true;

            // 存在しないファイルの名前が指定されたとき警告を表示する
            // デフォルトでTrueなので指定する必要はない
            saveFileDialog.CheckFileExists = false;

            // 存在しないパスが指定されたとき警告を表示する
            // デフォルトでTrueなので指定する必要はない
            saveFileDialog.CheckPathExists = true;

            // ダイアログを表示する
            DialogResult result = saveFileDialog.ShowDialog();

            // ダイアログ表示結果判定
            if (result != DialogResult.OK)
            {
                // ロギング
                Logger.DebugFormat("result:{0}", result);
                Logger.Debug("<<<<= FormRoute::SelectSaveTimetableFile()");

                // 空のファイル名を返却
                return string.Empty;
            }

            // ロギング
            Logger.DebugFormat("FileName:{0}", saveFileDialog.FileName);
            Logger.Debug("<<<<= FormRoute::SelectSaveTimetableFile()");

            // ファイル名を返却
            return saveFileDialog.FileName;
        }
        #endregion

        #region 時刻表ファイル保存
        /// <summary>
        /// 時刻表ファイル保存
        /// </summary>
        private void SaveTimetableFile()
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::SaveTimetableFile()");

            // DialogProgressオブジェクト生成
            using (DialogProgress dialogProgress = new DialogProgress("時刻表ファイル保存", doWorkSaveTimetableFile))
            {
                // 進行状況ダイアログを表示する
                DialogResult result = dialogProgress.ShowDialog(this);

                // 結果判定
                if (result == DialogResult.Cancel)
                {
                    // メッセージ表示
                    MessageBox.Show("時刻表ファイル保存をキャンセルしました。", "キャンセル", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                else if (result == DialogResult.Abort)
                {
                    // メッセージ表示
                    MessageBox.Show("時刻表ファイル保存が異常終了しました。", "異常終了", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (result == DialogResult.OK)
                {
                    // 何もしない
                }
            }

            // ロギング
            Logger.Debug("<<<<= FormRoute::SaveTimetableFile()");
        }

        /// <summary>
        /// doWorkSaveTimetableFile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void doWorkSaveTimetableFile(object sender, DoWorkEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::doWorkSaveTimetableFile(object, DoWorkEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // BackgroundWorkerオブジェクト取得
            BackgroundWorker backgroundWorker = (BackgroundWorker)sender;

            // 経過メッセージ表示
            backgroundWorker.ReportProgress(0, "0% 終了しました");

            // データベース書込
            DatabaseWrite(0, 25, backgroundWorker);

            // 経過メッセージ表示
            backgroundWorker.ReportProgress(25, "25% 終了しました");

            // タイトル設定
            SetTitle();

            // 経過メッセージ表示
            backgroundWorker.ReportProgress(50, "50% 終了しました");

            // データベース読込
            DatabaseRead(50, 75, backgroundWorker);

            // 経過メッセージ表示
            backgroundWorker.ReportProgress(75, "75% 終了しました");

            // 更新通知
            UpdateNotification();

            // 経過メッセージ表示
            backgroundWorker.ReportProgress(100, "100% 終了しました");

            // ロギング
            Logger.Debug("<<<<= FormRoute::doWorkSaveTimetableFile(object, DoWorkEventArgs)");
        }
        #endregion

        #region 更新通知
        /// <summary>
        /// 更新通知
        /// </summary>
        private void UpdateNotification()
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::UpdateNotification()");

            // TreeView
            m_TreeViewRoute.Update(m_CurrentRouteFileProperty);

            // 登録されているコントロールを判定(Panel2)
            foreach (var control in splitContainerMain.Panel2.Controls)
            {
                // Formオブジェクト取得
                Form form = control as Form;

                // コントロールで分岐する
                switch (control.GetType())
                {
                    case Type @_ when @_ == typeof(FormRouteFileProperty):
                        {
                            // フォーム取得
                            FormRouteFileProperty formProperty = form as FormRouteFileProperty;

                            // 更新通知
                            formProperty.UpdateNotification(m_CurrentRouteFileProperty);
                        }
                        break;
                    case Type @_ when @_ == typeof(FormStationProperties):
                        {
                            // フォーム取得
                            FormStationProperties formProperty = form as FormStationProperties;

                            // 更新通知
                            formProperty.UpdateNotification(m_CurrentRouteFileProperty);
                        }
                        break;
                    case Type @_ when @_ == typeof(FormTrainTypeProperties):
                        {
                            // フォーム取得
                            FormTrainTypeProperties formProperty = form as FormTrainTypeProperties;

                            // 更新通知
                            formProperty.UpdateNotification(m_CurrentRouteFileProperty);
                        }
                        break;
                    case Type @_ when @_ == typeof(FormDiagramProperties):
                        {
                            // フォーム取得
                            FormDiagramProperties formProperty = form as FormDiagramProperties;

                            // 更新通知
                            formProperty.UpdateNotification(m_CurrentRouteFileProperty);
                        }
                        break;
                    case Type @_ when @_ == typeof(FormDiagramDisplay):
                        {
                            // フォーム取得
                            FormDiagramDisplay formProperty = form as FormDiagramDisplay;

                            // 更新通知
                            formProperty.UpdateNotification(m_CurrentRouteFileProperty);
                        }
                        break;
                    case Type @_ when @_ == typeof(FormComment):
                        {
                            // フォーム取得
                            FormComment formProperty = form as FormComment;

                            // 更新通知
                            formProperty.UpdateNotification(m_CurrentRouteFileProperty);
                        }
                        break;
                    case Type @_ when @_ == typeof(FormTimetable):
                        {
                            // フォーム取得
                            FormTimetable formProperty = form as FormTimetable;

                            // 更新通知
                            formProperty.UpdateNotification(m_CurrentRouteFileProperty);
                        }
                        break;
                    case Type @_ when @_ == typeof(FormDiagramDisplay):
                        {
                            // フォーム取得
                            FormDiagramDisplay formProperty = form as FormDiagramDisplay;

                            // 更新通知
                            formProperty.UpdateNotification(m_CurrentRouteFileProperty);
                        }
                        break;
                    case Type @_ when @_ == typeof(FormStationTimetable):
                        {
                            // フォーム取得
                            FormStationTimetable formProperty = form as FormStationTimetable;

                            // 更新通知
                            formProperty.UpdateNotification(m_CurrentRouteFileProperty);
                        }
                        break;
                    case Type @_ when @_ == typeof(FormStationTimetableDisplay):
                        {
                            // フォーム取得
                            FormStationTimetableDisplay formProperty = form as FormStationTimetableDisplay;

                            // 更新通知
                            formProperty.UpdateNotification(m_CurrentRouteFileProperty);
                        }
                        break;
                    default:
                        break;
                }
            }

            // ロギング
            Logger.Debug("<<<<= FormRoute::UpdateNotification()");
        }
        #endregion

        #region 子フォーム関連
        /// <summary>
        /// 子フォームを登録する
        /// </summary>
        /// <param name="form"></param>
        public void AddMDIChildForm(Form form)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::AddMDIChildForm(Form)");
            Logger.DebugFormat("form:[{0}]", form);

            // 登録
            form.TopLevel = false;
            splitContainerMain.Panel2.Controls.Add(form);
            form.BringToFront();

            // ロギング
            Logger.Debug("<<<<= FormRoute::AddMDIChildForm(Form)");
        }

        /// <summary>
        /// 子フォームを取得する
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Form GetMDIChildForm(Type type)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::GetMDIChildForm(Type)");
            Logger.DebugFormat("type:[{0}]", type);

            // 登録されているコントロールを判定
            foreach (var control in splitContainerMain.Panel2.Controls)
            {
                // Formオブジェクト取得
                Form form = control as Form;

                // 種別で分岐
                if (type == form.GetType())
                {
                    // ロギング
                    Logger.Debug("登録:[あり]");
                    Logger.Debug("<<<<= FormRoute::GetMDIChildForm(Type)");

                    // 登録あり
                    return form;
                }
            }

            // ロギング
            Logger.Warn("登録:[なし]");

            // 登録なし
            throw new ArgumentException(string.Format("指定されたフォームは登録されていません：[{0}]", type.ToString()));
        }

        /// <summary>
        /// 子フォームを取得する
        /// </summary>
        /// <param name="type"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Form GetMDIChildForm(Type type, string text)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::GetMDIChildForm(Type, string)");
            Logger.DebugFormat("type:[{0}]", type);
            Logger.DebugFormat("text:[{0}]", text);

            // 登録されているコントロールを判定
            foreach (var control in splitContainerMain.Panel2.Controls)
            {
                // Formオブジェクト取得
                Form form = control as Form;

                // 種別で分岐
                if (type == form.GetType())
                {
                    // Text比較 
                    if (form.Text != text)
                    {
                        // 不一致
                        continue;
                    }

                    // ロギング
                    Logger.Debug("登録:[あり]");
                    Logger.Debug("<<<<= FormRoute::GetMDIChildForm(Type, string)");

                    // 登録あり
                    return form;
                }
            }

            // ロギング
            Logger.Warn("登録:[なし]");

            // 登録なし
            throw new ArgumentException(string.Format("指定されたフォームは登録されていません：[{0}][{1}]", type.ToString(), text));
        }

        /// <summary>
        /// 子フォームが登録されているか判定する
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsMDIChildForm(Type type)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::IsMDIChildForm(Type)");
            Logger.DebugFormat("type:[{0}]", type);

            // 登録されているコントロールを判定
            foreach (var control in splitContainerMain.Panel2.Controls)
            {
                // Formオブジェクト取得
                Form form = control as Form;

                // 種別で分岐
                if (type == form.GetType())
                {
                    // ロギング
                    Logger.Debug("登録:[あり]");
                    Logger.Debug("<<<<= FormRoute::IsMDIChildForm(Type)");

                    // 登録あり
                    return true;
                }
            }

            // ロギング
            Logger.Debug("登録:[なし]");
            Logger.Debug("<<<<= FormRoute::IsMDIChildForm(Type)");

            // 登録なし
            return false;
        }


        /// <summary>
        /// 子フォームが登録されているか判定する
        /// </summary>
        /// <param name="type"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool IsMDIChildForm(Type type, string text)
        {
            // ロギング
            Logger.Debug("=>>>> FormRoute::IsMDIChildForm(Type, string)");
            Logger.DebugFormat("type:[{0}]", type);
            Logger.DebugFormat("text:[{0}]", text);

            // 登録されているコントロールを判定
            foreach (var control in splitContainerMain.Panel2.Controls)
            {
                // Formオブジェクト取得
                Form form = control as Form;

                // 種別で分岐
                if (type == form.GetType())
                {
                    // Text比較 
                    if (form.Text != text)
                    {
                        // 不一致
                        continue;
                    }

                    // ロギング
                    Logger.Debug("登録:[あり]");
                    Logger.Debug("<<<<= FormRoute::IsMDIChildForm(Type, string)");

                    // 登録あり
                    return true;
                }
            }

            // ロギング
            Logger.Debug("登録:[なし]");
            Logger.Debug("<<<<= FormRoute::IsMDIChildForm(Type, string)");

            // 登録なし
            return false;
        }
        #endregion

        #endregion
    }
}
