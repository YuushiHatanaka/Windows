using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using TrainTimeTable.Common;
using TrainTimeTable.Property;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// ComboBoxTrainTypeクラス
    /// </summary>
    public class ComboBoxTrainType : ComboBox
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// TrainTypePropertiesオブジェクト
        /// </summary>
        private TrainTypeProperties m_TrainTypeProperties = new TrainTypeProperties();

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public ComboBoxTrainType(TrainTypeProperties properties)
            : base()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxTrainType::ComboBoxTrainType(TrainTypeProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 設定
            DropDownStyle = ComboBoxStyle.DropDownList;
            m_TrainTypeProperties.Copy(properties);

            // 更新開始
            BeginUpdate();

            // 初期化
            Initialization();

            // 更新終了
            EndUpdate();

            // ロギング
            Logger.Debug("<<<<= ComboBoxTrainType::ComboBoxTrainType(TrainTypeProperties)");
        }
        #endregion

        #region privateメソッド
        /// <summary>
        ///  初期化
        /// </summary>
        private void Initialization()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxTrainType::Initialization()");

            // リストを全てクリア
            Items.Clear();

            // 列車種別を繰り返す
            foreach (var type in m_TrainTypeProperties)
            {
                // 登録
                Items.Add(type.Name);
            }

            // 登録数判定
            if (Items.Count > 0)
            {
                // 選択インデックス初期化
                SelectedIndex = 0;
            }

            // ロギング
            Logger.Debug("<<<<= ComboBoxTrainType::Initialization()");
        }
        #endregion

        #region publicメソッド
        /// <summary>
        /// 選択要素取得
        /// </summary>
        /// <returns></returns>
        public int GetSelected()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxTrainType::GetSelected()");

            // 結果初期化
            int result = 0;

            // 選択インデックス判定
            if (SelectedIndex >= 0)
            {
                // 結果設定
                result = m_TrainTypeProperties.Find(t => t.Name == Items[SelectedIndex].ToString()).Seq - 1;
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= ComboBoxTrainType::GetSelected()");

            // 返却
            return result;
        }

        /// <summary>
        /// 選択設定
        /// </summary>
        /// <param name="typeNo"></param>
        public void SetSelected(int typeNo)
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxTrainType::SetSelected(int)");
            Logger.DebugFormat("typeNo:[{0}]", typeNo);

            // 文字列検索
            int result = m_TrainTypeProperties.FindIndex(t => t.Seq == typeNo + 1);

            // 文字列検索結果判定
            if (result >= 0)
            {
                // 選択インデックス設定
                SelectedIndex = result;
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= ComboBoxTrainType::SetSelected(int)");
        }
        #endregion
    }
}
