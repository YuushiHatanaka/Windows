using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Common.Diagnostics;

namespace Common.Net
{
    /// <summary>
    /// Telnetクライアントクラス
    /// </summary>
    public class TelnetClient : IDisposable
    {
        /// <summary>
        /// ホスト名
        /// </summary>
        private string m_Host = string.Empty;

        /// <summary>
        /// ポート番号
        /// </summary>
        private int m_Port = 23;

        /// <summary>
        /// IPアドレス
        /// </summary>
        private string m_IpAddress = string.Empty;

        /// <summary>
        /// サーバエンドポイント
        /// </summary>
        private IPEndPoint m_IPEndPoint = null;

        /// <summary>
        /// ソケット
        /// </summary>
        private Socket m_Socket = null;

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
        public TelnetClientConnectedEventHandler OnConnected;

        /// <summary>
        /// 受信イベント
        /// </summary>
        public TelnetClientReciveEventHandler OnRecive;

        /// <summary>
        /// 送信イベント
        /// </summary>
        public TelnetClientSendEventHandler OnSend;

        /// <summary>
        /// 切断イベント
        /// </summary>
        public TelnetClientDisonnectedEventHandler OnDisconnected;
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
        public TelnetClient(string host)
            : this(host, 23)
        {
            // 初期化
            this.Initialization();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public TelnetClient(string host, int port)
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
        ~TelnetClient()
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
            // ソケット生成
            this.m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.m_Socket.ReceiveBufferSize = this.m_ReciveBufferCapacity;
            this.m_Socket.SendBufferSize = this.m_SendBufferCapacity;
            this.m_Socket.ReceiveTimeout = this.m_Timeout.Recv;
            this.m_Socket.SendTimeout = this.m_Timeout.Send;
            this.m_Socket.Blocking = false;

            // 非同期で接続を待機
            IAsyncResult _result = this.m_Socket.BeginConnect(this.m_IPEndPoint, new AsyncCallback(this.OnConnectCallBack), this.m_Socket);

            // 接続待ち
            if (!this.OnConnectNotify.WaitOne(this.m_Timeout.Connect))
            {
                // 接続完了通知リセット
                this.OnConnectNotify.Reset();

                // 例外
                throw new TelnetClientException("接続タイムアウト:[" + this.m_Timeout.Connect + "]");
            }

            // 接続完了通知リセット
            this.OnConnectNotify.Reset();
        }

        /// <summary>
        /// 非同期接続のコールバックメソッド(別スレッドで実行される)
        /// </summary>
        /// <param name="asyncResult"></param>
        private void OnConnectCallBack(IAsyncResult asyncResult)
        {
            try
            {
                // ソケット取得
                Socket _Socket = (Socket)(asyncResult.AsyncState);

                // ロギング
                this.Logger.InfoFormat("接続：[{0} ⇒ {1}]", _Socket.LocalEndPoint.ToString(), _Socket.RemoteEndPoint.ToString());

                // ソケットが切断されているか？
                if (_Socket == null || !_Socket.Connected)
                {
                    return;
                }

                // 接続完了通知を設定
                this.OnConnectNotify.Set();

                // イベント呼出判定
                if (this.OnConnected != null)
                {
                    // イベントパラメータ設定
                    TelnetClientConnectedEventArgs _TelnetClientConnectedEventArgs = new TelnetClientConnectedEventArgs()
                    {
                        LocalEndPoint = _Socket.LocalEndPoint,
                        RemoteEndPoint = _Socket.RemoteEndPoint,
                    };

                    // イベント呼出し
                    this.OnConnected(this, _TelnetClientConnectedEventArgs);
                }
            }
            catch
            {
                // 接続完了通知リセット
                this.OnConnectNotify.Reset();
                return;
            }
        }
        #endregion

        #region 切断
        /// <summary>
        // ソケット通信接続の切断
        /// </summary>
        public void DisConnect()
        {
            // 切断判定
            if (!(this.m_Socket != null && this.m_Socket.Connected))
            {
                // 切断済みなので処理を終了
                return;
            }

            // 切断開始
            IAsyncResult _result = this.m_Socket.BeginDisconnect(false, new AsyncCallback(this.OnDisconnectCallBack), this.m_Socket);

            // 切断待ち
            if (!this.OnDisconnectNotify.WaitOne(this.m_Timeout.Disconnect))
            {
                // 切断完了通知リセット
                this.OnDisconnectNotify.Reset();

                // 例外
                throw new TelnetClientException("切断タイムアウト:[" + this.m_Timeout.Disconnect + "]");
            }

            // 切断完了通知リセット
            this.OnDisconnectNotify.Reset();
        }

        /// <summary>
        /// 非同期切断のコールバックメソッド(別スレッドで実行される)
        /// </summary>
        /// <param name="asyncResult"></param>
        private void OnDisconnectCallBack(IAsyncResult asyncResult)
        {
            // ソケット取得
            Socket _Socket = (Socket)(asyncResult.AsyncState);

            // ロギング
            this.Logger.InfoFormat("切断：[{0} ⇒ {1}]", _Socket.LocalEndPoint.ToString(), _Socket.RemoteEndPoint.ToString());

            // 切断・破棄
            if (_Socket != null)
            {
                _Socket.Disconnect(false);
                _Socket.Dispose();
            }

            // 切断通知
            this.OnDisconnectNotify.Set();

            // イベント呼出判定
            if (this.OnDisconnected != null)
            {
                // イベントパラメータ設定
                TelnetClientDisconnectedEventArgs _TelnetClientDisconnectedEventArgs = new TelnetClientDisconnectedEventArgs()
                {
                    RemoteEndPoint = this.m_IPEndPoint,
                };

                // イベント呼出
                this.OnDisconnected(this, _TelnetClientDisconnectedEventArgs);
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

            // 送信用Stream生成
            TelnetClientSendStream _TelnetClientSendStream = new TelnetClientSendStream();
            _TelnetClientSendStream.Stream = stream;
            _TelnetClientSendStream.Socket = this.m_Socket;

            // 送信開始
            IAsyncResult _result = this.m_Socket.BeginSend(stream.ToArray(), 0, stream.ToArray().Length, SocketFlags.None, new AsyncCallback(this.OnSendCallBack), _TelnetClientSendStream);

            // 送信応答待ち
            if (!this.OnSendNotify.WaitOne(this.m_Timeout.Send))
            {
                // 通知リセット
                this.OnSendNotify.Reset();

                // 例外
                throw new TelnetClientException("送信タイムアウト:[" + this.m_Timeout.Send + "]");
            }

            // 通知リセット
            this.OnSendNotify.Reset();
        }

        /// <summary>
        /// 非同期送信のコールバックメソッド(別スレッドで実行される)
        /// </summary>
        /// <param name="asyncResult"></param>
        private void OnSendCallBack(IAsyncResult asyncResult)
        {
            // オブジェクト変換
            TelnetClientSendStream _TelnetClientSendStream = (TelnetClientSendStream)asyncResult.AsyncState;

            // ソケットが切断されているか？
            if (_TelnetClientSendStream.Socket == null || !_TelnetClientSendStream.Socket.Connected)
            {
                return;
            }

            // 送信完了
            int bytesSent = _TelnetClientSendStream.Socket.EndSend(asyncResult);

            // ロギング
            this.Logger.InfoFormat("送信データ - {0:#,0} byte：\n{1}", _TelnetClientSendStream.Stream.Length, Debug.Dump(1, _TelnetClientSendStream.Stream));

            // 送信完了通知
            this.OnSendNotify.Set();

            // イベント呼出
            if (this.OnSend != null)
            {
                // イベントパラメータ生成
                TelnetClientSendEventArgs _TelnetClientSendEventArgs = new TelnetClientSendEventArgs()
                {
                    Socket = _TelnetClientSendStream.Socket,
                    Size = bytesSent,
                    Stream = _TelnetClientSendStream.Stream,
                };

                // イベント呼出し
                this.OnSend(this, _TelnetClientSendEventArgs);
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
            // 受信用Stream生成
            TelnetClientReciveStream _TelnetClientReciveStream = new TelnetClientReciveStream();
            _TelnetClientReciveStream.Socket = this.m_Socket;
            _TelnetClientReciveStream.Buffer = new byte[size];
            _TelnetClientReciveStream.Stream = new MemoryStream(size);

            // 結果受信待ち
            IAsyncResult _result = this.m_Socket.BeginReceive(_TelnetClientReciveStream.Buffer, 0, _TelnetClientReciveStream.Buffer.Length, SocketFlags.None, this.OnReciveCallback, _TelnetClientReciveStream);

            // データ受信待ち
            if (!this.OnReciveNotify.WaitOne(this.m_Timeout.Recv))
            {
                // 受信通知リセット
                this.OnReciveNotify.Reset();

                // 例外
                throw new TelnetClientException("受信タイムアウト:[" + this.m_Timeout.Recv + "]");
            }

            // 受信通知リセット
            this.OnReciveNotify.Reset();

            // MemoryStreamを返却
            return _TelnetClientReciveStream.Stream;
        }

        /// <summary>
        /// 非同期受信のコールバックメソッド(別スレッドで実行される)
        /// </summary>
        /// <param name="asyncResult"></param>
        private void OnReciveCallback(IAsyncResult asyncResult)
        {
            // オブジェクト変換
            TelnetClientReciveStream _TelnetClientReciveStream = (TelnetClientReciveStream)asyncResult.AsyncState;

            // ソケットが切断されているか？
            if (_TelnetClientReciveStream.Socket == null || !_TelnetClientReciveStream.Socket.Connected)
            {
                return;
            }

            // 非同期読込が終了しているか？
            int bytesRead = _TelnetClientReciveStream.Socket.EndReceive(asyncResult);

            // 受信データサイズ判定
            if (bytesRead > 0)
            {
                // ロギング
                this.Logger.InfoFormat("受信データ - {0:#,0} byte：\n{1}", bytesRead, Debug.Dump(1, _TelnetClientReciveStream.Buffer, bytesRead));

                // 残りがある場合にはデータ保持する
                if (_TelnetClientReciveStream.Stream != null)
                {
                    _TelnetClientReciveStream.Stream.Write(_TelnetClientReciveStream.Buffer, 0, bytesRead);
                }

                // バッファ全て受信したら残りがある場合があるので、再受信実施
                if (bytesRead >= _TelnetClientReciveStream.Socket.ReceiveBufferSize)
                {
                    // 再度受信待ちを実施
                    _TelnetClientReciveStream.Socket.BeginReceive(_TelnetClientReciveStream.Buffer, 0, _TelnetClientReciveStream.Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReciveCallback), _TelnetClientReciveStream);
                }
                else
                {
                    // 受信通知設定
                    this.OnReciveNotify.Set();

                    // イベント呼出
                    if (this.OnRecive != null)
                    {
                        // イベントパラメータ生成
                        TelnetClientReciveEventArgs _TelnetClientReciveEventArgs = new TelnetClientReciveEventArgs()
                        {
                            Socket = _TelnetClientReciveStream.Socket,
                            Size = bytesRead,
                            Stream = _TelnetClientReciveStream.Stream,
                        };

                        // イベント呼出し
                        this.OnRecive(this, _TelnetClientReciveEventArgs);
                    }
                }
            }
            else
            {
                // 受信通知設定
                this.OnReciveNotify.Set();

                // イベント呼出
                if (this.OnRecive != null)
                {
                    // イベントパラメータ生成
                    TelnetClientReciveEventArgs _TelnetClientReciveEventArgs = new TelnetClientReciveEventArgs()
                    {
                        Socket = _TelnetClientReciveStream.Socket,
                        Size = 0,
                        Stream = null,
                    };

                    // イベント呼出し
                    this.OnRecive(this, _TelnetClientReciveEventArgs);
                }
            }
        }
        #endregion
    }
}
