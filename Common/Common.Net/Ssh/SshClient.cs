using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// SSHクライアントクラス
    /// </summary>
    public class SshClient : TcpClientAbstract, TcpClientInterface, IDisposable
    {
        #region 接続情報
        /// <summary>
        /// 接続情報
        /// </summary>
        protected Renci.SshNet.ConnectionInfo m_ConnectionInfo = null;
        #endregion

        #region 認証情報リスト
        /// <summary>
        /// 認証情報リスト
        /// </summary>
        protected List<Renci.SshNet.AuthenticationMethod> m_AuthenticationMethodList = new List<Renci.SshNet.AuthenticationMethod>();
        #endregion

        #region SSHクライアント
        /// <summary>
        /// SSHクライアント
        /// </summary>
        protected Renci.SshNet.SshClient m_Client = null;
        #endregion

        #region ShellStreamオブジェクト
        /// <summary>
        /// ShellStreamオブジェクト
        /// </summary>
        protected Renci.SshNet.ShellStream m_ShellStream = null;
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
        public SshClient(string host, int port, string userName, string userPasword)
        {
            // 初期化
            this.Initialization(host, port, userName, userPasword);
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~SshClient()
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

            // 認証情報生成
            this.m_AuthenticationMethodList.Add(new PasswordAuthenticationMethod(userName, userPasword) { });

            // 接続情報生成
            this.m_ConnectionInfo = new ConnectionInfo(host, port, userName, this.m_AuthenticationMethodList.ToArray());

            // SSHクライアントオブジェクト生成
            this.m_Client = new Renci.SshNet.SshClient(this.m_ConnectionInfo);
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
            this.m_Client.Connect();

            // ロギング
            this.Logger.InfoFormat("接続：[{0}:{1}]", this.m_Client.ConnectionInfo.Host, this.m_Client.ConnectionInfo.Port.ToString());
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
                    throw new SshException("接続(SSH)に失敗しました(OperationCanceledException)", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new SshException("接続(SSH)に失敗しました(AggregateException)", ex);
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
            // 接続中か？
            if (!this.m_Client.IsConnected)
            {
                // 接続中ではないのでなにもしない
                return;
            }

            // 切断
            this.m_Client.Disconnect();

            // ロギング
            this.Logger.InfoFormat("切断：[{0}:{1}]", this.m_Client.ConnectionInfo.Host, this.m_Client.ConnectionInfo.Port.ToString());
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
                    throw new SshException("切断(SSH)に失敗しました(OperationCanceledException)", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new SshException("切断(SSH)に失敗しました(AggregateException)", ex);
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
            // 接続中か？
            if(!this.m_Client.IsConnected)
            {
                // 例外
                throw new SshClientException("接続状態(SSH)ではありません");
            }

            // 送信(書込み)
            this.m_ShellStream.Write(stream.ToArray(), 0, (int)stream.Length);

            // ロギング
            this.Logger.InfoFormat("送信 - {0:#,0} byte：\n{1}", stream.Length, Common.Diagnostics.Debug.Dump(1, stream));

            // ShellStreamフラッシュ
            this.m_ShellStream.Flush();
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
                    throw new SshException("送信(SSH)に失敗しました(OperationCanceledException)", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new SshException("送信(SSH)に失敗しました(AggregateException)", ex);
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
                    throw new SshException("送信(SSH)に失敗しました(OperationCanceledException)", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new SshException("送信(SSH)に失敗しました(AggregateException)", ex);
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
                    throw new SshException("送信(SSH)に失敗しました(OperationCanceledException)", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new SshException("送信(SSH)に失敗しました(AggregateException)", ex);
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
            // 接続中か？
            if (!this.m_Client.IsConnected)
            {
                // 例外
                throw new SshClientException("接続状態(SSH)ではありません");
            }

            // 受信が可能になるまで待合せ
            while (this.m_ShellStream.Length == 0)
            {
                // TODO:100ミリ秒Sleep
                Thread.Sleep(100);
            }

            // 受信バッファイサイズ生成
            byte[] reciveBuffer = new byte[size];

            // 受信
            int reciveSize = this.m_ShellStream.Read(reciveBuffer, 0, size);

            // 受信バッファ
            MemoryStream readStream = new MemoryStream(reciveSize);

            // 受信バッファに保存
            readStream.Write(reciveBuffer, 0, reciveSize);

            // ロギング
            this.Logger.InfoFormat("受信データ - {0:#,0} byte：\n{1}", readStream.Length, Common.Diagnostics.Debug.Dump(1, readStream));

            // ShellStreamフラッシュ
            this.m_ShellStream.Flush();

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
            // 接続中か？
            if (!this.m_Client.IsConnected)
            {
                // 例外
                throw new SshClientException("接続状態(SSH)ではありません");
            }

            // 送信用Stream生成
            SshClientSendStream sendStream = new SshClientSendStream();
            sendStream.ConnectionInfo = this.m_ConnectionInfo;
            sendStream.IPEndPoint = this.m_IPEndPoint;
            sendStream.ShellStream = this.m_ShellStream;
            sendStream.Size = (int)stream.Length;
            sendStream.Stream = stream;
            sendStream.SshClient = this.m_Client;

            // 非同期送信
            IAsyncResult _result = this.m_ShellStream.BeginWrite(stream.ToArray(), 0, stream.ToArray().Length, new AsyncCallback(this.SendCallBack), sendStream);
        }

        /// <summary>
        /// 非同期送信コールバック
        /// </summary>
        /// <param name="ar"></param>
        private void SendCallBack(IAsyncResult ar)
        {
            // オブジェクト変換
            SshClientSendStream stream = (SshClientSendStream)ar.AsyncState;

            // 送信完了
            stream.ShellStream.EndWrite(ar);

            // ロギング
            this.Logger.InfoFormat("送信データ - {0:#,0} byte：\n{1}", stream.Stream.Length, Common.Diagnostics.Debug.Dump(1, stream.Stream));

            // 送信通知
            this.OnSendNotify.Set();

            // イベント呼出
            if (this.OnSend != null)
            {
                // イベントパラメータ生成
                SshClientSendEventArgs eventArgs = new SshClientSendEventArgs()
                {
                    ConnectionInfo = stream.ConnectionInfo,
                    Size = (int)stream.Stream.Length,
                    Stream = stream.Stream,
                };

                // イベント呼出し
                this.OnSend(this, eventArgs);
            }

            // ShellStreamフラッシュ
            stream.ShellStream.Flush();

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
            // 接続中か？
            if (!this.m_Client.IsConnected)
            {
                // 例外
                throw new SshClientException("接続状態(SSH)ではありません");
            }

            // 受信用Stream生成
            SshClientReciveStream stream = new SshClientReciveStream();
            stream.Buffer = new byte[size];
            stream.ConnectionInfo = this.m_ConnectionInfo;
            stream.IPEndPoint = this.m_IPEndPoint;
            stream.ShellStream = this.m_ShellStream;
            stream.SshClient = this.m_Client;
            stream.Stream = new MemoryStream(size);

            // 非同期受信
            IAsyncResult result = this.m_ShellStream.BeginRead(stream.Buffer, 0, stream.Buffer.Length, this.ReciveAsyncCallback, stream);
        }

        /// <summary>
        /// 非同期受信コールバック
        /// </summary>
        /// <param name="ar"></param>
        private void ReciveAsyncCallback(IAsyncResult ar)
        {
            // オブジェクト変換
            SshClientReciveStream stream = (SshClientReciveStream)ar.AsyncState;

            // 非同期読込が終了
            int bytesRead = stream.ShellStream.EndRead(ar);
            System.Console.WriteLine("受信(非同期)データサイズ:{0}", bytesRead);

            // ロギング
            this.Logger.InfoFormat("受信(非同期)データ - {0:#,0} byte：\n{1}", bytesRead, Common.Diagnostics.Debug.Dump(1, stream.Buffer, bytesRead));

            // 受信通知
            this.OnReciveNotify.Set();

            // イベント呼出
            if (this.OnRecive != null)
            {
                // イベントパラメータ生成
                SshClientReciveEventArgs eventArgs = new SshClientReciveEventArgs()
                {
                    ConnectionInfo = stream.ConnectionInfo,
                    Size = bytesRead,
                    Stream = stream.Stream,
                };

                // イベント呼出し
                this.OnRecive(this, eventArgs);
            }

            // ShellStreamフラッシュ
            stream.ShellStream.Flush();

            // 受信通知リセット
            this.OnReciveNotify.Reset();
        }
        #endregion
    }
}
