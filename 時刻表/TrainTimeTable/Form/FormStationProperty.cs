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
using static System.Net.Mime.MediaTypeNames;

namespace TrainTimeTable
{
    /// <summary>
    /// FormStationPropertyクラス
    /// </summary>
    public partial class FormStationProperty : Form
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
        public delegate void UpdateEventHandler(object sender, StationPropertyUpdateEventArgs e);

        /// <summary>
        /// 更新 event
        /// </summary>
        public event UpdateEventHandler OnUpdate = delegate { };
        #endregion

        /// <summary>
        /// StationPropertyオブジェクト生成
        /// </summary>
        public StationProperty Property { get; set; } = new StationProperty();

        /// <summary>
        /// ComboBoxDiagramTrainInformation辞書オブジェクト
        /// </summary>
        private Dictionary<DirectionType, ComboBoxDiagramTrainInformation> m_ComboBoxDiagramTrainInformations = new Dictionary<DirectionType, ComboBoxDiagramTrainInformation>()
        {
            { DirectionType.Outbound, new ComboBoxDiagramTrainInformation(){ Dock= DockStyle.Fill} },
            { DirectionType.Inbound, new ComboBoxDiagramTrainInformation(){ Dock= DockStyle.Fill}  },
        };

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public FormStationProperty(StationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationProperty::FormStationProperty(StationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コンポーネント初期化
            InitializeComponent();

            // 設定
            Property.Copy(property);

            // ロギング
            Logger.Debug("<<<<= FormStationProperty::FormStationProperty(StationProperty)");
        }
        #endregion

        #region イベント
        #region FormStationPropertyイベント
        /// <summary>
        /// FormStationProperty_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormStationProperty_Load(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationProperty::FormStationProperty_Load(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 変換
            PropertyToControl();

            // コントコール設定
            tableLayoutPanelMain.Dock = DockStyle.Fill;
            tableLayoutPanelStationName.Dock = DockStyle.Fill;
            textBoxStationName.Dock = DockStyle.Fill;
            tableLayoutPanelItems.Dock = DockStyle.Fill;
            groupBoxStationTimeFormat.Dock = DockStyle.Fill;
            tableLayoutPanelStationTimeFormat.Dock = DockStyle.Fill;
            groupBoxStationScale.Dock = DockStyle.Fill;
            tableLayoutPanelStationScale.Dock = DockStyle.Fill;
            groupBoxDiagramTrainInformation.Dock = DockStyle.Fill;
            tableLayoutPanelDiagramTrainInformation.Dock = DockStyle.Fill;
            tableLayoutPanelDiagramTrainInformation.Controls.Add(m_ComboBoxDiagramTrainInformations[DirectionType.Outbound], 1, 0);
            tableLayoutPanelDiagramTrainInformation.Controls.Add(m_ComboBoxDiagramTrainInformations[DirectionType.Inbound], 1, 1);
            tableLayoutPanelButton.Dock = DockStyle.Fill;
            buttonOK.Dock = DockStyle.Fill;
            buttonCancel.Dock = DockStyle.Fill;

            // ロギング
            Logger.Debug("<<<<= FormStationProperty::FormStationProperty_Load(object, EventArgs)");
        }
        #endregion

        #region buttonイベント
        /// <summary>
        /// buttonOK_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationProperty::buttonOK_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 入力チェック
            if (!InputCheck())
            {
                // ロギング
                Logger.Debug("<<<<= FormStationProperty::buttonOK_Click(object, EventArgs)");

                // 入力エラー
                return;
            }

            // 変換
            ControlToProperty();

            // イベント呼出
            OnUpdate(this, new StationPropertyUpdateEventArgs() { Property = Property });

            // 正常終了
            DialogResult = DialogResult.OK;

            // ロギング
            Logger.Debug("<<<<= FormStationProperty::buttonOK_Click(object, EventArgs)");
        }

        /// <summary>
        /// buttonCancel_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationProperty::buttonCancel_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // キャンセル
            DialogResult = DialogResult.Cancel;

            // ロギング
            Logger.Debug("<<<<= FormStationProperty::buttonCancel_Click(object, EventArgs)");
        }
        #endregion
        #endregion

        #region privateメソッド
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns></returns>
        private bool InputCheck()
        {
            // ロギング
            Logger.Debug("=>>>> FormStationProperty::InputCheck()");

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= FormStationProperty::InputCheck()");

            // 正常終了
            return true;
        }

        /// <summary>
        /// プロパティ⇒コントコール変換
        /// </summary>
        private void PropertyToControl()
        {
            // ロギング
            Logger.Debug("=>>>> FormStationProperty::PropertyToControl()");

            // TODO:未実装
            textBoxStationName.Text = Property.Name;

            // ロギング
            Logger.Debug("<<<<= FormStationProperty::PropertyToControl()");
        }

        /// <summary>
        /// コントコール⇒プロパティ変換
        /// </summary>
        /// <returns></returns>
        private void ControlToProperty()
        {
            // ロギング
            Logger.Debug("=>>>> FormStationProperty::ControlToProperty()");

            // TODO:未実装
            Property.Name = textBoxStationName.Text;

            // ロギング
            Logger.Debug("<<<<= FormStationProperty::ControlToProperty()");
        }
        #endregion

    }
}
