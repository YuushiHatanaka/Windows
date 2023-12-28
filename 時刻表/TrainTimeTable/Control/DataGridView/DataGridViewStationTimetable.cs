using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Property;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// DataGridViewStationTimetableクラス
    /// </summary>
    public class DataGridViewStationTimetable : DataGridView
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// RouteFilePropertyオブジェクト
        /// </summary>
        private RouteFileProperty m_RouteFileProperty = null;

        /// <summary>
        /// Font辞書
        /// </summary>
        private Dictionary<string, Font> m_Dictionary = null;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>

        public DataGridViewStationTimetable(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStationTimetable::DataGridViewStationTimetable(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 設定
            m_RouteFileProperty = property;

            // Font辞書初期化
            m_Dictionary = m_RouteFileProperty.Fonts.GetFonts(new List<string>() { "駅時刻表一覧", });

            // Font設定
            Font = m_Dictionary["駅時刻表一覧"];

            // 初期化
            Initialization();

            // 更新
            Update(property);

            // ロギング
            Logger.Debug("<<<<= DataGridViewStationTimetable::DataGridViewStationTimetable(RouteFileProperty)");
        }
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStationTimetable::Initialization()");

            ReadOnly = true;                      // 読取専用
            AllowUserToDeleteRows = false;        // 行削除禁止
            AllowUserToAddRows = false;           // 行挿入禁止
            AllowUserToResizeRows = false;        // 行の高さ変更禁止
            ColumnHeadersVisible = false;         // 列ヘッダーを非表示にする
            RowHeadersVisible = false;            // 行ヘッダーを非表示にする
            MultiSelect = false;                  // セル、行、列が複数選択禁止
            //ヘッダーの色等
            //EnableHeadersVisualStyles = false;     // Visualスタイルを使用しない
            ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            //ColumnHeadersDefaultCellStyle.Font = m_Dictionary["時刻表ビュー"];
            ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //ヘッダ高さ
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            ColumnHeadersHeight = 28;
            Columns.Add("Number", "");
            Columns.Add("StationName", "駅名");
            Columns.Add("StationTimeFormat", "下り");
            Columns.Add("StationSize", "上り");

            // ロギング
            Logger.Debug("<<<<= DataGridViewStationTimetable::Initialization()");
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
            Logger.Debug("=>>>> DataGridViewStationTimetable::Update(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 全行クリア
            Rows.Clear();

            // 駅シーケンスリスト取得(昇順)
            var stationSequences = m_RouteFileProperty.StationSequences.OrderBy(t => t.Seq);

            // 駅を繰り返す
            int seq = 1;
            foreach (var stationSequence in stationSequences)
            {
                // 駅情報取得
                StationProperty station = m_RouteFileProperty.Stations.Find(t => t.Name == stationSequence.Name);

                // カラムリスト初期化
                List<string> values = new List<string>();

                // カラムリスト初期設定
                values.Add(seq.ToString());
                values.Add(station.Name);

                // カラム設定
                if (station.StartingStation)
                {
                    // 始発駅の場合
                    if (property.Route.OutboundDiaName != string.Empty)
                    {
                        values.Add(property.Route.OutboundDiaName);
                    }
                    else
                    {
                        values.Add("下り");
                    }
                    values.Add("");
                }
                else if (station.TerminalStation)
                {
                    // 終着駅の場合
                    values.Add("");
                    if (property.Route.InboundDiaName != string.Empty)
                    {
                        values.Add(property.Route.InboundDiaName);
                    }
                    else
                    {
                        values.Add("上り");
                    }
                }
                else
                {
                    // 上記以外の場合
                    if (property.Route.OutboundDiaName != string.Empty)
                    {
                        values.Add(property.Route.OutboundDiaName);
                    }
                    else
                    {
                        values.Add("下り");
                    }
                    if (property.Route.InboundDiaName != string.Empty)
                    {
                        values.Add(property.Route.InboundDiaName);
                    }
                    else
                    {
                        values.Add("上り");
                    }
                }

                // シーケンス番号更新
                seq++;

                // 行追加
                Rows.Add(values.ToArray());
            }

            // ロギング
            Logger.Debug("<<<<= DataGridViewStationTimetable::Update(RouteFileProperty)");
        }
        #endregion
    }
}
