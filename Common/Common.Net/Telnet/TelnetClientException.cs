using System;
using System.Diagnostics;

namespace Common.Net
{
    #region Telnetクライアント例外クラス
    /// <summary>
    /// Telnetクライアント例外クラス
    /// </summary>
    public class TelnetClientException : Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        public TelnetClientException(string message)
            : base(message)
        {
            Debug.WriteLine(message);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public TelnetClientException(string message, Exception innerException)
            : base(message, innerException)
        {
            Debug.WriteLine(message);
            Debug.WriteLine(innerException.Message);
        }
    }
    #endregion
}
