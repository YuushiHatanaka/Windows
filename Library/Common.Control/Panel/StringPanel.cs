using System.Drawing;
using System.Windows.Forms;

namespace Common.Control
{
    /// <summary>
    /// StringPanelクラス
    /// </summary>
    public partial class StringPanel : Panel
    {
        #region Labelオブジェクト
        /// <summary>
        /// Labelオブジェクト
        /// </summary>
        private Label m_Label = new Label();

        /// <summary>
        /// Labelオブジェクト
        /// </summary>
        public Label Label
        {
            get { return m_Label; }
        }
        #endregion

        #region Text表示位置
        /// <summary>
        /// Text表示位置
        /// </summary>
        public ContentAlignment TextAlign
        {
            get
            {
                return m_Label.TextAlign;
            }
            set
            {
                m_Label.TextAlign = value;
                Refresh();
            }
        }
        #endregion

        #region 自動サイズ調整
        /// <summary>
        /// 自動サイズ調整
        /// </summary>
        public bool AutoFontSize
        {
            get
            {
                return m_Label.AutoSize;
            }
            set
            {
                m_Label.AutoSize = value;
                Refresh();
            }
        }
        #endregion

        #region 表示文字列
        /// <summary>
        /// 表示文字列
        /// </summary>
        public override string Text
        {
            get
            {
                return m_Label.Text;
            }
            set
            {
                m_Label.Text = value;
                Refresh();
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StringPanel()
        {
            // 初期化
            Initialization();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StringPanel(string text)
            : this()
        {
            // 設定
            Text = text;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="font"></param>
        public StringPanel(Font font)
        {
            // 設定
            Font = font;

            // 初期化
            Initialization();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="font"></param>
        /// <param name="text"></param>
        public StringPanel(Font font, string text)
            : this(font)
        {
            // 設定
            Text = text;
        }
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            // 設定
            m_Label.Font = Font;
            m_Label.Dock = DockStyle.Fill;
            Controls.Add(m_Label);

            // 再描画
            Refresh();
        }
        #endregion
    }
}
