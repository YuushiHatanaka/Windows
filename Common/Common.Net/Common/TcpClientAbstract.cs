using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using log4net;
using log4net.Config;
using System.IO;
using System.Threading;

namespace Common.Net
{
    /// <summary>
    /// TCPクライアント抽象クラス
    /// </summary>
    public abstract class TcpClientAbstract
    {
        #region ホスト名
        /// <summary>
        /// ホスト名
        /// </summary>
        protected string m_Host = string.Empty;

        /// <summary>
        /// ホスト名
        /// </summary>
        public string Host { get { return this.m_Host; } }
        #endregion

        #region ポート番号
        /// <summary>
        /// ポート番号
        /// </summary>
        protected int m_Port = 0;

        /// <summary>
        /// ポート番号
        /// </summary>
        public int Port { get { return this.m_Port; } }
        #endregion

        #region IPアドレス
        /// <summary>
        /// IPアドレス
        /// </summary>
        protected string m_IpAddress = string.Empty;

        /// <summary>
        /// IPアドレス
        /// </summary>
        public string IpAddress { get { return this.m_IpAddress; } }
        #endregion

        #region エンドポイント
        /// <summary>
        /// エンドポイント
        /// </summary>
        protected IPEndPoint m_IPEndPoint = null;

        /// <summary>
        /// エンドポイント
        /// </summary>
        public IPEndPoint IPEndPoint { get { return this.m_IPEndPoint; } }
        #endregion

        #region TCPタイムアウト
        /// <summary>
        /// TCPタイムアウト
        /// </summary>
        protected TcpTimeout m_Timeout = new TcpTimeout();

        /// <summary>
        /// TCPタイムアウト
        /// </summary>
        public TcpTimeout Timeout { get { return this.m_Timeout; } }
        #endregion

        #region 送信バッファサイズ
        /// <summary>
        /// 送信バッファサイズ
        /// </summary>
        protected int m_SendBufferCapacity = 2046;

        /// <summary>
        /// 送信バッファサイズ
        /// </summary>
        public int SendBufferCapacity
        {
            get { return this.m_SendBufferCapacity; }
            set { this.m_SendBufferCapacity = value; }
        }
        #endregion

        #region 受信バッファサイズ
        /// <summary>
        /// 受信バッファサイズ
        /// </summary>
        protected int m_ReciveBufferCapacity = 2046;

        /// <summary>
        /// 受信バッファサイズ
        /// </summary>
        public int ReciveBufferCapacity
        {
            get { return this.m_ReciveBufferCapacity; }
            set { this.m_ReciveBufferCapacity = value; }
        }
        #endregion

        #region TCPタスクキャンセルトークン
        /// <summary>
        /// TCPタスクキャンセルトークン
        /// </summary>
        protected TcpCancellationTokenSource CancellationTokenSource = new TcpCancellationTokenSource();
        #endregion

        #region ユーザ名
        /// <summary>
        /// ユーザ名
        /// </summary>
        protected string m_UserName = string.Empty;

        /// <summary>
        /// ユーザ名
        /// </summary>
        public string UserName { get { return this.m_UserName; } }
        #endregion

        #region ユーザパスワード
        /// <summary>
        /// ユーザパスワード
        /// </summary>
        protected string m_UserPassword = string.Empty;

        /// <summary>
        /// ユーザパスワード
        /// </summary>
        public string UserPassword { get { return this.m_UserPassword; } }
        #endregion

        #region ログインプロンプト
        /// <summary>
        /// ログインプロンプト
        /// </summary>
        protected string m_LoginPrompt = @"^login: ";

        /// <summary>
        /// ログインプロンプト
        /// </summary>
        public string LoginPrompt { get { return this.m_LoginPrompt; } set { this.m_LoginPrompt = value; } }
        #endregion

        #region パスワードプロンプト
        /// <summary>
        /// パスワードプロンプト
        /// </summary>
        protected string m_PasswordPrompt = @"^Password:";

        /// <summary>
        /// パスワードプロンプト
        /// </summary>
        public string PasswordPrompt { get { return this.m_PasswordPrompt; } set { this.m_PasswordPrompt = value; } }
        #endregion

        #region コマンドプロンプト
        /// <summary>
        /// コマンドプロンプト
        /// </summary>
        protected string m_CommandPrompt = @"\$ $";

        /// <summary>
        /// コマンドプロンプト
        /// </summary>
        public string CommandPrompt { get { return this.m_CommandPrompt; } set { this.m_CommandPrompt = value; } }
        #endregion

        #region ロガー
        /// <summary>
        /// ロガー
        /// </summary>
        protected ILog Logger = null;
        #endregion

        #region 通知イベント
        /// <summary>
        /// 接続通知イベント
        /// </summary>
        public ManualResetEvent OnConnectNotify = new ManualResetEvent(false);

        /// <summary>
        /// 受信完了通知イベント
        /// </summary>
        public ManualResetEvent OnReciveNotify = new ManualResetEvent(false);

        /// <summary>
        /// 送信完了通知イベント
        /// </summary>
        public ManualResetEvent OnSendNotify = new ManualResetEvent(false);

        /// <summary>
        /// 切断通知イベント
        /// </summary>
        public ManualResetEvent OnDisconnectNotify = new ManualResetEvent(false);
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化
        /// </summary>
        protected virtual void Initialization(string host, int port, string userName, string userPasword)
        {
            #region 初期設定
            // 初期設定
            this.m_Host = host;
            this.m_Port = port;
            this.m_UserName = userName;
            this.m_UserPassword = userPasword;
            #endregion

            // エンドポイント設定
            this.m_IPEndPoint = this.GetIPEndPoint(host, port);

            #region ロギング設定
            // ログ設定があるxmlのパス
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4Net.xml");

            // Fileinfoタイプに宣言する。
            FileInfo file = new FileInfo(logPath);

            // LogManagerに設定する。
            XmlConfigurator.Configure(file);

            // ログ設定
            this.Logger = LogManager.GetLogger(typeof(TelnetClient));
            #endregion
        }
        #endregion

        #region エンドポイント設定
        /// <summary>
        /// エンドポイント設定
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        protected IPEndPoint GetIPEndPoint(string host, int port)
        {
            try
            {
                // ホスト名がIPアドレスとみなしてIPアドレスの解析
                IPAddress hostAddress = IPAddress.Parse(host);

                // ホストIPアドレス設定
                this.m_IpAddress = host;

                // エンドポイント返却
                return new IPEndPoint(hostAddress, port);
            }
            catch
            {
                // DNSに問合せ(IPv4)
                this.m_IpAddress = this.GetIPAddress(host, AddressFamily.InterNetwork);

                // エンドポイント返却
                return new IPEndPoint(IPAddress.Parse(this.m_IpAddress), port);
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
        protected string GetIPAddress(string host, AddressFamily addressFamily)
        {
            // ホスト名（またはIPアドレス）解決
            IPHostEntry ipentry = Dns.GetHostEntry(host);

            // IPアドレスを繰り返す
            foreach (IPAddress ip in ipentry.AddressList)
            {
                // アドレスファミリーが一致するか？
                if (ip.AddressFamily == addressFamily)
                {
                    // 一致したIPアドレス(文字列)を返却
                    return ip.ToString();
                }
            }

            // 該当なし(空文字)を返却
            return string.Empty;
        }
        #endregion
   }
}
