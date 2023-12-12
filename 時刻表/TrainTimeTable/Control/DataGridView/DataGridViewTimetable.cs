using log4net;
using Microsoft.VisualBasic;
using System;
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
using System.Windows.Forms;
using System.Xml.Linq;
using TrainTimeTable.Common;
using TrainTimeTable.Dialog;
using TrainTimeTable.Property;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// DataGridViewTimetableクラス
    /// </summary>
    public class DataGridViewTimetable : DataGridView
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

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
        /// Font辞書
        /// </summary>
        private Dictionary<string, Font> m_Dictionary = null;

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
            m_Dictionary = m_RouteFileProperty.Fonts.GetFonts(
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
            Font = m_Dictionary["時刻表ビュー"];

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
            // TODO:仮想モードにする
            // VirtualMode = true;
            // NewRowNeeded += new DataGridViewRowEventHandler(dataGridView1_NewRowNeeded);
            // RowsAdded += new DataGridViewRowsAddedEventHandler(dataGridView1_RowsAdded);
            // CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView1_CellValidating);
            // CellValueNeeded += new DataGridViewCellValueEventHandler(dataGridView1_CellValueNeeded);
            // CellValuePushed += new DataGridViewCellValueEventHandler(dataGridView1_CellValuePushed);

            // ヘッダーとすべてのセルの内容に合わせて、列の幅を自動調整する
            // AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // ヘッダーとすべてのセルの内容に合わせて、行の高さを自動調整する
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // ヘッダーの色等
            EnableHeadersVisualStyles = false;     // Visualスタイルを使用しない
            ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            ColumnHeadersDefaultCellStyle.Font = m_Dictionary["時刻表ヘッダー"];
            ColumnHeadersDefaultCellStyle.Font = Font;
            ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //ヘッダ高さ
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            ColumnHeadersHeight = 28;

            // イベント設定
            ColumnAdded += DataGridViewTimetable_ColumnAdded;
            CellPainting += DataGridViewTimetable_CellPainting;
            CellFormatting += DataGridViewTimetable_CellFormatting;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::Initialization()");
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <param name="property"></param>
        private void Initialization(int index, DirectionType type, RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::DataGridViewTimetable(int, DirectionType, RouteFileProperty)");
            Logger.DebugFormat("index   :[{0}]", index);
            Logger.DebugFormat("type    :[{0}]", type);
            Logger.DebugFormat("property:[{0}]", property);

            // 列初期化
            ColumnsInitialization(index, type, property);

            // 列車番号初期化
            RowsTrainNumberInitialization(index, type, property);

            // 列車種別初期化
            RowsTrainTypeInitialization(index, type, property);

            // 列車名初期化
            RowsTrainNameInitialization(index, type, property);

            // 列車記号初期化
            RowsTrainMarkInitialization(index, type, property);

            // 始発駅初期化
            RowsDepartingStationInitialization(index, type, property);

            // 終着駅初期化
            RowsDestinationStationInitialization(index, type, property);

            // 駅時刻初期化
            RowsStationTimeInitialization(index, type, property);

            // 備考初期化
            RowsRemarksStationInitialization(index, type, property);

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::DataGridViewTimetable(int, DirectionType, RouteFileProperty)");
        }

        /// <summary>
        /// 初期化(固定カラム)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <param name="property"></param>
        private void ColumnsInitialization(int index, DirectionType type, RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::ColumnsInitialization(int, DirectionType, RouteFileProperty)");
            Logger.DebugFormat("index   :[{0}]", index);
            Logger.DebugFormat("type    :[{0}]", type);
            Logger.DebugFormat("property:[{0}]", property);

            // 固定カラム追加
            Columns.Add("Distance", "距離");
            Columns.Add("StationMark", "駅マーク");
            Columns.Add("StationName", "駅名");
            Columns.Add("ArrivalAndDeparture", "発着");

            // 固定カラム幅設定
            Columns[0].Width = 48;  // 距離
            Columns[1].Width = 32;  // 駅マーク
            Columns[2].Width = 24 * property.Route.WidthOfStationNameField;  // 駅名
            Columns[3].Width = 24;  // 発着

            // 固定カラム背景色設定
            Columns[0].DefaultCellStyle.BackColor = SystemColors.Control;
            Columns[1].DefaultCellStyle.BackColor = SystemColors.Control;

            // 固定カラムフォント設定
            Columns[0].DefaultCellStyle.Font = m_Dictionary["駅間距離"];

            // 文字位置設定
            Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // TrainPropertiesオブジェクト取得
            TrainProperties trains = property.Diagrams[index].Trains[type];

            // 列車プロパティを繰り返す
            foreach (var train in trains)
            {
                // 列追加
                Columns.Add(string.Format("Train{0}", train.Seq), string.Format("{0}", train.No));
                Columns[Columns.Count - 1].Width = 8 * property.Route.TimetableTrainWidth;  // 列車
                Columns[Columns.Count - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Columns[Columns.Count - 1].DefaultCellStyle.ForeColor = property.TrainTypes[train.TrainType].StringsColor;
            }

            // 固定列(発着)設定
            Columns["ArrivalAndDeparture"].Frozen = true;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::ColumnsInitialization(int, DirectionType, RouteFileProperty)");
        }

        /// <summary>
        /// 初期化(列車番号)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <param name="property"></param>
        private void RowsTrainNumberInitialization(int index, DirectionType type, RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::RowsTrainNumberInitialization(int, DirectionType, RouteFileProperty)");
            Logger.DebugFormat("index   :[{0}]", index);
            Logger.DebugFormat("type    :[{0}]", type);
            Logger.DebugFormat("property:[{0}]", property);

            // TrainPropertiesオブジェクト取得
            TrainProperties trains = property.Diagrams[index].Trains[type];

            // List初期化
            List<string> columnsList = ColumnsListInitialization("列車番号");

            // 列車プロパティを繰り返す
            foreach (var train in trains)
            {
                // 追加
                columnsList.Add(string.Format("{0}", train.No));
            }

            // 行追加
            Rows.Add(columnsList.ToArray());

            // フォント設定
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_Dictionary["列車番号"];
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.BackColor = SystemColors.Control;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::RowsTrainNumberInitialization(int, DirectionType, RouteFileProperty)");
        }

        /// <summary>
        /// 初期化(列車種別)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <param name="property"></param>
        private void RowsTrainTypeInitialization(int index, DirectionType type, RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::RowsTrainTypeInitialization(int, DirectionType, RouteFileProperty)");
            Logger.DebugFormat("index   :[{0}]", index);
            Logger.DebugFormat("type    :[{0}]", type);
            Logger.DebugFormat("property:[{0}]", property);

            // TrainPropertiesオブジェクト取得
            TrainProperties trains = property.Diagrams[index].Trains[type];

            // List初期化
            List<string> columnsList = ColumnsListInitialization("列車種別");

            // 列車プロパティを繰り返す
            foreach (var train in trains)
            {
                // 追加
                columnsList.Add(string.Format("{0}", m_DiaProFont[property.TrainTypes[train.TrainType].Name]));
            }

            // 行追加
            Rows.Add(columnsList.ToArray());

            // フォント設定
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_Dictionary["列車種別"];
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.BackColor = SystemColors.Control;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::RowsTrainTypeInitialization(int, DirectionType, RouteFileProperty)");
        }

        /// <summary>
        /// 初期化(列車名)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <param name="property"></param>
        private void RowsTrainNameInitialization(int index, DirectionType type, RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::RowsTrainNameInitialization(int, DirectionType, RouteFileProperty)");
            Logger.DebugFormat("index   :[{0}]", index);
            Logger.DebugFormat("type    :[{0}]", type);
            Logger.DebugFormat("property:[{0}]", property);

            // TrainPropertiesオブジェクト取得
            TrainProperties trains = property.Diagrams[index].Trains[type];

            // List初期化
            List<string> columnsList = ColumnsListInitialization("列車名");

            // 列車プロパティを繰り返す
            foreach (var train in trains)
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
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_Dictionary["列車名"];
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.BackColor = SystemColors.Control;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::RowsTrainNameInitialization(int, DirectionType, RouteFileProperty)");
        }

        /// <summary>
        /// 初期化(列車記号)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <param name="property"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void RowsTrainMarkInitialization(int index, DirectionType type, RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::RowsTrainMarkInitialization(int, DirectionType, RouteFileProperty)");
            Logger.DebugFormat("index   :[{0}]", index);
            Logger.DebugFormat("type    :[{0}]", type);
            Logger.DebugFormat("property:[{0}]", property);

            // TrainPropertiesオブジェクト取得
            TrainProperties trains = property.Diagrams[index].Trains[type];

            // List初期化
            List<string> columnsList = ColumnsListInitialization("");

            // 列車プロパティを繰り返す
            foreach (var train in trains)
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
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_Dictionary["列車記号"];
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.BackColor = SystemColors.Control;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::RowsTrainMarkInitialization(int, DirectionType, RouteFileProperty)");
        }

        /// <summary>
        /// 初期化(始発駅)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <param name="property"></param>
        private void RowsDepartingStationInitialization(int index, DirectionType type, RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::RowsDepartingStationInitialization(int, DirectionType, RouteFileProperty)");
            Logger.DebugFormat("index   :[{0}]", index);
            Logger.DebugFormat("type    :[{0}]", type);
            Logger.DebugFormat("property:[{0}]", property);

            // TrainPropertiesオブジェクト取得
            TrainProperties trains = property.Diagrams[index].Trains[type];

            // List初期化
            List<string> columnsList = ColumnsListInitialization("始発駅");

            // 列車プロパティを繰り返す
            foreach (var train in trains)
            {
                // 列Listに追加
                columnsList.Add(string.Format("{0}", train.DepartingStation));
            }

            // 行追加
            Rows.Add(columnsList.ToArray());

            // フォント設定
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_Dictionary["始発駅"];
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.BackColor = SystemColors.Control;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::RowsDepartingStationInitialization(int, DirectionType, RouteFileProperty)");
        }

        /// <summary>
        /// 初期化(終着駅)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <param name="property"></param>
        private void RowsDestinationStationInitialization(int index, DirectionType type, RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::RowsDestinationStationInitialization(int, DirectionType, RouteFileProperty)");
            Logger.DebugFormat("index   :[{0}]", index);
            Logger.DebugFormat("type    :[{0}]", type);
            Logger.DebugFormat("property:[{0}]", property);

            // TrainPropertiesオブジェクト取得
            TrainProperties trains = property.Diagrams[index].Trains[type];

            // List初期化
            List<string> columnsList = ColumnsListInitialization("終着駅");

            // 列車プロパティを繰り返す
            foreach (var train in trains)
            {
                // 列Listに追加
                columnsList.Add(string.Format("{0}", train.DestinationStation));
            }

            // 行追加
            Rows.Add(columnsList.ToArray());

            // フォント設定
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_Dictionary["終着駅"];
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.BackColor = SystemColors.Control;
            Rows[Rows.Count - 1].Frozen = true;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::RowsDestinationStationInitialization(int, DirectionType, RouteFileProperty)");
        }

        /// <summary>
        /// 初期化(駅時刻)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <param name="property"></param>
        /// <exception cref="AggregateException"></exception>
        private void RowsStationTimeInitialization(int index, DirectionType type, RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::RowsStationTimeInitialization(int, DirectionType, RouteFileProperty)");
            Logger.DebugFormat("index   :[{0}]", index);
            Logger.DebugFormat("type    :[{0}]", type);
            Logger.DebugFormat("property:[{0}]", property);

            // 方向種別で分岐
            switch (type)
            {
                case DirectionType.Outbound:
                    RowsOutboundStationTimeInitialization(index, property);
                    break;
                case DirectionType.Inbound:
                    RowsInboundStationTimeInitialization(index, property);
                    break;
                default:
                    throw new AggregateException(string.Format("方向種別の異常を検出しました:[{0}]", type));
            }

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::RowsStationTimeInitialization(int, DirectionType, RouteFileProperty)");
        }

        /// <summary>
        /// 初期化(駅時刻：下り)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="property"></param>
        private void RowsOutboundStationTimeInitialization(int index, RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::RowsOutboundStationTimeInitialization(int, RouteFileProperty)");
            Logger.DebugFormat("index   :[{0}]", index);
            Logger.DebugFormat("property:[{0}]", property);

            // TrainPropertiesオブジェクト取得
            TrainProperties trains = property.Diagrams[index].Trains[DirectionType.Outbound];

            // 駅数分繰り返す
            for (int i = 0; i < property.Stations.Count; i++)
            {
                // 駅時刻行取得
                Dictionary<DepartureArrivalType, List<string>> columnsList = GetStationTimeRows(DirectionType.Outbound, property.Stations[i], i, trains);

                // 行追加
                foreach (DepartureArrivalType type in columnsList.Keys)
                {
                    // 駅距離を設定
                    columnsList[type][0] = GetDistanceStringBetweenStations(DirectionType.Outbound, i, property.Stations);

                    // 行追加
                    Rows.Add(columnsList[type].ToArray());
                }
            }

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::RowsOutboundStationTimeInitialization(int, RouteFileProperty)");
        }

        /// <summary>
        /// 初期化(駅時刻：上り)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="property"></param>
        private void RowsInboundStationTimeInitialization(int index, RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::RowsInboundStationTimeInitialization(int, RouteFileProperty)");
            Logger.DebugFormat("index   :[{0}]", index);
            Logger.DebugFormat("property:[{0}]", property);

            // TrainPropertiesオブジェクト取得
            TrainProperties trains = property.Diagrams[index].Trains[DirectionType.Inbound];

            // 駅数分繰り返す
            for (int i = property.Stations.Count - 1; i >= 0; i--)
            {
                // 駅時刻行取得
                Dictionary<DepartureArrivalType, List<string>> columnsList = GetStationTimeRows(DirectionType.Inbound, property.Stations[i], i, trains);

                // 行追加
                foreach (DepartureArrivalType type in columnsList.Keys)
                {
                    // 駅距離を設定
                    columnsList[type][0] = GetDistanceStringBetweenStations(DirectionType.Inbound, i, property.Stations);

                    // 行追加
                    Rows.Add(columnsList[type].ToArray());
                }
            }

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::RowsInboundStationTimeInitialization(index, RouteFileProperty)");
        }

        /// <summary>
        /// 初期化(備考)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <param name="property"></param>
        private void RowsRemarksStationInitialization(int index, DirectionType type, RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::RowsRemarksStationInitialization(int, DirectionType, RouteFileProperty)");
            Logger.DebugFormat("index   :[{0}]", index);
            Logger.DebugFormat("type    :[{0}]", type);
            Logger.DebugFormat("property:[{0}]", property);

            // TrainPropertiesオブジェクト取得
            TrainProperties trains = property.Diagrams[index].Trains[type];

            // List初期化
            List<string> columnsList = ColumnsListInitialization("備考");

            // 列車プロパティを繰り返す
            foreach (var train in trains)
            {
                columnsList.Add(string.Format("{0}", train.Remarks));
            }

            // 行追加
            Rows.Add(columnsList.ToArray());

            // TODO:フォント設定(暫定)
            Rows[Rows.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Rows[Rows.Count - 1].DefaultCellStyle.Font = m_Dictionary["備考"];

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::RowsRemarksStationInitialization(int, DirectionType, RouteFileProperty)");
        }
        #endregion

        #region publicメソッド
        /// <summary>
        /// 時刻表描画
        /// </summary>
        public void DrawTimetable()
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTimetable::DrawTimetable()");

            // ダイアログインデックス取得
            int diagramIndex = m_RouteFileProperty.Diagrams.GetIndex(m_DiagramName);

            // 初期化
            Initialization(diagramIndex, m_DirectionType, m_RouteFileProperty);

            // ロギング
            Logger.Debug("<<<<= DataGridViewTimetable::DrawTimetable()");
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
        #endregion
        #endregion
    }
}
