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
using TrainTimeTable.EventArgs;
using TrainTimeTable.Property;

namespace TrainTimeTable
{
    public partial class FormTrainProperty : Form
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
        public delegate void UpdateEventHandler(object sender, TrainPropertyUpdateEventArgs e);

        /// <summary>
        /// 更新 event
        /// </summary>
        public event UpdateEventHandler OnUpdate = delegate { };
        #endregion

        /// <summary>
        /// RouteFilePropertyオブジェクト
        /// </summary>
        private RouteFileProperty m_RouteFileProperty = null;

        /// <summary>
        /// TrainPropertyオブジェクト
        /// </summary>
        public TrainProperty Property { get; set; } = new TrainProperty();

        /// <summary>
        /// ComboBoxTrainTypeオブジェクト
        /// </summary>
        private ComboBoxTrainType m_ComboBoxTrainType = null;

        /// <summary>
        /// ComboBoxStationオブジェクト(発駅)
        /// </summary>
        private ComboBoxStation m_ComboBoxDepartingStation = null;

        /// <summary>
        /// ComboBoxStationオブジェクト(着駅)
        /// </summary>
        private ComboBoxStation m_ComboBoxDestinationStation = null;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        /// <param name="train"></param>
        public FormTrainProperty(RouteFileProperty property, TrainProperty train)
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainProperty::FormTrainProperty(RouteFileProperty, TrainProperty)");
            Logger.DebugFormat("property:[{0}]", property);
            Logger.DebugFormat("train   :[{0}]", train);

            // コンポーネント初期化
            InitializeComponent();

            // 設定
            m_RouteFileProperty = property;
            Property.Copy(train);
            m_ComboBoxTrainType = new ComboBoxTrainType(property.TrainTypeSequences, property.TrainTypes);
            m_ComboBoxDepartingStation = new ComboBoxStation(property.Stations);
            m_ComboBoxDestinationStation = new ComboBoxStation(property.Stations);

            // ロギング
            Logger.Debug("<<<<= FormTrainProperty::FormTrainProperty(RouteFileProperty, TrainProperty)");
        }
        #endregion

        #region イベント
        #region FormTrainPropertyイベント
        private void FormTrainProperty_Load(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainProperty::FormTrainProperty_Load(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 描画一時停止
            SuspendLayout();

            // 変換
            PropertyToControl();

            // コントコール設定
            tableLayoutPanelMain.Dock = DockStyle.Fill;
            tabControlTrainProperty.Dock = DockStyle.Fill;
            tableLayoutPanelTrainProperty.Dock = DockStyle.Fill;
            textBoxTrainNo.Dock = DockStyle.Fill;
            m_ComboBoxTrainType.Dock = DockStyle.Fill;
            tableLayoutPanelTrainProperty.Controls.Add(m_ComboBoxTrainType, 1, 1);
            textBoxTrainName.Dock = DockStyle.Fill;
            textBoxTrainNumber.Dock = DockStyle.Fill;
            m_ComboBoxDepartingStation.Dock = DockStyle.Fill;
            tableLayoutPanelTrainProperty.Controls.Add(m_ComboBoxDepartingStation, 1, 3);
            m_ComboBoxDestinationStation.Dock = DockStyle.Fill;
            tableLayoutPanelTrainProperty.Controls.Add(m_ComboBoxDestinationStation, 1, 4);
            textBoxRemarks.Dock = DockStyle.Fill;
            tableLayoutPanelButton.Dock = DockStyle.Fill;
            buttonOK.Dock = DockStyle.Fill;
            buttonCancel.Dock = DockStyle.Fill;

            // 描画再開
            ResumeLayout();

            // ロギング
            Logger.Debug("<<<<= FormTrainProperty::FormTrainProperty_Load(object, EventArgs)");
        }
        #endregion

        #region Buttonイベント
        /// <summary>
        /// buttonOK_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainProperty::buttonOK_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 入力チェック
            if (!InputCheck())
            {
                // ロギング
                Logger.Debug("<<<<= FormTrainProperty::buttonOK_Click(object, EventArgs)");

                // 入力エラー
                return;
            }

            // 変換
            ControlToProperty();

            // イベント呼出
            OnUpdate(this, new TrainPropertyUpdateEventArgs() { Property = Property });

            // 正常終了
            DialogResult = DialogResult.OK;

            // ロギング
            Logger.Debug("<<<<= FormTrainProperty::buttonOK_Click(object, EventArgs)");
        }

        /// <summary>
        /// buttonCancel_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainProperty::buttonCancel_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // キャンセル
            DialogResult = DialogResult.Cancel;

            // ロギング
            Logger.Debug("<<<<= FormTrainProperty::buttonCancel_Click(object, EventArgs)");
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns></returns>
        private bool InputCheck()
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainProperty::InputCheck()");

            // 入力判定
            if (textBoxTrainNo.Text == string.Empty)
            {
                // メッセージ表示
                MessageBox.Show("列車番号が指定されていません", AssemblyLibrary.GetTitleVersion(), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // 異常終了
                return false;
            }

            // ロギング
            Logger.Debug("<<<<= FormTrainProperty::InputCheck()");

            // 正常終了
            return true;
        }

        /// <summary>
        /// プロパティ⇒コントコール変換
        /// </summary>
        private void PropertyToControl()
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainProperty::PropertyToControl()");

            // 設定
            textBoxTrainNo.Text = Property.No;
            m_ComboBoxTrainType.SetSelected(Property.TrainTypeName);
            textBoxTrainName.Text = Property.Name;
            textBoxTrainNumber.Text = Property.Number;
            m_ComboBoxDepartingStation.SetSelected(Property.DepartingStation);
            m_ComboBoxDestinationStation.SetSelected(Property.DestinationStation);
            textBoxRemarks.Text = Property.Remarks;

            // ロギング
            Logger.Debug("<<<<= FormTrainProperty::PropertyToControl()");
        }

        /// <summary>
        /// コントコール⇒プロパティ変換
        /// </summary>
        /// <returns></returns>
        private void ControlToProperty()
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainProperty::ControlToProperty()");

            // 設定
            Property.No = textBoxTrainNo.Text;
            Property.TrainTypeName = m_ComboBoxTrainType.GetSelected();
            Property.Name = textBoxTrainName.Text;
            Property.Number = textBoxTrainNumber.Text;
            Property.DepartingStation = m_ComboBoxDepartingStation.GetSelected();
            Property.DestinationStation = m_ComboBoxDestinationStation.GetSelected();
            Property.Remarks = textBoxRemarks.Text;

            // ロギング
            Logger.DebugFormat("Property:[{0}]", Property);
            Logger.Debug("<<<<= FormTrainProperty::ControlToProperty()");
        }
        #endregion
        #endregion
    }
}
