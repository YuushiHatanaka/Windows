using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Common;

namespace TrainTimeTable.Component
{
    /// <summary>
    /// DictionaryStationDistanceFromReferenceStationクラス
    /// </summary>
    [Serializable]
    public class DictionaryStationDistanceFromReferenceStation : Dictionary<DirectionType, float>
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
        public DictionaryStationDistanceFromReferenceStation()
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryStationDistanceFromReferenceStation::DictionaryStationDistanceFromReferenceStation()");

            // ロギング
            Logger.Debug("<<<<= DictionaryStationDistanceFromReferenceStation::DictionaryStationDistanceFromReferenceStation()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public DictionaryStationDistanceFromReferenceStation(DictionaryStationDistanceFromReferenceStation properties)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryStationDistanceFromReferenceStation::DictionaryStationDistanceFromReferenceStation(DictionaryStationDistanceFromReferenceStation)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // コピー
            Copy(properties);

            // ロギング
            Logger.Debug("<<<<= DictionaryStationDistanceFromReferenceStation::DictionaryStationDistanceFromReferenceStation(DictionaryStationDistanceFromReferenceStation)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="properties"></param>
        public void Copy(DictionaryStationDistanceFromReferenceStation properties)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryStationDistanceFromReferenceStation::Copy(DictionaryStationDistanceFromReferenceStation)");
            Logger.DebugFormat("property:[{0}]", properties);

            // クリア
            Clear();

            // 要素を繰り返す
            foreach (var key in properties.Keys)
            {
                // 登録
                Add(key, properties[key]);
            }

            // ロギング
            Logger.Debug("<<<<= DictionaryStationDistanceFromReferenceStation::Copy(DictionaryStationDistanceFromReferenceStation)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool Compare(DictionaryStationDistanceFromReferenceStation properties)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryStationDistanceFromReferenceStation::Compare(DictionaryStationDistanceFromReferenceStation)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // リストを繰り返す
            foreach (var property in properties)
            {
                // キー登録判定
                if (!ContainsKey(property.Key))
                {
                    // ロギング
                    Logger.DebugFormat("key:[{0}][キー登録なし]", property.Key);
                    Logger.Debug("<<<<= DictionaryStationDistanceFromReferenceStation::Compare(DictionaryStationDistanceFromReferenceStation)");

                    // 不一致
                    return false;
                }

                // 内容判定
                if (this[property.Key] != property.Value)
                {
                    // ロギング
                    Logger.DebugFormat("Property:[{0}][{1}][不一致]", this[property.Key].ToString(), property.Value.ToString());
                    Logger.Debug("<<<<= DictionaryStationDistanceFromReferenceStation::Compare(DictionaryStationDistanceFromReferenceStation)");

                    // 不一致
                    return false;
                }
            }

            // ロギング
            Logger.Debug("result:[一致]");
            Logger.Debug("<<<<= DictionaryStationDistanceFromReferenceStation::Compare(DictionaryStationDistanceFromReferenceStation)");

            // 一致
            return true;
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
                result.AppendLine(indentstr + string.Format("キー:[{0}],値:[{1}]", property.Key.GetStringValue(), property.Value));
            }

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
