using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

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
        private string m_HostName = string.Empty;

        /// <summary>
        /// IPアドレス
        /// </summary>
        private string m_HostIpAddress = string.Empty;

        /// <summary>
        /// サーバエンドポイント
        /// </summary>
        private IPEndPoint m_IPEndPoint = null;

        /// <summary>
        /// ソケット
        /// </summary>
        private Socket m_Socket = null;

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

        #region タイムアウト
        /// <summary>
        /// 接続タイムアウト
        /// </summary>
        private int m_ConnectillisecondsTimeout = 10000;

        /// <summary>
        /// 受信タイムアウト
        /// </summary>
        private int m_ReciveMillisecondsTimeout = 10000;

        /// <summary>
        /// 送信タイムアウト
        /// </summary>
        private int m_SendMillisecondsTimeout = 10000;

        /// <summary>
        /// 切断タイムアウト
        /// </summary>
        private int m_DisconnectMillisecondsTimeout = 10000;
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

        /// <summary>
        /// 破棄フラグ
        /// </summary>
        private bool m_Disposed = false;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="hostName"></param>
        public TelnetClient(string hostName)
            : this(hostName, 23)
        {
            Trace.WriteLine("TelnetClient::TelnetClient(string)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="port"></param>
        public TelnetClient(string hostName, int port)
        {
            Trace.WriteLine("TelnetClient::TelnetClient(string, int)");

            // ホスト名設定
            this.m_HostName = hostName;

            try
            {
                // ホスト名がIPアドレスとみなしてIPアドレスの解析
                IPAddress hostAddress = IPAddress.Parse(hostName);

                // ホストIPアドレス設定
                this.m_HostIpAddress = hostName;

                // エンドポイント設定
                this.m_IPEndPoint = new IPEndPoint(hostAddress, port);
            }
            catch (FormatException ex)
            {
                Debug.WriteLine(ex.Message + "：[" + hostName + "]");

                // DNSに問合せ(IPv4)
                this.m_HostIpAddress = this.GetIPAddress(hostName, AddressFamily.InterNetwork);

                // エンドポイント設定
                this.m_IPEndPoint = new IPEndPoint(IPAddress.Parse(this.m_HostIpAddress), 21);
            }
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~TelnetClient()
        {
            Trace.WriteLine("TelnetClient::~TelnetClient()");

            // 破棄
            this.Dispose(false);
        }
        #endregion

        #region IPアドレス取得
        /// <summary>
        /// IPアドレス取得
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name=""></param>
        /// <returns></returns>
        private string GetIPAddress(string hostName, AddressFamily addressFamily)
        {
            Trace.WriteLine("TelnetClient::GetIPAddress(string, AddressFamily)");

            IPHostEntry ipentry = Dns.GetHostEntry(hostName);

            foreach (IPAddress ip in ipentry.AddressList)
            {
                if (ip.AddressFamily == addressFamily)
                {
                    return ip.ToString();
                }
            }
            return string.Empty;
        }
        #endregion

        #region 破棄
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            Trace.WriteLine("TelnetClient::Dispose()");

            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 破棄
        /// </summary>
        /// <param name="isDisposing"></param>
        protected virtual void Dispose(bool isDisposing)
        {
            Trace.WriteLine("TelnetClient::Dispose(bool)");

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
            Trace.WriteLine("TelnetClient::Connect()");

            // ソケット生成
            this.m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.m_Socket.ReceiveBufferSize = this.m_ReciveBufferCapacity;
            this.m_Socket.ReceiveTimeout = this.m_ReciveMillisecondsTimeout;
            this.m_Socket.SendTimeout = this.m_SendMillisecondsTimeout;
            this.m_Socket.Blocking = false;

            // 非同期で接続を待機
            this.m_Socket.BeginConnect(this.m_IPEndPoint, new AsyncCallback(this.OnConnectCallBack), this.m_Socket);

            // 接続待ち
            if (!this.OnConnectNotify.WaitOne(this.m_ConnectillisecondsTimeout))
            {
                // 接続完了通知リセット
                this.OnConnectNotify.Reset();

                // 例外
                throw new TelnetClientException("接続タイムアウト");
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
            Trace.WriteLine("TelnetClient::OnConnectCallBack(IAsyncResult)");

            try
            {
                // ソケット取得
                Socket _Socket = (Socket)(asyncResult.AsyncState);

                // ソケットが切断されているか？
                if (_Socket == null || !_Socket.Connected)
                {
                    Debug.WriteLine("既にソケットが切断されています");
                    return;
                }

                // 接続完了通知を設定
                this.OnConnectNotify.Set();

                // イベント呼出
                if (this.OnConnected != null)
                {
                    // パラメータ設定
                    TelnetClientConnectedEventArgs _TelnetClientConnectedEventArgs = new TelnetClientConnectedEventArgs()
                    {
                        LocalEndPoint = _Socket.LocalEndPoint,
                        RemoteEndPoint = _Socket.RemoteEndPoint,
                    };
                    this.OnConnected(this, _TelnetClientConnectedEventArgs);
                }
            }
            catch (Exception ex)
            {
                // 接続完了通知リセット
                Debug.WriteLine(ex.Message);
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
            Trace.WriteLine("TelnetClient::DisConnect()");

            // 切断判定
            if (this.m_Socket != null && this.m_Socket.Connected)
            {
                // 切断開始
                this.m_Socket.BeginDisconnect(false, new AsyncCallback(this.OnDisconnectCallBack), this.m_Socket);

                // 切断待ち
                if (!this.OnDisconnectNotify.WaitOne(this.m_DisconnectMillisecondsTimeout))
                {
                    // 切断完了通知リセット
                    this.OnDisconnectNotify.Reset();

                    // 例外
                    throw new TelnetClientException("切断タイムアウト");
                }

                // 切断完了通知リセット
                this.OnDisconnectNotify.Reset();
            }
        }

        /// <summary>
        /// 非同期切断のコールバックメソッド(別スレッドで実行される)
        /// </summary>
        /// <param name="asyncResult"></param>
        private void OnDisconnectCallBack(IAsyncResult asyncResult)
        {
            Trace.WriteLine("TelnetClient::OnDisconnectCallBack(IAsyncResult)");

            // ソケット取得
            Socket _Socket = (Socket)(asyncResult.AsyncState);

            // 切断・破棄
            if (_Socket != null)
            {
                _Socket.Disconnect(false);
                _Socket.Dispose();
            }

            // 切断通知
            this.OnDisconnectNotify.Set();

            // イベント呼出
            if (this.OnDisconnected != null)
            {
                TelnetClientDisconnectedEventArgs _TelnetClientDisconnectedEventArgs = new TelnetClientDisconnectedEventArgs()
                {
                    RemoteEndPoint = this.m_IPEndPoint,
                };
                this.OnDisconnected(this, _TelnetClientDisconnectedEventArgs);
            }
        }
        #endregion

        #region 送信
        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="data"></param>
        public void Send(byte[] data)
        {
            Trace.WriteLine("TelnetClient::Send(MemoryStream)");
            this.Send(new MemoryStream(data));
        }

        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="stream"></param>
        public void Send(MemoryStream stream)
        {
            Trace.WriteLine("TelnetClient::Send(MemoryStream)");

            TelnetClientSendStream _TelnetClientSendStream = new TelnetClientSendStream();
            _TelnetClientSendStream.Stream = stream;
            _TelnetClientSendStream.Socket = this.m_Socket;

            // 送信開始
            this.m_Socket.BeginSend(stream.ToArray(), 0, stream.ToArray().Length, SocketFlags.None, new AsyncCallback(this.OnSendCallBack), _TelnetClientSendStream);

            // 送信応答待ち
            if (!this.OnSendNotify.WaitOne(this.m_SendMillisecondsTimeout))
            {
                // 通知リセット
                this.OnSendNotify.Reset();

                // 例外
                throw new FtpClientException("メッセージ送信タイムアウト");
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
            Trace.WriteLine("TelnetClient::OnSendCallBack(IAsyncResult)");

            // オブジェクト変換
            TelnetClientSendStream _TelnetClientSendStream = (TelnetClientSendStream)asyncResult.AsyncState;

            // ソケットが切断されているか？
            if (_TelnetClientSendStream.Socket == null || !_TelnetClientSendStream.Socket.Connected)
            {
                Debug.WriteLine("既にソケットが切断されています");
                return;
            }

            // 送信完了
            int bytesSent = _TelnetClientSendStream.Socket.EndSend(asyncResult);
            Debug.WriteLine("送信完了サイズ：[{0}]", bytesSent);

            // 送信完了通知
            this.OnSendNotify.Set();
        }
        #endregion

        #region 受信
        /// <summary>
        /// 受信
        /// </summary>
        /// <returns></returns>
        public MemoryStream Recive()
        {
            Trace.WriteLine("TelnetClient::Recive()");

            // 結果受信待ち
            TelnetClientReciveStream _TelnetClientReciveStream = new TelnetClientReciveStream();
            _TelnetClientReciveStream.Socket = this.m_Socket;
            _TelnetClientReciveStream.Buffer = new byte[this.m_ReciveBufferCapacity];
            _TelnetClientReciveStream.Stream = new MemoryStream(this.m_ReciveBufferCapacity);
            this.m_Socket.BeginReceive(_TelnetClientReciveStream.Buffer, 0, _TelnetClientReciveStream.Buffer.Length, SocketFlags.None, this.OnReciveCallback, _TelnetClientReciveStream);

            // データ転送待ち
            if (!this.OnReciveNotify.WaitOne(this.m_ReciveMillisecondsTimeout))
            {
                // 受信通知リセット
                this.OnReciveNotify.Reset();

                // MemoryStreamを返却
                return _TelnetClientReciveStream.Stream;
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
            Trace.WriteLine("TelnetClient::OnReciveCallback(IAsyncResult)");

            // オブジェクト変換
            TelnetClientReciveStream _TelnetClientReciveStream = (TelnetClientReciveStream)asyncResult.AsyncState;

            // ソケットが切断されているか？
            if (_TelnetClientReciveStream.Socket == null || !_TelnetClientReciveStream.Socket.Connected)
            {
                Debug.WriteLine("既にソケットが切断されています");
                return;
            }

            // 非同期読込が終了しているか？
            int bytesRead = _TelnetClientReciveStream.Socket.EndReceive(asyncResult);
            Debug.WriteLine("　読込データサイズ:[{0}]", bytesRead);
            //Debug.WriteLine("　受信データサイズ:[{0}]", _TelnetClientReciveStream.Socket.ReceiveBufferSize);
            if (bytesRead > 0)
            {
                // 残りがある場合にはデータ保持する
                if (_TelnetClientReciveStream.Stream != null)
                {
                    _TelnetClientReciveStream.Stream.Write(_TelnetClientReciveStream.Buffer, 0, bytesRead);
                }

                // バッファ全て受信したら残りがある場合があるので、再受信実施
                if (bytesRead >= _TelnetClientReciveStream.Socket.ReceiveBufferSize)
                {
                    // 再度受信待ちを実施
                    Debug.WriteLine("再受信開始");
                    _TelnetClientReciveStream.Socket.BeginReceive(_TelnetClientReciveStream.Buffer, 0, _TelnetClientReciveStream.Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReciveCallback), _TelnetClientReciveStream);
                }
                else
                {
                    // 受信通知設定
                    this.OnReciveNotify.Set();
                }
            }

            else
            {
                // 受信通知設定
                this.OnReciveNotify.Set();
            }
        }
        #endregion
    }
}
