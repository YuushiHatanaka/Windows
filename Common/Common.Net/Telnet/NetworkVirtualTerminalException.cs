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
    }
    #endregion
}
