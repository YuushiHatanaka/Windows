using System;
using System.Diagnostics;

namespace Common.Net
{
    #region TELNET仮想ネットワーク端末例外クラス
    /// <summary>
    /// TELNET仮想ネットワーク端末例外クラス
    /// </summary>
    public class TelnetNetworkVirtualTerminalException : Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        public TelnetNetworkVirtualTerminalException(string message)
            : base(message)
        {
            Debug.WriteLine(message);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public TelnetNetworkVirtualTerminalException(string message, Exception innerException)
            : base(message, innerException)
        {
            Debug.WriteLine(message);
            Debug.WriteLine(innerException.Message);
        }
    }
    #endregion
}
