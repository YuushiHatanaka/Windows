using log4net;
using log4net.Repository.Hierarchy;
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
    /// NextStationPropertyクラス
    /// </summary>
    [Serializable]
    public class NextStationProperty
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
        /// 方向種別
        /// </summary>
        public DirectionType Direction { get; set; } = DirectionType.None;

        /// <summary>
        /// 次駅シーケンス番号
        /// </summary>
        public int NextStationSeq { get; set; } = 0;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NextStationProperty()
        {
            // ロギング
            Logger.Debug("=>>>> NextStationProperty::NextStationProperty()");

            // ロギング
            Logger.Debug("<<<<= NextStationProperty::NextStationProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public NextStationProperty(NextStationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> NextStationProperty::NextStationProperty(NextStationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            Copy(property);

            // ロギング
            Logger.Debug("<<<<= NextStationProperty::NextStationProperty(NextStationProperty)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="property"></param>
        public void Copy(NextStationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> NextStationProperty::Copy(NextStationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this ,property))
            {
                // コピー
                Name = property.Name;
                Direction = property.Direction;
                NextStationSeq = property.NextStationSeq;
            }

            // ロギング
            Logger.Debug("<<<<= NextStationProperty::Copy(NextStationProperty)");
        }
        #endregion   

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(NextStationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> NextStationProperty::Compare(NextStationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 比較
            if (Name != property.Name)
            {
                // ロギング
                Logger.DebugFormat("Name:[不一致][{0}][{1}]", Name, property.Name);
                Logger.Debug("<<<<= NextStationProperty::Compare(NextStationProperty)");

                // 不一致
                return false;
            }
            if (Direction != property.Direction)
            {
                // ロギング
                Logger.DebugFormat("Direction:[不一致][{0}][{1}]", Direction, property.Direction);
                Logger.Debug("<<<<= NextStationProperty::Compare(NextStationProperty)");

                // 不一致
                return false;
            }
            if (NextStationSeq != property.NextStationSeq)
            {
                // ロギング
                Logger.DebugFormat("NextStationSeq:[不一致][{0}][{1}]", NextStationSeq, property.NextStationSeq);
                Logger.Debug("<<<<= NextStationProperty::Compare(NextStationProperty)");

                // 不一致
                return false;
            }

            // ロギング
            Logger.DebugFormat("result:[一致]");
            Logger.Debug("<<<<= NextStationProperty::Compare(NextStationProperty)");

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
            result.AppendLine(indentstr + string.Format("＜次駅情報＞"));
            result.AppendLine(indentstr + string.Format("　駅名              :[{0}]", Name));
            result.AppendLine(indentstr + string.Format("　方向種別          :[{0}]", Direction.GetStringValue()));
            result.AppendLine(indentstr + string.Format("　次駅シーケンス番号:[{0}]", NextStationSeq));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
