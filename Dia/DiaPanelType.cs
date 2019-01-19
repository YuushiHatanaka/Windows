using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace Dia.Control
{
    public enum DiaPanelType : UInt32
    {
        /// <summary>
        /// なし
        /// </summary>
        Non = 0x0,
        /// <summary>
        /// 通常
        /// </summary>
        Normal = 0x01,
        /// <summary>
        /// 通過
        /// </summary>
        PassingThrough = 0x02,
        /// <summary>
        /// 他線区経由
        /// </summary>
        ViaAnotherLineSection = 0x04,
        /// <summary>
        /// この駅止まり
        /// </summary>
        ThisStationStppage = 0x08,
    }
}
