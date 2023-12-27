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
using static System.Data.Entity.Infrastructure.Design.Executor;
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
        /// ダイヤグラムID
        /// </summary>
        private int m_DiagramId = 0;

        /// <summary>
        /// ダイアログ名
        /// </summary>
        private string m_DiagramName = string.Empty;

        /// <summary>
        /// 方向種別
        /// </summary>
        private DirectionType m_DirectionType = DirectionType.None;

        /// <summary>
        /// RouteFilePropertyオブジェクト
        /// </summary>
        private RouteFileProperty m_RouteFileProperty = null;

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
            m_DiagramName = text;
            m_DirectionType = type;
            m_RouteFileProperty = property;
            Text = regName;

            // DataGridViewTimetableオブジェクト生成
            m_DataGridViewTimetable = new DataGridViewTimetable(m_DiagramName, m_DirectionType, m_RouteFileProperty);
            m_DataGridViewTimetable.Click += DataGridViewTimetable_Click;
            m_DataGridViewTimetable.OnTrainPropertyUpdate += DataGridViewTimetable_OnTrainPropertyUpdate;
            m_DataGridViewTimetable.OnStationPropertiesUpdate += DataGridViewTimetable_OnStationPropertiesUpdate;
            m_DataGridViewTimetable.OnStationTimePropertyUpdate += DataGridViewTimetable_OnStationTimePropertyUpdate;

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

            // 描画一時停止
            SuspendLayout();

            // コントコール設定
            m_DataGridViewTimetable.Dock = DockStyle.Fill;
            m_DataGridViewTimetable.Draw();
            panelMain.Controls.Add(m_DataGridViewTimetable);
            panelMain.Dock = DockStyle.Fill;

            // 描画再開
            ResumeLayout();

            // ロギング
            Logger.Debug("<<<<= FormTimetable::FormTimetable_Load(object, EventArgs)");
        }
        #endregion

        #region TableLayoutPanelTimetableイベント
        /// <summary>
        /// TableLayoutPanelTimetable_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TableLayoutPanelTimetable_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormTimetable::TableLayoutPanelTimetable_Click(object, MouseEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // フォームを前面に表示する
            BringToFront();

            // ロギング
            Logger.Debug("<<<<= FormTimetable::TableLayoutPanelTimetable_Click(object, MouseEventArgs)");
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

        /// <summary>
        /// DataGridViewTimetable_OnTrainPropertyUpdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewTimetable_OnTrainPropertyUpdate(object sender, TrainPropertyUpdateEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormTimetable::DataGridViewTimetable_OnTrainPropertyUpdate(object, TrainPropertyUpdateEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TimetableUpdateEventArgsオブジェクト
            TimetableUpdateEventArgs eventArgs = new TimetableUpdateEventArgs();
            eventArgs.UpdateType = e.Property.GetType();
            eventArgs.DiagramId = m_DiagramId;
            eventArgs.DiagramName = m_DiagramName;
            eventArgs.DirectionType = m_DirectionType;
            eventArgs.RouteFileProperty = m_RouteFileProperty;
            eventArgs.UpdateObject = e.Property;

            // 更新通知
            OnUpdate(this, eventArgs);

            // ロギング
            Logger.Debug("<<<<= FormTimetable::DataGridViewTimetable_OnTrainPropertyUpdate(object, TrainPropertyUpdateEventArgs)");
        }

        /// <summary>
        /// DataGridViewTimetable_OnStationPropertiesUpdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewTimetable_OnStationPropertiesUpdate(object sender, StationPropertiesUpdateEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormTimetable::DataGridViewTimetable_OnStationPropertiesUpdate(object, StationPropertiesUpdateEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TimetableUpdateEventArgsオブジェクト
            TimetableUpdateEventArgs eventArgs = new TimetableUpdateEventArgs();
            eventArgs.UpdateType = e.Properties.GetType();
            eventArgs.DiagramId = m_DiagramId;
            eventArgs.DiagramName = m_DiagramName;
            eventArgs.DirectionType = m_DirectionType;
            eventArgs.RouteFileProperty = m_RouteFileProperty;
            eventArgs.UpdateObject = e.Properties;

            // 更新通知
            OnUpdate(this, eventArgs);

            // ロギング
            Logger.Debug("<<<<= FormTimetable::DataGridViewTimetable_OnStationPropertiesUpdate(object, StationPropertiesUpdateEventArgs)");
        }

        /// <summary>
        /// DataGridViewTimetable_OnStationTimePropertyUpdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewTimetable_OnStationTimePropertyUpdate(object sender, StationTimePropertyUpdateEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormTimetable::DataGridViewTimetable_OnStationTimePropertyUpdate(object, StationTimePropertyUpdateEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TimetableUpdateEventArgsオブジェクト
            TimetableUpdateEventArgs eventArgs = new TimetableUpdateEventArgs();
            eventArgs.UpdateType = e.Property.GetType();
            eventArgs.DiagramId = m_DiagramId;
            eventArgs.DiagramName = m_DiagramName;
            eventArgs.DirectionType = m_DirectionType;
            eventArgs.RouteFileProperty = m_RouteFileProperty;
            eventArgs.UpdateObject = e.Property;

            // 更新通知
            OnUpdate(this, eventArgs);

            // ロギング
            Logger.Debug("<<<<= FormTimetable::DataGridViewTimetable_OnStationTimePropertyUpdate(object, StationTimePropertyUpdateEventArgs)");
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

            // 設定
            m_RouteFileProperty = property;

            // DataGridViewTimetable
            m_DataGridViewTimetable.Update(m_RouteFileProperty);

            // ロギング
            Logger.Debug("<<<<= FormTimetable::Update(RouteFileProperty)");
        }
        #endregion
    }
}
