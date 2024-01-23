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
    /// TrainSequencePropertyクラス
    /// </summary>
    [Serializable]
    public class TrainSequenceProperty
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
        public int Id { get; set; } = 0;

        /// <summary>
        /// シーケンス番号
        /// </summary>
        public int Seq { get; set; } = 0;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TrainSequenceProperty()
        {
            // ロギング
            Logger.Debug("=>>>> TrainSequenceProperty::TrainSequenceProperty()");

            // ロギング
            Logger.Debug("<<<<= TrainSequenceProperty::TrainSequenceProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public TrainSequenceProperty(TrainSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainSequenceProperty::TrainSequenceProperty(TrainSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            Copy(property);

            // ロギング
            Logger.Debug("<<<<= TrainSequenceProperty::TrainSequenceProperty(TrainSequenceProperty)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public TrainSequenceProperty(TrainProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainSequenceProperty::TrainSequenceProperty(TrainProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            DiagramName = property.DiagramName;
            Direction = property.Direction;
            Id = property.Id;
            Seq = -1;

            // ロギング
            Logger.Debug("<<<<= TrainSequenceProperty::TrainSequenceProperty(TrainProperty)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="property"></param>
        public void Copy(TrainSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainSequenceProperty::Copy(TrainSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this, property))
            {
                // コピー
                DiagramName = property.DiagramName;
                Direction = property.Direction;
                Id = property.Id;
                Seq = property.Seq;
            }

            // ロギング
            Logger.Debug("<<<<= TrainSequenceProperty::Copy(TrainSequenceProperty)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(TrainSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainSequenceProperty::Compare(TrainSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 比較
            if (DiagramName != property.DiagramName)
            {
                // ロギング
                Logger.DebugFormat("DiagramName:[不一致][{0}][{1}]", DiagramName, property.DiagramName);
                Logger.Debug("<<<<= TrainSequenceProperty::Compare(TrainSequenceProperty)");

                // 不一致
                return false;
            }
            if (Direction != property.Direction)
            {
                // ロギング
                Logger.DebugFormat("Direction:[不一致][{0}][{1}]", Direction, property.Direction);
                Logger.Debug("<<<<= TrainSequenceProperty::Compare(TrainSequenceProperty)");

                // 不一致
                return false;
            }
            if (Id != property.Id)
            {
                // ロギング
                Logger.DebugFormat("Id:[不一致][{0}][{1}]", Id, property.Id);
                Logger.Debug("<<<<= TrainSequenceProperty::Compare(TrainSequenceProperty)");

                // 不一致
                return false;
            }
            if (Seq != property.Seq)
            {
                // ロギング
                Logger.DebugFormat("Seq:[不一致][{0}][{1}]", Seq, property.Seq);
                Logger.Debug("<<<<= TrainSequenceProperty::Compare(TrainSequenceProperty)");

                // 不一致
                return false;
            }

            // ロギング
            Logger.DebugFormat("result:[一致]");
            Logger.Debug("<<<<= TrainSequenceProperty::Compare(TrainSequenceProperty)");

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
            result.AppendLine(indentstr + string.Format("　ダイヤグラム名:[{0}] ", DiagramName));
            result.AppendLine(indentstr + string.Format("　方向種別      :[{0}] ", Direction.GetStringValue()));
            result.AppendLine(indentstr + string.Format("　列車ID        :[{0}] ", Id));
            result.AppendLine(indentstr + string.Format("　シーケンス番号:[{0}] ", Seq));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
