using System;
using System.Diagnostics;

namespace Common.Net
{
    /// <summary>
    /// Telnet例外クラス
    /// </summary>
    public class TelnetException : Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        public TelnetException(string message)
            : base(message)
        {
            Debug.WriteLine(message);
        }
    }
}
