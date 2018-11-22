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
    /// 受信データクラス
    /// </summary>
    public class FtpClientReciveData
    {
        public Socket Socket = null;
        public byte[] Buffer = null;
        public FtpResponse Response = new FtpResponse();
        public MemoryStream Memory = new MemoryStream();
    }
}
