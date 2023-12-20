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
        /// TrainTypePropertyオブジェクト
        /// </summary>
        public TrainTypeProperty Property { get; set; } = new TrainTypeProperty();

        /// <summary>
        /// PanelLineオブジェクト
        /// </summary>
        private PanelLine m_PanelLine = new PanelLine();

        /// <summary>
        /// ComboBoxFontsオブジェクト
        /// </summary>
        private ComboBoxFonts m_ComboBoxFonts = null;

        /// <summary>
        /// ComboBoxLineStyleオブジェクト
        /// </summary>
        private ComboBoxLineStyle m_ComboBoxLineStyle = new ComboBoxLineStyle();

        /// <summary>
        /// ComboBoxStopStationClearlyIndicatedオブジェクト
        /// </summary>
        private ComboBoxStopStationClearlyIndicated m_ComboBoxStopStationClearlyIndicated = new ComboBoxStopStationClearlyIndicated();

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fonts"></param>
        /// <param name="property"></param>
        public FormTrainTypeProperty(FontProperties fonts, TrainTypeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainTypeProperty::FormTrainTypeProperty(FontProperties, TrainTypeProperty)");
            Logger.DebugFormat("fonts   :[{0}]", fonts);
            Logger.DebugFormat("property:[{0}]", property);

            // コンポーネント初期化
            InitializeComponent();

            // 設定
            m_ComboBoxFonts = new ComboBoxFonts(fonts);
            Property.Copy(property);

            // ロギング
            Logger.Debug("<<<<= FormTrainTypeProperty::FormTrainTypeProperty(FontProperties, TrainTypeProperty)");
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
            tableLayoutPanelTimeTableFont.Controls.Add(m_ComboBoxFonts, 2, 0);
            m_ComboBoxFonts.Dock = DockStyle.Fill;
            labelTimeTableFontValue.Dock = DockStyle.Fill;
            labelTimeTableFont.Dock = DockStyle.Fill;
            groupBoxDiagramFont.Dock = DockStyle.Fill;
            tableLayoutPanelDiagramFont.Dock = DockStyle.Fill;
            tableLayoutPanelDiagramFont.Controls.Add(m_PanelLine, 0, 0);
            m_PanelLine.Dock = DockStyle.Fill;
            buttonDiagramLineChangeColor.Dock = DockStyle.Fill;
            labelDiagramLineStyle.Dock = DockStyle.Fill;
            checkBoxDiagramLineBold.Dock = DockStyle.Fill;
            labelStopStationClearlyIndicated.Dock = DockStyle.Fill;
            m_ComboBoxLineStyle.Dock = DockStyle.Fill;
            tableLayoutPanelDiagramFont.Controls.Add(m_ComboBoxLineStyle, 1, 1);
            tableLayoutPanelDiagramFont.SetColumnSpan(m_ComboBoxLineStyle, 2);
            m_ComboBoxLineStyle.SelectedIndexChanged += ComboBoxLineStyle_SelectedIndexChanged;
            m_ComboBoxStopStationClearlyIndicated.Dock = DockStyle.Fill;
            tableLayoutPanelDiagramFont.Controls.Add(m_ComboBoxStopStationClearlyIndicated, 1, 2);
            tableLayoutPanelDiagramFont.SetColumnSpan(m_ComboBoxStopStationClearlyIndicated, 2);
            tableLayoutPanelButton.Dock = DockStyle.Fill;
            buttonOK.Dock = DockStyle.Fill;
            buttonCancel.Dock = DockStyle.Fill;

            // ロギング
            Logger.Debug("<<<<= FormTrainTypeProperty::FormTrainTypeProperty_Load(object, EventArgs)");
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

            // ColorDialogオブジェクト生成
            ColorDialog colorDialog = new ColorDialog();

            // 初期設定
            colorDialog.Color = panelStringColor.BackColor;

            // ColorDialog表示
            DialogResult dialogResult = colorDialog.ShowDialog();

            // ColorDialog表示結果判定
            if (dialogResult != DialogResult.OK)
            {
                // ロギング
                Logger.Debug("<<<<= FormTrainTypeProperty::buttonStringColorChange_Click(object, EventArgs)");

                // 何もしない
                return;
            }

            // 結果設定
            panelStringColor.BackColor = colorDialog.Color;

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

            // ColorDialogオブジェクト生成
            ColorDialog colorDialog = new ColorDialog();

            // 初期設定
            colorDialog.Color = m_PanelLine.GetColor();

            // ColorDialog表示
            DialogResult dialogResult = colorDialog.ShowDialog();

            // ColorDialog表示結果判定
            if (dialogResult != DialogResult.OK)
            {
                // ロギング
                Logger.Debug("<<<<= FormTrainTypeProperty::buttonDiagramLineChangeColor_Click(object, EventArgs)");

                // 何もしない
                return;
            }

            // 設定
            m_PanelLine.Bold = checkBoxDiagramLineBold.Checked;
            m_PanelLine.DashStyle = m_ComboBoxLineStyle.GetSelected();
            m_PanelLine.SetColor(colorDialog.Color);

            // ロギング
            Logger.Debug("<<<<= FormTrainTypeProperty::buttonDiagramLineChangeColor_Click(object, EventArgs)");
        }
        #endregion

        #region ComboBoxイベント
        /// <summary>
        /// ComboBoxLineStyle_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxLineStyle_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainTypeProperty::ComboBoxLineStyle_SelectedIndexChanged(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 設定
            m_PanelLine.Bold = checkBoxDiagramLineBold.Checked;
            m_PanelLine.DashStyle = m_ComboBoxLineStyle.GetSelected();
            m_PanelLine.Refresh();

            // ロギング
            Logger.Debug("<<<<= FormTrainTypeProperty::ComboBoxLineStyle_SelectedIndexChanged(object, EventArgs)");
        }
        #endregion

        #region CheckBoxイベント
        /// <summary>
        /// checkBoxDiagramLineBold_CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxDiagramLineBold_CheckedChanged(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormTrainTypeProperty::checkBoxDiagramLineBold_CheckedChanged(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 設定
            m_PanelLine.Bold = checkBoxDiagramLineBold.Checked;
            m_PanelLine.DashStyle = m_ComboBoxLineStyle.GetSelected();
            m_PanelLine.Refresh();

            // ロギング
            Logger.Debug("<<<<= FormTrainTypeProperty::checkBoxDiagramLineBold_CheckedChanged(object, EventArgs)");
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

            // 設定
            textBoxTrainTypeName.Text = Property.Name;
            textBoxAbbreviation.Text = Property.Abbreviation;
            panelStringColor.BackColor = Property.StringsColor;
            m_ComboBoxFonts.SetSelected(Property.TimetableFontName);
            m_ComboBoxLineStyle.SetSelected(Property.DiagramLineStyle);
            m_PanelLine.DashStyle = Property.DiagramLineStyle;
            m_PanelLine.SetColor(Property.DiagramLineColor);
            checkBoxDiagramLineBold.Checked = Property.DiagramLineBold;
            m_ComboBoxStopStationClearlyIndicated.SetSelected(Property.StopStationClearlyIndicated);

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

            // 設定
            Property.Name = textBoxTrainTypeName.Text;
            Property.Abbreviation = textBoxAbbreviation.Text;
            Property.TimetableFontName = m_ComboBoxFonts.GetSelected();
            Property.StringsColor = panelStringColor.BackColor;
            Property.DiagramLineStyle = m_PanelLine.DashStyle;
            Property.DiagramLineColor = m_PanelLine.GetColor();
            Property.DiagramLineBold = checkBoxDiagramLineBold.Checked;
            Property.StopStationClearlyIndicated = m_ComboBoxStopStationClearlyIndicated.GetSelected();

            // ロギング
            Logger.Debug("<<<<= FormTrainTypeProperty::ControlToProperty()");
        }
        #endregion
    }
}
