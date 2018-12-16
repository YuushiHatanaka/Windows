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
    public class TelnetClientSendStream
    {
        /// <summary>
        /// データ転送用ソケット
        /// </summary>
        public Socket Socket = null;

        /// <summary>
        /// データ転送用バッファ
        /// </summary>
        public byte[] Buffer = null;

        /// <summary>
        /// データ保持用Stream
        /// </summary>
        public MemoryStream Stream = null;

        /// <summary>
        /// 文字列変換
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder _StringBuilder = new StringBuilder();

            // 文字列作成
            _StringBuilder.AppendFormat("　Socket     : {0}\n", Socket);
            if (Socket != null)
            {
                _StringBuilder.AppendFormat("　└ Connected : {0}\n", Socket.Connected);
            }
            _StringBuilder.AppendFormat("　Buffer     : {0}\n", Buffer.ToString());
            _StringBuilder.AppendFormat("　Stream     : {0}\n", Stream.ToString());

            return _StringBuilder.ToString();
        }
    }
}
