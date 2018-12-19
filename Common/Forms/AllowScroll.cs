using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Windows.Forms
{
    /// <summary>
    /// スクロール方向
    /// </summary>
    public enum AllowScroll : byte
    {
        /// <summary>
        /// なし
        /// </summary>
        Non = 0x00,

        /// <summary>
        /// 縦方向
        /// </summary>
        Vietical = 0x01,

        /// <summary>
        /// 横方向
        /// </summary>
        SideWay = 0x02,
    }
}
