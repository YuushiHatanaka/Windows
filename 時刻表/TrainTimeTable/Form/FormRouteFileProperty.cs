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
using TrainTimeTable.Dialog;
using TrainTimeTable.EventArgs;
using TrainTimeTable.Property;

namespace TrainTimeTable
{
    /// <summary>
    /// FormRouteFilePropertyクラス
    /// </summary>
    public partial class FormRouteFileProperty : Form
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
        public delegate void UpdateEventHandler(object sender, RouteFilePropertyUpdateEventArgs e);

        /// <summary>
        /// 更新 event
        /// </summary>
        public event UpdateEventHandler OnUpdate = delegate { };
        #endregion

        /// <summary>
        /// RouteFilePropertyオブジェクト生成
        /// </summary>
        public RouteFileProperty Property { get; set; } = new RouteFileProperty();

        /// <summary>
        /// ダイアグラム名
        /// </summary>
        public string DiagramName { get; } = string.Empty;

        /// <summary>
        /// FontListBoxオブジェクト
        /// </summary>
        private ListBoxFont m_FontListBox = null;

        /// <summary>
        /// ColorListBoxオブジェクト
        /// </summary>
        private ListBoxColor m_ColorListBox = null;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public FormRouteFileProperty(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormRouteFileProperty::FormRouteFileProperty(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property.ToString());

            // コンポーネント初期化
            InitializeComponent();

            // 設定
            Property.Copy(property);

            // ロギング
            Logger.Debug("<<<<= FormRouteFileProperty::FormRouteFileProperty(RouteFileProperty)");
        }
        #endregion

        #region イベント
        #region FormRouteFilePropertyイベント
        /// <summary>
        /// FormRouteFileProperty_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormRouteFileProperty_Load(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRouteFileProperty::FormRouteFileProperty_Load(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // オブジェクト生成
            m_FontListBox = new ListBoxFont(Property.Fonts);
            m_FontListBox.SelectedIndexChanged += FontListBox_SelectedIndexChanged;
            if (Property.Fonts.Count > 0)
            {
                m_FontListBox.SelectedIndex = 0;
            }
            m_ColorListBox = new ListBoxColor(Property.Colors);
            m_ColorListBox.SelectedIndexChanged += ColorListBox_SelectedIndexChanged;
            if(Property.Colors.Count > 0)
            {
                m_ColorListBox.SelectedIndex = 0;
            }

            // 変換
            PropertyToControl();

            // コントコール設定
            tableLayoutPanelMain.Dock = DockStyle.Fill;
            tabControlMain.Dock = DockStyle.Fill;
            #region 路線
            tableLayoutPanelRoute.Dock = DockStyle.Fill;
            textBoxRouteName.Dock = DockStyle.Fill;
            textBoxOutboundDiagramName.Dock = DockStyle.Fill;
            textBoxInboundDiagramName.Dock = DockStyle.Fill;
            #endregion
            #region フォント・色
            tableLayoutPanelFontColor.Dock = DockStyle.Fill;
            #region フォント
            labellabelFontSettingsConfirmation.Dock = DockStyle.Fill;
            tableLayoutPanelFontColor.Controls.Add(m_FontListBox, 0, 1);
            m_FontListBox.Dock = DockStyle.Fill;
            tableLayoutPanelFontSettingsButton.Dock = DockStyle.Fill;
            buttonFontSettings.Dock = DockStyle.Fill;
            #endregion
            #region 色
            panelColorSettingConfirmation.Dock = DockStyle.Fill;
            tableLayoutPanelFontColor.Controls.Add(m_ColorListBox, 0, 5);
            m_ColorListBox.Dock = DockStyle.Fill;
            tableLayoutPanelColorSettingButton.Dock = DockStyle.Fill;
            buttonColorSetting.Dock = DockStyle.Fill;
            #endregion
            #endregion
            #region 時刻表画面
            tableLayoutPanelTimetableDisplay.Dock = DockStyle.Fill;
            numericUpDownWidthOfStationNameField.Dock = DockStyle.Fill;
            numericUpDownTimetableTrainWidth.Dock = DockStyle.Fill;
            #endregion
            #region ダイヤグラム画面
            tableLayoutPanelDiagramDisplay.Dock = DockStyle.Fill;
            dateTimePickerDiagramDtartingTime.Dock = DockStyle.Fill;
            numericUpDownStandardWidthBetweenStationsInTheDiagram.Dock = DockStyle.Fill;
            #endregion
            #region ボタン
            tableLayoutPanelButton.Dock = DockStyle.Fill;
            buttonOK.Dock = DockStyle.Fill;
            buttonCancel.Dock = DockStyle.None;
            #endregion

            // ロギング
            Logger.Debug("<<<<= FormRouteFileProperty::FormRouteFileProperty_Load(object, EventArgs)");
        }
        #endregion

        #region ListBoxイベント
        /// <summary>
        /// FontListBox_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FontListBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRouteFileProperty::FontListBox_SelectedIndexChanged(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択中フォント取得
            Font font = GetSelectFont();

            // フォント設定
            labellabelFontSettingsConfirmation.Font = font;

            // ロギング
            Logger.Debug("<<<<= FormRouteFileProperty::FontListBox_SelectedIndexChanged(object, EventArgs)");
        }

        /// <summary>
        /// ColorListBox_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorListBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRouteFileProperty::ColorListBox_SelectedIndexChanged(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択中Colorオブジェクト取得
            Color color = GetSelectColor();

            // 色設定
            panelColorSettingConfirmation.BackColor = color;

            // ロギング
            Logger.Debug("<<<<= FormRouteFileProperty::ColorListBox_SelectedIndexChanged(object, EventArgs)");
        }
        #endregion

        #region Buttonイベント
        /// <summary>
        /// buttonFontSettings_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonFontSettings_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRouteFileProperty::buttonFontSettings_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // Get the currently selected item in the ListBox.
            if (m_FontListBox.SelectedItem == null)
            {
                // メッセージ表示
                MessageBox.Show("変更する項目を選択してください。", "未選択エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // ロギング
                Logger.Warn("変更フォント未選択エラー");
                Logger.Debug("<<<<= FormRouteFileProperty::buttonFontSettings_Click(object, EventArgs)");

                // 何もしない
                return;
            }

            // 選択アイテム取得
            string curItem = m_FontListBox.SelectedItem.ToString();

            // FontPropertyオブジェクト取得
            FontProperty fontProperty = Property.Fonts.GetProperty(curItem);

            // DialogFontオブジェクト生成
            DialogFont dialogFont = new DialogFont(curItem, fontProperty);

            // フォント選択ダイアログ表示
            if (dialogFont.ShowDialog() != DialogResult.OK)
            {
                // ロギング
                Logger.Debug("<<<<= FormRouteFileProperty::buttonFontSettings_Click(object, EventArgs)");

                // 何もしない
                return;
            }

            // 設定フォント取得
            Font font = dialogFont.GetFont();

            // 色設定
            labellabelFontSettingsConfirmation.Font = font;

            // 設定
            Property.Fonts[curItem].Update(font);

            // ロギング
            Logger.Debug("<<<<= FormRouteFileProperty::buttonFontSettings_Click(object, EventArgs)");
        }

        /// <summary>
        /// buttonColorSetting_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonColorSetting_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRouteFileProperty::buttonColorSetting_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // Get the currently selected item in the ListBox.
            if (m_ColorListBox.SelectedItem == null)
            {
                // メッセージ表示
                MessageBox.Show("変更する項目を選択してください。", "未選択エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // ロギング
                Logger.Warn("変更色未選択エラー");
                Logger.Debug("<<<<= FormRouteFileProperty::buttonColorSetting_Click(object, EventArgs)");

                // 何もしない
                return;
            }

            // 選択アイテム取得
            string curItem = m_ColorListBox.SelectedItem.ToString();

            // ColorDialogオブジェクト生成
            ColorDialog colorDialog = new ColorDialog();

            // 設定
            colorDialog.Color = Property.Colors[curItem].Value;

            // 色選択ダイアログ表示
            if (colorDialog.ShowDialog() != DialogResult.OK)
            {
                // ロギング
                Logger.Debug("<<<<= FormRouteFileProperty::buttonColorSetting_Click(object, EventArgs)");

                // 何もしない
                return;
            }

            // 色設定
            panelColorSettingConfirmation.BackColor = colorDialog.Color;

            // 設定
            Property.Colors[curItem].Value = colorDialog.Color;

            // ロギング
            Logger.Debug("<<<<= FormRouteFileProperty::buttonColorSetting_Click(object, EventArgs)");
        }

        /// <summary>
        /// buttonOK_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRouteFileProperty::buttonOK_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 入力チェック
            if (!InputCheck())
            {
                // ロギング
                Logger.Debug("<<<<= FormRouteFileProperty::buttonOK_Click(object, EventArgs)");

                // 入力エラー
                return;
            }

            // 変換
            ControlToProperty();

            // イベント呼出
            OnUpdate(this, new RouteFilePropertyUpdateEventArgs() { Property = Property });

            // 正常終了
            DialogResult = DialogResult.OK;

            // ロギング
            Logger.Debug("<<<<= FormRouteFileProperty::buttonOK_Click(object, EventArgs)");
        }

        /// <summary>
        /// buttonCancel_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormRouteFileProperty::buttonCancel_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // キャンセル
            DialogResult = DialogResult.Cancel;

            // ロギング
            Logger.Debug("<<<<= FormRouteFileProperty::buttonCancel_Click(object, EventArgs)");
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
            Logger.Debug("=>>>> FormRouteFileProperty::UpdateNotification(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 更新
            Update(property);

            // ロギング
            Logger.Debug("<<<<= FormRouteFileProperty::UpdateNotification(RouteFileProperty)");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        public void Update(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormRouteFileProperty::Update(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            Property.Copy(property);

            // 変換
            PropertyToControl();

            // ロギング
            Logger.Debug("<<<<= FormRouteFileProperty::Update(RouteFileProperty)");
        }

        /// <summary>
        /// 削除通知
        /// </summary>
        /// <param name="property"></param>
        public void RemoveNotification(DiagramProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormRouteFileProperty::RemoveNotification(DiagramProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // ダイアグラム名判定
            if (property.Name == DiagramName)
            {
                // フォームクローズ
                Close();
            }

            // ロギング
            Logger.Debug("<<<<= FormRouteFileProperty::RemoveNotification(RouteFileProperty)");
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
            Logger.Debug("=>>>> FormRouteFileProperty::InputCheck()");

            #region 路線
            // TODO:未実装
            #endregion
            #region フォント色
            // TODO:未実装
            #endregion
            #region 時刻表画面
            // TODO:未実装
            #endregion
            #region ダイヤグラム画面
            // TODO:未実装
            #endregion

            // ロギング
            Logger.Debug("<<<<= FormRouteFileProperty::InputCheck()");

            // 正常終了
            return true;
        }

        /// <summary>
        /// プロパティ⇒コントコール変換
        /// </summary>
        private void PropertyToControl()
        {
            // ロギング
            Logger.Debug("=>>>> FormRouteFileProperty::PropertyToControl()");

            #region 路線
            textBoxRouteName.Text = Property.Route.Name;
            textBoxOutboundDiagramName.Text = Property.Route.OutboundDiaName;
            textBoxInboundDiagramName.Text = Property.Route.InboundDiaName;
            #endregion
            #region フォント色
            if (m_FontListBox.SelectedItem != null)
            {
                labellabelFontSettingsConfirmation.Font = Property.Fonts[m_FontListBox.SelectedItem.ToString()].GetFont();
            }
            else
            {
                labellabelFontSettingsConfirmation.Font = new Font(Const.DefaultFontName, Const.DefaultFontSize);
            }
            if (m_ColorListBox.SelectedItem != null)
            {
                panelColorSettingConfirmation.BackColor = Property.Colors[m_ColorListBox.SelectedItem.ToString()].Value;
            }
            else
            {
                panelColorSettingConfirmation.BackColor = Const.DefaultBackColor;
            }
            #endregion
            #region 時刻表画面
            numericUpDownWidthOfStationNameField.Value = int.Parse(Property.Route.WidthOfStationNameField.ToString());
            numericUpDownTimetableTrainWidth.Value = int.Parse(Property.Route.TimetableTrainWidth.ToString());
            #endregion
            #region ダイヤグラム画面
            dateTimePickerDiagramDtartingTime.Value = DateTime.Parse(Property.DiagramScreen.DiagramDtartingTime.ToString());
            numericUpDownStandardWidthBetweenStationsInTheDiagram.Value = int.Parse(Property.DiagramScreen.StandardWidthBetweenStationsInTheDiagram.ToString());
            #endregion

            // ロギング
            Logger.Debug("<<<<= FormRouteFileProperty::PropertyToControl()");
        }

        /// <summary>
        /// コントコール⇒プロパティ変換
        /// </summary>
        /// <returns></returns>
        private void ControlToProperty()
        {
            // ロギング
            Logger.Debug("=>>>> FormRouteFileProperty::ControlToProperty()");

            #region 路線
            Property.Route.Name = textBoxRouteName.Text;
            Property.Route.OutboundDiaName = textBoxOutboundDiagramName.Text;
            Property.Route.InboundDiaName = textBoxInboundDiagramName.Text;
            #endregion
            #region フォント色
            // TODO:未実装
            #endregion
            #region 時刻表画面
            Property.Route.WidthOfStationNameField = (int)numericUpDownWidthOfStationNameField.Value;
            Property.Route.TimetableTrainWidth = (int)numericUpDownTimetableTrainWidth.Value;
            #endregion
            #region ダイヤグラム画面
            Property.DiagramScreen.DiagramDtartingTime = dateTimePickerDiagramDtartingTime.Value;
            Property.DiagramScreen.StandardWidthBetweenStationsInTheDiagram = (int)numericUpDownStandardWidthBetweenStationsInTheDiagram.Value;
            #endregion

            // ロギング
            Logger.Debug("<<<<= FormRouteFileProperty::ControlToProperty()");
        }

        /// <summary>
        /// 選択Fontオブジェクト取得
        /// </summary>
        /// <returns></returns>
        private Font GetSelectFont()
        {
            // ロギング
            Logger.Debug("=>>>> FormRouteFileProperty::GetSelectFont()");

            // 結果オブジェクト生成
            Font result = new Font("Meiryo UI", 9);

            // Get the currently selected item in the ListBox.
            if (m_FontListBox.SelectedItem != null)
            {
                // 選択アイテム取得
                string curItem = m_FontListBox.SelectedItem.ToString();

                // 選択フォント名を判定
                if (Property.Fonts.ContainsKey(curItem))
                {
                    // Fontオブジェクト設定
                    result = Property.Fonts[curItem].GetFont();
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= FormRouteFileProperty::GetSelectFont()");

            // 返却
            return result;
        }

        /// <summary>
        /// 選択Colorオブジェクト取得
        /// </summary>
        /// <returns></returns>
        private Color GetSelectColor()
        {
            // ロギング
            Logger.Debug("=>>>> FormRouteFileProperty::GetSelectColor()");

            // 結果オブジェクト生成
            Color result = ColorTranslator.FromHtml("#000000");

            // Get the currently selected item in the ListBox.
            if (m_ColorListBox.SelectedItem != null)
            {
                // 選択アイテム取得
                string curItem = m_ColorListBox.SelectedItem.ToString();

                // 選択フォント名を判定
                if (Property.Colors.ContainsKey(curItem))
                {
                    // Colorオブジェクト設定
                    result = Property.Colors[curItem].Value;
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result.ToString());
            Logger.Debug("<<<<= FormRouteFileProperty::GetSelectColor()");

            // 返却
            return result;
        }
        #endregion
    }
}
