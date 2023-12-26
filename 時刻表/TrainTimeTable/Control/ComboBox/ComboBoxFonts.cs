using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Common;
using TrainTimeTable.Property;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// ComboBoxFontsクラス
    /// </summary>
    public class ComboBoxFonts : ComboBox
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public ComboBoxFonts(FontProperties properties)
            : base()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFonts::ComboBoxFonts(FontProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 設定
            DropDownStyle = ComboBoxStyle.DropDownList;

            // 更新開始
            BeginUpdate();

            // 初期化
            Initialization(properties);

            // 更新終了
            EndUpdate();

            // ロギング
            Logger.Debug("<<<<= ComboBoxFonts::ComboBoxFonts(FontProperties)");
        }
        #endregion

        #region privateメソッド
        /// <summary>
        ///  初期化
        /// </summary>
        /// <param name="properties"></param>
        private void Initialization(FontProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFonts::Initialization(FontProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // リストを全てクリア
            Items.Clear();

            // プロパティ(Key)分繰り返す
            foreach (var name in properties.Keys)
            {
                // 登録
                Items.Add(name);
            }

            // 登録数判定
            if (Items.Count > 0)
            {
                // 選択インデックス初期化
                SelectedIndex = 0;
            }

            // ロギング
            Logger.Debug("<<<<= ComboBoxFonts::Initialization(FontProperties)");
        }
        #endregion

        #region publicメソッド
        /// <summary>
        /// 選択要素取得
        /// </summary>
        /// <returns></returns>
        public string GetSelected()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFonts::GetSelected()");

            // 結果初期化
            string result = string.Empty;

            // 選択インデックス判定
            if (SelectedIndex >= 0)
            {
                // 結果設定
                result = Items[SelectedIndex].ToString();
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= ComboBoxFonts::GetSelected()");

            // 返却
            return result;
        }

        /// <summary>
        /// 選択設定
        /// </summary>
        /// <param name="name"></param>
        public void SetSelected(string name)
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxFonts::SetSelected(string)");
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
            Logger.Debug("<<<<= ComboBoxFonts::SetSelected(string)");
        }
        #endregion
    }
}
