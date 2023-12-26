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
    /// <summary>
    /// ComboBoxFontNameクラス
    /// </summary>
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

        #region 選択インデックス変更 Event
        /// <summary>
        /// 選択インデックス変更 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void SelectedIndexChangedEventHandler(object sender, FontNameSelectedIndexChangedEventArgs e);

        /// <summary>
        /// 選択インデックス変更 event
        /// </summary>
        public event SelectedIndexChangedEventHandler OnSelectedIndexChangedEvent = delegate { };
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ComboBoxFontName()
            : base()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFontName::ComboBoxFontName()");

            // 設定
            DropDownStyle = ComboBoxStyle.Simple;
            DrawMode = DrawMode.OwnerDrawFixed;
            SelectedIndexChanged += ComboBoxFontName_SelectedIndexChanged;
            DrawItem += ComboBoxFontName_DrawItem;

            // 更新開始
            BeginUpdate();

            // 初期化
            Initialization();

            // 更新終了
            EndUpdate();

            // ロギング
            Logger.Debug("<<<<= ComboBoxFontName::ComboBoxFontName()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        public ComboBoxFontName(string name)
            : this()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFontName::ComboBoxFontName(string)");
            Logger.DebugFormat("name:[{0}]", name);

            // 選択設定
            SetSelected(name);

            // ロギング
            Logger.Debug("<<<<= ComboBoxFontName::ComboBoxFontName(float)");
        }
        #endregion

        #region イベント
        #region ComboBoxFontNameイベント
        /// <summary>
        /// ComboBoxFontName_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxFontName_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFontName::ComboBoxFontName_SelectedIndexChanged(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // フォント名判定
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

            // ロギング
            Logger.Debug("<<<<= ComboBoxFontName::ComboBoxFontName_SelectedIndexChanged(object, EventArgs)");
        }

        /// <summary>
        /// ComboBoxFontName_DrawItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxFontName_DrawItem(object sender, DrawItemEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFontName::ComboBoxFontName_DrawItem(object, DrawItemEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

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

            // ロギング
            Logger.Debug("<<<<= ComboBoxFontName::ComboBoxFontName_DrawItem(object, DrawItemEventArgs)");
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
            Logger.Debug("=>>>> ComboBoxFontName::Initialization()");

            // リストを全てクリア
            Items.Clear();

            // InstalledFontCollectionオブジェクトの取得
            using (InstalledFontCollection ifc = new InstalledFontCollection())
            {
                // インストールされているすべてのフォントファミリアを取得
                FontFamily[] ffs = ifc.Families;

                // フォントファミリア分繰り返す
                foreach (FontFamily ff in ffs)
                {
                    //ここではスタイルにRegularが使用できるフォントのみを表示
                    if (ff.IsStyleAvailable(FontStyle.Regular))
                    {
                        // 登録
                        Items.Add(ff.Name);
                    }
                }
            }

            // 登録数判定
            if (Items.Count > 0)
            {
                // 選択インデックス初期化
                SelectedIndex = 0;
            }

            // ロギング
            Logger.Debug("<<<<= ComboBoxFontName::Initialization()");
        }

        /// <summary>
        /// 選択設定
        /// </summary>
        /// <param name="size"></param>
        private void SetSelected(string name)
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFontName::SetSelected(string)");
            Logger.DebugFormat("name:[{0}]", name);

            // 文字列検索
            int result = FindString(name);

            // 文字列検索結果判定
            if (result >= 0)
            {
                // 選択インデックス設定
                SelectedIndex = result;
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= ComboBoxFontName::SetSelected(string)");
        }
        #endregion
    }
}
