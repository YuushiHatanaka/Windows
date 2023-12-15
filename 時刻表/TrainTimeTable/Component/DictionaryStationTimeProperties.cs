using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Property;

namespace TrainTimeTable.Component
{
    /// <summary>
    /// DictionaryStationTimePropertiesクラス
    /// </summary>
    [Serializable]
    public class DictionaryStationTimeProperties : Dictionary<int, List<StationTimeProperty>>
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
        public DictionaryStationTimeProperties()
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryStationTimeProperties::DictionaryStationTimeProperties()");

            // ロギング
            Logger.Debug("<<<<= DictionaryStationTimeProperties::DictionaryStationTimeProperties()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public DictionaryStationTimeProperties(DictionaryStationTimeProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryStationTimeProperties::DictionaryStationTimeProperties(DictionaryStationDistanceFromReferenceStation)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // コピー
            Copy(properties);

            // ロギング
            Logger.Debug("<<<<= DictionaryStationTimeProperties::DictionaryStationTimeProperties(DictionaryStationDistanceFromReferenceStation)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="properties"></param>
        public void Copy(DictionaryStationTimeProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryStationTimeProperties::Copy(DictionaryStationTimeProperties)");
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
            Logger.Debug("<<<<= DictionaryStationTimeProperties::Copy(DictionaryStationTimeProperties)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool Compare(DictionaryStationTimeProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryStationTimeProperties::Compare(DictionaryStationTimeProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // リストを繰り返す
            foreach (var property in properties)
            {
                // キー登録判定
                if (!ContainsKey(property.Key))
                {
                    // ロギング
                    Logger.DebugFormat("key:[{0}][キー登録なし]", property.Key);
                    Logger.Debug("<<<<= DictionaryStationTimeProperties::Compare(DictionaryStationTimeProperties)");

                    // 不一致
                    return false;
                }

                // 内容判定
                if (this[property.Key] != property.Value)
                {
                    // ロギング
                    Logger.DebugFormat("Property:[{0}][{1}][不一致]", this[property.Key].ToString(), property.Value.ToString());
                    Logger.Debug("<<<<= DictionaryStationTimeProperties::Compare(DictionaryStationTimeProperties)");

                    // 不一致
                    return false;
                }
            }

            // ロギング
            Logger.Debug("result:[一致]");
            Logger.Debug("<<<<= DictionaryStationTimeProperties::Compare(DictionaryStationTimeProperties)");

            // 一致
            return true;
        }
        #endregion

        #region 登録
        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="property"></param>
        public void Add(int hour, StationTimeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryStationTimeProperties::Add(int, StationTimeProperty)");
            Logger.DebugFormat("hour    :[{0}]", hour);
            Logger.DebugFormat("property:[{0}]", property);

            // キー存在判定
            if (!ContainsKey(hour))
            {
                // キー登録なし
                Add(hour, new List<StationTimeProperty>());
            }

            // 登録
            this[hour].Add(property);

            // ロギング
            Logger.Debug("<<<<= DictionaryStationTimeProperties::Add(int, StationTimeProperty)");
        }
        #endregion

        #region 最大要素数取得
        /// <summary>
        /// 最大要素数取得
        /// </summary>
        /// <returns></returns>
        public int GetColumnMax()
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryStationTimeProperties::GetColumnMax()");

            // 結果設定
            int result = 0;

            // 要素を繰り返す
            foreach (var properties in this)
            {
                // 仮結果比較
                if (this[properties.Key].Count > result)
                {
                    // 要素数を設定
                    result = this[properties.Key].Count;
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DictionaryStationTimeProperties::GetColumnMax()");

            // 返却
            return result;
        }
        #endregion
    }
}
