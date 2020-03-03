using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace Common.Windows.Forms
{
    /// <summary>
    /// 点滅外観
    /// </summary>
    public class BlinkAppearance
    {
        public Color BorderColor = Color.White;
        public int BorderSize = 1;
        public Color CheckedBackColor = Color.White;
        public Color MouseDownBackColor = Color.White;
        public Color MouseOverBackColor = Color.White;

        /// <summary>
        /// 設定
        /// </summary>
        /// <param name="flatButtonAppearance"></param>
        public void Set(FlatButtonAppearance flatButtonAppearance)
        {
            this.BorderColor = flatButtonAppearance.BorderColor;
            this.BorderSize = flatButtonAppearance.BorderSize;
            this.CheckedBackColor = flatButtonAppearance.CheckedBackColor;
            this.MouseDownBackColor = flatButtonAppearance.MouseDownBackColor;
            this.MouseOverBackColor = flatButtonAppearance.MouseOverBackColor;
        }
    }
}
