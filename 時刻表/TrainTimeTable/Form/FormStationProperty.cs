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
        /// StationPropertyオブジェクト
        /// </summary>
        public StationProperty Property { get; set; } = null;

        /// <summary>
        /// StationPropertiesオブジェクト
        /// </summary>
        private StationProperties m_StationProperties { get; set; } = null;

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
        public FormStationProperty()
        {
            // ロギング
            Logger.Debug("=>>>> FormStationProperty::FormStationProperty()");

            // コンポーネント初期化
            InitializeComponent();

            // 設定
            Property = new StationProperty()
            {
                TimeFormat = TimeFormat.DepartureTime,
                StationScale = StationScale.GeneralStation,
            };

            // ロギング
            Logger.Debug("<<<<= FormStationProperty::FormStationProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public FormStationProperty(StationProperty property)
            : this()
        {
            // ロギング
            Logger.Debug("=>>>> FormStationProperty::FormStationProperty(StationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 設定
            Property.Copy(property);

            // ロギング
            Logger.Debug("<<<<= FormStationProperty::FormStationProperty(StationProperty)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public FormStationProperty(StationProperties stations)
            : this()
        {
            // ロギング
            Logger.Debug("=>>>> FormStationProperty::FormStationProperty(StationProperties)");
            Logger.DebugFormat("stations:[{0}]", stations);

            // 設定
            m_StationProperties = stations;

            // ロギング
            Logger.Debug("<<<<= FormStationProperty::FormStationProperty(StationProperties)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="stations"></param>
        /// <param name="property"></param>
        public FormStationProperty(StationProperties stations, StationProperty property)
            : this(stations)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationProperty::FormStationProperty(StationProperties, StationProperty)");
            Logger.DebugFormat("stations:[{0}]", stations);
            Logger.DebugFormat("property:[{0}]", property);

            // 設定
            Property.Copy(property);

            // ロギング
            Logger.Debug("<<<<= FormStationProperty::FormStationProperty(StationProperties, StationProperty)");
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

            // 入力判定
            if (textBoxStationName.Text == string.Empty)
            {
                // メッセージ表示
                MessageBox.Show("駅名が指定されていません", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // 異常終了
                return false;
            }

            // 同一名登録判定
            if (m_StationProperties?.Find(t => t.Name == textBoxStationName.Text) != null)
            {
                // エラーメッセージ
                MessageBox.Show(string.Format("既に登録されている駅名は使用できません:[{0}]", textBoxStationName.Text), "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // 異常終了
                return false;
            }
            
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

            // 設定
            textBoxStationName.Text = Property.Name;
            switch (Property.StationScale)
            {
                case StationScale.GeneralStation:
                    radioButtonStationScaleGeneral.Checked = true;
                    break;
                case StationScale.MainStation:
                    radioButtonStationScaleMain.Checked = true;
                    break;
                default:
                    throw new AggregateException(string.Format("駅規模の異常を検出しました:[{0}]", Property.StationScale));
            }
            switch(Property.TimeFormat)
            {
                case TimeFormat.DepartureTime:
                    radioButtonTimeFormatDepartureTime.Checked = true;
                    break;
                case TimeFormat.DepartureAndArrival:
                    radioButtonTimeFormatDepartureAndArrival.Checked = true;
                    break;
                case TimeFormat.OutboundArrivalTime:
                    radioButtonTimeFormatOutboundArrivalTime.Checked = true;
                    break;
                case TimeFormat.InboundArrivalTime:
                    radioButtonTimeFormatInboundArrivalTime.Checked = true;
                    break;
                case TimeFormat.OutboundArrivalAndDeparture:
                    radioButtonTimeFormatOutboundArrivalAndDeparture.Checked = true;
                    break;
                case TimeFormat.InboundDepartureAndArrival:
                    radioButtonTimeFormatInboundDepartureAndArrival.Checked = true;
                    break;
                default:
                    throw new AggregateException(string.Format("時刻形式の異常を検出しました:[{0}]", Property.TimeFormat));
            }
            m_ComboBoxDiagramTrainInformations[DirectionType.Outbound].SetSelected(Property.DiagramTrainInformations[DirectionType.Outbound]);
            m_ComboBoxDiagramTrainInformations[DirectionType.Inbound].SetSelected(Property.DiagramTrainInformations[DirectionType.Inbound]);
            checkBoxBorderLine.Checked = Property.Border;

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

            // 設定
            Property.Name = textBoxStationName.Text;
            if (radioButtonStationScaleMain.Checked)
            {
                Property.StationScale = StationScale.MainStation;
            }
            else
            {
                Property.StationScale = StationScale.GeneralStation;
            }
            if (radioButtonTimeFormatDepartureAndArrival.Checked)
            {
                Property.TimeFormat = TimeFormat.DepartureAndArrival;
            }
            else if (radioButtonTimeFormatOutboundArrivalTime.Checked)
            {
                Property.TimeFormat = TimeFormat.OutboundArrivalTime;
            }
            else if (radioButtonTimeFormatInboundArrivalTime.Checked)
            {
                Property.TimeFormat = TimeFormat.InboundArrivalTime;
            }
            else if (radioButtonTimeFormatOutboundArrivalAndDeparture.Checked)
            {
                Property.TimeFormat = TimeFormat.OutboundArrivalAndDeparture;
            }
            else if (radioButtonTimeFormatInboundDepartureAndArrival.Checked)
            {
                Property.TimeFormat = TimeFormat.InboundDepartureAndArrival;
            }
            else if (radioButtonTimeFormatDepartureTime.Checked)
            {
                Property.TimeFormat = TimeFormat.DepartureTime;
            }
            Property.DiagramTrainInformations[DirectionType.Outbound] = m_ComboBoxDiagramTrainInformations[DirectionType.Outbound].GetSelected();
            Property.DiagramTrainInformations[DirectionType.Inbound] = m_ComboBoxDiagramTrainInformations[DirectionType.Inbound].GetSelected();
            Property.Border = checkBoxBorderLine.Checked;

            // ロギング
            Logger.Debug("<<<<= FormStationProperty::ControlToProperty()");
        }
        #endregion
    }
}
