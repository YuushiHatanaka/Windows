using PrimS.Telnet;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Common.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace Common.Net
{
    /// <summary>
    /// NtpClientLibraryクラス
    /// </summary>
    public partial class NtpClientLibrary : UdpClientLibrary
    {
        #region クライアントオブジェクト
        /// <summary>
        /// クライアントオブジェクト
        /// </summary>
        private UdpClient m_Client = null;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        public NtpClientLibrary(string host)
            : base(host, 123)
        {
            // ロギング
            Logger.Debug("=>>>> NtpClientLibrary::NtpClientLibrary(string)");
            Logger.DebugFormat("host:{0}", host);

            // ロギング
            Logger.Debug("<<<<= NtpClientLibrary::NtpClientLibrary(string)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public NtpClientLibrary(string host, int port)
            : base(host, port)
        {
            // ロギング
            Logger.Debug("=>>>> NtpClientLibrary::NtpClientLibrary(string, int)");
            Logger.DebugFormat("host:{0}", host);
            Logger.DebugFormat("port:{0}", port);

            // ロギング
            Logger.Debug("<<<<= NtpClientLibrary::NtpClientLibrary(string, int)");
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
            Logger.Debug("=>>>> NtpClientLibrary::Dispose(bool)");

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
            Logger.Debug("<<<<= NtpClientLibrary::Dispose(bool)");
        }
        #endregion  

        #region 接続(同期)
        /// <summary>
        /// 接続(同期)
        /// </summary>
        public override bool Connect()
        {
            // ロギング
            Logger.Debug("=>>>> NtpClientLibrary::Connect()");

            // UdpClientオブジェクトを生成
            m_Client = new UdpClient();

            // 接続
            m_Client.Connect(m_HostInfo.IPEndPoint);

            // ロギング
            Logger.Debug("<<<<= NtpClientLibrary::Connect()");

            // 正常終了
            return true;
        }
        #endregion

        #region 切断(同期)
        /// <summary>
        /// 切断(同期)
        /// </summary>
        public override bool Disconnect()
        {
            // ロギング
            Logger.Debug("=>>>> NtpClientLibrary::Disconnect()");

            // 切断
            m_Client?.Close();
            m_Client?.Dispose();
            m_Client = null;

            // ロギング
            Logger.Debug("<<<<= NtpClientLibrary::Disconnect()");

            // 正常終了
            return true;
        }
        #endregion

        #region 送信(同期)
        /// <summary>
        /// 送信(同期)
        /// </summary>
        public void Send()
        {
            // ロギング
            Logger.Debug("=>>>> NtpClientLibrary::Send()");

            // NtpPacketオブジェクト取得
            NtpPacket packet = NtpPacket.CreateSendPacket();

            // 送信
            m_Client.Send(packet.PacketData, packet.PacketData.GetLength(0));

            // 送信内容表示
            Logger.InfoFormat("送信内容：【{0}:{1}】\n{2}\n＜詳細＞\n{3}",
                m_HostInfo.Host,
                m_HostInfo.Port,
                Dump.ToString(packet.PacketData),
                packet.ToString());

            // ロギング
            Logger.Debug("<<<<= NtpClientLibrary::Send()");
        }
        #endregion

        #region 受信(同期)
        /// <summary>
        /// 受信(同期)
        /// </summary>
        /// <returns></returns>
        public NtpPacket Receive()
        {
            // ロギング
            Logger.Debug("=>>>> NtpClientLibrary::Receive()");

            // 受信
            byte[] receiveData = m_Client.Receive(ref m_HostInfo.IPEndPoint);

            // NtpPacketオブジェクト生成
            NtpPacket packet = new NtpPacket(receiveData);

            // 受信内容表示
            Logger.InfoFormat("受信内容：【{0}:{1}】\n{2}\n＜詳細＞\n{3}",
                m_HostInfo.Host,
                m_HostInfo.Port,
                Dump.ToString(packet.PacketData),
                packet.ToString());

            // ロギング
            Logger.Debug("<<<<= NtpClientLibrary::Receive()");

            // 返却
            return packet;
        }
        #endregion

        #region 実行(同期)
        /// <summary>
        /// 実行(同期)
        /// </summary>
        /// <returns></returns>
        public NtpPacket Excexute()
        {
            // ロギング
            Logger.Debug("=>>>> NtpClientLibrary::GetData()");

            // 送信
            Send();

            // 受信
            NtpPacket result = Receive();

            // ロギング
            Logger.Debug(Dump.ToString(result.PacketData));
            Logger.Debug("<<<<= NtpClientLibrary::GetData()");

            // 返却
            return result;
        }
        #endregion
    }
}
