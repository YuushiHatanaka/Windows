using log4net;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using System.Xml.Linq;
using TrainTimeTable.Common;
using TrainTimeTable.Dialog;
using TrainTimeTable.EventArgs;
using TrainTimeTable.Property;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// DataGridViewTimetableラス
    /// </summary>
    public class DataGridViewTimetable : DataGridView
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
        /// 更新 event(駅時刻情報)
        /// </summary>
        public event StationTimePropertyHandler OnStationTimePropertyUpdate = delegate { };

        /// <summary>
        /// 更新 event(駅情報)
        /// </summary>
        public event StationPropertiesUpdateEventHandler OnStationPropertiesUpdate = delegate { };
        #endregion

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

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="text"></param>
        /// <param name="type"></param>
        /// <param name="property"></param>
        public DataGridViewTimetable(string text, DirectionType type, RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::DataGridViewTimetable(string, DirectionType, RouteFileProperty)");
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

            // Font設定
            Font = m_FontDictionary["時刻表ビュー"];

            // ダイヤグラムID取得
            m_DiagramId = m_RouteFileProperty.Diagrams.GetIndex(m_DiagramName);

            // 初期化
            Initialization();

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::DataGridViewTimetable(string, DirectionType, RouteFileProperty)");
        }
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::Initialization()");

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

            // イベント設定
            ColumnAdded += DataGridViewTimetable_ColumnAdded;
            CellPainting += DataGridViewTimetable_CellPainting;
            CellFormatting += DataGridViewTimetable_CellFormatting;
            MouseDoubleClick += DataGridViewTimetable_MouseDoubleClick;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::Initialization()");
        }
        #endregion

        #region 描画
        /// <summary>
        /// 描画
        /// </summary>
        public void Draw()
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::Draw()");

            // 描画一時停止
            SuspendLayout();

            // 列、行削除
            RowCount = 0;
            ColumnCount = 0;

            // 呼び出しAction一覧
            List<Action<TrainProperties>> actions = new List<Action<TrainProperties>>()
            {
                DrawColumns,                // 列描画
                DrawRowsTrainNumber,        // 列車番号描画
                DrawRowsTrainType,          // 列車種別描画
                DrawRowsTrainName,          // 列車名描画
                DrawRowsTrainMark,          // 列車記号描画
                DrawRowsDepartingStation,   // 始発駅描画
                DrawRowsDestinationStation, // 終着駅描画
                DrawRowsStationTime,        // 駅時刻描画
                DrawRowsRemarksStation,     // 備考初描画
            };

            // TrainPropertiesオブジェクト取得
            TrainProperties trains = m_RouteFileProperty.Diagrams[m_DiagramId].Trains[m_DirectionType];

            // Actionを繰り返す
            foreach (Action<TrainProperties> action in actions)
            {
                // Action実行
                action(trains);
            }

            // 描画再開
            ResumeLayout();

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::Draw()");
        }

        /// <summary>
        /// 列描画
        /// </summary>
        /// <param name="trains"></param>
        private void DrawColumns(TrainProperties trains)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::DrawColumns(TrainProperties)");
            Logger.DebugFormat("trains:[{0}]", trains);

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

            // 列車プロパティを繰り返す
            foreach (var train in trains)
            {
                // 列追加
                Columns.Add(string.Format("Train{0}", train.Id), string.Format("{0}", train.No));
                Columns[Columns.Count - 1].Width = 8 * m_RouteFileProperty.Route.TimetableTrainWidth;  // 列車
                Columns[Columns.Count - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Columns[Columns.Count - 1].DefaultCellStyle.ForeColor = m_RouteFileProperty.TrainTypes[train.TrainType].StringsColor;
            }

            // 固定列(発着)設定
            Columns["ArrivalAndDeparture"].Frozen = true;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::DrawColumns(TrainProperties)");
        }

        /// <summary>
        /// 列車番号描画
        /// </summary>
        /// <param name="properties"></param>
        private void DrawRowsTrainNumber(TrainProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::DrawRowsTrainNumber(TrainProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // List初期化
            List<string> columnsList = ColumnsListInitialization("列車番号");

            // 列車番号一覧を取得
            List<string> numberList = properties.Select(t => t.No).ToList();

            // 列車番号一覧を追加
            columnsList.AddRange(numberList);

            // 行追加
            Rows.Add(columnsList.ToArray());

            // フォント設定
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_FontDictionary["列車番号"];
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.BackColor = SystemColors.Control;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::DrawRowsTrainNumber(TrainProperties)");
        }

        /// <summary>
        /// 列車種別描画
        /// </summary>
        /// <param name="properties"></param>
        private void DrawRowsTrainType(TrainProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::DrawRowsTrainType(TrainProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // List初期化
            List<string> columnsList = ColumnsListInitialization("列車種別");

            // 列車プロパティを繰り返す
            foreach (var train in properties)
            {
                // 追加
                columnsList.Add(string.Format("{0}", m_DiaProFont[m_RouteFileProperty.TrainTypes[train.TrainType].Name]));
            }

            // 行追加
            Rows.Add(columnsList.ToArray());

            // フォント設定
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_FontDictionary["列車種別"];
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.BackColor = SystemColors.Control;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::DrawRowsTrainType(TrainProperties)");
        }

        /// <summary>
        /// 列車名描画
        /// </summary>
        /// <param name="properties"></param>
        private void DrawRowsTrainName(TrainProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::DrawRowsTrainName(TrainProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // List初期化
            List<string> columnsList = ColumnsListInitialization("列車名");

            // 列車プロパティを繰り返す
            foreach (var train in properties)
            {
                // 追加
                StringBuilder trainName = new StringBuilder();
                trainName.Append(train.Name);
                if (train.Number != string.Empty)
                {
                    trainName.Append(string.Format("{0}号", train.Number));
                }
                columnsList.Add(StringLibrary.VerticalText(trainName.ToString()));
            }

            // 行追加
            Rows.Add(columnsList.ToArray());

            // フォント設定
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_FontDictionary["列車名"];
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.BackColor = SystemColors.Control;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::DrawRowsTrainName(TrainProperties)");
        }

        /// <summary>
        /// 列車記号描画
        /// </summary>
        /// <param name="properties"></param>
        private void DrawRowsTrainMark(TrainProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::DrawRowsTrainMark(TrainProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // List初期化
            List<string> columnsList = ColumnsListInitialization("");

            // 列車プロパティを繰り返す
            foreach (var train in properties)
            {
                // 列車記号文字列
                StringBuilder sb = new StringBuilder();

                // 記号分繰り返す
                foreach (var mark in train.Marks)
                {
                    // 追加
                    sb.AppendLine(string.Format("{0}", m_DiaProFont[mark.MarkName]));
                }

                // 追加
                columnsList.Add(sb.ToString().TrimEnd(new char[] { '\r', '\n' }));
            }

            // 行追加
            Rows.Add(columnsList.ToArray());

            // フォント設定
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_FontDictionary["列車記号"];
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.BackColor = SystemColors.Control;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::DrawRowsTrainMark(TrainProperties)");
        }

        /// <summary>
        /// 始発駅描画
        /// </summary>
        /// <param name="properties"></param>
        private void DrawRowsDepartingStation(TrainProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::DrawRowsDepartingStation(TrainProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // List初期化
            List<string> columnsList = ColumnsListInitialization("始発駅");

            // 始発駅一覧を取得
            List<string> departingStationList = properties.Select(t => t.DepartingStation).ToList();

            // 始発駅一覧を登録
            columnsList.AddRange(departingStationList);

            // 行追加
            Rows.Add(columnsList.ToArray());

            // フォント設定
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_FontDictionary["始発駅"];
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.BackColor = SystemColors.Control;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::DrawRowsDepartingStation(TrainProperties)");
        }

        /// <summary>
        /// 終着駅描画
        /// </summary>
        /// <param name="properties"></param>
        private void DrawRowsDestinationStation(TrainProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::DrawRowsDestinationStation(TrainProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // List初期化
            List<string> columnsList = ColumnsListInitialization("終着駅");

            // 終着駅一覧を取得
            List<string> destinationStationList = properties.Select(t => t.DestinationStation).ToList();

            // 終着駅一覧を登録
            columnsList.AddRange(destinationStationList);

            // 行追加
            Rows.Add(columnsList.ToArray());

            // フォント設定
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_FontDictionary["終着駅"];
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.BackColor = SystemColors.Control;
            Rows[Rows.Count - 1].Frozen = true;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::DrawRowsDestinationStation(TrainProperties)");
        }

        /// <summary>
        /// 駅時刻描画
        /// </summary>
        /// <param name="properties"></param>
        /// <exception cref="AggregateException"></exception>
        private void DrawRowsStationTime(TrainProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::DrawRowsStationTime(TrainProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 方向種別で分岐
            switch (m_DirectionType)
            {
                case DirectionType.Outbound:
                    // 駅時刻描画(下り)
                    DrawRowsOutboundStationTime(properties);
                    break;
                case DirectionType.Inbound:
                    // 駅時刻描画(上り)
                    DrawRowsInboundStationTime(properties);
                    break;
                default:
                    throw new AggregateException(string.Format("方向種別の異常を検出しました:[{0}]", m_DirectionType));
            }

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::DrawRowsStationTime(TrainProperties)");
        }

        /// <summary>
        /// 駅時刻描画(下り)
        /// </summary>
        /// <param name="properties"></param>
        private void DrawRowsOutboundStationTime(TrainProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::DrawRowsOutboundStationTime(TrainProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 駅シーケンスリスト取得(昇順)
            var stationSequences = m_RouteFileProperty.StationSequences.OrderBy(t => t.Seq);

            // 駅を繰り返す
            foreach (var stationSequence in stationSequences)
            {
                // 駅情報取得
                StationProperty station = m_RouteFileProperty.Stations.Find(t => t.Name == stationSequence.Name);

                // 駅時刻行取得
                Dictionary<DepartureArrivalType, List<string>> columnsList = GetStationTimeRows(DirectionType.Outbound, station, properties);

                // 行追加
                foreach (DepartureArrivalType type in columnsList.Keys)
                {
                    // 駅距離を設定
                    columnsList[type][0] = GetDistanceStringBetweenStations(DirectionType.Outbound, station);

                    // 行追加
                    Rows.Add(columnsList[type].ToArray());
                }
            }

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::DrawRowsOutboundStationTime(TrainProperties)");
        }

        /// <summary>
        /// 駅時刻描画(上り)
        /// </summary>
        /// <param name="properties"></param>
        private void DrawRowsInboundStationTime(TrainProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::DrawRowsInboundStationTime(TrainProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 駅シーケンスリスト取得(降順)
            var stationSequences = m_RouteFileProperty.StationSequences.OrderByDescending(t => t.Seq);

            // 駅を繰り返す
            foreach (var stationSequence in stationSequences)
            {
                // 駅情報取得
                StationProperty station = m_RouteFileProperty.Stations.Find(t => t.Name == stationSequence.Name);

                // 駅時刻行取得
                Dictionary<DepartureArrivalType, List<string>> columnsList = GetStationTimeRows(DirectionType.Inbound, station, properties);

                // 行追加
                foreach (DepartureArrivalType type in columnsList.Keys)
                {
                    // 駅距離を設定
                    columnsList[type][0] = GetDistanceStringBetweenStations(DirectionType.Inbound, station);

                    // 行追加
                    Rows.Add(columnsList[type].ToArray());
                }
            }

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::DrawRowsInboundStationTime(TrainProperties)");
        }

        /// <summary>
        /// 備考描画
        /// </summary>
        /// <param name="property"></param>
        private void DrawRowsRemarksStation(TrainProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::DrawRowsRemarksStation(TrainProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // List初期化
            List<string> columnsList = ColumnsListInitialization("備考");

            // 備考一覧を取得
            List<string> remarksList = properties.Select(t => t.Remarks).ToList();

            // 終着駅一覧を登録
            columnsList.AddRange(remarksList);

            // 行追加
            Rows.Add(columnsList.ToArray());

            // TODO:フォント設定(暫定)
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_FontDictionary["備考"];

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::DrawRowsRemarksStation(TrainProperties)");
        }
        #endregion

        #region イベント
        #region DataGridViewTimetableイベント
        /// <summary>
        /// DataGridViewTimetable_ColumnAdded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewTimetable_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::DataGridViewTimetable_ColumnAdded(object, DataGridViewColumnEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 設定
            e.Column.FillWeight = 1;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::DataGridViewTimetable_ColumnAdded(object, DataGridViewColumnEventArgs)");
        }

        /// <summary>
        /// DataGridViewTimetable_CellFormatting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void DataGridViewTimetable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::DataGridViewTimetable_CellFormatting(object, DataGridViewCellFormattingEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 4カラム目(駅名)以外は処理しない
            if ((e.ColumnIndex == 2) && (e.RowIndex > 0))
            {
                // 前カラムと値が同じか判定
                if (IsTheSameCellValue(e.ColumnIndex, e.RowIndex))
                {
                    e.Value = "";
                    e.FormattingApplied = true; // 以降の書式設定は不要
                }
            }

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::DataGridViewTimetable_CellFormatting(object, DataGridViewCellFormattingEventArgs)");
        }

        /// <summary>
        /// DataGridViewTimetable_CellPainting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void DataGridViewTimetable_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::DataGridViewTimetable_ColumnAdded(object, DataGridViewColumnEventArgs)");
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
                    if (!IsTheSameCellValue(e.ColumnIndex, e.RowIndex))
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
            Logger.Debug("<<<<= DataGridViewTimetable::DataGridViewTimetable_ColumnAdded(object, DataGridViewColumnEventArgs)");
        }

        /// <summary>
        /// DataGridViewTimetable_MouseDoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewTimetable_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::DataGridViewTimetable_MouseDoubleClick(object, MouseEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // ダブルクリックされたセルの位置を取得
            HitTestInfo hitTestInfo = ((DataGridViewTimetable)sender).HitTest(e.X, e.Y);

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
            Logger.Debug("<<<<= DataGridViewTimetable::DataGridViewTimetable_MouseDoubleClick(object, MouseEventArgs)");
        }
        #endregion
        #endregion

        #region publicメソッド
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        public void Update(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::Update(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 旧データと同一か？
            if (m_OldRouteFileProperty.Compare(property))
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewTimetable::Update(RouteFileProperty)");

                // 何もしない
                return;
            }

            // 描画
            Draw();

            // 旧データ更新
            m_OldRouteFileProperty.Copy(property);

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::Update(RouteFileProperty)");
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// カラムリスト初期化
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<string> ColumnsListInitialization(string name)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::ColumnsListInitialization(string)");
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
            Logger.Debug("<<<<= DataGridViewTimetable::ColumnsListInitialization(string)");

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
            Logger.Debug("=>>>> DataGridViewTimetable::ColumnsListInitialization(string, string)");
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
            Logger.Debug("<<<<= DataGridViewTimetable::ColumnsListInitialization(string, string)");

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
            Logger.Debug("=>>>> DataGridViewTimetable::ColumnsListInitialization(DirectionType, StationProperty)");
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
            Logger.Debug("<<<<= DataGridViewTimetable::ColumnsListInitialization(DirectionType, StationProperty)");

            // 返却
            return result;
        }

        /// <summary>
        /// 駅時刻行取得
        /// </summary>
        /// <param name="inbound"></param>
        /// <param name="station"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        private Dictionary<DepartureArrivalType, List<string>> GetStationTimeRows(DirectionType type, StationProperty station, TrainProperties trains)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::GetStationTimeRows(DirectionType, StationProperty, TrainProperties)");
            Logger.DebugFormat("type   :[{0}]", type);
            Logger.DebugFormat("station:[{0}]", station);
            Logger.DebugFormat("trains :[{0}]", trains);

            // 結果オブジェクト生成
            Dictionary<DepartureArrivalType, List<string>> result = ColumnsListInitialization(type, station);

            // 列車プロパティを繰り返す
            foreach (var train in trains)
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
            Logger.Debug("<<<<= DataGridViewTimetable::GetStationTimeRows(DirectionType, StationProperty, TrainProperties)");

            // 返却
            return result;
        }

        /// <summary>
        /// 駅時刻行取得
        /// </summary>
        /// <param name="property"></param>
        /// <param name="directionType"></param>
        /// <param name="index"></param>
        /// <param name="trains"></param>
        /// <returns></returns>
        private Dictionary<DepartureArrivalType, List<string>> GetStationTimeRows(DirectionType type, StationProperty station, int index, TrainProperties trains)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::GetStationTimeRows(DirectionType, StationProperty, int, TrainProperties)");
            Logger.DebugFormat("type   :[{0}]", type);
            Logger.DebugFormat("station:[{0}]", station);
            Logger.DebugFormat("index  :[{0}]", index);
            Logger.DebugFormat("trains :[{0}]", trains);

            // 結果オブジェクト生成
            Dictionary<DepartureArrivalType, List<string>> result = ColumnsListInitialization(type, station);

            // 列車プロパティを繰り返す
            foreach (var train in trains)
            {
                // 列車時刻を取得
                Dictionary<DepartureArrivalType, string> trainTimes = GetStationTime(type, train, station, index);

                // 行追加
                foreach (var trainTime in trainTimes)
                {
                    // 登録
                    result[trainTime.Key].Add(trainTime.Value);
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DataGridViewTimetable::GetStationTimeRows(DirectionType, StationProperty, int, TrainProperties)");

            // 返却
            return result;
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
            Logger.Debug("=>>>> DataGridViewTimetable::GetStationTime(DirectionType, TrainProperty, StationProperty, int)");
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
            Logger.Debug("<<<<= DataGridViewTimetable::GetStationTime(DirectionType, TrainProperty, StationProperty, int)");

            // 返却
            return result;
        }

        /// <summary>
        /// 駅時刻取得
        /// </summary>
        /// <param name="type"></param>
        /// <param name="train"></param>
        /// <param name="station"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private Dictionary<DepartureArrivalType, string> GetStationTime(DirectionType type, TrainProperty train, StationProperty station, int index)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::GetStationTime(DirectionType, TrainProperty, StationProperty, int)");
            Logger.DebugFormat("type   :[{0}]", type);
            Logger.DebugFormat("train  :[{0}]", train);
            Logger.DebugFormat("station:[{0}]", station);
            Logger.DebugFormat("index  :[{0}]", index);

            // 結果オブジェクト生成
            Dictionary<DepartureArrivalType, string> result = null;

            // StationTimePropertyオブジェクト取得
            StationTimeProperty stationTime = train.StationTimes[index];

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
            Logger.Debug("<<<<= DataGridViewTimetable::GetStationTime(DirectionType, TrainProperty, StationProperty, int)");

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
            Logger.Debug("=>>>> DataGridViewTimetable::GetStationTime(DirectionType, TrainProperty, StationProperty, StationTimeProperty)");
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
            Logger.Debug("<<<<= DataGridViewTimetable::GetStationTime(DirectionType, TrainProperty, StationProperty, StationTimeProperty)");

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
            Logger.Debug("=>>>> DataGridViewTimetable::GetStationTimeValue(StationProperty, string)");
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
            Logger.Debug("<<<<= DataGridViewTimetable::GetStationTimeValue(StationProperty, string)");

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
            Logger.Debug("=>>>> DataGridViewTimetable::GetStationTime(DirectionType, TrainProperty, StationProperty, string)");
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
            Logger.Debug("<<<<= DataGridViewTimetable::GetStationTime(DirectionType, TrainProperty, StationProperty, string)");

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
            Logger.Debug("=>>>> DataGridViewTimetable::GetDistanceStringBetweenStations(DirectionType, int, StationProperties)");
            Logger.DebugFormat("type      :[{0}]", type);
            Logger.DebugFormat("properties:[{0}]", property);

            // 結果オブジェクト設定
            float result = property.StationDistanceFromReferenceStations[type];

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DataGridViewTimetable::GetDistanceStringBetweenStations(DirectionType, int, StationProperties)");

            // 返却(文字列変換)
            return string.Format("{0:#,0.0}", result);
        }

        /// <summary>
        /// 駅間距離文字列取得
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        private string GetDistanceStringBetweenStations(DirectionType type, int index, StationProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::GetDistanceStringBetweenStations(DirectionType, int, StationProperties)");
            Logger.DebugFormat("type      :[{0}]", type);
            Logger.DebugFormat("index     :[{0}]", index);
            Logger.DebugFormat("properties:[{0}]", properties);

            // 駅オブジェクト取得
            StationProperty stationProperty = properties[index];

            // 結果オブジェクト設定
            float result = stationProperty.StationDistanceFromReferenceStations[type];

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DataGridViewTimetable::GetDistanceStringBetweenStations(DirectionType, int, StationProperties)");

            // 返却(文字列変換)
            return string.Format("{0:#,0.0}", result);
        }

        /// <summary>
        /// 指定したセルと1つ上のセルの値を比較
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private bool IsTheSameCellValue(int column, int row)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::IsTheSameCellValue(int, int)");
            Logger.DebugFormat("column:[{0}]", column);
            Logger.DebugFormat("row   :[{0}]", row);

            DataGridViewCell cell1 = this[column, row];
            DataGridViewCell cell2 = this[column, row - 1];

            if (cell1.Value == null || cell2.Value == null)
            {
                // ロギング
                Logger.DebugFormat("result:[{0}]", false);
                Logger.Debug("<<<<= DataGridViewTimetable::IsTheSameCellValue(int, int)");

                // 不一致(false)を返却
                return false;
            }

            // ここでは文字列としてセルの値を比較
            if (cell1.Value.ToString() == cell2.Value.ToString())
            {
                // ロギング
                Logger.DebugFormat("result:[{0}]", true);
                Logger.Debug("<<<<= DataGridViewTimetable::IsTheSameCellValue(int, int)");

                // 一致(true)を返却
                return true;
            }
            else
            {
                // ロギング
                Logger.DebugFormat("result:[{0}]", false);
                Logger.Debug("<<<<= DataGridViewTimetable::IsTheSameCellValue(int, int)");

                // 不一致(false)を返却
                return false;
            }
        }


        /// <summary>
        /// 列車情報編集
        /// </summary>
        /// <param name="info"></param>
        private void EditTrainInformation(HitTestInfo info)
        {
            // 列車情報を取得
            TrainProperty property = m_RouteFileProperty.Diagrams[m_DiagramId].Trains[m_DirectionType][info.ColumnIndex - 4];

            // FormTrainPropertyオブジェクト生成
            FormTrainProperty form = new FormTrainProperty(property);

            // FormTrainProperty表示
            DialogResult dialogResult = form.ShowDialog();

            // FormTrainProperty表示結果判定
            if (dialogResult == DialogResult.OK)
            {
                // 結果保存
                property.Copy(form.Property);

                // 更新通知
                OnTrainPropertyUpdate(this, new TrainPropertyUpdateEventArgs() { Property = property });
            }
        }

        /// <summary>
        /// 列車時刻編集
        /// </summary>
        /// <param name="info"></param>
        private void EditTrainTimeInformation(HitTestInfo info)
        {
            // 列車情報を取得
            TrainProperty trainProperty = m_RouteFileProperty.Diagrams[m_DiagramId].Trains[m_DirectionType][info.ColumnIndex - 4];

            // 列車時刻情報を取得
            StationTimeProperty property = trainProperty.StationTimes[info.RowIndex - 6];

            // FormStationTimePropertyオブジェクト生成
            FormStationTimeProperty form = new FormStationTimeProperty(property);

            // FormStationTimeProperty表示
            DialogResult dialogResult = form.ShowDialog();

            // FormStationTimeProperty表示結果判定
            if (dialogResult == DialogResult.OK)
            {
                // 結果保存
                property.Copy(form.Property);

                // 更新通知
                OnStationTimePropertyUpdate(this, new StationTimePropertyUpdateEventArgs() { Property = property });
            }
        }

        /// <summary>
        /// 駅情報編集
        /// </summary>
        /// <param name="info"></param>
        private void EditStationInformation(HitTestInfo info)
        {
            // 駅名セルを取得
            DataGridViewCell stationCell = this[2, info.RowIndex];

            // 駅情報を取得
            StationProperty property = m_RouteFileProperty.Stations.Find(t => t.Name == stationCell.Value.ToString());

            // 駅情報判定
            if (property != null)
            {
                // StationPropertiesUpdateEventArgsオブジェクト生成
                StationPropertiesUpdateEventArgs eventArgs = new StationPropertiesUpdateEventArgs();
                eventArgs.OldProperties.Copy(m_RouteFileProperty.Stations);
                eventArgs.Properties = m_RouteFileProperty.Stations;

                // 旧駅名保存
                string oldStationName = property.Name;

                // FormStationPropertyオブジェクト生成
                FormStationProperty form = new FormStationProperty(property);

                // FormStationProperty表示
                DialogResult dialogResult = form.ShowDialog();

                // FormStationProperty表示結果判定
                if (dialogResult == DialogResult.OK)
                {
                    // 同一名判定
                    if (m_RouteFileProperty.Stations.Find(t => t.Name == form.Property.Name) != null)
                    {
                        // エラーメッセージ
                        MessageBox.Show(string.Format("既に登録されている駅名は使用できません:[{0}]", form.Property.Name), "エラー", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    // 駅名(キーが変更されたか？)
                    if (oldStationName != form.Property.Name)
                    {
                        eventArgs.OldStationName = oldStationName;
                        eventArgs.NewStationName = form.Property.Name;

                        // 駅名変換
                        m_RouteFileProperty.ChangeStationName(oldStationName, form.Property.Name);
                    }

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
                Logger.WarnFormat("選択対象駅が存在していません：[{0}]", stationCell.Value.ToString());
                Logger.Warn(m_RouteFileProperty.Stations.ToString());
            }
        }
        #endregion
    }
}
