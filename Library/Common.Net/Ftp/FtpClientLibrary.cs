using Renci.SshNet.Common;
using Renci.SshNet.Messages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace Common.Net
{
    /// <summary>
    /// FtpClientLibraryクラス
    /// </summary>
    public partial class FtpClientLibrary : TcpClientLibrary, InterfaceTcpClientLibrary
    {
        #region ログイン状態
        /// <summary>
        /// ログイン状態
        /// </summary>
        public bool IsLogin { get; protected set; }
        #endregion

        #region 制御用ソケット
        /// <summary>
        /// 制御用ソケット
        /// </summary>
        private Socket m_Socket = null;
        #endregion

        #region タイムアウト
        /// <summary>
        /// Accept
        /// </summary>
        public TimeSpan AcceptTimeout { get; set; } = new TimeSpan(0, 0, 0, 10, 0);

        /// <summary>
        /// DownLoad
        /// </summary>
        public TimeSpan DownLoadTimeout { get; set; } = new TimeSpan(0, 0, 0, 10, 0);
        #endregion

        #region ユーザ情報
        /// <summary>
        /// ユーザ情報
        /// </summary>
        public UserInfo UserInfo { get; set; } = new UserInfo();
        #endregion

        #region ユーザ情報
        /// <summary>
        /// ユーザ情報
        /// </summary>
        public ServerInfo ServerInfo { get; set; } = new ServerInfo();
        #endregion

        #region バッファサイズ
        /// <summary>
        /// 送信バッファサイズ
        /// </summary>
        public int SendBufferCapacity { get; set; } = 4096;

        /// <summary>
        /// 受信バッファサイズ
        /// </summary>
        public int ReciveBufferCapacity { get; set; } = 4096;
        #endregion

        /// <summary>
        /// FtpClientDataConnectionオブジェクト
        /// </summary>
        private FtpClientDataConnection m_FtpClientDataConnection = null;

        #region 通知イベント
        /// <summary>
        /// 接続完了通知イベント
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
        /// コマンド応答完了通知イベント
        /// </summary>
        private ManualResetEvent OnCommandResponceNotify = new ManualResetEvent(false);

        /// <summary>
        /// 切断完了通知イベント
        /// </summary>
        private ManualResetEvent OnDisconnectNotify = new ManualResetEvent(false);

        /// <summary>
        /// DownLoad完了通知イベント
        /// </summary>
        private ManualResetEvent OnDownLoadNotify = new ManualResetEvent(false);

        /// <summary>
        /// ソケット受入完了通知イベント
        /// </summary>
        private ManualResetEvent OnAcceptNotify = new ManualResetEvent(false);
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        public FtpClientLibrary(string host)
            : base(host, 21)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::FtpClientLibrary(string)");
            Logger.DebugFormat("host:{0}", host);

            // ロギング
            Logger.Debug("<<<<= FtpClientLibrary::FtpClientLibrary(string)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public FtpClientLibrary(string host, int port)
            : base(host, port)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::FtpClientLibrary(string, int)");
            Logger.DebugFormat("host:{0}", host);
            Logger.DebugFormat("port:{0}", port);

            // ロギング
            Logger.Debug("<<<<= FtpClientLibrary::FtpClientLibrary(string, int)");
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
            Logger.Debug("=>>>> FtpClientLibrary::Dispose(bool)");

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
            Logger.Debug("<<<<= FtpClientLibrary::Dispose(bool)");
        }
        #endregion

        #region 接続(同期)
        /// <summary>
        /// 接続(同期)
        /// </summary>
        public override bool Connect()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::Connect()");

            // ソケット生成
            m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_Socket.ReceiveTimeout = (int)Timeout.Recive.TotalMilliseconds;
            m_Socket.SendTimeout = (int)Timeout.Send.TotalMilliseconds;
            m_Socket.Blocking = false;

            // 非同期で接続を待機
            m_Socket.BeginConnect(m_HostInfo.IPEndPoint, new AsyncCallback(OnConnectCallBack), m_Socket);

            // 接続待ち
            if (!OnConnectNotify.WaitOne((int)Timeout.Connect.TotalMilliseconds))
            {
                // 接続完了通知リセット
                OnConnectNotify.Reset();

                // 例外
                throw new FtpClientException("接続タイムアウト");
            }

            // 接続完了通知リセット
            OnConnectNotify.Reset();

            // ロギング
            Logger.Debug("<<<<= FtpClientLibrary::Connect()");

            // 正常終了
            return true;
        }

        /// <summary>
        /// 非同期接続のコールバックメソッド(別スレッドで実行される)
        /// </summary>
        /// <param name="ar"></param>
        private void OnConnectCallBack(IAsyncResult ar)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClient::OnConnectCallBack(IAsyncResult)");
            Logger.DebugFormat("ar:{0}", ar);

            try
            {
                // ソケット取得
                Socket socket = (Socket)(ar.AsyncState);

                // ソケットが切断されているか？
                if (socket == null || !socket.Connected)
                {
                    // ロギング
                    Logger.Warn("既にソケットが切断されています");
                    return;
                }

                // コマンド受信応答待ち
                FtpClientReciveStream reciveStream = CommandResponseWaiting((Socket)ar.AsyncState);

                // 接続結果判定
                if (reciveStream.Response.StatusCode != 220)
                {
                    // ロギング
                    Logger.ErrorFormat("接続に失敗しました：{0}", reciveStream.Response.ToString());

                    // 接続完了通知リセット
                    OnConnectNotify.Reset();
                    return;
                }

                // 接続完了通知を設定
                OnConnectNotify.Set();
            }
            catch (Exception ex)
            {
                // ロギング
                Logger.Error(ex.Message);

                // 接続完了通知リセット
                OnConnectNotify.Reset();
                return;
            }

            // ロギング
            Logger.Debug("<<<<= FtpClient::OnConnectCallBack(IAsyncResult)");
        }
        #endregion

        #region 切断(同期)
        /// <summary>
        /// 切断(同期)
        /// </summary>
        public override bool Disconnect()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClient::Disconnect()");

            // 切断判定
            if (m_Socket != null && m_Socket.Connected)
            {
                // 切断開始
                m_Socket.BeginDisconnect(false, new AsyncCallback(OnDisconnectCallBack), m_Socket);

                // 切断待ち
                if (!OnDisconnectNotify.WaitOne((int)Timeout.Disconnect.TotalMilliseconds))
                {
                    // 切断完了通知リセット
                    OnDisconnectNotify.Reset();

                    // 例外
                    throw new FtpClientException("切断タイムアウト");
                }

                // 切断完了通知リセット
                OnDisconnectNotify.Reset();
            }

            // ロギング
            Logger.Debug("<<<<= FtpClient::Disconnect()");

            // 正常終了
            return true;
        }

        /// <summary>
        /// 非同期切断のコールバックメソッド(別スレッドで実行される)
        /// </summary>
        /// <param name="ar"></param>
        private void OnDisconnectCallBack(IAsyncResult ar)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClient::OnDisconnectCallBack(IAsyncResult)");
            Logger.DebugFormat("ar:{0}", ar);

            // ソケット取得
            Socket socket = (Socket)(ar.AsyncState);

            // 切断・破棄
            socket?.Disconnect(false);
            socket?.Dispose();

            // 切断通知
            OnDisconnectNotify.Set();

            // ロギング
            Logger.Debug("<<<<= FtpClient::OnDisconnectCallBack(IAsyncResult)");
        }
        #endregion

        #region ログイン(同期)
        /// <summary>
        /// ログイン(同期)
        /// </summary>
        /// <returns></returns>
        public bool Login()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::Login()");

            // コマンド送信
            FtpResponse response = null;

            // ユーザ名送信
            response = USER(UserInfo.Name);

            // ユーザ名送信判定
            if (!response.Result)
            {
                // ログイン状態設定
                IsLogin = false;

                // ロギング
                Logger.DebugFormat("response:{0}", response.ToString());
                Logger.Debug("<<<<= FtpClientLibrary::Login()");

                // 異常終了
                return false;
            }

            // パスワード送信
            response = PASS(UserInfo.Password);

            // パスワード送信判定
            if (!response.Result)
            {
                // ログイン状態設定
                IsLogin = false;

                // ロギング
                Logger.DebugFormat("response:{0}", response.ToString());
                Logger.Debug("<<<<= FtpClientLibrary::Login()");

                // 異常終了
                return false;
            }

            // ログイン状態設定
            IsLogin = true;

            // ロギング
            Logger.Debug("<<<<= FtpClientLibrary::Login()");

            // 正常終了
            return true;
        }
        #endregion

        #region ログアウト(同期)
        /// <summary>
        /// ログアウト(同期)
        /// </summary>
        /// <returns></returns>
        public bool Logout()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::Logout()");

            // QUIT送信
            QUIT();

            // 初期化
            IsLogin = false;

            // ロギング
            Logger.Debug("<<<<= FtpClientLibrary::Logout()");

            // 正常終了
            return true;
        }
        #endregion

        #region パッシブモード初期化
        /// <summary>
        /// パッシブモード初期化
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        private void InitPassiveMode(string host, int port)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::InitPassiveMode(string, int)");
            Logger.DebugFormat("host:{0}", host);
            Logger.DebugFormat("port:{0}", port);

            // データコネクションを設定
            Logger.InfoFormat("パッシブモード：[{0}:{1}]", host, port);
            m_FtpClientDataConnection = new FtpClientDataConnection()
            {
                Mode = FtpTransferMode.Passive,
                IpAddress = host,
                Port = port,
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            };

            // ロギング
            Logger.Debug("<<<<= FtpClientLibrary::InitPassiveMode(string, int)");
        }
        #endregion

        #region アクティブモード初期化
        /// <summary>
        /// アクティブモード初期化
        /// </summary>
        private void InitActiveMode()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::InitActiveMode()");

            // ポート番号決定
            string[] hosts = m_Socket.LocalEndPoint.ToString().Split(new char[] { ':' });
            TcpListener listener = new TcpListener(IPAddress.Parse(hosts[0]), 0);
            listener.Start();
            string[] ports = listener.LocalEndpoint.ToString().Split(new char[] { ':' });

            // データコネクションを設定
            Logger.InfoFormat("アクティブモード：[{0}:{1}]", hosts[0], ports[1]);
            m_FtpClientDataConnection = new FtpClientDataConnection()
            {
                Mode = FtpTransferMode.Active,
                IpAddress = hosts[0],
                Port = int.Parse(ports[1]),
                Listener = listener,
                Socket = listener.Server
            };

            // ロギング
            Logger.Debug("<<<<= FtpClientLibrary::InitActiveMode()");
        }
        #endregion

        #region 送信
        #region 送信(同期)
        /// <summary>
        /// 送信(同期)
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public bool Send(string txt)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::Send(string)");
            Logger.DebugFormat("txt:{0}", txt);

            // ソケット判定
            if (m_Socket == null || !m_Socket.Connected)
            {
                // ロギング
                Logger.Warn("既にソケットが切断されています");
                return false;
            }

            // FtpClientSendStreamオブジェクト生成
            FtpClientSendStream sendStream = new FtpClientSendStream();
            sendStream.Buffer = ServerInfo.Encoding.GetBytes(txt);
            sendStream.Socket = m_Socket;

            // 送信開始
            m_Socket.BeginSend(sendStream.Buffer, 0, sendStream.Buffer.Length, SocketFlags.None, new AsyncCallback(OnSendCallBack), sendStream);

            // 送信応答待ち
            if (!OnSendNotify.WaitOne((int)Timeout.Send.TotalMilliseconds))
            {
                // 通知リセット
                OnSendNotify.Reset();

                // 例外
                throw new FtpClientException("メッセージ送信タイムアウト");
            }

            // 通知リセット
            OnSendNotify.Reset();

            // ロギング
            Logger.Debug("<<<<= FtpClientLibrary::Send(string)");

            // 正常終了
            return true;
        }

        /// <summary>
        /// 非同期送信のコールバックメソッド(別スレッドで実行される)
        /// </summary>
        /// <param name="ar"></param>
        private void OnSendCallBack(IAsyncResult ar)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::OnSendCallBack(IAsyncResult)");
            Logger.DebugFormat("ar:{0}", ar);

            // オブジェクト変換
            FtpClientSendStream sendStream = (FtpClientSendStream)ar.AsyncState;

            // ソケットが切断されているか？
            if (sendStream.Socket == null || !sendStream.Socket.Connected)
            {
                // ロギング
                Logger.Warn("既にソケットが切断されています");
                return;
            }

            // 送信完了
            int bytesSent = sendStream.Socket.EndSend(ar);

            // 送信完了通知
            OnSendNotify.Set();

            // ロギング
            Logger.DebugFormat("送信完了サイズ：[{0}]", bytesSent);
            Logger.Debug("<<<<= FtpClientLibrary::OnSendCallBack(IAsyncResult)");
        }
        #endregion
        #endregion

        #region 読込
        #region 読込(同期)
        /// <summary>
        /// 読込(同期)
        /// </summary>
        /// <returns></returns>
        public override string Read()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::Read()");

            // MemoryStream受信
            MemoryStream stream = Recive();

            // リスト変換
            string[] stringArray = MemoryStreamToStringArray(stream, new string[] { "\r\n" });

            // 結果オブジェクト生成
            StringBuilder result = new StringBuilder();

            // 結果に追加
            foreach(string s in stringArray)
            {
                result.Append(s);
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::Recive()");

            // 返却
            return result.ToString();
        }

        /// <summary>
        /// 受信
        /// </summary>
        /// <returns></returns>
        private MemoryStream Recive()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::Recive()");

            // 結果オブジェクト
            MemoryStream result = null;

            // 転送モードによる
            if (m_FtpClientDataConnection.Mode == FtpTransferMode.Active)
            {
                // アクティブモード転送
                result = ActiveRecive();
            }
            else
            {
                // パッシブモード転送
                result = PassiveRecive();
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= FtpClientLibrary::Recive()");

            // 返却
            return result;
        }

        /// <summary>
        /// 受信(アクティブモード転送)
        /// </summary>
        /// <returns></returns>
        private MemoryStream ActiveRecive()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::ActiveRecive()");

            // 結果受信待ち
            FtpClientReciveStream reciveStream = new FtpClientReciveStream();
            reciveStream.Socket = m_FtpClientDataConnection.Socket;
            reciveStream.Buffer = new byte[ReciveBufferCapacity];
            reciveStream.Stream = new MemoryStream(ReciveBufferCapacity);
            reciveStream.FileStream = m_FtpClientDataConnection.FileStream;
            m_FtpClientDataConnection.Socket.BeginReceive(reciveStream.Buffer, 0, reciveStream.Buffer.Length, SocketFlags.None, OnReciveCallback, reciveStream);

            // データ転送待ち
            if (!OnReciveNotify.WaitOne(DownLoadTimeout))
            {
                // 転送通知リセット
                OnReciveNotify.Reset();

                // 例外
                throw new FtpClientException("受信(Active)送信タイムアウト");
            }

            // 転送通知リセット
            OnReciveNotify.Reset();

            // ロギング
            Logger.Debug("<<<<= FtpClientLibrary::ActiveRecive()");

            // MemoryStreamを返却
            return reciveStream.Stream;
        }

        /// <summary>
        /// 受信(パッシブモード転送)
        /// </summary>
        /// <returns></returns>
        private MemoryStream PassiveRecive()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::PassiveRecive()");

            // 結果受信待ち
            FtpClientReciveStream reciveStream = new FtpClientReciveStream();
            reciveStream.Socket = m_FtpClientDataConnection.Socket;
            reciveStream.Buffer = new byte[ReciveBufferCapacity];
            reciveStream.Stream = new MemoryStream(ReciveBufferCapacity);
            reciveStream.FileStream = m_FtpClientDataConnection.FileStream;
            m_FtpClientDataConnection.Socket.BeginReceive(reciveStream.Buffer, 0, reciveStream.Buffer.Length, SocketFlags.None, OnReciveCallback, reciveStream);

            // データ転送待ち
            if (!OnReciveNotify.WaitOne(DownLoadTimeout))
            {
                // 転送通知リセット
                OnReciveNotify.Reset();

                // 例外
                throw new FtpClientException("受信(Passive)送信タイムアウト");
            }

            // 転送通知リセット
            OnReciveNotify.Reset();

            // ロギング
            Logger.Debug("<<<<= FtpClientLibrary::PassiveRecive()");

            // MemoryStreamを返却
            return reciveStream.Stream;
        }
        #endregion

        /// <summary>
        /// 非同期受信のコールバックメソッド(別スレッドで実行される)
        /// </summary>
        /// <param name="ar"></param>
        private void OnReciveCallback(IAsyncResult ar)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClient::OnReciveCallback(IAsyncResult)");
            Logger.DebugFormat("ar:{0}", ar);

            // オブジェクト変換
            FtpClientReciveStream reciveStream = (FtpClientReciveStream)ar.AsyncState;

            // ソケットが切断されているか？
            if (reciveStream.Socket == null || !reciveStream.Socket.Connected)
            {
                // ロギング
                Logger.Warn("既にソケットが切断されています");
                return;
            }

            // 非同期読込が終了しているか？
            int bytesRead = reciveStream.Socket.EndReceive(ar);

            // ロギング
            Logger.InfoFormat("読込データサイズ:[{0}]", bytesRead);

            // 読込サイズ判定
            if (bytesRead > 0)
            {
                // 残りがある場合にはデータ保持する
                if (reciveStream.Stream != null)
                {
                    reciveStream.Stream.Write(reciveStream.Buffer, 0, bytesRead);
                }

                // ファイル書込み
                if (reciveStream.FileStream != null)
                {
                    reciveStream.FileStream.Write(reciveStream.Buffer, 0, bytesRead);
                }

                // 再度受信待ちを実施
                reciveStream.Socket.BeginReceive(reciveStream.Buffer, 0, reciveStream.Buffer.Length, SocketFlags.None, new AsyncCallback(OnReciveCallback), reciveStream);
            }
            else
            {
                // 転送通知設定
                OnReciveNotify.Set();
            }

            // ロギング
            Logger.Debug("<<<<= FtpClient::OnReciveCallback(IAsyncResult)");
        }
        #endregion

        #region アップロード
        /// <summary>
        /// アップロード
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        private bool Upload(MemoryStream memoryStream)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClient::Upload(MemoryStream)");
            Logger.DebugFormat("memoryStream  :{0}", memoryStream.ToString());

            // 結果オブジェクト
            bool result = false;

            // 転送モードによる
            if (m_FtpClientDataConnection.Mode == FtpTransferMode.Active)
            {
                // アクティブモード転送
                result = ActiveUpload(memoryStream);
            }
            else
            {
                // パッシブモード転送
                result = PassiveUpload(memoryStream);
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= FtpClientLibrary::Upload()");

            // 返却
            return result;
        }

        /// <summary>
        /// アクティブモード転送(ローカル⇒サーバ)
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        private bool ActiveUpload(MemoryStream memoryStream)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClient::ActiveUpload(MemoryStream)");
            Logger.DebugFormat("memoryStream  :{0}", memoryStream.ToString());

            // ソケット判定
            if (m_FtpClientDataConnection.Socket == null || !m_FtpClientDataConnection.Socket.Connected)
            {
                // ロギング
                Logger.Warn("既にソケットが切断されています");

                // 異常終了
                return false;
            }

            // FtpClientSendStreamオブジェクト生成
            FtpClientSendStream sendStream = new FtpClientSendStream();
            sendStream.Stream = memoryStream;
            sendStream.Socket = m_FtpClientDataConnection.Socket;

            // 送信開始
            m_FtpClientDataConnection.Socket.BeginSend(memoryStream.ToArray(), 0, memoryStream.ToArray().Length, SocketFlags.None, new AsyncCallback(OnUploadCallback), sendStream);

            // 送信応答待ち
            if (!OnSendNotify.WaitOne(Timeout.Send))
            {
                // 通知リセット
                OnSendNotify.Reset();

                // 例外
                throw new FtpClientException("Upload(Active)タイムアウト");
            }

            // 通知リセット
            OnSendNotify.Reset();

            // ロギング
            Logger.Debug("<<<<= FtpClient::ActiveUpload(FtpClientDataConnection, MemoryStream)");

            // 正常終了
            return true;
        }

        /// <summary>
        /// パッシブモード転送(ローカル⇒サーバ)
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        private bool PassiveUpload(MemoryStream memoryStream)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClient::PassiveUpload(MemoryStream)");
            Logger.DebugFormat("dataConnection:\n{0}", m_FtpClientDataConnection.ToString());
            Logger.DebugFormat("memoryStream  :{0}", memoryStream.ToString());

            // ソケット判定
            if (m_FtpClientDataConnection.Socket == null || !m_FtpClientDataConnection.Socket.Connected)
            {
                // ロギング
                Logger.Warn("既にソケットが切断されています");

                // 異常終了
                return false;
            }

            // FtpClientSendStreamオブジェクト生成
            FtpClientSendStream sendStream = new FtpClientSendStream();
            sendStream.Stream = memoryStream;
            sendStream.Socket = m_FtpClientDataConnection.Socket;

            // 送信開始
            m_FtpClientDataConnection.Socket.BeginSend(memoryStream.ToArray(), 0, memoryStream.ToArray().Length, SocketFlags.None, new AsyncCallback(OnUploadCallback), sendStream);

            // 送信応答待ち
            if (!OnSendNotify.WaitOne(Timeout.Send))
            {
                // 通知リセット
                OnSendNotify.Reset();

                // 例外
                throw new FtpClientException("Upload(Passive)送信タイムアウト");
            }

            // 通知リセット
            OnSendNotify.Reset();

            // ロギング
            Logger.Debug("<<<<= FtpClient::PassiveUpload(MemoryStream)");

            // 正常終了
            return true;
        }

        /// <summary>
        /// 非同期転送のコールバックメソッド(別スレッドで実行される)
        /// </summary>
        /// <param name="asyncResult"></param>
        private void OnUploadCallback(IAsyncResult asyncResult)
        {
            Trace.WriteLine("FtpClient::OnPassiveUploadCallback(IAsyncResult)");

            // オブジェクト変換
            FtpClientSendStream sendStream = (FtpClientSendStream)asyncResult.AsyncState;

            // ソケットが切断されているか？
            if (sendStream.Socket == null || !sendStream.Socket.Connected)
            {
                Debug.WriteLine("既にソケットが切断されています");
                return;
            }

            // 送信完了
            int bytesSent = sendStream.Socket.EndSend(asyncResult);

            // ロギング
            Logger.InfoFormat("送信完了サイズ:[{0}]", bytesSent);

            // 送信完了通知
            OnSendNotify.Set();

            // TODO:イベント呼出し
        }
        #endregion

        #region ダウンロード
        /// <summary>
        /// ダウンロード
        /// </summary>
        /// <returns></returns>
        private MemoryStream Download()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClient::Download()");

            // 結果オブジェクト
            MemoryStream result = null;

            // 転送モードによる
            if (m_FtpClientDataConnection.Mode == FtpTransferMode.Active)
            {
                // アクティブモード転送
                result = ActiveDownload();
            }
            else
            {
                // パッシブモード転送
                result = PassiveDownload();
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= FtpClientLibrary::Download()");

            // 返却
            return result;
        }

        /// <summary>
        /// ダウンロード(アクティブモード転送)
        /// </summary>
        /// <returns></returns>
        private MemoryStream ActiveDownload()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClient::ActiveDownload()");

            // 結果受信待ち
            FtpClientReciveStream reciveStream = new FtpClientReciveStream();
            reciveStream.Socket = m_FtpClientDataConnection.Socket;
            reciveStream.Buffer = new byte[ReciveBufferCapacity];
            reciveStream.Stream = new MemoryStream(ReciveBufferCapacity);
            reciveStream.FileStream = m_FtpClientDataConnection.FileStream;
            m_FtpClientDataConnection.Socket.BeginReceive(reciveStream.Buffer, 0, reciveStream.Buffer.Length, SocketFlags.None, OnDownloadCallback, reciveStream);

            // データ転送待ち
            if (!OnDownLoadNotify.WaitOne(DownLoadTimeout))
            {
                // 転送通知リセット
                OnDownLoadNotify.Reset();

                // 例外
                throw new FtpClientException("Download(Active)タイムアウト");
            }

            // 転送通知リセット
            OnDownLoadNotify.Reset();

            // ロギング
            Logger.DebugFormat("stream:{0}", reciveStream.Stream.ToString());
            Logger.Debug("<<<<= FtpClient::Download()");

            // 返却
            return reciveStream.Stream;
        }

        /// <summary>
        /// ダウンロード(パッシブモード転送)
        /// </summary>
        /// <returns></returns>
        private MemoryStream PassiveDownload()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClient::PassiveDownload()");

            // 結果受信待ち
            FtpClientReciveStream reciveStream = new FtpClientReciveStream();
            reciveStream.Socket = m_FtpClientDataConnection.Socket;
            reciveStream.Buffer = new byte[ReciveBufferCapacity];
            reciveStream.Stream = new MemoryStream(ReciveBufferCapacity);
            reciveStream.FileStream = m_FtpClientDataConnection.FileStream;
            m_FtpClientDataConnection.Socket.BeginReceive(reciveStream.Buffer, 0, reciveStream.Buffer.Length, SocketFlags.None, OnDownloadCallback, reciveStream);

            // データ転送待ち
            if (!OnDownLoadNotify.WaitOne(DownLoadTimeout))
            {
                // 転送通知リセット
                OnDownLoadNotify.Reset();

                // 例外
                throw new FtpClientException("Passive(Active)タイムアウト");
            }

            // 転送通知リセット
            OnDownLoadNotify.Reset();

            // ロギング
            Logger.DebugFormat("stream:{0}", reciveStream.Stream.ToString());
            Logger.Debug("<<<<= FtpClient::PassiveDownload()");

            // 返却
            return reciveStream.Stream;
        }

        /// <summary>
        /// 非同期転送のコールバックメソッド(別スレッドで実行される)
        /// </summary>
        /// <param name="ar"></param>
        private void OnDownloadCallback(IAsyncResult ar)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClient::OnDownloadCallback(IAsyncResult)");
            Logger.DebugFormat("ar:{0}", ar);

            // オブジェクト変換
            FtpClientReciveStream reciveStream = (FtpClientReciveStream)ar.AsyncState;

            // ソケットが切断されているか？
            if (reciveStream.Socket == null || !reciveStream.Socket.Connected)
            {
                // ロギング
                Logger.Warn("既にソケットが切断されています");
                return;
            }

            // 非同期読込が終了しているか？
            int bytesRead = reciveStream.Socket.EndReceive(ar);

            // ロギング
            Logger.InfoFormat("読込データサイズ:[{0}]", bytesRead);

            // 読込サイズ判定
            if (bytesRead > 0)
            {
                // 残りがある場合にはデータ保持する
                if (reciveStream.Stream != null)
                {
                    reciveStream.Stream.Write(reciveStream.Buffer, 0, bytesRead);
                }

                // ファイル書込み
                if (reciveStream.FileStream != null)
                {
                    reciveStream.FileStream.Write(reciveStream.Buffer, 0, bytesRead);
                }

                // 再度受信待ちを実施
                reciveStream.Socket.BeginReceive(reciveStream.Buffer, 0, reciveStream.Buffer.Length, SocketFlags.None, new AsyncCallback(OnDownloadCallback), reciveStream);
            }
            else
            {
                // 転送通知設定
                OnDownLoadNotify.Set();
            }

            // TODO:イベント呼出し
        }
        #endregion

        #region コマンド(同期)
        /// <summary>
        /// 1つ上位のディレクトリ(親ディレクトリ)をカレントディレクトリとする
        /// </summary>
        public FtpResponse CDUP()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::CDUP()");

            // コマンド送信
            FtpResponse response = CommandExec("CDUP");

            // 応答判定
            if (response.StatusCode == 550)
            {
                // ロギング
                Logger.ErrorFormat("異常終了(CDUP):{0}", response.ToString());

                // 実行結果設定
                response.Result = false;
            }
            else if (response.StatusCode != 250)
            {
                // 例外送出
                throw new FtpClientException(response);
            }

            // ロギング
            Logger.DebugFormat("response:{0}", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::CDUP()");

            // 返却
            return response;
        }

        /// <summary>
        /// 指定したディレクトリをカレントディレクトリとする
        /// </summary>
        /// <param name="directoryName">ディレクトリ名</param>
        /// <returns>true:正常(250 Directory successfully changed.) false:異常(550 Failed to change directory.)</returns>
        /// <exception cref="FtpClientException"></exception>
        public FtpResponse CWD(string directoryName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::CWD(string)");
            Logger.DebugFormat("directoryName:{0}", directoryName);

            // コマンド送信
            FtpResponse response = CommandExec("CWD", directoryName);

            // 応答判定
            if (response.StatusCode == 550)
            {
                // ロギング
                Logger.ErrorFormat("異常終了(CWD):{0}", response.ToString());

                // 実行結果設定
                response.Result = false;
            }
            else if (response.StatusCode != 250)
            {
                // 例外送出
                throw new FtpClientException(response);
            }

            // ロギング
            Logger.Debug("<<<<= FtpClientLibrary::CWD(string)");

            // 返却
            return response;
        }

        /// <summary>
        /// 指定したファイルを削除する
        /// </summary>
        /// <param name="fileName"></param>
        public FtpResponse DELE(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::DELE(string)");
            Logger.DebugFormat("fileName:{0}", fileName);

            // コマンド送信
            FtpResponse response = CommandExec("DELE", fileName);

            // 応答判定
            if (response.StatusCode != 250)
            {
                // 例外送出
                throw new FtpClientException(response);
            }

            // ロギング
            Logger.DebugFormat("response:{0}", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::DELE(string)");

            // 返却
            return response;
        }

        #region LIST
        /// <summary>
        /// 現在のワーキングディレクトリ内のファイル一覧を表示する
        /// </summary>
        /// <param name="path"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public FtpResponse LIST(string path, out string[] result)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::LIST(stringe, out string[])");
            Logger.DebugFormat("path:{0}", path);

            // 結果オブジェクト
            FtpResponse response = null;

            // 転送モードによる
            if (m_FtpClientDataConnection.Mode == FtpTransferMode.Active)
            {
                // アクティブモード
                response = ActiveLIST(path, out result);
            }
            else
            {
                // パッシブモード
                response = PassiveLIST(path, out result);
            }

            // ロギング
            Logger.Debug("<<<<= FtpClientLibrary::LIST(string, out string[])");

            // 返却
            return response;
        }

        /// <summary>
        /// 現在のワーキングディレクトリ内のファイル一覧を表示する(Active)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private FtpResponse ActiveLIST(string path, out string[] result)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::ActiveLIST(string, out string[])");
            Logger.DebugFormat("path:{0}", path);

            // コマンド送信
            FtpResponse response = CommandExec("LIST", path);

            // 応答判定
            if (response.StatusCode != 150)
            {
                // 例外送出
                throw new FtpClientException(response);
            }

            // 結果受信待ち
            FtpClientReciveStream reciveStream = CommandResponseWaiting(m_Socket);

            // 応答判定
            if (reciveStream.Response.StatusCode != 226)
            {
                // 例外送出
                throw new FtpClientException(reciveStream.Response);
            }

            // ソケット接続
            m_FtpClientDataConnection.Listener.Start();
            IAsyncResult asyncResult = m_FtpClientDataConnection.Listener.BeginAcceptSocket(new AsyncCallback(OnAcceptNotifyCallBack), m_FtpClientDataConnection);

            // ACCEPT待ち
            if (!OnAcceptNotify.WaitOne(AcceptTimeout))
            {
                // 転送通知リセット
                OnAcceptNotify.Reset();

                // 例外
                throw new TimeoutException("ACCEPT待ちタイムアウト");
            }

            // 転送通知リセット
            OnAcceptNotify.Reset();

            // ソケット設定
            m_FtpClientDataConnection.Socket = m_FtpClientDataConnection.Listener.EndAcceptSocket(asyncResult);

            // ファイル一覧受信
            MemoryStream stream = Recive();

            // ソケット切断
            m_FtpClientDataConnection.Listener.Stop();

            // リスト変換
            result = MemoryStreamToStringArray(stream, new string[] { "\r\n" });

            // ロギング
            Logger.DebugFormat("response:[{0}]", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::ActiveLIST(string)");

            // 返却
            return response;
        }

        /// <summary>
        /// 現在のワーキングディレクトリ内のファイル一覧を表示する(Passive)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private FtpResponse PassiveLIST(string path, out string[] result)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::PassiveLIST(string)");
            Logger.DebugFormat("path:{0}", path);

            // ソケット接続
            m_FtpClientDataConnection.Socket.Connect(m_FtpClientDataConnection.IpAddress, m_FtpClientDataConnection.Port);

            // コマンド送信
            FtpResponse response = CommandExec("LIST", path);

            // 応答判定
            if (response.StatusCode != 150)
            {
                // ソケット切断
                m_FtpClientDataConnection.Socket.Close();

                // 例外送出
                throw new FtpClientException(response);
            }

            // 結果受信待ち
            FtpClientReciveStream reciveStream = CommandResponseWaiting(m_Socket);

            // 応答判定
            if (reciveStream.Response.StatusCode != 226)
            {
                // ソケット切断
                m_FtpClientDataConnection.Socket.Close();

                // 例外送出
                throw new FtpClientException(reciveStream.Response);
            }

            // ファイル一覧受信
            MemoryStream stream = Recive();

            // ソケット切断
            m_FtpClientDataConnection.Socket.Close();

            // リスト変換
            result = MemoryStreamToStringArray(stream, new string[] { "\r\n" });

            // ロギング
            Logger.DebugFormat("response:[{0}]", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::PassiveLIST(string)");

            // 返却
            return response;
        }
        #endregion

        /// <summary>
        /// 指定したディレクトリを作成する
        /// </summary>
        /// <param name="directoryName"></param>
        /// <exception cref="FtpClientException"></exception>
        public FtpResponse MKD(string directoryName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::MKD(string)");
            Logger.DebugFormat("directoryName:{0}", directoryName);

            // コマンド送信
            FtpResponse response = CommandExec("MKD", directoryName);

            // 応答判定
            if (response.StatusCode == 550)
            {
                // ロギング
                Logger.ErrorFormat("異常終了(MKD):{0}", response.ToString());

                // 実行結果設定
                response.Result = false;
            }
            else if (response.StatusCode != 257)
            {
                // 例外送出
                throw new FtpClientException(response);
            }

            // ロギング
            Logger.DebugFormat("response:{0}", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::MKD(string)");

            // 返却
            return response;
        }

        #region NLST
        /// <summary>
        /// 現在のワーキングディレクトリ内のファイル一覧を表示する
        /// </summary>
        /// <param name="result"></param>
        /// <param name="fileName"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public FtpResponse NLST(out List<string> result, string fileName, params string[] option)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::NLST(out FtpResponse, string, params string[])");
            Logger.DebugFormat("fileName:{0}", fileName);
            Logger.DebugFormat("option  :{0}", option.Length);

            // 結果オブジェクト
            FtpResponse response = null;

            // 転送モードによる
            if (m_FtpClientDataConnection.Mode == FtpTransferMode.Active)
            {
                // アクティブモード
                response = ActiveNLST(out result, fileName, option);
            }
            else
            {
                // パッシブモード
                response = PassiveNLST(out result, fileName, option);
            }

            // ロギング
            Logger.Debug("<<<<= FtpClientLibrary::NLST(out List<string> result, string, params string[])");

            // 返却
            return response;
        }

        /// <summary>
        /// 現在のワーキングディレクトリ内のファイル一覧を表示する(Passive)
        /// </summary>
        /// <param name="result"></param>
        /// <param name="fileName"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        private FtpResponse ActiveNLST(out List<string> result, string fileName, params string[] option)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::ActiveNLST(out List<string>, string, params string[])");
            Logger.DebugFormat("fileName:{0}", fileName);
            Logger.DebugFormat("option  :{0}", option.Length);

            // コマンドパラメータ設定
            List<string> parameters = new List<string>(option);
            parameters.Add(fileName);

            // コマンド送信
            FtpResponse response = CommandExec(string.Format("NLST ", fileName), parameters.ToArray());

            // 応答判定
            if (response.StatusCode != 150)
            {
                // 例外送出
                throw new FtpClientException(response);
            }

            // 結果受信待ち
            FtpClientReciveStream reciveStream = CommandResponseWaiting(m_Socket);

            // 応答判定
            if (reciveStream.Response.StatusCode != 226)
            {
                // 例外送出
                throw new FtpClientException(reciveStream.Response);
            }

            // ソケット接続
            m_FtpClientDataConnection.Listener.Start();
            IAsyncResult asyncResult = m_FtpClientDataConnection.Listener.BeginAcceptSocket(new AsyncCallback(OnAcceptNotifyCallBack), m_FtpClientDataConnection);

            // ACCEPT待ち
            if (!OnAcceptNotify.WaitOne(AcceptTimeout))
            {
                // 転送通知リセット
                OnAcceptNotify.Reset();

                // 例外
                throw new TimeoutException("ACCEPT待ちタイムアウト");
            }

            // 転送通知リセット
            OnAcceptNotify.Reset();

            // ソケット設定
            m_FtpClientDataConnection.Socket = m_FtpClientDataConnection.Listener.EndAcceptSocket(asyncResult);

            // ファイル一覧受信
            MemoryStream stream = Recive();

            // ソケット切断
            m_FtpClientDataConnection.Listener.Stop();

            // リスト変換
            result = new List<string>(MemoryStreamToStringArray(stream, new string[] { "\r\n" }));

            // ロギング
            Logger.DebugFormat("response:[{0}]", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::ActiveNLST(out List<string>, string, params string[])");

            // 返却
            return response;
        }

        /// <summary>
        /// 現在のワーキングディレクトリ内のファイル一覧を表示する(Passive)
        /// </summary>
        /// <param name="result"></param>
        /// <param name="fileName"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        private FtpResponse PassiveNLST(out List<string> result, string fileName, params string[] option)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::PassiveNLST(out List<string>, string, params string[])");
            Logger.DebugFormat("fileName:{0}", fileName);
            Logger.DebugFormat("option  :{0}", option.Length);

            // ソケット接続
            m_FtpClientDataConnection.Socket.Connect(m_FtpClientDataConnection.IpAddress, m_FtpClientDataConnection.Port);

            // コマンドパラメータ設定
            List<string> parameters = new List<string>(option)
            {
                fileName
            };

            // コマンド送信
            FtpResponse response = CommandExec("NLST", parameters.ToArray());

            // 応答判定
            if (response.StatusCode != 150)
            {
                // ソケット切断
                m_FtpClientDataConnection.Socket.Close();

                // 例外送出
                throw new FtpClientException(response);
            }

            // 結果受信待ち
            FtpClientReciveStream reciveStream = CommandResponseWaiting(m_Socket);

            // 応答判定
            if (reciveStream.Response.StatusCode != 226)
            {
                // ソケット切断
                m_FtpClientDataConnection.Socket.Close();

                // 例外送出
                throw new FtpClientException(reciveStream.Response);
            }

            // ファイル一覧受信
            MemoryStream stream = Recive();

            // ソケット切断
            m_FtpClientDataConnection.Socket.Close();

            // リスト変換
            result = new List<string>(MemoryStreamToStringArray(stream, new string[] { "\r\n" }));

            // ロギング
            Logger.DebugFormat("response:[{0}]", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::PassiveNLST(out List<string>, string, params string[])");

            // 返却
            return response;
        }
        #endregion

        /// <summary>
        /// 何もしない。サーバの稼動を確認するために実⾏される。
        /// </summary>
        public FtpResponse NOOP()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::NOOP()");

            // コマンド送信
            FtpResponse response = CommandExec("NOOP");

            // 応答判定
            if (response.StatusCode != 200)
            {
                // 例外送出
                throw new FtpClientException(response);
            }

            // ロギング
            Logger.DebugFormat("response:{0}", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::NOOP()");

            // 返却
            return response;
        }

        /// <summary>
        /// ユーザーのパスワードを指定する
        /// </summary>
        /// <param name="userPassword"></param>
        /// <exception cref="FtpClientException"></exception>
        public FtpResponse PASS(string userPassword)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::PASS(string)");
            Logger.DebugFormat("userPassword:{0}", new string('*', userPassword.Length));

            // コマンド送信
            FtpResponse response = CommandExec("PASS", userPassword);

            // 応答判定
            if (response.StatusCode != 230)
            {
                // 例外送出
                throw new FtpClientException(response);
            }

            // ロギング
            Logger.DebugFormat("response:{0}", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::PASS(string)");

            // 返却
            return response;
        }

        /// <summary>
        /// パッシブモードへの移行を指示する。
        /// </summary>
        /// <exception cref="FtpClientException"></exception>
        /// <returns></returns>
        public FtpResponse PASV()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::PASV()");

            // コマンド送信
            FtpResponse response = CommandExec("PASV");

            // 応答判定
            if (response.StatusCode != 227)
            {
                // 例外送出
                throw new FtpClientException(response);
            }

            // 受信したIPアドレスとポートを抽出
            string host = string.Empty;
            string[] ports = null;
            int port = 0;
            Regex regex = new Regex(@"\((?<address>[0-9]+,[0-9]+,[0-9]+,[0-9]+),(?<port>[0-9]+,[0-9]+)\)", RegexOptions.Singleline);
            for (Match match = regex.Match(response.StatusDetail.ToString()); match.Success; match = match.NextMatch())
            {
                host = match.Groups["address"].Value.ToString().Replace(',', '.');
                ports = match.Groups["port"].Value.ToString().Split(new char[] { ',' });
            }

            // ポート設定
            if (ports.Length != 2)
            {
                // 例外送出
                throw new FtpClientException("IPアドレス／ポート抽出異常");
            }
            port = ((int.Parse(ports[0]) & 0x000000ff) << 8) + (int.Parse(ports[1]) & 0x000000ff);

            // パッシブモード初期化
            InitPassiveMode(host, port);

            // ロギング
            Logger.DebugFormat("response:[{0}]", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::PASV()");

            // 返却
            return response;
        }

        /// <summary>
        /// データコネクションで使用するIPアドレス（通常はクライアント）とポート番号を指示する
        /// </summary>
        /// <returns></returns>
        public FtpResponse PORT()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::PORT()");

            // アクディブモード初期化
            InitActiveMode();

            // IPアドレス変換
            string host = m_FtpClientDataConnection.IpAddress.Replace(".", ",");

            // ポート番号分解
            int[] ports = new int[2] { (m_FtpClientDataConnection.Port & 0x0000ff00) >> 8, (m_FtpClientDataConnection.Port & 0x000000ff) };

            // パラメータ設定
            string parameter = string.Format("{0},{1},{2}", host, ports[0], ports[1]);

            // コマンド送信
            FtpResponse response = CommandExec("PORT", parameter);

            // 応答判定
            if (response.StatusCode != 200)
            {
                // 例外送出
                throw new FtpClientException(response);
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::PORT()");

            // 返却
            return response;
        }

        /// <summary>
        /// 現在のワーキングディレクトリを表示する
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public FtpResponse PWD(out string result)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::PWD(out FtpResponse)");

            // コマンド送信
            FtpResponse response = CommandExec("PWD");

            // 応答判定
            if (response.StatusCode != 257)
            {
                // 例外送出
                throw new FtpClientException(response);
            }

            // カレントディレクトリ抽出
            result = string.Empty;
            Regex regex = new Regex("\"(?<path>.*)\"", RegexOptions.Singleline);
            for (Match match = regex.Match(response.StatusDetail.ToString()); match.Success; match = match.NextMatch())
            {
                result = match.Groups["path"].Value.ToString().Replace("\"", "");
            }

            // ロギング
            Logger.DebugFormat("response:[{0}]", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::PWD(out FtpResponse)");

            // 返却
            return response;
        }

        /// <summary>
        /// ログアウトする
        /// </summary>
        /// <exception cref="FtpClientException"></exception>
        public FtpResponse QUIT()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::QUIT()");

            // コマンド送信
            FtpResponse response = CommandExec("QUIT");

            // 応答判定
            if (response.StatusCode != 221)
            {
                // 例外送出
                throw new FtpClientException(response);
            }

            // ロギング
            Logger.DebugFormat("response:{0}", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::QUIT()");

            // 返却
            return response;
        }

        #region RETR
        /// <summary>
        /// 指定したファイルの内容をサーバから取得する
        /// </summary>
        /// <param name="response"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FtpResponse RETR(string fileName, out MemoryStream result)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::RETR(string, out MemoryStream)");
            Logger.DebugFormat("fileName:{0}", fileName);

            // 結果オブジェクト
            FtpResponse response = null;

            // 転送モードによる
            if (m_FtpClientDataConnection.Mode == FtpTransferMode.Active)
            {
                // アクティブモード
                response = ActiveRETR(fileName, out result);
            }
            else
            {
                // パッシブモード
                response = PassiveRETR(fileName, out result);
            }

            // ロギング
            Logger.DebugFormat("response:{0}", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::RETR(string, out MemoryStream)");

            // 返却
            return response;
        }

        /// <summary>
        /// 指定したファイルの内容をサーバから取得する(Active)
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public FtpResponse ActiveRETR(string fileName, out MemoryStream result)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::ActiveRETR(string, out MemoryStream)");
            Logger.DebugFormat("fileName:{0}", fileName);

            // コマンド送信
            FtpResponse response = CommandExec("RETR", fileName);

            // 応答判定
            if (response.StatusCode != 150)
            {
                // 例外送出
                throw new FtpClientException(response);
            }

            // 結果受信待ち
            FtpClientReciveStream reciveStream = CommandResponseWaiting(m_Socket);

            // 応答判定
            if (reciveStream.Response.StatusCode != 226)
            {
                // 例外送出
                throw new FtpClientException(reciveStream.Response);
            }

            // ソケット接続
            m_FtpClientDataConnection.Listener.Start();
            IAsyncResult asyncResult = m_FtpClientDataConnection.Listener.BeginAcceptSocket(new AsyncCallback(OnAcceptNotifyCallBack), m_FtpClientDataConnection);

            // ACCEPT待ち
            if (!OnAcceptNotify.WaitOne(AcceptTimeout))
            {
                // 転送通知リセット
                OnAcceptNotify.Reset();

                // 例外
                throw new TimeoutException("ACCEPT待ちタイムアウト");
            }

            // 転送通知リセット
            OnAcceptNotify.Reset();

            // ソケット設定
            m_FtpClientDataConnection.Socket = m_FtpClientDataConnection.Listener.EndAcceptSocket(asyncResult);

            // ファイル転送(ローカル⇒サーバ)
            result = Download();

            // 転送結果判定
            if (result == null)
            {
                // 例外送出
                throw new FtpClientException("ファイル転送に失敗しました：" + fileName);
            }

            // ソケット切断
            m_FtpClientDataConnection.Listener.Stop();

            // ロギング
            Logger.DebugFormat("response:[{0}]", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::ActiveRETR(string, out MemoryStream)");

            // 返却
            return response;
        }

        /// <summary>
        /// 指定したファイルの内容をサーバから取得する(Passive)
        /// </summary>
        /// <param name="dataConnection"></param>
        /// <param name="response"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FtpResponse PassiveRETR(string fileName, out MemoryStream result)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::PassiveRETR(string, out MemoryStream)");
            Logger.DebugFormat("fileName:{0}", fileName);

            // ソケット接続
            m_FtpClientDataConnection.Socket.Connect(m_FtpClientDataConnection.IpAddress, m_FtpClientDataConnection.Port);

            // コマンド送信
            FtpResponse response = CommandExec("RETR", fileName);

            // 応答判定
            if (response.StatusCode != 150)
            {
                // ソケット切断
                m_FtpClientDataConnection.Socket.Close();

                // 例外送出
                throw new FtpClientException(response);
            }

            // 結果受信待ち
            FtpClientReciveStream reciveStream = CommandResponseWaiting(m_Socket);

            // 応答判定
            if (reciveStream.Response.StatusCode != 226)
            {
                // ソケット切断
                m_FtpClientDataConnection.Socket.Close();

                // 例外送出
                throw new FtpClientException(reciveStream.Response);
            }

            // ファイル転送(ローカル⇒サーバ)
            result = Download();

            // 転送結果判定
            if (result == null)
            {
                // 例外送出
                throw new FtpClientException("ファイル転送に失敗しました：" + fileName);
            }

            // ソケット切断
            m_FtpClientDataConnection.Socket.Close();

            // ロギング
            Logger.DebugFormat("response:[{0}]", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::PassiveRETR(string, out MemoryStream)");

            // 返却
            return response;
        }
        #endregion

        /// <summary>
        /// 指定したディレクトリを削除する
        /// </summary>
        /// <param name="directoryName"></param>
        public FtpResponse RMD(string directoryName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::RMD(string)");
            Logger.DebugFormat("directoryName:{0}", directoryName);

            // コマンド送信
            FtpResponse response = CommandExec("RMD", directoryName);

            // 応答判定
            if (response.StatusCode == 550)
            {
                // ロギング
                Logger.ErrorFormat("異常終了(RMD):{0}", response.ToString());

                // 実行結果設定
                response.Result = false;
            }
            else if (response.StatusCode != 250)
            {
                // 例外送出
                throw new FtpClientException(response);
            }

            // ロギング
            Logger.DebugFormat("response:{0}", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::RMD(string)");

            // 返却
            return response;
        }

        /// <summary>
        /// 指定したファイル名を変更する(変更元ファイル名の指定)。
        /// RNTOを続けて実行しなくてはならない
        /// </summary>
        /// <param name="fileName">変更元ファイル名</param>
        /// <returns></returns>
        public FtpResponse RNFR(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::RNFR(string)");
            Logger.DebugFormat("directoryName:{0}", fileName);

            // コマンド送信
            FtpResponse response = CommandExec("RNFR", fileName);

            // 応答判定
            if (response.StatusCode == 550)
            {
                // ロギング
                Logger.ErrorFormat("異常終了(RNFR):{0}", response.ToString());

                // 実行結果設定
                response.Result = false;
            }
            else if (response.StatusCode != 350)
            {
                // 例外送出
                throw new FtpClientException(response);
            }

            // ロギング
            Logger.DebugFormat("response:{0}", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::RNFR(string)");

            // 返却
            return response;
        }

        /// <summary>
        /// RNFRコマンドで指定したファイルを、指定したファイル名に変更する。
        /// </summary>
        /// <param name="fileName">変更先ファイル名</param>
        /// <returns></returns>
        public FtpResponse RNTO(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::RNTO(string)");
            Logger.DebugFormat("directoryName:{0}", fileName);

            // コマンド送信
            FtpResponse response = CommandExec("RNTO", fileName);

            // 応答判定
            if (response.StatusCode == 550)
            {
                // ロギング
                Logger.ErrorFormat("異常終了(RNTO):{0}", response.ToString());

                // 実行結果設定
                response.Result = false;
            }
            else if (response.StatusCode != 250)
            {
                // 例外送出
                throw new FtpClientException(response);
            }

            // ロギング
            Logger.DebugFormat("response:{0}", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::RNTO(string)");

            // 返却
            return response;
        }

        /// <summary>
        /// 任意のOSコマンドを実行する
        /// </summary>
        /// <param name="command"></param>
        public FtpResponse SITE(string command)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::SITE(string)");
            Logger.DebugFormat("command:{0}", command);

            // コマンド送信
            FtpResponse response = CommandExec("SITE", command);

            // 応答判定
            if (response.StatusCode != 200)
            {
                // 例外送出
                throw new FtpClientException(response);
            }

            // ロギング
            Logger.DebugFormat("response:{0}", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::SITE(string)");

            // 返却
            return response;
        }

        #region STOR
        /// <summary>
        /// 指定したファイル名で、サーバへ送信するデータでファイルを作成する
        /// <remarks>同⼀名のファイルがすでにある場合には、上書きする</remarks>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FtpResponse STOR(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::STOR(string)");
            Logger.DebugFormat("fileName:{0}", fileName);

            // 結果オブジェクト
            FtpResponse response = null;

            // 転送モードによる
            if (m_FtpClientDataConnection.Mode == FtpTransferMode.Active)
            {
                // アクティブモード
                response = ActiveSTOR(fileName);
            }
            else
            {
                // パッシブモード
                response = PassiveSTOR(fileName);
            }

            // ロギング
            Logger.DebugFormat("response:{0}", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::STOR(string)");

            // 返却
            return response;
        }

        /// <summary>
        /// 指定したファイル名で、サーバへ送信するデータでファイルを作成する(Passive)
        /// <remarks>同⼀名のファイルがすでにある場合には、上書きする</remarks>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FtpResponse ActiveSTOR(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::ActiveSTOR(string)");
            Logger.DebugFormat("fileName:{0}", fileName);

            // コマンド送信
            FtpResponse response = CommandExec("STOR", Path.GetFileName(fileName));

            // 応答判定
            if (response.StatusCode != 150)
            {
                // 例外送出
                throw new FtpClientException(response);
            }

            // ソケット接続
            m_FtpClientDataConnection.Listener.Start();
            IAsyncResult _IAsyncResult = m_FtpClientDataConnection.Listener.BeginAcceptSocket(new AsyncCallback(OnAcceptNotifyCallBack), m_FtpClientDataConnection);

            // ACCEPT待ち
            if (!OnAcceptNotify.WaitOne(AcceptTimeout))
            {
                // 転送通知リセット
                OnAcceptNotify.Reset();

                // 実行結果設定
                response.Result = false;

                // ロギング
                Logger.DebugFormat("response:{0}", response.ToString());
                Logger.Debug("<<<<= FtpClientLibrary::ActiveSTOR(string)");

                // 返却
                return response;
            }

            // 転送通知リセット
            OnAcceptNotify.Reset();

            // ソケット設定
            m_FtpClientDataConnection.Socket = m_FtpClientDataConnection.Listener.EndAcceptSocket(_IAsyncResult);

            // ファイル送信
            if (!FileUpload(fileName))
            {
                // ソケット切断
                m_FtpClientDataConnection.Socket.Close();

                // 実行結果設定
                response.Result = false;

                // ロギング
                Logger.DebugFormat("response:{0}", response.ToString());
                Logger.Debug("<<<<= FtpClientLibrary::ActiveSTOR(string)");

                // 返却
                return response;
            }

            // ソケット切断
            m_FtpClientDataConnection.Socket.Close();

            // 結果受信待ち
            FtpClientReciveStream reciveStream = CommandResponseWaiting(m_Socket);

            // 応答判定
            if (reciveStream.Response.StatusCode != 226)
            {
                // 例外送出
                throw new FtpClientException(reciveStream.Response);
            }

            // ロギング
            Logger.DebugFormat("response:{0}", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::ActiveSTOR(string)");

            // 返却
            return response;
        }

        /// <summary>
        /// 指定したファイル名で、サーバへ送信するデータでファイルを作成する(Passive)
        /// <remarks>同⼀名のファイルがすでにある場合には、上書きする</remarks>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FtpResponse PassiveSTOR(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::PassiveSTOR(string)");
            Logger.DebugFormat("fileName:{0}", fileName);

            // ソケット接続
            m_FtpClientDataConnection.Socket.Connect(m_FtpClientDataConnection.IpAddress, m_FtpClientDataConnection.Port);

            // コマンド送信
            FtpResponse response = CommandExec("STOR", Path.GetFileName(fileName));

            // 応答判定
            if (response.StatusCode != 150)
            {
                // ソケット切断
                m_FtpClientDataConnection.Socket.Close();

                // 例外送出
                throw new FtpClientException(response);
            }

            // ファイル送信
            if (!FileUpload(fileName))
            {
                // ソケット切断
                m_FtpClientDataConnection.Socket.Close();

                // 実行結果設定
                response.Result = false;

                // ロギング
                Logger.DebugFormat("response:{0}", response.ToString());
                Logger.Debug("<<<<= FtpClientLibrary::PassiveSTOR(string)");

                // 返却
                return response;
            }

            // ソケット切断
            m_FtpClientDataConnection.Socket.Close();

            // 結果受信待ち
            FtpClientReciveStream reciveStream = CommandResponseWaiting(m_Socket);

            // 応答判定
            if (reciveStream.Response.StatusCode != 226)
            {
                // 例外送出
                throw new FtpClientException(reciveStream.Response);
            }

            // ロギング
            Logger.DebugFormat("response:{0}", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::PassiveSTOR(string)");

            // 返却
            return response;
        }
        #endregion

        #region STOU
        /// <summary>
        /// 指定したファイル名で、サーバへ送信するデータでファイルを作成する。
        /// <remarks>すでに同一名のファイルがあった場合には、重ならないようなファイル名を自動的に付けて作成する</remarks>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FtpResponse STOU(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::STOU(string)");
            Logger.DebugFormat("fileName:{0}", fileName);

            // 結果オブジェクト
            FtpResponse response = null;

            // 転送モードによる
            if (m_FtpClientDataConnection.Mode == FtpTransferMode.Active)
            {
                // アクティブモード
                response = ActiveSTOU(fileName);
            }
            else
            {
                // パッシブモード
                response = PassiveSTOU(fileName);
            }

            // ロギング
            Logger.DebugFormat("response:{0}", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::STOU(string)");

            // 返却
            return response;
        }

        /// <summary>
        /// 指定したファイル名で、サーバへ送信するデータでファイルを作成する(Active)
        /// <remarks>すでに同一名のファイルがあった場合には、重ならないようなファイル名を自動的に付けて作成する</remarks>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FtpResponse ActiveSTOU(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::ActiveSTOU(string)");
            Logger.DebugFormat("fileName:{0}", fileName);

            // コマンド送信
            FtpResponse response = CommandExec("STOU", Path.GetFileName(fileName));

            // 応答判定
            if (response.StatusCode != 150)
            {
                // ソケット切断
                m_FtpClientDataConnection.Socket.Close();

                // 例外送出
                throw new FtpClientException(response);
            }

            // ソケット接続
            m_FtpClientDataConnection.Listener.Start();
            IAsyncResult _IAsyncResult = m_FtpClientDataConnection.Listener.BeginAcceptSocket(new AsyncCallback(OnAcceptNotifyCallBack), m_FtpClientDataConnection);

            // ACCEPT待ち
            if (!OnAcceptNotify.WaitOne(AcceptTimeout))
            {
                // 転送通知リセット
                OnAcceptNotify.Reset();

                // 実行結果設定
                response.Result = false;

                // ロギング
                Logger.DebugFormat("response:{0}", response.ToString());
                Logger.Debug("<<<<= FtpClientLibrary::ActiveSTOU(string)");

                // 返却
                return response;
            }

            // 転送通知リセット
            OnAcceptNotify.Reset();

            // ソケット設定
            m_FtpClientDataConnection.Socket = m_FtpClientDataConnection.Listener.EndAcceptSocket(_IAsyncResult);

            // ファイル送信
            if (!FileUpload(fileName))
            {
                // ソケット切断
                m_FtpClientDataConnection.Socket.Close();

                // 実行結果設定
                response.Result = false;

                // ロギング
                Logger.DebugFormat("response:{0}", response.ToString());
                Logger.Debug("<<<<= FtpClientLibrary::ActiveSTOU(string)");

                // 返却
                return response;
            }

            // ソケット切断
            m_FtpClientDataConnection.Socket.Close();

            // 結果受信待ち
            FtpClientReciveStream reciveStream = CommandResponseWaiting(m_Socket);

            // 応答判定
            if (reciveStream.Response.StatusCode != 226)
            {
                // 例外送出
                throw new FtpClientException(reciveStream.Response);
            }

            // ロギング
            Logger.DebugFormat("response:{0}", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::ActiveSTOU(string)");

            // 返却
            return response;
        }

        /// <summary>
        /// 指定したファイル名で、サーバへ送信するデータでファイルを作成する(Passive)
        /// <remarks>すでに同一名のファイルがあった場合には、重ならないようなファイル名を自動的に付けて作成する</remarks>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FtpResponse PassiveSTOU(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::PassiveSTOU(string)");
            Logger.DebugFormat("fileName:{0}", fileName);

            // ソケット接続
            m_FtpClientDataConnection.Socket.Connect(m_FtpClientDataConnection.IpAddress, m_FtpClientDataConnection.Port);

            // コマンド送信
            FtpResponse response = CommandExec("STOU", Path.GetFileName(fileName));

            // 応答判定
            if (response.StatusCode != 150)
            {
                // ソケット切断
                m_FtpClientDataConnection.Socket.Close();

                // 例外送出
                throw new FtpClientException(response);
            }

            // ファイル送信
            if (!FileUpload(fileName))
            {
                // ソケット切断
                m_FtpClientDataConnection.Socket.Close();

                // 実行結果設定
                response.Result = false;

                // ロギング
                Logger.DebugFormat("response:{0}", response.ToString());
                Logger.Debug("<<<<= FtpClientLibrary::PassiveSTOU(string)");

                // 返却
                return response;
            }

            // ソケット切断
            m_FtpClientDataConnection.Socket.Close();

            // 結果受信待ち
            FtpClientReciveStream reciveStream = CommandResponseWaiting(m_Socket);

            // 応答判定
            if (reciveStream.Response.StatusCode != 226)
            {
                // 例外送出
                throw new FtpClientException(reciveStream.Response);
            }

            // ロギング
            Logger.DebugFormat("response:{0}", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::PassiveSTOU(string)");

            // 返却
            return response;
        }
        #endregion

        /// <summary>
        /// ファイルアップロード
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool FileUpload(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::FileUpload(string)");
            Logger.DebugFormat("fileName:{0}", fileName);

            // ファイル送信
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                // 送信分割数分繰り返す
                long remainingSize = fileStream.Length;
                for (long count = 0; count < fileStream.Length; count += SendBufferCapacity)
                {
                    // 送信サイズ決定
                    long sendSize = 0;
                    if (remainingSize > SendBufferCapacity)
                    {
                        sendSize = SendBufferCapacity;
                    }
                    else
                    {
                        sendSize = remainingSize;
                    }

                    // ロギング
                    Logger.DebugFormat("└ 送信ァイルサイズ  ：[{0}]", sendSize);

                    // ファイルをMemoryStreamに読み込む
                    MemoryStream stream = new MemoryStream((int)sendSize);
                    byte[] bytes = new byte[sendSize];
                    fileStream.Read(bytes, 0, bytes.Length);
                    stream.Write(bytes, 0, bytes.Length);

                    // ファイル転送(ローカル⇒サーバ)
                    if (!Upload(stream))
                    {
                        // 異常終了
                        return false;
                    }

                    // 残りサイズを決定
                    remainingSize -= sendSize;
                    Logger.DebugFormat("└ 残りファイルサイズ：[{0}]", remainingSize);
                }
            }

            // ロギング
            Logger.Debug("<<<<= FtpClientLibrary::FileUpload(string)");

            // 正常終了
            return true;
        }

        /// <summary>
        /// システム名を表示する
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public string SYST(out FtpResponse response)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::SYST(out FtpResponse)");

            // コマンド送信
            response = CommandExec("SYST");

            // 応答判定
            if (response.StatusCode == 215)
            {
                // ロギング
                Logger.DebugFormat("StatusDetail:{0}", response.StatusDetail);
                Logger.Debug("<<<<= FtpClientLibrary::SYST()");

                // 結果を返却
                return response.StatusDetail;
            }

            // ロギング
            Logger.DebugFormat("StatusDetail:string.Empty");
            Logger.Debug("<<<<= FtpClientLibrary::SYST()");

            // 空文字を返却
            return string.Empty;
        }

        /// <summary>
        /// 指定したユーザー名でログインする
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="response"></param>
        /// <exception cref="FtpClientException"></exception>
        public FtpResponse USER(string userName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::USER(string)");
            Logger.DebugFormat("userName:{0}", userName);

            // コマンド送信
            FtpResponse response = CommandExec("USER", userName);

            // 応答判定
            if (response.StatusCode != 331)
            {
                // 例外送出
                throw new FtpClientException(response.ToString());
            }

            // ロギング
            Logger.DebugFormat("response:{0}", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::USER(string)");

            // 返却
            return response;
        }

        /// <summary>
        /// 転送するファイルのファイル構造をオプションで示す
        /// </summary>
        /// <param name="option">A：ASCII I：Image（バイナリ）</param>
        /// <exception cref="FtpClientException"></exception>
        public FtpResponse TYPE(string option)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::TYPE(string)");
            Logger.DebugFormat("option:{0}", option);

            // コマンド送信
            FtpResponse response = CommandExec("TYPE", option);

            // 応答判定
            if (response.StatusCode != 200)
            {
                // 例外送出
                throw new FtpClientException(response);
            }

            // ロギング
            Logger.DebugFormat("response:{0}", response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::TYPE(string)");

            // 返却
            return response;
        }
        #endregion

        #region コマンド実行
        /// <summary>
        /// コマンド実行
        /// </summary>
        /// <param name="command"></param>
        public FtpResponse CommandExec(string command, params string[] parameter)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::CommandExec(string, params string[])");
            Logger.DebugFormat("command  :{0}", command);
            Logger.DebugFormat("parameter:{0}", parameter.Length);

            // 送信コマンド作成
            string send_command = MakeCommandString(command, parameter);
            Logger.InfoFormat("送信コマンド：[{0}]", send_command);

            // コマンド送信
            Send(send_command + "\r\n");

            // ソケット判定
            if (m_Socket == null && m_Socket.Connected)
            {
                // ロギング
                Logger.Warn("既にソケットが切断されています");
                return null;
            }

            // コマンド受信応答待ち
            FtpClientReciveStream reciveStream = CommandResponseWaiting(m_Socket);

            // ロギング
            Logger.Debug(reciveStream.Response.ToString());
            Logger.Debug("<<<<= FtpClientLibrary::CommandExec(string, params string[])");

            // コマンド応答
            return reciveStream.Response;
        }

        /// <summary>
        /// コマンド文字列作成
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private string MakeCommandString(string command, params string[] parameter)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::MakeCommandString(string, params string[])");
            Logger.DebugFormat("command  :{0}", command);
            Logger.DebugFormat("parameter:{0}", parameter.Length);

            /// 結果オブジェクト生成
            StringBuilder result = new StringBuilder();

            // 結果にコマンド追加
            result.Append(command + " ");

            // コマンドパラメータ分繰り返す
            foreach (string param in parameter)
            {
                // 結果にコマンドパラメータ追加
                result.Append(param + " ");
            }

            // ロギング
            Logger.Debug("<<<<= FtpClientLibrary::MakeCommandString(string, params string[])");

            // 返却(最後の空白は削除)
            return result.ToString().TrimEnd(new char[] { ' ' });
        }
        #endregion

        #region コマンド応答待ち
        /// <summary>
        /// コマンド応答待ち
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private FtpClientReciveStream CommandResponseWaiting(Socket socket)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClient::CommandResponseWaiting(Socket)");
            Logger.DebugFormat("socket:{0}", socket);

            // コマンド応答データオブジェクト生成
            FtpClientReciveStream reciveStream = new FtpClientReciveStream();
            reciveStream.Socket = socket;
            reciveStream.Buffer = new byte[1];
            reciveStream.Stream = new MemoryStream();
            reciveStream.Socket.BeginReceive(reciveStream.Buffer, 0, 1, SocketFlags.None, OnReciveCommandCallBack, reciveStream);

            // データ受信待ち
            if (!OnCommandResponceNotify.WaitOne((int)Timeout.Recive.TotalMilliseconds))
            {
                // コマンド応答完了通知リセット
                OnCommandResponceNotify.Reset();

                // 例外
                throw new FtpClientException("コマンド応答受信タイムアウト");
            }

            // コマンド応答完了通知リセット
            OnCommandResponceNotify.Reset();

            // コマンド応答を返却
            return reciveStream;
        }

        /// <summary>
        /// コマンド応答受信コールバック
        /// </summary>
        /// <param name="asyncResult"></param>
        private void OnReciveCommandCallBack(IAsyncResult ar)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClient::OnReciveCommandCallBack(IAsyncResult)");
            Logger.DebugFormat("ar:{0}", ar);

            // オブジェクト変換
            FtpClientReciveStream reciveStream = (FtpClientReciveStream)ar.AsyncState;

            // ソケットが切断されているか？
            if (reciveStream.Socket == null || !reciveStream.Socket.Connected)
            {
                // ロギング
                Logger.Warn("既にソケットが切断されています");
                return;
            }

            // 受信終了
            int endReceive = reciveStream.Socket.EndReceive(ar);

            // 受信されているか？
            if (endReceive > 0)
            {
                // コマンド受信は1文字のみ認める
                if (endReceive != 1)
                {
                    // 例外
                    throw new FtpClientException("コマンド応答受信サイズ異常:[" + endReceive + "]");
                }

                // バッファ内に保持する
                reciveStream.Stream.WriteByte(reciveStream.Buffer[0]);

                // 改行を受信していない場合
                if (reciveStream.Buffer[0] != '\n')
                {
                    // 受信再開
                    reciveStream.Socket.BeginReceive(reciveStream.Buffer, 0, 1, SocketFlags.None, OnReciveCommandCallBack, reciveStream);
                }
                else
                {
                    // コマンド応答結果変換
                    reciveStream.Response = ResponseParse(reciveStream.Stream.ToArray());

                    // コマンド応答受信設定
                    OnCommandResponceNotify.Set();
                }
            }
            else
            {
                // コマンド応答受信設定
                OnCommandResponceNotify.Set();
            }

            // ロギング
            Logger.Debug("<<<<= FtpClient::OnReciveCommandCallBack(IAsyncResult)");
        }
        #endregion

        #region 応答解析
        /// <summary>
        /// 応答解析
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private FtpResponse ResponseParse(byte[] response)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClient::ResponseParse(byte[])");
            Logger.DebugFormat("response:{0}", response);

            // 結果オブジェクト
            StringBuilder result = new StringBuilder();

            // 結果に追加
            result.Append(Encoding.GetString(response).TrimEnd());

            // ロギング
            Logger.Debug("<<<<= FtpClient::ResponseParse(byte[])");

            // 返却
            return ResponseParse(result);
        }

        /// <summary>
        /// 応答解析
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private FtpResponse ResponseParse(StringBuilder response)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClient::ResponseParse(StringBuilder)");
            Logger.DebugFormat("response:{0}", response.ToString());

            // 結果オブジェクト
            FtpResponse result = new FtpResponse();

            // 正規表現オブジェクト生成
            Regex regex = new Regex("^(?<code>[1-6][0-9][0-9])[ ]*(?<detail>.*)", RegexOptions.Singleline);

            // 正規表現判定
            for (Match match = regex.Match(response.ToString()); match.Success; match = match.NextMatch())
            {
                // 結果設定
                result.StatusCode = int.Parse(match.Groups["code"].Value);
                result.StatusDetail = match.Groups["detail"].Value;
            }

            // ロギング
            Logger.InfoFormat("コマンド応答：{0}", result.ToString());
            Logger.Debug("<<<<= FtpClient::ResponseParse(StringBuilder)");

            // 返却
            return result;
        }
        #endregion

        #region Accept完了通知
        /// <summary>
        /// 非同期Accept完了通知のコールバックメソッド(別スレッドで実行される)
        /// </summary>
        /// <param name="ar"></param>
        private void OnAcceptNotifyCallBack(IAsyncResult ar)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClient::OnAcceptNotifyCallBack(IAsyncResult)");
            Logger.DebugFormat("ar:{0}", ar);

            // オブジェクト変換
            FtpClientDataConnection dataConnection = (FtpClientDataConnection)ar.AsyncState;

            // ロギング
            Logger.DebugFormat(dataConnection.ToString());

            // Accept完了通知
            OnAcceptNotify.Set();

            // ロギング
            Logger.Debug("<<<<= FtpClient::OnAcceptNotifyCallBack(IAsyncResult)");
        }
        #endregion

        #region MemoryStreamから文字列配列に変換
        /// <summary>
        /// MemoryStreamから文字列配列に変換
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <param name="separetor"></param>
        /// <returns></returns>
        private string[] MemoryStreamToStringArray(MemoryStream memoryStream, string[] separetor)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClient::MemoryStreamToStringArray(MemoryStream, string[])");
            Logger.DebugFormat("memoryStream:{0}", memoryStream);
            Logger.DebugFormat("separetor   :{0}", separetor.Length);

            // MemoryStreamを文字列化(改行を含む1行：但し、最終改行は削除)
            string getString = Encoding.GetString(memoryStream.ToArray()).TrimEnd('\r', '\n');

            // 分割
            string[] result = getString.Split(separetor, StringSplitOptions.None);

            // ロギング
            Logger.DebugFormat("result:{0}", result.Length);
            Logger.Debug("<<<<= FtpClient::MemoryStreamToStringArray(MemoryStream, string[])");

            // 返却
            return result;
        }
        #endregion
    }
}
