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
    /// TrainTypeSequencePropertyクラス
    /// </summary>
    [Serializable]
    public class TrainTypeSequenceProperty
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// 種別名
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
        public TrainTypeSequenceProperty()
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeSequenceProperty::TrainTypeSequenceProperty()");

            // ロギング
            Logger.Debug("<<<<= TrainTypeSequenceProperty::TrainTypeSequenceProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public TrainTypeSequenceProperty(TrainTypeSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeSequenceProperty::TrainTypeSequenceProperty(TrainTypeSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            Copy(property);

            // ロギング
            Logger.Debug("<<<<= TrainTypeSequenceProperty::TrainTypeSequenceProperty(TrainTypeSequenceProperty)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="property"></param>
        public void Copy(TrainTypeSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeSequenceProperty::Copy(TrainTypeSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this, property))
            {
                // コピー
                Name = property.Name;
                Seq = property.Seq;
            }

            // ロギング
            Logger.Debug("<<<<= TrainTypeSequenceProperty::Copy(TrainTypeSequenceProperty)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(TrainTypeSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeSequenceProperty::Compare(TrainTypeSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 比較
            if (Name != property.Name)
            {
                // ロギング
                Logger.DebugFormat("Name:[不一致][{0}][{1}]", Name, property.Name);
                Logger.Debug("<<<<= TrainTypeSequenceProperty::Compare(TrainTypeSequenceProperty)");

                // 不一致
                return false;
            }
            if (Seq != property.Seq)
            {
                // ロギング
                Logger.DebugFormat("Seq:[不一致][{0}][{1}]", Seq, property.Seq);
                Logger.Debug("<<<<= TrainTypeSequenceProperty::Compare(TrainTypeSequenceProperty)");

                // 不一致
                return false;
            }

            // ロギング
            Logger.DebugFormat("result:[一致]");
            Logger.Debug("<<<<= TrainTypeSequenceProperty::Compare(TrainTypeSequenceProperty)");

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
            result.AppendLine(indentstr + string.Format("＜列車種別シーケンス情報＞"));
            result.AppendLine(indentstr + string.Format("　列車種別名    :[{0}] ", Name));
            result.AppendLine(indentstr + string.Format("　シーケンス番号:[{0}] ", Seq));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
