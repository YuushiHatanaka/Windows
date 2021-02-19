using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// SSHクラス
    /// </summary>
    public class Ssh : Common.Net.SshClient, TcpInterface, IDisposable
    {
        #region 仮想端末クラスオブジェクト
        /// <summary>
        /// 仮想端末クラスオブジェクト
        /// </summary>
        protected SshNetworkVirtualTerminal NetworkVirtualTerminal = new SshNetworkVirtualTerminal();
        #endregion

        #region SSHタスクキャンセルトークン
        /// <summary>
        /// SSHタスクキャンセルトークン
        /// </summary>
        protected SshCancellationTokenSource SshCancellationTokenSource = new SshCancellationTokenSource();
        #endregion

        #region 破棄済みフラグ
        /// <summary>
        /// 破棄済みフラグ
        /// </summary>
        private bool m_Disposed = false;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPasword"></param>
        public Ssh(string userName, string userPasword)
            : this("localhost", 22, userName, userPasword)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        /// <param name="userName"></param>
        /// <param name="userPasword"></param>
        public Ssh(string host, string userName, string userPasword)
            : this(host, 22, userName, userPasword)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="userPasword"></param>
        public Ssh(string host, int port, string userName, string userPasword)
            : base(host, port, userName, userPasword)
        {
            // 初期化
            this.Initialization(host, port, userName, userPasword);
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~Ssh()
        {
            // 破棄
            this.Dispose();
        }
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="userPasword"></param>
        protected override void Initialization(string host, int port, string userName, string userPasword)
        {
            // 基底クラス初期化
            base.Initialization(host, port, userName, userPasword);

            // TODO:未実装
        }
        #endregion

        #region 破棄
        /// <summary>
        /// 破棄
        /// </summary>
        public new void Dispose()
        {
            this.Dispose(true);
            base.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 破棄
        /// </summary>
        /// <param name="isDisposing"></param>
        protected override void Dispose(bool isDisposing)
        {
            // 破棄しているか？
            if (!this.m_Disposed)
            {
                // アンマネージドリソース解放

                // マネージドリソース解放
                if (isDisposing)
                {
                }

                // 破棄済みを設定
                this.m_Disposed = true;
            }
        }
        #endregion

        #region ShellStreamオブジェクト取得
        /// <summary>
        /// ShellStreamオブジェクト取得
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SshNetworkVirtualTerminalException"></exception>
        protected Renci.SshNet.ShellStream GetShellStream()
        {
            // SSHクライアントオブジェクト判定
            if (this.m_Client == null)
            {
                // 異常終了(例外)
                throw new SshException("SSHクライアントオブジェクトが生成されていません：[{" + this.m_IPEndPoint.ToString() + "]");
            }

            // ShellStreamオブジェクト取得
            return this.m_Client.CreateShellStream(
                this.NetworkVirtualTerminal.TerminalType,
                (uint)this.NetworkVirtualTerminal.Pixel.Width,
                (uint)this.NetworkVirtualTerminal.Pixel.Height,
                (uint)this.NetworkVirtualTerminal.Size.Width,
                (uint)this.NetworkVirtualTerminal.Size.Height,
                this.NetworkVirtualTerminal.ShellStreamBufferSize);
        }
        #endregion

        #region ログイン
        /// <summary>
        /// ログイン
        /// </summary>
        /// <returns></returns>
        public string Login()
        {
            StringBuilder result = new StringBuilder();
            string expectResult = string.Empty;

            // 接続判定
            if (!this.m_Client.IsConnected)
            {
                // 異常終了(例外)
                throw new SshException("接続に失敗しました：[{" + this.m_IPEndPoint.ToString() + "]");
            }

            // ShellStreamオブジェクト取得
            this.m_ShellStream = this.GetShellStream();

            // コマンドプロンプト待ち
            expectResult = this.Expect(this.m_CommandPrompt);

            // 結果追加
            result.Append(expectResult);

            // 結果返却
            return result.ToString();
        }

        /// <summary>
        /// ログイン
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public string Login(int timeout)
        {
            string _result = string.Empty;

            // Taskオブジェクト生成
            using (this.SshCancellationTokenSource.Login = new CancellationTokenSource())
            {
                // タイムアウト設定
                this.SshCancellationTokenSource.Login.CancelAfter(timeout);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    // ログイン
                    _result = this.Login();
                }, this.SshCancellationTokenSource.Login.Token);

                try
                {
                    // タスク待ち
                    task.Wait(this.SshCancellationTokenSource.Login.Token);
                }
                catch (OperationCanceledException ex)
                {
                    // 例外
                    throw new TelnetException("ログイン(Ssh)に失敗しました(OperationCanceledException)", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new TelnetException("ログイン(Ssh)に失敗しました(AggregateException)", ex);
                }
                finally
                {
                    // Taskオブジェクト破棄
                    this.SshCancellationTokenSource.Login = null;
                }
            }

            // 結果返却
            return _result;
        }
        #endregion

        #region ログアウト
        /// <summary>
        /// ログアウト
        /// </summary>
        /// <returns></returns>
        public void Logout()
        {
            // exitコマンド送信
            this.WriteLine("exit");
        }

        /// <summary>
        /// ログアウト
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public void Logout(int timeout)
        {
            // Taskオブジェクト生成
            using (this.SshCancellationTokenSource.Logout = new CancellationTokenSource())
            {
                // タイムアウト設定
                this.SshCancellationTokenSource.Logout.CancelAfter(timeout);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    // ログアウト
                    this.Logout();
                }, this.SshCancellationTokenSource.Logout.Token);

                try
                {
                    // タスク待ち
                    task.Wait(this.SshCancellationTokenSource.Logout.Token);
                }
                catch (OperationCanceledException ex)
                {
                    // 例外
                    throw new TelnetException("ログアウト(Ssh)に失敗しました(OperationCanceledException)", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new TelnetException("ログアウト(Ssh)に失敗しました(AggregateException)", ex);
                }
                finally
                {
                    // Taskオブジェクト破棄
                    this.SshCancellationTokenSource.Logout = null;
                }
            }
        }
        #endregion

        #region 文字列送信
        /// <summary>
        /// 文字列送信
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public void Write(string str)
        {
            // MemoryStreamオブジェクト生成
            MemoryStream sendStream = new MemoryStream();

            // エンコード
            byte[] data = this.NetworkVirtualTerminal.RemoteEncoding.GetBytes(str);

            // MemoryStreamオブジェクト書込
            sendStream.Write(data, 0, data.Length);

            // 送信
            this.Send(sendStream);
        }

        /// <summary>
        /// 文字列送信
        /// </summary>
        /// <param name="str"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public void Write(string str, int timeout)
        {
            // Taskオブジェクト生成
            using (this.SshCancellationTokenSource.WriteLine = new CancellationTokenSource())
            {
                // タイムアウト設定
                this.SshCancellationTokenSource.WriteLine.CancelAfter(timeout);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    // 文字列送信
                    this.Write(str);
                }, this.SshCancellationTokenSource.WriteLine.Token);

                try
                {
                    // タスク待ち
                    task.Wait(this.SshCancellationTokenSource.WriteLine.Token);
                }
                catch (OperationCanceledException ex)
                {
                    // 例外
                    throw new SshException("文字列送信(Telnet)に失敗しました(OperationCanceledException)", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new SshException("文字列送信(Telnet)に失敗しました(AggregateException)", ex);
                }
                finally
                {
                    // Taskオブジェクト破棄
                    this.SshCancellationTokenSource.WriteLine = null;
                }
            }
        }

        /// <summary>
        /// 文字列送信
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine(string line)
        {
            // 書込(文字列+改行)
            this.Write(line);
            this.Write(this.NetworkVirtualTerminal.WriteNewLine);
        }

        /// <summary>
        /// 文字列送信
        /// </summary>
        /// <param name="line"></param>
        /// <param name="timeout"></param>
        public void WriteLine(string line, int timeout)
        {
            // Taskオブジェクト生成
            using (this.SshCancellationTokenSource.WriteLine = new CancellationTokenSource())
            {
                // タイムアウト設定
                this.SshCancellationTokenSource.WriteLine.CancelAfter(timeout);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    // 文字列送信
                    this.WriteLine(line);
                }, this.SshCancellationTokenSource.WriteLine.Token);

                try
                {
                    // タスク待ち
                    task.Wait(this.SshCancellationTokenSource.WriteLine.Token);
                }
                catch (OperationCanceledException ex)
                {
                    // 例外
                    throw new SshException("文字列送信(Telnet)に失敗しました(OperationCanceledException)", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new SshException("文字列送信(Telnet)に失敗しました(AggregateException)", ex);
                }
                finally
                {
                    // Taskオブジェクト破棄
                    this.SshCancellationTokenSource.WriteLine = null;
                }
            }
        }
        #endregion
 
        #region コマンド実行
        /// <summary>
        /// コマンド実行
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public string Execute(string command)
        {
            StringBuilder result = new StringBuilder();
            string expectResult = string.Empty;

            // コマンド送信
            this.WriteLine(command);

            // コマンドプロンプト待ち
            expectResult = this.Expect(this.m_CommandPrompt);

            // 結果追加
            result.Append(expectResult);

            // 結果返却
            return result.ToString();
        }

        /// <summary>
        /// コマンド実行
        /// </summary>
        /// <param name="command"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public string Execute(string command, int timeout)
        {
            string _result = string.Empty;

            // Taskオブジェクト生成
            using (this.SshCancellationTokenSource.Execute = new CancellationTokenSource())
            {
                // タイムアウト設定
                this.SshCancellationTokenSource.Execute.CancelAfter(timeout);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    // コマンド実行
                    _result = this.Execute(command);
                }, this.SshCancellationTokenSource.Execute.Token);

                try
                {
                    // タスク待ち
                    task.Wait(this.SshCancellationTokenSource.Execute.Token);
                }
                catch (OperationCanceledException ex)
                {
                    // 例外
                    throw new TelnetException("コマンド実行(Ssh)に失敗しました(OperationCanceledException)", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new TelnetException("コマンド実行(Ssh)に失敗しました(AggregateException)", ex);
                }
                finally
                {
                    // Taskオブジェクト破棄
                    this.SshCancellationTokenSource.Execute = null;
                }
            }
            
            // 結果返却
            return _result;
        }
        #endregion

        #region 結果待ち
        /// <summary>
        /// 結果待ち
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Expect(string text)
        {
            StringBuilder result = new StringBuilder();

            // 一致するまで繰り返し
            while (true)
            {
                // 受信
                using (MemoryStream stream = this.Recive())
                {
                    // 文字コード変換
                    string resultRecive = this.NetworkVirtualTerminal.LocalEncoding.GetString(stream.ToArray());

                    // 結果格納
                    result.Append(resultRecive);

                    // 文字列比較
                    Regex regex = new Regex(text, RegexOptions.Compiled | RegexOptions.Multiline);
                    if (regex.IsMatch(resultRecive.ToString()))
                    {
                        break;
                    }
                }
            }

            // 結果を返却する
            return result.ToString();
        }

        /// <summary>
        /// 結果待ち
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Expect(string text, int timeout)
        {
            string _result = string.Empty;

            // Taskオブジェクト生成
            using (this.SshCancellationTokenSource.Expect = new CancellationTokenSource())
            {
                // タイムアウト設定
                this.SshCancellationTokenSource.Expect.CancelAfter(timeout);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    // 結果待ち
                    _result = this.Expect(text);
                }, this.SshCancellationTokenSource.Expect.Token);

                try
                {
                    // タスク待ち
                    task.Wait(this.SshCancellationTokenSource.Expect.Token);
                }
                catch (OperationCanceledException ex)
                {
                    // 例外
                    throw new SshException("結果待ち(Ssh)に失敗しました(OperationCanceledException)", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new SshException("結果待ち(Ssh)に失敗しました(AggregateException)", ex);
                }
                finally
                {
                    // Taskオブジェクト破棄
                    this.SshCancellationTokenSource.Expect = null;
                }
            }

            // 結果返却
            return _result;
        }
        #endregion
    }
}
