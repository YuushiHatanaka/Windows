using log4net;
using log4net.Repository.Hierarchy;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// TcpClientLibraryクラス
    /// </summary>
    public class TcpClientLibrary : IDisposable
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Disposeフラグ
        /// <summary>
        /// Disposeフラグ
        /// </summary>
        protected bool m_Disposed = false;
        #endregion

        #region ホスト情報
        /// <summary>
        /// ホスト情報
        /// </summary>
        protected HostInfo m_HostInfo = null;
        #endregion

        #region タイムアウト情報
        /// <summary>
        /// タイムアウト情報
        /// </summary>
        protected Timeout Timeout = new Timeout();
        #endregion

        #region CancellationTokenSourceオブジェクト
        /// <summary>
        /// CancellationTokenSourceオブジェクト
        /// </summary>
        protected CancellationTokenSource m_CancellationTokenSource = new CancellationTokenSource();
        #endregion

        #region 文字エンコーディング
        /// <summary>
        /// 文字エンコーディング
        /// </summary>
        public Encoding Encoding = Encoding.GetEncoding("SHIFT_JIS");
        #endregion

        #region event delegate
        /// <summary>
        /// 接続 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ConnectedEventHandler(object sender, TcpClientConnectedEventArgs e);

        /// <summary>
        /// 切断 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void DisonnectedEventHandler(object sender, TcpClientDisconnectedEventArgs e);

        /// <summary>
        /// 送信 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void SendEventHandler(object sender, TcpClientSendEventArgs e);

        /// <summary>
        /// 受信 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ReciveEventHandler(object sender, TcpClientReciveEventArgs e);

        /// <summary>
        /// キャンセル event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void CanceledEventHandler(object sender, TcpClientCanceledEventArgs e);
        #endregion

        #region event
        /// <summary>
        /// 接続 event
        /// </summary>
        public event ConnectedEventHandler OnConnected = delegate { };

        /// <summary>
        /// 切断 event
        /// </summary>
        public event DisonnectedEventHandler OnDisconnected = delegate { };

        /// <summary>
        /// 送信 event
        /// </summary>
        public SendEventHandler OnSend = delegate { };

        /// <summary>
        /// 受信 event
        /// </summary>
        public ReciveEventHandler OnRecive = delegate { };

        /// <summary>
        /// キャンセル event
        /// </summary>
        public CanceledEventHandler OnCanceled = delegate { };
        #endregion

        #region 通知イベント
        /// <summary>
        /// キャンセル完了通知イベント
        /// </summary>
        public ManualResetEvent OnCancelCompletedNotify = new ManualResetEvent(false);
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public TcpClientLibrary(string host, int port)
        {
            // ロギング
            Logger.Debug("=>>>> TcpClientLibrary::TcpClientLibrary(string, int)");
            Logger.DebugFormat("host:{0}", host);
            Logger.DebugFormat("port:{0}", port);

            // 初期化
            Initialization(host, port);

            // ロギング
            Logger.Debug("<<<<= TcpClientLibrary::TcpClientLibrary(string, int)");
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~TcpClientLibrary()
        {
            // ロギング
            Logger.Debug("=>>>> TcpClientLibrary::~TcpClientLibrary()");

            // リソース破棄
            Dispose(false);

            // ロギング
            Logger.Debug("<<<<= TcpClientLibrary::~TcpClientLibrary()");
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // ロギング
            Logger.Debug("=>>>> TcpClientLibrary::Dispose()");

            // リソース破棄
            Dispose(true);

            // ガベージコレクション
            GC.SuppressFinalize(this);

            // ロギング
            Logger.Debug("<<<<= TcpClientLibrary::Dispose()");
        }


        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            // ロギング
            Logger.Debug("=>>>> TcpClientLibrary::Dispose(bool)");

            if (!m_Disposed)
            {
                if (disposing)
                {
                    // TODO: Dispose managed resources here.
                    m_CancellationTokenSource?.Dispose();
                }

                // TODO: Free unmanaged resources here.

                // Note disposing has been done.
                m_Disposed = true;
            }

            // ロギング
            Logger.Debug("<<<<= TcpClientLibrary::Dispose(bool)");
        }
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        protected virtual void Initialization(string host, int port)
        {
            // ロギング
            Logger.Debug("=>>>> TcpClientLibrary::Initialization(string, int)");

            // 初期設定
            m_HostInfo = new HostInfo(host, port);

            // ロギング
            Logger.Debug("<<<<= TcpClientLibrary::Initialization(string, int)");
        }
        #endregion

        #region キャンセル
        #region キャンセル(非同期)
        /// <summary>
        /// キャンセル(非同期)
        /// </summary>
        public async Task AsyncCancel()
        {
            // ロギング
            Logger.Debug("=>>>> TcpClientLibrary::AsyncCancel()");

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(Timeout.Cancel);

            // イベントパラメータ作成
            TcpClientCanceledEventArgs eventArgs = new TcpClientCanceledEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // キャンセル
                    Cancel();
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TcpClientException("キャンセルに失敗しました", ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TcpClientException("キャンセルに失敗しました", ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TcpClientException("キャンセルに失敗しました", ex);
            }
            finally
            {
                // イベント
                OnCanceled(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncCancel()");
            }
        }
        #endregion

        #region キャンセル(同期)
        /// <summary>
        /// キャンセル(同期)
        /// </summary>
        public virtual void Cancel()
        {
            // ロギング
            Logger.Debug("=>>>> TcpClientLibrary::Cancel()");

            // キャンセル通知
            m_CancellationTokenSource.Cancel(true);

            // ロギング
            Logger.Debug("<<<<= TcpClientLibrary::Cancel()");
        }
        #endregion
        #endregion

        #region リセット
        /// <summary>
        /// リセット
        /// </summary>
        /// <remarks>Cancel後に本オブジェクトを再利用する場合に必ず呼び出すこと</remarks>
        public virtual void Reset()
        {
            // ロギング
            Logger.Debug("=>>>> TcpClientLibrary::Reset()");

            // 破棄
            m_CancellationTokenSource?.Dispose();
            m_CancellationTokenSource = null;

            // 再生成
            m_CancellationTokenSource = new CancellationTokenSource();

            // ロギング
            Logger.Debug("<<<<= TcpClientLibrary::Reset()");
        }
        #endregion

        #region 接続
        #region 接続(非同期)
        /// <summary>
        /// 接続(非同期)
        /// </summary>
        public async Task AsyncConnect()
        {
            // ロギング
            Logger.Debug("=>>>> TcpClientLibrary::AsyncConnect()");

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(Timeout.Connect);

            // イベントパラメータ作成
            TcpClientConnectedEventArgs eventArgs = new TcpClientConnectedEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // 接続
                    if (!Connect())
                    {
                        // 結果設定
                        eventArgs.Result = false;
                    }
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TcpClientException("接続に失敗しました", ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TcpClientException("接続に失敗しました", ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TcpClientException("接続に失敗しました", ex);
            }
            finally
            {
                // イベント
                OnConnected(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncConnect()");
            }
        }
        #endregion

        #region 接続(同期)
        /// <summary>
        /// 接続(同期)
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public virtual bool Connect()
        {
            // 例外(未実装)
            throw new NotImplementedException();
        }
        #endregion
        #endregion

        #region 切断
        #region 切断(非同期)
        /// <summary>
        /// 切断(非同期)
        /// </summary>
        public async Task AsyncDisconnect()
        {
            // ロギング
            Logger.Debug("=>>>> TcpClientLibrary::AsyncDisconnect()");

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(Timeout.Disconnect);

            // イベントパラメータ作成
            TcpClientDisconnectedEventArgs eventArgs = new TcpClientDisconnectedEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // 切断
                    if (!Disconnect())
                    {
                        // 結果設定
                        eventArgs.Result = false;
                    }
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TcpClientException("切断に失敗しました", ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TcpClientException("切断に失敗しました", ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TcpClientException("切断に失敗しました", ex);
            }
            finally
            {
                // イベント
                OnDisconnected(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncDisconnect()");
            }
        }
        #endregion

        #region 切断(同期)
        /// <summary>
        /// 切断(同期)
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public virtual bool Disconnect()
        {
            // 例外(未実装)
            throw new NotImplementedException();
        }
        #endregion
        #endregion

        #region 読込
        #region 読込(非同期)
        /// <summary>
        /// 読込(非同期)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TcpClientException"></exception>
        public async Task AsyncRead()
        {
            // ロギング
            Logger.Debug("=>>>> TcpClientLibrary::AsyncRead()");

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(Timeout.Recive);

            // イベントパラメータ作成
            TcpClientReciveEventArgs eventArgs = new TcpClientReciveEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // 結果登録
                    eventArgs.Strings.Append(Read());

                    // 読込結果判定
                    if (eventArgs.Strings.Length == 0)
                    {
                        // 結果設定
                        eventArgs.Result = false;
                    }
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TcpClientException("読込に失敗しました", ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TcpClientException("読込に失敗しました", ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TcpClientException("読込に失敗しました", ex);
            }
            finally
            {
                // イベント
                OnRecive(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncRead()");
            }
        }
        #endregion

        #region 読込(同期)
        /// <summary>
        /// 読込(同期)
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public virtual string Read()
        {
            // 例外(未実装)
            throw new NotImplementedException();
        }
        #endregion
        #endregion

        #region 書込
        #region 書込(非同期)
        /// <summary>
        /// 書込(非同期)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="TcpClientException"></exception>
        public async Task AsyncWrite(string text)
        {
            // ロギング
            Logger.Debug("=>>>> TcpClientLibrary::AsyncWrite(string)");
            Logger.DebugFormat("text:{0}", text);

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(Timeout.Recive);

            // イベントパラメータ作成
            TcpClientSendEventArgs eventArgs = new TcpClientSendEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // 書込
                    if (!Write(text))
                    {
                        // 結果設定
                        eventArgs.Result = false;
                    }
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TcpClientException("書込に失敗しました", ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TcpClientException("書込に失敗しました", ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TcpClientException("書込に失敗しました", ex);
            }
            finally
            {
                // イベント
                OnSend(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncWrite(string)");
            }
        }
        #endregion

        #region 書込(同期)
        /// <summary>
        /// 書込(同期)
        /// </summary>
        /// <param name="text"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual bool Write(string text)
        {
            // 例外(未実装)
            throw new NotImplementedException();
        }
        #endregion

        #region 書込(非同期)
        /// <summary>
        /// 改行付与書込(非同期)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="TcpClientException"></exception>
        public async Task AsyncWriteLine(string text)
        {
            // ロギング
            Logger.Debug("=>>>> TcpClientLibrary::AsyncWriteLine(string)");
            Logger.DebugFormat("text:{0}", text);

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(Timeout.Recive);

            // イベントパラメータ作成
            TcpClientSendEventArgs eventArgs = new TcpClientSendEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // 書込
                    if (!WriteLine(text))
                    {
                        // 結果設定
                        eventArgs.Result = false;
                    }
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TcpClientException("書込に失敗しました", ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TcpClientException("書込に失敗しました", ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TcpClientException("書込に失敗しました", ex);
            }
            finally
            {
                // イベント
                OnSend(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncWriteLine(string)");
            }
        }
        #endregion

        #region 改行付与書込(同期)
        /// <summary>
        /// 改行付与書込(同期)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public virtual bool WriteLine(string str)
        {
            // 例外(未実装)
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}
