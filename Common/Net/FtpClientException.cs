using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// FTPクライアント例外クラス
    /// </summary>
    public class FtpClientException : Exception
    {
        /// <summary>
        /// FTP応答
        /// </summary>
        public FtpResponse Response = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        public FtpClientException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        public FtpClientException(FtpResponse response)
            : base(response.ToString())
        {
            this.Response = response;
        }
    }
}
