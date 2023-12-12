using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Common;
using TrainTimeTable.Property;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// RoutePropertyクラス
    /// </summary>
    [Serializable]
    public class RouteProperty
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
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 下りダイヤ名
        /// </summary>
        public string OutboundDiaName { get; set; } = string.Empty;

        /// <summary>
        /// 上りダイヤ名
        /// </summary>
        public string InboundDiaName { get; set; } = string.Empty;

        /// <summary>
        /// 駅名欄の幅
        /// </summary>
        public int WidthOfStationNameField { get; set; } = 6;

        /// <summary>
        /// 時刻表の列車の幅
        /// </summary>
        public int TimetableTrainWidth { get; set; } = 5;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RouteProperty()
        {
            // ロギング
            Logger.Debug("=>>>> RouteProperty::RouteProperty()");

            // ロギング
            Logger.Debug("<<<<= RouteProperty::RouteProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public RouteProperty(RouteProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> RouteProperty::RouteProperty(RouteProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            Copy(property);

            // ロギング
            Logger.Debug("<<<<= RouteProperty::RouteProperty(RouteProperty)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="property"></param>
        public void Copy(RouteProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> RouteProperty::Copy(RouteProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this ,property))
            {
                // コピー
                Name = property.Name;
                OutboundDiaName = property.OutboundDiaName;
                InboundDiaName = property.InboundDiaName;
                WidthOfStationNameField = property.WidthOfStationNameField;
                TimetableTrainWidth = property.TimetableTrainWidth;
            }

            // ロギング
            Logger.Debug("<<<<= RouteProperty::Copy(RouteProperty)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(RouteProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> RouteProperty::Compare(RouteProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 比較
            if (Name != property.Name)
            {
                // ロギング
                Logger.DebugFormat("Name:[不一致][{0}][{1}]", Name, property.Name);
                Logger.Debug("<<<<= RouteProperty::Compare(RouteProperty)");

                // 不一致
                return false;
            }
            if (OutboundDiaName != property.OutboundDiaName)
            {
                // ロギング
                Logger.DebugFormat("OutboundDiaName:[不一致][{0}][{1}]", OutboundDiaName, property.OutboundDiaName);
                Logger.Debug("<<<<= RouteProperty::Compare(RouteProperty)");

                // 不一致
                return false;
            }
            if (InboundDiaName != property.InboundDiaName)
            {
                // ロギング
                Logger.DebugFormat("InboundDiaName:[不一致][{0}][{1}]", InboundDiaName, property.InboundDiaName);
                Logger.Debug("<<<<= RouteProperty::Compare(RouteProperty)");

                // 不一致
                return false;
            }
            if (WidthOfStationNameField != property.WidthOfStationNameField)
            {
                // ロギング
                Logger.DebugFormat("WidthOfStationNameField:[不一致][{0}][{1}]", WidthOfStationNameField, property.WidthOfStationNameField);
                Logger.Debug("<<<<= RouteProperty::Compare(RouteProperty)");

                // 不一致
                return false;
            }
            if (TimetableTrainWidth != property.TimetableTrainWidth)
            {
                // ロギング
                Logger.DebugFormat("TimetableTrainWidth:[不一致][{0}][{1}]", TimetableTrainWidth, property.TimetableTrainWidth);
                Logger.Debug("<<<<= RouteProperty::Compare(RouteProperty)");

                // 不一致
                return false;
            }

            // ロギング
            Logger.DebugFormat("result:[一致]");
            Logger.Debug("<<<<= RouteProperty::Compare(RouteProperty)");

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
            result.AppendLine(indentstr + string.Format("＜路線情報＞"));
            result.AppendLine(indentstr + string.Format("　路線名          :[{0}] ", Name));
            result.AppendLine(indentstr + string.Format("　下りダイヤ名    :[{0}] ", OutboundDiaName));
            result.AppendLine(indentstr + string.Format("　上りダイヤ名    :[{0}] ", InboundDiaName));
            result.AppendLine(indentstr + string.Format("　駅名欄の幅      :[{0}] ", WidthOfStationNameField));
            result.AppendLine(indentstr + string.Format("　時刻表の列車の幅:[{0}] ", TimetableTrainWidth));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
