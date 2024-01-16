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
using TrainTimeTable.EventArgs;
using TrainTimeTable.Property;

namespace TrainTimeTable
{
    /// <summary>
    /// FormDiagramDisplayクラス
    /// </summary>
    public partial class FormDiagramDisplay : Form
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
        private FormRoute m_Owner;

        /// <summary>
        /// RouteFilePropertyオブジェクト
        /// </summary>
        private RouteFileProperty m_RouteFileProperty;

        /// <summary>
        /// PictureBoxHoursオブジェクト
        /// </summary>
        private PictureBoxHours m_PictureBoxHours = null;

        /// <summary>
        /// PictureBoxStationsオブジェクト
        /// </summary>
        private PictureBoxStations m_PictureBoxStations = null;

        /// <summary>
        /// PictureBoxDiagramオブジェクト
        /// </summary>
        private PictureBoxDiagram m_PictureBoxDiagram = null;

        /// <summary>
        /// 列車番号表示
        /// </summary>
        public bool TrainNumberDisplay { get; set; } = true;

        /// <summary>
        /// 列車名表示
        /// </summary>
        public bool TrainNameDisplay { get; set; } = true;

        /// <summary>
        /// 列車時刻表示
        /// </summary>
        public bool TrainTimeDisplay { get; set; } = false;

        /// <summary>
        /// 横幅描画割合
        /// </summary>
        public float WidthPercent { get; set; } = 100.0f;

        /// <summary>
        /// 横幅描画拡大縮小割合
        /// </summary>
        private float m_WidthPercentStep { get; set; } = 5.0f;

        /// <summary>
        /// 横幅描画割合最小値
        /// </summary>
        private float m_WidthPercentMinimum { get; set; } = 10.0f;

        /// <summary>
        /// 横幅描画割合最大値
        /// </summary>
        private float m_WidthPercentMaximum { get; set; } = 200.0f;

        /// <summary>
        /// 縦幅描画割合
        /// </summary>
        public float HeightPercent { get; set; } = 100.0f;

        /// <summary>
        /// 縦幅描画割合
        /// </summary>
        private float m_HeightPercentStep { get; set; } = 5.0f;

        /// <summary>
        /// 縦幅描画割合最小値
        /// </summary>
        private float m_HeightPercentMinimum { get; set; } = 10.0f;

        /// <summary>
        /// 縦幅描画割合最大値
        /// </summary>
        private float m_HeightPercentMaximum { get; set; } = 200.0f;

        /// <summary>
        /// 印刷処理中ページ番号
        /// </summary>
        private int m_CurrentPageNumber { get; set; } = 0;

        /// <summary>
        /// ダイアグラム名
        /// </summary>
        public string DiagramName { get; private set; } = string.Empty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="regName"></param>
        /// <param name="daiagramName"></param>
        /// <param name="property"></param>
        public FormDiagramDisplay(FormRoute owner, string regName, string daiagramName, RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::FormDiagramDisplay(FormRoute, string, string, RouteFileProperty)");
            Logger.DebugFormat("owner       :[{0}]", owner);
            Logger.DebugFormat("regName     :[{0}]", regName);
            Logger.DebugFormat("daiagramName:[{0}]", daiagramName);
            Logger.DebugFormat("property    :[{0}]", property);

            // コンポーネント初期化
            InitializeComponent();

            // 設定
            m_Owner = owner;
            m_RouteFileProperty = property;
            Text = string.Format("{0}", regName);
            DiagramName = daiagramName;
            m_PictureBoxHours = new PictureBoxHours(m_Owner, m_RouteFileProperty);
            m_PictureBoxStations = new PictureBoxStations(m_Owner, m_RouteFileProperty);
            m_PictureBoxDiagram = new PictureBoxDiagram(m_Owner, daiagramName, m_RouteFileProperty);

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::FormDiagramDisplay(FormRoute, string, string, RouteFileProperty)");
        }

        #region イベント
        #region FormDiagramDisplayイベント
        /// <summary>
        /// FormDiagramDisplay_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormDiagramDisplay_Load(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::FormDiagramDisplay_Load(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // コントコール設定
            tableLayoutPanelMain.Dock = DockStyle.Fill;
            tableLayoutPanelMain.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, m_RouteFileProperty.Route.WidthOfStationNameField * 20);
            panelHours.Dock = DockStyle.Fill;
            panelHours.Controls.Add(m_PictureBoxHours);
            panelStations.Dock = DockStyle.Fill;
            panelStations.Controls.Add(m_PictureBoxStations);
            panelDiagram.Dock = DockStyle.Fill;
            panelDiagram.Controls.Add(m_PictureBoxDiagram);
            panelDiagram.Scroll += PanelDiagram_Scroll;
            panelDiagram.MouseWheel += PanelDiagram_MouseWheel;

            if (TrainNumberDisplay)
            {
                toolStripButtonDisplayOfTrainNumber.CheckState = CheckState.Checked;
            }
            if (TrainNameDisplay)
            {
                toolStripButtonDisplayOfTrainName.CheckState = CheckState.Checked;
            }
            if (TrainTimeDisplay)
            {
                toolStripButtonDisplayOfTrainTime.CheckState = CheckState.Checked;
            }

            // Task実行
            Task.Run(async () =>
            {
                Invoke(new Action(() =>
                {
                    // 描画更新
                    SuspendLayout();
                    m_PictureBoxHours.UpdateDraw();
                    m_PictureBoxStations.UpdateDraw();
                    m_PictureBoxDiagram.UpdateDraw();
                    UpdateLabelInfomation();
                    ResumeLayout(false);
                }));
                await Task.Delay(100);
            });

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::FormDiagramDisplay_Load(object, EventArgs)");
        }

        /// <summary>
        /// FormDiagramDisplay_Resize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormDiagramDisplay_Resize(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::FormDiagramDisplay_Resize(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 縦横位置更新
            m_PictureBoxStations.Location = new Point(0, m_PictureBoxDiagram.Location.Y);
            m_PictureBoxHours.Location = new Point(m_PictureBoxDiagram.Location.X, 0);

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::FormDiagramDisplay_Resize(object, EventArgs)");
        }
        #endregion

        #region PanelDiagramイベント
        /// <summary>
        /// PanelDiagram_Scroll
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelDiagram_Scroll(object sender, ScrollEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::PanelDiagram_Scroll(object, ScrollEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 位置更新
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                // 縦スクロールの場合
                UpdateVerticalLocation();
            }
            else
            {
                // 横スクロールの場合
                UpdateHorizontalLocation();
            }

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::PanelDiagram_Scroll(object, ScrollEventArgs)");
        }

        /// <summary>
        /// PanelDiagram_MouseWheel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelDiagram_MouseWheel(object sender, MouseEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::PanelDiagram_Scroll(object, MouseEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 位置更新
            UpdateVerticalLocation();

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::PanelDiagram_Scroll(object, MouseEventArgs)");
        }

        /// <summary>
        /// 位置更新
        /// </summary>
        private void UpdateLocation()
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::UpdateLocation()");

            // 位置更新
            UpdateVerticalLocation();
            UpdateHorizontalLocation();

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::UpdateLocation()");
        }

        /// <summary>
        /// 位置更新(縦)
        /// </summary>
        private void UpdateVerticalLocation()
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::UpdateVerticalLocation()");

            // 位置更新
            m_PictureBoxStations.Location = new Point(0, m_PictureBoxDiagram.Location.Y);

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::UpdateVerticalLocation()");
        }

        /// <summary>
        /// 位置更新(横)
        /// </summary>
        private void UpdateHorizontalLocation()
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::UpdateHorizontalLocation()");

            // 位置更新
            m_PictureBoxHours.Location = new Point(m_PictureBoxDiagram.Location.X, 0);

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::UpdateHorizontalLocation()");
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
            Logger.Debug("=>>>> FormDiagramDisplay::toolStripButtonPrintPreview_Click(object, EventArgs)");
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
            Logger.Debug("<<<<= FormDiagramDisplay::toolStripButtonPrintPreview_Click(object, EventArgs)");
        }

        /// <summary>
        /// PrintDocument_PrintPage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::PrintDocument_PrintPage(object, PrintPageEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 現在のページ番号を更新
            m_CurrentPageNumber++;

            // 縮尺決定
            float percent = DiagramDrawLibrary.DetermineScale(e.PageBounds.Height, m_PictureBoxHours.Image.Height + m_PictureBoxStations.Image.Height);

            // 縮尺判定
            if (percent > 100)
            {
                // 100%を超えたら補正する
                percent = 100;
            }

            // 縮尺サイズ取得
            SizeF hoursSize = m_PictureBoxHours.GetDrawSize(percent);
            SizeF stationsSize = m_PictureBoxStations.GetDrawSize(percent);
            SizeF diagramSize = m_PictureBoxDiagram.GetDrawSize(percent);

            // 画像取得範囲設定
            RectangleF hoursImageAcquisitionRange = new RectangleF() { X = 0, Y = 0, Width = e.PageBounds.Width - stationsSize.Width, Height = hoursSize.Height };
            RectangleF diagramImageAcquisitionRange = new RectangleF() { X = 0, Y = 0, Width = e.PageBounds.Width - stationsSize.Width, Height = e.PageBounds.Height - hoursSize.Height };

            // イメージ取得
            bool next = true;
            Image stationsImage = m_PictureBoxStations.ResizeImage(stationsSize);
            Image hoursImage = m_PictureBoxHours.GetDrawImage(m_CurrentPageNumber, percent, hoursSize, e.PageBounds, hoursImageAcquisitionRange, ref next);
            Image diagramImage = m_PictureBoxDiagram.GetDrawImage(m_CurrentPageNumber, percent, diagramSize, e.PageBounds, diagramImageAcquisitionRange, ref next);

            // 次ページ有無設定
            e.HasMorePages = next;

            // ロギング
            Logger.InfoFormat("【印刷情報】");
            Logger.InfoFormat("印刷処理中ページ番号  :{0}", m_CurrentPageNumber);
            Logger.InfoFormat("画像サイズ(時)        :{0}", m_PictureBoxHours.Image.Size);
            Logger.InfoFormat("画像サイズ(駅)        :{0}", m_PictureBoxStations.Image.Size);
            Logger.InfoFormat("画像サイズ(ダイヤ)    :{0}", m_PictureBoxDiagram.Image.Size);
            Logger.InfoFormat("描画範囲              :{0}", e.PageBounds);
            Logger.InfoFormat("縮尺                  :{0}", percent);
            Logger.InfoFormat("縮尺サイズ(時)        :{0}", hoursSize);
            Logger.InfoFormat("縮尺サイズ(駅)        :{0}", stationsSize);
            Logger.InfoFormat("縮尺サイズ(ダイヤ)    :{0}", diagramSize);
            Logger.InfoFormat("画像取得範囲(時)      :{0}", hoursImageAcquisitionRange);
            Logger.InfoFormat("画像取得範囲(ダイヤ)  :{0}", diagramImageAcquisitionRange);
            Logger.InfoFormat("取得画像サイズ(時)    :{0}", hoursImage?.Size);
            Logger.InfoFormat("取得画像サイズ(駅)    :{0}", stationsImage?.Size);
            Logger.InfoFormat("取得画像サイズ(ダイヤ):{0}", diagramImage?.Size);
            Logger.InfoFormat("次ページ有無          :{0}", next);

            // 画像描画
            // TODO:開始位置は暫定
            e.Graphics.DrawImage(stationsImage, 0, hoursSize.Height - 2, stationsImage.Width, stationsImage.Height);
            e.Graphics.DrawImage(hoursImage, stationsSize.Width - 2, 0, hoursImage.Width, hoursImage.Height);
            e.Graphics.DrawImage(diagramImage, stationsSize.Width - 2, hoursSize.Height - 2, diagramImage.Width, diagramImage.Height);

            // リソース開放
            stationsImage.Dispose();
            hoursImage.Dispose();
            diagramImage.Dispose();

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::PrintDocument_PrintPage(object, PrintPageEventArgs)");
        }

        /// <summary>
        /// toolStripButtonPrinterSetting_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonPrinterSetting_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::toolStripButtonPrinterSetting_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::toolStripButtonPrinterSetting_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripButtonHorizontalReduction_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonHorizontalReduction_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::toolStripButtonHorizontalReduction_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 横幅描画割合計算
            WidthPercent -= m_WidthPercentStep;

            // 最小値判定
            if (m_WidthPercentMinimum > WidthPercent)
            {
                WidthPercent = m_WidthPercentMinimum;
            }

            // 表示更新
            SuspendLayout();
            m_PictureBoxHours.UpdateDraw(m_RouteFileProperty, WidthPercent, HeightPercent);
            m_PictureBoxStations.UpdateDraw(m_RouteFileProperty, WidthPercent, HeightPercent);
            m_PictureBoxDiagram.UpdateDraw(m_RouteFileProperty, WidthPercent, HeightPercent);
            UpdateLocation();
            UpdateLabelInfomation();
            ResumeLayout(false);

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::toolStripButtonHorizontalReduction_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripButtonHorizontalExpansion_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonHorizontalExpansion_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::toolStripButtonHorizontalExpansion_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 横幅描画割合計算
            WidthPercent += m_WidthPercentStep;

            // 最大値判定
            if (WidthPercent > m_WidthPercentMaximum)
            {
                WidthPercent = m_WidthPercentMaximum;
            }

            // 表示更新
            SuspendLayout();
            m_PictureBoxHours.UpdateDraw(m_RouteFileProperty, WidthPercent, HeightPercent);
            m_PictureBoxStations.UpdateDraw(m_RouteFileProperty, WidthPercent, HeightPercent);
            m_PictureBoxDiagram.UpdateDraw(m_RouteFileProperty, WidthPercent, HeightPercent);
            UpdateLocation();
            UpdateLabelInfomation();
            ResumeLayout(false);

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::toolStripButtonHorizontalExpansion_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripButtonVerticalReduction_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonVerticalReduction_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::toolStripButtonVerticalReduction_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 縦幅描画割合計算
            HeightPercent -= m_HeightPercentStep;

            // 最小値判定
            if (m_HeightPercentMinimum > HeightPercent)
            {
                HeightPercent = m_HeightPercentMinimum;
            }

            // 表示更新
            SuspendLayout();
            m_PictureBoxHours.UpdateDraw(m_RouteFileProperty, WidthPercent, HeightPercent);
            m_PictureBoxStations.UpdateDraw(m_RouteFileProperty, WidthPercent, HeightPercent);
            m_PictureBoxDiagram.UpdateDraw(m_RouteFileProperty, WidthPercent, HeightPercent);
            UpdateLocation();
            UpdateLabelInfomation();
            ResumeLayout(false);

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::toolStripButtonVerticalReduction_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripButtonVerticalExpansion_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonVerticalExpansion_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::toolStripButtonVerticalExpansion_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 縦幅描画割合計算
            HeightPercent += m_HeightPercentStep;

            // 最大値判定
            if (HeightPercent > m_HeightPercentMaximum)
            {
                HeightPercent = m_HeightPercentMaximum;
            }

            // 表示更新
            SuspendLayout();
            m_PictureBoxHours.UpdateDraw(m_RouteFileProperty, WidthPercent, HeightPercent);
            m_PictureBoxStations.UpdateDraw(m_RouteFileProperty, WidthPercent, HeightPercent);
            m_PictureBoxDiagram.UpdateDraw(m_RouteFileProperty, WidthPercent, HeightPercent);
            UpdateLocation();
            UpdateLabelInfomation();
            ResumeLayout(false);

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::toolStripButtonVerticalExpansion_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripButtonVerticalAxisDirectionReset_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonVerticalAxisDirectionReset_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::toolStripButtonVerticalAxisDirectionReset_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 初期化
            WidthPercent = 100.0f;
            HeightPercent = 100.0f;

            // 表示更新
            SuspendLayout();
            m_PictureBoxHours.UpdateDraw(m_RouteFileProperty, WidthPercent, HeightPercent);
            m_PictureBoxStations.UpdateDraw(m_RouteFileProperty, WidthPercent, HeightPercent);
            m_PictureBoxDiagram.UpdateDraw(m_RouteFileProperty, WidthPercent, HeightPercent);
            UpdateLocation();
            UpdateLabelInfomation();
            ResumeLayout(false);

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::toolStripButtonVerticalAxisDirectionReset_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripButtonDisplayOfTrainNumber_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonDisplayOfTrainNumber_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::toolStripButtonDisplayOfTrainNumber_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // ToolStripButtonオブジェクト取得
            ToolStripButton tsb = (ToolStripButton)sender;

            //チェック状態を反転させる
            tsb.Checked = !tsb.Checked;
            TrainNumberDisplay = !TrainNumberDisplay;

            // 表示更新
            SuspendLayout();
            m_PictureBoxDiagram.UpdateDraw(m_RouteFileProperty, TrainNumberDisplay, TrainNameDisplay, TrainTimeDisplay);
            UpdateLabelInfomation();
            ResumeLayout(false);

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::toolStripButtonDisplayOfTrainNumber_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripButtonDisplayOfTrainName_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonDisplayOfTrainName_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::toolStripButtonDisplayOfTrainName_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // ToolStripButtonオブジェクト取得
            ToolStripButton tsb = (ToolStripButton)sender;

            //チェック状態を反転させる
            tsb.Checked = !tsb.Checked;
            TrainNameDisplay = !TrainNameDisplay;

            // 表示更新
            SuspendLayout();
            m_PictureBoxDiagram.UpdateDraw(m_RouteFileProperty, TrainNumberDisplay, TrainNameDisplay, TrainTimeDisplay);
            UpdateLabelInfomation();
            ResumeLayout(false);

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::toolStripButtonDisplayOfTrainName_Click(object, EventArgs)");
        }

        /// <summary>
        /// toolStripButtonDisplayOfTrainTime_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonDisplayOfTrainTime_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::toolStripButtonDisplayOfTrainTime_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // ToolStripButtonオブジェクト取得
            ToolStripButton tsb = (ToolStripButton)sender;

            //チェック状態を反転させる
            tsb.Checked = !tsb.Checked;
            TrainTimeDisplay = !TrainTimeDisplay;

            // 表示更新
            SuspendLayout();
            m_PictureBoxDiagram.UpdateDraw(m_RouteFileProperty, TrainNumberDisplay, TrainNameDisplay, TrainTimeDisplay);
            UpdateLabelInfomation();
            ResumeLayout(false);

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::toolStripButtonDisplayOfTrainTime_Click(object, EventArgs)");
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
            Logger.Debug("=>>>> FormDiagramDisplay::UpdateNotification(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 更新
            Update(property);

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::UpdateNotification(RouteFileProperty)");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        public void Update(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::Update(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 描画更新
            SuspendLayout();
            m_PictureBoxHours.UpdateDraw(property);
            m_PictureBoxStations.UpdateDraw(property);
            m_PictureBoxDiagram.UpdateDraw(property, TrainNumberDisplay, TrainNameDisplay, TrainTimeDisplay);
            m_RouteFileProperty = property;
            UpdateLabelInfomation();
            ResumeLayout(false);

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::Update(RouteFileProperty)");
        }

        /// <summary>
        /// 削除通知
        /// </summary>
        /// <param name="property"></param>
        public void RemoveNotification(DiagramProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationProperties::RemoveNotification(DiagramProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // ダイアグラム名判定
            if(property.Name == DiagramName)
            {
                // フォームクローズ
                Close();
            }

            // ロギング
            Logger.Debug("<<<<= FormStationProperties::RemoveNotification(RouteFileProperty)");
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// UpdateLabelInfomation
        /// </summary>
        private void UpdateLabelInfomation()
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramDisplay::UpdateLabelInfomation()");

            // 表示文字列作成
            string message = string.Format("横:{0}%,縦:{1}%,列車番号:{2},列車名:{3},時刻:{4}", WidthPercent, HeightPercent, TrainNumberDisplay, TrainNameDisplay, TrainTimeDisplay);

            // 設定
            toolStripStatusLabelInfomation.Text = message;

            // ロギング
            Logger.Debug("<<<<= FormDiagramDisplay::UpdateLabelInfomation()");
        }
        #endregion
    }
}
