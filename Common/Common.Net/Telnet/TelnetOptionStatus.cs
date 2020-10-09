using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// Telnetオプション状態
    /// </summary>
    public enum TelnetOptionStatus : byte
    {
        No, Yes, WantNo, WantYes
    }
    public enum TelnetOptionQueue { Empty, Opposite };

}
