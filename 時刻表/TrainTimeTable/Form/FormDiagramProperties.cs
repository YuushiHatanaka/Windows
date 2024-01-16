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
    /// FormDiagramPropertiesクラス
    /// </summary>
    public partial class FormDiagramProperties : Form
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
        public delegate void UpdateEventHandler(object sender, DiagramPropertiesUpdateEventArgs e);

        /// <summary>
        /// 更新 event
        /// </summary>
        public event UpdateEventHandler OnUpdate = delegate { };

        /// <summary>
        /// 削除 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void RemoveEventHandler(object sender, DiagramPropertiesRemoveEventArgs e);

        /// <summary>
        /// 削除 event
        /// </summary>
        public event RemoveEventHandler OnRemove = delegate { };
        #endregion

        /// <summary>
        /// RouteFilePropertyオブジェクト
        /// </summary>
        private RouteFileProperty m_RouteFileProperty = null;

        /// <summary>
        /// 旧RouteFilePropertyオブジェクト
        /// </summary>
        private RouteFileProperty m_OldRouteFileProperty = new RouteFileProperty();

        /// <summary>
        /// ListBoxDiagramオブジェクト
        /// </summary>
        private ListBoxDiagram m_ListBoxDiagram = null;

        /// <summary>
        /// ダイアグラム名
        /// </summary>
        public string DiagramName { get; private set; } = string.Empty;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public FormDiagramProperties(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperties::FormDiagramProperties(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コンポーネント初期化
            InitializeComponent();

            // 設定
            m_RouteFileProperty = property;
            m_OldRouteFileProperty.Copy(property);
            m_ListBoxDiagram = new ListBoxDiagram(property);

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperties::FormDiagramProperties(RouteFileProperty)");
        }
        #endregion

        #region イベント
        #region FormDiagramPropertiesイベント
        /// <summary>
        /// FormDiagramProperties_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormDiagramProperties_Load(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperties::FormDiagramProperties_Load(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // コントコール設定
            tableLayoutPanelMain.Dock = DockStyle.Fill;
            groupBoxMain.Dock = DockStyle.Fill;
            groupBoxMain.Controls.Add(m_ListBoxDiagram);
            m_ListBoxDiagram.SelectedIndexChanged += ListBoxDiagram_SelectedIndexChanged;
            m_ListBoxDiagram.Dock = DockStyle.Fill;
            tableLayoutPanelButton.Dock = DockStyle.Fill;
            buttonCreateNew.Dock = DockStyle.Fill;
            buttonProperty.Dock = DockStyle.Fill;
            buttonCopy.Dock = DockStyle.Fill;
            buttonRemove.Dock = DockStyle.Fill;
            buttonUp.Dock = DockStyle.Fill;
            buttonDown.Dock = DockStyle.Fill;
            buttonClose.Dock = DockStyle.Fill;

            // リストボックス選択反映
            ListBoxSelectedReflection();

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperties::FormDiagramProperties_Load(object, EventArgs)");
        }
        #endregion

        #region ListBoxDiagramイベント
        /// <summary>
        /// ListBoxDiagram_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxDiagram_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperties::ListBoxDiagram_SelectedIndexChanged(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // リストボックス選択反映
            ListBoxSelectedReflection();

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperties::ListBoxDiagram_SelectedIndexChanged(object, EventArgs)");
        }
        #endregion

        #region Buttonイベント
        /// <summary>
        /// buttonCreateNew_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreateNew_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperties::buttonCreateNew_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // FormDiaPropertyオブジェクト生成
            FormDiagramProperty formDiagramProperty = new FormDiagramProperty();

            // 結果判定
            if (formDiagramProperty.ShowDialog() != DialogResult.OK)
            {
                // ロギング
                Logger.Debug("<<<<= FormDiagramProperties::buttonCreateNew_Click(object, EventArgs)");

                // 何もしない
                return;
            }

            // 登録
            if (m_ListBoxDiagram.Exists(formDiagramProperty.Property.Name))
            {
                // エラーメッセージ作成
                string msg = string.Format(
                    "ダイヤ名 {0} は既に存在しています。\r\nダイヤ名は重複してはいけません。\r\n他の名前を指定してください。",
                    formDiagramProperty.Property.Name);

                // メッセージ表示
                MessageBox.Show(msg, AssemblyLibrary.GetTitleVersion(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 登録
            m_ListBoxDiagram.Items.Add(formDiagramProperty.Property);
            m_RouteFileProperty.RegistonDiagrams(formDiagramProperty.Property);

            // シーケンス番号更新
            m_ListBoxDiagram.UpdateSeq();

            // 更新通知
            OnUpdate(this, new DiagramPropertiesUpdateEventArgs() { Properties = m_ListBoxDiagram.ToArray() });

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperties::buttonCreateNew_Click(object, EventArgs)");
        }

        /// <summary>
        /// buttonProperty_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonProperty_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperties::buttonProperty_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択インデクス取得
            int selectedIndex = GetSelectedIndex();

            // 選択状態設定
            if (selectedIndex < 0)
            {
                // ロギング
                Logger.Debug("<<<<= FormDiagramProperties::buttonProperty_Click(object, EventArgs)");

                // 選択なし
                return;
            }

            // 選択項目取得
            DiagramProperty result = GetSelectedCondition();

            // FormDiaPropertyオブジェクト生成
            FormDiagramProperty formDiagramProperty = new FormDiagramProperty(result);

            // フォーム表示
            if (formDiagramProperty.ShowDialog() != DialogResult.OK)
            {
                // ロギング
                Logger.Debug("<<<<= FormDiagramProperties::buttonProperty_Click(object, EventArgs)");

                // キャンセルなので何もしない
                return;
            }

            // ダイアグラム名変更判定
            if (result.Name != formDiagramProperty.Property.Name)
            {
                // 登録
                if (m_ListBoxDiagram.Exists(formDiagramProperty.Property.Name))
                {
                    // エラーメッセージ作成
                    string msg = string.Format(
                        "ダイヤ名 {0} は既に存在しています。\r\nダイヤ名は重複してはいけません。\r\n他の名前を指定してください。",
                        formDiagramProperty.Property.Name);

                    // メッセージ表示
                    MessageBox.Show(msg, AssemblyLibrary.GetTitleVersion(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ダイアグラム名変更
                m_RouteFileProperty.ChangeDiagramName(result.Name, formDiagramProperty.Property.Name);
                formDiagramProperty.Property.ChangeDiagramName();

                // 更新
                m_ListBoxDiagram.Update(selectedIndex, formDiagramProperty.Property);

                // 更新通知
                OnUpdate(this, new DiagramPropertiesUpdateEventArgs() { Properties = m_ListBoxDiagram.ToArray() });
            }

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperties::buttonProperty_Click(object, EventArgs)");
        }

        /// <summary>
        /// buttonCopy_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCopy_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperties::buttonCopy_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択インデクス取得
            int selectedIndex = GetSelectedIndex();

            // 選択状態設定
            if (selectedIndex < 0)
            {
                // ロギング
                Logger.Debug("<<<<= FormDiagramProperties::buttonCopy_Click(object, EventArgs)");

                // 選択なし
                return;
            }

            // 選択項目取得
            DiagramProperty result = GetSelectedCondition();

            // FormDiaPropertyオブジェクト生成
            FormDiagramProperty formDiagramProperty = new FormDiagramProperty(result);

            // フォーム表示
            if (formDiagramProperty.ShowDialog() != DialogResult.OK)
            {
                // ロギング
                Logger.Debug("<<<<= FormDiagramProperties::buttonCopy_Click(object, EventArgs)");

                // キャンセルなので何もしない
                return;
            }

            // 登録
            if (m_ListBoxDiagram.Exists(formDiagramProperty.Property.Name))
            {
                // エラーメッセージ作成
                string msg = string.Format(
                    "ダイヤ名 {0} は既に存在しています。\r\nダイヤ名は重複してはいけません。\r\n他の名前を指定してください。",
                    formDiagramProperty.Property.Name);

                // メッセージ表示
                MessageBox.Show(msg, AssemblyLibrary.GetTitleVersion(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ダイアグラム名変更
            formDiagramProperty.Property.ChangeDiagramName();

            // 登録
            m_ListBoxDiagram.Items.Add(formDiagramProperty.Property);
            m_RouteFileProperty.RegistonDiagrams(formDiagramProperty.Property);

            // シーケンス番号更新
            m_ListBoxDiagram.UpdateSeq();

            // 更新通知
            OnUpdate(this, new DiagramPropertiesUpdateEventArgs() { Properties = m_ListBoxDiagram.ToArray() });

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperties::buttonCopy_Click(object, EventArgs)");
        }

        /// <summary>
        /// buttonRemove_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRemove_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperties::buttonRemove_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択インデクス取得
            int selectedIndex = GetSelectedIndex();

            // 選択状態設定
            if (selectedIndex < 0)
            {
                // ロギング
                Logger.Debug("<<<<= FormDiagramProperties::buttonCopy_Click(object, EventArgs)");

                // 選択なし
                return;
            }

            // 選択項目取得
            DiagramProperty result = GetSelectedCondition();

            // メッセージ表示
            DialogResult dialogResult = MessageBox.Show(
                string.Format("ダイヤ「{0}」を削除します。\r\nよろしいですか？", result.Name),
                "ダイヤの削除",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            // メッセージ表示結果判定
            if (dialogResult == DialogResult.OK)
            {
                // 削除
                m_ListBoxDiagram.Items.RemoveAt(selectedIndex);
                m_RouteFileProperty.RemoveDiagram(result.Name);

                // シーケンス番号更新
                m_ListBoxDiagram.UpdateSeq();

                // 旧データにコピー
                m_OldRouteFileProperty.Copy(m_RouteFileProperty);

                // 更新通知
                OnRemove(this, new DiagramPropertiesRemoveEventArgs() { Properties = new DiagramProperties() { result } });
            }

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperties::buttonRemove_Click(object, EventArgs)");
        }

        /// <summary>
        /// buttonUp_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUp_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperties::buttonUp_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 入れ替え
            FormDiagramProperties_KeysShiftAndUp();

            // シーケンス番号更新
            m_ListBoxDiagram.UpdateSeq();

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperties::buttonUp_Click(object, EventArgs)");
        }

        /// <summary>
        /// buttonDown_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDown_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperties::buttonDown_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 入れ替え
            FormDiagramProperties_KeysShiftAndDown();

            // シーケンス番号更新
            m_ListBoxDiagram.UpdateSeq();

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperties::buttonDown_Click(object, EventArgs)");
        }

        /// <summary>
        /// buttonClose_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClose_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperties::buttonClose_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 変更判定
            if (m_OldRouteFileProperty.Diagrams.Compare(m_ListBoxDiagram.ToArray()))
            {
                // 変更されていないのでキャンセル
                DialogResult = DialogResult.Cancel;

                // ロギング
                Logger.Debug("<<<<= FormDiagramProperties::buttonClose_Click(object, EventArgs)");
                return;
            }

            // シーケンス番号更新
            m_ListBoxDiagram.UpdateSeq();

            // 更新通知
            OnUpdate(this, new DiagramPropertiesUpdateEventArgs() { Properties = m_ListBoxDiagram.ToArray() });

            // 正常終了
            DialogResult = DialogResult.OK;

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperties::buttonClose_Click(object, EventArgs)");
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
            Logger.Debug("=>>>> FormDiagramProperties::UpdateNotification(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 設定
            m_OldRouteFileProperty.Copy(property);

            // リストボックス更新
            m_ListBoxDiagram.Update(property.Diagrams);

            // リストボックス選択反映
            ListBoxSelectedReflection();

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperties::UpdateNotification(RouteFileProperty)");
        }

        /// <summary>
        /// 削除通知
        /// </summary>
        /// <param name="property"></param>
        public void RemoveNotification(DiagramProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperties::RemoveNotification(DiagramProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // ダイアグラム名判定
            if (property.Name == DiagramName)
            {
                // フォームクローズ
                Close();
            }

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperties::RemoveNotification(RouteFileProperty)");
        }
        #endregion

        #region privateメソッド
        #region ListBoxメソッド
        /// <summary>
        ///リストボックス選択反映
        /// </summary>
        private void ListBoxSelectedReflection()
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperties::ListBoxSelectedReflection()");

            // 選択状態判定
            if (m_ListBoxDiagram.SelectedIndex == -1)
            {
                // 未選択状態の場合
                buttonProperty.Enabled = false;
                buttonCopy.Enabled = false;
                buttonRemove.Enabled = false;
                buttonUp.Enabled = false;
                buttonDown.Enabled = false;
            }
            else
            {
                // 選択状態の場合
                buttonProperty.Enabled = true;
                buttonCopy.Enabled = true;
                buttonRemove.Enabled = true;

                // 選択状態の場合
                if ((m_ListBoxDiagram.Items.Count == 0) || (m_ListBoxDiagram.Items.Count == 1))
                {
                    // リストボックスに登録がない、または1件の登録しかない場合
                    buttonUp.Enabled = false;
                    buttonDown.Enabled = false;
                }
                else
                {
                    // 選択状態判定
                    if (m_ListBoxDiagram.SelectedIndex == 0)
                    {
                        // 1件目が選択状態の場合
                        buttonUp.Enabled = false;
                        buttonDown.Enabled = true;
                    }
                    else if (m_ListBoxDiagram.SelectedIndex == m_ListBoxDiagram.Items.Count - 1)
                    {
                        // 最終項目が選択状態の場合
                        buttonUp.Enabled = true;
                        buttonDown.Enabled = false;
                    }
                    else
                    {
                        // 上記以外の場合
                        buttonUp.Enabled = true;
                        buttonDown.Enabled = true;
                    }
                }
            }

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperties::ListBoxSelectedReflection()");
        }

        /// <summary>
        /// 選択インデックス取得
        /// </summary>
        /// <returns></returns>
        public int GetSelectedIndex()
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperties::GetSelectedIndex()");

            // 選択状態設定
            if (m_ListBoxDiagram.Items.Count == 0)
            {
                // ロギング
                Logger.Debug("<<<<= FormDiagramProperties::GetSelectedIndex()");

                // 選択なし
                return -1;
            }

            // ロギング
            Logger.DebugFormat("SelectedIndices:[{0}]", m_ListBoxDiagram.SelectedIndex);
            Logger.Debug("<<<<= FormDiagramProperties::GetSelectedIndex()");

            // 返却
            return m_ListBoxDiagram.SelectedIndex;
        }

        /// <summary>
        /// 選択情報取得
        /// </summary>
        /// <returns></returns>
        public DiagramProperty GetSelectedCondition()
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperties::GetSelectedCondition()");

            // 選択状態設定
            if (m_ListBoxDiagram.Items.Count == 0)
            {
                // ロギング
                Logger.Debug("<<<<= FormDiagramProperties::GetSelectedCondition()");

                // 選択なし
                return null;
            }

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperties::GetSelectedCondition()");

            // 返却
            return (DiagramProperty)m_ListBoxDiagram.Items[m_ListBoxDiagram.SelectedIndex];
        }

        /// <summary>
        /// FormDiagramProperties_KeysShiftAndUp
        /// </summary>
        private void FormDiagramProperties_KeysShiftAndUp()
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperties::FormDiagramProperties_KeysShiftAndUp()");

            // 選択インデクス取得
            int selectedIndex = GetSelectedIndex();

            // 選択状態設定
            if (selectedIndex < 0)
            {
                // ロギング
                Logger.Debug("<<<<= FormDiagramProperties::FormDiagramProperties_KeysShiftAndUp()");

                // 未選択なので何もしない
                return;
            }

            // インデックス判定
            if (selectedIndex == 0)
            {
                // ロギング
                Logger.Debug("<<<<= FormDiagramProperties::FormDiagramProperties_KeysShiftAndUp()");

                // 先頭なので何もしない
                return;
            }

            // 入れ替え
            m_ListBoxDiagram.ChangeOrder(selectedIndex - 1, selectedIndex);

            // シーケンス番号更新
            m_ListBoxDiagram.UpdateSeq();

            // 選択状態変更
            m_ListBoxDiagram.SelectedIndex = selectedIndex - 1;

            // 更新通知
            OnUpdate(this, new DiagramPropertiesUpdateEventArgs() { Properties = m_ListBoxDiagram.ToArray() });

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperties::FormDiagramProperties_KeysShiftAndUp()");
        }

        /// <summary>
        /// FormDiagramProperties_KeysShiftAndDown
        /// </summary>
        private void FormDiagramProperties_KeysShiftAndDown()
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperties::FormDiagramProperties_KeysShiftAndDown()");

            // 選択インデクス取得
            int selectedIndex = GetSelectedIndex();

            // 選択状態設定
            if (selectedIndex < 0)
            {
                // ロギング
                Logger.Debug("<<<<= FormDiagramProperties::FormDiagramProperties_KeysShiftAndDown()");

                // 未選択なので何もしない
                return;
            }

            // インデックス判定
            if (selectedIndex == m_ListBoxDiagram.Items.Count)
            {
                // ロギング
                Logger.Debug("<<<<= FormDiagramProperties::FormDiagramProperties_KeysShiftAndDown()");

                // 末尾なので何もしない
                return;
            }

            // 入れ替え
            m_ListBoxDiagram.ChangeOrder(selectedIndex, selectedIndex + 1);

            // シーケンス番号更新
            m_ListBoxDiagram.UpdateSeq();

            // 選択状態変更
            m_ListBoxDiagram.SelectedIndex = selectedIndex + 1;

            // 更新通知
            OnUpdate(this, new DiagramPropertiesUpdateEventArgs() { Properties = m_ListBoxDiagram.ToArray() });

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperties::FormDiagramProperties_KeysShiftAndDown()");
        }
        #endregion
        #endregion
    }
}
