using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Property;
using static log4net.Appender.ColoredConsoleAppender;
using TrainTimeTable.Component;
using log4net.Repository.Hierarchy;
using log4net;
using System.Reflection;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// ListBoxFontクラス
    /// </summary>
    public class ListBoxFont : ListBox
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
        public ListBoxFont(FontProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> ListBoxFont::ListBoxFont(FontProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 更新開始
            BeginUpdate();

            // 初期化
            Initialization(properties);

            // 更新終了
            EndUpdate();

            // ロギング
            Logger.Debug("<<<<= ListBoxFont::ListBoxFont(FontProperties)");
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
            Logger.Debug("=>>>> ListBoxFont::Initialization(FontProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // リストを全てクリア
            Items.Clear();

            // プロパティ分繰り返す
            foreach (var property in properties)
            {
                // 登録
                Items.Add(property.Key);
            }

            // ロギング
            Logger.Debug("<<<<= ListBoxFont::Initialization(FontProperties)");
        }
        #endregion
    }
}
