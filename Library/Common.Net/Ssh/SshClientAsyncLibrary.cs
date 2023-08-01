using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.Net.FtpClientLibrary;

namespace Common.Net
{
    /// <summary>
    /// SshClientLibraryクラス
    /// </summary>
    partial class SshClientLibrary
    {
        #region タイムアウト
        /// <summary>
        /// Login
        /// </summary>
        public TimeSpan LoginTimeout { get; set; } = new TimeSpan(0, 0, 0, 10, 0);

        /// <summary>
        /// Logout
        /// </summary>
        public TimeSpan LogoutTimeout { get; set; } = new TimeSpan(0, 0, 0, 10, 0);

        /// <summary>
        /// 実行
        /// </summary>
        public TimeSpan ExecuteTimeout { get; set; } = new TimeSpan(0, 0, 0, 30, 0);
        #endregion

        #region event delegate
        /// <summary>
        /// ログイン event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void LoginEventHandler(object sender, SshClientLoginEventArgs e);

        /// <summary>
        /// ログアウト event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void LogoutEventHandler(object sender, SshClientLogoutEventArgs e);

        /// <summary>
        /// コマンド実行 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void CommandExecuteEventHandler(object sender, SshClientCommandExecuteEventArgs e);
        #endregion

        #region event
        /// <summary>
        /// ログイン event
        /// </summary>
        public event LoginEventHandler OnLogined = delegate { };

        /// <summary>
        /// ログアウト event
        /// </summary>
        public event LogoutEventHandler OnLogouted = delegate { };

        /// <summary>
        /// CommandExecute event
        /// </summary>
        public event CommandExecuteEventHandler OnCommandExecute = delegate { };
        #endregion

        #region ログイン(非同期)
        /// <summary>
        /// ログイン(非同期)
        /// </summary>
        /// <returns></returns>
        public async Task AsyncLogin()
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibrary::AsyncLogin()");

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(LoginTimeout);

            // イベントパラメータ作成
            SshClientLoginEventArgs eventArgs = new SshClientLoginEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.UserName = UserInfo.Name;

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // ログイン
                    if (!Login())
                    {
                        // 結果設定
                        eventArgs.Result = false;

                        // ログイン状態設定
                        IsLogin = false;
                    }
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // ログイン状態設定
                IsLogin = false;

                // 例外
                throw new SshClientException("ログインに失敗しました", ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // ログイン状態設定
                IsLogin = false;

                // 例外
                throw new SshClientException("ログインに失敗しました", ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // ログイン状態設定
                IsLogin = false;

                // 例外
                throw new SshClientException("ログインに失敗しました", ex);
            }
            finally
            {
                // イベント
                OnLogined(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= SshClientLibrary::AsyncLogin()");
            }
        }
        #endregion

        #region ログアウト(非同期)
        /// <summary>
        /// ログアウト(非同期)
        /// </summary>
        /// <returns></returns>
        public async Task AsyncLogout()
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibrary::AsyncLogout()");

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(LogoutTimeout);

            // イベントパラメータ作成
            SshClientLogoutEventArgs eventArgs = new SshClientLogoutEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.UserName = UserInfo.Name;

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // ログアウト
                    if (!Logout())
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
                throw new SshClientException("ログアウトに失敗しました", ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new SshClientException("ログアウトに失敗しました", ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new SshClientException("ログアウトに失敗しました", ex);
            }
            finally
            {
                // ログイン状態設定
                IsLogin = false;

                // イベント
                OnLogouted(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= SshClientLibrary::AsyncLogout()");
            }
        }
        #endregion

        #region 実行(非同期)
        /// <summary>
        /// 実行(非同期)
        /// </summary>
        /// <returns></returns>
        public async Task AsyncExcexute(string command)
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibrary::AsyncExcexute(string)");
            Logger.DebugFormat("command:{0}", command);

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(ExecuteTimeout);

            // イベントパラメータ作成
            SshClientCommandExecuteEventArgs eventArgs = new SshClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.Command = command;

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // 実行
                    eventArgs.ExecuteResult = Excexute(command);

                    // 実行結果判定
                    if (eventArgs.ExecuteResult.Length == 0)
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
                throw new SshClientException(string.Format("「{0}」の実行に失敗しました", command), ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new SshClientException(string.Format("「{0}」の実行に失敗しました", command), ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new SshClientException(string.Format("「{0}」の実行に失敗しました", command), ex);
            }
            finally
            {
                // ログイン状態設定
                IsLogin = false;

                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= SshClientLibrary::AsyncExcexute(string)");
            }
        }
        #endregion 
    }
}
