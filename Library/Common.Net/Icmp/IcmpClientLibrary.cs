using Common.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// IcmpClientLibraryクラス
    /// </summary>
    public partial class IcmpClientLibrary : UdpClientLibrary
    {
        #region コンストラクタ
        /// <summary>
        /// Pingオブジェクト
        /// </summary>
        private Ping m_Ping = null;
        #endregion

        #region オプション
        /// <summary>
        /// オプション
        /// </summary>
        private PingOptions m_Options = null;
        #endregion

        #region 送信バッファ
        /// <summary>
        /// 送信バッファ
        /// </summary>
        private byte[] m_SendBuffer = null;
        #endregion

        /// <summary>
        /// タイムアウト
        /// </summary>
        private TimeSpan m_Timeout = new TimeSpan(0, 0, 10);

        /// <summary>
        /// 結果クラス
        /// </summary>
        private IcmpStatistics m_Statistics = null;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        public IcmpClientLibrary(string host)
            : this(host, 0)
        {
            // ロギング
            Logger.Debug("=>>>> IcmpClientLibrary::IcmpClientLibrary(string)");
            Logger.DebugFormat("host:{0}", host);

            // ロギング
            Logger.Debug("<<<<= IcmpClientLibrary::IcmpClientLibrary(string)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public IcmpClientLibrary(string host, int port)
            : base(host, port)
        {
            // ロギング
            Logger.Debug("=>>>> IcmpClientLibrary::IcmpClientLibrary(string, int)");
            Logger.DebugFormat("host:{0}", host);
            Logger.DebugFormat("port:{0}", port);

            // オブジェクト生成
            m_Ping = new Ping();
            m_Options = new PingOptions(128, false);
            m_SendBuffer = Encoding.ASCII.GetBytes(new string('A', 32));

            // ロギング
            Logger.Debug("<<<<= IcmpClientLibrary::IcmpClientLibrary(string, int)");
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            // ロギング
            Logger.Debug("=>>>> IcmpClientLibrary::Dispose(bool)");

            if (!m_Disposed)
            {
                if (disposing)
                {
                    // TODO: Dispose managed resources here.
                }

                // TODO: Free unmanaged resources here.

                // Note disposing has been done.
                m_Disposed = true;
            }

            // ロギング
            Logger.Debug("<<<<= IcmpClientLibrary::Dispose(bool)");
        }
        #endregion  

        #region 送信(同期)
        /// <summary>
        /// 送信(同期)
        /// </summary>
        /// <param name="wait"></param>
        /// <exception cref="IcmpClientException"></exception>
        public void Send(int wait)
        {
            // ロギング
            Logger.Debug("=>>>> IcmpClientLibrary::Send(int)");
            Logger.DebugFormat("wait:{0}", wait);

            // 送信初期化
            SendInitialization();

            // 繰り返し
            while (true)
            {
                // キャンセル判定
                if (m_CancellationTokenSource.IsCancellationRequested)
                {
                    // 繰り返し終了
                    break;
                }

                // 送信実行
                SendExec(m_HostInfo.IPAddress.ToString(), wait);
            }

            // ロギング
            Logger.Debug("<<<<= IcmpClientLibrary::Send(int)");
        }

        /// <summary>
        /// 送信(同期)
        /// </summary>
        /// <param name="count"></param>
        /// <param name="wait"></param>
        /// <exception cref="IcmpClientException"></exception>
        public void Send(int count, int wait)
        {
            // ロギング
            Logger.Debug("=>>>> IcmpClientLibrary::Send(int, int)");
            Logger.DebugFormat("count:{0}", count);
            Logger.DebugFormat("wait :{0}", wait);

            // 送信初期化
            SendInitialization();

            // 送信回数分繰り返す
            for (int i = 0; i < count; i++)
            {
                // キャンセル判定
                if (m_CancellationTokenSource.IsCancellationRequested)
                {
                    // 繰り返し終了
                    break;
                }

                // 送信実行
                SendExec(m_HostInfo.IPAddress.ToString(), wait);
            }

            // ロギング
            Logger.Debug("<<<<= IcmpClientLibrary::Send(int, int)");
        }

        /// <summary>
        /// 送信初期化
        /// </summary>
        private void SendInitialization()
        {
            // IPアドレス(送信元)
            IPAddress localIPAddress = CommonLibrary.GetLocalIPAddress(AddressFamily.InterNetwork);
            if (localIPAddress == null)
            {
                throw new IcmpClientException("送信元が見つかりません");
            }

            // 結果オブジェクト生成
            m_Statistics = new IcmpStatistics(localIPAddress, m_HostInfo.IPAddress);

            // ロギング
            Logger.InfoFormat("PING from {0} to {1}({2}) {3} bytes of data.",
                    localIPAddress.ToString(),
                    m_HostInfo.Host,
                    m_HostInfo.IPAddress.ToString(),
                    m_SendBuffer.Length);
        }

        /// <summary>
        /// 送信実行
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="wait"></param>
        /// <returns></returns>
        private PingReply SendExec(string ipAddress, int wait)
        {
            // ロギング
            Logger.Debug("=>>>> IcmpClientLibrary::SendExec(string, int)");
            Logger.DebugFormat("ipAddress:{0}", ipAddress);
            Logger.DebugFormat("wait     :{0}", wait);

            // Ping送信
            PingReply pingReply = m_Ping.Send(ipAddress, (int)m_Timeout.TotalMilliseconds, m_SendBuffer, m_Options);

            // 結果追加
            m_Statistics.PingReplys.Add(pingReply);

            // ロギング
            Logger.InfoFormat(IcmpClientLibrary.ShowPingReply(pingReply));

            // 次回送信待ち
            Thread.Sleep(wait);

            // ロギング
            Logger.Debug("<<<<= IcmpClientLibrary::SendExec(string, int)");

            // 返却
           return pingReply;
        }

        /// <summary>
        /// 結果文字列取得
        /// </summary>
        /// <param name="pingReply"></param>
        public static string ShowPingReply(PingReply pingReply)
        {
            // 結果オブジェクト生成
            StringBuilder result = new StringBuilder();

            //結果を取得
            if (pingReply.Status == IPStatus.Success)
            {
                // 文字列作成
                result.AppendFormat("Reply from {0}:bytes={1} time={2}ms TTL={3}",
                    pingReply.Address, pingReply.Buffer.Length,
                    pingReply.RoundtripTime, pingReply.Options.Ttl);
            }
            else
            {
                // 文字列作成
                result.AppendFormat("Status={0}", pingReply.Status);
            }

            // 返却
            return result.ToString();
        }
        #endregion

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
                if (e.Reply.Status == IPStatus.Success)
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
