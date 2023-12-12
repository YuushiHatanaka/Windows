using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// Constクラス
    /// </summary>
    public class Const
    {
        /// <summary>
        /// デフォルトデータベースファイル名
        /// </summary>
        public static string DefaultFileName { get; set; } = "default.db";

        /// <summary>
        /// デフォルトフォント名
        /// </summary>
        public static string DefaultFontName { get; set; } = "ＭＳ ゴシック";

        /// <summary>
        /// デフォルトフォントサイズ
        /// </summary>
        public static float DefaultFontSize { get; set; } = 9.0f;

        /// <summary>
        /// デフォルトフォントスタイル
        /// </summary>
        public static FontStyle DefaultFontStyle { get; set; } = FontStyle.Regular;

        /// <summary>
        /// デフォルト色
        /// </summary>
        public static Color DefaultBackColor = Color.Black;
    }
}
