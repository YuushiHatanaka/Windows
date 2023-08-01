using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// UdpClientEventArgsクラス
    /// </summary>
    public class UdpClientEventArgs : EventArgs
    {
        /// <summary>
        /// 処理結果
        /// </summary>
        public bool Result { get; set; } = true;

        /// <summary>
        /// 例外情報
        /// </summary>
        public Exception Exception { get; set; } = null;

        /// <summary>
        /// IPアドレス
        /// </summary>
        public IPAddress IPAddress { get; set; } = null;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UdpClientEventArgs()
            : base()
        {
        }
        #endregion

        #region 文字列化
        /// <summary>
        /// 文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // 結果オブジェクト生成
            StringBuilder result = new StringBuilder();

            // 文字列作成
            result.AppendFormat("Result   : {0}\n", Result);
            if (Exception != null)
            {
                result.AppendFormat("Exception: {0}\n", Exception.ToString());
            }
            else
            {
                result.AppendFormat("Exception: null\n");
            }
            if (IPAddress != null)
            {
                result.AppendFormat("IPAddress: {0}\n", IPAddress.ToString());
            }
            else
            {
                result.AppendFormat("IPAddress: null\n");
            }

            // 返却
            return result.ToString();
        }
        #endregion 
    }
}
