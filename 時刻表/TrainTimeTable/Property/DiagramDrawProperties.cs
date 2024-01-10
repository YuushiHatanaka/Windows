using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// DiagramDrawPropertiesクラス
    /// </summary>
    [Serializable]
    public class DiagramDrawProperties : List<DiagramDrawProperty>
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
        public DiagramDrawProperties()
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawProperties::DiagramDrawProperties()");

            // ロギング
            Logger.Debug("<<<<= DiagramDrawProperties::DiagramDrawProperties()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public DiagramDrawProperties(DiagramDrawProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawProperties::DiagramDrawProperties(DiagramDrawProperties)");
            Logger.DebugFormat("property:[{0}]", properties);

            // コピー
            Copy(properties);

            // ロギング
            Logger.Debug("<<<<= DiagramDrawProperties::DiagramDrawProperties(DiagramDrawProperties)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="properties"></param>
        public void Copy(DiagramDrawProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawProperties::Copy(DiagramDrawProperties)");
            Logger.DebugFormat("property:[{0}]", properties);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this, properties))
            {
                // リストを一旦クリア
                Clear();

                // オブジェクトを繰り返す
                foreach (var property in properties)
                {
                    // 登録
                    Add(new DiagramDrawProperty(property));
                }
            }

            // ロギング
            Logger.Debug("<<<<= DiagramDrawProperties::Copy(DiagramDrawProperties)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(DiagramDrawProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawProperties::Compare(DiagramDrawProperties)");
            Logger.DebugFormat("property:[{0}]", properties);

            // 要素数判定
            if (Count != properties.Count)
            {
                // ロギング
                Logger.DebugFormat("Count:[{0}][{1}][不一致]", Count, properties.Count);
                Logger.Debug("<<<<= DiagramDrawProperties::Compare(DiagramDrawProperties)");

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
                    Logger.Debug("<<<<= DiagramDrawProperties::Compare(DiagramDrawProperties)");

                    // 不一致
                    return false;
                }

                // 要素インクリメント
                i++;
            }

            // ロギング
            Logger.Debug("result:[一致]");
            Logger.Debug("<<<<= DiagramDrawProperties::Compare(DiagramDrawProperties)");

            // 一致
            return true;
        }
        #endregion
    }
}
