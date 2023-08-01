using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// NtpClientAsyncLibraryクラス
    /// </summary>
    partial class NtpClientLibrary
    {
        #region タイムアウト
        /// <summary>
        /// 実行
        /// </summary>
        public TimeSpan ExecuteTimeout { get; set; } = new TimeSpan(0, 0, 0, 30, 0);
        #endregion

        #region event delegate
        /// <summary>
        /// コマンド実行 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void CommandExecuteEventHandler(object sender, NtpClientCommandExecuteEventArgs e);
        #endregion

        #region event
        /// <summary>
        /// CommandExecute event
        /// </summary>
        public event CommandExecuteEventHandler OnCommandExecute = delegate { };
        #endregion

        #region 実行(非同期)
        /// <summary>
        /// 実行(非同期)
        /// </summary>
        /// <returns></returns>
        public async Task AsyncExcexute()
        {
            // ロギング
            Logger.Debug("=>>>> NtpClientLibrary::AsyncExcexute()");

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(ExecuteTimeout);

            // イベントパラメータ作成
            NtpClientCommandExecuteEventArgs eventArgs = new NtpClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // 実行
                    eventArgs.ExecuteResult = Excexute();

                    // 実行結果判定
                    if (eventArgs.ExecuteResult.PacketData.Length == 0)
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
                throw new NtpClientException("実行に失敗しました", ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new NtpClientException("実行に失敗しました", ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new NtpClientException("実行に失敗しました", ex);
            }
            finally
            {
                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= NtpClientLibrary::AsyncExcexute()");
            }
        }
        #endregion
    }
}
