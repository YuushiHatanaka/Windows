using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Performance
{
    public class PerformanceCounterException : Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Message">例外メッセージ</param>
        public PerformanceCounterException(String Message)
            : base(Message)
        {
        }
    }
}