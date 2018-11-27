using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        }
    }
}
