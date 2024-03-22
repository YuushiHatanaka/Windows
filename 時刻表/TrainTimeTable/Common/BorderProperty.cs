using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// BorderPropertyクラス
    /// </summary>
    public class BorderProperty
    {
        /// <summary>
        /// 枠線の色を設定
        /// </summary>
        public Color Color { get; set; } = Color.Black;

        /// <summary>
        /// 枠線の太さを設定
        /// </summary>
        public float Width { get; set; } = 1.0f;
    }
}
