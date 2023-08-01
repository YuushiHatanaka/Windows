using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;

namespace Common.Net
{
    /// <summary>
    /// NtpClientExceptionクラス
    /// </summary>
    public class NtpClientException : LoggingException
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NtpClientException()
            : base()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="msg"></param>
        public NtpClientException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public NtpClientException(string format, params object[] arg)
            : base(string.Format(format, arg))
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="innerException"></param>
        public NtpClientException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }
        #endregion
    }
}
