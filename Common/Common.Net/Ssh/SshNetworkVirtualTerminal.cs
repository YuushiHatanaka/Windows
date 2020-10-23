using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Net
{
    #region イベントパラメータ
    /// <summary>
    /// 受信イベントパラメータ
    /// </summary>
    public class SshNetworkVirtualTerminalReadEventArgs : EventArgs
    {
        /// <summary>
        /// バッファ
        /// </summary>
        public StringBuilder StringBuilder = new StringBuilder();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SshNetworkVirtualTerminalReadEventArgs()
            : base()
        {
        }
    }

    /// <summary>
    /// 送信イベントパラメータ
    /// </summary>
    public class SshNetworkVirtualTerminalWriteEventArgs : EventArgs
    {
        /// <summary>
        /// バッファ
        /// </summary>
        public StringBuilder StringBuilder = new StringBuilder();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SshNetworkVirtualTerminalWriteEventArgs()
            : base()
        {
        }
    }

    /// <summary>
    /// 例外イベントパラメータ
    /// </summary>
    public class SshNetworkVirtualTerminalExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// 例外
        /// </summary>
        public Exception Exception = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SshNetworkVirtualTerminalExceptionEventArgs()
            : base()
        {
        }
    }
    #endregion

    #region イベントdelegate
    /// <summary>
    /// 受信イベントdelegate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SshNetworkVirtualTerminalReadEventHandler(object sender, SshNetworkVirtualTerminalReadEventArgs e);

    /// <summary>
    /// 送信イベントdelegate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SshNetworkVirtualTerminalWriteEventHandler(object sender, SshNetworkVirtualTerminalWriteEventArgs e);

    /// <summary>
    /// 例外イベントdelegate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SshNetworkVirtualTerminalExceptionEventHandler(object sender, SshNetworkVirtualTerminalExceptionEventArgs e);
    #endregion

    /// <summary>
    /// SSH仮想ネットワーク端末クラス
    /// </summary>
    public class SshNetworkVirtualTerminal : SshClientWrap
    {
        #region イベントハンドラ
        /// <summary>
        /// 読込イベントハンドラ
        /// </summary>
        public SshNetworkVirtualTerminalReadEventHandler OnRead;

        /// <summary>
        /// 書込イベントハンドラ
        /// </summary>
        public SshNetworkVirtualTerminalWriteEventHandler OnWrite;

        /// <summary>
        /// 例外イベントハンドラ
        /// </summary>
        public SshNetworkVirtualTerminalExceptionEventHandler OnException;
        #endregion

        #region 文字コード(Local)
        /// <summary>
        /// 文字コード(Local)
        /// </summary>
        private Encoding m_LocalEncoding;

        /// <summary>
        /// 文字コード(Local)
        /// </summary>
        public Encoding LocalEncoding { get { return this.m_LocalEncoding; } set { this.m_LocalEncoding = value; } }
        #endregion

        #region 文字コード(Remote)
        /// <summary>
        /// 文字コード(Remote)
        /// </summary>
        private Encoding m_RemoteEncoding;

        /// <summary>
        /// 文字コード(Remote)
        /// </summary>
        public Encoding RemoteEncoding { get { return this.m_RemoteEncoding; } set { this.m_RemoteEncoding = value; } }
        #endregion

        #region 改行コード(送信)
        /// <summary>
        /// 改行コード(送信)
        /// </summary>
        private string m_WriteNewLine = string.Empty;

        /// <summary>
        /// 改行コード(送信)
        /// </summary>
        public string WriteNewLine { get { return this.m_WriteNewLine; } set { this.m_WriteNewLine = value; } }
        #endregion

        #region 改行コード(受信)
        /// <summary>
        /// 改行コード(受信)
        /// </summary>
        private string m_ReadNewLine = string.Empty;

        /// <summary>
        /// 改行コード(受信)
        /// </summary>
        public string ReadNewLine { get { return this.m_ReadNewLine; } set { this.m_ReadNewLine = value; } }
        #endregion

        #region 受信タスク
        /// <summary>
        /// 受信タスクオブジェクト
        /// </summary>
        private Task m_ReciveTask = null;

        /// <summary>
        /// 受信タスクトークン
        /// </summary>
        private CancellationTokenSource m_CancellationTokenSource = null;
        #endregion

        /// <summary>
        /// 受信イベント通知
        /// </summary>
        private ManualResetEvent OnReadNotify = new ManualResetEvent(false);

        /// <summary>
        /// 受信バッファ
        /// </summary>
        private StringBuilder m_ReadString = new StringBuilder();

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SshNetworkVirtualTerminal()
            : base("localhost")
        {
            // 初期化
            this.Initialization();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        public SshNetworkVirtualTerminal(string host)
            : base(host)
        {
            // 初期化
            this.Initialization();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public SshNetworkVirtualTerminal(string host, int port)
            : base(host, port)
        {
            // 初期化
            this.Initialization();
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~SshNetworkVirtualTerminal()
        {
        }
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            // デフォルト設定
            this.m_LocalEncoding = Encoding.GetEncoding("UTF-8");
            this.m_RemoteEncoding = Encoding.GetEncoding("UTF-8");
            this.m_ReadNewLine = "\n";
            this.m_WriteNewLine = "\n";

            // イベントハンドラ設定
            this.OnConnected += this.ConnectedEvent;
            this.OnSend += this.SendEvent;
            this.OnRecive += this.ReciveEvent;
            this.OnDisconnected += this.DisonnectedEvent;
        }
        #endregion

        #region 受信タスク
        /// <summary>
        /// 受信タスク
        /// </summary>
        private void ReciveTask()
        {
            // 無限ループ
            while (true)
            {
                // Taskキャンセル判定
                if (this.m_CancellationTokenSource.IsCancellationRequested)
                {
                    // キャンセルされたらTaskを終了する.
                    break;
                }

                try
                {
                    // 受信
                    this.Recive();
                }
                catch (Exception ex)
                {
                    // イベント呼出し判定
                    if (this.OnException != null)
                    {
                        // イベントパラメータ生成
                        SshNetworkVirtualTerminalExceptionEventArgs eventArgs = new SshNetworkVirtualTerminalExceptionEventArgs()
                        {
                            Exception = ex
                        };

                        // イベント呼出し
                        this.OnException(this, eventArgs);
                    }
                }
            }

            // 切断
            this.DisConnect();
        }
        #endregion

        #region 非同期関連コールバックメソッド
        /// <summary>
        /// 非同期接続のコールバックメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectedEvent(object sender, SshClientConnectedEventArgs e)
        {
            // 非同期処理をCancelするためのTokenを取得.
            this.m_CancellationTokenSource = new CancellationTokenSource();

            // 非同期処理開始
            this.m_ReciveTask = Task.Factory.StartNew(() =>
            {
                // 受信タスク実行
                this.ReciveTask();
            }, this.m_CancellationTokenSource.Token);
        }

        /// <summary>
        /// 非同期切断のコールバックメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisonnectedEvent(object sender, SshClientDisconnectedEventArgs e)
        {
            // 受信タスクトークン破棄
            if (this.m_CancellationTokenSource != null)
            {
                this.m_CancellationTokenSource.Cancel();
            }

            try
            {
                // 受信タスク終了待ち
                if (this.m_ReciveTask != null)
                {
                    this.m_ReciveTask.Wait(this.Timeout.Disconnect, this.m_CancellationTokenSource.Token);
                }
            }
            catch (OperationCanceledException)
            {
                // ロギング
                this.Logger.WarnFormat("受信タスク終了：[OperationCanceledException]");
            }
            catch (AggregateException)
            {
                // ロギング
                this.Logger.WarnFormat("受信タスク終了：[AggregateException]");
            }

            // 受信タスクトークン破棄
            if (this.m_CancellationTokenSource != null)
            {
                this.m_CancellationTokenSource.Dispose();
                this.m_CancellationTokenSource = null;
            }
        }

        /// <summary>
        /// 非同期送信のコールバックメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendEvent(object sender, SshClientSendEventArgs e)
        {
            // 送信通知
            if (this.OnWrite != null)
            {
                // パラメータ生成
                SshNetworkVirtualTerminalWriteEventArgs args = new SshNetworkVirtualTerminalWriteEventArgs();
                args.StringBuilder.Append(this.LocalEncoding.GetString(e.Stream.ToArray()));

                // イベント呼出し
                this.OnWrite(this, args);
            }
        }

        /// <summary>
        /// 非同期受信のコールバックメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReciveEvent(object sender, SshClientReciveEventArgs e)
        {
            // 受信判定
            if (e.Size > 0)
            {
                // 受信データオブジェクト
                MemoryStream memoryStream = new MemoryStream();

                // MemorySteramのサイズ判定
                if (e.Stream.Length > 0)
                {
                    // ロギング
                    this.Logger.Info(this.RemoteEncoding.GetString(memoryStream.ToArray()));

                    // 受信通知
                    if (this.OnRead != null)
                    {
                        // パラメータ生成
                        SshNetworkVirtualTerminalReadEventArgs args = new SshNetworkVirtualTerminalReadEventArgs();
                        args.StringBuilder.Append(this.RemoteEncoding.GetString(memoryStream.ToArray()));

                        // イベント呼出し
                        this.OnRead(this, args);
                    }
                }
            }
        }
        #endregion

        #region 書込
        /// <summary>
        /// 書込
        /// </summary>
        /// <param name="str"></param>
        public void Write(string str)
        {
            // MemoryStreamオブジェクト生成
            MemoryStream sendStream = new MemoryStream();

            // エンコード
            byte[] data = this.RemoteEncoding.GetBytes(str);

            // MemoryStreamオブジェクト書込
            sendStream.Write(data, 0, data.Length);

            // 送信
            this.Send(sendStream);
        }

        /// <summary>
        /// 書込
        /// </summary>
        /// <param name="str"></param>
        public void WriteLine(string str)
        {
            // 書込(文字列+改行)
            this.Write(str + this.m_WriteNewLine);
        }
        #endregion

        #region 読込
        /// <summary>
        /// 読込
        /// </summary>
        /// <returns></returns>
        public StringBuilder Read()
        {
            // 結果オブジェクト生成
            StringBuilder result = new StringBuilder();

            //　イベント登録
            this.OnRead += this.ReadEventHandler;

            // 受信イベント待ち
            if (!this.OnReadNotify.WaitOne())
            {
                // 通知解除
                this.OnReadNotify.Reset();

                // TODO:例外
                return null;
            }

            // 通知解除
            this.OnReadNotify.Reset();

            // 結果登録
            result.Append(this.m_ReadString);

            // クリア
            lock (this.m_ReadString)
            {
                this.m_ReadString.Length = 0;
                this.m_ReadString.Clear();
            }

            //　イベント解除
            this.OnRead -= this.ReadEventHandler;

            // 結果返却
            return result;
        }

        /// <summary>
        /// 読込
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public StringBuilder Read(string str)
        {
            // 結果オブジェクト生成
            StringBuilder result = new StringBuilder();

            // 文字列受信まで繰り返す
            while (true)
            {
                // 読込
                StringBuilder readString = this.Read();

                // 結果登録
                result.Append(readString.ToString());

                // 文字列比較
                Regex regex = new Regex(str, RegexOptions.Compiled | RegexOptions.Multiline);
                if (regex.IsMatch(readString.ToString()))
                {
                    break;
                }
            }

            // 結果返却
            return result;
        }

        /// <summary>
        /// 読込
        /// </summary>
        /// <param name="str"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public StringBuilder Read(string str, int timeout)
        {
            // 結果オブジェクト生成
            StringBuilder result = new StringBuilder();

            // Taskオブジェクト生成
            using (CancellationTokenSource source = new CancellationTokenSource())
            {
                // タイムアウト設定
                source.CancelAfter(timeout);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    // 読込
                    StringBuilder readString = this.Read(str);

                    // 結果登録
                    result.Append(readString.ToString());
                }, source.Token);

                try
                {
                    // タスク待ち
                    task.Wait(source.Token);
                    Console.WriteLine("＜タスク終了＞");
                    return result;
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("OperationCanceledExceptionが発生しました。");
                    return null;
                }
                catch (AggregateException)
                {
                    Console.WriteLine("AggregateExceptionが発生しました。");
                    return null;
                }
            }
        }

        /// <summary>
        /// 読込
        /// </summary>
        /// <returns></returns>
        public StringBuilder ReadLine()
        {
            // TODO:未実装
            return null;
        }

        /// <summary>
        /// 読込イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadEventHandler(object sender, SshNetworkVirtualTerminalReadEventArgs e)
        {
            // 受信文字列追加
            lock (this.m_ReadString)
            {
                // 受信文字列追加
                this.m_ReadString.Append(e.StringBuilder.ToString());

                // 受信通知
                this.OnReadNotify.Set();
            }
        }
        #endregion
    }
}
