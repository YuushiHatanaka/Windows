using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// IcmpClientLibraryクラス
    /// </summary>
    partial class IcmpClientLibrary
    {
        #region タイムアウト
        /// <summary>
        /// 実行
        /// </summary>
        public TimeSpan ExecuteTimeout { get; set; } = new TimeSpan(0, 0, 0, 10, 0);
        #endregion

        #region event delegate
        /// <summary>
        /// 応答 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ResponseEventHandler(object sender, IcmpClientResponseEventArgs e);

        /// <summary>
        /// 完了 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void CompletedEventHandler(object sender, IcmpClientCompletedEventArgs e);
        #endregion

        #region event
        /// <summary>
        /// 応答 event
        /// </summary>
        public event ResponseEventHandler OnResponse = delegate { };

        /// <summary>
        /// 応答 event
        /// </summary>
        public event CompletedEventHandler OnCompleted = delegate { };
        #endregion

        #region 送信(非同期)
        /// <summary>
        /// 送信(非同期)
        /// </summary>
        /// <param name="wait"></param>
        /// <returns></returns>
        /// <exception cref="NtpClientException"></exception>
        public async Task AsyncSend(int wait)
        {
            // ロギング
            Logger.Debug("=>>>> IcmpClientLibrary::AsyncSend(int)");
            Logger.DebugFormat("wait:{0}", wait);

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(ExecuteTimeout);

            // イベントパラメータ作成
            IcmpClientCompletedEventArgs eventArgs = new IcmpClientCompletedEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // 送信初期化
                    SendInitialization();

                    // IcmpClientResponseEventArgsオブジェクト生成
                    IcmpClientResponseEventArgs responseEventArgs = new IcmpClientResponseEventArgs();
                    responseEventArgs.IPAddress = m_HostInfo.IPAddress;
                    responseEventArgs.ToIpAddress = m_Statistics.ToIpAddress;
                    responseEventArgs.FromIpAddress = m_Statistics.FromIpAddress;

                    // 繰り返し
                    while (true)
                    {
                        // キャンセル判定
                        if (m_CancellationTokenSource.IsCancellationRequested)
                        {
                            // 繰り返し終了
                            break;
                        }

                        // 送信実行
                        responseEventArgs.PingReply = SendExec(m_HostInfo.IPAddress.ToString(), wait);

                        // イベント
                        OnResponse(this, responseEventArgs);
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
                throw new NtpClientException("送信に失敗しました", ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new NtpClientException("送信に失敗しました", ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new NtpClientException("送信に失敗しました", ex);
            }
            finally
            {
                // 結果登録
                eventArgs.Statistics = m_Statistics;

                // イベント
                OnCompleted(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= NtpClientLibrary::AsyncSend(int)");
            }
        }

        /// <summary>
        /// 送信(非同期)
        /// </summary>
        /// <param name="count"></param>
        /// <param name="wait"></param>
        /// <exception cref="IcmpClientException"></exception>
        public async Task AsyncSend(int count, int wait)
        {
            // ロギング
            Logger.Debug("=>>>> NtpClientLibrary::AsyncSend(int, int)");
            Logger.DebugFormat("count:{0}", count);
            Logger.DebugFormat("wait :{0}", wait);

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(ExecuteTimeout);

            // イベントパラメータ作成
            IcmpClientCompletedEventArgs eventArgs = new IcmpClientCompletedEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // 送信初期化
                    SendInitialization();

                    // IcmpClientResponseEventArgsオブジェクト生成
                    IcmpClientResponseEventArgs responseEventArgs = new IcmpClientResponseEventArgs();
                    responseEventArgs.IPAddress = m_HostInfo.IPAddress;
                    responseEventArgs.ToIpAddress = m_Statistics.ToIpAddress;
                    responseEventArgs.FromIpAddress = m_Statistics.FromIpAddress;

                    // 送信回数分繰り返す
                    for (int i = 0; i < count; i++)
                    {
                        // キャンセル判定
                        if (m_CancellationTokenSource.IsCancellationRequested)
                        {
                            // 繰り返し終了
                            break;
                        }

                        // 送信実行
                        responseEventArgs.PingReply = SendExec(m_HostInfo.IPAddress.ToString(), wait);

                        // イベント
                        OnResponse(this, responseEventArgs);
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
                throw new NtpClientException("送信に失敗しました", ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new NtpClientException("送信に失敗しました", ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new NtpClientException("送信に失敗しました", ex);
            }
            finally
            {
                // 結果登録
                eventArgs.Statistics = m_Statistics;

                // イベント
                OnCompleted(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= NtpClientLibrary::AsyncSend(int, int)");
            }
        }
        #endregion
    }
}
