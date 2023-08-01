using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;

namespace Common.Net
{
    /// <summary>
    /// FtpClientExceptionクラス
    /// </summary>
    public class FtpClientException : LoggingException
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FtpClientException()
            : base()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="msg"></param>
        public FtpClientException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="response"></param>
        public FtpClientException(FtpResponse response)
            : base(response.ToString())
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public FtpClientException(string format, params object[] arg)
            : base(string.Format(format, arg))
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="innerException"></param>
        public FtpClientException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }
        #endregion
    }
}
