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
    /// TrainMarkPropertyクラス
    /// </summary>
    [Serializable]
    public class TrainMarkProperty
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// ダイヤグラム名
        /// </summary>
        public string DiagramName { get; set; } = string.Empty;

        /// <summary>
        /// 方向種別
        /// </summary>
        public DirectionType Direction { get; set; } = DirectionType.None;

        /// <summary>
        /// 列車ID
        /// </summary>
        public int TrainId { get; set; } = -1;

        /// <summary>
        /// 記号名
        /// </summary>
        public string MarkName { get; set; } = string.Empty;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TrainMarkProperty()
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkProperty::TrainMarkProperty()");

            // ロギング
            Logger.Debug("<<<<= TrainMarkProperty::TrainMarkProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public TrainMarkProperty(TrainMarkProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkProperty::TrainMarkProperty(TrainMarkProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            Copy(property);

            // ロギング
            Logger.Debug("<<<<= TrainMarkProperty::TrainMarkProperty(TrainMarkProperty)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="property"></param>
        public void Copy(TrainMarkProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkProperty::Copy(TrainMarkProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this ,property))
            {
                // コピー
                DiagramName = property.DiagramName;
                Direction = property.Direction;
                TrainId = property.TrainId;
                MarkName = property.MarkName;
            }

            // ロギング
            Logger.Debug("<<<<= TrainMarkProperty::Copy(TrainMarkProperty)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(TrainMarkProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkProperty::Compare(TrainMarkProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 比較
            if (DiagramName != property.DiagramName)
            {
                // ロギング
                Logger.DebugFormat("DiagramName:[不一致][{0}][{1}]", DiagramName, property.DiagramName);
                Logger.Debug("<<<<= TrainMarkProperty::Compare(TrainMarkProperty)");

                // 不一致
                return false;
            }
            if (Direction != property.Direction)
            {
                // ロギング
                Logger.DebugFormat("Direction:[不一致][{0}][{1}]", Direction, property.Direction);
                Logger.Debug("<<<<= TrainMarkProperty::Compare(TrainMarkProperty)");

                // 不一致
                return false;
            }
            if (TrainId != property.TrainId)
            {
                // ロギング
                Logger.DebugFormat("TrainId:[不一致][{0}][{1}]", TrainId, property.TrainId);
                Logger.Debug("<<<<= TrainMarkProperty::Compare(TrainMarkProperty)");

                // 不一致
                return false;
            }
            if (MarkName != property.MarkName)
            {
                // ロギング
                Logger.DebugFormat("MarkName:[不一致][{0}][{1}]", MarkName, property.MarkName);
                Logger.Debug("<<<<= TrainMarkProperty::Compare(TrainMarkProperty)");

                // 不一致
                return false;
            }

            // ロギング
            Logger.DebugFormat("result:[一致]");
            Logger.Debug("<<<<= StationTimeProperty::Compare(StationTimeProperty)");

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
            result.AppendLine(indentstr + string.Format("＜駅時刻情報＞"));
            result.AppendLine(indentstr + string.Format("　ダイヤグラム名:[{0}] ", DiagramName));
            result.AppendLine(indentstr + string.Format("　方向種別      :[{0}] ", Direction.GetStringValue()));
            result.AppendLine(indentstr + string.Format("　列車ID        :[{0}] ", TrainId));
            result.AppendLine(indentstr + string.Format("　記号名        :[{0}] ", MarkName));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
