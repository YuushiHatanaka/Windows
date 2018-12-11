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
    /// 受信Streamクラス
    /// </summary>
    public class FtpClientReciveStream
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
        /// FTP応答クラスオブジェクト
        /// </summary>
        public FtpResponse Response = new FtpResponse();

        /// <summary>
        /// データ保持用Stream
        /// </summary>
        public MemoryStream Stream = null;

        /// <summary>
        /// データ書込み用FileStream
        /// </summary>
        public FileStream FileStream = null;

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
            _StringBuilder.AppendFormat("　Response   : {0}\n", Response.ToString());
            _StringBuilder.AppendFormat("　Stream     : {0}\n", Stream.ToString());

            return _StringBuilder.ToString();
        }
    }
}
