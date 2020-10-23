using System;
using System.Diagnostics;

namespace Common.Net
{
    /// <summary>
    /// SSHクライアント例外クラス
    /// </summary>
    public class SshClientExceptions : Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        public SshClientExceptions(string message)
            : base(message)
        {
            Debug.WriteLine(message);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public SshClientExceptions(string message, Exception innerException)
            : base(message, innerException)
        {
            Debug.WriteLine(message);
            Debug.WriteLine(innerException.Message);
        }
    }
}
