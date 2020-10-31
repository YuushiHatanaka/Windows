using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Common.Net
{
    #region Streamクラス
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
        /// データ保持用Stream
        /// </summary>
        public MemoryStream Stream = null;
    }

    /// <summary>
    /// 受信Streamクラス
    /// </summary>
    public class TelnetClientReciveStream
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
    }
    #endregion
}
