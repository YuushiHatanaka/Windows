using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// データ転送の接続方向
    /// </summary>
    public enum FtpTransferMode : byte
    {
        /// <summary>
        /// アクティブモード
        /// </summary>
        Active,

        /// <summary>
        /// パッシブモード
        /// </summary>
        Passive,
    }
}
