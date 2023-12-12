using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// DiagramDrawPropertyクラス
    /// </summary>
    [Serializable]
    public class DiagramDrawProperty
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// 駅情報オブジェクト
        /// </summary>
        public StationProperty Station { get; set; }

        /// <summary>
        /// 描画位置(縦)
        /// </summary>
        public int DrawHeight { get; set; }

        /// <summary>
        /// 描画サイズ
        /// </summary>
        public int PenSize { get; set; }

        /// <summary>
        /// 描画Fontオブジェクト
        /// </summary>
        public Font Font { get; set; }

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DiagramDrawProperty()
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawProperty::DiagramDrawProperty()");

            // ロギング
            Logger.Debug("<<<<= DiagramDrawProperty::DiagramDrawProperty()");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="property"></param>
        public void Copy(DiagramDrawProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawProperty::Copy(DiagramDrawProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this , property))
            {
                // コピー
                Station.Copy(property.Station);
                DrawHeight = property.DrawHeight;
                PenSize = property.PenSize;
                Font = new Font(property.Font, property.Font.Style);
            }

            // ロギング
            Logger.Debug("<<<<= DiagramDrawProperty::Copy(DiagramDrawProperty)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(DiagramDrawProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawProperty::Compare(DiagramDrawProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 比較
            if (!Station.Compare(property.Station))
            {
                // ロギング
                Logger.DebugFormat("Station:[不一致][{0}][{1}]", Station, property.Station);
                Logger.Debug("<<<<= DiagramDrawProperty::Compare(DiagramDrawProperty)");

                // 不一致
                return false;
            }
            if (DrawHeight != property.DrawHeight)
            {
                // ロギング
                Logger.DebugFormat("DrawHeight:[不一致][{0}][{1}]", DrawHeight, property.DrawHeight);
                Logger.Debug("<<<<= DiagramDrawProperty::Compare(DiagramDrawProperty)");

                // 不一致
                return false;
            }
            if (PenSize != property.PenSize)
            {
                // ロギング
                Logger.DebugFormat("PenSize:[不一致][{0}][{1}]", PenSize, property.PenSize);
                Logger.Debug("<<<<= DiagramDrawProperty::Compare(DiagramDrawProperty)");

                // 不一致
                return false;
            }
            if (!Font.Equals(property.Font))
            {
                // ロギング
                Logger.DebugFormat("Font:[不一致][{0}][{1}]", Font, property.Font);
                Logger.Debug("<<<<= DiagramDrawProperty::Compare(DiagramDrawProperty)");

                // 不一致
                return false;
            }

            // ロギング
            Logger.DebugFormat("result:[一致]");
            Logger.Debug("<<<<= DiagramDrawProperty::Compare(DiagramDrawProperty)");

            // 一致
            return true;
        }
        #endregion
    }
}
