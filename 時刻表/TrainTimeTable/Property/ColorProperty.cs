using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// ColorPropertyクラス
    /// </summary>
    [Serializable]
    public class ColorProperty
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// Alpha値
        /// </summary>
        public int Alpha { get; set; } = 0;

        /// <summary>
        /// R値
        /// </summary>
        public int R { get; set; } = 0;

        /// <summary>
        /// G値
        /// </summary>
        public int G { get; set; } = 0;

        /// <summary>
        /// B値
        /// </summary>
        public int B { get; set; } = 0;

        /// <summary>
        /// Colorオブジェクト
        /// </summary>
        public Color Value { get; set; } = Color.Black;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ColorProperty()
        {
            // ロギング
            Logger.Debug("=>>>> ColorProperty::ColorProperty()");

            // ロギング
            Logger.Debug("<<<<= ColorProperty::ColorProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="color"></param>
        public ColorProperty(Color color)
        {
            // ロギング
            Logger.Debug("=>>>> ColorProperty::ColorProperty(Color)");
            Logger.DebugFormat("color:[{0}]", color);

            // 設定
            Alpha = color.A;
            R = color.R;
            B = color.B;
            G = color.G;
            Value = color;

            // ロギング
            Logger.Debug("<<<<= ColorProperty::ColorProperty(Color)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public ColorProperty(int alpha, int r, int g, int b)
        {
            // ロギング
            Logger.Debug("=>>>> ColorProperty::ColorProperty(int, int, int, int)");
            Logger.DebugFormat("alpha:[{0}]", alpha);
            Logger.DebugFormat("r    :[{0}]", r);
            Logger.DebugFormat("g    :[{0}]", g);
            Logger.DebugFormat("b    :[{0}]", b);

            // 設定
            Alpha = alpha;
            R = r;
            G = g;
            B = b;
            Value = Color.FromArgb(Alpha, R, G, B);

            // ロギング
            Logger.Debug("<<<<= ColorProperty::ColorProperty(int, int, int, int)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="property"></param>
        public void Copy(ColorProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> ColorProperty::Copy(ColorProperty)");
            Logger.DebugFormat("ColorProperty:[{0}]", property);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this , property))
            {
                // コピー
                Alpha = property.Alpha;
                R = property.R;
                G = property.G;
                B = property.B;
                Value = property.Value;
            }

            // ロギング
            Logger.Debug("<<<<= ColorProperty::Copy(ColorProperty)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(ColorProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> ColorProperty::Compare(ColorProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 比較
            if (Alpha != property.Alpha)
            {
                // ロギング
                Logger.DebugFormat("Alpha:[不一致][{0}][{1}]", Alpha, property.Alpha);
                Logger.Debug("<<<<= ColorProperty::Compare(ColorProperty)");

                // 不一致
                return false;
            }
            if (R != property.R)
            {
                // ロギング
                Logger.DebugFormat("R:[不一致][{0}][{1}]", R, property.R);
                Logger.Debug("<<<<= ColorProperty::Compare(ColorProperty)");

                // 不一致
                return false;
            }
            if (G != property.G)
            {
                // ロギング
                Logger.DebugFormat("G:[不一致][{0}][{1}]", G, property.G);
                Logger.Debug("<<<<= ColorProperty::Compare(ColorProperty)");

                // 不一致
                return false;
            }
            if (B != property.B)
            {
                // ロギング
                Logger.DebugFormat("B:[不一致][{0}][{1}]", B, property.B);
                Logger.Debug("<<<<= ColorProperty::Compare(ColorProperty)");

                // 不一致
                return false;
            }
            if (Value != property.Value)
            {
                // ロギング
                Logger.DebugFormat("Value:[不一致][{0}][{1}]", Value, property.Value);
                Logger.Debug("<<<<= ColorProperty::Compare(ColorProperty)");

                // 不一致
                return false;
            }

            // ロギング
            Logger.DebugFormat("result:[一致]");
            Logger.Debug("<<<<= ColorProperty::Compare(ColorProperty)");

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
            result.AppendLine(indentstr + string.Format("＜カラー情報＞"));
            result.AppendLine(indentstr + string.Format("　Alpha:[{0}] ", Alpha));
            result.AppendLine(indentstr + string.Format("　R    :[{0}] ", R));
            result.AppendLine(indentstr + string.Format("　G    :[{0}] ", G));
            result.AppendLine(indentstr + string.Format("　B    :[{0}] ", B));
            result.AppendLine(indentstr + string.Format("　Value:[{0}] ", Value));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
