using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using TrainTimeTable.EventArgs;
using TrainTimeTable.Common;
using log4net;
using System.Reflection;

namespace TrainTimeTable.Control
{
    public class ComboBoxFontName : ComboBox
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string Value
        {
            get
            {
                return Text;
            }
        }
        #endregion

        public delegate void SelectedIndexChangedEventHandler(object sender, FontNameSelectedIndexChangedEventArgs e);
        public event SelectedIndexChangedEventHandler OnSelectedIndexChangedEvent = delegate { };

        public ComboBoxFontName()
            : base()
        {
            DropDownStyle = ComboBoxStyle.Simple;
            DrawMode = DrawMode.OwnerDrawFixed;
            SelectedIndexChanged += ComboBoxFontName_SelectedIndexChanged;
            DrawItem += ComboBoxFontName_DrawItem;

            BeginUpdate();

            Initialization();

            EndUpdate();
        }

        public ComboBoxFontName(string name)
            : this()
        {
            SetSelected(name);
        }

        private void Initialization()
        {
            Items.Clear();

            // InstalledFontCollectionオブジェクトの取得
            using (InstalledFontCollection ifc = new InstalledFontCollection())
            {
                // インストールされているすべてのフォントファミリアを取得
                FontFamily[] ffs = ifc.Families;

                foreach (FontFamily ff in ffs)
                {
                    //ここではスタイルにRegularが使用できるフォントのみを表示
                    if (ff.IsStyleAvailable(FontStyle.Regular))
                    {
                        Items.Add(ff.Name);
                    }
                }
            }

            if (Items.Count > 0)
            {
                SelectedIndex = 0;
            }
        }

        private void SetSelected(string name)
        {
            int index = FindString(name);
            if (index >= 0)
            {
                SelectedIndex = index;
            }
        }

        private void ComboBoxFontName_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (Text != string.Empty)
            {
                // InstalledFontCollectionオブジェクトの取得
                using (InstalledFontCollection installedFontCollection = new InstalledFontCollection())
                {
                    // FontFamilyオブジェクト取得
                    FontFamily fontFamily = installedFontCollection.Families.First(ff => ff.Name == Text);

                    // イベント情報設定
                    FontNameSelectedIndexChangedEventArgs eventArgs = new FontNameSelectedIndexChangedEventArgs();
                    eventArgs.FontFamily = installedFontCollection.Families.First(ff => ff.Name == Text);

                    // イベント発行
                    OnSelectedIndexChangedEvent(this, eventArgs);
                }
            }
        }

        private void ComboBoxFontName_DrawItem(object sender, DrawItemEventArgs e)
        {
            // アイテムが存在するかを確認
            if (e.Index >= 0)
            {
                // 描画するアイテムのテキストを取得
                string itemText = Items[e.Index].ToString();

                // カスタムフォントを作成
                using (Font customFont = new Font(itemText, Const.DefaultFontSize))
                {
                    // 描画色を設定
                    Brush brush = Brushes.Black;

                    // アイテムのテキストを描画
                    e.DrawBackground();
                    e.Graphics.DrawString(itemText, customFont, brush, e.Bounds, StringFormat.GenericDefault);
                }
            }
        }
    }
}
