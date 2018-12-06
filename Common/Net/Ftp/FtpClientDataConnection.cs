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
        public Socket Socket = null;

        /// <summary>
        /// 文字列変換
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder _StringBuilder = new StringBuilder();

            // 文字列作成
            _StringBuilder.AppendFormat("　Mode       : {0}\n", Mode.ToString());
            _StringBuilder.AppendFormat("　IpAddress  : {0}\n", IpAddress);
            _StringBuilder.AppendFormat("　Port       : {0}\n", Port.ToString());
            _StringBuilder.AppendFormat("　ReciveSize : {0}\n", ReciveSize.ToString());
            _StringBuilder.AppendFormat("　Socket     : {0}\n", Socket);
            if (Socket != null)
            {
                _StringBuilder.AppendFormat("　└ Connected : {0}\n", Socket.Connected);
            }

            return _StringBuilder.ToString();
        }
    }
}
