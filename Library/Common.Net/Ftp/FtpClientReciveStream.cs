using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Net
{
    /// <summary>
    /// FtpClientReciveStreamクラス
    /// </summary>
    public class FtpClientReciveStream
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

        #region データ書込み用FileStream
        /// <summary>
        /// データ書込み用FileStream
        /// </summary>
        public FileStream FileStream = null;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FtpClientReciveStream()
        {
        }
        #endregion
    }
}
