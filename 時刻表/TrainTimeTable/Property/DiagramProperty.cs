using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using TrainTimeTable.Common;
using TrainTimeTable.Component;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// DiagramPropertyクラス
    /// </summary>
    [Serializable]
    public class DiagramProperty
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private readonly static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// ダイヤ名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 列車辞書
        /// </summary>
        public DictionaryTrain Trains { get; set; } = new DictionaryTrain()
        {
            { DirectionType.Outbound, new TrainProperties() },
            { DirectionType.Inbound, new TrainProperties() },
        };

        /// <summary>
        /// 列車シーケンス辞書
        /// </summary>
        public DictionaryTrainSequence TrainSequence { get; set; } = new DictionaryTrainSequence()
        {
            { DirectionType.Outbound, new TrainSequenceProperties() },
            { DirectionType.Inbound, new TrainSequenceProperties() },
        };

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DiagramProperty()
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperty::DiagramProperty()");

            // ロギング
            Logger.Debug("<<<<= DiagramProperty::DiagramProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public DiagramProperty(DiagramProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperty::DiagramProperty(DiagramProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            Copy(property);

            // ロギング
            Logger.Debug("<<<<= DiagramProperty::DiagramProperty(DiagramProperty)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="property"></param>
        public void Copy(DiagramProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperty::Copy(DiagramProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this , property))
            {
                // コピー
                Name = property.Name;
                Trains.Copy(property.Trains);
                TrainSequence.Copy(property.TrainSequence);
            }

            // ロギング
            Logger.Debug("<<<<= DiagramProperty::Copy(DiagramProperty)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(DiagramProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperty::Compare(DiagramProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 比較
            if (Name != property.Name)
            {
                // ロギング
                Logger.DebugFormat("Name:[不一致][{0}][{1}]", Name, property.Name);
                Logger.Debug("<<<<= DiagramProperty::Compare(DiagramProperty)");

                // 不一致
                return false;
            }
            if (!Trains.Compare(property.Trains))
            {
                // ロギング
                Logger.DebugFormat("Trains:[不一致][{0}][{1}]", Trains, property.Trains);
                Logger.Debug("<<<<= DiagramProperty::Compare(DiagramProperty)");

                // 不一致
                return false;
            }
            if (!TrainSequence.Compare(property.TrainSequence))
            {
                // ロギング
                Logger.DebugFormat("TrainSequence:[不一致][{0}][{1}]", TrainSequence, property.TrainSequence);
                Logger.Debug("<<<<= DiagramProperty::Compare(DiagramProperty)");

                // 不一致
                return false;
            }
            
            // ロギング
            Logger.DebugFormat("result:[一致]");
            Logger.Debug("<<<<= DiagramProperty::Compare(DiagramProperty)");

            // 一致
            return true;
        }
        #endregion

        #region 文字列化
        /// <summary>
        /// 文字列化
        /// </summary>
        /// <remarks>※このメソッドはデバッグ用には使用不可</remarks>
        /// <returns></returns>
        public override string ToString()
        {
            // 結果オブジェクト生成
            StringBuilder result = new StringBuilder();

            // 文字列追加
            result.Append(Name);

            // 返却
            return result.ToString();
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
            result.AppendLine(indentstr + string.Format("＜ダイヤグラム情報＞"));
            result.AppendLine(indentstr + string.Format("　ダイヤ名:[{0}] ", Name));
            result.Append(Trains.ToString(indent + 1));
            result.Append(TrainSequence.ToString(indent + 1));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
