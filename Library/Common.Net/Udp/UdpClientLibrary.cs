using Common.Net.Udp;
using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// UdpClientLibraryクラス
    /// </summary>
    public class UdpClientLibrary : IDisposable
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

        #region event delegate
        /// <summary>
        /// 接続 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ConnectedEventHandler(object sender, UdpClientConnectedEventArgs e);

        /// <summary>
        /// 切断 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void DisonnectedEventHandler(object sender, UdpClientDisconnectedEventArgs e);

        /// <summary>
        /// 送信 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void SendEventHandler(object sender, UdpClientSendEventArgs e);

        /// <summary>
        /// 受信 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ReciveEventHandler(object sender, UdpClientReciveEventArgs e);
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
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public UdpClientLibrary(string host, int port)
        {
            // ロギング
            Logger.Debug("=>>>> UdpClientLibrary::UdpClientLibrary(string, int)");
            Logger.DebugFormat("host:{0}", host);
            Logger.DebugFormat("port:{0}", port);

            // 初期化
            Initialization(host, port);

            // ロギング
            Logger.Debug("<<<<= UdpClientLibrary::UdpClientLibrary(string, int)");
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~UdpClientLibrary()
        {
            // ロギング
            Logger.Debug("=>>>> UdpClientLibrary::~UdpClientLibrary()");

            // リソース破棄
            Dispose(false);

            // ロギング
            Logger.Debug("<<<<= UdpClientLibrary::~UdpClientLibrary()");
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // ロギング
            Logger.Debug("=>>>> UdpClientLibrary::Dispose()");

            // リソース破棄
            Dispose(true);

            // ガベージコレクション
            GC.SuppressFinalize(this);

            // ロギング
            Logger.Debug("<<<<= UdpClientLibrary::Dispose()");
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            // ロギング
            Logger.Debug("=>>>> UdpClientLibrary::Dispose(bool)");

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
            Logger.Debug("<<<<= UdpClientLibrary::Dispose(bool)");
        }
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        private void Initialization(string host, int port)
        {
            // ロギング
            Logger.Debug("=>>>> UdpClientLibrary::Initialization(string, int)");

            // 初期設定
            m_HostInfo = new HostInfo(host, port);

            // ロギング
            Logger.Debug("<<<<= UdpClientLibrary::Initialization(string, int)");
        }
        #endregion

        #region キャンセル
        /// <summary>
        /// キャンセル
        /// </summary>
        public virtual void Cancel()
        {
            // ロギング
            Logger.Debug("=>>>> UdpClientLibrary::Cancel()");

            // キャンセル通知
            m_CancellationTokenSource.Cancel(true);

            // ロギング
            Logger.Debug("<<<<= UdpClientLibrary::Cancel()");
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
            Logger.Debug("=>>>> UdpClientLibrary::AsyncConnect()");

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(Timeout.Connect);

            // イベントパラメータ作成
            UdpClientConnectedEventArgs eventArgs = new UdpClientConnectedEventArgs();
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
                throw new UdpClientException("接続に失敗しました", ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new UdpClientException("接続に失敗しました", ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new UdpClientException("接続に失敗しました", ex);
            }
            finally
            {
                // イベント
                OnConnected(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= UdpClientLibrary::AsyncConnect()");
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
            Logger.Debug("=>>>> UdpClientLibrary::AsyncDisconnect()");

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(Timeout.Disconnect);

            // イベントパラメータ作成
            UdpClientDisconnectedEventArgs eventArgs = new UdpClientDisconnectedEventArgs();
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
                throw new UdpClientException("切断に失敗しました", ex);
            }
            finally
            {
                // イベント
                OnDisconnected(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= UdpClientLibrary::AsyncDisconnect()");
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
    }
}
