using System;
using System.Diagnostics;

namespace Common.Net
{
    /// <summary>
    /// Pingクライアント例外クラス
    /// </summary>
    public class PingClientException : Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        public PingClientException(string message)
            : base(message)
        {
            Debug.WriteLine(message);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public PingClientException(string message, Exception innerException)
            : base(message, innerException)
        {
            Debug.WriteLine(message);
            Debug.WriteLine(innerException.Message);
        }
    }
}
