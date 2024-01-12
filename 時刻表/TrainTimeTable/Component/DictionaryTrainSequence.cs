using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Common;
using TrainTimeTable.Property;

namespace TrainTimeTable.Component
{
    /// <summary>
    /// DictionaryTrainSequenceクラス
    /// </summary>
    [Serializable]
    public class DictionaryTrainSequence : Dictionary<DirectionType, TrainSequenceProperties>
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
        public DictionaryTrainSequence()
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryTrainSequence::DictionaryTrainSequence()");

            // ロギング
            Logger.Debug("<<<<= DictionaryTrainSequence::DictionaryTrainSequence()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public DictionaryTrainSequence(DictionaryTrainSequence properties)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryTrainSequence::DictionaryTrainSequence(DictionaryTrainSequence)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // コピー
            Copy(properties);

            // ロギング
            Logger.Debug("<<<<= DictionaryTrainSequence::DictionaryTrainSequence(DictionaryTrainSequence)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="properties"></param>
        public void Copy(DictionaryTrainSequence properties)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryTrainSequence::Copy(DictionaryTrainSequence)");
            Logger.DebugFormat("property:[{0}]", properties);

            // クリア
            Clear();

            // 要素を繰り返す
            foreach (var key in properties.Keys)
            {
                // 登録
                Add(key, new TrainSequenceProperties(properties[key]));
            }

            // ロギング
            Logger.Debug("<<<<= DictionaryTrainSequence::Copy(DictionaryTrainSequence)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool Compare(DictionaryTrainSequence properties)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryTrainSequence::Compare(DictionaryTrainSequence)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // リストを繰り返す
            foreach (var property in properties)
            {
                // キー登録判定
                if (!ContainsKey(property.Key))
                {
                    // ロギング
                    Logger.DebugFormat("key:[{0}][キー登録なし]", property.Key);
                    Logger.Debug("<<<<= DictionaryTrainSequence::Compare(DictionaryTrainSequence)");

                    // 不一致
                    return false;
                }

                // 内容判定
                if (!this[property.Key].Compare(property.Value))
                {
                    // ロギング
                    Logger.DebugFormat("Property:[{0}][{1}][不一致]", this[property.Key].ToString(), property.Value.ToString());
                    Logger.Debug("<<<<= DictionaryTrainSequence::Compare(DictionaryTrainSequence)");

                    // 不一致
                    return false;
                }
            }

            // ロギング
            Logger.Debug("result:[一致]");
            Logger.Debug("<<<<= DictionaryTrainSequence::Compare(DictionaryTrainSequence)");

            // 一致
            return true;
        }
        #endregion

        #region ダイアグラム名変更
        /// <summary>
        /// ダイアグラム名変更
        /// </summary>
        /// <param name="name"></param>
        public void ChangeDiagramName(string name)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryTrainSequence::ChangeDiagramName(string)");
            Logger.DebugFormat("name:[{0}]", name);

            // リストを繰り返す
            foreach (var property in this)
            {
                // ダイアグラム名変更
                property.Value.ChangeDiagramName(name);
            }

            // ロギング
            Logger.Debug("<<<<= DictionaryTrainSequence::ChangeDiagramName(string)");
        }
        #endregion

        #region 文字列化
        /// <summary>
        /// 文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // 文字列化返却
            return ToString(0);
        }

        /// <summary>
        /// 文字列化
        /// </summary>
        /// <param name="indent"></param>
        /// <returns></returns>
        public string ToString(int indent)
        {
            // 結果オブジェクト生成
            StringBuilder result = new StringBuilder();

            // インデント生成
            string indentstr = new string('　', indent);

            // リストを繰り返す
            foreach (var property in this)
            {
                // 文字列追加
                result.AppendLine(indentstr + string.Format("キー:[{0}]", property.Key));
                result.Append(property.Value.ToString(indent + 1));
            }

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
