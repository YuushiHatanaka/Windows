using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Windows.Documents;
using System.Windows.Forms;
using TrainTimeTable.Common;
using TrainTimeTable.EventArgs;
using TrainTimeTable.Property;
using static System.Collections.Specialized.BitVector32;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// VirtualDataGridViewTimeTableクラス
    /// </summary>
    public class VirtualDataGridViewTimeTable : DataGridView
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region 更新 Event
        /// <summary>
        /// 更新 event delegate(列車情報)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void TrainPropertyUpdateHandler(object sender, TrainPropertyUpdateEventArgs e);

        /// <summary>
        /// 更新 event delegate(列車情報)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void TrainPropertiesUpdateHandler(object sender, TrainPropertiesUpdateEventArgs e);

        /// <summary>
        /// 更新 event delegate(駅時刻情報)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void StationTimePropertyHandler(object sender, StationTimePropertyUpdateEventArgs e);

        /// <summary>
        /// 更新 event delegate(駅情報)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void StationPropertiesUpdateEventHandler(object sender, StationPropertiesUpdateEventArgs e);

        /// <summary>
        /// 更新 event(列車情報)
        /// </summary>
        public event TrainPropertyUpdateHandler OnTrainPropertyUpdate = delegate { };

        /// <summary>
        /// 更新 event(列車情報)
        /// </summary>
        public event TrainPropertiesUpdateHandler OnTrainPropertiesUpdate = delegate { };

        /// <summary>
        /// 更新 event(駅時刻情報)
        /// </summary>
        public event StationTimePropertyHandler OnStationTimePropertyUpdate = delegate { };

        /// <summary>
        /// 更新 event(駅情報)
        /// </summary>
        public event StationPropertiesUpdateEventHandler OnStationPropertiesUpdate = delegate { };
        #endregion

        /// <summary>
        /// ダイヤグラム名
        /// </summary>
        private string m_DiagramName = string.Empty;

        /// <summary>
        /// 方向種別
        /// </summary>
        private DirectionType m_DirectionType = DirectionType.None;

        /// <summary>
        /// 旧RouteFilePropertyオブジェクト
        /// </summary>
        private RouteFileProperty m_OldRouteFileProperty = new RouteFileProperty();

        /// <summary>
        /// RouteFilePropertyオブジェクト
        /// </summary>
        private RouteFileProperty m_RouteFileProperty = null;

        /// <summary>
        /// Font辞書
        /// </summary>
        private Dictionary<string, Font> m_FontDictionary = null;

        /// <summary>
        /// DiaProFontオブジェクト
        /// </summary>
        private DiaProFont m_DiaProFont = new DiaProFont();

        /// <summary>
        /// 駅描写情報
        /// </summary>
        private List<StationDrawRowInfomation> m_StationDrawRowInfomation = new List<StationDrawRowInfomation>();

        /// <summary>
        /// 描画対象列車情報
        /// </summary>
        private TrainProperties m_DrawTrainProperties = new TrainProperties();

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="text"></param>
        /// <param name="type"></param>
        /// <param name="property"></param>
        public VirtualDataGridViewTimeTable(string text, DirectionType type, RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable(string, DirectionType, RouteFileProperty)");
            Logger.DebugFormat("text    :[{0}]", text);
            Logger.DebugFormat("type    :[{0}]", type);
            Logger.DebugFormat("property:[{0}]", property);

            // 設定
            m_DiagramName = text;
            m_DirectionType = type;
            m_RouteFileProperty = property;
            m_OldRouteFileProperty.Copy(property);
            m_FontDictionary = m_RouteFileProperty.Fonts.GetFonts(
                new List<string>()
                {
                    "時刻表ビュー",
                    "時刻表ヘッダー",
                    "路線",
                    "駅間距離",
                    "列車番号",
                    "列車種別",
                    "列車名",
                    "列車記号",
                    "始発駅",
                    "終着駅",
                    "備考",
                    "主要駅",
                    "一般駅",
                });

            // 描画対象列車情報更新
            UpdateDrawTrainProperties();

            // Font設定
            Font = m_FontDictionary["時刻表ビュー"];

            // 初期化
            Initialization();

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable(string, DirectionType, RouteFileProperty)");
        }
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::Initialization()");

            // 設定
            DoubleBuffered = true;                // ２次バッファーを使用
            ReadOnly = true;                      // 読取専用
            AllowUserToDeleteRows = false;        // 行削除禁止
            AllowUserToAddRows = false;           // 行挿入禁止
            AllowUserToResizeRows = false;        // 行の高さ変更禁止
            ColumnHeadersVisible = false;         // 列ヘッダーを非表示にする
            RowHeadersVisible = false;            // 行ヘッダーを非表示にする
            MultiSelect = false;                  // セル、行、列が複数選択禁止
            GridColor = Color.DarkGray;
            BackgroundColor = Color.White;
            AutoGenerateColumns = false;

            // ヘッダーとすべてのセルの内容に合わせて、行の高さを自動調整する
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // ヘッダーの色等
            EnableHeadersVisualStyles = false;     // Visualスタイルを使用しない
            ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            ColumnHeadersDefaultCellStyle.Font = m_FontDictionary["時刻表ヘッダー"];
            ColumnHeadersDefaultCellStyle.Font = Font;
            ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //ヘッダ高さ
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            ColumnHeadersHeight = 28;

            // 仮想モード設定
            VirtualMode = true;
            CellValueNeeded += new DataGridViewCellValueEventHandler(VirtualDataGridViewTimeTable_CellValueNeeded);
            CellValuePushed += new DataGridViewCellValueEventHandler(VirtualDataGridViewTimeTable_CellValuePushed);
            NewRowNeeded += new DataGridViewRowEventHandler(VirtualDataGridViewTimeTable_NewRowNeeded);
            RowValidated += new DataGridViewCellEventHandler(VirtualDataGridViewTimeTable_RowValidated);
            RowDirtyStateNeeded += new QuestionEventHandler(VirtualDataGridViewTimeTable_RowDirtyStateNeeded);
            CancelRowEdit += new QuestionEventHandler(VirtualDataGridViewTimeTable_CancelRowEdit);
            UserDeletingRow += new DataGridViewRowCancelEventHandler(VirtualDataGridViewTimeTable_UserDeletingRow);

            // コンテキストメニュー追加
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.AddRange
            (
                new ToolStripItem[]
                {
                    new ToolStripMenuItem("切り取り(&T)", null, CutoutOnClick,"cutout"){ ShortcutKeys = (Keys.Control|Keys.X) },
                    new ToolStripMenuItem("コピー(&C)", null, CopyOnClick,"copy"){ ShortcutKeys = (Keys.Control|Keys.C) },
                    new ToolStripMenuItem("列車の前に貼り付ける", null, PastingTrainInFrontOnClick,"pasting_train_in_front"){ ShortcutKeys = (Keys.Control|Keys.F) },
                    new ToolStripMenuItem("列車の後に貼り付ける", null, PastingTrainLaterOnClick,"pasting_train_later"){ ShortcutKeys = (Keys.Control|Keys.L) },
                    new ToolStripMenuItem("消去", null, DeleteOnClick,"delete"){ ShortcutKeys = Keys.Delete },
                    new ToolStripSeparator(){ Name = "separator1" },
                    new ToolStripMenuItem("時刻のみ貼り付け", null, PasteOnlyTheTimeOnClick,"paste_only_the_time"),
                    new ToolStripSeparator(){ Name = "separator2" },
                    new ToolStripMenuItem("列車のプロパティ", null, TrainPropertiesOnClick,"train_properties"){ ShortcutKeys = (Keys.Control|Keys.Enter) },
                    new ToolStripMenuItem("列車を前に挿入する", null, InsertTrainInFrontOnClick,"insert_train_in_front"){ ShortcutKeys = (Keys.Control|Keys.Home) },
                    new ToolStripMenuItem("列車を後に挿入する", null, InsertTrainLaterOnClick,"insert_train_later"){ ShortcutKeys = (Keys.Control|Keys.End) },
                    new ToolStripSeparator(){ Name = "separator3" },
                    new ToolStripMenuItem("駅のプロパティ", null, StationPropertiesOnClick,"station_properties"){ ShortcutKeys = (Keys.Control|Keys.Alt|Keys.Enter) },
                    new ToolStripMenuItem("駅時刻のプロパティ", null, StationTimePropertiesOnClick,"station_time_properties"),
                    new ToolStripSeparator(){ Name = "separator4" },
                    new ToolStripMenuItem("前の列車と入れ替え", null, ReplaceThePreviousTrainOnClick,"replace_the_previous_train"),
                    new ToolStripMenuItem("後の列車と入れ替え",null, ReplaceWithNextTrainOnClick,"replace_with_next_train"),
                    new ToolStripSeparator(){ Name = "separator5" },
                    new ToolStripMenuItem("時刻消去", null, EraseTimeOnClick,"erase_time"),
                    new ToolStripMenuItem("通過", null, PassingOnClick,"passing"),
                    new ToolStripMenuItem("通過-停車", null, PassingStoppingOnClick,"passing_stopping"),
                    new ToolStripMenuItem("経由なし", null, NoRouteOnClick,"no_route"),
                    new ToolStripSeparator(){ Name = "separator6" },
                    new ToolStripMenuItem("当駅始発", null, FirstTrainFromThisStationOnClick,"first_train_from_this_station"),
                    new ToolStripMenuItem("当駅止り", null, StopsAtThisStationOnClick,"stops_at_this_station"),
                    new ToolStripMenuItem("直通化", null, DirectCommunicationOnClick,"direct_communication"),
                    new ToolStripMenuItem("分断", null, DivisionOnClick,"division"),
                }
            );

            // コントロール設定
            ContextMenuStrip = contextMenuStrip;
            ContextMenuStrip.Opened += ContextMenuStripOpened;

            // イベント設定
            ColumnAdded += new DataGridViewColumnEventHandler(VirtualDataGridViewTimeTable_ColumnAdded);
            CellPainting += new DataGridViewCellPaintingEventHandler(VirtualDataGridViewTimeTable_CellPainting);
            CellFormatting += new DataGridViewCellFormattingEventHandler(VirtualDataGridViewTimeTable_CellFormatting);
            MouseDoubleClick += new MouseEventHandler(VirtualDataGridViewTimeTable_MouseDoubleClick);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::Initialization()");
        }
        #endregion

        #region イベント
        #region VirtualDataGridViewTimeTableイベント
        /// <summary>
        /// VirtualDataGridViewTimeTable_CellValueNeeded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VirtualDataGridViewTimeTable_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_CellValueNeeded(object, DataGridViewCellValueEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 初期化
            int stationMaxIndex = 6 + m_StationDrawRowInfomation.Count;

            // 駅距離か？
            if (e.ColumnIndex == 0 && (e.RowIndex >= 6 && e.RowIndex < stationMaxIndex))
            {
                // 駅距離取得
                e.Value = GetStationDistanceCellValueNeeded(e.RowIndex - 6);
            }
            // 駅マークか？
            else if (e.ColumnIndex == 1 && (e.RowIndex >= 6 && e.RowIndex < stationMaxIndex))
            {
                // 駅マーク取得
                e.Value = GetStationMarkCellValueNeeded(e.RowIndex - 6);
            }
            else if (e.ColumnIndex == 2 && e.RowIndex == 0)
            {
                e.Value = "列車番号";
            }
            else if (e.ColumnIndex == 2 && e.RowIndex == 1)
            {
                e.Value = "列車種別";
            }
            else if (e.ColumnIndex == 2 && e.RowIndex == 2)
            {
                e.Value = "列車名";
            }
            else if (e.ColumnIndex == 2 && e.RowIndex == 3)
            {
                e.Value = "列車名";
            }
            else if (e.ColumnIndex == 2 && e.RowIndex == 4)
            {
                e.Value = "始発";
            }
            else if (e.ColumnIndex == 2 && e.RowIndex == 5)
            {
                e.Value = "終着";
            }
            else if (e.ColumnIndex == 2 && (e.RowIndex >= 6 && e.RowIndex < stationMaxIndex))
            {
                // 駅名取得
                e.Value = GetStationNameCellValueNeeded(e.RowIndex - 6);
            }
            // 発着か？
            else if (e.ColumnIndex == 3 && (e.RowIndex >= 6 && e.RowIndex < stationMaxIndex))
            {
                // 駅発着種別取得
                e.Value = GetDepartureArrivalTypeCellValueNeeded(e.RowIndex - 6);
            }
            // 列車番号か？
            else if (e.ColumnIndex >= 4 && e.RowIndex == 0)
            {
                // 列車番号取得
                e.Value = GetTrainNoCellValueNeeded(e.ColumnIndex - 4);
            }
            // 列車種別か？
            else if (e.ColumnIndex >= 4 && e.RowIndex == 1)
            {
                // 列車種別取得
                e.Value = GetTrainTypeCellValueNeeded(e.ColumnIndex - 4);
            }
            // 列車名か？
            else if (e.ColumnIndex >= 4 && e.RowIndex == 2)
            {
                // 列車名取得
                e.Value = GetTrainNameCellValueNeeded(e.ColumnIndex - 4);
            }
            // 列車記号か？
            else if (e.ColumnIndex >= 4 && e.RowIndex == 3)
            {
                // 列車記号取得
                e.Value = GetTrainMarkCellValueNeeded(e.ColumnIndex - 4);
            }
            // 始発駅か？
            else if (e.ColumnIndex >= 4 && e.RowIndex == 4)
            {
                // 始発駅取得
                e.Value = GetDepartingStationCellValueNeeded(e.ColumnIndex - 4);
            }
            // 終着駅か？
            else if (e.ColumnIndex >= 4 && e.RowIndex == 5)
            {
                // 終着駅取得
                e.Value = GetDestinationStationCellValueNeeded(e.ColumnIndex - 4);
            }
            // 列車時刻か？
            else if (e.ColumnIndex >= 4 && ((e.RowIndex >= 6) && e.RowIndex < stationMaxIndex))
            {
                // 列車時刻取得
                e.Value = GetStationTimeCellValueNeeded(e.ColumnIndex - 4, e.RowIndex - 6);
            }
            // 備考か？
            else if (e.ColumnIndex >= 4 && e.RowIndex == stationMaxIndex)
            {
                // 備考取得
                e.Value = GetRemarkCellValueNeeded(e.ColumnIndex - 4);
            }

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_CellValueNeeded(object, DataGridViewCellValueEventArgs)");
        }

        /// <summary>
        /// VirtualDataGridViewTimeTable_CellValuePushed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VirtualDataGridViewTimeTable_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_CellValuePushed(object, DataGridViewCellValueEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_CellValuePushed(object, DataGridViewCellValueEventArgs)");
        }

        /// <summary>
        /// VirtualDataGridViewTimeTable_NewRowNeeded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VirtualDataGridViewTimeTable_NewRowNeeded(object sender, DataGridViewRowEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_NewRowNeeded(object, DataGridViewRowEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_NewRowNeeded(object, DataGridViewRowEventArgs)");
        }

        /// <summary>
        /// VirtualDataGridViewTimeTable_RowValidated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VirtualDataGridViewTimeTable_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_RowValidated(object, DataGridViewCellEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_RowValidated(object, DataGridViewCellEventArgs)");
        }

        /// <summary>
        /// VirtualDataGridViewTimeTable_RowDirtyStateNeeded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VirtualDataGridViewTimeTable_RowDirtyStateNeeded(object sender, QuestionEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_RowDirtyStateNeeded(object, QuestionEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_RowDirtyStateNeeded(object, QuestionEventArgs)");
        }

        /// <summary>
        /// VirtualDataGridViewTimeTable_CancelRowEdit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VirtualDataGridViewTimeTable_CancelRowEdit(object sender, QuestionEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_CancelRowEdit(object, QuestionEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_CancelRowEdit(object, QuestionEventArgs)");
        }

        /// <summary>
        /// VirtualDataGridViewTimeTable_UserDeletingRow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VirtualDataGridViewTimeTable_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_UserDeletingRow(object, DataGridViewRowCancelEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_UserDeletingRow(object, DataGridViewRowCancelEventArgs)");
        }

        /// <summary>
        /// VirtualDataGridViewTimeTable_ColumnAdded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VirtualDataGridViewTimeTable_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_ColumnAdded(object, DataGridViewColumnEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 設定
            e.Column.FillWeight = 1;

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_ColumnAdded(object, DataGridViewColumnEventArgs)");
        }

        /// <summary>
        /// VirtualDataGridViewTimeTable_CellPainting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VirtualDataGridViewTimeTable_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_CellPainting(object, DataGridViewCellPaintingEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // セルの下側の境界線を「境界線なし」に設定
            e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;

            // 設定
            if (e.ColumnIndex == 2)
            {
                // セルの上側の境界線を「境界線なし」に設定
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;

                // 最初の行は上側境界線を「境界線あり」に設定する
                if (e.RowIndex == 0)
                {
                    // セルの上側の境界線を既定の境界線に設定
                    e.AdvancedBorderStyle.Top = AdvancedCellBorderStyle.Top;
                }
                // 最終行は下側境界線を「境界線あり」に設定する
                else if (e.RowIndex == Rows.Count - 1)
                {
                    // セルの上側の境界線を既定の境界線に設定
                    e.AdvancedBorderStyle.Top = AdvancedCellBorderStyle.Top;

                    // セルの下側の境界線を既定の境界線に設定
                    e.AdvancedBorderStyle.Bottom = AdvancedCellBorderStyle.Top;
                }
                else
                {
                    // 前カラムと値が同じか判定
                    if (!IsTheSameCellValue(e.ColumnIndex, 0, e.RowIndex, -1))
                    {
                        // セルの上側の境界線を既定の境界線に設定
                        e.AdvancedBorderStyle.Top = AdvancedCellBorderStyle.Top;
                    }
                }
            }
            else
            {
                // セルの上側の境界線を既定の境界線に設定
                e.AdvancedBorderStyle.Top = AdvancedCellBorderStyle.Top;

                // 最終行は下側境界線を「境界線あり」に設定する
                if (e.RowIndex == Rows.Count - 1)
                {
                    // セルの下側の境界線を既定の境界線に設定
                    e.AdvancedBorderStyle.Bottom = AdvancedCellBorderStyle.Top;
                }
            }

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_CellPainting(object, DataGridViewCellPaintingEventArgs)");
        }

        /// <summary>
        /// VirtualDataGridViewTimeTable_CellFormatting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VirtualDataGridViewTimeTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_CellFormatting(object, DataGridViewCellFormattingEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 4カラム目(駅名)以外は処理しない
            if ((e.ColumnIndex == 2) && (e.RowIndex > 0))
            {
                // 前カラムと値が同じか判定
                if (IsTheSameCellValue(e.ColumnIndex, 0, e.RowIndex, -1))
                {
                    e.Value = "";
                    e.FormattingApplied = true; // 以降の書式設定は不要
                }
            }

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_CellFormatting(object, DataGridViewCellFormattingEventArgs)");
        }

        /// <summary>
        /// VirtualDataGridViewTimeTable_MouseDoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VirtualDataGridViewTimeTable_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_MouseDoubleClick(object, MouseEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // ダブルクリックされたセルの位置を取得
            HitTestInfo hitTestInfo = ((VirtualDataGridViewTimeTable)sender).HitTest(e.X, e.Y);

            // 列車列か？
            if ((hitTestInfo.ColumnIndex >= 4) && (hitTestInfo.RowIndex >= 0 && hitTestInfo.RowIndex < 6))
            {
                // 列車情報編集
                EditTrainInformation(hitTestInfo);
            }
            // 列車列の時刻部分か？
            else if ((hitTestInfo.ColumnIndex >= 4) && (hitTestInfo.RowIndex >= 6))
            {
                // 列車時刻編集
                EditTrainTimeInformation(hitTestInfo);
            }
            // 駅列か？
            else if ((hitTestInfo.ColumnIndex < 4) && (hitTestInfo.RowIndex >= 6))
            {
                // 駅情報編集
                EditStationInformation(hitTestInfo);
            }

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::VirtualDataGridViewTimeTable_MouseDoubleClick(object, MouseEventArgs)");
        }
        #endregion

        #region ContextMenuStripイベント
        /// <summary>
        /// CutoutOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutoutOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::CutoutOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択項目取得
            TrainProperty property = GetSelectedTrainProperty();

            // 選択状態設定
            if (property == null)
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeCutoutOnClick(object, EventArgs)");

                // 選択なし
                return;
            }

            // クリップボードにコピー
            SetTrainPropertyObjectToClipboard(property);

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.GetDiagram(m_DiagramName);

            // 列車削除
            m_RouteFileProperty.RemoveTrain(m_DirectionType, property);

            // 更新
            Update(m_DirectionType, diagramProperty.TrainSequence[m_DirectionType], diagramProperty.Trains[m_DirectionType]);

            // イベント呼出
            OnTrainPropertiesUpdate(this, new TrainPropertiesUpdateEventArgs() { DirectionType = m_DirectionType, Sequences = diagramProperty.TrainSequence[m_DirectionType], Properties = diagramProperty.Trains[m_DirectionType] });

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::CutoutOnClick(object, EventArgs)");
        }

        /// <summary>
        /// CopyOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::CopyOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択項目取得
            TrainProperty property = GetSelectedTrainProperty();

            // 選択状態設定
            if (property == null)
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeCutoutOnClick(object, EventArgs)");

                // 選択なし
                return;
            }

            // クリップボードにコピー
            SetTrainPropertyObjectToClipboard(property);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::CopyOnClick(object, EventArgs)");
        }

        /// <summary>
        /// PastingTrainInFrontOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PastingTrainInFrontOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::PastingTrainInFrontOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TrainPropertyオブジェクト取得
            TrainProperty property = GetTrainPropertyObjectFromClipboard();

            // クリップボードの内容判定
            if (property == null)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::PastingTrainInFrontOnClick(object, EventArgs)");

                // 対象外
                return;
            }

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.GetDiagram(m_DiagramName);

            // 選択インデクス取得
            int index = GetSelectedTrainColumnIndex();

            // 選択インデックス判定
            if (index < 0)
            {
                index = diagramProperty.Trains[m_DirectionType].Count + 1;
            }

            // 列車挿入
            m_RouteFileProperty.InsertTrain(m_DirectionType, index, property);

            // 更新
            Update(m_DirectionType, diagramProperty.TrainSequence[m_DirectionType], diagramProperty.Trains[m_DirectionType]);

            // イベント呼出
            OnTrainPropertiesUpdate(this, new TrainPropertiesUpdateEventArgs() { DirectionType = m_DirectionType, Sequences = diagramProperty.TrainSequence[m_DirectionType], Properties = diagramProperty.Trains[m_DirectionType] });

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::PastingTrainInFrontOnClick(object, EventArgs)");
        }

        /// <summary>
        /// PastingTrainLaterOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PastingTrainLaterOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::PastingTrainLaterOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TrainPropertyオブジェクト取得
            TrainProperty property = GetTrainPropertyObjectFromClipboard();

            // クリップボードの内容判定
            if (property == null)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::PastingTrainLaterOnClick(object, EventArgs)");

                // 対象外
                return;
            }

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.GetDiagram(m_DiagramName);

            // 選択インデクス取得
            int index = GetSelectedTrainColumnIndex();

            // 選択インデックス判定
            if (index < 0)
            {
                index = diagramProperty.Trains[m_DirectionType].Count + 1;
            }

            // 列車挿入
            m_RouteFileProperty.InsertTrain(m_DirectionType, index + 1, property);

            // 更新
            Update(m_DirectionType, diagramProperty.TrainSequence[m_DirectionType], diagramProperty.Trains[m_DirectionType]);

            // イベント呼出
            OnTrainPropertiesUpdate(this, new TrainPropertiesUpdateEventArgs() { DirectionType = m_DirectionType, Sequences = diagramProperty.TrainSequence[m_DirectionType], Properties = diagramProperty.Trains[m_DirectionType] });

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::PastingTrainLaterOnClick(object, EventArgs)");
        }

        /// <summary>
        /// DeleteOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::DeleteOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択項目取得
            TrainProperty property = GetSelectedTrainProperty();

            // 選択状態設定
            if (property == null)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::DeleteOnClick(object, EventArgs)");

                // 選択なし
                return;
            }

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.GetDiagram(m_DiagramName);

            // 列車削除
            m_RouteFileProperty.RemoveTrain(m_DirectionType, property);

            // 更新
            Update(m_DirectionType, diagramProperty.TrainSequence[m_DirectionType], diagramProperty.Trains[m_DirectionType]);

            // イベント呼出
            OnTrainPropertiesUpdate(this, new TrainPropertiesUpdateEventArgs() { DirectionType = m_DirectionType, Sequences = diagramProperty.TrainSequence[m_DirectionType], Properties = diagramProperty.Trains[m_DirectionType] });

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::DeleteOnClick(object, EventArgs)");
        }

        /// <summary>
        /// PasteOnlyTheTimeOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasteOnlyTheTimeOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::PasteOnlyTheTimeOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TrainPropertyオブジェクト取得
            TrainProperty property = GetTrainPropertyObjectFromClipboard();

            // クリップボードの内容判定
            if (property == null)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::PastingOnClick(object, EventArgs)");

                // 対象外
                return;
            }

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.GetDiagram(m_DiagramName);

            // 選択インデクス取得
            int index = GetSelectedTrainColumnIndex();

            // 選択インデックス判定
            if (index < 0)
            {
                index = diagramProperty.Trains[m_DirectionType].Count + 1;
            }

            // 列車時間コピー
            m_RouteFileProperty.PasteOnlyTheTrainTime(m_DirectionType, index, property);

            // 更新
            Update(m_DirectionType, diagramProperty.TrainSequence[m_DirectionType], diagramProperty.Trains[m_DirectionType]);

            // イベント呼出
            OnTrainPropertiesUpdate(this, new TrainPropertiesUpdateEventArgs() { DirectionType = m_DirectionType, Sequences = diagramProperty.TrainSequence[m_DirectionType], Properties = diagramProperty.Trains[m_DirectionType] });

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::PasteOnlyTheTimeOnClick(object, EventArgs)");
        }

        /// <summary>
        /// TrainPropertiesOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrainPropertiesOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::TrainPropertiesOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択項目取得
            TrainProperty property = GetSelectedTrainProperty();

            // 選択状態設定
            if (property == null)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::TrainPropertiesOnClick(object, EventArgs)");

                // 選択なし
                return;
            }

            // 編集
            Edit(property);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::TrainPropertiesOnClick(object, EventArgs)");
        }

        /// <summary>
        /// InsertTrainInFrontOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertTrainInFrontOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::InsertTrainInFrontOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.GetDiagram(m_DiagramName);

            // 選択インデクス取得
            int index = GetSelectedTrainColumnIndex();

            // 選択インデックス判定
            if (index < 0)
            {
                index = 0;
            }

            // TrainPropertyオブジェクト生成
            TrainProperty property = new TrainProperty(diagramProperty.Name, m_DirectionType, m_RouteFileProperty.Stations);

            // 列車挿入
            m_RouteFileProperty.InsertTrain(m_DirectionType, index, property);

            // 更新
            Update(m_DirectionType, diagramProperty.TrainSequence[m_DirectionType], diagramProperty.Trains[m_DirectionType]);

            // イベント呼出
            OnTrainPropertiesUpdate(this, new TrainPropertiesUpdateEventArgs() { DirectionType = m_DirectionType, Sequences = diagramProperty.TrainSequence[m_DirectionType], Properties = diagramProperty.Trains[m_DirectionType] });

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::InsertTrainInFrontOnClick(object, EventArgs)");
        }

        /// <summary>
        /// InsertTrainLaterOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertTrainLaterOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::InsertTrainInFrontOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.GetDiagram(m_DiagramName);

            // 選択インデクス取得
            int index = GetSelectedTrainColumnIndex();

            // 選択インデックス判定
            if (index < 0)
            {
                index = diagramProperty.Trains[m_DirectionType].Count + 1;
            }

            // TrainPropertyオブジェクト生成
            TrainProperty property = new TrainProperty(diagramProperty.Name, m_DirectionType, m_RouteFileProperty.Stations);

            // 列車挿入
            m_RouteFileProperty.InsertTrain(m_DirectionType, index + 1, property);

            // 更新
            Update(m_DirectionType, diagramProperty.TrainSequence[m_DirectionType], diagramProperty.Trains[m_DirectionType]);

            // イベント呼出
            OnTrainPropertiesUpdate(this, new TrainPropertiesUpdateEventArgs() { DirectionType = m_DirectionType, Sequences = diagramProperty.TrainSequence[m_DirectionType], Properties = diagramProperty.Trains[m_DirectionType] });

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::InsertTrainInFrontOnClick(object, EventArgs)");
        }

        /// <summary>
        /// StationPropertiesOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StationPropertiesOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::StationPropertiesOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択行・列取得
            int columnIndex = GetSelectedColumnIndex();
            int rowIndex = GetSelectedRowIndex();

            // 選択行・列取得判定
            if (columnIndex == -1 || rowIndex == -1)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::StationPropertiesOnClick(object, EventArgs)");

                // 何もしない
                return;
            }

            // 駅情報編集
            EditStationInformation(columnIndex, rowIndex);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::StationPropertiesOnClick(object, EventArgs)");
        }

        /// <summary>
        /// StationTimePropertiesOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StationTimePropertiesOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::StationTimePropertiesOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択行・列取得
            DataGridLocation dataLocation = GetSelectedIndex();

            // 選択行・列取得判定
            if (dataLocation.Column == -1 || dataLocation.Row == -1)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::StationPropertiesOnClick(object, EventArgs)");

                // 何もしない
                return;
            }

            // 列車時刻編集
            EditTrainTimeInformation(dataLocation.Column, dataLocation.Row);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::StationTimePropertiesOnClick(object, EventArgs)");
        }

        /// <summary>
        /// ReplaceThePreviousTrainOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReplaceThePreviousTrainOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::ReplaceThePreviousTrainOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択項目取得
            TrainProperty property = GetSelectedTrainProperty();

            // 選択状態設定
            if (property == null)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::ReplaceThePreviousTrainOnClick(object, EventArgs)");

                // 選択なし
                return;
            }

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.GetDiagram(m_DiagramName);

            // 列車入替
            m_RouteFileProperty.ReplacementTrain(m_DirectionType, -1, property);

            // 更新
            Update(m_DirectionType, diagramProperty.TrainSequence[m_DirectionType], diagramProperty.Trains[m_DirectionType]);

            // イベント呼出
            OnTrainPropertiesUpdate(this, new TrainPropertiesUpdateEventArgs() { DirectionType = m_DirectionType, Sequences = diagramProperty.TrainSequence[m_DirectionType], Properties = diagramProperty.Trains[m_DirectionType] });

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::ReplaceThePreviousTrainOnClick(object, EventArgs)");
        }

        /// <summary>
        /// ReplaceWithNextTrainOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReplaceWithNextTrainOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::ReplaceWithNextTrainOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択項目取得
            TrainProperty property = GetSelectedTrainProperty();

            // 選択状態設定
            if (property == null)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::ReplaceThePreviousTrainOnClick(object, EventArgs)");

                // 選択なし
                return;
            }

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.GetDiagram(m_DiagramName);

            // 列車入替
            m_RouteFileProperty.ReplacementTrain(m_DirectionType, 1, property);

            // 更新
            Update(m_DirectionType, diagramProperty.TrainSequence[m_DirectionType], diagramProperty.Trains[m_DirectionType]);

            // イベント呼出
            OnTrainPropertiesUpdate(this, new TrainPropertiesUpdateEventArgs() { DirectionType = m_DirectionType, Sequences = diagramProperty.TrainSequence[m_DirectionType], Properties = diagramProperty.Trains[m_DirectionType] });

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::ReplaceWithNextTrainOnClick(object, EventArgs)");
        }

        /// <summary>
        /// EraseTimeOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EraseTimeOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::EraseTimeOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択行・列取得
            DataGridLocation dataLocation = GetSelectedIndex();

            // 選択行・列取得判定
            if (dataLocation.Column == -1 || dataLocation.Row == -1)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::EraseTimeOnClick(object, EventArgs)");

                // 何もしない
                return;
            }

            // 列車時刻情報を取得
            StationTimeProperty property = GetTargetTrainStationTime(dataLocation);

            // 時刻消去
            property.EraseTime();

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.GetDiagram(m_DiagramName);

            // 更新
            Update(m_DirectionType, diagramProperty.TrainSequence[m_DirectionType], diagramProperty.Trains[m_DirectionType]);

            // イベント呼出
            OnTrainPropertiesUpdate(this, new TrainPropertiesUpdateEventArgs() { DirectionType = m_DirectionType, Sequences = diagramProperty.TrainSequence[m_DirectionType], Properties = diagramProperty.Trains[m_DirectionType] });

            // 選択位置設定(復元)
            SelectionPositionSetting(dataLocation);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::EraseTimeOnClick(object, EventArgs)");
        }

        /// <summary>
        /// PassingOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PassingOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::PassingOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択行・列取得
            DataGridLocation dataLocation = GetSelectedIndex();

            // 選択行・列取得判定
            if (dataLocation.Column == -1 || dataLocation.Row == -1)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::PassingOnClick(object, EventArgs)");

                // 何もしない
                return;
            }

            // 列車時刻情報を取得
            StationTimeProperty property = GetTargetTrainStationTime(dataLocation);

            // 通過設定
            property.SetPassing();

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.GetDiagram(m_DiagramName);

            // 更新
            Update(m_DirectionType, diagramProperty.TrainSequence[m_DirectionType], diagramProperty.Trains[m_DirectionType]);

            // イベント呼出
            OnTrainPropertiesUpdate(this, new TrainPropertiesUpdateEventArgs() { DirectionType = m_DirectionType, Sequences = diagramProperty.TrainSequence[m_DirectionType], Properties = diagramProperty.Trains[m_DirectionType] });

            // 選択位置設定(復元)
            SelectionPositionSetting(dataLocation);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::PassingOnClick(object, EventArgs)");
        }

        /// <summary>
        /// PassingStoppingOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PassingStoppingOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::PassingStoppingOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択行・列取得
            DataGridLocation dataLocation = GetSelectedIndex();

            // 選択行・列取得判定
            if (dataLocation.Column == -1 || dataLocation.Row == -1)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::PassingOnClick(object, EventArgs)");

                // 何もしない
                return;
            }

            // 列車時刻情報を取得
            StationTimeProperty property = GetTargetTrainStationTime(dataLocation);

            // 通過-停車設定
            property.SetPassingStopping();

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.GetDiagram(m_DiagramName);

            // 更新
            Update(m_DirectionType, diagramProperty.TrainSequence[m_DirectionType], diagramProperty.Trains[m_DirectionType]);

            // イベント呼出
            OnTrainPropertiesUpdate(this, new TrainPropertiesUpdateEventArgs() { DirectionType = m_DirectionType, Sequences = diagramProperty.TrainSequence[m_DirectionType], Properties = diagramProperty.Trains[m_DirectionType] });

            // 選択位置設定(復元)
            SelectionPositionSetting(dataLocation);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::PassingStoppingOnClick(object, EventArgs)");
        }

        /// <summary>
        /// NoRouteOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoRouteOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::NoRouteOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択行・列取得
            DataGridLocation dataLocation = GetSelectedIndex();

            // 選択行・列取得判定
            if (dataLocation.Column == -1 || dataLocation.Row == -1)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::NoRouteOnClick(object, EventArgs)");

                // 何もしない
                return;
            }

            // 列車時刻情報を取得
            StationTimeProperty property = GetTargetTrainStationTime(dataLocation);

            // 経由なし設定
            property.SetNoRoute();

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.GetDiagram(m_DiagramName);

            // 更新
            Update(m_DirectionType, diagramProperty.TrainSequence[m_DirectionType], diagramProperty.Trains[m_DirectionType]);

            // イベント呼出
            OnTrainPropertiesUpdate(this, new TrainPropertiesUpdateEventArgs() { DirectionType = m_DirectionType, Sequences = diagramProperty.TrainSequence[m_DirectionType], Properties = diagramProperty.Trains[m_DirectionType] });

            // 選択位置設定(復元)
            SelectionPositionSetting(dataLocation);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::NoRouteOnClick(object, EventArgs)");
        }

        /// <summary>
        /// FirstTrainFromThisStationOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FirstTrainFromThisStationOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::FirstTrainFromThisStationOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択行・列取得
            DataGridLocation dataLocation = GetSelectedIndex();

            // 選択行・列取得判定
            if (dataLocation.Column == -1 || dataLocation.Row == -1)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::FirstTrainFromThisStationOnClick(object, EventArgs)");

                // 何もしない
                return;
            }

            // 選択項目取得
            TrainProperty property = GetSelectedTrainProperty();

            // 選択状態設定
            if (property == null)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::FirstTrainFromThisStationOnClick(object, EventArgs)");

                // 選択なし
                return;
            }

            // 列車時刻情報を取得
            StationTimeProperty timeProperty = GetTargetTrainStationTime(dataLocation);

            // 当駅始発設定
            property.StartStationSetting(m_DirectionType, m_RouteFileProperty.StationSequences, timeProperty.StationName);

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.GetDiagram(m_DiagramName);

            // 更新
            Update(m_DirectionType, diagramProperty.TrainSequence[m_DirectionType], diagramProperty.Trains[m_DirectionType]);

            // イベント呼出
            OnTrainPropertiesUpdate(this, new TrainPropertiesUpdateEventArgs() { DirectionType = m_DirectionType, Sequences = diagramProperty.TrainSequence[m_DirectionType], Properties = diagramProperty.Trains[m_DirectionType] });

            // 選択位置設定(復元)
            SelectionPositionSetting(dataLocation);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::FirstTrainFromThisStationOnClick(object, EventArgs)");
        }

        /// <summary>
        /// StopsAtThisStationOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopsAtThisStationOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::StopsAtThisStationOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択行・列取得
            DataGridLocation dataLocation = GetSelectedIndex();

            // 選択行・列取得判定
            if (dataLocation.Column == -1 || dataLocation.Row == -1)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::FirstTrainFromThisStationOnClick(object, EventArgs)");

                // 何もしない
                return;
            }

            // 選択項目取得
            TrainProperty property = GetSelectedTrainProperty();

            // 選択状態設定
            if (property == null)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::FirstTrainFromThisStationOnClick(object, EventArgs)");

                // 選択なし
                return;
            }

            // 列車時刻情報を取得
            StationTimeProperty timeProperty = GetTargetTrainStationTime(dataLocation);

            // 当駅止り設定
            property.StopStationSetting(m_DirectionType, m_RouteFileProperty.StationSequences, timeProperty.StationName);

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.GetDiagram(m_DiagramName);

            // 更新
            Update(m_DirectionType, diagramProperty.TrainSequence[m_DirectionType], diagramProperty.Trains[m_DirectionType]);

            // イベント呼出
            OnTrainPropertiesUpdate(this, new TrainPropertiesUpdateEventArgs() { DirectionType = m_DirectionType, Sequences = diagramProperty.TrainSequence[m_DirectionType], Properties = diagramProperty.Trains[m_DirectionType] });

            // 選択位置設定(復元)
            SelectionPositionSetting(dataLocation);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::StopsAtThisStationOnClick(object, EventArgs)");
        }

        /// <summary>
        /// DirectCommunicationOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DirectCommunicationOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::DirectCommunicationOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択行・列取得
            DataGridLocation dataLocation = GetSelectedIndex();

            // 選択行・列取得判定
            if (dataLocation.Column == -1 || dataLocation.Row == -1)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::DirectCommunicationOnClick(object, EventArgs)");

                // 何もしない
                return;
            }

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.GetDiagram(m_DiagramName);

            // 選択インデクス取得
            int index = GetSelectedTrainColumnIndex();

            // 選択インデックス判定
            if (index < 0)
            {
                index = 0;
            }

            // 選択項目取得
            TrainProperty property = GetSelectedTrainProperty();

            // 選択状態設定
            if (property == null)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::DirectCommunicationOnClick(object, EventArgs)");

                // 選択なし
                return;
            }

            // 選択項目取得
            TrainProperty nextTrainProperty = GetSelectedNextTrainProperty(1);

            // 選択状態設定
            if (nextTrainProperty == null)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::DirectCommunicationOnClick(object, EventArgs)");

                // 選択なし
                return;
            }

            // 選択駅取得
            string selectedStationName = GetSelectedStationName(dataLocation.Row);

            // 結合
            property.Join(m_RouteFileProperty.Stations, m_RouteFileProperty.StationSequences, selectedStationName, nextTrainProperty);

            // 列車削除
            m_RouteFileProperty.RemoveTrain(m_DirectionType, nextTrainProperty);

            // 更新
            Update(m_DirectionType, diagramProperty.TrainSequence[m_DirectionType], diagramProperty.Trains[m_DirectionType]);

            // イベント呼出
            OnTrainPropertiesUpdate(this, new TrainPropertiesUpdateEventArgs() { DirectionType = m_DirectionType, Sequences = diagramProperty.TrainSequence[m_DirectionType], Properties = diagramProperty.Trains[m_DirectionType] });

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::DirectCommunicationOnClick(object, EventArgs)");
        }

        /// <summary>
        /// DivisionOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DivisionOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::DivisionOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択行・列取得
            DataGridLocation dataLocation = GetSelectedIndex();

            // 選択行・列取得判定
            if (dataLocation.Column == -1 || dataLocation.Row == -1)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::DivisionOnClick(object, EventArgs)");

                // 何もしない
                return;
            }

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.GetDiagram(m_DiagramName);

            // 選択インデクス取得
            int index = GetSelectedTrainColumnIndex();

            // 選択インデックス判定
            if (index < 0)
            {
                index = 0;
            }

            // 選択項目取得
            TrainProperty property = GetSelectedTrainProperty();

            // 選択状態設定
            if (property == null)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::DivisionOnClick(object, EventArgs)");

                // 選択なし
                return;
            }

            // 選択駅取得
            string selectedStationName = GetSelectedStationName(dataLocation.Row);

            // 列車削除
            m_RouteFileProperty.RemoveTrain(m_DirectionType, property);

            // 分割
            TrainProperties divideTrains = property.Divide(m_RouteFileProperty.Stations, m_RouteFileProperty.StationSequences, selectedStationName);

            // 列車挿入
            m_RouteFileProperty.InsertTrain(m_DirectionType, index, divideTrains);

            // 更新
            Update(m_DirectionType, diagramProperty.TrainSequence[m_DirectionType], diagramProperty.Trains[m_DirectionType]);

            // イベント呼出
            OnTrainPropertiesUpdate(this, new TrainPropertiesUpdateEventArgs() { DirectionType = m_DirectionType, Sequences = diagramProperty.TrainSequence[m_DirectionType], Properties = diagramProperty.Trains[m_DirectionType] });

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::DivisionOnClick(object, EventArgs)");
        }

        #region ContextMenuStripOpened
        /// <summary>
        /// ContextMenuStripOpened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenuStripOpened(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::ContextMenuStripOpened(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択インデクス取得
            int selectedColumnIndex = GetSelectedColumnIndex();
            int selectedRowIndex = GetSelectedRowIndex();

            // 選択列判定
            if (selectedColumnIndex > 3)
            {
                // 列車列設定
                ContextMenuStripOpenedTrainColumnSelection(sender, e, selectedColumnIndex, selectedRowIndex);
            }
            else if (selectedColumnIndex >= 0)
            {
                // 駅列設定
                ContextMenuStripOpenedStationColumnSelection(sender, e, selectedColumnIndex, selectedRowIndex);
            }

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::ContextMenuStripOpened(object, EventArgs)");
        }

        /// <summary>
        /// ContextMenuStripOpenedTrainColumnSelection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="column"></param>
        /// <param name="row"></param>
        private void ContextMenuStripOpenedTrainColumnSelection(object sender, System.EventArgs e, int column, int row)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::ContextMenuStripOpenedTrainColumnSelection(object, EventArgs, int ,int)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);
            Logger.DebugFormat("column:[{0}]", column);
            Logger.DebugFormat("row   :[{0}]", row);

            #region 表示/非表示設定
            ContextMenuStrip.Items["cutout"].Visible = true;
            ContextMenuStrip.Items["copy"].Visible = true;
            ContextMenuStrip.Items["pasting_train_in_front"].Visible = true;
            ContextMenuStrip.Items["pasting_train_later"].Visible = true;
            ContextMenuStrip.Items["delete"].Visible = true;
            ContextMenuStrip.Items["separator1"].Visible = true;
            ContextMenuStrip.Items["paste_only_the_time"].Visible = true;
            ContextMenuStrip.Items["separator2"].Visible = true;
            ContextMenuStrip.Items["train_properties"].Visible = true;
            ContextMenuStrip.Items["insert_train_in_front"].Visible = true;
            ContextMenuStrip.Items["insert_train_later"].Visible = true;
            ContextMenuStrip.Items["separator3"].Visible = true;
            ContextMenuStrip.Items["station_properties"].Visible = false;
            ContextMenuStrip.Items["station_time_properties"].Visible = true;
            ContextMenuStrip.Items["separator4"].Visible = true;
            ContextMenuStrip.Items["replace_the_previous_train"].Visible = true;
            ContextMenuStrip.Items["replace_with_next_train"].Visible = true;
            ContextMenuStrip.Items["separator5"].Visible = true;
            ContextMenuStrip.Items["erase_time"].Visible = true;
            ContextMenuStrip.Items["passing"].Visible = true;
            ContextMenuStrip.Items["passing_stopping"].Visible = true;
            ContextMenuStrip.Items["no_route"].Visible = true;
            ContextMenuStrip.Items["separator6"].Visible = true;
            ContextMenuStrip.Items["first_train_from_this_station"].Visible = true;
            ContextMenuStrip.Items["stops_at_this_station"].Visible = true;
            ContextMenuStrip.Items["direct_communication"].Visible = true;
            ContextMenuStrip.Items["division"].Visible = true;
            #endregion

            // 列車列の場合
            ContextMenuStrip.Items["cutout"].Enabled = true;
            ContextMenuStrip.Items["copy"].Enabled = true;
            ContextMenuStrip.Items["delete"].Enabled = true;
            ContextMenuStrip.Items["train_properties"].Enabled = true;
            ContextMenuStrip.Items["insert_train_in_front"].Enabled = true;
            ContextMenuStrip.Items["insert_train_later"].Enabled = true;
            if (column == 4)
            {
                ContextMenuStrip.Items["replace_the_previous_train"].Enabled = false;
            }
            else
            {
                ContextMenuStrip.Items["replace_the_previous_train"].Enabled = true;
            }
            if (column == ColumnCount - 1)
            {
                ContextMenuStrip.Items["replace_with_next_train"].Enabled = false;
            }
            else
            {
                ContextMenuStrip.Items["replace_with_next_train"].Enabled = true;
            }
            if ((row > 5) && (row < RowCount - 1))
            {
                ContextMenuStrip.Items["station_time_properties"].Enabled = true;
                ContextMenuStrip.Items["station_properties"].Enabled = true;
                ContextMenuStrip.Items["erase_time"].Enabled = true;
                ContextMenuStrip.Items["passing"].Enabled = true;
                ContextMenuStrip.Items["passing_stopping"].Enabled = true;
                ContextMenuStrip.Items["no_route"].Enabled = true;
                ContextMenuStrip.Items["first_train_from_this_station"].Enabled = true;
                ContextMenuStrip.Items["stops_at_this_station"].Enabled = true;

                // 選択項目取得
                TrainProperty property = GetSelectedTrainProperty();

                // 無効化
                ContextMenuStrip.Items["direct_communication"].Enabled = false;
                ContextMenuStrip.Items["division"].Enabled = false;

                // 直通化/分断化設定
                if (property != null)
                {
                    // 選択駅取得
                    string selectedStationName = GetSelectedStationName(row);

                    // 選択項目取得
                    TrainProperty nextTrainProperty = GetSelectedNextTrainProperty(1);

                    // 直通化判定
                    if (property.IsJoin(m_RouteFileProperty.Stations, m_RouteFileProperty.StationSequences, selectedStationName, nextTrainProperty))
                    {
                        // 有効化
                        ContextMenuStrip.Items["direct_communication"].Enabled = true;
                    }

                    // 分割化判定
                    if (property.IsDivisible(m_RouteFileProperty.Stations, m_RouteFileProperty.StationSequences, selectedStationName))
                    {
                        // 有効化
                        ContextMenuStrip.Items["division"].Enabled = true;
                    }
                }
            }
            else
            {
                ContextMenuStrip.Items["station_time_properties"].Enabled = false;
                ContextMenuStrip.Items["station_properties"].Enabled = false;
                ContextMenuStrip.Items["erase_time"].Enabled = false;
                ContextMenuStrip.Items["passing"].Enabled = false;
                ContextMenuStrip.Items["passing_stopping"].Enabled = false;
                ContextMenuStrip.Items["no_route"].Enabled = false;
                ContextMenuStrip.Items["first_train_from_this_station"].Enabled = false;
                ContextMenuStrip.Items["stops_at_this_station"].Enabled = false;
                ContextMenuStrip.Items["direct_communication"].Enabled = false;
                ContextMenuStrip.Items["division"].Enabled = false;
            }

            // クリップボードからコピー
            IDataObject dataObject = Clipboard.GetDataObject();

            // クリップボードの内容判定
            if (!(dataObject != null && dataObject.GetDataPresent(typeof(TrainProperty))))
            {
                ContextMenuStrip.Items["pasting_train_in_front"].Enabled = false;
                ContextMenuStrip.Items["pasting_train_later"].Enabled = false;
                ContextMenuStrip.Items["paste_only_the_time"].Enabled = false;
            }
            else
            {
                ContextMenuStrip.Items["pasting_train_in_front"].Enabled = true;
                ContextMenuStrip.Items["pasting_train_later"].Enabled = true;
                ContextMenuStrip.Items["paste_only_the_time"].Enabled = true;
            }

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::ContextMenuStripOpenedTrainColumnSelection(object, EventArgs, int ,int)");
        }

        /// <summary>
        /// ContextMenuStripOpenedStationColumnSelection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="column"></param>
        /// <param name="row"></param>
        private void ContextMenuStripOpenedStationColumnSelection(object sender, System.EventArgs e, int column, int row)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::ContextMenuStripOpenedStationColumnSelection(object, EventArgs, int ,int)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);
            Logger.DebugFormat("column:[{0}]", column);
            Logger.DebugFormat("row   :[{0}]", row);

            #region 表示/非表示設定
            ContextMenuStrip.Items["cutout"].Visible = false;
            ContextMenuStrip.Items["copy"].Visible = false;
            ContextMenuStrip.Items["pasting_train_in_front"].Visible = false;
            ContextMenuStrip.Items["pasting_train_later"].Visible = false;
            ContextMenuStrip.Items["delete"].Visible = false;
            ContextMenuStrip.Items["separator1"].Visible = false;
            ContextMenuStrip.Items["paste_only_the_time"].Visible = false;
            ContextMenuStrip.Items["separator2"].Visible = false;
            ContextMenuStrip.Items["train_properties"].Visible = false;
            ContextMenuStrip.Items["insert_train_in_front"].Visible = false;
            ContextMenuStrip.Items["insert_train_later"].Visible = false;
            ContextMenuStrip.Items["separator3"].Visible = false;
            ContextMenuStrip.Items["station_properties"].Visible = true;
            ContextMenuStrip.Items["station_time_properties"].Visible = false;
            ContextMenuStrip.Items["separator4"].Visible = false;
            ContextMenuStrip.Items["replace_the_previous_train"].Visible = false;
            ContextMenuStrip.Items["replace_with_next_train"].Visible = false;
            ContextMenuStrip.Items["separator5"].Visible = false;
            ContextMenuStrip.Items["erase_time"].Visible = false;
            ContextMenuStrip.Items["passing"].Visible = false;
            ContextMenuStrip.Items["passing_stopping"].Visible = false;
            ContextMenuStrip.Items["no_route"].Visible = false;
            ContextMenuStrip.Items["separator6"].Visible = false;
            ContextMenuStrip.Items["first_train_from_this_station"].Visible = false;
            ContextMenuStrip.Items["stops_at_this_station"].Visible = false;
            ContextMenuStrip.Items["direct_communication"].Visible = false;
            ContextMenuStrip.Items["division"].Visible = false;
            #endregion

            // 駅列の場合
            ContextMenuStrip.Items["cutout"].Enabled = false;
            ContextMenuStrip.Items["copy"].Enabled = false;
            ContextMenuStrip.Items["pasting_train_in_front"].Visible = false;
            ContextMenuStrip.Items["pasting_train_later"].Visible = false;
            ContextMenuStrip.Items["delete"].Enabled = false;
            ContextMenuStrip.Items["paste_only_the_time"].Enabled = false;
            ContextMenuStrip.Items["train_properties"].Enabled = false;
            ContextMenuStrip.Items["insert_train_in_front"].Enabled = false;
            ContextMenuStrip.Items["insert_train_later"].Enabled = false;
            if ((row > 5) && (row < RowCount - 1))
            {
                ContextMenuStrip.Items["station_properties"].Enabled = true;
            }
            else
            {
                ContextMenuStrip.Items["station_properties"].Enabled = false;
            }
            ContextMenuStrip.Items["station_time_properties"].Enabled = false;
            ContextMenuStrip.Items["replace_the_previous_train"].Enabled = false;
            ContextMenuStrip.Items["replace_with_next_train"].Enabled = false;
            ContextMenuStrip.Items["erase_time"].Enabled = false;
            ContextMenuStrip.Items["passing"].Enabled = false;
            ContextMenuStrip.Items["passing_stopping"].Enabled = false;
            ContextMenuStrip.Items["no_route"].Enabled = false;
            ContextMenuStrip.Items["first_train_from_this_station"].Enabled = false;
            ContextMenuStrip.Items["stops_at_this_station"].Enabled = false;
            ContextMenuStrip.Items["direct_communication"].Enabled = false;
            ContextMenuStrip.Items["division"].Enabled = false;

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::ContextMenuStripOpenedStationColumnSelection(object, EventArgs, int ,int)");
        }
        #endregion
        #endregion
        #endregion

        #region publicメソッド
        #region 描画
        /// <summary>
        /// 描画
        /// </summary>
        public void Draw()
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::Draw()");

            // 描画一時停止
            SuspendLayout();

            // 列、行削除
            RowCount = 0;
            ColumnCount = 0;

            // 列描画
            DrawColumns();

            // 列車番号描画
            DrawRowsTrainNumber();

            // 列車種別描画
            DrawRowsTrainType();

            // 列車名描画
            DrawRowsTrainName();

            // 列車記号描画
            DrawRowsTrainMark();

            // 始発駅描画
            DrawRowsDepartingStation();

            // 終着駅描画
            DrawRowsDestinationStation();

            // 駅時刻描画
            DrawRowsStationTime();

            // 備考描画
            DrawRowsRemarksStation();

            // 描画再開
            ResumeLayout();

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::Draw()");
        }

        /// <summary>
        /// 列描画
        /// </summary>
        private void DrawColumns()
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::DrawColumns()");

            // 固定カラム追加
            Columns.Add("Distance", "距離");
            Columns.Add("StationMark", "駅マーク");
            Columns.Add("StationName", "駅名");
            Columns.Add("ArrivalAndDeparture", "発着");

            // 固定カラム幅設定
            Columns[0].Width = 48;  // 距離
            Columns[1].Width = 32;  // 駅マーク
            Columns[2].Width = 24 * m_RouteFileProperty.Route.WidthOfStationNameField;  // 駅名
            Columns[3].Width = 24;  // 発着

            // 固定カラム背景色設定
            Columns[0].DefaultCellStyle.BackColor = SystemColors.Control;
            Columns[1].DefaultCellStyle.BackColor = SystemColors.Control;

            // 固定カラムフォント設定
            Columns[0].DefaultCellStyle.Font = m_FontDictionary["駅間距離"];

            // 文字位置設定
            Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // 列車描写プロパティを繰り返す
            foreach (TrainProperty train in m_DrawTrainProperties)
            {
                // 列追加
                Columns.Add(string.Format("Train{0}", train.Id), string.Format("{0}", train.No));
                Columns[Columns.Count - 1].Width = 8 * m_RouteFileProperty.Route.TimetableTrainWidth;  // 列車
                Columns[Columns.Count - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Columns[Columns.Count - 1].DefaultCellStyle.ForeColor = m_RouteFileProperty.TrainTypes.GetTrainType(train.TrainTypeName).StringsColor;
            }

            // 固定列(発着)設定
            Columns["ArrivalAndDeparture"].Frozen = true;

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::DrawColumns()");
        }

        /// <summary>
        /// 列車番号描画
        /// </summary>
        private void DrawRowsTrainNumber()
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::DrawRowsTrainNumber()");

            // 行追加
            RowCount += 1;

            // フォント設定
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_FontDictionary["列車番号"];
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.BackColor = SystemColors.Control;

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::DrawRowsTrainNumber()");
        }

        /// <summary>
        /// 列車種別描画
        /// </summary>
        private void DrawRowsTrainType()
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::DrawRowsTrainType()");

            // 行追加
            RowCount += 1;

            // フォント設定
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_FontDictionary["列車種別"];
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.BackColor = SystemColors.Control;

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::DrawRowsTrainType()");
        }

        /// <summary>
        /// 列車名描画
        /// </summary>
        private void DrawRowsTrainName()
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::DrawRowsTrainName()");

            // 行追加
            RowCount += 1;

            // フォント設定
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_FontDictionary["列車名"];
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.BackColor = SystemColors.Control;

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::DrawRowsTrainName()");
        }

        /// <summary>
        /// 列車記号描画
        /// </summary>
        private void DrawRowsTrainMark()
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::DrawRowsTrainMark()");

            // 行追加
            RowCount += 1;

            // フォント設定
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_FontDictionary["列車記号"];
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.BackColor = SystemColors.Control;

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::DrawRowsTrainMark()");
        }

        /// <summary>
        /// 始発駅描画
        /// </summary>
        private void DrawRowsDepartingStation()
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::DrawRowsDepartingStation()");

            // 行追加
            RowCount += 1;

            // フォント設定
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_FontDictionary["始発駅"];
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.BackColor = SystemColors.Control;

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::DrawRowsDepartingStation()");
        }

        /// <summary>
        /// 終着駅描画
        /// </summary>
        private void DrawRowsDestinationStation()
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::DrawRowsDestinationStation()");

            // 行追加
            RowCount += 1;

            // フォント設定
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_FontDictionary["終着駅"];
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.BackColor = SystemColors.Control;
            Rows[Rows.Count - 1].Frozen = true;

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::DrawRowsDestinationStation()");
        }

        /// <summary>
        /// 駅時刻描画
        /// </summary>
        private void DrawRowsStationTime()
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::DrawRowsStationTime()");

            // 方向種別で分岐
            switch (m_DirectionType)
            {
                case DirectionType.Outbound:
                    // 駅時刻描画(下り)
                    DrawRowsOutboundStationTime();
                    break;
                case DirectionType.Inbound:
                    // 駅時刻描画(上り)
                    DrawRowsInboundStationTime();
                    break;
                default:
                    throw new AggregateException(string.Format("方向種別の異常を検出しました:[{0}]", m_DirectionType));
            }

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::DrawRowsStationTime()");
        }

        /// <summary>
        /// 駅時刻描画(下り)
        /// </summary>
        private void DrawRowsOutboundStationTime()
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::DrawRowsInboundStationTime()");

            // 初期化
            m_StationDrawRowInfomation.Clear();

            // 駅シーケンスリスト取得(昇順)
            var stationSequences = m_RouteFileProperty.StationSequences.OrderBy(t => t.Seq);

            // 駅を繰り返す
            foreach (var stationSequence in stationSequences)
            {
                // 駅情報取得
                StationProperty station = m_RouteFileProperty.Stations.Find(t => t.Name == stationSequence.Name);

                // 駅時刻行取得
                Dictionary<DepartureArrivalType, List<string>> columnsList = GetStationTimeRows(DirectionType.Outbound, station);

                // 駅時刻行分繰り返す
                foreach (var departureArrivalType in columnsList.Keys)
                {
                    var info = new StationDrawRowInfomation()
                    {
                        Name = station.Name,
                        DepartureArrivalType = departureArrivalType,
                        Distance = GetDistanceStringBetweenStations(DirectionType.Outbound, station),
                    };

                    // 登録
                    m_StationDrawRowInfomation.Add(info);
                }

                // 行追加
                RowCount += columnsList.Count;
            }

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::DrawRowsInboundStationTime()");
        }

        /// <summary>
        /// 駅時刻描画(上り)
        /// </summary>
        private void DrawRowsInboundStationTime()
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::DrawRowsInboundStationTime()");

            // 初期化
            m_StationDrawRowInfomation.Clear();

            // 駅シーケンスリスト取得(降順)
            var stationSequences = m_RouteFileProperty.StationSequences.OrderByDescending(t => t.Seq);

            // 駅を繰り返す
            foreach (var stationSequence in stationSequences)
            {
                // 駅情報取得
                StationProperty station = m_RouteFileProperty.Stations.Find(t => t.Name == stationSequence.Name);

                // 駅時刻行取得
                Dictionary<DepartureArrivalType, List<string>> columnsList = GetStationTimeRows(DirectionType.Inbound, station);

                // 駅時刻行分繰り返す
                foreach (var departureArrivalType in columnsList.Keys)
                {
                    var info = new StationDrawRowInfomation()
                    {
                        Name = station.Name,
                        DepartureArrivalType = departureArrivalType,
                        Distance = GetDistanceStringBetweenStations(DirectionType.Inbound, station),
                    };

                    // 登録
                    m_StationDrawRowInfomation.Add(info);
                }

                // 行追加
                RowCount += columnsList.Count;
            }

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::DrawRowsInboundStationTime()");
        }

        /// <summary>
        /// 備考描画
        /// </summary>
        private void DrawRowsRemarksStation()
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::DrawRowsRemarksStation()");

            // 行追加
            RowCount += 1;

            // TODO:フォント設定(暫定)
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_FontDictionary["備考"];

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::DrawRowsRemarksStation()");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        public void Update(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::Update(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 旧データと同一か？
            if (!m_OldRouteFileProperty.Compare(property))
            {
                // データ更新
                m_RouteFileProperty.Copy(property);

                // 描写情報更新
                UpdateDrawTrainProperties();

                // 描画
                Draw();

                // 旧データ更新
                m_OldRouteFileProperty.Copy(property);
            }

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::Update(RouteFileProperty)");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sequence"></param>
        /// <param name="properties"></param>
        public void Update(DirectionType type, TrainSequenceProperties sequence, TrainProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::Update(TrainSequenceProperties, TrainProperties)");
            Logger.DebugFormat("type      :[{0}]", type.GetStringValue());
            Logger.DebugFormat("sequence  :[{0}]", sequence);
            Logger.DebugFormat("properties:[{0}]", properties);

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.Diagrams.Find(t => t.Name == m_DiagramName);

            // データ更新
            diagramProperty.TrainSequence[type].Copy(sequence);
            diagramProperty.Trains[type].Copy(properties);

            // 描写情報更新
            UpdateDrawTrainProperties();

            // 描画
            Draw();

            // 旧データ更新
            m_OldRouteFileProperty.Copy(m_RouteFileProperty);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::Update(TrainSequenceProperties, TrainProperties)");
        }

        #region 描画対象列車情報更新
        /// <summary>
        /// 描画対象列車情報更新
        /// </summary>
        private void UpdateDrawTrainProperties()
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::UpdateDrawTrainProperties()");

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.GetDiagram(m_DiagramName);

            // TrainSequencePropertiesオブジェクト取得
            TrainSequenceProperties sequences = diagramProperty.TrainSequence[m_DirectionType];

            // TrainPropertiesオブジェクト取得
            TrainProperties trains = diagramProperty.Trains[m_DirectionType];

            // クリア
            m_DrawTrainProperties.Clear();

            // シーケンスを繰り返す
            foreach (var sequence in sequences.OrderBy(s=>s.Seq))
            {
                // TrainPropertyオブジェクト取得
                TrainProperty train = trains.Find(t=>t.Id == sequence.Id);

                // 登録
                m_DrawTrainProperties.Add(train);
            }

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::UpdateDrawTrainProperties()");
        }
        #endregion
        #endregion

        #region 編集
        /// <summary>
        /// 編集
        /// </summary>
        /// <param name="property"></param>
        public void Edit(TrainProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::Edit(TrainProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // FormTrainPropertyオブジェクト生成
            FormTrainProperty form = new FormTrainProperty(m_RouteFileProperty, property);

            // FormTrainProperty表示
            DialogResult dialogResult = form.ShowDialog();

            // FormTrainProperty表示結果判定
            if (dialogResult == DialogResult.OK)
            {
                // 結果比較
                if (!property.Compare(form.Property))
                {
                    // 結果保存
                    property.Copy(form.Property);

                    // 更新通知
                    OnTrainPropertyUpdate(this, new TrainPropertyUpdateEventArgs() { Property = property });
                }
            }

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::Edit(TrainProperty)");
        }
        #endregion
        #endregion

        #region privateメソッド
        #region カラムリスト初期化
        /// <summary>
        /// カラムリスト初期化
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<string> ColumnsListInitialization(string name)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::ColumnsListInitialization(string)");
            Logger.DebugFormat("name:[{0}]", name);

            // 結果オブジェクト生成
            List<string> result = new List<string>()
            {
                "",     // 距離
                "",     // 駅マーク
                name,   // 駅名
                ""      // 発着
            };

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::ColumnsListInitialization(string)");

            // 返却
            return result;
        }

        /// <summary>
        /// カラムリスト初期化
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arrivalAndDeparture"></param>
        /// <returns></returns>
        private List<string> ColumnsListInitialization(string name, string arrivalAndDeparture)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::ColumnsListInitialization(string, string)");
            Logger.DebugFormat("name               :[{0}]", name);
            Logger.DebugFormat("arrivalAndDeparture:[{0}]", arrivalAndDeparture);

            // 結果オブジェクト生成
            List<string> result = new List<string>()
            {
                "",                     // 距離
                "",                     // 駅マーク
                name,                   // 駅名
                arrivalAndDeparture     // 発着
            };

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::ColumnsListInitialization(string, string)");

            // 返却
            return result;
        }

        /// <summary>
        /// カラムリスト初期化
        /// </summary>
        /// <param name="property"></param>
        private Dictionary<DepartureArrivalType, List<string>> ColumnsListInitialization(DirectionType type, StationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::ColumnsListInitialization(DirectionType, StationProperty)");
            Logger.DebugFormat("type    :[{0}]", type);
            Logger.DebugFormat("property:[{0}]", property);

            // 結果オブジェクト生成
            Dictionary<DepartureArrivalType, List<string>> result = new Dictionary<DepartureArrivalType, List<string>>();

            // 方向種別で分岐
            switch (type)
            {
                case DirectionType.Outbound:
                    // 時刻形式で分岐
                    switch (property.TimeFormat)
                    {
                        case TimeFormat.DepartureTime:
                            // 設定
                            result.Add(DepartureArrivalType.Departure, ColumnsListInitialization(property.Name, "発"));
                            break;
                        case TimeFormat.DepartureAndArrival:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, ColumnsListInitialization(property.Name, "着"));
                            result.Add(DepartureArrivalType.Departure, ColumnsListInitialization(property.Name, "発"));
                            break;
                        case TimeFormat.OutboundArrivalTime:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, ColumnsListInitialization(property.Name, "着"));
                            break;
                        case TimeFormat.InboundArrivalTime:
                            // 設定
                            result.Add(DepartureArrivalType.Departure, ColumnsListInitialization(property.Name, "発"));
                            break;
                        case TimeFormat.OutboundArrivalAndDeparture:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, ColumnsListInitialization(property.Name, "着"));
                            result.Add(DepartureArrivalType.Departure, ColumnsListInitialization(property.Name, "発"));
                            break;
                        case TimeFormat.InboundDepartureAndArrival:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, ColumnsListInitialization(property.Name, "着"));
                            result.Add(DepartureArrivalType.Departure, ColumnsListInitialization(property.Name, "発"));
                            break;
                        default:
                            throw new AggregateException(string.Format("時刻形式の異常を検出しました:[{0}]", property.TimeFormat));
                    }
                    break;
                case DirectionType.Inbound:
                    // 時刻形式で分岐
                    switch (property.TimeFormat)
                    {
                        case TimeFormat.DepartureTime:
                            // 設定
                            result.Add(DepartureArrivalType.Departure, ColumnsListInitialization(property.Name, "発"));
                            break;
                        case TimeFormat.DepartureAndArrival:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, ColumnsListInitialization(property.Name, "着"));
                            result.Add(DepartureArrivalType.Departure, ColumnsListInitialization(property.Name, "発"));
                            break;
                        case TimeFormat.OutboundArrivalTime:
                            // 設定
                            result.Add(DepartureArrivalType.Departure, ColumnsListInitialization(property.Name, "発"));
                            break;
                        case TimeFormat.InboundArrivalTime:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, ColumnsListInitialization(property.Name, "着"));
                            break;
                        case TimeFormat.OutboundArrivalAndDeparture:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, ColumnsListInitialization(property.Name, "着"));
                            result.Add(DepartureArrivalType.Departure, ColumnsListInitialization(property.Name, "発"));
                            break;
                        case TimeFormat.InboundDepartureAndArrival:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, ColumnsListInitialization(property.Name, "着"));
                            result.Add(DepartureArrivalType.Departure, ColumnsListInitialization(property.Name, "発"));
                            break;
                        default:
                            throw new AggregateException(string.Format("時刻形式の異常を検出しました:[{0}]", property.TimeFormat));
                    }
                    break;
                default:
                    throw new AggregateException(string.Format("方向種別の異常を検出しました:[{0}]", type));
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::ColumnsListInitialization(DirectionType, StationProperty)");

            // 返却
            return result;
        }
        #endregion

        #region クリップボード関連
        /// <summary>
        /// TrainPropertyオブジェクト設定(クリップボード)
        /// </summary>
        /// <param name="property"></param>
        public void SetTrainPropertyObjectToClipboard(TrainProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::SetTrainPropertyObjectToClipboard(TrainProperty)");

            // クリップボードにコピー
            Clipboard.SetDataObject(new TrainProperty(property), false);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::SetTrainPropertyObjectToClipboard(TrainProperty)");
        }

        /// <summary>
        /// TrainPropertyオブジェクト取得(クリップボード)
        /// </summary>
        /// <returns></returns>
        private TrainProperty GetTrainPropertyObjectFromClipboard()
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetTrainPropertyObjectFromClipboard()");

            // クリップボードからコピー
            IDataObject dataObject = Clipboard.GetDataObject();

            // クリップボードの内容判定
            if (!(dataObject != null && dataObject.GetDataPresent(typeof(TrainProperty))))
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetTrainPropertyObjectFromClipboard()");

                // データなし
                return null;
            }

            // TrainPropertyオブジェクト取得
            TrainProperty result = dataObject.GetData(typeof(TrainProperty)) as TrainProperty;

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetTrainPropertyObjectFromClipboard()");

            // 返却
            return result;
        }
        #endregion

        #region セル比較
        /// <summary>
        /// セル比較
        /// </summary>
        /// <param name="column"></param>
        /// <param name="columnOffset"></param>
        /// <param name="row"></param>
        /// <param name="rowOffset"></param>
        /// <returns></returns>
        private bool IsTheSameCellValue(int column, int columnOffset, int row, int rowOffset)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::IsTheSameCellValue(int, int, int, int)");
            Logger.DebugFormat("column      :[{0}]", column);
            Logger.DebugFormat("columnOffset:[{0}]", columnOffset);
            Logger.DebugFormat("row         :[{0}]", row);
            Logger.DebugFormat("rowOffset   :[{0}]", rowOffset);

            // 結果設定
            bool result = true;

            // DataGridViewCellオブジェクト取得
            DataGridViewCell srcDataGridViewCell = this[column, row];
            DataGridViewCell dstDataGridViewCell = this[column + columnOffset, row + rowOffset];

            // ロギング
            Logger.DebugFormat("srcDataGridViewCell:[{0}]", srcDataGridViewCell.Value);
            Logger.DebugFormat("dstDataGridViewCell:[{0}]", dstDataGridViewCell.Value);

            // 判定用ループ
            do
            {
                // セルの値を判定
                if (srcDataGridViewCell.Value == null || dstDataGridViewCell.Value == null)
                {
                    // 結果設定
                    result = false;
                    break;
                }

                // 文字列としてセルの値を比較
                if (srcDataGridViewCell.Value.ToString() != dstDataGridViewCell.Value.ToString())
                {
                    // 結果設定
                    result = false;
                    break;
                }
                break;
            } while (true);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::IsTheSameCellValue(int, int, int, int)");

            // 返却
            return result;
        }
        #endregion

        #region 選択位置設定
        /// <summary>
        /// 選択位置設定
        /// </summary>
        /// <param name="location"></param>
        private void SelectionPositionSetting(DataGridLocation location)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::SelectionPositionSetting(DataGridLocation)");
            Logger.DebugFormat("location:[{0}]", location);

            // 設定
            SelectionPositionSetting(location.Column, location.Row);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::SelectionPositionSetting(DataGridLocation)");
        }

        /// <summary>
        /// 選択位置設定
        /// </summary>
        /// <param name="location"></param>
        private void SelectionPositionSetting(int column, int row)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::SelectionPositionSetting(int, int)");
            Logger.DebugFormat("column:[{0}]", column);
            Logger.DebugFormat("row   :[{0}]", row);

            // 設定
            this[column, row].Selected = true;

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::SelectionPositionSetting(int, int)");
        }
        #endregion

        #region 選択インデックス取得
        /// <summary>
        /// 選択インデックス取得
        /// </summary>
        /// <returns></returns>
        private DataGridLocation GetSelectedIndex()
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetSelectedIndex()");

            // 結果オブジェクト生成
            DataGridLocation result = new DataGridLocation();

            // 選択インデックス取得
            result.Column = GetSelectedColumnIndex();
            result.Row = GetSelectedRowIndex();

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetSelectedIndex()");

            // 返却
            return result;
        }

        /// <summary>
        /// 選択インデックス(列)取得
        /// </summary>
        /// <returns></returns>
        private int GetSelectedColumnIndex()
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetSelectedColumnIndex()");

            // 選択状態設定
            if (SelectedCells.Count == 0)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetSelectedColumnIndex()");

                // 選択なし
                return -1;
            }

            // 選択インデックス設定
            int result = SelectedCells[0].ColumnIndex;

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetSelectedColumnIndex()");

            // 返却
            return result;
        }

        /// <summary>
        /// 選択インデックス(行)取得
        /// </summary>
        /// <returns></returns>
        private int GetSelectedRowIndex()
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetSelectedRowIndex()");

            // 選択状態設定
            if (SelectedCells.Count == 0)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetSelectedRowIndex()");

                // 選択なし
                return -1;
            }

            // 選択インデックス設定
            int result = SelectedCells[0].RowIndex;

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetSelectedRowIndex()");

            // 返却
            return result;
        }

        /// <summary>
        /// 選択列車インデックス取得
        /// </summary>
        /// <returns></returns>
        private int GetSelectedTrainColumnIndex()
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetSelectedTrainColumnIndex()");

            // 選択インデックス設定
            int result = GetSelectedColumnIndex() - 4;

            // 選択状態設定
            if (result < 0)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetSelectedTrainColumnIndex()");

                // 選択なし
                return -1;
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetSelectedTrainColumnIndex()");

            // 返却
            return result;
        }
        #endregion

        #region 選択列車情報取得
        /// <summary>
        /// 選択列車情報取得
        /// </summary>
        /// <returns></returns>
        private TrainProperty GetSelectedTrainProperty()
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetSelectedTrainProperty()");

            // 選択インデックス設定
            int index = GetSelectedTrainColumnIndex();

            // 選択状態設定
            if (index < 0)
            {
                // ロギング
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetSelectedTrainProperty()");

                // 選択なし
                return null;
            }

            // 列車情報を取得
            TrainProperty result = m_DrawTrainProperties[index];

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetSelectedCondition()");

            // 返却
            return result;
        }

        /// <summary>
        /// 選択隣列車情報取得
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private TrainProperty GetSelectedNextTrainProperty(int offset)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetSelectedNextTrainProperty(int)");
            Logger.DebugFormat("offset:[{0}]", offset);

            // 選択インデックス設定
            int index = GetSelectedTrainColumnIndex();

            // 選択状態判定
            if (index < 0)
            {
                // ロギング
                Logger.Debug("result:[選択なし]");
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetSelectedNextTrainProperty(int)");

                // 選択なし
                return null;
            }

            // 選択次判定
            if (index + offset < 0 || index + offset >= m_DrawTrainProperties.Count)
            {
                // ロギング
                Logger.Debug("result:[範囲対象外]");
                Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetSelectedNextTrainProperty(int)");

                // 範囲対象外
                return null;
            }

            // 列車情報を取得
            TrainProperty result = m_DrawTrainProperties[index + offset];

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetSelectedNextTrainProperty(int)");

            // 返却
            return result;
        }
        #endregion

        #region 描画文字列取得
        /// <summary>
        /// 駅距離取得
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private string GetStationDistanceCellValueNeeded(int row)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetStationDistanceCellValueNeeded(int)");
            Logger.DebugFormat("row:[{0}]", row);

            // 結果設定
            string result = m_StationDrawRowInfomation[row].Distance;

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetStationDistanceCellValueNeeded(int)");

            // 返却
            return result;
        }

        /// <summary>
        /// 駅マーク取得
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private string GetStationMarkCellValueNeeded(int row)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetStationMarkCellValueNeeded(int)");
            Logger.DebugFormat("row:[{0}]", row);

            // TODO:未実装
            string result = string.Empty;

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetStationMarkCellValueNeeded(int)");

            // 返却
            return result;
        }

        /// <summary>
        /// 駅名取得
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private string GetStationNameCellValueNeeded(int row)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetStationNameCellValueNeeded(int)");
            Logger.DebugFormat("row:[{0}]", row);

            // 結果設定
            string result = m_StationDrawRowInfomation[row].Name;

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetStationNameCellValueNeeded(int)");

            // 返却
            return result;
        }

        /// <summary>
        /// 駅発着種別取得
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private string GetDepartureArrivalTypeCellValueNeeded(int row)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetTrainNoCellValueNeeded(int)");
            Logger.DebugFormat("row:[{0}]", row);

            // 結果設定
            string result = m_StationDrawRowInfomation[row].DepartureArrivalType.GetStringValue();

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetTrainNoCellValueNeeded(int)");

            // 返却
            return result;
        }

        /// <summary>
        /// 列車番号取得
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private string GetTrainNoCellValueNeeded(int column)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetTrainNoCellValueNeeded(int)");
            Logger.DebugFormat("column:[{0}]", column);

            // 結果設定
            string result = m_DrawTrainProperties[column].No;

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetTrainNoCellValueNeeded(int)");

            // 返却
            return result;
        }

        /// <summary>
        /// 列車種別取得
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private string GetTrainTypeCellValueNeeded(int column)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetTrainTypeCellValueNeeded(int)");
            Logger.DebugFormat("column:[{0}]", column);

            // 結果設定
            string result = string.Format("{0}", m_DiaProFont[m_DrawTrainProperties[column].TrainTypeName]);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetTrainTypeCellValueNeeded(int)");

            // 返却
            return result;
        }

        /// <summary>
        /// 列車名取得
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private string GetTrainNameCellValueNeeded(int column)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetTrainNameCellValueNeeded(int)");
            Logger.DebugFormat("column:[{0}]", column);

            // 結果初期化
            StringBuilder result = new StringBuilder();

            // 列車名追加
            result.Append(m_DrawTrainProperties[column].Name);

            // 列車号数追加
            if (m_DrawTrainProperties[column].Number != string.Empty)
            {
                result.Append(string.Format("{0}号", m_DrawTrainProperties[column].Number));
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetTrainNameCellValueNeeded(int)");

            // 返却
            return StringLibrary.VerticalText(result.ToString());
        }

        /// <summary>
        /// 列車記号取得
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private string GetTrainMarkCellValueNeeded(int column)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetTrainMarkCellValueNeeded(int)");
            Logger.DebugFormat("column:[{0}]", column);

            // 結果初期化
            StringBuilder result = new StringBuilder();


            // 記号分繰り返す
            foreach (var mark in m_DrawTrainProperties[column].Marks)
            {
                // 追加
                result.AppendLine(string.Format("{0}", m_DiaProFont[mark.MarkName]));
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetTrainMarkCellValueNeeded(int)");

            // 返却
            return result.ToString().TrimEnd(new char[] { '\r', '\n' });
        }

        /// <summary>
        /// 始発駅取得
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private string GetDepartingStationCellValueNeeded(int column)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetDepartingStationCellValueNeeded(int)");
            Logger.DebugFormat("column:[{0}]", column);

            // 結果設定
            string result = m_DrawTrainProperties[column].DepartingStation;

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetDepartingStationCellValueNeeded(int)");

            // 返却
            return result;
        }

        /// <summary>
        /// 終着駅取得
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private string GetDestinationStationCellValueNeeded(int column)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetDestinationStationCellValueNeeded(int)");
            Logger.DebugFormat("column:[{0}]", column);

            // 結果設定
            string result = m_DrawTrainProperties[column].DestinationStation;

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetDestinationStationCellValueNeeded(int)");

            // 返却
            return result;
        }

        /// <summary>
        /// 列車時刻取得
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private string GetStationTimeCellValueNeeded(int column, int row)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetStationTimeCellValueNeeded(int, int)");
            Logger.DebugFormat("column:[{0}]", column);
            Logger.DebugFormat("row   :[{0}]", row);

            // 結果初期化
            string result = string.Empty;

            // StationDrawRowInfomationオブジェクト取得
            StationDrawRowInfomation stationDrawRowInfomation = m_StationDrawRowInfomation[row];

            // StationPropertyオブジェクト取得
            StationProperty stationProperty = m_RouteFileProperty.Stations.Find(s => s.Name == stationDrawRowInfomation.Name);

            // TrainPropertyオブジェクト取得
            TrainProperty trainProperty = m_DrawTrainProperties[column];

            // 駅時刻取得
            var stationTimes = GetStationTime(m_DirectionType, trainProperty, stationProperty);

            // 結果設定
            result = stationTimes[stationDrawRowInfomation.DepartureArrivalType];

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetStationTimeCellValueNeeded(int, int)");

            // 返却
            return result;
        }

        /// <summary>
        /// 備考取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetRemarkCellValueNeeded(int column)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetRemarkCellValueNeeded(int)");
            Logger.DebugFormat("column:[{0}]", column);

            // 結果設定
            string result = m_DrawTrainProperties[column].Remarks;

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetRemarkCellValueNeeded(int)");

            // 返却
            return result;
        }
        #endregion

        #region 描写情報取得
        /// <summary>
        /// 駅時刻行取得
        /// </summary>
        /// <param name="inbound"></param>
        /// <param name="station"></param>
        /// <param name="sequences"></param>
        /// <param name="trains"></param>
        /// <returns></returns>
        private Dictionary<DepartureArrivalType, List<string>> GetStationTimeRows(DirectionType type, StationProperty station)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetStationTimeRows(DirectionType, StationProperty)");
            Logger.DebugFormat("type     :[{0}]", type);
            Logger.DebugFormat("station  :[{0}]", station);

            // 結果オブジェクト生成
            Dictionary<DepartureArrivalType, List<string>> result = ColumnsListInitialization(type, station);

            // 列車描写プロパティを繰り返す
            foreach (TrainProperty train in m_DrawTrainProperties)
            {
                // 列車時刻を取得
                Dictionary<DepartureArrivalType, string> trainTimes = GetStationTime(type, train, station);

                // 行追加
                foreach (var trainTime in trainTimes)
                {
                    // 登録
                    result[trainTime.Key].Add(trainTime.Value);
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetStationTimeRows(DirectionType, StationProperty)");

            // 返却
            return result;
        }

        /// <summary>
        /// 駅間距離文字列取得
        /// </summary>
        /// <param name="outbound"></param>
        /// <param name="stations"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        private string GetDistanceStringBetweenStations(DirectionType type, StationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetDistanceStringBetweenStations(DirectionType, int, StationProperties)");
            Logger.DebugFormat("type      :[{0}]", type);
            Logger.DebugFormat("properties:[{0}]", property);

            // 結果オブジェクト設定
            float result = property.StationDistanceFromReferenceStations[type];

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetDistanceStringBetweenStations(DirectionType, int, StationProperties)");

            // 返却(文字列変換)
            return string.Format("{0:#,0.0}", result);
        }

        /// <summary>
        /// 駅時刻取得
        /// </summary>
        /// <param name="type"></param>
        /// <param name="train"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        private Dictionary<DepartureArrivalType, string> GetStationTime(DirectionType type, TrainProperty train, StationProperty station)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetStationTime(DirectionType, TrainProperty, StationProperty, int)");
            Logger.DebugFormat("type   :[{0}]", type);
            Logger.DebugFormat("train  :[{0}]", train);
            Logger.DebugFormat("station:[{0}]", station);

            // 結果オブジェクト生成
            Dictionary<DepartureArrivalType, string> result = null;

            // StationTimePropertyオブジェクト取得
            StationTimeProperty stationTime = train.StationTimes.Find(t => t.StationName == station.Name);

            // 駅扱いで分岐
            switch (stationTime.StationTreatment)
            {
                // 「停車」の場合
                case StationTreatment.Stop:
                    // 「停車時刻」を設定
                    result = GetStationStopTime(type, train, station, stationTime);
                    break;
                // 「通過」の場合
                case StationTreatment.Passing:
                    // 「通過」を設定
                    result = GetStationTime(type, train, station, m_DiaProFont["通過①"]);
                    break;
                // 「経由なし」の場合
                case StationTreatment.NoRoute:
                    // 「他線区経由」を設定
                    result = GetStationTime(type, train, station, m_DiaProFont["他線区経由"]);
                    break;
                // 「運行なし」の場合
                case StationTreatment.NoService:
                    // 駅規模判定
                    switch (station.TimeFormat)
                    {
                        case TimeFormat.DepartureAndArrival:
                        case TimeFormat.OutboundArrivalAndDeparture:
                        case TimeFormat.InboundDepartureAndArrival:
                            // 「時刻無し」を設定
                            result = GetStationTime(type, train, station, m_DiaProFont["時刻無し②"]);
                            break;
                        default:
                            // 駅規模判定
                            if (station.StationScale != StationScale.GeneralStation)
                            {
                                // 始発駅、終着駅か？
                                if (station.StartingStation || station.TerminalStation)
                                {
                                    // 「時刻無し」を設定
                                    result = GetStationTime(type, train, station, m_DiaProFont["時刻無し②"]);
                                }
                                else
                                {
                                    // 「基準線」を設定
                                    result = GetStationTime(type, train, station, m_DiaProFont["照準線①"]);
                                }
                            }
                            else
                            {
                                // 「時刻無し」を設定
                                result = GetStationTime(type, train, station, m_DiaProFont["時刻無し②"]);
                            }
                            break;
                    }
                    break;
                // 「上記以外」の場合
                default:
                    // 例外
                    throw new AggregateException(string.Format("駅扱いの異常を検出しました:[{0}]", stationTime.StationTreatment));
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetStationTime(DirectionType, TrainProperty, StationProperty, int)");

            // 返却
            return result;
        }

        /// <summary>
        /// 駅時刻取得(停車時刻)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="train"></param>
        /// <param name="station"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        /// <exception cref="AggregateException"></exception>
        private Dictionary<DepartureArrivalType, string> GetStationStopTime(DirectionType type, TrainProperty train, StationProperty station, StationTimeProperty time)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetStationTime(DirectionType, TrainProperty, StationProperty, StationTimeProperty)");
            Logger.DebugFormat("type   :[{0}]", type);
            Logger.DebugFormat("train  :[{0}]", train);
            Logger.DebugFormat("station:[{0}]", station);
            Logger.DebugFormat("time   :[{0}]", time);

            // 結果オブジェクト生成
            Dictionary<DepartureArrivalType, string> result = new Dictionary<DepartureArrivalType, string>();

            // 方向種別で分岐
            switch (type)
            {
                case DirectionType.Outbound:
                    // 時刻形式で分岐
                    switch (station.TimeFormat)
                    {
                        case TimeFormat.DepartureTime:
                            // 設定
                            if (time.DepartureTime != string.Empty)
                            {
                                result.Add(DepartureArrivalType.Departure, GetStationTimeValue(station, time.DepartureTime));
                            }
                            else
                            {
                                result.Add(DepartureArrivalType.Departure, GetStationTimeValue(station, time.ArrivalTime));
                            }
                            break;
                        case TimeFormat.DepartureAndArrival:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, GetStationTimeValue(station, time.ArrivalTime));
                            result.Add(DepartureArrivalType.Departure, GetStationTimeValue(station, time.DepartureTime));
                            break;
                        case TimeFormat.OutboundArrivalTime:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, GetStationTimeValue(station, time.ArrivalTime));
                            break;
                        case TimeFormat.InboundArrivalTime:
                            // 設定
                            result.Add(DepartureArrivalType.Departure, GetStationTimeValue(station, time.DepartureTime));
                            break;
                        case TimeFormat.OutboundArrivalAndDeparture:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, GetStationTimeValue(station, time.ArrivalTime));
                            result.Add(DepartureArrivalType.Departure, GetStationTimeValue(station, time.DepartureTime));
                            break;
                        case TimeFormat.InboundDepartureAndArrival:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, GetStationTimeValue(station, time.ArrivalTime));
                            result.Add(DepartureArrivalType.Departure, GetStationTimeValue(station, time.DepartureTime));
                            break;
                        default:
                            throw new AggregateException(string.Format("時刻形式の異常を検出しました:[{0}]", station.TimeFormat));
                    }
                    break;
                case DirectionType.Inbound:
                    // 時刻形式で分岐
                    switch (station.TimeFormat)
                    {
                        case TimeFormat.DepartureTime:
                            // 設定
                            result.Add(DepartureArrivalType.Departure, GetStationTimeValue(station, time.DepartureTime));
                            break;
                        case TimeFormat.DepartureAndArrival:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, GetStationTimeValue(station, time.ArrivalTime));
                            result.Add(DepartureArrivalType.Departure, GetStationTimeValue(station, time.DepartureTime));
                            break;
                        case TimeFormat.OutboundArrivalTime:
                            // 設定
                            result.Add(DepartureArrivalType.Departure, GetStationTimeValue(station, time.DepartureTime));
                            break;
                        case TimeFormat.InboundArrivalTime:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, GetStationTimeValue(station, time.ArrivalTime));
                            break;
                        case TimeFormat.OutboundArrivalAndDeparture:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, GetStationTimeValue(station, time.ArrivalTime));
                            result.Add(DepartureArrivalType.Departure, GetStationTimeValue(station, time.DepartureTime));
                            break;
                        case TimeFormat.InboundDepartureAndArrival:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, GetStationTimeValue(station, time.ArrivalTime));
                            result.Add(DepartureArrivalType.Departure, GetStationTimeValue(station, time.DepartureTime));
                            break;
                        default:
                            throw new AggregateException(string.Format("時刻形式の異常を検出しました:[{0}]", station.TimeFormat));
                    }
                    break;
                default:
                    throw new AggregateException(string.Format("方向種別の異常を検出しました:[{0}]", type));
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetStationTime(DirectionType, TrainProperty, StationProperty, StationTimeProperty)");

            // 返却
            return result;
        }

        /// <summary>
        /// 駅時刻文字列取得
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetStationTimeValue(StationProperty station, string value)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetStationTimeValue(StationProperty, string)");
            Logger.DebugFormat("station:[{0}]", station);
            Logger.DebugFormat("value  :[{0}]", value);

            // 結果設定
            string result = value;

            // 文字判定
            if (value == string.Empty)
            {
                // 駅規模判定
                switch (station.TimeFormat)
                {
                    case TimeFormat.DepartureAndArrival:
                    case TimeFormat.OutboundArrivalAndDeparture:
                    case TimeFormat.InboundDepartureAndArrival:
                        // 「時刻無し」を設定
                        result = m_DiaProFont["時刻無し②"];
                        break;
                    default:
                        // 駅規模判定
                        if (station.StationScale != StationScale.GeneralStation)
                        {
                            // 始発駅、終着駅か？
                            if (station.StartingStation || station.TerminalStation)
                            {
                                // 「時刻無し」を設定
                                result = m_DiaProFont["時刻無し②"];
                            }
                            else
                            {
                                // 「基準線」を設定
                                result = m_DiaProFont["照準線①"];
                            }
                        }
                        else
                        {
                            // 「時刻無し」を設定
                            result = m_DiaProFont["時刻無し②"];
                        }
                        break;
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetStationTimeValue(StationProperty, string)");

            // 返却
            return result;
        }

        /// <summary>
        /// 停車時刻取得
        /// </summary>
        /// <param name="type"></param>
        /// <param name="train"></param>
        /// <param name="station"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="AggregateException"></exception>
        private Dictionary<DepartureArrivalType, string> GetStationTime(DirectionType type, TrainProperty train, StationProperty station, string value)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetStationTime(DirectionType, TrainProperty, StationProperty, string)");
            Logger.DebugFormat("type   :[{0}]", type);
            Logger.DebugFormat("train  :[{0}]", train);
            Logger.DebugFormat("station:[{0}]", station);
            Logger.DebugFormat("value  :[{0}]", value);

            // 結果オブジェクト生成
            Dictionary<DepartureArrivalType, string> result = new Dictionary<DepartureArrivalType, string>();

            // 方向種別で分岐
            switch (type)
            {
                case DirectionType.Outbound:
                    // 時刻形式で分岐
                    switch (station.TimeFormat)
                    {
                        case TimeFormat.DepartureTime:
                            // 設定
                            result.Add(DepartureArrivalType.Departure, value);
                            break;
                        case TimeFormat.DepartureAndArrival:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, value);
                            result.Add(DepartureArrivalType.Departure, value);
                            break;
                        case TimeFormat.OutboundArrivalTime:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, value);
                            break;
                        case TimeFormat.InboundArrivalTime:
                            // 設定
                            result.Add(DepartureArrivalType.Departure, value);
                            break;
                        case TimeFormat.OutboundArrivalAndDeparture:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, value);
                            result.Add(DepartureArrivalType.Departure, value);
                            break;
                        case TimeFormat.InboundDepartureAndArrival:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, value);
                            result.Add(DepartureArrivalType.Departure, value);
                            break;
                        default:
                            throw new AggregateException(string.Format("時刻形式の異常を検出しました:[{0}]", station.TimeFormat));
                    }
                    break;
                case DirectionType.Inbound:
                    // 時刻形式で分岐
                    switch (station.TimeFormat)
                    {
                        case TimeFormat.DepartureTime:
                            // 設定
                            result.Add(DepartureArrivalType.Departure, value);
                            break;
                        case TimeFormat.DepartureAndArrival:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, value);
                            result.Add(DepartureArrivalType.Departure, value);
                            break;
                        case TimeFormat.OutboundArrivalTime:
                            // 設定
                            result.Add(DepartureArrivalType.Departure, value);
                            break;
                        case TimeFormat.InboundArrivalTime:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, value);
                            break;
                        case TimeFormat.OutboundArrivalAndDeparture:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, value);
                            result.Add(DepartureArrivalType.Departure, value);
                            break;
                        case TimeFormat.InboundDepartureAndArrival:
                            // 設定
                            result.Add(DepartureArrivalType.Arrival, value);
                            result.Add(DepartureArrivalType.Departure, value);
                            break;
                        default:
                            throw new AggregateException(string.Format("時刻形式の異常を検出しました:[{0}]", station.TimeFormat));
                    }
                    break;
                default:
                    throw new AggregateException(string.Format("方向種別の異常を検出しました:[{0}]", type));
            }


            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetStationTime(DirectionType, TrainProperty, StationProperty, string)");

            // 返却
            return result;
        }
        #endregion

        #region 列車情報編集
        /// <summary>
        /// 列車情報編集
        /// </summary>
        /// <param name="info"></param>
        private void EditTrainInformation(HitTestInfo info)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::EditTrainInformation(HitTestInfo)");
            Logger.DebugFormat("info:[{0}]", info);

            // 列車情報編集
            EditTrainInformation(info.ColumnIndex, info.RowIndex);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::EditTrainInformation(HitTestInfo)");
        }

        /// <summary>
        /// 列車情報編集
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        private void EditTrainInformation(int column, int row)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::EditTrainInformation(int, int)");
            Logger.DebugFormat("column:[{0}]", column);
            Logger.DebugFormat("row   :[{0}]", row);

            // 列車情報を取得
            TrainProperty property = m_RouteFileProperty.Diagrams.Find(t => t.Name == m_DiagramName).Trains[m_DirectionType][column - 4];

            // 編集
            Edit(property);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::EditTrainInformation(int, int)");
        }
        #endregion

        #region 列車時刻編集
        /// <summary>
        /// 列車時刻編集
        /// </summary>
        /// <param name="info"></param>
        private void EditTrainTimeInformation(HitTestInfo info)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::EditTrainTimeInformation(HitTestInfo)");
            Logger.DebugFormat("info:[{0}]", info);

            // 列車時刻編集
            EditTrainTimeInformation(info.ColumnIndex, info.RowIndex);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::EditTrainTimeInformation(HitTestInfo)");
        }

        /// <summary>
        /// 列車時刻編集
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        private void EditTrainTimeInformation(int column, int row)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::EditTrainTimeInformation(int, int)");
            Logger.DebugFormat("column:[{0}]", column);
            Logger.DebugFormat("row   :[{0}]", row);

            // 列車時刻情報を取得
            StationTimeProperty property = GetTargetTrainStationTime(new DataGridLocation() { Row = row, Column = column });

            // FormStationTimePropertyオブジェクト生成
            FormStationTimeProperty form = new FormStationTimeProperty(m_RouteFileProperty, property);

            // FormStationTimeProperty表示
            DialogResult dialogResult = form.ShowDialog();

            // FormStationTimeProperty表示結果判定
            if (dialogResult == DialogResult.OK)
            {
                // 結果比較
                if (!property.Compare(form.Property))
                {
                    // 結果保存
                    property.Copy(form.Property);

                    // 更新通知
                    OnStationTimePropertyUpdate(this, new StationTimePropertyUpdateEventArgs() { Property = property });
                }
            }

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::EditTrainTimeInformation(int, int)");
        }
        #endregion

        #region 駅名取得
        /// <summary>
        /// 駅名取得
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private string GetSelectedStationName(int row)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::GetSelectedStationName(HitintTestInfo)");
            Logger.DebugFormat("row:[{0}]", row);

            // 初期化
            int stationMaxIndex = 6 + m_StationDrawRowInfomation.Count;

            // 判定
            if (!(row >= 6 && row < stationMaxIndex))
            {
                // ロギング
                Logger.WarnFormat("result:[駅名なし][{0}]", row);
                Logger.Warn("<<<<= VirtualDataGridViewTimeTable::GetSelectedStationName(HitintTestInfo)");

                // 駅名なし
                return "";
            }

            // 駅名セルを取得
            DataGridViewCell stationCell = this[2, row];

            // 結果設定
            string result = stationCell.Value.ToString();

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::GetSelectedStationName(HitintTestInfo)");

            // 返却
            return result;
        }
        #endregion

        #region 駅情報編集
        /// <summary>
        /// 駅情報編集
        /// </summary>
        /// <param name="info"></param>
        private void EditStationInformation(HitTestInfo info)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::EditStationInformation(HitTestInfo)");
            Logger.DebugFormat("info:[{0}]", info);

            // 駅情報編集
            EditStationInformation(info.ColumnIndex, info.RowIndex);

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::EditStationInformation(HitTestInfo)");
        }

        /// <summary>
        /// 駅情報編集
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        private void EditStationInformation(int column, int row)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::EditStationInformation(int, int)");
            Logger.DebugFormat("column:[{0}]", column);
            Logger.DebugFormat("row   :[{0}]", row);

            // 駅名取得
            string stationName = GetSelectedStationName(row);

            // 駅情報を取得
            StationProperty property = m_RouteFileProperty.GetStation(stationName);

            // 駅情報判定
            if (property != null)
            {
                // StationPropertiesUpdateEventArgsオブジェクト生成
                StationPropertiesUpdateEventArgs eventArgs = new StationPropertiesUpdateEventArgs();
                eventArgs.OldStationName = property.Name;
                eventArgs.OldProperties.Copy(m_RouteFileProperty.Stations);
                eventArgs.Properties = m_RouteFileProperty.Stations;

                // FormStationPropertyオブジェクト生成
                FormStationProperty form = new FormStationProperty(property);

                // FormStationProperty表示
                DialogResult dialogResult = form.ShowDialog();

                // FormStationProperty表示結果判定
                if (dialogResult == DialogResult.OK)
                {
                    // StationPropertiesUpdateEventArgsオブジェクト設定
                    eventArgs.NewStationName = form.Property.Name;

                    // 駅名変換
                    m_RouteFileProperty.ChangeStationName(eventArgs.OldStationName, form.Property.Name);

                    // 結果保存
                    property.Copy(form.Property);

                    // 変更されたか？
                    if (!eventArgs.OldProperties.Compare(m_RouteFileProperty.Stations))
                    {
                        // 更新通知
                        OnStationPropertiesUpdate(this, eventArgs);
                    }
                }
            }
            else
            {
                // ロギング
                Logger.WarnFormat("選択対象駅が存在していません：[{0}]", stationName);
                Logger.Warn(m_RouteFileProperty.Stations.ToString());
            }

            // ロギング
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::EditStationInformation(int, int)");
        }
        #endregion

        #region 列車時刻編集対象StationTimePropertyオブジェクト取得
        /// <summary>
        /// 列車時刻編集対象StationTimePropertyオブジェクト取得
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private StationTimeProperty GetTargetTrainStationTime(DataGridLocation location)
        {
            // ロギング
            Logger.Debug("=>>>> VirtualDataGridViewTimeTable::EditTrainTimeInformation(DataGridLocation)");
            Logger.DebugFormat("location:[{0}]", location);

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.GetDiagram(m_DiagramName);

            // 駅名取得
            string stationName = GetSelectedStationName(location.Row);

            // 駅情報を取得
            StationProperty stationProperty = m_RouteFileProperty.GetStation(stationName);

            // 列車情報を取得
            TrainProperty trainProperty = diagramProperty.Trains[m_DirectionType][location.Column - 4];

            // 列車時刻情報を取得
            StationTimeProperty result = trainProperty.StationTimes.Find(s => s.StationName == stationProperty.Name);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= VirtualDataGridViewTimeTable::EditTrainTimeInformation(DataGridLocation)");

            // 返却
            return result;
        }
        #endregion
        #endregion
    }
}
