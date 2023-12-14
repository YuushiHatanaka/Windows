using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TrainTimeTable.Common;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// TrainMarkSequencePropertyクラス
    /// </summary>
    [Serializable]
    public class TrainMarkSequenceProperty
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// ダイヤグラム番号
        /// </summary>
        public int DiagramIndex { get; set; } = -1;

        /// <summary>
        /// 列車番号(インデックス)
        /// </summary>
        public int TrainIndex { get; set; } = -1;

        /// <summary>
        /// 方向種別
        /// </summary>
        public DirectionType Direction { get; set; } = DirectionType.None;

        /// <summary>
        /// 記号名
        /// </summary>
        public string MarkName { get; set; } = string.Empty;

        /// <summary>
        /// シーケンス番号
        /// </summary>
        public int Seq { get; set; } = 0;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TrainMarkSequenceProperty()
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkSequenceProperty::TrainMarkSequenceProperty()");

            // ロギング
            Logger.Debug("<<<<= TrainMarkSequenceProperty::TrainMarkSequenceProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public TrainMarkSequenceProperty(TrainMarkSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkSequenceProperty::TrainMarkSequenceProperty(TrainMarkSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            Copy(property);

            // ロギング
            Logger.Debug("<<<<= TrainMarkSequenceProperty::TrainMarkSequenceProperty(TrainMarkSequenceProperty)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="property"></param>
        public void Copy(TrainMarkSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkSequenceProperty::Copy(TrainMarkSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this, property))
            {
                // コピー
                DiagramIndex = property.DiagramIndex;
                TrainIndex = property.TrainIndex;
                Direction = property.Direction;
                MarkName = property.MarkName;
                Seq = property.Seq;
            }

            // ロギング
            Logger.Debug("<<<<= TrainMarkSequenceProperty::Copy(TrainMarkSequenceProperty)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(TrainMarkSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkSequenceProperty::Compare(TrainMarkSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 比較
            if (DiagramIndex != property.DiagramIndex)
            {
                // ロギング
                Logger.DebugFormat("DiagramIndex:[不一致][{0}][{1}]", DiagramIndex, property.DiagramIndex);
                Logger.Debug("<<<<= TrainMarkSequenceProperty::Compare(TrainMarkSequenceProperty)");

                // 不一致
                return false;
            }
            if (TrainIndex != property.TrainIndex)
            {
                // ロギング
                Logger.DebugFormat("TrainIndex:[不一致][{0}][{1}]", TrainIndex, property.TrainIndex);
                Logger.Debug("<<<<= TrainMarkSequenceProperty::Compare(TrainMarkSequenceProperty)");

                // 不一致
                return false;
            }
            if (Direction != property.Direction)
            {
                // ロギング
                Logger.DebugFormat("Direction:[不一致][{0}][{1}]", Direction, property.Direction);
                Logger.Debug("<<<<= TrainMarkSequenceProperty::Compare(TrainMarkSequenceProperty)");

                // 不一致
                return false;
            }
            if (MarkName != property.MarkName)
            {
                // ロギング
                Logger.DebugFormat("MarkName:[不一致][{0}][{1}]", MarkName, property.MarkName);
                Logger.Debug("<<<<= TrainMarkSequenceProperty::Compare(TrainMarkSequenceProperty)");

                // 不一致
                return false;
            }
            if (Seq != property.Seq)
            {
                // ロギング
                Logger.DebugFormat("Seq:[不一致][{0}][{1}]", Seq, property.Seq);
                Logger.Debug("<<<<= TrainMarkSequenceProperty::Compare(TrainMarkSequenceProperty)");

                // 不一致
                return false;
            }

            // ロギング
            Logger.DebugFormat("result:[一致]");
            Logger.Debug("<<<<= TrainMarkSequenceProperty::Compare(TrainMarkSequenceProperty)");

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
            result.AppendLine(indentstr + string.Format("　ダイヤグラム番号      :[{0}] ", DiagramIndex));
            result.AppendLine(indentstr + string.Format("　列車番号(インデックス):[{0}] ", TrainIndex));
            result.AppendLine(indentstr + string.Format("　方向種別              :[{0}] ", Direction.GetStringValue()));
            result.AppendLine(indentstr + string.Format("　記号名                :[{0}] ", MarkName));
            result.AppendLine(indentstr + string.Format("　シーケンス番号        :[{0}] ", Seq));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
