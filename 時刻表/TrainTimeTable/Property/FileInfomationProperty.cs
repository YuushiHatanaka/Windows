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
    /// FileInfomationPropertyクラス
    /// </summary>
    [Serializable]
    public class FileInfomationProperty
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// 路線名
        /// </summary>
        public string RouteName { get; set; } = string.Empty;

        /// <summary>
        /// インポートファイル種別
        /// </summary>
        public string ImportFileType { get; set; } = string.Empty;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileInfomationProperty()
        {
            // ロギング
            Logger.Debug("=>>>> FileInfomationProperty::FileInfomationProperty()");

            // ロギング
            Logger.Debug("<<<<= FileInfomationProperty::FileInfomationProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public FileInfomationProperty(FileInfomationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FileInfomationProperty::FileInfomationProperty(FileInfomationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            Copy(property);

            // ロギング
            Logger.Debug("<<<<= FileInfomationProperty::FileInfomationProperty(FileInfomationProperty)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="property"></param>
        public void Copy(FileInfomationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FileInfomationProperty::Copy(FileInfomationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this ,property))
            {
                // コピー
                RouteName = property.RouteName;
                ImportFileType = property.ImportFileType;
            }

            // ロギング
            Logger.Debug("<<<<= FileInfomationProperty::Copy(FileInfomationProperty)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(FileInfomationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FileInfomationProperty::Compare(FileInfomationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 比較
            if (RouteName != property.RouteName)
            {
                // ロギング
                Logger.DebugFormat("RouteName:[不一致][{0}][{1}]", RouteName, property.RouteName);
                Logger.Debug("<<<<= FileInfomationProperty::Compare(FileInfomationProperty)");

                // 不一致
                return false;
            }
            if (ImportFileType != property.ImportFileType)
            {
                // ロギング
                Logger.DebugFormat("ImportFileType:[不一致][{0}][{1}]", ImportFileType, property.ImportFileType);
                Logger.Debug("<<<<= FileInfomationProperty::Compare(FileInfomationProperty)");

                // 不一致
                return false;
            }

            // ロギング
            Logger.DebugFormat("result:[一致]");
            Logger.Debug("<<<<= FileInfomationProperty::Compare(FileInfomationProperty)");

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

            // 文字列追加
            result.AppendLine(indentstr + string.Format("＜ファイル情報＞"));
            result.AppendLine(indentstr + string.Format("　路線名                :[{0}] ", RouteName));
            result.AppendLine(indentstr + string.Format("　インポートファイル種別:[{0}] ", ImportFileType));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
