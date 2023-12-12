using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using TrainTimeTable.Common;
using TrainTimeTable.Property;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// RouteFilePropertyクラス
    /// </summary>
    [Serializable]
    public class RouteFileProperty
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// ファイル情報プロパティ
        /// </summary>
        public FileInfomationProperty FileInfo = new FileInfomationProperty();

        /// <summary>
        /// 路線プロパティ
        /// </summary>
        public RouteProperty Route = new RouteProperty();

        /// <summary>
        /// フォントプロパティ
        /// </summary>
        public FontProperties Fonts = new FontProperties();

        /// <summary>
        /// 色プロパティ
        /// </summary>
        public ColorProperties Colors = new ColorProperties();

        /// <summary>
        /// ダイヤグラム画面プロパティ
        /// </summary>
        public DiagramScreenProperty DiagramScreen = new DiagramScreenProperty();

        /// <summary>
        /// 駅プロパティ
        /// </summary>
        public StationProperties Stations = new StationProperties();

        /// <summary>
        /// 列車種別プロパティ
        /// </summary>
        public TrainTypeProperties TrainTypes = new TrainTypeProperties();

        /// <summary>
        /// コメントプロパティ
        /// </summary>
        public StringBuilder Comment = new StringBuilder();

        /// <summary>
        /// ダイヤグラムプロパティ
        /// </summary>
        public DiagramProperties Diagrams = new DiagramProperties();

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RouteFileProperty()
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::RouteFileProperty()");

            // ロギング
            Logger.Debug("<<<<= RouteFileProperty::RouteFileProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public RouteFileProperty(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::RouteFileProperty(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            Copy(property);

            // ロギング
            Logger.Debug("<<<<= RouteFileProperty::RouteFileProperty(RouteFileProperty)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="property"></param>
        public void Copy(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::Copy(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this ,property))
            {
                // コピー
                FileInfo.Copy(property.FileInfo);
                Route.Copy(property.Route);
                Fonts.Copy(property.Fonts);
                Colors.Copy(property.Colors);
                DiagramScreen.Copy(property.DiagramScreen);
                Stations.Copy(property.Stations);
                TrainTypes.Copy(property.TrainTypes);
                Comment.Clear();
                Comment.Append(property.Comment);
                Diagrams.Copy(property.Diagrams);
            }

            // ロギング
            Logger.Debug("<<<<= RouteFileProperty::Copy(RouteFileProperty)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::Compare(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 比較
            if (!FileInfo.Compare(property.FileInfo))
            {
                // ロギング
                Logger.DebugFormat("FileInfo:[不一致][{0}][{1}]", FileInfo, property.FileInfo);
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }
            if (!Route.Compare(property.Route))
            {
                // ロギング
                Logger.DebugFormat("Route:[不一致][{0}][{1}]", Route, property.Route);
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }
            if (!Fonts.Compare(property.Fonts))
            {
                // ロギング
                Logger.DebugFormat("Fonts:[不一致][{0}][{1}]", Fonts, property.Fonts);
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }
            if (!Colors.Compare(property.Colors))
            {
                // ロギング
                Logger.DebugFormat("Colors:[不一致][{0}][{1}]", Colors, property.Colors);
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }
            if (!DiagramScreen.Compare(property.DiagramScreen))
            {
                // ロギング
                Logger.DebugFormat("DiagramScreen:[不一致][{0}][{1}]", DiagramScreen, property.DiagramScreen);
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }
            if (!Stations.Compare(property.Stations))
            {
                // ロギング
                Logger.DebugFormat("Stations:[不一致][{0}][{1}]", Stations, property.Stations);
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }
            if (!TrainTypes.Compare(property.TrainTypes))
            {
                // ロギング
                Logger.DebugFormat("TrainTypes:[不一致][{0}][{1}]", TrainTypes, property.TrainTypes);
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }
            if (Comment.ToString() != property.Comment.ToString())
            {
                // ロギング
                Logger.DebugFormat("Comment:[不一致][{0}][{1}]", Comment.ToString(), property.Comment.ToString());
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }
            if (!Diagrams.Compare(property.Diagrams))
            {
                // ロギング
                Logger.DebugFormat("Diagrams:[不一致][{0}][{1}]", Diagrams, property.Diagrams);
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }

            // ロギング
            Logger.DebugFormat("result:[一致]");
            Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

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
            result.Append(FileInfo.ToString(indent + 1));
            result.Append(Route.ToString(indent + 1));
            result.Append(Fonts.ToString(indent + 1));
            result.Append(Colors.ToString(indent + 1));
            result.Append(DiagramScreen.ToString(indent + 1));
            result.Append(Stations.ToString(indent + 1));
            result.Append(TrainTypes.ToString(indent + 1));
            result.AppendLine(indentstr + string.Format("＜コメント＞"));
            result.AppendLine(indentstr + string.Format("　{0}", Comment.ToString()));
            result.Append(Diagrams.ToString(indent + 1));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
