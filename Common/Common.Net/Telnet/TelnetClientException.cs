using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace Common.Net
{
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
            Trace.WriteLine("TelnetClientException::TelnetClientException(string)");
            Debug.WriteLine(message);
        }
    }
}
