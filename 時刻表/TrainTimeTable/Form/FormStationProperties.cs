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
using TrainTimeTable.Control;
using TrainTimeTable.EventArgs;
using TrainTimeTable.Property;
using static System.Net.Mime.MediaTypeNames;

namespace TrainTimeTable
{
    /// <summary>
    /// FormStationPropertiesクラス
    /// </summary>
    public partial class FormStationProperties : Form
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
        public delegate void UpdateEventHandler(object sender, StationPropertiesUpdateEventArgs e);

        /// <summary>
        /// 更新 event
        /// </summary>
        public event UpdateEventHandler OnUpdate = delegate { };
        #endregion

        /// <summary>
        /// RouteFilePropertyオブジェクト
        /// </summary>
        private RouteFileProperty m_RouteFileProperty { get; set; } = null;

        /// <summary>
        /// DataGridViewStationオブジェクト
        /// </summary>
        private DataGridViewStation m_DataGridViewStation { get; set; } = null;

        /// <summary>
        /// ダイアグラム名
        /// </summary>
        public string DiagramName { get; private set; } = string.Empty;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public FormStationProperties(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationProperties::FormStationProperties(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コンポーネント初期化
            InitializeComponent();

            // 設定
            m_RouteFileProperty = property;
            m_DataGridViewStation = new DataGridViewStation(property);
            m_DataGridViewStation.OnUpdate += DataGridViewStation_OnUpdate;

            // ロギング
            Logger.Debug("<<<<= FormStationProperties::FormStationProperties(RouteFileProperty)");
        }
        #endregion

        #region イベント
        #region FormStationPropertiesイベント
        /// <summary>
        /// FormStationProperties_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormStationProperties_Load(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationProperties::FormStationProperties_Load(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // コントコール設定
            m_DataGridViewStation.Dock = DockStyle.Fill;
            Controls.Add(m_DataGridViewStation);

            // ロギング
            Logger.Debug("<<<<= FormStationProperties::FormStationProperties_Load(object, EventArgs)");
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
            Logger.Debug("=>>>> FormStationProperties::UpdateNotification(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 更新
            Update(property);

            // ロギング
            Logger.Debug("<<<<= FormStationProperties::UpdateNotification(RouteFileProperty)");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        public void Update(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationProperties::Update(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= FormStationProperties::Update(RouteFileProperty)");
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
            if (property.Name == DiagramName)
            {
                // フォームクローズ
                Close();
            }

            // ロギング
            Logger.Debug("<<<<= FormStationProperties::RemoveNotification(DiagramProperty)");
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// DataGridViewStation_OnUpdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewStation_OnUpdate(object sender, StationPropertiesUpdateEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationProperties::DataGridViewStation_OnUpdate(object, StationPropertiesUpdateEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 更新イベント呼び出し
            OnUpdate(sender, e);

            // ロギング
            Logger.Debug("<<<<= FormStationProperties::DataGridViewStation_OnUpdate(object, StationPropertiesUpdateEventArgs)");
        }
        #endregion
    }
}
