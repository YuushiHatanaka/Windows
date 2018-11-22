using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// Telnet例外クラス
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

        }
    }
}
