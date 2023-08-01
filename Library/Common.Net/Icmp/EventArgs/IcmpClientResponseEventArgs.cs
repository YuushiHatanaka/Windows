using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;

namespace Common.Net
{
    /// <summary>
    /// IcmpClientResponseEventArgsクラス
    /// </summary>
    public class IcmpClientResponseEventArgs : UdpClientEventArgs
    {
        #region 送信元IPアドレス
        /// <summary>
        /// 送信元IPアドレス
        /// </summary>
        public IPAddress FromIpAddress = null;
        #endregion

        #region 送信先IPアドレス
        /// <summary>
        /// 送信先IPアドレス
        /// </summary>
        public IPAddress ToIpAddress = null;
        #endregion

        #region 結果
        /// <summary>
        /// 結果
        /// </summary>
        public PingReply PingReply = null;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IcmpClientResponseEventArgs()
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
            result.AppendFormat("FromIpAddress: {0}\n", FromIpAddress.ToString());
            result.AppendFormat("ToIpAddress  : {0}\n", ToIpAddress.ToString());
            result.AppendFormat("└ {0}\n", IcmpClientLibrary.ShowPingReply(PingReply));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}