using Renci.SshNet;
using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Common.Diagnostics;

namespace Common.Net
{
    /// <summary>
    /// SSHクライアント(Wrap)クラス
    /// </summary>
    public class SshClientWrap : IDisposable
    {
        /// <summary>
        /// ホスト名
        /// </summary>
        protected string m_Host = string.Empty;

        /// <summary>
        /// ポート番号
        /// </summary>
        protected int m_Port = 22;

        /// <summary>
        /// IPアドレス
        /// </summary>
        protected string m_IpAddress = string.Empty;

        /// <summary>
        /// サーバエンドポイント
        /// </summary>
        protected IPEndPoint m_IPEndPoint = null;

        /// <summary>
        /// ユーザ名
        /// </summary>
        protected string m_UserName = string.Empty;

        /// <summary>
        /// ユーザパスワード
        /// </summary>
        protected string m_UserPassword = string.Empty;

        /// <summary>
        /// 接続情報
        /// </summary>
        protected ConnectionInfo m_ConnectionInfo = null;

        /// <summary>
        /// SSHクライアントオブジェクト
        /// </summary>
        protected SshClient m_SshClient = null;

        /// <summary>
        /// SSHシェルStreamオブジェクト
        /// </summary>
        protected ShellStream m_ShellStream = null;

        /// <summary>
        /// 破棄フラグ
        /// </summary>
        private bool m_Disposed = false;

        /// <summary>
        /// ロガー
        /// </summary>
        protected ILog Logger = null;

        #region タイムアウト
        /// <summary>
        /// TCPタイムアウト
        /// </summary>
        private TcpTimeout m_Timeout = new TcpTimeout();

        /// <summary>
        /// TCPタイムアウト
        /// </summary>
        public TcpTimeout Timeout { get { return this.m_Timeout; } }
        #endregion

        #region 送信バッファサイズ
        /// <summary>
        /// 送信バッファサイズ
        /// </summary>
        private int m_SendBufferCapacity = 8192;

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
        private int m_ReciveBufferCapacity = 8192;

        /// <summary>
        /// 受信バッファサイズ
        /// </summary>
        public int ReciveBufferCapacity
        {
            get { return this.m_ReciveBufferCapacity; }
            set { this.m_ReciveBufferCapacity = value; }
        }
        #endregion

        #region 通知イベント
        /// <summary>
        /// 接続通知イベント
        /// </summary>
        private ManualResetEvent OnConnectNotify = new ManualResetEvent(false);

        /// <summary>
        /// 受信完了通知イベント
        /// </summary>
        private ManualResetEvent OnReciveNotify = new ManualResetEvent(false);

        /// <summary>
        /// 送信完了通知イベント
        /// </summary>
        private ManualResetEvent OnSendNotify = new ManualResetEvent(false);

        /// <summary>
        /// 切断通知イベント
        /// </summary>
        private ManualResetEvent OnDisconnectNotify = new ManualResetEvent(false);
        #endregion

        #region イベント定義
        /// <summary>
        /// 接続イベント
        /// </summary>
        public SshClientConnectedEventHandler OnConnected;

        /// <summary>
        /// 受信イベント
        /// </summary>
        public SshClientReciveEventHandler OnRecive;

        /// <summary>
        /// 送信イベント
        /// </summary>
        public SshClientSendEventHandler OnSend;

        /// <summary>
        /// 切断イベント
        /// </summary>
        public SshClientDisonnectedEventHandler OnDisconnected;
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

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        public SshClientWrap(string host)
            : this(host, 22)
        {
            // 初期化
            this.Initialization();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public SshClientWrap(string host, int port)
        {
            // 初期化
            this.Initialization();

            // ホスト名設定
            this.m_Host = host;

            //　ポート番号設定
            this.m_Port = port;

            try
            {
                // ホスト名がIPアドレスとみなしてIPアドレスの解析
                IPAddress hostAddress = IPAddress.Parse(host);

                // ホストIPアドレス設定
                this.m_IpAddress = host;

                // エンドポイント設定
                this.m_IPEndPoint = new IPEndPoint(hostAddress, port);
            }
            catch
            {
                // DNSに問合せ(IPv4)
                this.m_IpAddress = this.GetIPAddress(host, AddressFamily.InterNetwork);

                // エンドポイント設定
                this.m_IPEndPoint = new IPEndPoint(IPAddress.Parse(this.m_IpAddress), port);
            }
        }
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
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

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~SshClientWrap()
        {
            // 破棄
            this.Dispose(false);
        }
        #endregion

        #region 破棄
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 破棄
        /// </summary>
        /// <param name="isDisposing"></param>
        protected virtual void Dispose(bool isDisposing)
        {
            // 破棄しているか？
            if (!this.m_Disposed)
            {
                // アンマネージドリソース解放

                // マネージドリソース解放
                if (isDisposing)
                {
                    // 切断
                    this.DisConnect();
                }

                // 破棄済みを設定
                this.m_Disposed = true;
            }
        }
        #endregion

        #region 接続
        /// <summary>
        /// 接続
        /// </summary>
        public void Connect()
        {
            // Taskオブジェクト生成
            using (CancellationTokenSource source = new CancellationTokenSource())
            {
                // タイムアウト設定
                source.CancelAfter(this.m_Timeout.Connect);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        // 接続情報設定
                        this.m_ConnectionInfo = new ConnectionInfo(
                            this.m_IpAddress,
                            this.m_Port,
                            this.m_UserName,
                            new AuthenticationMethod[] {
                        new PasswordAuthenticationMethod(this.m_UserName, this.m_UserPassword)
                            }
                        );

                        // SSHクライアントオブジェクト生成
                        this.m_SshClient = new SshClient(this.m_ConnectionInfo);

                        // 接続
                        this.m_SshClient.Connect();

                        // 接続判定
                        if (!this.m_SshClient.IsConnected)
                        {
                            // 異常終了
                            throw new SshClientExceptions("サーバ接続(SSH)に失敗しました:[" + this.m_IpAddress + ":" + this.m_Port.ToString() + "]");
                        }

                        // ロギング
                        this.Logger.InfoFormat("接続：[ ⇒ {0}:{1}]", this.m_SshClient.ConnectionInfo.Host, this.m_SshClient.ConnectionInfo.Port);

                        // ShellStreamオブジェクト取得
                        // TODO:値暫定
                        this.m_ShellStream = this.m_SshClient.CreateShellStream(this.m_IpAddress, 80, 24, 800, 600, 4096);
                    }
                    catch(Exception ex)
                    {
                        // 例外
                        throw new SshClientExceptions("サーバ接続(SSH)に失敗しました:[" + this.m_IpAddress + ":" + this.m_Port.ToString() + "]", ex);
                    }
                }, source.Token);

                try
                {
                    // タスク待ち
                    task.Wait(source.Token);

                    // 通知リセット
                    this.OnConnectNotify.Reset();

                    // イベント呼出判定
                    if (this.OnConnected != null)
                    {
                        // イベントパラメータ設定
                        SshClientConnectedEventArgs _SshClientConnectedEventArgs = new SshClientConnectedEventArgs()
                        {
                            ConnectionInfo = this.m_ConnectionInfo,
                        };

                        // イベント呼出し
                        this.OnConnected(this, _SshClientConnectedEventArgs);
                    }

                }
                catch (OperationCanceledException ex)
                {
                    // 通知リセット
                    this.OnConnectNotify.Reset();

                    // 例外
                    throw new SshClientExceptions("サーバ接続(SSH)に失敗しました:[" + this.m_IpAddress + ":" + this.m_Port.ToString() + "]", ex);
                }
                catch (AggregateException ex)
                {
                    // 通知リセット
                    this.OnConnectNotify.Reset();

                    // 例外
                    throw new SshClientExceptions("サーバ接続(SSH)に失敗しました:[" + this.m_IpAddress + ":" + this.m_Port.ToString() + "]", ex);
                }
            }
        }
        #endregion

        #region 切断
        /// <summary>
        // ソケット通信接続の切断
        /// </summary>
        public void DisConnect()
        {
            // Taskオブジェクト生成
            using (CancellationTokenSource source = new CancellationTokenSource())
            {
                // タイムアウト設定
                source.CancelAfter(this.m_Timeout.Disconnect);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        // 接続終了
                        if (this.m_SshClient != null)
                        {
                            this.m_SshClient.Disconnect();
                        }
                    }
                    catch (Exception ex)
                    {
                        // 切断完了通知リセット
                        this.OnDisconnectNotify.Reset();

                        // 例外
                        throw new SshClientExceptions("サーバ切断(SSH)に失敗しました:[" + this.m_IpAddress + ":" + this.m_Port.ToString() + "]", ex);
                    }
                }, source.Token);

                try
                {
                    // タスク待ち
                    task.Wait(source.Token);

                    // 切断完了通知リセット
                    this.OnDisconnectNotify.Reset();

                    // ロギング
                    this.Logger.InfoFormat("切断：[ ⇒ {0}:{1}]", this.m_SshClient.ConnectionInfo.Host, this.m_SshClient.ConnectionInfo.Port);

                    // イベント呼出判定
                    if (this.OnDisconnected != null)
                    {
                        // イベントパラメータ設定
                        SshClientDisconnectedEventArgs _SshClientDisconnectedEventArgs = new SshClientDisconnectedEventArgs()
                        {
                            ConnectionInfo = this.m_ConnectionInfo,
                        };

                        // イベント呼出し
                        this.OnDisconnected(this, _SshClientDisconnectedEventArgs);
                    }

                }
                catch (OperationCanceledException ex)
                {
                    // 接続完了通知リセット
                    this.OnConnectNotify.Reset();

                    // 例外
                    throw new SshClientExceptions("サーバ切断(SSH)に失敗しました:[" + this.m_IpAddress + ":" + this.m_Port.ToString() + "]", ex);
                }
                catch (AggregateException ex)
                {
                    // 接続完了通知リセット
                    this.OnConnectNotify.Reset();

                    // 例外
                    throw new SshClientExceptions("サーバ切断(SSH)に失敗しました:[" + this.m_IpAddress + ":" + this.m_Port.ToString() + "]", ex);
                }
            }
        }
        #endregion

        #region 送信
        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="data"></param>
        public void Send(byte data)
        {
            // 送信データ生成
            byte[] senddata = new byte[1] { data };

            // 送信
            this.Send(new MemoryStream(senddata));
        }

        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="data"></param>
        public void Send(byte[] data)
        {
            // 送信
            this.Send(new MemoryStream(data));
        }

        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="stream"></param>
        public void Send(MemoryStream stream)
        {
            // 送信サイズ判定
            if (stream.Length == 0)
            {
                // 送信するものがないので、何もしない
                return;
            }

            // Taskオブジェクト生成
            using (CancellationTokenSource source = new CancellationTokenSource())
            {
                // タイムアウト設定
                source.CancelAfter(this.m_Timeout.Send);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    // 送信
                    this.m_ShellStream.Write(stream.ToArray(), 0, (int)stream.Length);
                }, source.Token);

                try
                {
                    // タスク待ち
                    task.Wait(source.Token);

                    // 通知リセット
                    this.OnSendNotify.Reset();

                    // イベント呼出判定
                    if (this.OnSend != null)
                    {
                        // イベントパラメータ設定
                        SshClientSendEventArgs _SshClientSendEventArgs = new SshClientSendEventArgs()
                        {
                            ConnectionInfo = this.m_ConnectionInfo,
                            Size = (int)stream.Length,
                            Stream = stream,
                        };

                        // イベント呼出し
                        this.OnSend(this, _SshClientSendEventArgs);
                    }

                }
                catch (OperationCanceledException ex)
                {
                    // 通知リセット
                    this.OnSendNotify.Reset();

                    // 例外
                    throw new SshClientExceptions("送信(SSH)に失敗しました:[" + this.m_IpAddress + ":" + this.m_Port.ToString() + "]", ex);
                }
                catch (AggregateException ex)
                {
                    // 通知リセット
                    this.OnSendNotify.Reset();

                    // 例外
                    throw new SshClientExceptions("送信(SSH)に失敗しました:[" + this.m_IpAddress + ":" + this.m_Port.ToString() + "]", ex);
                }
            }
        }
        #endregion

        #region 受信
        /// <summary>
        /// 受信
        /// </summary>
        /// <returns></returns>
        public MemoryStream Recive()
        {
            // 受信
            return this.Recive(this.m_ReciveBufferCapacity);
        }

        /// <summary>
        /// 受信
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public MemoryStream Recive(int size)
        {
            // 返却用オブジェクト生成
            MemoryStream memoryStream = new MemoryStream();

            // Taskオブジェクト生成
            using (CancellationTokenSource source = new CancellationTokenSource())
            {
                // タイムアウト設定
                source.CancelAfter(this.m_Timeout.Recv);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    // ShellStreamにバッファ長が0以上になるまで待合せ
                    while (this.m_ShellStream.Length == 0)
                    {
                        Thread.Sleep(100);
                    }

                    // 読込
                    byte[] _readbuffer = new byte[this.m_ShellStream.Length];
                    if (this.m_ShellStream.Read(_readbuffer, 0, _readbuffer.Length) != 0)
                    {
                        // 文字コード変換
                        System.Text.Encoding src = Common.Text.Encoding.GetCode(_readbuffer);
                        System.Text.Encoding dest = System.Text.Encoding.GetEncoding("Shift_JIS");
                        byte[] temp = System.Text.Encoding.Convert(src, dest, _readbuffer);
                        memoryStream.Write(temp, 0, temp.Length);
                    }
                }, source.Token);

                try
                {
                    // タスク待ち
                    task.Wait(source.Token);

                    // 通知リセット
                    this.OnReciveNotify.Reset();

                    // イベント呼出判定
                    if (this.OnRecive != null)
                    {
                        // イベントパラメータ設定
                        SshClientReciveEventArgs _SshClientReciveEventArgs = new SshClientReciveEventArgs()
                        {
                            ConnectionInfo = this.m_ConnectionInfo,
                            Size = (int)memoryStream.Length,
                            Stream = memoryStream,
                        };

                        // イベント呼出し
                        this.OnRecive(this, _SshClientReciveEventArgs);
                    }

                    // 結果返却
                    return memoryStream;
                }
                catch (OperationCanceledException ex)
                {
                    // 通知リセット
                    this.OnReciveNotify.Reset();

                    // 例外
                    throw new SshClientExceptions("受信(SSH)に失敗しました:[" + this.m_IpAddress + ":" + this.m_Port.ToString() + "]", ex);
                }
                catch (AggregateException ex)
                {
                    // 通知リセット
                    this.OnReciveNotify.Reset();

                    // 例外
                    throw new SshClientExceptions("受信(SSH)に失敗しました:[" + this.m_IpAddress + ":" + this.m_Port.ToString() + "]", ex);
                }
            }
        }
        #endregion
    }
}
