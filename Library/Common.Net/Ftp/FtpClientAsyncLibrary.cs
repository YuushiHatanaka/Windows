using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// FtpClientLibraryクラス(partial)
    /// </summary>
    partial class FtpClientLibrary
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
        /// コマンド実行
        /// </summary>
        public TimeSpan CommandTimeout { get; set; } = new TimeSpan(0, 0, 0, 30, 0);
        #endregion

        #region event delegate
        /// <summary>
        /// ログイン event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void LoginEventHandler(object sender, FtpClientLoginEventArgs e);

        /// <summary>
        /// ログアウト event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void LogoutEventHandler(object sender, FtpClientLogoutEventArgs e);

        /// <summary>
        /// UpLoad event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void UpLoadEventHandler(object sender, FtpClientUpLoadEventArgs e);

        /// <summary>
        /// DownLoad event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void DownLoadEventHandler(object sender, FtpClientDownLoadEventArgs e);

        /// <summary>
        /// コマンド実行 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void CommandExecuteEventHandler(object sender, FtpClientCommandExecuteEventArgs e);
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
        /// UpLoad event
        /// </summary>
        public event UpLoadEventHandler OnUpLoaded = delegate { };

        /// <summary>
        /// DownLoad event
        /// </summary>
        public event DownLoadEventHandler OnDownLoaded = delegate { };

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
            Logger.Debug("=>>>> FtpClientLibrary::AsyncLogin()");

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(LoginTimeout);

            // イベントパラメータ作成
            FtpClientLoginEventArgs eventArgs = new FtpClientLoginEventArgs();
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
                throw new FtpClientException("ログインに失敗しました", ex);
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
                throw new FtpClientException("ログインに失敗しました", ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TcpClientException("ログインに失敗しました", ex);
            }
            finally
            {
                // イベント
                OnLogined(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncLogin()");
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
            Logger.Debug("=>>>> FtpClientLibrary::AsyncLogout()");

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(LogoutTimeout);

            // イベントパラメータ作成
            FtpClientLogoutEventArgs eventArgs = new FtpClientLogoutEventArgs();
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
                throw new FtpClientException("ログアウトに失敗しました", ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException("ログアウトに失敗しました", ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new TcpClientException("ログアウトに失敗しました", ex);
            }
            finally
            {
                // ログイン状態設定
                IsLogin = false;

                // イベント
                OnLogouted(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncLogout()");
            }
        }
        #endregion

        #region コマンド(非同期)
        /// <summary>
        /// 1つ上位のディレクトリ(親ディレクトリ)をカレントディレクトリとする
        /// </summary>
        public async Task AsyncCDUP()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::AsyncCDUP()");

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(CommandTimeout);

            // イベントパラメータ作成
            FtpClientCommandExecuteEventArgs eventArgs = new FtpClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.CommandType = FtpCommand.CDUP;
            eventArgs.Command = string.Format("CDUP");

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // コマンド実行
                    eventArgs.Response = CDUP();

                    // 結果設定
                    eventArgs.Result = eventArgs.Response.Result;
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            finally
            {
                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncCDUP()");
            }
        }

        /// <summary>
        /// 指定したディレクトリをカレントディレクトリとする
        /// </summary>
        /// <param name="directoryName">ディレクトリ名</param>
        /// <returns>true:正常(250 Directory successfully changed.) false:異常(550 Failed to change directory.)</returns>
        /// <exception cref="FtpClientException"></exception>
        public async Task AsyncCWD(string directoryName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::AsyncCWD(string)");
            Logger.DebugFormat("directoryName:{0}", directoryName);

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(CommandTimeout);

            // イベントパラメータ作成
            FtpClientCommandExecuteEventArgs eventArgs = new FtpClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.CommandType = FtpCommand.CWD;
            eventArgs.Command = string.Format("CWD {0}", directoryName);

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // コマンド実行
                    eventArgs.Response = CWD(directoryName);

                    // 結果設定
                    eventArgs.Result = eventArgs.Response.Result;
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            finally
            {
                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncCWD(string)");
            }
        }

        /// <summary>
        /// 指定したファイルを削除する
        /// </summary>
        /// <param name="fileName"></param>
        public async Task AsyncDELE(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::AsyncDELE(string)");
            Logger.DebugFormat("fileName:{0}", fileName);

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(CommandTimeout);

            // イベントパラメータ作成
            FtpClientCommandExecuteEventArgs eventArgs = new FtpClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.CommandType = FtpCommand.DELE;
            eventArgs.Command = string.Format("DELE {0}", fileName);

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // コマンド実行
                    eventArgs.Response = DELE(fileName);

                    // 結果設定
                    eventArgs.Result = eventArgs.Response.Result;
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            finally
            {
                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncDELE(string)");
            }
        }

        /// <summary>
        /// 現在のワーキングディレクトリ内のファイル一覧を表示する
        /// </summary>
        /// <param name="path"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task AsyncLIST(string path)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::AsyncLIST(stringe)");
            Logger.DebugFormat("path:{0}", path);

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(CommandTimeout);

            // イベントパラメータ作成
            FtpClientCommandExecuteEventArgs eventArgs = new FtpClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.CommandType = FtpCommand.LIST;
            eventArgs.Command = string.Format("LIST {0}", path);

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // 結果オブジェクト
                    string[] result = null;

                    // コマンド実行
                    eventArgs.Response = LIST(path, out result);
                    eventArgs.ExecuteResult = result;

                    // 結果設定
                    eventArgs.Result = eventArgs.Response.Result;
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            finally
            {
                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncLIST(stringe)");
            }
        }

        /// <summary>
        /// 指定したディレクトリを作成する
        /// </summary>
        /// <param name="directoryName"></param>
        /// <exception cref="FtpClientException"></exception>
        public async Task AsyncMKD(string directoryName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::AsyncMKD(string)");
            Logger.DebugFormat("directoryName:{0}", directoryName);

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(CommandTimeout);

            // イベントパラメータ作成
            FtpClientCommandExecuteEventArgs eventArgs = new FtpClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.CommandType = FtpCommand.MKD;
            eventArgs.Command = string.Format("MKD {0}", directoryName);

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // コマンド実行
                    eventArgs.Response = MKD(directoryName);

                    // 結果設定
                    eventArgs.Result = eventArgs.Response.Result;
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            finally
            {
                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncMKD(string)");
            }
        }

        /// <summary>
        /// 現在のワーキングディレクトリ内のファイル一覧を表示する
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public async Task AsyncNLST(string fileName, params string[] option)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::AsyncNLST(stringe, params string[])");
            Logger.DebugFormat("fileName:{0}", fileName);
            Logger.DebugFormat("option  :{0}", option.Length);

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(CommandTimeout);

            // イベントパラメータ作成
            FtpClientCommandExecuteEventArgs eventArgs = new FtpClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.CommandType = FtpCommand.NLST;
            StringBuilder optionBuilder = new StringBuilder();
            foreach (string o in option)
            {
                optionBuilder.Append(string.Format("{0} ", o));
            }
            eventArgs.Command = string.Format("NLST {0} {1}", fileName, optionBuilder.ToString().TrimEnd());

            try
            {
                // Task開始
                await Task.Run(() =>
                {

                    // 結果オブジェクト
                    List<string> result = null;

                    // コマンド実行
                    eventArgs.Response = NLST(out result, fileName, option);
                    eventArgs.ExecuteResult = result;

                    // 結果設定
                    eventArgs.Result = eventArgs.Response.Result;
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            finally
            {
                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= FtpClientLibrary::AsyncNLST(stringe, params string[])");
            }
        }

        /// <summary>
        /// 何もしない。サーバの稼動を確認するために実⾏される。
        /// </summary>
        public async Task AsyncNOOP()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::AsyncNOOP()");

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(CommandTimeout);

            // イベントパラメータ作成
            FtpClientCommandExecuteEventArgs eventArgs = new FtpClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.CommandType = FtpCommand.NOOP;
            eventArgs.Command = string.Format("NOOP");

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // コマンド実行
                    eventArgs.Response = NOOP();

                    // 結果設定
                    eventArgs.Result = eventArgs.Response.Result;
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            finally
            {
                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncNOOP()");
            }
        }

        /// <summary>
        /// ユーザーのパスワードを指定する
        /// </summary>
        /// <param name="userPassword">ユーザーパスワード</param>
        /// <exception cref="FtpClientException"></exception>
        public async Task AsyncPASS(string userPassword)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::AsyncPASS(string)");
            Logger.DebugFormat("userPassword:{0}", new string('*', userPassword.Length));

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(CommandTimeout);

            // イベントパラメータ作成
            FtpClientCommandExecuteEventArgs eventArgs = new FtpClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.CommandType = FtpCommand.PASS;
            eventArgs.Command = string.Format("PASS {0}", userPassword);

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // コマンド実行
                    eventArgs.Response = PASS(userPassword);

                    // 結果設定
                    eventArgs.Result = eventArgs.Response.Result;
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            finally
            {
                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncPASS(string)");
            }
        }

        /// <summary>
        /// パッシブモードへの移行を指示する。
        /// </summary>
        /// <exception cref="FtpClientException"></exception>
        /// <returns></returns>
        public async Task AsyncPASV()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::AsyncPASV()");

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(CommandTimeout);

            // イベントパラメータ作成
            FtpClientCommandExecuteEventArgs eventArgs = new FtpClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.CommandType = FtpCommand.PASV;
            eventArgs.Command = string.Format("PASV");

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // コマンド実行
                    eventArgs.Response = PASV();

                    // 結果設定
                    eventArgs.Result = eventArgs.Response.Result;
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            finally
            {
                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncPASV()");
            }
        }

        /// <summary>
        /// データコネクションで使用するIPアドレス（通常はクライアント）とポート番号を指示する
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task AsyncPORT()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::AsyncPORT()");

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(CommandTimeout);

            // イベントパラメータ作成
            FtpClientCommandExecuteEventArgs eventArgs = new FtpClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.CommandType = FtpCommand.PORT;
            eventArgs.Command = string.Format("PORT");

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // コマンド実行
                    eventArgs.Response = PORT();

                    // 結果設定
                    eventArgs.Result = eventArgs.Response.Result;
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            finally
            {
                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncPORT()");
            }
        }

        /// <summary>
        /// 現在のワーキングディレクトリを表示する
        /// </summary>
        /// <returns></returns>
        public async Task AsyncPWD()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::AsyncPWD()");

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(CommandTimeout);

            // イベントパラメータ作成
            FtpClientCommandExecuteEventArgs eventArgs = new FtpClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.CommandType = FtpCommand.PWD;
            eventArgs.Command = string.Format("PWD");

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // コマンド実行
                    string result = string.Empty;
                    eventArgs.Response = PWD(out result);

                    // コマンド実行結果設定
                    eventArgs.ExecuteResult = result;

                    // 結果設定
                    eventArgs.Result = eventArgs.Response.Result;
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            finally
            {
                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncPWD()");
            }
        }

        /// <summary>
        /// ログアウトする
        /// </summary>
        /// <exception cref="FtpClientException"></exception>
        public async Task AsyncQUIT()
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::AsyncQUIT()");

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(CommandTimeout);

            // イベントパラメータ作成
            FtpClientCommandExecuteEventArgs eventArgs = new FtpClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.CommandType = FtpCommand.QUIT;
            eventArgs.Command = string.Format("QUIT");

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // コマンド実行
                    eventArgs.Response = QUIT();

                    // 結果設定
                    eventArgs.Result = eventArgs.Response.Result;
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            finally
            {
                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncQUIT()");
            }
        }

        /// <summary>
        /// 指定したファイルの内容をサーバから取得する
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task AsyncRETR(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::AsyncRETR(string)");
            Logger.DebugFormat("fileName:{0}", fileName);

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(CommandTimeout);

            // イベントパラメータ作成
            FtpClientCommandExecuteEventArgs eventArgs = new FtpClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.CommandType = FtpCommand.RETR;
            eventArgs.Command = string.Format("RETR {0}", fileName);

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // 結果オブジェクト
                    MemoryStream result = null;

                    // コマンド実行
                    eventArgs.Response = RETR(fileName, out result);
                    eventArgs.ExecuteResult = result;

                    // 結果設定
                    eventArgs.Result = eventArgs.Response.Result;
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            finally
            {
                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= FtpClientLibrary::AsyncRETR(string)");
            }
        }

        /// <summary>
        /// 指定したディレクトリを削除する
        /// </summary>
        /// <param name="directoryName"></param>
        public async Task AsyncRMD(string directoryName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::AsyncRMD(string)");
            Logger.DebugFormat("directoryName:{0}", directoryName);

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(CommandTimeout);

            // イベントパラメータ作成
            FtpClientCommandExecuteEventArgs eventArgs = new FtpClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.CommandType = FtpCommand.RMD;
            eventArgs.Command = string.Format("RMD {0}", directoryName);

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // コマンド実行
                    eventArgs.Response = RMD(directoryName);

                    // 結果設定
                    eventArgs.Result = eventArgs.Response.Result;
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            finally
            {
                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncRMD(string)");
            }
        }

        /// <summary>
        /// 指定したファイル名を変更する(変更元ファイル名の指定)。
        /// RNTOを続けて実行しなくてはならない
        /// </summary>
        /// <param name="fileName">変更元ファイル名</param>
        /// <returns></returns>
        public async Task AsyncRNFR(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::AsyncRNFR(string)");
            Logger.DebugFormat("fileName:{0}", fileName);

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(CommandTimeout);

            // イベントパラメータ作成
            FtpClientCommandExecuteEventArgs eventArgs = new FtpClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.CommandType = FtpCommand.RNFR;
            eventArgs.Command = string.Format("RNFR {0}", fileName);

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // コマンド実行
                    eventArgs.Response = RNFR(fileName);

                    // 結果設定
                    eventArgs.Result = eventArgs.Response.Result;
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            finally
            {
                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncRNFR(string)");
            }
        }

        /// <summary>
        /// RNFRコマンドで指定したファイルを、指定したファイル名に変更する。
        /// </summary>
        /// <param name="fileName">変更先ファイル名</param>
        /// <returns></returns>
        public async Task AsyncRNTO(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::AsyncRNTO(string)");
            Logger.DebugFormat("fileName:{0}", fileName);

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(CommandTimeout);

            // イベントパラメータ作成
            FtpClientCommandExecuteEventArgs eventArgs = new FtpClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.CommandType = FtpCommand.RNTO;
            eventArgs.Command = string.Format("RNTO {0}", fileName);

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // コマンド実行
                    eventArgs.Response = RNTO(fileName);

                    // 結果設定
                    eventArgs.Result = eventArgs.Response.Result;
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            finally
            {
                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncRNTO(string)");
            }
        }

        /// <summary>
        /// 任意のOSコマンドを実行する
        /// </summary>
        /// <param name="command"></param>
        public async Task AsyncSITE(string command)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::AsyncSITE(string)");
            Logger.DebugFormat("command:{0}", command);

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(CommandTimeout);

            // イベントパラメータ作成
            FtpClientCommandExecuteEventArgs eventArgs = new FtpClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.CommandType = FtpCommand.SITE;
            eventArgs.Command = string.Format("SITE {0}", command);

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // コマンド実行
                    eventArgs.Response = SITE(command);

                    // 結果設定
                    eventArgs.Result = eventArgs.Response.Result;
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            finally
            {
                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncSITE(string)");
            }
        }

        /// <summary>
        /// 指定したファイル名で、サーバへ送信するデータでファイルを作成する
        /// <remarks>同⼀名のファイルがすでにある場合には、上書きする</remarks>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task AsyncSTOR(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::AsyncSTOR(stringe)");
            Logger.DebugFormat("fileName:{0}", fileName);

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(CommandTimeout);

            // イベントパラメータ作成
            FtpClientCommandExecuteEventArgs eventArgs = new FtpClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.CommandType = FtpCommand.STOR;
            eventArgs.Command = string.Format("STOR {0}", fileName);

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // コマンド実行
                    eventArgs.Response = STOR(fileName);

                    // 結果設定
                    eventArgs.Result = eventArgs.Response.Result;
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            finally
            {
                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncSTOR(stringe)");
            }
        }

        /// <summary>
        /// 指定したファイル名で、サーバへ送信するデータでファイルを作成する。
        /// <remarks>すでに同一名のファイルがあった場合には、重ならないようなファイル名を自動的に付けて作成する</remarks>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task AsyncSTOU(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibrary::AsyncSTOU(stringe)");
            Logger.DebugFormat("fileName:{0}", fileName);

            // タイムアウト設定
            m_CancellationTokenSource.CancelAfter(CommandTimeout);

            // イベントパラメータ作成
            FtpClientCommandExecuteEventArgs eventArgs = new FtpClientCommandExecuteEventArgs();
            eventArgs.IPAddress = m_HostInfo.IPAddress;
            eventArgs.CommandType = FtpCommand.STOU;
            eventArgs.Command = string.Format("STOU {0}", fileName);

            try
            {
                // Task開始
                await Task.Run(() =>
                {
                    // コマンド実行
                    eventArgs.Response = STOU(fileName);

                    // 結果設定
                    eventArgs.Result = eventArgs.Response.Result;
                }, m_CancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (AggregateException ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            catch (Exception ex)
            {
                // 結果設定
                eventArgs.Result = false;

                // 例外設定
                eventArgs.Exception = ex;

                // 例外
                throw new FtpClientException(
                    string.Format("コマンド実行[{0}]に失敗しました",
                        eventArgs.CommandType.ToString()),
                        ex);
            }
            finally
            {
                // イベント
                OnCommandExecute(this, eventArgs);

                // ロギング
                Logger.Debug("<<<<= TcpClientLibrary::AsyncSTOU(stringe)");
            }
        }
        #endregion
    }
}
