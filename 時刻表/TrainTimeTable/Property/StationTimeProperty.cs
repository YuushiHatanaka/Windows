using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using System.Xml.Linq;
using TrainTimeTable.Common;
using TrainTimeTable.Property;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// StationTimePropertyクラス
    /// </summary>
    [Serializable]
    public class StationTimeProperty
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
        /// 列車Id
        /// </summary>
        public int TrainId { get; set; } = -1;

        /// <summary>
        /// シーケンス番号
        /// </summary>
        [Obsolete("StationTimePropertyクラスのシーケンス番号は今後使用不可となる予定です", false)]
        public int Seq { get; set; } = 0;

        /// <summary>
        /// 駅名
        /// </summary>
        public string StationName { get; set; } = string.Empty;

        /// <summary>
        /// 駅扱い
        /// </summary>
        public StationTreatment StationTreatment { get; set; } = StationTreatment.None;

        /// <summary>
        /// 発時刻
        /// </summary>
        public string DepartureTime { get; set; } = string.Empty;

        /// <summary>
        /// 推定時刻(発時刻)
        /// </summary>
        public bool EstimatedDepartureTime { get; set; } = false;

        /// <summary>
        /// 着時刻
        /// </summary>
        public string ArrivalTime { get; set; } = string.Empty;

        /// <summary>
        /// 推定時刻(着時刻)
        /// </summary>
        public bool EstimatedArrivalTime { get; set; } = false;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StationTimeProperty()
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeProperty::StationTimeProperty()");

            // ロギング
            Logger.Debug("<<<<= StationTimeProperty::StationTimeProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public StationTimeProperty(StationTimeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeProperty::StationTimeProperty(StationTimeProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            Copy(property);

            // ロギング
            Logger.Debug("<<<<= StationTimeProperty::StationTimeProperty(StationTimeProperty)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="property"></param>
        public void Copy(StationTimeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeProperty::Copy(StationTimeProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this ,property))
            {
                // コピー
                DiagramName = property.DiagramName;
                Direction = property.Direction;
                TrainId = property.TrainId;
                Seq = property.Seq;
                StationName = property.StationName;
                StationTreatment = property.StationTreatment;
                DepartureTime = property.DepartureTime;
                EstimatedDepartureTime = property.EstimatedDepartureTime;
                ArrivalTime = property.ArrivalTime;
                EstimatedArrivalTime = property.EstimatedArrivalTime;
            }

            // ロギング
            Logger.Debug("<<<<= StationTimeProperty::Copy(StationTimeProperty)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(StationTimeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeProperty::Compare(StationTimeProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 比較
            if (DiagramName != property.DiagramName)
            {
                // ロギング
                Logger.DebugFormat("DiagramName:[不一致][{0}][{1}]", DiagramName, property.DiagramName);
                Logger.Debug("<<<<= StationTimeProperty::Compare(StationTimeProperty)");

                // 不一致
                return false;
            }
            if (Direction != property.Direction)
            {
                // ロギング
                Logger.DebugFormat("Direction:[不一致][{0}][{1}]", Direction, property.Direction);
                Logger.Debug("<<<<= StationTimeProperty::Compare(StationTimeProperty)");

                // 不一致
                return false;
            }
            if (TrainId != property.TrainId)
            {
                // ロギング
                Logger.DebugFormat("TrainId:[不一致][{0}][{1}]", TrainId, property.TrainId);
                Logger.Debug("<<<<= StationTimeProperty::Compare(StationTimeProperty)");

                // 不一致
                return false;
            }
            if (Seq != property.Seq)
            {
                // ロギング
                Logger.DebugFormat("Seq:[不一致][{0}][{1}]", Seq, property.Seq);
                Logger.Debug("<<<<= StationTimeProperty::Compare(StationTimeProperty)");

                // 不一致
                return false;
            }
            if (StationName != property.StationName)
            {
                // ロギング
                Logger.DebugFormat("StationName:[不一致][{0}][{1}]", StationName, property.StationName);
                Logger.Debug("<<<<= StationTimeProperty::Compare(StationTimeProperty)");

                // 不一致
                return false;
            }
            if (StationTreatment != property.StationTreatment)
            {
                // ロギング
                Logger.DebugFormat("StationTreatment:[不一致][{0}][{1}]", StationTreatment, property.StationTreatment);
                Logger.Debug("<<<<= StationTimeProperty::Compare(StationTimeProperty)");

                // 不一致
                return false;
            }
            if (DepartureTime != property.DepartureTime)
            {
                // ロギング
                Logger.DebugFormat("DepartureTime:[不一致][{0}][{1}]", DepartureTime, property.DepartureTime);
                Logger.Debug("<<<<= StationTimeProperty::Compare(StationTimeProperty)");

                // 不一致
                return false;
            }
            if (EstimatedDepartureTime != property.EstimatedDepartureTime)
            {
                // ロギング
                Logger.DebugFormat("EstimatedDepartureTime:[不一致][{0}][{1}]", EstimatedDepartureTime, property.EstimatedDepartureTime);
                Logger.Debug("<<<<= StationTimeProperty::Compare(StationTimeProperty)");

                // 不一致
                return false;
            }
            if (ArrivalTime != property.ArrivalTime)
            {
                // ロギング
                Logger.DebugFormat("ArrivalTime:[不一致][{0}][{1}]", ArrivalTime, property.ArrivalTime);
                Logger.Debug("<<<<= StationTimeProperty::Compare(StationTimeProperty)");

                // 不一致
                return false;
            }
            if (EstimatedArrivalTime != property.EstimatedArrivalTime)
            {
                // ロギング
                Logger.DebugFormat("EstimatedArrivalTime:[不一致][{0}][{1}]", EstimatedArrivalTime, property.EstimatedArrivalTime);
                Logger.Debug("<<<<= StationTimeProperty::Compare(StationTimeProperty)");

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

        #region 発時刻DateTime取得
        /// <summary>
        /// 発時刻DateTime取得
        /// </summary>
        /// <returns></returns>
        public DateTime GetDepartureTimeValue()
        {
            DateTime result = DateTime.MinValue;

            if (DepartureTime != string.Empty)
            {
                if (!DateTime.TryParseExact(DepartureTime, "HHmm", null, System.Globalization.DateTimeStyles.None, out result))
                {
                    // TODO:推定時刻
                }
            }
            else
            {
                // TODO:推定時刻
            }

            return result;
        }

        /// <summary>
        /// 着時刻DateTime取得
        /// </summary>
        /// <returns></returns>
        public DateTime GetArrivalTimeValue()
        {
            DateTime result = DateTime.MinValue;

            if (ArrivalTime != string.Empty)
            {
                result = DateTime.ParseExact(ArrivalTime, "HHmm", null);
            }
            else
            {
                // TODO:推定時刻
            }

            return result;
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
            result.AppendLine(indentstr + string.Format("　ダイヤグラム名  :[{0}] ", DiagramName));
            result.AppendLine(indentstr + string.Format("　方向種別        :[{0}] ", Direction.GetStringValue()));
            result.AppendLine(indentstr + string.Format("　列車ID          :[{0}] ", TrainId));
            result.AppendLine(indentstr + string.Format("　シーケンス番号  :[{0}] ", Seq));
            result.AppendLine(indentstr + string.Format("　駅名            :[{0}] ", StationName));
            result.AppendLine(indentstr + string.Format("　駅扱い          :[{0}] ", StationTreatment.GetStringValue()));
            result.AppendLine(indentstr + string.Format("　発時刻          :[{0}] ", DepartureTime));
            result.AppendLine(indentstr + string.Format("　推定時刻(発時刻):[{0}] ", EstimatedDepartureTime));
            result.AppendLine(indentstr + string.Format("　着時刻          :[{0}] ", ArrivalTime));
            result.AppendLine(indentstr + string.Format("　推定時刻(着時刻):[{0}] ", EstimatedArrivalTime));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
