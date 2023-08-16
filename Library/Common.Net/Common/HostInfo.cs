using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// HostInfoクラス
    /// </summary>
    public class HostInfo
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region ホスト名
        /// <summary>
        /// ホスト名
        /// </summary>
        public string Host { get; private set; } = string.Empty;
        #endregion

        #region ポート番号
        /// <summary>
        /// ポート番号
        /// </summary>
        public int Port { get; private set; } = 0;
        #endregion

        #region ネットワークエンドポイント
        /// <summary>
        /// ネットワークエンドポイント
        /// </summary>
        public IPEndPoint IPEndPoint = null;
        #endregion

        #region IPアドレス
        /// <summary>
        /// IPアドレス
        /// </summary>
        public IPAddress IPAddress = null;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public HostInfo(string host, int port)
        {
            // ロギング
            Logger.Debug("=>>>> HostInfo::HostInfo(string, int)");
            Logger.DebugFormat("host:{0}", host);
            Logger.DebugFormat("port:{0}", port);

            // 設定
            Host = host;
            Port = port;

            // エンドポイント取得
            IPEndPoint = GetIPEndPoint(host, port);

            // ロギング
            Logger.Debug("<<<<= HostInfo::HostInfo(string, int)");
        }
        #endregion

        /// <summary>
        /// 文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // 返却
            return IPAddress.ToString();
        }

        #region エンドポイント設定
        /// <summary>
        /// エンドポイント設定
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        private IPEndPoint GetIPEndPoint(string host, int port)
        {
            // ロギング
            Logger.Debug("=>>>> HostInfo::GetIPEndPoint(string, int)");
            Logger.DebugFormat("host:{0}", host);
            Logger.DebugFormat("port:{0}", port);

            try
            {
                // ホスト名がIPアドレスとみなしてIPアドレスの解析
                IPAddress = IPAddress.Parse(host);

                // エンドポイント取得
                IPEndPoint result = new IPEndPoint(IPAddress, port);

                // ロギング
                Logger.DebugFormat("IPAddress:{0}", IPAddress.ToString());
                Logger.DebugFormat("result   :{0}", result.ToString());
                Logger.Debug("<<<<= HostInfo::GetIPEndPoint(string, int)");

                // 返却
                return result;
            }
            catch
            {
                // DNSに問合せ(IPv4)
                string ipAddress = GetIPAddress(host, AddressFamily.InterNetwork);

                // エンドポイント取得
                IPEndPoint result = new IPEndPoint(IPAddress.Parse(ipAddress), port);

                // ロギング
                Logger.DebugFormat("ipAddress:{0}", ipAddress);
                Logger.DebugFormat("result:{0}", result.ToString());
                Logger.Debug("<<<<= HostInfo::GetIPEndPoint(string, int)");

                // 返却
                return result;
            }
        }
        #endregion

        #region IPアドレス取得
        /// <summary>
        /// IPアドレス取得
        /// </summary>
        /// <param name="host"></param>
        /// <param name="addressFamily"></param>
        /// <returns></returns>
        private string GetIPAddress(string host, AddressFamily addressFamily)
        {
            // ロギング
            Logger.Debug("=>>>> HostInfo::GetIPAddress(string, AddressFamily)");
            Logger.DebugFormat("host         :{0}", host);
            Logger.DebugFormat("AddressFamily:{0}", addressFamily.ToString());

            // ホスト名（またはIPアドレス）解決
            IPHostEntry ipentry = Dns.GetHostEntry(host);

            // IPアドレスを繰り返す
            foreach (IPAddress ip in ipentry.AddressList)
            {
                // アドレスファミリーが一致するか？
                if (ip.AddressFamily == addressFamily)
                {
                    // 設定
                    IPAddress = ip;

                    // ロギング
                    Logger.DebugFormat("ip:{0}", ip.ToString());
                    Logger.Debug("<<<<= HostInfo::GetIPAddress(string, AddressFamily)");

                    // 一致したIPアドレス(文字列)を返却
                    return ip.ToString();
                }
            }

            // ロギング
            Logger.Debug("ip:string.Empty");
            Logger.Debug("<<<<= HostInfo::GetIPAddress(string, AddressFamily)");

            // 該当なし(空文字)を返却
            return string.Empty;
        }
        #endregion
    }
}
