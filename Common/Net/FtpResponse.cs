using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// FTP応答クラス
    /// </summary>
    public class FtpResponse
    {
        public int StatusCode = 0;
        public string StatusDetail = string.Empty;

        public override string ToString()
        {
            return this.StatusCode + " " + this.StatusDetail;
        }
    }
}
