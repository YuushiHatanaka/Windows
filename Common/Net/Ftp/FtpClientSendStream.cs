using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;
using System.Net.Sockets;

namespace Common.Net
{
    /// <summary>
    /// 送信Streamクラス
    /// </summary>
    public class FtpClientSendStream
    {
        public Socket Socket = null;
        public MemoryStream Stream = null;
        public FtpResponse Response = new FtpResponse();
    }
}
