using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// FTPコマンド
    /// </summary>
    public enum FtpCommand
    {
        /// <summary>
        /// 未設定
        /// </summary>
        NONE = 0,

        /// <summary>
        /// CDUP
        /// </summary>
        CDUP,

        /// <summary>
        /// CWD
        /// </summary>
        CWD,

        /// <summary>
        /// DELETE
        /// </summary>
        DELE,

        /// <summary>
        /// LIST
        /// </summary>
        LIST,

        /// <summary>
        /// MKD
        /// </summary>
        MKD,

        /// <summary>
        /// NLST
        /// </summary>
        NLST,

        /// <summary>
        /// NOOP
        /// </summary>
        NOOP,

        /// <summary>
        /// PASS
        /// </summary>
        PASS,

        /// <summary>
        /// PASV
        /// </summary>
        PASV,

        /// <summary>
        /// PORT
        /// </summary>
        PORT,

        /// <summary>
        /// PWD
        /// </summary>
        PWD,

        /// <summary>
        /// QUIT
        /// </summary>
        QUIT,

        /// <summary>
        /// RETR
        /// </summary>
        RETR,

        /// <summary>
        /// RMD
        /// </summary>
        RMD,

        /// <summary>
        /// RNFR
        /// </summary>
        RNFR,

        /// <summary>
        /// RNTO
        /// </summary>
        RNTO,

        /// <summary>
        /// SITE
        /// </summary>
        SITE,

        /// <summary>
        /// STOR
        /// </summary>
        STOR,

        /// <summary>
        /// STOU
        /// </summary>
        STOU,
    }
}
