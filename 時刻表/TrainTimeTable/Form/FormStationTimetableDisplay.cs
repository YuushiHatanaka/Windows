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
        /// RouteFilePropertyオブジェクト
        /// </summary>
        private RouteFileProperty m_RouteFileProperty = null;

        /// <summary>
        /// ダイヤグラムインデックス
        /// </summary>
        private int m_DiagramIndex = 0;

        /// <summary>
        /// 方向種別
        /// </summary>
        private DirectionType m_DirectionType = DirectionType.None;

        /// <summary>
        /// StationPropertyオブジェクト
        /// </summary>
        private StationProperty m_StationProperty = null;

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

            // 設定
            m_DirectionType = type;
            m_RouteFileProperty = property;
            m_StationProperty = station;

            // ダイヤインデックス取得
            m_DiagramIndex = property.Diagrams.GetIndex(text);

            // タイトル取得
            Text = GetTitle(type, station.Name, property.Diagrams[m_DiagramIndex].Name);

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

            // 設定
            m_RouteFileProperty = property;

            // タイトル取得
            Text = GetTitle(m_DirectionType, m_StationProperty.Name, property.Diagrams[m_DiagramIndex].Name);

            // 更新
            m_DataGridViewStationTimetableDisplay.Update(m_DiagramIndex, m_DirectionType, m_StationProperty, m_RouteFileProperty);

            // ロギング
            Logger.Debug("<<<<= FormStationTimetableDisplay::UpdateNotification(RouteFileProperty)");
        }
        #endregion
    }
}
