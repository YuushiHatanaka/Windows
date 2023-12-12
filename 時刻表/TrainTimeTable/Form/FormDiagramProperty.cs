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
using TrainTimeTable.Property;

namespace TrainTimeTable
{
    /// <summary>
    /// FormDiagramPropertyクラス
    /// </summary>
    public partial class FormDiagramProperty : Form
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// DiagramPropertyオブジェクト
        /// </summary>
        public DiagramProperty Property = new DiagramProperty();

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormDiagramProperty()
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperty::FormDiagramProperty()");

            // コンポーネント初期化
            InitializeComponent();

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperty::FormDiagramProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public FormDiagramProperty(DiagramProperty property)
            : this()
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperty::FormDiagramProperty(DiagramProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 設定
            Property.Copy(property);

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperty::FormDiagramProperty(DiagramProperty)");
        }
        #endregion

        #region イベント
        #region FormDiagramPropertyイベント
        /// <summary>
        /// FormDiagramProperty_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormDiagramProperty_Load(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperty::FormDiagramProperty_Load(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 変換
            PropertyToControl();

            // コントコール設定
            tableLayoutPanelMain.Dock = DockStyle.Fill;
            textBoxDiagramName.Dock = DockStyle.Fill;
            buttonOK.Dock = DockStyle.Fill;
            buttonCancel.Dock = DockStyle.Fill;

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperty::FormDiagramProperty_Load(object, EventArgs)");
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
            Logger.Debug("=>>>> FormDiagramProperty::buttonOK_Click(object, EventArgs)");
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

            // 正常
            DialogResult = DialogResult.OK;

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperty::buttonOK_Click(object, EventArgs)");
        }

        /// <summary>
        /// buttonCancel_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperty::buttonCancel_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // キャンセル
            DialogResult = DialogResult.Cancel;

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperty::buttonCancel_Click(object, EventArgs)");
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
            Logger.Debug("=>>>> FormDiagramProperty::InputCheck()");

            // 入力判定
            if (textBoxDiagramName.Text == string.Empty)
            {
                // メッセージ表示
                MessageBox.Show("ダイヤ名が指定されていません", AssemblyLibrary.GetTitleVersion(), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // 異常終了
                return false;
            }

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperty::InputCheck()");

            // 正常終了
            return true;
        }

        /// <summary>
        /// プロパティ⇒コントコール変換
        /// </summary>
        private void PropertyToControl()
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperty::PropertyToControl()");

            // 変換
            textBoxDiagramName.Text = Property.Name;

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperty::PropertyToControl()");
        }

        /// <summary>
        /// コントコール⇒プロパティ変換
        /// </summary>
        /// <returns></returns>
        private void ControlToProperty()
        {
            // ロギング
            Logger.Debug("=>>>> FormDiagramProperty::ControlToProperty()");

            // 変換
            Property.Name = textBoxDiagramName.Text;

            // ロギング
            Logger.Debug("<<<<= FormDiagramProperty::ControlToProperty()");
        }
        #endregion
    }
}
