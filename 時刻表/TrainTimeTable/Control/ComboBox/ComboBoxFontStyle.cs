using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using TrainTimeTable.Common;
using TrainTimeTable.EventArgs;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// ComboBoxFontStyleクラス
    /// </summary>
    public class ComboBoxFontStyle : ComboBox
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region 選択インデックス変更 Event
        /// <summary>
        /// 選択インデックス変更 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void SelectedIndexChangedEventHandler(object sender, FontStyleSelectedIndexChangedEventArgs e);

        /// <summary>
        /// 選択インデックス変更 event
        /// </summary>
        public event SelectedIndexChangedEventHandler OnSelectedIndexChangedEvent = delegate { };
        #endregion

        /// <summary>
        /// フォント名
        /// </summary>
        private string m_FontName = Const.DefaultFontName;

        /// <summary>
        /// フォントスタイル
        /// </summary>
        private Dictionary<FontStyle, string> m_FontStyles = new Dictionary<FontStyle, string>()
        {
            { FontStyle.Regular                , "標準"},
            { FontStyle.Bold                   , "太字"},
            { FontStyle.Italic                 , "斜体"},
            { FontStyle.Bold | FontStyle.Italic, "太字 斜体"},
        };

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ComboBoxFontStyle()
            : base()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFontStyle::ComboBoxFontStyle()");

            // 設定
            DropDownStyle = ComboBoxStyle.Simple;
            DrawMode = DrawMode.OwnerDrawFixed;
            SelectedIndexChanged += ComboBoxFontStyle_SelectedIndexChanged;
            DrawItem += ComboBoxFontStyle_DrawItem;

            // 更新開始
            BeginUpdate();

            // 初期化
            Initialization();

            // 更新終了
            EndUpdate();

            // ロギング
            Logger.Debug("<<<<= ComboBoxFontStyle::ComboBoxFontStyle()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="style"></param>
        public ComboBoxFontStyle(FontStyle style)
            : this()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFontStyle::ComboBoxFontStyle(FontStyle)");
            Logger.DebugFormat("style:[{0}]", style);

            // 選択設定
            SetSelected(style);

            // ロギング
            Logger.Debug("<<<<= ComboBoxFontStyle::ComboBoxFontStyle()");
        }
        #endregion

        #region イベント
        #region ComboBoxFontStyleイベント
        /// <summary>
        /// ComboBoxFontStyle_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxFontStyle_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFonts::ComboBoxFontStyle_SelectedIndexChanged(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // フォント名判定
            if (Text != string.Empty)
            {
                // イベント情報設定
                FontStyleSelectedIndexChangedEventArgs eventArgs = new FontStyleSelectedIndexChangedEventArgs();
                eventArgs.Style = GetFontStyle();

                // イベント発行
                OnSelectedIndexChangedEvent(this, eventArgs);
            }

            // ロギング
            Logger.Debug("<<<<= ComboBoxFonts::ComboBoxFontStyle_SelectedIndexChanged(object, EventArgs)");
        }

        /// <summary>
        /// ComboBoxFontStyle_DrawItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxFontStyle_DrawItem(object sender, DrawItemEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFonts::ComboBoxFontStyle_DrawItem(object, DrawItemEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // アイテムが存在するかを確認
            if (e.Index >= 0)
            {
                // 描画するアイテムのテキストを取得
                string itemText = Items[e.Index].ToString();

                FontStyle fontStyle = GetFontStyle(itemText);

                // カスタムフォントを作成
                using (Font customFont = new Font(m_FontName, Const.DefaultFontSize, fontStyle))
                {
                    // 描画色を設定
                    Brush brush = Brushes.Black;

                    // アイテムのテキストを描画
                    e.DrawBackground();
                    e.Graphics.DrawString(itemText, customFont, brush, e.Bounds, StringFormat.GenericDefault);
                }
            }

            // ロギング
            Logger.Debug("<<<<= ComboBoxFonts::ComboBoxFontStyle_DrawItem(object, DrawItemEventArgs)");
        }

        /// <summary>
        /// OnComboBoxFontNameSelectedIndexChangedEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnComboBoxFontNameSelectedIndexChangedEvent(object sender, FontNameSelectedIndexChangedEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFonts::OnComboBoxFontNameSelectedIndexChangedEvent(object, FontNameSelectedIndexChangedEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 更新開始
            BeginUpdate();

            // リストを全てクリア
            Items.Clear();

            // フォント名設定
            m_FontName = e.FontFamily.Name;

            // FontStyle.Regular使用可否判定
            if (e.FontFamily.IsStyleAvailable(FontStyle.Regular))
            {
                // 登録
                Items.Add(m_FontStyles[FontStyle.Regular]);
            }
            // FontStyle.Bold使用可否判定
            if (e.FontFamily.IsStyleAvailable(FontStyle.Bold))
            {
                // 登録
                Items.Add(m_FontStyles[FontStyle.Bold]);
            }
            // FontStyle.Italic使用可否判定
            if (e.FontFamily.IsStyleAvailable(FontStyle.Italic))
            {
                // 登録
                Items.Add(m_FontStyles[FontStyle.Italic]);
            }
            // FontStyle.Bold、FontStyle.Italic使用可否判定
            if (e.FontFamily.IsStyleAvailable(FontStyle.Bold | FontStyle.Italic))
            {
                // 登録
                Items.Add(m_FontStyles[FontStyle.Bold | FontStyle.Italic]);
            }

            // 登録数判定
            if (Items.Count > 0)
            {
                // 選択インデックス初期化
                SelectedIndex = 0;
            }

            // 更新終了
            EndUpdate();

            // ロギング
            Logger.Debug("<<<<= ComboBoxFonts::OnComboBoxFontNameSelectedIndexChangedEvent(object, FontNameSelectedIndexChangedEventArgs)");
        }
        #endregion
        #endregion

        #region privateメソッド
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFontStyle::Initialization()");

            // リストを全てクリア
            Items.Clear();

            // プロパティ(Value)分繰り返す
            foreach (var value in m_FontStyles.Values)
            {
                // 登録
                Items.Add(value);
            }

            // 登録数判定
            if (Items.Count > 0)
            {
                // 選択インデックス初期化
                SelectedIndex = 0;
            }

            // ロギング
            Logger.Debug("<<<<= ComboBoxFontStyle::Initialization()");
        }

        /// <summary>
        /// 選択設定
        /// </summary>
        /// <param name="style"></param>
        private void SetSelected(FontStyle style)
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFontStyle::SetSelected(FontStyle)");
            Logger.DebugFormat("style:[{0}]", style);

            // キー存在判定
            if (m_FontStyles.ContainsKey(style))
            {
                // 検索文字列設定
                string indexString = m_FontStyles[style];

                // 文字列検索
                int result = FindString(indexString);

                // 文字列検索結果判定
                if (result >= 0)
                {
                    // 選択インデックス設定
                    SelectedIndex = result;
                }

                // ロギング
                Logger.DebugFormat("result:[{0}]", result);
            }

            // ロギング
            Logger.Debug("<<<<= ComboBoxFontStyle::SetSelected(FontStyle)");
        }

        /// <summary>
        /// フォントスタイル取得
        /// </summary>
        /// <returns></returns>
        public FontStyle GetFontStyle()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFontStyle::GetFontStyle()");

            // 結果オブジェクト設定
            FontStyle result = GetFontStyle(Text);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= ComboBoxFontStyle::GetFontStyle()");

            // 返却
            return result;
        }

        /// <summary>
        /// フォントスタイル取得
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FontStyle GetFontStyle(string name)
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFontStyle::GetFontStyle(string)");
            Logger.DebugFormat("name:[{0}]", name);

            // 結果オブジェクト初期化
            FontStyle result = FontStyle.Regular;

            // フォント名で分岐
            switch (name)
            {
                case "標準":
                    // 結果設定
                    result = FontStyle.Regular;
                    break;
                case "太字":
                    // 結果設定
                    result = FontStyle.Bold;
                    break;
                case "斜体":
                    // 結果設定
                    result = FontStyle.Italic;
                    break;
                case "太字 斜体":
                    // 結果設定
                    result = FontStyle.Bold | FontStyle.Italic;
                    break;
                default:
                    // 結果設定
                    result = Const.DefaultFontStyle;
                    break;
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= ComboBoxFontStyle::GetFontStyle(string)");

            // 返却
            return result;
        }
        #endregion
    }
}
