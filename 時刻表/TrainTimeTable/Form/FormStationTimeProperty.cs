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
using TrainTimeTable.EventArgs;
using TrainTimeTable.Property;
using static System.Net.Mime.MediaTypeNames;

namespace TrainTimeTable
{
    /// <summary>
    /// FormStationTimePropertyクラス
    /// </summary>
    public partial class FormStationTimeProperty : Form
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
        public delegate void UpdateEventHandler(object sender, StationTimePropertyUpdateEventArgs e);

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
        /// StationTimePropertyオブジェクト
        /// </summary>
        public StationTimeProperty Property { get; set; } = new StationTimeProperty();

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        /// <param name="stationTime"></param>
        public FormStationTimeProperty(RouteFileProperty property, StationTimeProperty stationTime)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationTimeProperty::FormStationTimeProperty(RouteFileProperty, StationTimeProperty)");
            Logger.DebugFormat("property   :[{0}]", property);
            Logger.DebugFormat("stationTime:[{0}]", stationTime);

            // コンポーネント初期化
            InitializeComponent();

            // 設定
            TrainProperty trainProperty = property.Diagrams.Find(d => d.Name == stationTime.DiagramName).Trains[stationTime.Direction].Find(t => t.Id == stationTime.TrainId);
            Text = string.Format("駅時刻：[{0} - {1} {2}]", trainProperty?.No, stationTime.StationName, stationTime.Direction.GetStringValue());
            m_RouteFileProperty = property;
            Property.Copy(stationTime);

            // ロギング
            Logger.Debug("<<<<= FormStationTimeProperty::FormStationTimeProperty(RouteFileProperty, StationTimeProperty)");
        }
        #endregion

        #region イベント
        #region FormStationTimePropertyイベント
        private void FormStationTimeProperty_Load(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationTimeProperty::FormStationTimeProperty_Load(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 描画一時停止
            SuspendLayout();

            // 変換
            PropertyToControl();

            // コントコール設定
            tableLayoutPanelMain.Dock = DockStyle.Fill;
            tableLayoutPanelStationTime.Dock = DockStyle.Fill;
            groupBoxStationTreatment.Dock = DockStyle.Fill;
            tableLayoutPanelStationTreatment.Dock = DockStyle.Fill;
            maskedTextBoxArrivalTime.Dock = DockStyle.Fill;
            maskedTextBoxDepartureTime.Dock = DockStyle.Fill;
            tableLayoutPanelButton.Dock = DockStyle.Fill;
            buttonOK.Dock = DockStyle.Fill;
            buttonCancel.Dock = DockStyle.Fill;

            // 描画再開
            ResumeLayout();

            // ロギング
            Logger.Debug("<<<<= FormStationTimeProperty::FormStationTimeProperty_Load(object, EventArgs)");
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
            Logger.Debug("=>>>> FormStationTimeProperty::buttonOK_Click(object, EventArgs)");
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
            OnUpdate(this, new StationTimePropertyUpdateEventArgs() { Property = Property });

            // 正常終了
            DialogResult = DialogResult.OK;

            // ロギング
            Logger.Debug("<<<<= FormStationTimeProperty::buttonOK_Click(object, EventArgs)");
        }

        /// <summary>
        /// buttonCancel_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormStationTimeProperty::buttonCancel_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // キャンセル
            DialogResult = DialogResult.Cancel;

            // ロギング
            Logger.Debug("<<<<= FormStationTimeProperty::buttonCancel_Click(object, EventArgs)");
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
            Logger.Debug("=>>>> FormStationTimeProperty::InputCheck()");

            DateTime tmp = DateTime.Now;

            // 時刻形式判定
            if (maskedTextBoxArrivalTime.Text.Replace(":", "").Trim() != string.Empty)
            {
                // 時刻文字列DateTime変換
                bool result = DateTime.TryParseExact(maskedTextBoxArrivalTime.Text, "HH:mm",null, System.Globalization.DateTimeStyles.None, out tmp);

                // 結果判定
                if (!result)
                {
                    // メッセージ表示
                    MessageBox.Show(string.Format("着時刻の形式が誤っています:[{0}]", maskedTextBoxArrivalTime.Text), AssemblyLibrary.GetTitleVersion(), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // 異常終了
                    return false;
                }
            }
            // 時刻形式判定
            if (maskedTextBoxDepartureTime.Text.Replace(":", "").Trim() != string.Empty)
            {
                // 時刻文字列DateTime変換
                bool result = DateTime.TryParseExact(maskedTextBoxDepartureTime.Text, "HH:mm", null, System.Globalization.DateTimeStyles.None, out tmp);

                // 結果判定
                if (!result)
                {
                    // メッセージ表示
                    MessageBox.Show(string.Format("発時刻の形式が誤っています:[{0}]", maskedTextBoxDepartureTime.Text), AssemblyLibrary.GetTitleVersion(), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // 異常終了
                    return false;
                }
            }

            // ロギング
            Logger.Debug("<<<<= FormStationTimeProperty::InputCheck()");

            // 正常終了
            return true;
        }

        /// <summary>
        /// プロパティ⇒コントコール変換
        /// </summary>
        private void PropertyToControl()
        {
            // ロギング
            Logger.Debug("=>>>> FormStationTimeProperty::ControlToProperty()");

            // 設定
            switch(Property.StationTreatment)
            {
                case StationTreatment.NoService:
                    radioButtonNoService.Checked = true;
                    break;
                case StationTreatment.Stop:
                    radioButtonStop.Checked = true;
                    break;
                case StationTreatment.Passing:
                    radioButtonPassing.Checked = true;
                    break;
                case StationTreatment.NoRoute:
                    radioButtonNoRoute.Checked = true;
                    break;
                default:
                    // 例外
                    throw new AggregateException(string.Format("駅扱いの異常を検出しました:[{0}]", Property.StationTreatment));
            }
            if (Property.ArrivalTime != string.Empty)
            {
                maskedTextBoxArrivalTime.Text = Property.ArrivalTime;
            }
            if (Property.DepartureTime != string.Empty)
            {
                maskedTextBoxDepartureTime.Text = Property.DepartureTime;
            }

            // ロギング
            Logger.Debug("<<<<= FormStationTimeProperty::ControlToProperty()");
        }

        /// <summary>
        /// コントコール⇒プロパティ変換
        /// </summary>
        /// <returns></returns>
        private void ControlToProperty()
        {
            // ロギング
            Logger.Debug("=>>>> FormStationTimeProperty::ControlToProperty()");

            // 設定
            if (radioButtonNoService.Checked)
            {
                Property.StationTreatment = StationTreatment.NoService;
            }
            else if (radioButtonStop.Checked)
            {
                Property.StationTreatment = StationTreatment.Stop;
            }
            else if (radioButtonPassing.Checked)
            {
                Property.StationTreatment = StationTreatment.Passing;
            }
            else
            {
                Property.StationTreatment = StationTreatment.NoRoute;
            }
            Property.ArrivalTime = maskedTextBoxArrivalTime.Text.Replace(":", "").Trim();
            Property.DepartureTime = maskedTextBoxDepartureTime.Text.Replace(":", "").Trim();

            // ロギング
            Logger.DebugFormat("Property:[{0}]", Property);
            Logger.Debug("<<<<= FormStationTimeProperty::ControlToProperty()");
        }
        #endregion
    }
}
