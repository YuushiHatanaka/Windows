using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Property;
using TrainTimeTable.Component;
using log4net;
using System.Reflection;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// ListBoxColorクラス
    /// </summary>
    public class ListBoxColor : ListBox
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
        public ListBoxColor(ColorProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> ListBoxColor::ListBoxColor(ColorProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 更新開始
            BeginUpdate();

            // 初期化
            Initialization(properties);

            // 更新終了
            EndUpdate();

            // ロギング
            Logger.Debug("<<<<= ListBoxColor::ListBoxColor(ColorProperties)");
        }
        #endregion

        #region privateメソッド
        /// <summary>
        ///  初期化
        /// </summary>
        /// <param name="properties"></param>
        private void Initialization(ColorProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> ListBoxColor::Initialization(ColorProperties)");
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
            Logger.Debug("<<<<= ListBoxColor::Initialization(ColorProperties)");
        }
        #endregion
    }
}
