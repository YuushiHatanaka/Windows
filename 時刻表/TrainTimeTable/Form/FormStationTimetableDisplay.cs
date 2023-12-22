using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Common;
using TrainTimeTable.Control;
using TrainTimeTable.Property;
using static System.Net.Mime.MediaTypeNames;

namespace TrainTimeTable
{
    /// <summary>
    /// FormStationTimetableDisplayクラス
    /// </summary>
    public partial class FormStationTimetableDisplay : Form
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// DataGridViewStationTimetableDisplayオブジェクト
        /// </summary>
        private DataGridViewStationTimetableDisplay m_DataGridViewStationTimetableDisplay = null;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="text"></param>
        /// <param name="type"></param>
        /// <param name="station"></param>
        /// <param name="property"></param>
        public FormStationTimetableDisplay(string text, DirectionType type, StationProperty station, RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationTimetableDisplay::FormStationTimetableDisplay(string, DirectionType, StationProperty, RouteFileProperty)");
            Logger.DebugFormat("text    :[{0}]", text);
            Logger.DebugFormat("type    :[{0}]", type);
            Logger.DebugFormat("station :[{0}]", station);
            Logger.DebugFormat("property:[{0}]", property);

            // コンポーネント初期化
            InitializeComponent();

            // ダイヤインデックス取得
            int diagramIndex = property.Diagrams.GetIndex(text);

            // タイトル取得
            Text = GetTitle(type, station.Name, property.Diagrams[diagramIndex].Name);

            // DataGridViewStationTimetableDisplayオブジェクト生成
            m_DataGridViewStationTimetableDisplay = new DataGridViewStationTimetableDisplay(text, type, station, property);

            // ロギング
            Logger.Debug("<<<<= FormStationTimetableDisplay::FormStationTimetableDisplay(string, DirectionType, StationProperty, RouteFileProperty)");
        }
        #endregion

        #region イベント
        /// <summary>
        /// FormStationTimetableDisplay_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormStationTimetableDisplay_Load(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationTimetable::FormStationTimetableDisplay_Load(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 設定
            m_DataGridViewStationTimetableDisplay.Dock = DockStyle.Fill;
            Controls.Add(m_DataGridViewStationTimetableDisplay);

            // ロギング
            Logger.Debug("<<<<= FormStationTimetable::FormStationTimetableDisplay_Load(object, EventArgs)");
        }
        #endregion

        #region publicメソッド
        /// <summary>
        /// タイトル設定
        /// </summary>
        /// <param name="type"></param>
        /// <param name="stationName"></param>
        /// <param name="diagramName"></param>
        /// <returns></returns>
        public static string GetTitle(DirectionType type, string stationName, string diagramName)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationTimetableDisplay::GetTitle(DirectionType, string, string)");
            Logger.DebugFormat("type       :[{0}]", type);
            Logger.DebugFormat("stationName:[{0}]", stationName);
            Logger.DebugFormat("diagramName:[{0}]", diagramName);

            // 結果設定
            string result = string.Format("{0} {1} 駅時刻表【{2}】", stationName, diagramName, type.GetStringValue());

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= FormStationTimetableDisplay::GetTitle(DirectionType, string, string)");

            // 返却
            return result;
        }

        /// <summary>
        /// 更新通知
        /// </summary>
        public void UpdateNotification(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationTimetableDisplay::UpdateNotification(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= FormStationTimetableDisplay::UpdateNotification(RouteFileProperty)");
        }
        #endregion
    }
}
