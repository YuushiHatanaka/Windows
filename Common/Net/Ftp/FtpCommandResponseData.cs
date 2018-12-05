using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Net.Sockets;
using System.IO;

namespace Common.Net
{
    /// <summary>
    /// コマンド応答受信データ
    /// </summary>
    public class FtpCommandResponseData
    {
        /// <summary>
        /// ソケット
        /// </summary>
        public Socket Socket = null;

        /// <summary>
        /// 受信バッファ
        /// </summary>
        public byte[] Buffer = null;

        /// <summary>
        /// 応答コード
        /// </summary>
        public FtpResponse Response = new FtpResponse();

        /// <summary>
        /// 応答データ
        /// </summary>
        public MemoryStream ResponseData = new MemoryStream();
    }
}
