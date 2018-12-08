using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;

namespace Common.Net
{
    /// <summary>
    /// FTPクライアントクラス
    /// </summary>
    public class FtpClient : IDisposable
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

        /// <summary>
        /// 受信バッファサイズ
        /// </summary>
        private int m_ReciveBufferCapacity = 4096;

        #region エンコーダー
        /// <summary>
        /// エンコーダー(受信)
        /// </summary>
        private Encoding m_ReciveEncoding = Encoding.Default;

        /// <summary>
        /// エンコーダー(送信)
        /// </summary>
        private Encoding m_SendEncoding = Encoding.Default;
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
        /// 転送タイムアウト
        /// </summary>
        private int m_DownLoadMillisecondsTimeout = 10000;
        #endregion

        #region 通知イベント
        /// <summary>
        /// 接続完了通知イベント
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
        /// コマンド応答完了通知イベント
        /// </summary>
        public ManualResetEvent OnCommandResponceNotify = new ManualResetEvent(false);

        /// <summary>
        /// 切断完了通知イベント
        /// </summary>
        public ManualResetEvent OnDisconnectNotify = new ManualResetEvent(false);

        /// <summary>
        /// DownLoad完了通知イベント
        /// </summary>
        public ManualResetEvent OnDownLoadNotify = new ManualResetEvent(false);
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="hostName"></param>
        public FtpClient(string hostName)
            : this(hostName, 21)
        {
            Trace.WriteLine("FtpClient::FtpClient(string)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="port"></param>
        public FtpClient(string hostName, int port)
        {
            Trace.WriteLine("FtpClient::FtpClient(string, int)");

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
        ~FtpClient()
        {
            Trace.WriteLine("FtpClient::~FtpClient()");

            // 破棄
            this.Dispose(false);
        }
        #endregion

        #region 破棄
        /// <summary>
        /// 破棄フラグ
        /// </summary>
        private bool m_Disposed = false;

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            Trace.WriteLine("FtpClient::Dispose()");

            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 破棄
        /// </summary>
        /// <param name="isDisposing"></param>
        protected virtual void Dispose(bool isDisposing)
        {
            Trace.WriteLine("FtpClient::Dispose(bool)");

            // 破棄しているか？
            if (!this.m_Disposed)
            {
                // TODO:未実装(アンマネージドリソース解放)

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

        #region IPアドレス取得
        /// <summary>
        /// IPアドレス取得
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name=""></param>
        /// <returns></returns>
        private string GetIPAddress(string hostName, AddressFamily addressFamily)
        {
            Trace.WriteLine("FtpClient::GetIPAddress(string, AddressFamily)");

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

        #region MemoryStreamから文字列配列に変換
        /// <summary>
        /// MemoryStreamから文字列配列に変換
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <param name="separetor"></param>
        /// <returns></returns>
        private string[] MemoryStreamToStringArray(MemoryStream memoryStream, string[] separetor)
        {
            Trace.WriteLine("FtpClient::MemoryStreamToStringArray(MemoryStream, string[])");

            // MemoryStreamを文字列化(改行を含む1行：但し、最終改行は削除)
            string _GetString = this.m_ReciveEncoding.GetString(memoryStream.ToArray()).TrimEnd('\r', '\n');

            string[] _List = _GetString.Split(separetor, StringSplitOptions.None);
#if DEBUG
            foreach (string list in _List)
            {
                Debug.WriteLine(list);
            }
#endif
            return _List;
        }
        #endregion

        #region パッシブモード初期化
        /// <summary>
        /// パッシブモード初期化
        /// </summary>
        /// <param name="ipAddressString"></param>
        /// <param name="port"></param>
        private FtpClientDataConnection InitPassiveMode(string ipAddressString, int port)
        {
            Trace.WriteLine("FtpClient::InitPassiveMode(string, int)");

            // データコネクションを設定
            Debug.WriteLine("パッシブモード：[{0}:{1}]", ipAddressString, port);
            FtpClientDataConnection _FtpClientDataConnection = new FtpClientDataConnection()
            {
                Mode = FtpTransferMode.Passive,
                IpAddress = ipAddressString,
                Port = port,
                ReciveSize = this.m_ReciveBufferCapacity,
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            };
            return _FtpClientDataConnection;
        }
        #endregion

        #region アクティブモード初期化
        /// <summary>
        /// アクティブモード初期化
        /// </summary>
        /// <param name="ipAddressString"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        private FtpClientDataConnection InitActiveMode(string ipAddressString, int port)
        {
            Trace.WriteLine("FtpClient::InitActiveMode(string, int)");

            // データコネクションを設定
            Debug.WriteLine("アクティブモード：[{0}:{1}]", ipAddressString, port);
            FtpClientDataConnection _FtpClientDataConnection = new FtpClientDataConnection()
            {
                Mode = FtpTransferMode.Active,
                IpAddress = ipAddressString,
                Port = port,
                ReciveSize = this.m_ReciveBufferCapacity,
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            };
            return _FtpClientDataConnection;
        }
        #endregion

        #region 接続
        /// <summary>
        /// 接続
        /// </summary>
        public void Connect()
        {
            Trace.WriteLine("FtpClient::Connect()");

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
                throw new FtpClientException("接続タイムアウト");
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
            Trace.WriteLine("FtpClient::OnConnectCallBack(IAsyncResult)");

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

                // コマンド受信応答待ち
                FtpCommandResponseData _FtpCommandResponseData = this.CommandResponseWaiting((Socket)asyncResult.AsyncState);

                // 接続結果判定
                if (_FtpCommandResponseData.Response.StatusCode != 220)
                {
                    // 接続完了通知リセット
                    this.OnConnectNotify.Reset();
                    Debug.WriteLine("接続に失敗しました：" + _FtpCommandResponseData.Response.ToString());
                    return;
                }

                // 接続完了通知を設定
                this.OnConnectNotify.Set();
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
            Trace.WriteLine("FtpClient::DisConnect()");

            // 切断判定
            if (this.m_Socket != null && this.m_Socket.Connected)
            {
                // 切断
                this.m_Socket?.Disconnect(false);

                // 破棄
                this.m_Socket?.Dispose();
            }

            // ソケット初期化
            this.m_Socket = null;

            // 切断通知
            this.OnDisconnectNotify.Set();

            // 切断通知リセット
            this.OnDisconnectNotify.Reset();
        }
        #endregion

        #region 送信
        /// <summary>
        /// メッセージの送信(非同期処理)
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            Trace.WriteLine("FtpClient::Send(string)");

            // ソケット判定
            if (this.m_Socket == null || !this.m_Socket.Connected)
            {
                // 既にソケットが切断されている
                Debug.WriteLine("既にソケットが切断されています");
                return;
            }

            FtpClientSendData _FtpClientSendData = new FtpClientSendData();
            _FtpClientSendData.Buffer = this.m_SendEncoding.GetBytes(message);
            _FtpClientSendData.Socket = this.m_Socket;

            // 送信開始
            this.m_Socket.BeginSend(_FtpClientSendData.Buffer, 0, _FtpClientSendData.Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnSendCallBack), _FtpClientSendData);

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
            Trace.WriteLine("FtpClient::OnSendCallBack(IAsyncResult)");

            // オブジェクト変換
            FtpClientSendData _FtpClientSendData = (FtpClientSendData)asyncResult.AsyncState;

            // ソケットが切断されているか？
            if (_FtpClientSendData.Socket == null || !_FtpClientSendData.Socket.Connected)
            {
                Debug.WriteLine("既にソケットが切断されています");
                return;
            }

            // 送信完了
            int bytesSent = _FtpClientSendData.Socket.EndSend(asyncResult);
            Debug.WriteLine("送信完了サイズ：[{0}]", bytesSent);

            // 送信完了通知
            this.OnSendNotify.Set();
        }

        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="dataConnection"></param>
        /// <param name="stream"></param>
        public void Send(FtpClientDataConnection dataConnection, MemoryStream stream)
        {
            Trace.WriteLine("FtpClient::Send(MemoryStream)");

            // ソケット判定
            if (dataConnection.Socket == null || !dataConnection.Socket.Connected)
            {
                // 既にソケットが切断されている
                Debug.WriteLine("既にソケットが切断されています");
                return;
            }

            FtpClientSendStream _FtpClientSendStream = new FtpClientSendStream();
            _FtpClientSendStream.Stream = stream;
            _FtpClientSendStream.Socket = dataConnection.Socket;

            // 送信開始
            dataConnection.Socket.BeginSend(stream.ToArray(), 0, stream.ToArray().Length, SocketFlags.None, new AsyncCallback(this.OnSendStreamCallBack), _FtpClientSendStream);

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
        private void OnSendStreamCallBack(IAsyncResult asyncResult)
        {
            Trace.WriteLine("FtpClient::OnSendStreamCallBack(IAsyncResult)");

            // オブジェクト変換
            FtpClientSendStream _FtpClientSendStream = (FtpClientSendStream)asyncResult.AsyncState;

            // ソケットが切断されているか？
            if (_FtpClientSendStream.Socket == null || !_FtpClientSendStream.Socket.Connected)
            {
                Debug.WriteLine("既にソケットが切断されています");
                return;
            }

            // 送信完了
            int bytesSent = _FtpClientSendStream.Socket.EndSend(asyncResult);
            Debug.WriteLine("送信完了サイズ：[{0}]", bytesSent);

            // 送信完了通知
            this.OnSendNotify.Set();
        }
        #endregion

        #region コマンド応答
        /// <summary>
        /// コマンド応答待ち
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private FtpCommandResponseData CommandResponseWaiting(Socket socket)
        {
            Trace.WriteLine("FtpClient::CommandResponseWaiting(Socket)");

            // コマンド応答データオブジェクト生成
            FtpCommandResponseData _FtpCommandResponseData = new FtpCommandResponseData();
            _FtpCommandResponseData.Socket = socket;
            _FtpCommandResponseData.Buffer = new byte[1];
            _FtpCommandResponseData.Socket.BeginReceive(_FtpCommandResponseData.Buffer, 0, 1, SocketFlags.None, this.OnReciveCommandCallBack, _FtpCommandResponseData);

            // データ受信待ち
            if (!this.OnCommandResponceNotify.WaitOne(this.m_ReciveMillisecondsTimeout))
            {
                // コマンド応答完了通知リセット
                this.OnCommandResponceNotify.Reset();

                // 例外
                throw new FtpClientException("コマンド応答受信タイムアウト");
            }

            // コマンド応答完了通知リセット
            this.OnCommandResponceNotify.Reset();

            // コマンド応答を返却
            return _FtpCommandResponseData;
        }

        /// <summary>
        /// コマンド応答受信コールバック
        /// </summary>
        /// <param name="asyncResult"></param>
        private void OnReciveCommandCallBack(IAsyncResult asyncResult)
        {
            //Trace.WriteLine("FtpClient::OnReciveCommandCallBack(IAsyncResult)");

            // オブジェクト変換
            FtpCommandResponseData _FtpCommandResponseData = (FtpCommandResponseData)asyncResult.AsyncState;

            // ソケットが切断されているか？
            if (_FtpCommandResponseData.Socket == null || !_FtpCommandResponseData.Socket.Connected)
            {
                Debug.WriteLine("既にソケットが切断されています");
                return;
            }

            // 受信終了
            int _EndReceive = _FtpCommandResponseData.Socket.EndReceive(asyncResult);

            // 受信されているか？
            if (_EndReceive > 0)
            {
                // コマンド受信は1文字のみ認める
                if (_EndReceive != 1)
                {
                    // 例外
                    throw new FtpClientException("コマンド応答受信サイズ異常:[" + _EndReceive + "]");
                }

                // バッファ内に保持する
                _FtpCommandResponseData.ResponseData.WriteByte(_FtpCommandResponseData.Buffer[0]);

                // 改行を受信していない場合
                if (_FtpCommandResponseData.Buffer[0] != '\n')
                {
                    // 受信再開
                    _FtpCommandResponseData.Socket.BeginReceive(_FtpCommandResponseData.Buffer, 0, 1, SocketFlags.None, this.OnReciveCommandCallBack, _FtpCommandResponseData);
                }
                else
                {
                    // コマンド応答結果変換
                    _FtpCommandResponseData.Response = this.ResponseParse(_FtpCommandResponseData.ResponseData.ToArray());

                    // コマンド応答受信設定
                    this.OnCommandResponceNotify.Set();
                }
            }
            else
            {
                // コマンド応答受信設定
                this.OnCommandResponceNotify.Set();
            }
        }

        /// <summary>
        /// 応答解析
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private FtpResponse ResponseParse(byte[] response)
        {
            Trace.WriteLine("FtpClient::ResponseParse(byte[])");

            StringBuilder _ReciveStringBuilder = new StringBuilder();
            _ReciveStringBuilder.Append(this.m_ReciveEncoding.GetString(response).TrimEnd());
            return this.ResponseParse(_ReciveStringBuilder);
        }

        /// <summary>
        /// 応答解析
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private FtpResponse ResponseParse(StringBuilder response)
        {
            Trace.WriteLine("FtpClient::ResponseParse(StringBuilder)");

            FtpResponse _FtpResponse = new FtpResponse();

            Regex _Regex = new Regex("^(?<code>[1-6][0-9][0-9])[ ]*(?<detail>.*)", RegexOptions.Singleline);
            for (Match _Match = _Regex.Match(response.ToString()); _Match.Success; _Match = _Match.NextMatch())
            {
                _FtpResponse.StatusCode = int.Parse(_Match.Groups["code"].Value);
                _FtpResponse.StatusDetail = _Match.Groups["detail"].Value;
            }

            // コマンド応答を返却
            Debug.WriteLine("コマンド応答：" + _FtpResponse.ToString());
            return _FtpResponse;
        }
        #endregion

        #region アップロード
        /// <summary>
        /// アップロード
        /// </summary>
        /// <param name="dataConnection"></param>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        private bool Upload(FtpClientDataConnection dataConnection, MemoryStream memoryStream)
        {
            Trace.WriteLine("FtpClient::Upload(FtpClientDataConnection, MemoryStream)");

            // 転送モードによる
            if (dataConnection.Mode == FtpTransferMode.Active)
            {
                // アクティブモード転送
                return this.ActiveUpload(dataConnection, memoryStream);
            }
            else
            {
                // パッシブモード転送
                return this.PassiveUpload(dataConnection, memoryStream);
            }
        }

        /// <summary>
        /// アクティブモード転送(ローカル⇒サーバ)
        /// </summary>
        /// <param name="dataConnection"></param>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        private bool ActiveUpload(FtpClientDataConnection dataConnection, MemoryStream memoryStream)
        {
            Trace.WriteLine("FtpClient::ActiveUpload(FtpClientDataConnection, MemoryStream)");

            // TODO:未実装
            return false;
        }

        /// <summary>
        /// パッシブモード転送(ローカル⇒サーバ)
        /// </summary>
        /// <param name="dataConnection"></param>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        private bool PassiveUpload(FtpClientDataConnection dataConnection, MemoryStream memoryStream)
        {
            Trace.WriteLine("FtpClient::PassiveUpload(FtpClientDataConnection, MemoryStream)");

            // データ送信
            this.Send(dataConnection, memoryStream);

            // 正常終了
            return true;
        }
        #endregion

        #region ダウンロード
        /// <summary>
        /// ダウンロード
        /// </summary>
        /// <param name="dataConnection"></param>
        /// <returns></returns>
        private MemoryStream Download(FtpClientDataConnection dataConnection)
        {
            Trace.WriteLine("FtpClient::Download()");

            // 転送モードによる
            if (dataConnection.Mode == FtpTransferMode.Active)
            {
                // アクティブモード転送
                return this.ActiveDownload(dataConnection);
            }
            else
            {
                // パッシブモード転送
                return this.PassiveDownload(dataConnection);
            }
        }

        /// <summary>
        /// ダウンロード(アクティブモード転送)
        /// </summary>
        /// <param name="dataConnection"></param>
        /// <returns></returns>
        private MemoryStream ActiveDownload(FtpClientDataConnection dataConnection)
        {
            Trace.WriteLine("FtpClient::ActiveDownload(FtpClientDataConnection)");

            // TODO:未実装
            return null;
        }

        /// <summary>
        /// ダウンロード(パッシブモード転送)
        /// </summary>
        /// <param name="dataConnection"></param>
        /// <returns></returns>
        private MemoryStream PassiveDownload(FtpClientDataConnection dataConnection)
        {
            Trace.WriteLine("FtpClient::PassiveDownload(FtpClientDataConnection)");
            Debug.Write(dataConnection.ToString());

            // 結果受信待ち
            FtpClientReciveData _FtpClientReciveData = new FtpClientReciveData();
            _FtpClientReciveData.Socket = dataConnection.Socket;
            _FtpClientReciveData.Buffer = new byte[dataConnection.ReciveSize];
            dataConnection.Socket.BeginReceive(_FtpClientReciveData.Buffer, 0, _FtpClientReciveData.Buffer.Length, SocketFlags.None, this.OnPassiveDownloadCallback, _FtpClientReciveData);

            // データ転送待ち
            if (!this.OnDownLoadNotify.WaitOne(this.m_DownLoadMillisecondsTimeout))
            {
                // 転送通知リセット
                this.OnDownLoadNotify.Reset();
                return null;
            }

            // 転送通知リセット
            this.OnDownLoadNotify.Reset();

            // MemoryStreamを返却
            return _FtpClientReciveData.Memory;
        }

        /// <summary>
        /// 非同期転送のコールバックメソッド(別スレッドで実行される)
        /// </summary>
        /// <param name="asyncResult"></param>
        private void OnPassiveDownloadCallback(IAsyncResult asyncResult)
        {
            Trace.WriteLine("FtpClient::OnPassiveDownloadCallback(IAsyncResult)");

            // オブジェクト変換
            FtpClientReciveData _FtpClientReciveData = (FtpClientReciveData)asyncResult.AsyncState;

            // ソケットが切断されているか？
            if (_FtpClientReciveData.Socket == null || !_FtpClientReciveData.Socket.Connected)
            {
                Debug.WriteLine("既にソケットが切断されています");
                return;
            }

            // 非同期読込が終了しているか？
            int bytesRead = _FtpClientReciveData.Socket.EndReceive(asyncResult);
            Debug.WriteLine("　読込データサイズ:[{0}]", bytesRead);
            if (bytesRead > 0)
            {
                // 残りがある場合にはデータ保持しつつ、再度受信待ちを実施
                _FtpClientReciveData.Memory.Write(_FtpClientReciveData.Buffer, 0, bytesRead);
                _FtpClientReciveData.Socket.BeginReceive(_FtpClientReciveData.Buffer, 0, _FtpClientReciveData.Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnPassiveDownloadCallback), _FtpClientReciveData);
            }
            else
            {
                // 転送通知設定
                this.OnDownLoadNotify.Set();
            }
        }
        #endregion

        #region ログイン・ログアウト
        /// <summary>
        /// ログイン
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        public void Login(string userName, string userPassword)
        {
            Trace.WriteLine("FtpClient::Login(string, string)");

            // ユーザ名送信
            this.USER(userName);

            // パスワード送信
            this.PASS(userPassword);
        }

        /// <summary>
        /// ログアウト
        /// </summary>
        public void Logout()
        {
            Trace.WriteLine("FtpClient::Logout()");

            // QUIT送信
            this.QUIT();
        }
        #endregion

        #region FTPコマンド
        /// <summary>
        /// 1つ上位のディレクトリ(親ディレクトリ)をカレントディレクトリとする
        /// </summary>
        public bool CDUP()
        {
            Trace.WriteLine("FtpClient::CDUP()");

            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("CDUP");

            // 応答判定
            if (_FtpResponse.StatusCode == 550)
            {
                // 異常終了
                return false;
            }
            else if (_FtpResponse.StatusCode != 250)
            {
                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }

            // 正常終了
            return true;
        }


        /// <summary>
        /// 指定したディレクトリをカレントディレクトリとする
        /// </summary>
        /// <param name="directoryName">ディレクトリ名</param>
        /// <returns>true:正常(250 Directory successfully changed.) false:異常(550 Failed to change directory.)</returns>
        /// <exception cref="FtpClientException"></exception>
        public bool CWD(string directoryName)
        {
            Trace.WriteLine("FtpClient::CWD(string)");

            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("CWD", directoryName);

            // 応答判定
            if (_FtpResponse.StatusCode == 550)
            {
                // 異常終了
                return false;
            }
            else if (_FtpResponse.StatusCode != 250)
            {
                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }

            // 正常終了
            return true;
        }

        /// <summary>
        /// 指定したファイルを削除する
        /// </summary>
        /// <param name="fileName"></param>
        public void DELE(string fileName)
        {
            Trace.WriteLine("FtpClient::DELE(string)");

            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("DELE", fileName);

            // 応答判定
            if (_FtpResponse.StatusCode != 250)
            {
                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }
        }

        /// <summary>
        /// 現在のワーキングディレクトリ内のファイル一覧を表示する
        /// </summary>
        /// <param name="dataConnection"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[] LIST(FtpClientDataConnection dataConnection, string path)
        {
            Trace.WriteLine("FtpClient::LIST(string)");
            Debug.Write(dataConnection.ToString());

            // ソケット接続
            dataConnection.Socket.Connect(dataConnection.IpAddress, dataConnection.Port);

            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("LIST", path);

            // 応答判定
            if (_FtpResponse.StatusCode != 150)
            {
                // ソケット切断
                dataConnection.Socket.Close();

                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }

            // 結果受信待ち
            FtpCommandResponseData _FtpCommandResponseData = this.CommandResponseWaiting(this.m_Socket);

            // 応答判定
            if (_FtpCommandResponseData.Response.StatusCode != 226)
            {
                // ソケット切断
                dataConnection.Socket.Close();

                // 例外送出
                throw new FtpClientException(_FtpCommandResponseData.Response);
            }

            // ファイル一覧受信
            MemoryStream _MemoryStream = this.Download(dataConnection);

            // ソケット切断
            dataConnection.Socket.Close();

            // リスト変換して返却
            return this.MemoryStreamToStringArray(_MemoryStream, new string[] { "\r\n" });
        }

        /// <summary>
        /// 指定したディレクトリを作成する
        /// </summary>
        /// <param name="directoryName"></param>
        /// <exception cref="FtpClientException"></exception>
        public bool MKD(string directoryName)
        {
            Trace.WriteLine("FtpClient::MKD(string)");

            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("MKD", directoryName);

            // 応答判定
            if (_FtpResponse.StatusCode == 550)
            {
                // ディレクトリ作成失敗
                Debug.WriteLine("ディレクトリ作成失敗：" + directoryName);
                return false;
            }
            else if (_FtpResponse.StatusCode != 257)
            {
                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }

            // 正常終了
            return true;
        }

        /// <summary>
        /// 現在のワーキングディレクトリ内のファイル一覧を表示する
        /// </summary>
        /// <param name="dataConnection"></param>
        /// <param name="fileName"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public string[] NLST(FtpClientDataConnection dataConnection, string fileName, params string[] option)
        {
            Trace.WriteLine("FtpClient::NLST(string, string[])");
            Debug.Write(dataConnection.ToString());

            // ソケット接続
            dataConnection.Socket.Connect(dataConnection.IpAddress, dataConnection.Port);

            // コマンドパラメータ設定
            List<string> _ParameterList = new List<string>(option);
            _ParameterList.Add(fileName);

            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("NLST", _ParameterList.ToArray());

            // 応答判定
            if (_FtpResponse.StatusCode != 150)
            {
                // ソケット切断
                dataConnection.Socket.Close();

                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }

            // 結果受信待ち
            FtpCommandResponseData _FtpCommandResponseData = this.CommandResponseWaiting(this.m_Socket);

            // 応答判定
            if (_FtpCommandResponseData.Response.StatusCode != 226)
            {
                // ソケット切断
                dataConnection.Socket.Close();

                // 例外送出
                throw new FtpClientException(_FtpCommandResponseData.Response);
            }

            // ファイル一覧受信
            MemoryStream _MemoryStream = this.Download(dataConnection);

            // ソケット切断
            dataConnection.Socket.Close();

            // リスト変換して返却
            return this.MemoryStreamToStringArray(_MemoryStream, new string[] { "\r\n" });
        }

        /// <summary>
        /// ユーザーのパスワードを指定する
        /// </summary>
        /// <param name="userPassword">ユーザーパスワード</param>
        /// <exception cref="FtpClientException"></exception>
        public void PASS(string userPassword)
        {
            Trace.WriteLine("FtpClient::PASS(string)");

            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("PASS", userPassword);

            // 応答判定
            if (_FtpResponse.StatusCode != 230)
            {
                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }
        }

        /// <summary>
        /// パッシブモードへの移行を指示する。
        /// </summary>
        /// <exception cref="FtpClientException"></exception>
        /// <returns></returns>
        public FtpClientDataConnection PASV()
        {
            Trace.WriteLine("FtpClient::PASV()");

            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("PASV");

            // 応答判定
            if (_FtpResponse.StatusCode != 227)
            {
                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }

            // 受信したIPアドレスとポートを抽出
            string _ipAddressString = string.Empty;
            string[] _portStrings = null;
            int _Port = 0;
            Regex _Regex = new Regex(@"\((?<address>[0-9]+,[0-9]+,[0-9]+,[0-9]+),(?<port>[0-9]+,[0-9]+)\)", RegexOptions.Singleline);
            for (Match _Match = _Regex.Match(_FtpResponse.StatusDetail.ToString()); _Match.Success; _Match = _Match.NextMatch())
            {
                _ipAddressString = _Match.Groups["address"].Value.ToString().Replace(',', '.');
                _portStrings = _Match.Groups["port"].Value.ToString().Split(new char[] { ',' });
            }

            // ポート設定
            if (_portStrings.Length != 2)
            {
                // 例外送出
                throw new FtpClientException("IPアドレス／ポート抽出異常");
            }
            _Port = ((int.Parse(_portStrings[0]) & 0x000000ff) << 8) + (int.Parse(_portStrings[1]) & 0x000000ff);

            // パッシブモード初期化
            return this.InitPassiveMode(_ipAddressString, _Port);
        }

        /// <summary>
        /// データコネクションで使用するIPアドレス（通常はクライアント）とポート番号を指示する
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public FtpClientDataConnection PORT(string ipaddress, int port)
        {
            Trace.WriteLine("FtpClient::PORT(string, string)");
            Debug.WriteLine("　ipaddress：" + ipaddress);
            Debug.WriteLine("　port     ：" + port);

            // IPアドレス変換
            string _IpAddress = ipaddress.Replace(".", ",");

            // ポート番号分解
            int[] _Port = new int[2] { (port & 0x0000ff00) >> 8, (port & 0x000000ff) };

            // パラメータ設定
            string _Parameter = string.Format("{0},{1},{2}", _IpAddress, _Port[0], _Port[1]);

            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("PORT", _Parameter);

            // 応答判定
            if (_FtpResponse.StatusCode != 200)
            {
                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }

            // アクディブモード初期化
            return this.InitActiveMode(ipaddress, port);
        }

        /// <summary>
        /// 現在のワーキングディレクトリを表示する
        /// </summary>
        /// <returns></returns>
        public string PWD()
        {
            Trace.WriteLine("FtpClient::PWD()");

            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("PWD");

            // 応答判定
            if (_FtpResponse.StatusCode != 257)
            {
                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }

            // カレントディレクトリ抽出
            string _Path = string.Empty;
            Regex _Regex = new Regex("\"(?<path>.*)\"", RegexOptions.Singleline);
            for (Match _Match = _Regex.Match(_FtpResponse.StatusDetail.ToString()); _Match.Success; _Match = _Match.NextMatch())
            {
                _Path = _Match.Groups["path"].Value.ToString().Replace("\"", "");
            }

            // 結果を返却する
            return _Path;
        }

        /// <summary>
        /// ログアウトする
        /// </summary>
        /// <exception cref="FtpClientException"></exception>
        public void QUIT()
        {
            Trace.WriteLine("FtpClient::QUIT()");

            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("QUIT");

            // 応答判定
            if (_FtpResponse.StatusCode != 221)
            {
                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }
        }

        /// <summary>
        /// 指定したファイルの内容をサーバから取得する
        /// </summary>
        /// <param name="dataConnection"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public MemoryStream RETR(FtpClientDataConnection dataConnection, string fileName)
        {
            Trace.WriteLine("FtpClient::RETR(FtpClientDataConnection, string)");
            Debug.Write(dataConnection.ToString());
            Debug.WriteLine("　fileName   :" + fileName);

            // ソケット接続
            dataConnection.Socket.Connect(dataConnection.IpAddress, dataConnection.Port);

            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("RETR", fileName);

            // 応答判定
            if (_FtpResponse.StatusCode != 150)
            {
                // ソケット切断
                dataConnection.Socket.Close();

                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }

            // 結果受信待ち
            FtpCommandResponseData _FtpCommandResponseData = this.CommandResponseWaiting(this.m_Socket);

            // 応答判定
            if (_FtpCommandResponseData.Response.StatusCode != 226)
            {
                // ソケット切断
                dataConnection.Socket.Close();

                // 例外送出
                throw new FtpClientException(_FtpCommandResponseData.Response);
            }

            // ファイル転送(ローカル⇒サーバ)
            MemoryStream _MemoryStream = this.Download(dataConnection);

            // 転送結果判定
            if (_MemoryStream == null)
            {
                // 例外送出
                throw new FtpClientException("ファイル転送に失敗しました：" + fileName);
            }

            // ソケット切断
            dataConnection.Socket.Close();

            // 受信結果を返却する
            return _MemoryStream;
        }

        /// <summary>
        /// 指定したディレクトリを削除する
        /// </summary>
        /// <param name="directoryName"></param>
        public bool RMD(string directoryName)
        {
            Trace.WriteLine("FtpClient::RMD(string)");

            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("RMD", directoryName);

            // 応答判定
            if (_FtpResponse.StatusCode == 550)
            {
                // ファイル削除失敗
                Debug.WriteLine("ファイル削除失敗：" + directoryName);
                return false;
            }
            else if (_FtpResponse.StatusCode != 250)
            {
                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }

            // 正常終了
            return true;
        }

        /// <summary>
        /// 指定したファイル名を変更する(変更元ファイル名の指定)。
        /// RNTOを続けて実行しなくてはならない
        /// </summary>
        /// <param name="fileName">変更元ファイル名</param>
        /// <returns></returns>
        public bool RNFR(string fileName)
        {
            Trace.WriteLine("FtpClient::RNFR(string)");

            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("RNFR", fileName);

            // 応答判定
            if (_FtpResponse.StatusCode == 550)
            {
                // ファイル削除失敗
                Debug.WriteLine("ファイル名変更失敗(RNFR)：" + fileName);
                return false;
            }
            else if (_FtpResponse.StatusCode != 350)
            {
                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }

            // 正常終了
            return true;
        }

        /// <summary>
        /// RNFRコマンドで指定したファイルを、指定したファイル名に変更する。
        /// </summary>
        /// <param name="fileName">変更先ファイル名</param>
        /// <returns></returns>
        public bool RNTO(string fileName)
        {
            Trace.WriteLine("FtpClient::RNTO(string)");

            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("RNTO", fileName);

            // 応答判定
            if (_FtpResponse.StatusCode == 550)
            {
                // ファイル削除失敗
                Debug.WriteLine("ファイル名変更失敗(RNTO)：" + fileName);
                return false;
            }
            else if (_FtpResponse.StatusCode != 250)
            {
                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }

            // 正常終了
            return true;
        }

        /// <summary>
        /// 任意のOSコマンドを実行する
        /// </summary>
        /// <param name="command"></param>
        public void SITE(string command)
        {
            Trace.WriteLine("FtpClient::SITE(string)");

            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("SITE", command);

            // 応答判定
            if (_FtpResponse.StatusCode != 200)
            {
                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }
        }

        /// <summary>
        /// 指定したファイル名で、サーバへ送信するデータでファイルを作成する。
        /// <remarks>同⼀名のファイルがすでにある場合には、上書きする</remarks>
        /// </summary>
        /// <param name="dataConnection"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool STOR(FtpClientDataConnection dataConnection, string fileName)
        {
            Trace.WriteLine("FtpClient::STOR(FtpClientDataConnection, string)");
            Debug.Write(dataConnection.ToString());
            Debug.WriteLine("　fileName   :" + fileName);

            // ソケット接続
            dataConnection.Socket.Connect(dataConnection.IpAddress, dataConnection.Port);

            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("STOR", Path.GetFileName(fileName));

            // 応答判定
            if (_FtpResponse.StatusCode != 150)
            {
                // ソケット切断
                dataConnection.Socket.Close();

                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }

            // ファイルをMemoryStreamに読み込む
            MemoryStream _MemoryStream = new MemoryStream();
            using (FileStream _FileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[_FileStream.Length];
                _FileStream.Read(bytes, 0, (int)_FileStream.Length);
                _MemoryStream.Write(bytes, 0, (int)_FileStream.Length);
                Debug.WriteLine("送信ファイルサイズ：[{0}]", _FileStream.Length);
            }

            // ファイル転送(サーバ⇒ローカル)
            bool result = this.Upload(dataConnection, _MemoryStream);

            // ソケット切断
            dataConnection.Socket.Close();

            // 結果受信待ち
            FtpCommandResponseData _FtpCommandResponseData = this.CommandResponseWaiting(this.m_Socket);

            // 応答判定
            if (_FtpCommandResponseData.Response.StatusCode != 226)
            {
                // 例外送出
                throw new FtpClientException(_FtpCommandResponseData.Response);
            }

            // 転送結果を返却
            return result;
        }

        /// <summary>
        /// 指定したファイル名で、サーバへ送信するデータでファイルを作成する。
        /// <remarks>すでに同一名のファイルがあった場合には、重ならないようなファイル名を自動的に付けて作成する</remarks>
        /// </summary>
        /// <param name="dataConnection"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool STOU(FtpClientDataConnection dataConnection, string fileName)
        {
            Trace.WriteLine("FtpClient::STOU(FtpClientDataConnection, string)");
            Debug.Write(dataConnection.ToString());
            Debug.WriteLine("　fileName   :" + fileName);

            // ソケット接続
            dataConnection.Socket.Connect(dataConnection.IpAddress, dataConnection.Port);

            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("STOU", Path.GetFileName(fileName));

            // 応答判定
            if (_FtpResponse.StatusCode != 150)
            {
                // ソケット切断
                dataConnection.Socket.Close();

                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }

            // ファイルをMemoryStreamに読み込む
            MemoryStream _MemoryStream = new MemoryStream();
            using (FileStream _FileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[_FileStream.Length];
                _FileStream.Read(bytes, 0, (int)_FileStream.Length);
                _MemoryStream.Write(bytes, 0, (int)_FileStream.Length);
                Debug.WriteLine("送信ファイルサイズ：[{0}]", _FileStream.Length);
            }

            // ファイル転送(サーバ⇒ローカル)
            bool result = this.Upload(dataConnection, _MemoryStream);

            // ソケット切断
            dataConnection.Socket.Close();

            // 結果受信待ち
            FtpCommandResponseData _FtpCommandResponseData = this.CommandResponseWaiting(this.m_Socket);

            // 応答判定
            if (_FtpCommandResponseData.Response.StatusCode != 226)
            {
                // 例外送出
                throw new FtpClientException(_FtpCommandResponseData.Response);
            }

            // 転送結果を返却
            return result;
        }


        /// <summary>
        /// システム名を表示する
        /// </summary>
        /// <returns></returns>
        public string SYST()
        {
            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("SYST");

            // 応答判定
            if (_FtpResponse.StatusCode == 215)
            {
                // 結果を返却
                return _FtpResponse.StatusDetail;
            }

            // 空文字を返却
            return string.Empty;
        }

        /// <summary>
        /// 指定したユーザー名でログインする
        /// </summary>
        /// <param name="userName">ユーザ名</param>
        /// <exception cref="FtpClientException"></exception>
        public void USER(string userName)
        {
            Trace.WriteLine("FtpClient::USER(string)");

            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("USER", userName);

            // 応答判定
            if (_FtpResponse.StatusCode != 331)
            {
                // 例外送出
                throw new FtpClientException(_FtpResponse.ToString());
            }
        }

        /// <summary>
        /// 転送するファイルのファイル構造をオプションで示す
        /// </summary>
        /// <param name="option">A：ASCII I：Image（バイナリ）</param>
        /// <exception cref="FtpClientException"></exception>
        public void TYPE(string option)
        {
            Trace.WriteLine("FtpClient::TYPE(string)");

            // コマンド送信
            FtpResponse _FtpResponse = this.CommandExec("TYPE", option);

            // 応答判定
            if (_FtpResponse.StatusCode != 200)
            {
                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }
        }
        #endregion

        #region FTPコマンド実行
        /// <summary>
        /// コマンド実行
        /// </summary>
        /// <param name="command"></param>
        public FtpResponse CommandExec(string command, params string[] parameter)
        {
            Trace.WriteLine("FtpClient::CommandExec(string, params string[])");

            // コマンド作成
            string _Command = this.MakeCommandString(command, parameter);

            // コマンド送信
            Debug.WriteLine("送信コマンド：[" + _Command + "]");
            this.Send(_Command + "\r\n");

            // ソケット判定
            if (this.m_Socket == null && this.m_Socket.Connected)
            {
                Debug.WriteLine("既にソケットが切断されています");
                return null;
            }

            // コマンド受信応答待ち
            FtpCommandResponseData _FtpCommandResponseData = this.CommandResponseWaiting(this.m_Socket);

            // コマンド応答
            return _FtpCommandResponseData.Response;
        }

        /// <summary>
        /// コマンド文字列作成
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private string MakeCommandString(string command, params string[] parameter)
        {
            Trace.WriteLine("FtpClient::MakeCommandString(string, params string[])");

            StringBuilder _CommandBuilder = new StringBuilder();
            _CommandBuilder.Append(command + " ");
            foreach (string param in parameter)
            {
                _CommandBuilder.Append(param + " ");
            }
            return _CommandBuilder.ToString().TrimEnd(new char[] { ' ' });
        }
        #endregion
    }
}