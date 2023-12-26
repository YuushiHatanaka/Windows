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
using System.Drawing.Drawing2D;
using System.Xml.Linq;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// ComboBoxLineStyleクラス
    /// </summary>
    public class ComboBoxLineStyle : ComboBox
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
        public ComboBoxLineStyle()
            : base()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxLineStyle::ComboBoxLineStyle()");

            // 設定
            DropDownStyle = ComboBoxStyle.DropDownList;

            // 更新開始
            BeginUpdate();

            // 初期化
            Initialization();

            // 更新終了
            EndUpdate();

            // ロギング
            Logger.Debug("<<<<= ComboBoxLineStyle::ComboBoxLineStyle()");
        }
        #endregion

        #region privateメソッド
        /// <summary>
        ///  初期化
        /// </summary>
        private void Initialization()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxLineStyle::Initialization()");

            // リストを全てクリア
            Items.Clear();

            // DashStyleを繰り返す
            for (DashStyle i = DashStyle.Solid; i <= DashStyle.DashDotDot; i++)
            {
                // 登録
                Items.Add(new DashStyleInfo(i));
            }

            // 登録数判定
            if (Items.Count > 0)
            {
                // 選択インデックス初期化
                SelectedIndex = 0;
            }

            // ロギング
            Logger.Debug("<<<<= ComboBoxLineStyle::Initialization()");
        }
        #endregion

        #region publicメソッド
        /// <summary>
        /// 選択要素取得
        /// </summary>
        /// <returns></returns>
        public DashStyle GetSelected()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxLineStyle::GetSelected()");

            // 結果初期化
            DashStyle result = DashStyle.Solid;

            // 選択インデックス判定
            if (SelectedIndex >= 0)
            {
                // 結果設定
                result = ((DashStyleInfo)(Items[SelectedIndex])).Style;
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= ComboBoxLineStyle::GetSelected()");

            // 返却
            return result;
        }

        /// <summary>
        /// 選択設定
        /// </summary>
        /// <param name="style"></param>
        public void SetSelected(DashStyle style)
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxLineStyle::SetSelected(DashStyle)");
            Logger.DebugFormat("style:[{0}]", style.GetStringValue());

            // 文字列検索
            int result = FindString(new DashStyleInfo(style).ToString());

            // 文字列検索結果判定
            if (result >= 0)
            {
                // 選択インデックス設定
                SelectedIndex = result;
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= ComboBoxLineStyle::SetSelected(DashStyle)");
        }
        #endregion
    }
}
