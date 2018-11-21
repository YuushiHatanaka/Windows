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

namespace Common.Net
{
    public class FtpClientReciveData
    {
        public Socket Socket = null;
        public byte[] Buffer = null;
        public FtpResponse Response = new FtpResponse();
    }

    public class FtpClientSendData
    {
        public Socket Socket = null;
        public byte[] Buffer = null;
        public FtpResponse Response = new FtpResponse();
    }

    /// <summary>
    /// FTPクライアントクラス
    /// </summary>
    public class FtpClient : IDisposable
    {
        /// <summary>
        /// サーバエンドポイント
        /// </summary>
        public IPEndPoint m_IPEndPoint = null;

        /// <summary>
        /// ソケット
        /// </summary>
        private Socket m_Socket = null;

        private int m_ReciveBufferCapacity = 4096;

        /// <summary>
        /// エンコーダー
        /// </summary>
        private Encoding m_ReciveEncoding = Encoding.Default;
        private Encoding m_SendEncoding = Encoding.Default;

        /// <summary>
        /// 受信タイムアウト
        /// </summary>
        private int m_ReciveMillisecondsTimeout = 100;

        /// <summary>
        /// 送信タイムアウト
        /// </summary>
        private int m_SendMillisecondsTimeout = 100;

        /// <summary>
        /// コマンド応答タイムアウト
        /// </summary>
        private int m_CommandResponceMillisecondsTimeout = 1000;

        /// <summary>
        /// コマンド応答
        /// </summary>
        private FtpResponse m_FtpResponse = new FtpResponse();

        public ManualResetEvent OnConnectEvent = new ManualResetEvent(false);
        public ManualResetEvent OnReciveEvent = new ManualResetEvent(false);
        public ManualResetEvent OnSendEvent = new ManualResetEvent(false);
        public ManualResetEvent OnCommandResponceEvent = new ManualResetEvent(false);
        public ManualResetEvent OnDisconnectEvent = new ManualResetEvent(false);

        public FtpClient(string hostName)
        {
            IPAddress hostAddress = IPAddress.Parse(hostName);
            this.m_IPEndPoint = new IPEndPoint(hostAddress, 21);
        }

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~FtpClient()
        {
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

        #region 接続
        /// <summary>
        /// 接続
        /// </summary>
        public void Connect()
        {
            // ソケット生成
            this.m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 非同期で接続を待機
            this.m_Socket.BeginConnect(this.m_IPEndPoint, new AsyncCallback(this.OnConnectCallBack), this.m_Socket);
        }

        /// <summary>
        /// 非同期接続のコールバックメソッド(別スレッドで実行される)
        /// </summary>
        /// <param name="asyncResult"></param>
        private void OnConnectCallBack(IAsyncResult asyncResult)
        {
            Trace.WriteLine("FtpClient::OnConnectedCallBack");

            // 結果受信待ち
            FtpClientReciveData _FtpClientReciveData = new FtpClientReciveData();
            _FtpClientReciveData.Socket = (Socket)asyncResult.AsyncState;
            _FtpClientReciveData.Buffer = new byte[this.m_ReciveBufferCapacity];
            _FtpClientReciveData.Socket.BeginReceive(_FtpClientReciveData.Buffer, 0, _FtpClientReciveData.Buffer.Length, SocketFlags.None, this.OnConnectReceiveCallback, _FtpClientReciveData);
        }

        /// <summary>
        /// 非同期接続後の結果受信用コールバックメソッド(別スレッドで実行される)
        /// </summary>
        /// <param name="asyncResult"></param>
        private void OnConnectReceiveCallback(IAsyncResult asyncResult)
        {
            Trace.WriteLine("FtpClient::ReceiveCallback");

            FtpClientReciveData _FtpClientReciveData = (FtpClientReciveData)asyncResult.AsyncState;

            // 接続結果判定
            FtpResponse _FtpResponse = this.ResponseParse(_FtpClientReciveData.Buffer);
            if (_FtpResponse.StatusCode != 220)
            {
                // 接続完了設定せずに終了(待ち受け側はTimeOut)
                Debug.WriteLine("接続に失敗しました：" + _FtpResponse.ToString());
                return;
            }

            // 接続完了を設定
            this.OnConnectEvent.Set();

            // イベントリセット
            this.OnConnectEvent.Reset();
        }
        #endregion

        #region 切断
        /// <summary>
        // ソケット通信接続の切断
        /// </summary>
        public void DisConnect()
        {
            // 切断
            this.m_Socket?.Disconnect(false);

            // 破棄
            this.m_Socket?.Dispose();

            // 切断通知
            this.OnDisconnectEvent.Set();

            // イベントリセット
            this.OnDisconnectEvent.Reset();
        }
        #endregion

        #region 送信
        /// <summary>
        // メッセージの送信(非同期処理)
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            FtpClientSendData _FtpClientSendData = new FtpClientSendData();
            _FtpClientSendData.Buffer = this.m_SendEncoding.GetBytes(message);
            _FtpClientSendData.Socket = this.m_Socket;

            // 送信開始
            this.m_Socket.BeginSend(_FtpClientSendData.Buffer, 0, _FtpClientSendData.Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnSendCallBack), _FtpClientSendData);

            // 送信応答待ち
            if (!this.OnSendEvent.WaitOne(this.m_SendMillisecondsTimeout))
            {
                throw new FtpClientException("メッセージ送信T.O.");
            }
        }

        /// <summary>
        /// 非同期送信のコールバックメソッド(別スレッドで実行される)
        /// </summary>
        /// <param name="asyncResult"></param>
        private void OnSendCallBack(IAsyncResult asyncResult)
        {
            Trace.WriteLine("FtpClient::OnSendCallBack");

            // オブジェクト変換
            FtpClientSendData _FtpClientSendData = (FtpClientSendData)asyncResult.AsyncState;

            // 送信完了通知
            this.OnSendEvent.Set();

            // イベントリセット
            this.OnSendEvent.Reset();
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
            FtpResponse _FtpResponse = new FtpResponse();

            Regex reg = new Regex("^(?<code>[1-6][0-9][0-9])[ ]*(?<detail>.*)", RegexOptions.Singleline);
            for (Match m = reg.Match(response.ToString()); m.Success; m = m.NextMatch())
            {
                _FtpResponse.StatusCode = int.Parse(m.Groups["code"].Value);
                _FtpResponse.StatusDetail = m.Groups["detail"].Value;
            }

            return _FtpResponse;
        }
        #endregion

        /// <summary>
        /// ログイン
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        public void Login(string userName, string userPassword)
        {
            FtpResponse _FtpResponse = null;

            // ユーザ名送信
            _FtpResponse = this.CommandExec("USER", userName);

            // 応答判定
            if (_FtpResponse.StatusCode != 331)
            {
                // 例外送出
                throw new FtpClientException(_FtpResponse.ToString());
            }

            // パスワード送信
            _FtpResponse = this.CommandExec("PASS", userPassword);

            // 応答判定
            if (_FtpResponse.StatusCode != 230)
            {
                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }
        }

        /// <summary>
        /// ログアウト
        /// </summary>
        public void Logout()
        {
            // QUIT送信
            FtpResponse _FtpResponse = this.CommandExec("QUIT");

            // 応答判定
            if (_FtpResponse.StatusCode != 221)
            {
                // 例外送出
                throw new FtpClientException(_FtpResponse);
            }
        }

        /// <summary>
        /// コマンド実行
        /// </summary>
        /// <param name="command"></param>
        public FtpResponse CommandExec(string command, params string[] parameter)
        {
            // コマンド作成
            StringBuilder _CommandBuilder = new StringBuilder();
            _CommandBuilder.Append(command + " ");
            foreach (string param in parameter)
            {
                _CommandBuilder.Append(param + " ");
            }
            string _Command = _CommandBuilder.ToString().TrimEnd(new char[] { ' ' });

            // コマンド送信
            Debug.WriteLine("送信コマンド：[" + _Command + "]");
            this.Send(_Command + "\r\n");

            // 結果受信待ち
            FtpClientReciveData _FtpClientReciveData = new FtpClientReciveData();
            _FtpClientReciveData.Socket = this.m_Socket;
            _FtpClientReciveData.Buffer = new byte[this.m_ReciveBufferCapacity];
            _FtpClientReciveData.Socket.BeginReceive(_FtpClientReciveData.Buffer, 0, _FtpClientReciveData.Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnCommandResponceCallBack), _FtpClientReciveData);

            // コマンド応答待ち
            if (!this.OnCommandResponceEvent.WaitOne(this.m_CommandResponceMillisecondsTimeout))
            {
                throw new FtpClientException("コマンド応答T.O.");
            }

            // コマンド応答
            FtpResponse _FtpResponse = null;
            lock (this.m_FtpResponse)
            {
                _FtpResponse = this.m_FtpResponse;
                this.m_FtpResponse = new FtpResponse();
            }
            Debug.WriteLine("コマンド応答：" + _FtpResponse.ToString());
            return _FtpResponse;
        }

        /// <summary>
        /// コマンド実行結果応答コールバック
        /// </summary>
        /// <param name="asyncResult"></param>
        private void OnCommandResponceCallBack(IAsyncResult asyncResult)
        {
            Trace.WriteLine("FtpClient::OnCommandResponceCallBack");

            FtpClientReciveData _FtpClientReciveData = (FtpClientReciveData)asyncResult.AsyncState;

            lock (this.m_FtpResponse)
            {
                this.m_FtpResponse = this.ResponseParse(_FtpClientReciveData.Buffer);
                Debug.WriteLine("コマンド応答結果：" + this.m_FtpResponse.ToString());
            }

            // コマンド応答通知
            this.OnCommandResponceEvent.Set();

            // イベントリセット
            this.OnCommandResponceEvent.Reset();
        }
    }
}

