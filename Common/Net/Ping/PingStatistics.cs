using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;

namespace Common.Net
{
    /// <summary>
    /// Ping結果クラス
    /// </summary>
    public class PingStatistics
    {
        /// <summary>
        /// 送信元IPアドレス
        /// </summary>
        private IpAddress m_FromIpAddress = null;

        /// <summary>
        /// 送信先IPアドレス
        /// </summary>
        private IpAddress m_ToIpAddress = null;

        /// <summary>
        /// 結果リスト
        /// </summary>
        private List<PingReply> m_PingReply = new List<PingReply>();

        /// <summary>
        /// 送信元IPアドレス
        /// </summary>
        public IpAddress FromIpAddress
        {
            get { return this.m_FromIpAddress; }
            set { this.m_FromIpAddress = value; }
        }

        /// <summary>
        /// 送信先IPアドレス
        /// </summary>
        public IpAddress ToIpAddress
        {
            get { return this.m_ToIpAddress; }
            set { this.m_ToIpAddress = value; }
        }

        /// <summary>
        /// 結果リスト
        /// </summary>
        public List<PingReply> PingReply
        {
            get { return this.m_PingReply; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public PingStatistics(IpAddress from, IpAddress to)
        {
            // 設定
            this.m_FromIpAddress = from;
            this.ToIpAddress = to;

            // クリア
            this.Clear();
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~PingStatistics()
        {
            // クリア
            this.Clear();
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            // 結果リストをクリア
            this.m_PingReply.Clear();

        }
    }
}
