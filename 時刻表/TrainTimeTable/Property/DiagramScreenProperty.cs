using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TrainTimeTable.Common;
using TrainTimeTable.Property;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// DiagramScreenProperty
    /// </summary>
    [Serializable]
    public class DiagramScreenProperty
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// ダイヤグラム起点時刻
        /// </summary>
        public DateTime DiagramDtartingTime { get; set; } = DateTime.ParseExact("0400", "HHmm", null);

        /// <summary>
        /// ダイヤグラムの規定の駅間幅
        /// </summary>
        public int StandardWidthBetweenStationsInTheDiagram { get; set; } = 60;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DiagramScreenProperty()
        {
            // ロギング
            Logger.Debug("=>>>> DiagramScreenProperty::DiagramScreenProperty()");

            // ロギング
            Logger.Debug("<<<<= DiagramScreenProperty::DiagramScreenProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public DiagramScreenProperty(DiagramScreenProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramScreenProperty::DiagramScreenProperty(DiagramScreenProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            Copy(property);

            // ロギング
            Logger.Debug("<<<<= DiagramScreenProperty::DiagramScreenProperty(DiagramScreenProperty)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="property"></param>
        public void Copy(DiagramScreenProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramScreenProperty::Copy(DiagramScreenProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this , property))
            {
                // コピー
                DiagramDtartingTime = new DateTime(
                    property.DiagramDtartingTime.Year,
                    property.DiagramDtartingTime.Month,
                    property.DiagramDtartingTime.Day,
                    property.DiagramDtartingTime.Hour,
                    property.DiagramDtartingTime.Minute,
                    property.DiagramDtartingTime.Second);
                StandardWidthBetweenStationsInTheDiagram = property.StandardWidthBetweenStationsInTheDiagram;
            }

            // ロギング
            Logger.Debug("<<<<= DiagramScreenProperty::Copy(DiagramScreenProperty)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(DiagramScreenProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramScreenProperty::Compare(DiagramScreenProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 比較
            if (DiagramDtartingTime != property.DiagramDtartingTime)
            {
                // ロギング
                Logger.DebugFormat("DiagramDtartingTime:[不一致][{0}][{1}]", DiagramDtartingTime, property.DiagramDtartingTime);
                Logger.Debug("<<<<= DiagramScreenProperty::Compare(DiagramScreenProperty)");

                // 不一致
                return false;
            }
            if (StandardWidthBetweenStationsInTheDiagram != property.StandardWidthBetweenStationsInTheDiagram)
            {
                // ロギング
                Logger.DebugFormat("StandardWidthBetweenStationsInTheDiagram:[不一致][{0}][{1}]", StandardWidthBetweenStationsInTheDiagram, property.StandardWidthBetweenStationsInTheDiagram);
                Logger.Debug("<<<<= DiagramScreenProperty::Compare(DiagramScreenProperty)");

                // 不一致
                return false;
            }

            // ロギング
            Logger.DebugFormat("result:[一致]");
            Logger.Debug("<<<<= DiagramScreenProperty::Compare(DiagramScreenProperty)");

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
            result.AppendLine(indentstr + string.Format("＜ダイヤグラム画面情報＞"));
            result.AppendLine(indentstr + string.Format("　ダイヤグラム起点時刻      :[{0}] ", DiagramDtartingTime.ToString("HH:mm")));
            result.AppendLine(indentstr + string.Format("　ダイヤグラムの規定の駅間幅:[{0}] ", StandardWidthBetweenStationsInTheDiagram));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
