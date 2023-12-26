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
    /// <summary>
    /// ComboBoxFontSizeクラス
    /// </summary>
    public class ComboBoxFontSize : ComboBox
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
        public delegate void SelectedIndexChangedEventHandler(object sender, FontSizeSelectedIndexChangedEventArgs e);

        /// <summary>
        /// 選択インデックス変更 event
        /// </summary>
        public event SelectedIndexChangedEventHandler OnSelectedIndexChangedEvent = delegate { };
        #endregion

        /// <summary>
        /// フォントサイズリスト
        /// </summary>
        private List<float> m_FontSize { get; } = new List<float>() { 6, 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

        /// <summary>
        /// 値取得
        /// </summary>
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

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ComboBoxFontSize()
            : base()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFontSize::ComboBoxFontSize()");

            // 設定
            DropDownStyle = ComboBoxStyle.Simple;
            DrawMode = DrawMode.OwnerDrawFixed;
            SelectedIndexChanged += ComboBoxFontSize_SelectedIndexChanged;
            DrawItem += ComboBoxFontSize_DrawItem;

            // 更新開始
            BeginUpdate();

            // 初期化
            Initialization();

            // 更新終了
            EndUpdate();

            // ロギング
            Logger.Debug("<<<<= ComboBoxFontSize::ComboBoxFontSize()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="size"></param>
        public ComboBoxFontSize(float size)
            : this()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFontSize::ComboBoxFontSize(float)");
            Logger.DebugFormat("size:[{0}]", size);

            // 選択設定
            SetSelected(size);

            // ロギング
            Logger.Debug("<<<<= ComboBoxFontSize::ComboBoxFontSize(float)");
        }
        #endregion

        #region イベント
        #region ComboBoxFontSizeイベント
        /// <summary>
        /// ComboBoxFontStyle_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void ComboBoxFontSize_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFontSize::ComboBoxFontSize_SelectedIndexChanged(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // フォントサイズ判定
            if (Text != string.Empty)
            {
                // イベント情報設定
                FontSizeSelectedIndexChangedEventArgs eventArgs = new FontSizeSelectedIndexChangedEventArgs();
                eventArgs.Size = float.Parse(Text);

                // イベント発行
                OnSelectedIndexChangedEvent(this, eventArgs);
            }

            // ロギング
            Logger.Debug("<<<<= ComboBoxFontSize::ComboBoxFontSize_SelectedIndexChanged(object, EventArgs)");
        }

        /// <summary>
        /// ComboBoxFontSize_DrawItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxFontSize_DrawItem(object sender, DrawItemEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFontSize::ComboBoxFontSize_DrawItem(object, DrawItemEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

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

            // ロギング
            Logger.Debug("<<<<= ComboBoxFontSize::ComboBoxFontSize_DrawItem(object, DrawItemEventArgs)");
        }

        /// <summary>
        /// OnComboBoxFontNameSelectedIndexChangedEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnComboBoxFontNameSelectedIndexChangedEvent(object sender, FontNameSelectedIndexChangedEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFontSize::OnComboBoxFontNameSelectedIndexChangedEvent(object, FontNameSelectedIndexChangedEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 更新開始
            BeginUpdate();

            // リストを全てクリア
            Items.Clear();

            // 初期化
            Initialization();

            // 登録数判定
            if (Items.Count > 0)
            {
                // 選択インデックス初期化
                SelectedIndex = 0;
            }

            // 更新終了
            EndUpdate();

            // ロギング
            Logger.Debug("<<<<= ComboBoxFontSize::OnComboBoxFontNameSelectedIndexChangedEvent(object, FontNameSelectedIndexChangedEventArgs)");
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
            Logger.Debug("=>>>> ComboBoxFontSize::Initialization()");

            // リストを全てクリア
            Items.Clear();

            // フォントサイズ分繰り返す
            foreach (var value in m_FontSize)
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
            Logger.Debug("<<<<= ComboBoxFontSize::Initialization()");
        }

        /// <summary>
        /// 選択設定
        /// </summary>
        /// <param name="size"></param>
        private void SetSelected(float size)
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFontSize::SetSelected(float)");
            Logger.DebugFormat("size:[{0}]", size);

            // 指定されたサイズの近似値を取得
            var diff = m_FontSize.Select((x, index) => {
                var diffX = Math.Abs(x - size);
                return new { index, diffX };
            });

            // 取得したリストの若番を設定
            int result = diff.OrderBy(d => d.diffX).First().index;

            // 登録数判定
            if (Items.Count > 0)
            {
                // 選択インデックス初期化
                SelectedIndex = result;
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= ComboBoxFontSize::SetSelected(float)");
        }
        #endregion
    }
}
