using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

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
            Debug.WriteLine(message);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        public FtpClientException(FtpResponse response)
            : base(response.ToString())
        {
            Debug.WriteLine(response.ToString());
            this.Response = response;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public FtpClientException(string message, Exception innerException)
            : base(message, innerException)
        {
            Debug.WriteLine(message);
            Debug.WriteLine(innerException.Message);
        }
    }
}
