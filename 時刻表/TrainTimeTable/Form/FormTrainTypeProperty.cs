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
using TrainTimeTable.EventArgs;
using TrainTimeTable.Property;

namespace TrainTimeTable
{
    /// <summary>
    /// FormTrainTypePropertyクラス
    /// </summary>
    public partial class FormTrainTypeProperty : Form
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
        public delegate void UpdateEventHandler(object sender, TrainTypePropertyUpdateEventArgs e);

        /// <summary>
        /// 更新 event
        /// </summary>
        public event UpdateEventHandler OnUpdate = delegate { };
        #endregion

        /// <summary>
        /// TrainTypePropertyオブジェクト生成
        /// </summary>
        public TrainTypeProperty Property { get; set; } = new TrainTypeProperty();

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public FormTrainTypeProperty(TrainTypeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainTypeProperty::FormTrainTypeProperty(TrainTypeProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コンポーネント初期化
            InitializeComponent();

            // 設定
            Property.Copy(property);

            // ロギング
            Logger.Debug("<<<<= FormTrainTypeProperty::FormTrainTypeProperty(TrainTypeProperty)");
        }
        #endregion

        #region イベント
        #region FormTrainTypePropertyイベント
        /// <summary>
        /// FormTrainTypeProperty_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormTrainTypeProperty_Load(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainTypeProperty::FormTrainTypeProperty_Load(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 変換
            PropertyToControl();

            // コントコール設定
            tableLayoutPanelMain.Dock = DockStyle.Fill;
            tableLayoutPanelTrainTypeName.Dock = DockStyle.Fill;
            labelTrainTypeName.Dock = DockStyle.Fill;
            textBoxTrainTypeName.Dock = DockStyle.Fill;
            labelAbbreviation.Dock = DockStyle.Fill;
            textBoxAbbreviation.Dock = DockStyle.Fill;
            tableLayoutPanelStringColor.Dock = DockStyle.Fill;
            labelStringColor.Dock = DockStyle.Fill;
            panelStringColor.Dock = DockStyle.Fill;
            buttonStringColorChange.Dock = DockStyle.Fill;
            groupBoxTimeTableFont.Dock = DockStyle.Fill;
            tableLayoutPanelTimeTableFont.Dock = DockStyle.Fill;
            panelTimeTableFont.Dock = DockStyle.Fill;
            labelTimeTableFont.Dock = DockStyle.Fill;
            groupBoxDiagramFont.Dock = DockStyle.Fill;
            tableLayoutPanelDiagramFont.Dock = DockStyle.Fill;
            panelDiagramLine.Dock = DockStyle.Fill;
            buttonDiagramLineChangeColor.Dock = DockStyle.Fill;
            labelDiagramLineStyle.Dock = DockStyle.Fill;
            comboBoxDiagramLineStyle.Dock = DockStyle.Fill;
            checkBoxDiagramLineBold.Dock = DockStyle.Fill;
            labelStopStationClearlyIndicated.Dock = DockStyle.Fill;
            comboBoxStopStationClearlyIndicated.Dock = DockStyle.Fill;
            tableLayoutPanelButton.Dock = DockStyle.Fill;
            buttonOK.Dock = DockStyle.Fill;
            buttonCancel.Dock = DockStyle.Fill;

            // ロギング
            Logger.Debug("<<<<= FormTrainTypeProperty::FormTrainTypeProperty_Load(object, EventArgs)");
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
            Logger.Debug("=>>>> FormTrainTypeProperty::buttonOK_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 入力チェック
            if (!InputCheck())
            {
                // ロギング
                Logger.Debug("<<<<= FormTrainTypeProperty::buttonOK_Click(object, EventArgs)");

                // 入力エラー
                return;
            }

            // 変換
            ControlToProperty();

            // イベント呼出
            OnUpdate(this, new TrainTypePropertyUpdateEventArgs() { Property = Property });

            // 正常終了
            DialogResult = DialogResult.OK;

            // ロギング
            Logger.Debug("<<<<= FormTrainTypeProperty::buttonOK_Click(object, EventArgs)");
        }

        /// <summary>
        /// buttonCancel_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainTypeProperty::buttonCancel_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // キャンセル
            DialogResult = DialogResult.Cancel;

            // ロギング
            Logger.Debug("<<<<= FormTrainTypeProperty::buttonCancel_Click(object, EventArgs)");
        }

        /// <summary>
        /// buttonStringColorChange_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStringColorChange_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainTypeProperty::buttonStringColorChange_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= FormTrainTypeProperty::buttonStringColorChange_Click(object, EventArgs)");
        }

        /// <summary>
        /// buttonDiagramLineChangeColor_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDiagramLineChangeColor_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainTypeProperty::buttonDiagramLineChangeColor_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= FormTrainTypeProperty::buttonDiagramLineChangeColor_Click(object, EventArgs)");
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
            Logger.Debug("=>>>> FormTrainTypeProperty::InputCheck()");

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= FormTrainTypeProperty::InputCheck()");

            // 正常終了
            return true;
        }

        /// <summary>
        /// プロパティ⇒コントコール変換
        /// </summary>
        private void PropertyToControl()
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainTypeProperty::PropertyToControl()");

            // TODO:未実装
            textBoxTrainTypeName.Text = Property.Name;

            // ロギング
            Logger.Debug("<<<<= FormTrainTypeProperty::PropertyToControl()");
        }

        /// <summary>
        /// コントコール⇒プロパティ変換
        /// </summary>
        /// <returns></returns>
        private void ControlToProperty()
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainTypeProperty::ControlToProperty()");

            // TODO:未実装
            Property.Name = textBoxTrainTypeName.Text;

            // ロギング
            Logger.Debug("<<<<= FormTrainTypeProperty::ControlToProperty()");
        }
        #endregion
    }
}
