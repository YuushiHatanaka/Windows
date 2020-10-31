using System;
using System.Diagnostics;

namespace Common.Net
{
    /// <summary>
    /// 仮想ネットワーク端末例外クラス
    /// </summary>
    public class SshNetworkVirtualTerminalException : Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        public SshNetworkVirtualTerminalException(string message)
            : base(message)
        {
            Debug.WriteLine(message);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public SshNetworkVirtualTerminalException(string message, Exception innerException)
            : base(message, innerException)
        {
            Debug.WriteLine(message);
            Debug.WriteLine(innerException.Message);
        }
    }
}
