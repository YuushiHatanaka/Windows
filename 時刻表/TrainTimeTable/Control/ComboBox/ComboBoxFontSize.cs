using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Common;
using TrainTimeTable.EventArgs;

namespace TrainTimeTable.Control
{
    public class ComboBoxFontSize : ComboBox
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        private List<float> m_FontSize = new List<float>() { 6, 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

        public delegate void SelectedIndexChangedEventHandler(object sender, FontSizeSelectedIndexChangedEventArgs e);
        public event SelectedIndexChangedEventHandler OnSelectedIndexChangedEvent = delegate { };

        public float Value
        {
            get
            {
                if (SelectedIndex < 0)
                {
                    return Const.DefaultFontSize;
                }
                return float.Parse(Text);
            }
        }

        public ComboBoxFontSize()
            : base()
        {
            DropDownStyle = ComboBoxStyle.Simple;
            DrawMode = DrawMode.OwnerDrawFixed;
            SelectedIndexChanged += ComboBoxFontSize_SelectedIndexChanged;
            DrawItem += ComboBoxFontSize_DrawItem;

            BeginUpdate();

            Initialization();

            EndUpdate();
        }

        public ComboBoxFontSize(float size)
            : this()
        {
            SetSelected(size);
        }

        private void Initialization()
        {
            Items.Clear();

            foreach(var value in m_FontSize)
            {
                Items.Add(value);
            }

            if (Items.Count > 0)
            {
                SelectedIndex = 0;
            }
        }

        private void SetSelected(float size)
        {
            var diff = m_FontSize.Select((x, index) => {
                var diffX = Math.Abs(x - size);
                return new { index, diffX };
            });
            var targetIndex = diff.OrderBy(d => d.diffX).First().index;
            if (targetIndex >= 0)
            {
                SelectedIndex = targetIndex;
            }
        }

        private void ComboBoxFontSize_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (Text != string.Empty)
            {
                // イベント情報設定
                FontSizeSelectedIndexChangedEventArgs eventArgs = new FontSizeSelectedIndexChangedEventArgs();
                eventArgs.Size = float.Parse(Text);

                // イベント発行
                OnSelectedIndexChangedEvent(this, eventArgs);
            }
        }

        private void ComboBoxFontSize_DrawItem(object sender, DrawItemEventArgs e)
        {
            // アイテムが存在するかを確認
            if (e.Index >= 0)
            {
                // 描画するアイテムのテキストを取得
                string itemText = Items[e.Index].ToString();

                // カスタムフォントを作成
                using (Font customFont = new Font(Const.DefaultFontName, Const.DefaultFontSize))
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

            Initialization();

            if (Items.Count > 0)
            {
                SelectedIndex = 0;
            }

            EndUpdate();
        }
    }
}
