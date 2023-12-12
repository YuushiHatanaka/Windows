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
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace TrainTimeTable
{
    /// <summary>
    /// FormTrainTypePropertiesクラス
    /// </summary>
    public partial class FormTrainTypeProperties : Form
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
        public delegate void UpdateEventHandler(object sender, TrainTypePropertiesUpdateEventArgs e);

        /// <summary>
        /// 更新 event
        /// </summary>
        public event UpdateEventHandler OnUpdate = delegate { };
        #endregion

        /// <summary>
        /// TrainTypePropertiesオブジェクト
        /// </summary>
        private TrainTypeProperties m_TrainTypeProperties = null;

        /// <summary>
        /// DataGridViewTrainTypeオブジェクト
        /// </summary>
        private DataGridViewTrainType m_DataGridViewTrainType = null;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public FormTrainTypeProperties(TrainTypeProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainTypeProperties::FormTrainTypeProperties(StationProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // コンポーネント初期化
            InitializeComponent();

            // 設定
            m_TrainTypeProperties = properties;
            m_DataGridViewTrainType = new DataGridViewTrainType(properties);
            m_DataGridViewTrainType.OnUpdate += DataGridViewTrainType_OnUpdate;

            // ロギング
            Logger.Debug("<<<<= FormTrainTypeProperties::FormTrainTypeProperties(StationProperties)");
        }
        #endregion

        #region イベント
        #region FormTrainTypePropertiesイベント
        /// <summary>
        /// FormTrainTypeProperties_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormTrainTypeProperties_Load(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainTypeProperties::FormTrainTypeProperties_Load(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // コントコール設定
            m_DataGridViewTrainType.Dock = DockStyle.Fill;
            Controls.Add(m_DataGridViewTrainType);

            // ロギング
            Logger.Debug("<<<<= FormTrainTypeProperties::FormTrainTypeProperties_Load(object, EventArgs)");
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
            Logger.Debug("=>>>> FormTrainTypeProperties::UpdateNotification(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 更新
            Update(property);

            // ロギング
            Logger.Debug("<<<<= FormTrainTypeProperties::UpdateNotification(RouteFileProperty)");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        public void Update(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainTypeProperties::Update(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= FormTrainTypeProperties::Update(RouteFileProperty)");
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// DataGridViewTrainType_OnUpdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewTrainType_OnUpdate(object sender, TrainTypePropertiesUpdateEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainTypeProperties::DataGridViewTrainType_OnUpdate(object, TrainTypePropertiesUpdateEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 更新イベント呼び出し
            OnUpdate(sender, e);

            // ロギング
            Logger.Debug("<<<<= FormTrainTypeProperties::DataGridViewTrainType_OnUpdate(object, TrainTypePropertiesUpdateEventArgs)");
        }
        #endregion
    }
}
