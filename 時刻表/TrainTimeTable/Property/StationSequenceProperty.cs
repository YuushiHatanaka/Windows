using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TrainTimeTable.Common;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// StationSequencePropertyクラス
    /// </summary>
    [Serializable]
    public class StationSequenceProperty
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// 駅名
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
        public StationSequenceProperty()
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceProperty::StationSequenceProperty()");

            // ロギング
            Logger.Debug("<<<<= StationSequenceProperty::StationSequenceProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public StationSequenceProperty(StationSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceProperty::StationSequenceProperty(StationSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            Copy(property);

            // ロギング
            Logger.Debug("<<<<= StationSequenceProperty::StationSequenceProperty(StationSequenceProperty)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="property"></param>
        public StationSequenceProperty(int seq, StationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceProperty::StationSequenceProperty(int, StationProperty)");
            Logger.DebugFormat("seq     :[{0}]", seq);
            Logger.DebugFormat("property:[{0}]", property);

            // 設定
            Name = property.Name;
            Seq = seq;

            // ロギング
            Logger.Debug("<<<<= StationSequenceProperty::StationSequenceProperty(int, StationProperty)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="property"></param>
        public void Copy(StationSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceProperty::Copy(StationSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this, property))
            {
                // コピー
                Name = property.Name;
                Seq = property.Seq;
            }

            // ロギング
            Logger.Debug("<<<<= StationSequenceProperty::Copy(StationSequenceProperty)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(StationSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceProperty::Compare(StationSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 比較
            if (Name != property.Name)
            {
                // ロギング
                Logger.DebugFormat("Name:[不一致][{0}][{1}]", Name, property.Name);
                Logger.Debug("<<<<= StationSequenceProperty::Compare(StationSequenceProperty)");

                // 不一致
                return false;
            }
            if (Seq != property.Seq)
            {
                // ロギング
                Logger.DebugFormat("Seq:[不一致][{0}][{1}]", Seq, property.Seq);
                Logger.Debug("<<<<= StationSequenceProperty::Compare(StationSequenceProperty)");

                // 不一致
                return false;
            }

            // ロギング
            Logger.DebugFormat("result:[一致]");
            Logger.Debug("<<<<= StationSequenceProperty::Compare(StationSequenceProperty)");

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
            result.AppendLine(indentstr + string.Format("＜駅シーケンス情報＞"));
            result.AppendLine(indentstr + string.Format("　駅名          :[{0}]", Name));
            result.AppendLine(indentstr + string.Format("　シーケンス番号:[{0}]", Seq));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
