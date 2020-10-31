using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// TELNETクライアントクラス
    /// </summary>
    public class TelnetClient : TcpClientAbstract, TcpClientInterface, IDisposable
    {
        #region ソケット
        /// <summary>
        /// ソケット
        /// </summary>
        protected Socket m_Socket = null;
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

        #region 破棄済みフラグ
        /// <summary>
        /// 破棄済みフラグ
        /// </summary>
        private bool m_Disposed = false;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="userPasword"></param>
        public TelnetClient(string host, int port, string userName, string userPasword)
        {
            // 初期化
            this.Initialization(host, port, userName, userPasword);
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~TelnetClient()
        {
            // 破棄
            this.Dispose();
        }
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="userPasword"></param>
        protected override void Initialization(string host, int port, string userName, string userPasword)
        {
            // 基底クラス初期化
            base.Initialization(host, port, userName, userPasword);

            // ソケット生成
            this.m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.m_Socket.ReceiveBufferSize = this.m_ReciveBufferCapacity;
            this.m_Socket.SendBufferSize = this.m_SendBufferCapacity;
            this.m_Socket.ReceiveTimeout = this.m_Timeout.Recv;
            this.m_Socket.SendTimeout = this.m_Timeout.Send;
            this.m_Socket.Blocking = true;
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
            // 接続
            this.m_Socket.Connect(this.m_IPEndPoint.Address, this.m_Port);

            // ロギング
            this.Logger.InfoFormat("接続：[{0} ⇒ {1}]", this.m_Socket.LocalEndPoint.ToString(), this.m_Socket.RemoteEndPoint.ToString());
        }

        /// <summary>
        /// 接続
        /// </summary>
        /// <param name="timeout"></param>
        public void Connect(int timeout)
        {
            // Taskオブジェクト生成
            using (this.CancellationTokenSource.Connect = new CancellationTokenSource())
            {
                // タイムアウト設定
                this.CancellationTokenSource.Connect.CancelAfter(timeout);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    // 接続
                    this.Connect();
                }, this.CancellationTokenSource.Connect.Token);

                try
                {
                    // タスク待ち
                    task.Wait(this.CancellationTokenSource.Connect.Token);
                }
                catch (OperationCanceledException ex)
                {
                    // 例外
                    throw new TelnetException("接続(Telnet)に失敗しました(OperationCanceledException)", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new TelnetException("接続(Telnet)に失敗しました(AggregateException)", ex);
                }
                finally
                {
                    // Taskオブジェクト破棄
                    this.CancellationTokenSource.Connect = null;
                }
            }
        }
        #endregion

        #region 切断
        /// <summary>
        /// 切断
        /// </summary>
        public void Disconnect()
        {
            // 切断
            this.m_Socket.Disconnect(true);

            // ロギング
            this.Logger.InfoFormat("切断：[{0}]", this.m_Socket.RemoteEndPoint.ToString());
        }

        /// <summary>
        /// 切断
        /// </summary>
        /// <param name="timeout"></param>
        public void Disconnect(int timeout)
        {
            // Taskオブジェクト生成
            using (this.CancellationTokenSource.Disconnect = new CancellationTokenSource())
            {
                // タイムアウト設定
                this.CancellationTokenSource.Disconnect.CancelAfter(timeout);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    // 切断
                    this.Disconnect();
                }, this.CancellationTokenSource.Disconnect.Token);

                try
                {
                    // タスク待ち
                    task.Wait(this.CancellationTokenSource.Disconnect.Token);
                }
                catch (OperationCanceledException ex)
                {
                    // 例外
                    throw new TelnetException("切断(Telnet)に失敗しました(OperationCanceledException)", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new TelnetException("切断(Telnet)切断に失敗しました(AggregateException)", ex);
                }
                finally
                {
                    // Taskオブジェクト破棄
                    this.CancellationTokenSource.Disconnect = null;
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
            // 送信
            this.m_Socket.Send(stream.ToArray(), (int)stream.Length, SocketFlags.None);

            // ロギング
            this.Logger.InfoFormat("送信 - {0:#,0} byte：\n{1}", stream.Length, Common.Diagnostics.Debug.Dump(1, stream));
        }

        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        public void Send(byte data, int timeout)
        {
            // Taskオブジェクト生成
            using (this.CancellationTokenSource.Send = new CancellationTokenSource())
            {
                // タイムアウト設定
                this.CancellationTokenSource.Send.CancelAfter(timeout);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    // 送信
                    this.Send(data);
                }, this.CancellationTokenSource.Send.Token);

                try
                {
                    // タスク待ち
                    task.Wait(this.CancellationTokenSource.Send.Token);
                }
                catch (OperationCanceledException ex)
                {
                    // 例外
                    throw new TelnetClientException("送信(Telnet)に失敗しました(OperationCanceledException)", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new TelnetClientException("送信(Telnet)に失敗しました(AggregateException)", ex);
                }
                finally
                {
                    // Taskオブジェクト破棄
                    this.CancellationTokenSource.Send = null;
                }
            }
        }

        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        public void Send(byte[] data, int timeout)
        {
            // Taskオブジェクト生成
            using (this.CancellationTokenSource.Send = new CancellationTokenSource())
            {
                // タイムアウト設定
                this.CancellationTokenSource.Send.CancelAfter(timeout);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    // 送信
                    this.Send(data);
                }, this.CancellationTokenSource.Send.Token);

                try
                {
                    // タスク待ち
                    task.Wait(this.CancellationTokenSource.Send.Token);
                }
                catch (OperationCanceledException ex)
                {
                    // 例外
                    throw new TelnetClientException("送信(Telnet)に失敗しました(OperationCanceledException)", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new TelnetClientException("送信(Telnet)に失敗しました(AggregateException)", ex);
                }
                finally
                {
                    // Taskオブジェクト破棄
                    this.CancellationTokenSource.Send = null;
                }
            }
        }

        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="timeout"></param>
        public void Send(MemoryStream stream, int timeout)
        {
            // Taskオブジェクト生成
            using (this.CancellationTokenSource.Send = new CancellationTokenSource())
            {
                // タイムアウト設定
                this.CancellationTokenSource.Send.CancelAfter(timeout);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    // 送信
                    this.Send(stream);
                }, this.CancellationTokenSource.Send.Token);

                try
                {
                    // タスク待ち
                    task.Wait(this.CancellationTokenSource.Send.Token);
                }
                catch (OperationCanceledException ex)
                {
                    // 例外
                    throw new TelnetClientException("送信(Telnet)に失敗しました(OperationCanceledException)", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new TelnetClientException("送信(Telnet)に失敗しました(AggregateException)", ex);
                }
                finally
                {
                    // Taskオブジェクト破棄
                    this.CancellationTokenSource.Send = null;
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
            // 受信バッファイサイズ生成
            byte[] reciveBuffer = new byte[size];

            // 受信
            int reciveSize = this.m_Socket.Receive(reciveBuffer, size, SocketFlags.None);

            // 受信バッファ
            MemoryStream readStream = new MemoryStream(reciveSize);

            // 受信バッファに保存
            readStream.Write(reciveBuffer, 0, reciveSize);

            // ロギング
            this.Logger.InfoFormat("受信データ - {0:#,0} byte：\n{1}", readStream.Length, Common.Diagnostics.Debug.Dump(1, readStream));

            // MemoryStreamを返却
            return readStream;
        }

        /// <summary>
        /// 受信
        /// </summary>
        /// <param name="size"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public MemoryStream Recive(int size, int timeout)
        {
            // 受信バッファ
            MemoryStream readStream = new MemoryStream(size);

            // Taskオブジェクト生成
            using (this.CancellationTokenSource.Recv = new CancellationTokenSource())
            {
                // タイムアウト設定
                this.CancellationTokenSource.Recv.CancelAfter(timeout);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    // 受信
                    readStream = this.Recive(size);
                }, this.CancellationTokenSource.Recv.Token);

                try
                {
                    // タスク待ち
                    task.Wait(this.CancellationTokenSource.Recv.Token);
                }
                catch (OperationCanceledException ex)
                {
                    // 例外
                    throw new TelnetException("受信(Telnet)に失敗しました(OperationCanceledException)", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new TelnetException("受信(Telnet)切断に失敗しました(AggregateException)", ex);
                }
                finally
                {
                    // Taskオブジェクト破棄
                    this.CancellationTokenSource.Recv = null;
                }
            }

            // MemoryStreamを返却
            return readStream;
        }
        #endregion

        #region 非同期送信
        /// <summary>
        /// 非同期送信
        /// </summary>
        /// <param name="data"></param>
        public void SendAsync(byte data)
        {
            // 送信データ生成
            byte[] senddata = new byte[1] { data };

            // 非同期送信
            this.SendAsync(new MemoryStream(senddata));
        }

        /// <summary>
        /// 非同期送信
        /// </summary>
        /// <param name="data"></param>
        public void SendAsync(byte[] data)
        {
            // 非同期送信
            this.SendAsync(new MemoryStream(data));
        }

        /// <summary>
        /// 非同期送信
        /// </summary>
        /// <param name="stream"></param>
        public void SendAsync(MemoryStream stream)
        {
            // 送信用Stream生成
            TelnetClientSendStream sendStream = new TelnetClientSendStream();
            sendStream.Socket = this.m_Socket;
            sendStream.Stream = stream;

            // 非同期送信
            IAsyncResult _result = this.m_Socket.BeginSend(stream.ToArray(), 0, stream.ToArray().Length, SocketFlags.None, new AsyncCallback(this.SendCallBack), sendStream);
        }

        /// <summary>
        /// 非同期送信コールバック
        /// </summary>
        /// <param name="ar"></param>
        private void SendCallBack(IAsyncResult ar)
        {
            // オブジェクト変換
            TelnetClientSendStream stream = (TelnetClientSendStream)ar.AsyncState;

            // 送信完了
            int bytesSent = stream.Socket.EndSend(ar);

            // ロギング
            this.Logger.InfoFormat("送信データ - {0:#,0} byte：\n{1}", stream.Stream.Length, Common.Diagnostics.Debug.Dump(1, stream.Stream));

            // 送信通知
            this.OnSendNotify.Set();

            // イベント呼出
            if (this.OnSend != null)
            {
                // イベントパラメータ生成
                TelnetClientSendEventArgs eventArgs = new TelnetClientSendEventArgs()
                {
                    Socket = stream.Socket,
                    Size = bytesSent,
                    Stream = stream.Stream,
                };

                // イベント呼出し
                this.OnSend(this, eventArgs);
            }

            // 送信通知リセット
            this.OnSendNotify.Reset();
        }
        #endregion

        #region 非同期受信
        /// <summary>
        /// 非同期受信
        /// </summary>
        /// <returns></returns>
        public void ReciveAsync()
        {
            // 非同期受信
            this.ReciveAsync(this.m_ReciveBufferCapacity);
        }

        /// <summary>
        /// 非同期受信
        /// </summary>
        /// <param name="size"></param>
        public void ReciveAsync(int size)
        {
            // 受信用Stream生成
            TelnetClientReciveStream stream = new TelnetClientReciveStream();
            stream.Buffer = new byte[size];
            stream.Socket = this.m_Socket;
            stream.Stream = new MemoryStream(size);

            // 非同期受信
            IAsyncResult result = this.m_Socket.BeginReceive(stream.Buffer, 0, stream.Buffer.Length, SocketFlags.None, this.ReciveAsyncCallback, stream);
        }

        /// <summary>
        /// 非同期受信コールバック
        /// </summary>
        /// <param name="ar"></param>
        private void ReciveAsyncCallback(IAsyncResult ar)
        {
            // オブジェクト変換
            TelnetClientReciveStream stream = (TelnetClientReciveStream)ar.AsyncState;

            // 非同期読込が終了
            int bytesRead = stream.Socket.EndReceive(ar);

            // ロギング
            this.Logger.InfoFormat("受信(非同期)データ - {0:#,0} byte：\n{1}", bytesRead, Common.Diagnostics.Debug.Dump(1, stream.Buffer, bytesRead));

            // 受信通知
            this.OnReciveNotify.Set();

            // イベント呼出
            if (this.OnRecive != null)
            {
                // イベントパラメータ生成
                TelnetClientReciveEventArgs eventArgs = new TelnetClientReciveEventArgs()
                {
                    Socket = stream.Socket,
                    Size = bytesRead,
                    Stream = stream.Stream,
                };

                // イベント呼出し
                this.OnRecive(this, eventArgs);
            }

            // 受信通知リセット
            this.OnReciveNotify.Reset();
        }
        #endregion
    }
}
