using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Common;
using TrainTimeTable.Control;
using TrainTimeTable.Dialog;
using TrainTimeTable.EventArgs;
using TrainTimeTable.Property;
using static System.Net.Mime.MediaTypeNames;

namespace TrainTimeTable
{
    /// <summary>
    /// FormTimetableクラス
    /// </summary>
    public partial class FormTimetable : Form
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region 更新 Event
        /// <summary>
        /// 更新 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void UpdateEventHandler(object sender, TimetableUpdateEventArgs e);

        /// <summary>
        /// 更新 event
        /// </summary>
        public event UpdateEventHandler OnUpdate = delegate { };
        #endregion

        /// <summary>
        /// FormRouteオブジェクト
        /// </summary>
        private FormRoute m_Owner = null;

        /// <summary>
        /// DataGridViewTimetableオブジェクト
        /// </summary>
        private DataGridViewTimetable m_DataGridViewTimetable = null;

        /// <summary>
        /// 印刷処理中ページ番号
        /// </summary>
        private int m_CurrentPageNumber { get; set; } = 0;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="text"></param>
        /// <param name="regName"></param>
        /// <param name="type"></param>
        /// <param name="property"></param>
        public FormTimetable(FormRoute owner, string text, string regName, DirectionType type, RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormTimetable::FormTimetable(FormRoute, string, string, DirectionType, RouteFileProperty)");
            Logger.DebugFormat("owner   :[{0}]", owner);
            Logger.DebugFormat("text    :[{0}]", text);
            Logger.DebugFormat("regName :[{0}]", regName);
            Logger.DebugFormat("type    :[{0}]", type);
            Logger.DebugFormat("property:[{0}]", property);

            // コンポーネント初期化
            InitializeComponent();

            // 設定
            m_Owner = owner;
            Text = regName;

            // DataGridViewTimetableオブジェクト生成
            m_DataGridViewTimetable = new DataGridViewTimetable(text, type, property);
            m_DataGridViewTimetable.Click += DataGridViewTimetable_Click;

            // ロギング
            Logger.Debug("<<<<= FormTimetable::FormTimetable(FormRoute, string, string, DirectionType, RouteFileProperty)");
        }
        #endregion

        #region イベント
        #region FormTimetableイベント
        /// <summary>
        /// FormTimetable_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormTimetable_Load(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormTimetable::FormTimetable_Load(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // コントコール設定
            m_DataGridViewTimetable.Dock = DockStyle.Fill;
            m_DataGridViewTimetable.Draw();
            panelMain.Controls.Add(m_DataGridViewTimetable);
            panelMain.Dock = DockStyle.Fill;

            // ロギング
            Logger.Debug("<<<<= FormTimetable::FormTimetable_Load(object, EventArgs)");
        }
        #endregion

        #region DataGridViewTimetableイベント
        /// <summary>
        /// DataGridViewTimetable_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewTimetable_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormTimetable::DataGridViewTimetable_Click(object, MouseEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // フォームを前面に表示する
            BringToFront();

            // ロギング
            Logger.Debug("<<<<= FormTimetable::DataGridViewTimetable_Click(object, MouseEventArgs)");
        }
        #endregion

        #region toolStripButtonイベント
        /// <summary>
        /// toolStripButtonPrint_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonPrint_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::toolStripButtonPrint_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // PrintDocumentオブジェクトの作成
            PrintDocument printDocument = new PrintDocument();

            // PrintPageイベントハンドラの追加
            printDocument.PrintPage += PrintDocument_PrintPage;

            // ページ番号を初期化
            m_CurrentPageNumber = 0;

            // PrintDialogオブジェクト生成
            PrintDialog printDialog = new PrintDialog();
            printDialog.AllowSomePages = true;

            // 印刷するPrintDocumentを設定
            printDialog.Document = printDocument;

            // 印刷ダイアログを表示
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                // 印刷を開始する
                printDocument.Print();
            }

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::toolStripButtonPrint_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripButtonPrintPreview_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonPrintPreview_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormTimetable::toolStripButtonPrintPreview_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // PrintDocumentオブジェクトの作成
            PrintDocument printDocument = new PrintDocument();

            // PrintPageイベントハンドラの追加
            printDocument.PrintPage += PrintDocument_PrintPage;

            // PrintPreviewDialogオブジェクトの作成
            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();

            // プレビューするPrintDocumentを設定
            printPreviewDialog.Document = printDocument;

            // ページ番号を初期化
            m_CurrentPageNumber = 0;

            // 印刷プレビューダイアログを表示する
            printPreviewDialog.ShowDialog();

            // ロギング
            Logger.Debug("<<<<= FormTimetable::toolStripButtonPrintPreview_Click(object, EventArgs)");
        }

        /// <summary>
        /// PrintDocument_PrintPage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormTimetable::PrintDocument_PrintPage(object, PrintPageEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 現在のページ番号を更新
            m_CurrentPageNumber++;

            // TODO:未実装

            // 次ページ有無設定
            e.HasMorePages = false;

            // ロギング
            Logger.Debug("<<<<= FormTimetable::PrintDocument_PrintPage(object, PrintPageEventArgs)");
        }

        /// <summary>
        /// toolStripButtonPrinterSetting_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonPrinterSetting_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormTimetable::toolStripButtonPrinterSetting_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= FormTimetable::toolStripButtonPrinterSetting_Click(object, EventArgs)");
        }
        #endregion
        #endregion

        #region publicメソッド
        /// <summary>
        /// 更新通知
        /// </summary>
        /// <param name="property"></param>
        public void UpdateNotification(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormTimetable::UpdateNotification(TimetableProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 更新
            Update(property);

            // ロギング
            Logger.Debug("<<<<= FormTimetable::UpdateNotification(TimetableProperty)");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        public void Update(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormTimetable::Update(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= FormTimetable::Update(RouteFileProperty)");
        }
        #endregion
    }
}
