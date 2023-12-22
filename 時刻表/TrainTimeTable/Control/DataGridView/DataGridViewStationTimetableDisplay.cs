using log4net;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Common;
using TrainTimeTable.Component;
using TrainTimeTable.Property;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// DataGridViewStationTimetableDisplayクラス
    /// </summary>
    public class DataGridViewStationTimetableDisplay : DataGridView
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        //private Dictionary<int, List<StationTimeProperty>> m_StationTimeProperties = new Dictionary<int, List<StationTimeProperty>>();

        // 情報テーブル
        private DictionaryStationTimeProperties m_StationTimeProperties = new DictionaryStationTimeProperties();

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
        /// <param name="station"></param>
        /// <param name="property"></param>
        public DataGridViewStationTimetableDisplay(string text, DirectionType type, StationProperty station, RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStationTimetableDisplay::DataGridViewStationTimetableDisplay(string, DirectionType, StationProperty, RouteFileProperty)");
            Logger.DebugFormat("text    :[{0}]", text);
            Logger.DebugFormat("type    :[{0}]", type);
            Logger.DebugFormat("station :[{0}]", station);
            Logger.DebugFormat("property:[{0}]", property);

            // 設定
            m_RouteFileProperty = property;

            // Font辞書初期化
            m_Dictionary = m_RouteFileProperty.Fonts.GetFonts(
                new List<string>()
                {
                    "駅時刻表",
                });

            // Font設定
            Font = m_Dictionary["駅時刻表"];

            // 初期化
            Initialization();

            // ダイヤインデックス取得
            int diagramIndex = property.Diagrams.GetIndex(text);

            // 更新
            Update(diagramIndex, type, station);

            // ロギング
            Logger.Debug("<<<<= DataGridViewStationTimetableDisplay::DataGridViewStationTimetableDisplay(string, DirectionType, StationProperty, RouteFileProperty)");
        }
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStationTimetableDisplay::Initialization()");

            // 設定
            ReadOnly = true;                      // 読取専用
            AllowUserToDeleteRows = false;        // 行削除禁止
            AllowUserToAddRows = false;           // 行挿入禁止
            AllowUserToResizeRows = false;        // 行の高さ変更禁止
            ColumnHeadersVisible = false;         // 列ヘッダーを非表示にする
            RowHeadersVisible = false;            // 行ヘッダーを非表示にする
            MultiSelect = false;                  // セル、行、列が複数選択禁止
            //ヘッダーとすべてのセルの内容に合わせて、列の幅を自動調整する
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //ヘッダーとすべてのセルの内容に合わせて、行の高さを自動調整する
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            // ヘッダーの色等
            //EnableHeadersVisualStyles = false;     // Visualスタイルを使用しない
            ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            ColumnHeadersDefaultCellStyle.Font = m_Dictionary["駅時刻表"];
            ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //ヘッダ高さ
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            ColumnHeadersHeight = 28;
            Columns.Add("Hour", "");
            Columns["Hour"].Frozen = true;
            //CellPainting += DataGridViewStationTimetableDisplay_CellPainting;

            // ロギング
            Logger.Debug("<<<<= DataGridViewStationTimetableDisplay::Initialization()");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <param name="station"></param>
        private void Update(int index, DirectionType type, StationProperty station)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStationTimetableDisplay::Update(int, DirectionType, StationProperty)");
            Logger.DebugFormat("index  :[{0}]", index);
            Logger.DebugFormat("type   :[{0}]", type);
            Logger.DebugFormat("station:[{0}]", station);

            // 列車分繰り返す
            foreach (var trainProperty in m_RouteFileProperty.Diagrams[index].Trains[type])
            {
                // 時刻表数判定
                if (trainProperty.StationTimes.Count == 0)
                {
                    // ロギング
                    Logger.WarnFormat("{0}駅({1})の以下の列車は時刻表登録がありません", station.Name, station.Seq);
                    Logger.Warn(trainProperty.ToString());

                    // 登録がないのでスキップ
                    continue;
                }

                // StationTimePropertyオブジェクト取得
                StationTimeProperty stationTimeProperty = trainProperty.StationTimes.Find(t => t.StationName == station.Name);

                // 駅扱い判定
                if (stationTimeProperty.StationTreatment != StationTreatment.Stop)
                {
                    // 停車以外はスキップ
                    continue;
                }

                // 発時刻判定
                if (stationTimeProperty.DepartureTime == string.Empty && !stationTimeProperty.EstimatedDepartureTime)
                {
                    // ロギング
                    Logger.WarnFormat("{0}駅({1})の以下の列車は発時刻登録がありません", station.Name, station.Seq);
                    Logger.Warn(trainProperty.ToString());
                    Logger.Warn(stationTimeProperty.ToString());

                    // 登録がないのでスキップ
                    continue;
                }

                // 発時刻DateTime取得
                DateTime departureTime = stationTimeProperty.GetDepartureTimeValue();

                // 情報テーブルに登録
                m_StationTimeProperties.Add(departureTime.Hour, stationTimeProperty);
            }

            // カラム数最大値を取得
            int columnMax = m_StationTimeProperties.GetColumnMax();

            // カラム追加
            ColumnsAdd(0, columnMax);

            // 行追加
            RowsAdd(columnMax, m_RouteFileProperty.DiagramScreen, m_RouteFileProperty.TrainTypes, m_RouteFileProperty.Diagrams[index].Trains[type]);

            // ロギング
            Logger.Debug("<<<<= DataGridViewStationTimetableDisplay::Update(int, DirectionType, StationProperty)");
        }

        /// <summary>
        /// カラム追加
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void ColumnsAdd(int start, int end)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStationTimetableDisplay::ColumnsAdd(int, int)");

            // カラム追加
            for (int i = start; i < end; i++)
            {
                // 列のセルのテキストを折り返して表示する
                Columns.Add(string.Format("column{0}", i), "");
                Columns[string.Format("column{0}", i)].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                Columns[string.Format("column{0}", i)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // ロギング
            Logger.Debug("<<<<= DataGridViewStationTimetableDisplay::ColumnsAdd(int, int)");
        }

        /// <summary>
        /// 行追加
        /// </summary>
        /// <param name="columnMax"></param>
        /// <param name="screenProperty"></param>
        /// <param name="trainTypeProperties"></param>
        /// <param name="trainProperties"></param>
        private void RowsAdd(int columnMax, DiagramScreenProperty screenProperty, TrainTypeProperties trainTypeProperties, TrainProperties trainProperties)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStationTimetableDisplay::RowsAdd(int, DiagramScreenProperty, TrainTypeProperties, TrainProperties)");
            Logger.DebugFormat("columnMax          :[{0}]", columnMax);
            Logger.DebugFormat("screenProperty     :[{0}]", screenProperty);
            Logger.DebugFormat("trainTypeProperties:[{0}]", trainTypeProperties);
            Logger.DebugFormat("trainProperties    :[{0}]", trainProperties);

            // 開始時間を取得
            DateTime currentDateTime = screenProperty.DiagramDtartingTime;

            // 24時間分繰り返す
            for (int hour = 0; hour < 24; hour++)
            {
                // カラム文字列
                List<string> columns = new List<string> { string.Format("{0:D2}", currentDateTime.Hour) };

                // カラム色
                List<Color> colors = new List<Color>() { Color.Black };

                // 当該時間の列車時刻が存在しているか？
                if (m_StationTimeProperties.ContainsKey(currentDateTime.Hour))
                {
                    // 表示ソート
                    var sortStationTimeProperties = m_StationTimeProperties[currentDateTime.Hour].OrderBy(t => t.DepartureTime);

                    // 存在していたら列車時刻分繰り返す
                    foreach (var property in sortStationTimeProperties)//m_StationTimeProperties[currentDateTime.Hour])
                    {
                        // 追加文字列オブジェクト
                        StringBuilder sb = new StringBuilder();

                        // 列車情報取得
                        TrainProperty trainProperty = trainProperties.Find(t => t.Id == property.TrainId);

                        // 列車種別
                        string trainType = m_DiaProFont[trainTypeProperties[trainProperty.TrainType].Name];
                        if (trainType != string.Empty) { sb.AppendLine(trainType); }

                        // 列車名取得
                        string trainName = string.Format("{0}{1}", trainProperty.Name, trainProperty.Number);
                        if (trainName != string.Empty) { sb.AppendLine(trainName); }

                        // 発時刻(分)取得
                        string departureTime = property.DepartureTime.Substring(2);
                        sb.AppendLine(departureTime);

                        // 行先取得
                        string destination = trainProperty.DestinationStation;
                        sb.Append(destination);

                        // 登録
                        columns.Add(sb.ToString());
                        colors.Add(trainTypeProperties[trainProperty.TrainType].StringsColor);
                    }
                }

                // 行追加
                Rows.Add(columns.ToArray());

                // 色追加
                int cell = 0;
                foreach (var color in colors)
                {
                    Rows[Rows.Count - 1].Cells[cell++].Style.ForeColor = color;
                }

                // 1時間後の日付を取得
                currentDateTime = currentDateTime.AddHours(1);
            }

            // ロギング
            Logger.Debug("<<<<= DataGridViewStationTimetableDisplay::RowsAdd(int, DiagramScreenProperty, TrainTypeProperties, TrainProperties)");
        }
        #endregion
    }
}
