using System;
using System.Diagnostics;

namespace Common.Net
{
    #region 仮想ネットワーク端末例外クラス
    /// <summary>
    /// 仮想ネットワーク端末例外クラス
    /// </summary>
    public class NetworkVirtualTerminalException : Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        public NetworkVirtualTerminalException(string message)
            : base(message)
        {
            Debug.WriteLine(message);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public NetworkVirtualTerminalException(string message, Exception innerException)
            : base(message, innerException)
        {
            Debug.WriteLine(message);
            Debug.WriteLine(innerException.Message);
        }
    }
    #endregion
}
