using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.EventArgs;
using TrainTimeTable.Property;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// ComboBoxStationクラス
    /// </summary>
    public class ComboBoxStation : ComboBox
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// StationPropertiesオブジェクト
        /// </summary>
        private StationProperties m_StationProperties = new StationProperties();

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public ComboBoxStation(StationProperties properties)
            : base()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxStation::ComboBoxStation(StationProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 設定
            DropDownStyle = ComboBoxStyle.DropDown;
            m_StationProperties.Copy(properties);

            // 更新開始
            BeginUpdate();

            // 初期化
            Initialization();

            // 更新終了
            EndUpdate();
        }
        #endregion

        #region privateメソッド
        /// <summary>
        ///  初期化
        /// </summary>
        private void Initialization()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxStation::Initialization()");

            // リストを全てクリア
            Items.Clear();

            // 列車種別を繰り返す
            foreach (var station in m_StationProperties)
            {
                // 登録
                Items.Add(station.Name);
            }

            // 登録数判定
            if (Items.Count > 0)
            {
                // 選択インデックス初期化
                SelectedIndex = 0;
            }

            // ロギング
            Logger.Debug("<<<<= ComboBoxStation::Initialization()");
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
            Logger.Debug("=>>>> ComboBoxStation::GetSelected()");

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
            Logger.Debug("<<<<= ComboBoxStation::GetSelected()");

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
            Logger.Debug("=>>>> ComboBoxStation::SetSelected(int)");
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
            Logger.Debug("<<<<= ComboBoxStation::SetSelected(int)");
        }
        #endregion
    }
}
