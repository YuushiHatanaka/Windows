using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;
using TrainTimeTable.Control;
using TrainTimeTable.EventArgs;
using TrainTimeTable.Common;
using TrainTimeTable.Property;
using log4net;
using System.Reflection;

namespace TrainTimeTable.Dialog
{
    /// <summary>
    /// DialogFontクラス
    /// </summary>
    public partial class DialogFont : Form
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// ComboBoxFontNameオブジェクト
        /// </summary>
        private ComboBoxFontName m_ComboBoxFontName = null;

        /// <summary>
        /// ComboBoxFontStyleオブジェクト
        /// </summary>
        private ComboBoxFontStyle m_ComboBoxFontStyle = null;

        /// <summary>
        /// ComboBoxFontSizeオブジェクト
        /// </summary>
        private ComboBoxFontSize m_ComboBoxFontSize = null;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="property"></param>
        public DialogFont(string name, FontProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DialogFont::DialogFont(string, FontProperty)");
            Logger.DebugFormat("name    :[{0}]", name);
            Logger.DebugFormat("property:[{0}]", property.ToString());

            // コンポーネント初期化
            InitializeComponent();

            // 設定
            Text = string.Format("フォント - {0}", name);
            m_ComboBoxFontName = new ComboBoxFontName(property.Name);
            m_ComboBoxFontStyle = new ComboBoxFontStyle(property.FontStyle);
            m_ComboBoxFontSize = new ComboBoxFontSize(property.Size);

            // ロギング
            Logger.Debug("<<<<= DialogFont::DialogFont(string, FontProperty)");
        }
        #endregion

        #region イベント
        #region DialogFontイベント
        /// <summary>
        /// DialogFont_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DialogFont_Load(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DialogFont::DialogFont_Load(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // コントコール設定
            tableLayoutPanelMain.Dock = DockStyle.Fill;
            tableLayoutPanelMain.Controls.Add(m_ComboBoxFontName, 0, 1);
            tableLayoutPanelMain.Controls.Add(m_ComboBoxFontStyle, 1, 1);
            tableLayoutPanelMain.Controls.Add(m_ComboBoxFontSize, 2, 1);
            m_ComboBoxFontName.Dock = DockStyle.Fill;
            m_ComboBoxFontStyle.Dock = DockStyle.Fill;
            m_ComboBoxFontSize.Dock = DockStyle.Fill;

            m_ComboBoxFontName.OnSelectedIndexChangedEvent += m_ComboBoxFontStyle.OnComboBoxFontNameSelectedIndexChangedEvent;
            m_ComboBoxFontName.OnSelectedIndexChangedEvent += m_ComboBoxFontSize.OnComboBoxFontNameSelectedIndexChangedEvent;
            m_ComboBoxFontName.OnSelectedIndexChangedEvent += OnComboBoxFontNameSelectedIndexChangedEvent;
            m_ComboBoxFontStyle.OnSelectedIndexChangedEvent += OnComboBoxFontStyleSelectedIndexChangedEvent;
            m_ComboBoxFontSize.OnSelectedIndexChangedEvent += OnComboBoxFontSizeSelectedIndexChangedEvent;

            tableLayoutPanelButton.Dock = DockStyle.Fill;
            groupBoxSample.Dock = DockStyle.Fill;
            labelSample.Dock = DockStyle.Fill;
            buttonOK.Dock = DockStyle.Fill;
            buttonCancel.Dock = DockStyle.Fill;

            // Font反映
            labelSample.Font = GetFont();

            // ロギング
            Logger.Debug("<<<<= DialogFont::DialogFont_Load(object, EventArgs)");
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
            Logger.Debug("=>>>> DialogFont::buttonOK_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 結果設定
            DialogResult = DialogResult.OK;

            // ロギング
            Logger.Debug("<<<<= DialogFont::buttonOK_Click(object, EventArgs)");
        }

        /// <summary>
        /// buttonCancel_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DialogFont::buttonCancel_Click(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 結果設定
            DialogResult = DialogResult.Cancel;

            // ロギング
            Logger.Debug("<<<<= DialogFont::buttonCancel_Click(object, EventArgs)");
        }
        #endregion

        #region ComboBoxイベント
        /// <summary>
        /// OnComboBoxFontNameSelectedIndexChangedEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComboBoxFontNameSelectedIndexChangedEvent(object sender, FontNameSelectedIndexChangedEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DialogFont::OnComboBoxFontNameSelectedIndexChangedEvent(object, FontNameSelectedIndexChangedEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // Font反映
            labelSample.Font = GetFont();

            // ロギング
            Logger.Debug("<<<<= DialogFont::OnComboBoxFontNameSelectedIndexChangedEvent(object, FontNameSelectedIndexChangedEventArgs)");
        }

        /// <summary>
        /// OnComboBoxFontStyleSelectedIndexChangedEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComboBoxFontStyleSelectedIndexChangedEvent(object sender, FontStyleSelectedIndexChangedEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DialogFont::OnComboBoxFontNameSelectedIndexChangedEvent(object, FontStyleSelectedIndexChangedEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // Font反映
            labelSample.Font = GetFont();

            // ロギング
            Logger.Debug("<<<<= DialogFont::OnComboBoxFontNameSelectedIndexChangedEvent(object, FontStyleSelectedIndexChangedEventArgs)");
        }

        /// <summary>
        /// OnComboBoxFontSizeSelectedIndexChangedEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComboBoxFontSizeSelectedIndexChangedEvent(object sender, FontSizeSelectedIndexChangedEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DialogFont::OnComboBoxFontSizeSelectedIndexChangedEvent(object, FontSizeSelectedIndexChangedEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // Font反映
            labelSample.Font = GetFont();

            // ロギング
            Logger.Debug("<<<<= DialogFont::OnComboBoxFontSizeSelectedIndexChangedEvent(object, FontSizeSelectedIndexChangedEventArgs)");
        }
        #endregion
        #endregion

        #region publicメソッド
        /// <summary>
        /// Fontオブジェクト取得
        /// </summary>
        /// <returns></returns>
        public Font GetFont()
        {
            // ロギング
            Logger.Debug("=>>>> DialogFont::GetFont()");

            // Fontオブジェクト取得
            Font result = new Font(m_ComboBoxFontName.Value, m_ComboBoxFontSize.Value, m_ComboBoxFontStyle.Value);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DialogFont::GetFont()");

            // 返却
            return result;
        }
        #endregion
    }
}
