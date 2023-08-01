using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// IcmpStatisticsクラス
    /// </summary>
    public class IcmpStatistics
    {
        /// <summary>
        /// 送信元IPアドレス
        /// </summary>
        public IPAddress FromIpAddress = null;

        /// <summary>
        /// 送信先IPアドレス
        /// </summary>
        public IPAddress ToIpAddress = null;

        /// <summary>
        /// 結果リスト
        /// </summary>
        public List<PingReply> PingReplys = new List<PingReply>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public IcmpStatistics(IPAddress from, IPAddress to)
        {
            // 設定
            FromIpAddress = from;
            ToIpAddress = to;

            // クリア
            Clear();
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~IcmpStatistics()
        {
            // クリア
            Clear();
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            // 結果リストをクリア
            PingReplys.Clear();
        }
    }
}
