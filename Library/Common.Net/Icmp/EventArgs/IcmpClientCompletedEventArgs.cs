using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using log4net.Repository.Hierarchy;

namespace Common.Net
{
    /// <summary>
    /// IcmpClientCompletedEventArgsクラス
    /// </summary>
    public class IcmpClientCompletedEventArgs : UdpClientEventArgs
    {
        #region IcmpStatisticsクラスオブジェクト
        /// <summary>
        /// IcmpStatisticsクラスオブジェクト
        /// </summary>
        public IcmpStatistics Statistics = null;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IcmpClientCompletedEventArgs()
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
            result.AppendFormat("FromIpAddress: {0}\n", Statistics.FromIpAddress.ToString());
            result.AppendFormat("ToIpAddress  : {0}\n", Statistics.ToIpAddress.ToString());
            foreach (PingReply item in Statistics.PingReplys)
            {
                result.AppendFormat("└ {0}\n", IcmpClientLibrary.ShowPingReply(item));
            }

            // 返却
            return result.ToString();
        }
        #endregion
    }
}