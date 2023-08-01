using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;

namespace Common.Net
{
    /// <summary>
    /// IcmpClientExceptionクラス
    /// </summary>
    public class IcmpClientException : LoggingException
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IcmpClientException()
            : base()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="msg"></param>
        public IcmpClientException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public IcmpClientException(string format, params object[] arg)
            : base(string.Format(format, arg))
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="innerException"></param>
        public IcmpClientException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }
        #endregion
    }
}
