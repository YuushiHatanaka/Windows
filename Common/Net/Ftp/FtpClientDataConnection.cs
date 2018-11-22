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
    /// データコネクションクラス
    /// </summary>
    public class FtpClientDataConnection
    {
        public FtpTransferMode Mode = FtpTransferMode.Active;
        public string IpAddress = string.Empty;
        public int Port;
        public int ReciveSize;
        public string ReciveSizeUnit = string.Empty;
        public Socket Socket = null;
    }
}
