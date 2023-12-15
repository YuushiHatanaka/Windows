using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// DiagramSequencePropertyクラス
    /// </summary>
    [Serializable]
    public class DiagramSequenceProperty
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// ダイヤ名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// シーケンス番号
        /// </summary>
        public int Seq { get; set; } = 0;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DiagramSequenceProperty()
        {
            // ロギング
            Logger.Debug("=>>>> DiagramSequenceProperty::DiagramSequenceProperty()");

            // ロギング
            Logger.Debug("<<<<= DiagramSequenceProperty::DiagramSequenceProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public DiagramSequenceProperty(DiagramSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramSequenceProperty::DiagramSequenceProperty(DiagramSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            Copy(property);

            // ロギング
            Logger.Debug("<<<<= DiagramSequenceProperty::DiagramSequenceProperty(DiagramSequenceProperty)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="property"></param>
        public void Copy(DiagramSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramSequenceProperty::Copy(DiagramSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this, property))
            {
                // コピー
                Name = property.Name;
                Seq = property.Seq;
            }

            // ロギング
            Logger.Debug("<<<<= DiagramSequenceProperty::Copy(DiagramSequenceProperty)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(DiagramSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramSequenceProperty::Compare(DiagramSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 比較
            if (Name != property.Name)
            {
                // ロギング
                Logger.DebugFormat("Name:[不一致][{0}][{1}]", Name, property.Name);
                Logger.Debug("<<<<= DiagramSequenceProperty::Compare(DiagramSequenceProperty)");

                // 不一致
                return false;
            }
            if (Seq != property.Seq)
            {
                // ロギング
                Logger.DebugFormat("Seq:[不一致][{0}][{1}]", Seq, property.Seq);
                Logger.Debug("<<<<= DiagramSequenceProperty::Compare(DiagramSequenceProperty)");

                // 不一致
                return false;
            }

            // ロギング
            Logger.DebugFormat("result:[一致]");
            Logger.Debug("<<<<= DiagramSequenceProperty::Compare(DiagramSequenceProperty)");

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
            result.AppendLine(indentstr + string.Format("＜ダイヤグラムシーケンス情報＞"));
            result.AppendLine(indentstr + string.Format("　ダイヤ名    　:[{0}] ", Name));
            result.AppendLine(indentstr + string.Format("　シーケンス番号:[{0}] ", Seq));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
