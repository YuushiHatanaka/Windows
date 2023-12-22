using log4net;
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
using TrainTimeTable.EventArgs;
using TrainTimeTable.Property;

namespace TrainTimeTable
{
    /// <summary>
    /// FormStationTimetableクラス
    /// </summary>
    public partial class FormStationTimetable : Form
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
        public delegate void UpdateEventHandler(object sender, StationTimetableUpdateEventArgs e);

        /// <summary>
        /// 更新 event
        /// </summary>
        public event UpdateEventHandler OnUpdate = delegate { };
        #endregion

        /// <summary>
        /// FormRoute(親)オブジェクト
        /// </summary>
        private FormRoute m_Owner = null;

        /// <summary>
        /// ダイヤグラムID
        /// </summary>
        private int m_DiagramId = 0;

        /// <summary>
        /// RouteFilePropertyオブジェクト
        /// </summary>
        private RouteFileProperty m_RouteFileProperty = null;

        /// <summary>
        /// DataGridViewStationTimetableオブジェクト
        /// </summary>
        private DataGridViewStationTimetable m_DataGridViewStationTimetable = null;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="regName"></param>
        /// <param name="index"></param>
        /// <param name="property"></param>
        public FormStationTimetable(FormRoute owner, string regName, int index, RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationTimetable::FormStationTimetable(FormRoute, string, int, RouteFileProperty)");
            Logger.DebugFormat("owner   :[{0}]", owner);
            Logger.DebugFormat("regName :[{0}]", regName);
            Logger.DebugFormat("index   :[{0}]", index);
            Logger.DebugFormat("property:[{0}]", property);

            InitializeComponent();

            // 設定
            m_Owner = owner;
            m_DiagramId = index;
            m_RouteFileProperty = property;
            m_DataGridViewStationTimetable = new DataGridViewStationTimetable(property);
            m_DataGridViewStationTimetable.CellClick += DataGridViewStationTimetable_CellClick;

            // タイトル設定
            Text = string.Format("{0}", regName);

            // ロギング
            Logger.Debug("<<<<= FormStationTimetable::FormStationTimetable(FormRoute, string, int, RouteFileProperty)");
        }
        #endregion

        #region イベント
        #region FormStationTimetableイベント

        private void FormStationTimetable_Load(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationTimetable::FormStationTimetable_Load(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 設定
            m_DataGridViewStationTimetable.Dock = DockStyle.Fill;
            Controls.Add(m_DataGridViewStationTimetable);

            // ロギング
            Logger.Debug("<<<<= FormStationTimetable::FormStationTimetable_Load(object, EventArgs)");
        }
        #endregion

        #region DataGridViewStationTimetableイベント
        /// <summary>
        /// DataGridViewStationTimetable_CellClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewStationTimetable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationTimetable::DataGridViewStationTimetable_CellClick(object, DataGridViewCellEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 方向種別を初期化
            DirectionType directionType = DirectionType.None;

            // カラム位置で分岐する
            switch (e.ColumnIndex)
            {
                case 2:
                    directionType = DirectionType.Outbound;
                    break;
                case 3:
                    directionType = DirectionType.Inbound;
                    break;
                default:
                    // ロギング
                    Logger.Debug("<<<<= FormStationTimetable::DataGridViewStationTimetable_CellClick(object, DataGridViewCellEventArgs)");
                    // 何もしない
                    return;
            }

            DataGridViewStationTimetable stationTimetable = (DataGridViewStationTimetable)sender;

            // 駅名セルを取得
            DataGridViewCell stationCell = stationTimetable[1, e.RowIndex];

            // 駅情報を取得
            StationProperty property = m_RouteFileProperty.Stations.Find(t => t.Name == stationCell.Value.ToString());

            // 駅情報判定
            if (property != null)
            {
                // 登録名設定
                string regName = FormStationTimetableDisplay.GetTitle(directionType, property.Name, m_RouteFileProperty.Diagrams[m_DiagramId].Name);

                // 登録判定
                if (!m_Owner.IsMDIChildForm(typeof(FormStationTimetableDisplay), regName))
                {
                    // フォーム生成
                    FormStationTimetableDisplay form = new FormStationTimetableDisplay(m_RouteFileProperty.Diagrams[m_DiagramId].Name, directionType, property, m_RouteFileProperty);

                    // フォーム登録
                    m_Owner.AddMDIChildForm(form);

                    // フォーム表示
                    form.Show();
                }
                else
                {
                    // フォーム取得
                    FormStationTimetableDisplay form = m_Owner.GetMDIChildForm(typeof(FormStationTimetableDisplay), regName) as FormStationTimetableDisplay;

                    // フォームを前面表示する
                    form.BringToFront();
                    form.WindowState = FormWindowState.Normal;
                }
            }
            else
            {
                // ロギング
                Logger.WarnFormat("選択対象駅が存在していません：[{0}]", stationCell.Value.ToString());
                Logger.Warn(m_RouteFileProperty.Stations.ToString());
            }

            // ロギング
            Logger.Debug("<<<<= FormStationTimetable::DataGridViewStationTimetable_CellClick(object, DataGridViewCellEventArgs)");
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
            Logger.Debug("=>>>> FormStationTimetable::UpdateNotification(TimetableProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 更新
            Update(property);

            // ロギング
            Logger.Debug("<<<<= FormStationTimetable::UpdateNotification(TimetableProperty)");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        public void Update(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationTimetable::Update(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 更新
            m_DataGridViewStationTimetable.Update(property);
            m_RouteFileProperty = property;

            // ロギング
            Logger.Debug("<<<<= FormStationTimetable::Update(RouteFileProperty)");
        }
        #endregion
    }
}
