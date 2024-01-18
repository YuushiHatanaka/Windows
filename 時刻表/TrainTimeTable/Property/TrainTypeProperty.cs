using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using TrainTimeTable.Common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// TrainTypePropertyクラス
    /// </summary>
    [Serializable]
    public class TrainTypeProperty
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// 種別名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 略称
        /// </summary>
        public string Abbreviation { get; set; } = string.Empty;

        /// <summary>
        /// 文字色
        /// </summary>
        public Color StringsColor { get; set; } = Color.Black;

        /// <summary>
        /// 時刻表Font名
        /// </summary>
        public string TimetableFontName { get; set; } = "時刻表ビュー 1";

        /// <summary>
        /// ダイヤグラム線色
        /// </summary>
        public Color DiagramLineColor { get; set; } = Color.Black;

        /// <summary>
        /// ダイヤグラム線スタイル
        /// </summary>
        public DashStyle DiagramLineStyle { get; set; } = DashStyle.Solid;

        /// <summary>
        /// ダイヤグラム線スタイル(太線)
        /// </summary>
        public bool DiagramLineBold { get; set; } = false;

        /// <summary>
        /// 停車駅明示
        /// </summary>
        public StopMarkDrawType StopStationClearlyIndicated { get; set; } = StopMarkDrawType.DrawOnStop;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TrainTypeProperty()
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeProperty::TrainTypeProperty()");

            // ロギング
            Logger.Debug("<<<<= TrainTypeProperty::TrainTypeProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public TrainTypeProperty(TrainTypeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeProperty::TrainTypeProperty(TrainTypeProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            Copy(property);

            // ロギング
            Logger.Debug("<<<<= TrainTypeProperty::TrainTypeProperty(TrainTypeProperty)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="property"></param>
        public void Copy(TrainTypeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeProperty::Copy(TrainTypeProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this ,property))
            {
                // コピー
                Name = property.Name;
                Abbreviation = property.Abbreviation;
                StringsColor = property.StringsColor;
                TimetableFontName = property.TimetableFontName;
                DiagramLineColor = property.DiagramLineColor;
                DiagramLineStyle = property.DiagramLineStyle;
                DiagramLineBold = property.DiagramLineBold;
                StopStationClearlyIndicated = property.StopStationClearlyIndicated;
            }

            // ロギング
            Logger.Debug("<<<<= TrainTypeProperty::Copy(TrainTypeProperty)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(TrainTypeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeProperty::Compare(TrainTypeProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 比較
            if (Name != property.Name)
            {
                // ロギング
                Logger.DebugFormat("Name:[不一致][{0}][{1}]", Name, property.Name);
                Logger.Debug("<<<<= TrainTypeProperty::Compare(TrainTypeProperty)");

                // 不一致
                return false;
            }
            if (Abbreviation != property.Abbreviation)
            {
                // ロギング
                Logger.DebugFormat("Abbreviation:[不一致][{0}][{1}]", Abbreviation, property.Abbreviation);
                Logger.Debug("<<<<= TrainTypeProperty::Compare(TrainTypeProperty)");

                // 不一致
                return false;
            }
            if (StringsColor != property.StringsColor)
            {
                // ロギング
                Logger.DebugFormat("StringsColor:[不一致][{0}][{1}]", StringsColor, property.StringsColor);
                Logger.Debug("<<<<= TrainTypeProperty::Compare(TrainTypeProperty)");

                // 不一致
                return false;
            }
            if (TimetableFontName != property.TimetableFontName)
            {
                // ロギング
                Logger.DebugFormat("TimetableFontName:[不一致][{0}][{1}]", TimetableFontName, property.TimetableFontName);
                Logger.Debug("<<<<= TrainTypeProperty::Compare(TrainTypeProperty)");

                // 不一致
                return false;
            }
            if (DiagramLineColor != property.DiagramLineColor)
            {
                // ロギング
                Logger.DebugFormat("DiagramLineColor:[不一致][{0}][{1}]", DiagramLineColor, property.DiagramLineColor);
                Logger.Debug("<<<<= TrainTypeProperty::Compare(TrainTypeProperty)");

                // 不一致
                return false;
            }
            if (DiagramLineStyle != property.DiagramLineStyle)
            {
                // ロギング
                Logger.DebugFormat("DiagramLineStyle:[不一致][{0}][{1}]", DiagramLineStyle, property.DiagramLineStyle);
                Logger.Debug("<<<<= TrainTypeProperty::Compare(TrainTypeProperty)");

                // 不一致
                return false;
            }
            if (DiagramLineBold != property.DiagramLineBold)
            {
                // ロギング
                Logger.DebugFormat("DiagramLineBold:[不一致][{0}][{1}]", DiagramLineBold, property.DiagramLineBold);
                Logger.Debug("<<<<= TrainProperty::Compare(TrainProperty)");

                // 不一致
                return false;
            }
            if (StopStationClearlyIndicated != property.StopStationClearlyIndicated)
            {
                // ロギング
                Logger.DebugFormat("StopStationClearlyIndicated:[不一致][{0}][{1}]", StopStationClearlyIndicated, property.StopStationClearlyIndicated);
                Logger.Debug("<<<<= TrainProperty::Compare(TrainProperty)");

                // 不一致
                return false;
            }

            // ロギング
            Logger.DebugFormat("result:[一致]");
            Logger.Debug("<<<<= TrainProperty::Compare(TrainProperty)");

            // 一致
            return true;
        }
        #endregion

        #region Penオブジェクト取得
        /// <summary>
        /// Penオブジェクト取得
        /// </summary>
        /// <returns></returns>
        public Pen GetDiagramLinePen()
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeProperty::GetDiagramLinePen()");

            // 結果オブジェクト生成
            Pen result = new Pen(DiagramLineColor, DiagramLineBold ? 2 : 1);
            result.DashStyle = DiagramLineStyle;

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("=>>>> TrainTypeProperty::GetDiagramLinePen()");

            // 返却
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
            result.AppendLine(indentstr + string.Format("＜列車種別＞"));
            result.AppendLine(indentstr + string.Format("　種別名                      :[{0}]", Name));
            result.AppendLine(indentstr + string.Format("　略称                        :[{0}]", Abbreviation));
            result.AppendLine(indentstr + string.Format("　文字色                      :[{0}]", StringsColor));
            result.AppendLine(indentstr + string.Format("　時刻表Font名                :[{0}]", TimetableFontName));
            result.AppendLine(indentstr + string.Format("　ダイヤグラム線色            :[{0}]", DiagramLineColor));
            result.AppendLine(indentstr + string.Format("　ダイヤグラム線スタイル      :[{0}]", DiagramLineStyle));
            result.AppendLine(indentstr + string.Format("　ダイヤグラム線スタイル(太線):[{0}]", DiagramLineBold));
            result.AppendLine(indentstr + string.Format("　停車駅明示                  :[{0}]", StopStationClearlyIndicated.GetStringValue()));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
