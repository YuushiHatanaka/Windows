using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// FtpClientSendStreamクラス
    /// </summary>
    public class FtpClientSendStream
    {
        #region データ転送用ソケット
        /// <summary>
        /// データ転送用ソケット
        /// </summary>
        public Socket Socket = null;
        #endregion

        #region データ転送用バッファ
        /// <summary>
        /// データ転送用バッファ
        /// </summary>
        public byte[] Buffer = null;
        #endregion

        #region FTP応答クラスオブジェクト
        /// <summary>
        /// FTP応答クラスオブジェクト
        /// </summary>
        public FtpResponse Response = new FtpResponse();
        #endregion

        #region データ保持用Stream
        /// <summary>
        /// データ保持用Stream
        /// </summary>
        public MemoryStream Stream = null;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FtpClientSendStream()
        {
        }
        #endregion
    }
}
