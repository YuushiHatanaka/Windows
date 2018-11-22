using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Net.Sockets;

namespace Common.Net
{
    /// <summary>
    /// 送信データクラス
    /// </summary>
    public class FtpClientSendData
    {
        public Socket Socket = null;
        public byte[] Buffer = null;
        public FtpResponse Response = new FtpResponse();
    }
}
