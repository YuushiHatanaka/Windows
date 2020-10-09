using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;

namespace Common.Net
{
    /// <summary>
    /// Pingクライアントクラス
    /// </summary>
    public class PingClient
    {
        /// <summary>
        /// Pingオブジェクト
        /// </summary>
        private Ping m_Ping = null;

        /// <summary>
        /// オプション
        /// </summary>
        private PingOptions m_Options = null;

        /// <summary>
        /// 送信バッファ
        /// </summary>
        private byte[] m_SendBuffer = null;

        /// <summary>
        /// タイムアウト
        /// </summary>
        private int m_Timeout = 10000;

        /// <summary>
        /// 結果クラス
        /// </summary>
        private PingStatistics m_Statistics = null;

        /// <summary>
        /// ホスト名
        /// </summary>
        public PingStatistics Statistics
        {
            get { return this.m_Statistics; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PingClient()
        {
            // オブジェクト生成
            this.m_Ping = new Ping();
            this.m_Options = new PingOptions(128, false);
            this.m_SendBuffer = System.Text.Encoding.ASCII.GetBytes(new string('A', 32));
        }

        /// <summary>
        /// ローカルアドレス取得(IPv4)
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        private IPAddress GetLocalIPAddress(AddressFamily family)
        {
            IPAddress[] _IPAddress = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in _IPAddress)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    return address;
                }
            }
            return null;
        }

        /// <summary>
        /// 送信(IPv4)
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="count"></param>
        /// <param name="wait"></param>
        /// <exception cref="PingClientException"></exception>
        public void Send(string ipAddress, int count, int wait)
        {
            // IPアドレス(送信元)
            IPAddress localIPAddress = this.GetLocalIPAddress(AddressFamily.InterNetwork);
            if (localIPAddress == null)
            {
                throw new PingClientException("送信元が見つかりません");
            }

            // IPアドレス(送信先)
            IPAddress remoteIpAddress = new IPAddress(Encoding.ASCII.GetBytes(ipAddress));
            if (remoteIpAddress == null)
            {
                throw new PingClientException("送信先が見つかりません：[" + ipAddress + "]");
            }

            // 結果オブジェクト生成
            this.m_Statistics = new PingStatistics(localIPAddress, remoteIpAddress);

            Debug.WriteLine("PING from {0} to {1}({2}) {3} bytes of data.",
                localIPAddress.ToString(),
                ipAddress,
                remoteIpAddress.ToString(),
                this.m_SendBuffer.Length);

            // 送信回数分繰り返す
            for (int i = 0; i < count; i++)
            {
                // 送信実行
                PingReply _PingReply = this.SendExec(remoteIpAddress.ToString());

                // 結果追加
                this.m_Statistics.PingReply.Add(_PingReply);

                // 結果表示(DEBUG)
                this.ShowPingReply(_PingReply);

                // 次回送信待ち
                Thread.Sleep(wait);
            }
        }

        /// <summary>
        /// 送信実行
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        private PingReply SendExec(string ipAddress)
        {
            // Ping送信
            return this.m_Ping.Send(ipAddress, this.m_Timeout, this.m_SendBuffer, this.m_Options);
        }

        /// <summary>
        /// 結果表示(DEBUG)
        /// </summary>
        /// <param name="pingReply"></param>
        private void ShowPingReply(PingReply pingReply)
        {
            //結果を取得
            if (pingReply.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                Debug.WriteLine("Reply from {0}:bytes={1} time={2}ms TTL={3}",
                    pingReply.Address, pingReply.Buffer.Length,
                    pingReply.RoundtripTime, pingReply.Options.Ttl);
            }
            else
            {
                Debug.WriteLine("Status={0}", pingReply.Status);
            }
        }

        /// <summary>
        /// Ping応答イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPingCompleted(object sender, PingCompletedEventArgs e)
        {
            Trace.WriteLine("PingClient::OnPingCompleted");

            if (e.Cancelled)
            {
                Console.WriteLine("Pingがキャンセルされました。");
            }
            else if (e.Error != null)
            {
                Console.WriteLine("エラー:" + e.Error.Message);
            }
            else
            {
                // 結果を取得
                if (e.Reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    Console.WriteLine("Reply from {0}:bytes={1} time={2}ms TTL={3}",
                        e.Reply.Address, e.Reply.Buffer.Length,
                        e.Reply.RoundtripTime, e.Reply.Options.Ttl);
                }
                else
                {
                    Console.WriteLine("Ping送信に失敗。({0})", e.Reply.Status);
                }
            }
        }
    }
}
