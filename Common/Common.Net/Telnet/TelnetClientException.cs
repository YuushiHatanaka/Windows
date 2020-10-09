﻿using System;
using System.Diagnostics;

namespace Common.Net
{
    #region Telnetクライアント例外クラス
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
            Debug.WriteLine(message);
        }
    }
    #endregion
}
