using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Common;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// StationTimePropertiesクラス
    /// </summary>
    [Serializable]
    public class StationTimeProperties : List<StationTimeProperty>
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
        public StationTimeProperties()
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeProperties::StationTimeProperties()");

            // ロギング
            Logger.Debug("<<<<= StationTimeProperties::StationTimeProperties()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public StationTimeProperties(StationTimeProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeProperties::StationTimeProperties(StationTimeProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // コピー
            Copy(properties);

            // ロギング
            Logger.Debug("<<<<= StationTimeProperties::StationTimeProperties(StationProperties)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="collection"></param>
        public StationTimeProperties(IEnumerable<StationTimeProperty> collection) : base(collection)
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeProperties::StationTimeProperties(IEnumerable<StationTimeProperty>)");
            Logger.DebugFormat("collection:[{0}]", collection);

            // コピー
            Copy(collection);

            // ロギング
            Logger.Debug("<<<<= StationTimeProperties::StationTimeProperties(IEnumerable<StationTimeProperty>)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="properties"></param>
        public void Copy(StationTimeProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeProperties::Copy(StationTimeProperties)");
            Logger.DebugFormat("property:[{0}]", properties);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this ,properties))
            {
                // クリア
                Clear();

                // 要素を繰り返す
                foreach (var property in properties)
                {
                    // 登録
                    Add(new StationTimeProperty(property));
                }
            }

            // ロギング
            Logger.Debug("<<<<= StationTimeProperties::Copy(StationTimeProperties)");
        }

        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="collection"></param>
        private void Copy(IEnumerable<StationTimeProperty> collection)
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeProperties::Copy(IEnumerable<StationTimeProperty>)");
            Logger.DebugFormat("collection:[{0}]", collection);

            // クリア
            Clear();

            // 要素を繰り返す
            foreach (var property in collection)
            {
                // 登録
                Add(new StationTimeProperty(property));
            }

            // ロギング
            Logger.Debug("<<<<= StationTimeProperties::Copy(IEnumerable<StationTimeProperty>)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool Compare(StationTimeProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeProperties::Compare(StationTimeProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 要素数判定
            if (Count != properties.Count)
            {
                // ロギング
                Logger.DebugFormat("Count:[{0}][{1}][不一致]", Count, properties.Count);
                Logger.Debug("<<<<= StationTimeProperties::Compare(StationTimeProperties)");

                // 不一致
                return false;
            }

            // 要素を繰り返す
            int i = 0;
            foreach (var property in properties)
            {
                // 各要素比較
                if (!this[i].Compare(property))
                {
                    // ロギング
                    Logger.DebugFormat("Property:[不一致][{0}][{1}]", this[i].ToString(), property.ToString());
                    Logger.Debug("<<<<= StationTimeProperties::Compare(StationTimeProperties)");

                    // 不一致
                    return false;
                }

                // 要素インクリメント
                i++;
            }

            // ロギング
            Logger.Debug("result:[一致]");
            Logger.Debug("<<<<= StationTimeProperties::Compare(TrainProperties)");

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
            int i = 0;
            foreach (var property in this)
            {
                // 文字列追加
                result.AppendLine(indentstr + string.Format("《配列番号:[{0}]》", i++));
                result.Append(property.ToString(indent + 1));
            }

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
