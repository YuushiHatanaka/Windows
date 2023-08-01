using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// TelnetClientLibraryクラス
    /// </summary>
    partial class TelnetClientLibrary
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
        public delegate void LoginEventHandler(object sender, TelnetClientLoginEventArgs e);

        /// <summary>
        /// ログアウト event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void LogoutEventHandler(object sender, TelnetClientLogoutEventArgs e);

        /// <summary>
        /// コマンド実行 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void CommandExecuteEventHandler(object sender, TelnetClientCommandExecuteEventArgs e);
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
        /// DownLoad event
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
            Logger.Debug("=>>>> TelnetClientLibrary::AsyncLogin()");

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(LoginTimeout);

            // イベントパラメータ作成
            TelnetClientLoginEventArgs eventArgs = new TelnetClientLoginEventArgs();
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
                throw new TelnetClientException("ログインに失敗しました", ex);
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
                throw new TelnetClientException("ログインに失敗しました", ex);
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
                throw new TelnetClientException("ログインに失敗しました", ex);
            }
            finally
            {
                // イベント
                OnLogined(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TelnetClientLibrary::AsyncLogin()");
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
            Logger.Debug("=>>>> TelnetClientLibrary::AsyncLogout()");

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(LogoutTimeout);

            // イベントパラメータ作成
            TelnetClientLogoutEventArgs eventArgs = new TelnetClientLogoutEventArgs();
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
                throw new TelnetClientException("ログアウトに失敗しました", ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TelnetClientException("ログアウトに失敗しました", ex);
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
                throw new TelnetClientException("ログアウトに失敗しました", ex);
            }
            finally
            {
                // ログイン状態設定
                IsLogin = false;

                // イベント
                OnLogouted(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TelnetClientLibrary::AsyncLogout()");
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
            Logger.Debug("=>>>> TelnetClientLibrary::AsyncExcexute(string)");
            Logger.DebugFormat("command:{0}", command);

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(ExecuteTimeout);

            // イベントパラメータ作成
            TelnetClientCommandExecuteEventArgs eventArgs = new TelnetClientCommandExecuteEventArgs();
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
                throw new TelnetClientException(string.Format("「{0}」の実行に失敗しました", command), ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TelnetClientException(string.Format("「{0}」の実行に失敗しました", command), ex);
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
                throw new TelnetClientException(string.Format("「{0}」の実行に失敗しました", command), ex);
            }
            finally
            {
                // ログイン状態設定
                IsLogin = false;

                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TelnetClientLibrary::AsyncExcexute(string)");
            }
        }
        #endregion 
    }
}
