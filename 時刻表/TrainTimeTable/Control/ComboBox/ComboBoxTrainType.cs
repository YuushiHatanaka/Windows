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
        /// TrainTypeSequencePropertiesオブジェクト
        /// </summary>
        private TrainTypeSequenceProperties m_TrainTypeSequenceProperties = new TrainTypeSequenceProperties();

        /// <summary>
        /// TrainTypePropertiesオブジェクト
        /// </summary>
        private TrainTypeProperties m_TrainTypeProperties = new TrainTypeProperties();

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sequences"></param>
        /// <param name="properties"></param>
        public ComboBoxTrainType(TrainTypeSequenceProperties sequences, TrainTypeProperties properties)
            : base()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxTrainType::ComboBoxTrainType(TrainSequenceProperties, TrainTypeProperties)");
            Logger.DebugFormat("sequences :[{0}]", sequences);
            Logger.DebugFormat("properties:[{0}]", properties);

            // 設定
            DropDownStyle = ComboBoxStyle.DropDownList;
            m_TrainTypeSequenceProperties.Copy(sequences);
            m_TrainTypeProperties.Copy(properties);

            // 更新開始
            BeginUpdate();

            // 初期化
            Initialization();

            // 更新終了
            EndUpdate();

            // ロギング
            Logger.Debug("<<<<= ComboBoxTrainType::ComboBoxTrainType(TrainSequenceProperties, TrainTypeProperties)");
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
            foreach (var sequence in m_TrainTypeSequenceProperties.OrderBy(t => t.Seq))
            {
                // TrainTypePropertyオブジェクト取得
                TrainTypeProperty trainTypeProperty = m_TrainTypeProperties.Find(t=>t.Name == sequence.Name);

                // 登録
                Items.Add(trainTypeProperty.Name);
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
        public string GetSelected()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxTrainType::GetSelected()");

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
            Logger.Debug("<<<<= ComboBoxTrainType::GetSelected()");

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
            Logger.Debug("=>>>> ComboBoxTrainType::SetSelected(string)");
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
            Logger.Debug("<<<<= ComboBoxTrainType::SetSelected(int)");
        }
        #endregion
    }
}
