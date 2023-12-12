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
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using TrainTimeTable.Common;
using TrainTimeTable.EventArgs;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace TrainTimeTable.Control
{
    public class ComboBoxFontStyle : ComboBox
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        private string m_FontName = Const.DefaultFontName;

        public delegate void SelectedIndexChangedEventHandler(object sender, FontStyleSelectedIndexChangedEventArgs e);
        public event SelectedIndexChangedEventHandler OnSelectedIndexChangedEvent = delegate { };

        private Dictionary<FontStyle, string> m_FontStyles = new Dictionary<FontStyle, string>()
        {
            { FontStyle.Regular                , "標準"},
            { FontStyle.Bold                   , "太字"},
            { FontStyle.Italic                 , "斜体"},
            { FontStyle.Bold | FontStyle.Italic, "太字 斜体"},
        };

        public FontStyle Value
        {
            get
            {
                return GetFontStyle();
            }
        }

        public ComboBoxFontStyle()
            : base()
        {
            DropDownStyle = ComboBoxStyle.Simple;
            DrawMode = DrawMode.OwnerDrawFixed;
            SelectedIndexChanged += ComboBoxFontStyle_SelectedIndexChanged;
            DrawItem += ComboBoxFontStyle_DrawItem;

            BeginUpdate();

            Initialization();

            EndUpdate();
        }

        public ComboBoxFontStyle(FontStyle style)
            : this()
        {
            SetSelected(style);
        }

        private void Initialization()
        {
            Items.Clear();

            foreach (var value in m_FontStyles.Values)
            {
                Items.Add(value);
            }

            if (Items.Count > 0)
            {
                SelectedIndex = 0;
            }
        }

        private void SetSelected(FontStyle style)
        {
            if (m_FontStyles.ContainsKey(style))
            {
                string indexString = m_FontStyles[style];
                int index = FindString(indexString);
                if (index >= 0)
                {
                    SelectedIndex = index;
                }
            }
        }


        private void ComboBoxFontStyle_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (Text != string.Empty)
            {
                // イベント情報設定
                FontStyleSelectedIndexChangedEventArgs eventArgs = new FontStyleSelectedIndexChangedEventArgs();
                eventArgs.Style = GetFontStyle();

                // イベント発行
                OnSelectedIndexChangedEvent(this, eventArgs);
            }
        }

        private void ComboBoxFontStyle_DrawItem(object sender, DrawItemEventArgs e)
        {
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
        }

        public void OnComboBoxFontNameSelectedIndexChangedEvent(object sender, FontNameSelectedIndexChangedEventArgs e)
        {
            BeginUpdate();

            Items.Clear();

            m_FontName = e.FontFamily.Name;

            // FontStyle.Regular使用可否判定
            if (e.FontFamily.IsStyleAvailable(FontStyle.Regular))
            {
                Items.Add(m_FontStyles[FontStyle.Regular]);
            }
            // FontStyle.Bold使用可否判定
            if (e.FontFamily.IsStyleAvailable(FontStyle.Bold))
            {
                Items.Add(m_FontStyles[FontStyle.Bold]);
            }
            // FontStyle.Italic使用可否判定
            if (e.FontFamily.IsStyleAvailable(FontStyle.Italic))
            {
                Items.Add(m_FontStyles[FontStyle.Italic]);
            }
            // FontStyle.Bold、FontStyle.Italic使用可否判定
            if (e.FontFamily.IsStyleAvailable(FontStyle.Bold | FontStyle.Italic))
            {
                Items.Add(m_FontStyles[FontStyle.Bold | FontStyle.Italic]);
            }

            if (Items.Count > 0)
            {
                SelectedIndex = 0;
            }

            EndUpdate();
        }

        public FontStyle GetFontStyle()
        {
            return GetFontStyle(Text);
        }

        public FontStyle GetFontStyle(string name)
        {
            FontStyle result = FontStyle.Regular;

            switch (name)
            {
                case "標準":
                    result = FontStyle.Regular;
                    break;
                case "太字":
                    result = FontStyle.Bold;
                    break;
                case "斜体":
                    result = FontStyle.Italic;
                    break;
                case "太字 斜体":
                    result = FontStyle.Bold | FontStyle.Italic;
                    break;
                default:
                    result = Const.DefaultFontStyle;
                    break;
            }

            return result;
        }
    }
}
