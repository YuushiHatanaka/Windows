using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// SSHクラス
    /// </summary>
    public class Ssh : SshNetworkVirtualTerminal
    {
        /// <summary>
        /// コマンドプロンプト
        /// </summary>
        private string m_CommandPrompt = @"\$ $";

        /// <summary>
        /// exitコマンド
        /// </summary>
        private string m_ExitCommand = "exit";

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPasword"></param>
        public Ssh(string userName, string userPasword)
            : base("localhost")
        {
            // 初期化
            this.Initialization(userName, userPasword);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        /// <param name="userName"></param>
        /// <param name="userPasword"></param>
        public Ssh(string host, string userName, string userPasword)
            : base(host)
        {
            // 初期化
            this.Initialization(userName, userPasword);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="userPasword"></param>
        public Ssh(string host, int port, string userName, string userPasword)
            : base(host, port)
        {
            // 初期化
            this.Initialization(userName, userPasword);
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~Ssh()
        {
        }
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPasword"></param>
        private void Initialization(string userName, string userPasword)
        {
            // 初期設定
            this.m_UserName = userName;
            this.m_UserPassword = userPasword;
            this.OnException += this.ExceptionEventHandler;
        }
        #endregion

        #region ログイン
        /// <summary>
        /// ログイン
        /// </summary>
        public string Login()
        {
            StringBuilder result = new StringBuilder(); // 結果用

            // 接続
            this.Connect();

            // プロンプト取得待ち
            List<string> waitResult = this.Wait(this.m_CommandPrompt);

            // 結果格納
            foreach (string str in waitResult)
            {
                result.AppendLine(str);
            }

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
            string result = string.Empty;   // 結果用

            // Taskオブジェクト生成
            using (CancellationTokenSource source = new CancellationTokenSource())
            {
                // タイムアウト設定
                source.CancelAfter(timeout);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    // ログイン
                    result = this.Login();
                }, source.Token);

                try
                {
                    // タスク待ち
                    task.Wait(source.Token);
                    return result;
                }
                catch (OperationCanceledException ex)
                {
                    // 例外
                    throw new TelnetException("ログインに失敗しました", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new TelnetException("ログインに失敗しました", ex);
                }
            }
        }
        #endregion

        #region ログアウト
        /// <summary>
        /// ログアウト
        /// </summary>
        public string Logout()
        {
            StringBuilder result = new StringBuilder(); // 結果用

            // exitコマンド送信
            this.m_ShellStream.WriteLine(this.m_ExitCommand);

            // 受信待ち
            List<string> waitResult = this.Wait();

            // 結果格納
            foreach (string str in waitResult)
            {
                result.AppendLine(str);
            }

            // 切断
            this.DisConnect();

            // 結果返却
            return result.ToString();
        }

        /// <summary>
        /// ログアウト
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public string Logout(int timeout)
        {
            string result = string.Empty;   // 結果用

            // Taskオブジェクト生成
            using (CancellationTokenSource source = new CancellationTokenSource())
            {
                // タイムアウト設定
                source.CancelAfter(timeout);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    // ログアウト
                    result = this.Logout();
                }, source.Token);

                try
                {
                    // タスク待ち
                    task.Wait(source.Token);
                    return result;
                }
                catch (OperationCanceledException ex)
                {
                    // 例外
                    throw new TelnetException("ログアウトに失敗しました", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new TelnetException("ログアウトに失敗しました", ex);
                }
            }
        }
        #endregion

        #region コマンド実行
        /// <summary>
        /// コマンド実行
        /// </summary>
        public string Execute(string command)
        {
            StringBuilder read = null;                  // 読込用
            StringBuilder result = new StringBuilder(); // 結果用

            // コマンド送信
            this.WriteLine(command);

            // コマンドプロンプト待ち
            read = this.Read(this.m_CommandPrompt);
            if (read == null)
            {
                // 例外
                throw new TelnetException("コマンド実行に失敗しました:[コマンドプロンプト待ち]");
            }

            // 結果追加
            result.Append(read.ToString());

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
            string result = string.Empty;   // 結果用

            // Taskオブジェクト生成
            using (CancellationTokenSource source = new CancellationTokenSource())
            {
                // タイムアウト設定
                source.CancelAfter(timeout);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    // コマンド実行
                    result = this.Execute(command);
                }, source.Token);

                try
                {
                    // タスク待ち
                    task.Wait(source.Token);
                    return result;
                }
                catch (OperationCanceledException ex)
                {
                    // 例外
                    throw new TelnetException("コマンド実行に失敗しました", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new TelnetException("コマンド実行に失敗しました", ex);
                }
            }
        }
        #endregion

        #region 待合せ
        /// <summary>
        /// 待合せ
        /// </summary>
        /// <returns></returns>
        public List<string> Wait()
        {
            return this.Wait(this.m_CommandPrompt);
        }

        /// <summary>
        /// ShellStream待合せ
        /// </summary>
        /// <param name="regex"></param>
        /// <returns></returns>
        public List<string> Wait(string regex)
        {
            // 返却用オブジェクト生成
            List<string> lines = new List<string>();

            // 無限ループ
            while (true)
            {
                // ShellStreamにバッファ長が0以上になるまで待合せ
                while (this.m_ShellStream.Length == 0)
                {
                    Thread.Sleep(100);
                }

                // 読込
                byte[] _readbuffer = new byte[this.m_ShellStream.Length];
                if (this.m_ShellStream.Read(_readbuffer, 0, (int)this.m_ShellStream.Length) == 0)
                {
                    break;
                }

                // 文字コード変換
                System.Text.Encoding src = Common.Text.Encoding.GetCode(_readbuffer);
                System.Text.Encoding dest = System.Text.Encoding.GetEncoding("Shift_JIS");
                byte[] temp = System.Text.Encoding.Convert(src, dest, _readbuffer);
                string readLines = dest.GetString(temp);

                // 結果格納
                foreach (string line in Regex.Split(readLines, @"\n"))
                {
                    string _line = Regex.Replace(line, @"\r", "");
                    lines.Add(_line);
                }

                // 正規表現で比較
                if ((regex != string.Empty) && (Regex.IsMatch(readLines, regex, RegexOptions.Multiline)))
                {
                    // 一致
                    break;
                }
            }

            // 先頭(入力行)と最後(待合せ文字列)を削除
            if (lines.Count > 0)
            {
                lines.RemoveAt(lines.Count - 1);
            }
            if (lines.Count > 0)
            {
                lines.RemoveAt(0);
            }

            // 一致
            this.m_ShellStream.Flush();
            return lines;
        }

        /// <summary>
        /// 待合せ
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public List<string> Wait(int timeout)
        {
            return this.Wait(this.m_CommandPrompt, timeout);
        }

        /// <summary>
        /// 待合せ
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public List<string> Wait(string regex, int timeout)
        {
            // 返却用オブジェクト生成
            List<string> lines = new List<string>();

            // Taskオブジェクト生成
            using (CancellationTokenSource source = new CancellationTokenSource())
            {
                // タイムアウト設定
                source.CancelAfter(timeout);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    // 待合せ
                    lines = this.Wait(regex);
                }, source.Token);

                try
                {
                    // タスク待ち
                    task.Wait(source.Token);
                    return lines;
                }
                catch (OperationCanceledException ex)
                {
                    // 例外
                    throw new TelnetException("待合せに失敗しました", ex);
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new TelnetException("待合せに失敗しました", ex);
                }
            }
        }

        #region 例外イベントハンドラ
        /// <summary>
        /// 例外イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExceptionEventHandler(object sender, SshNetworkVirtualTerminalExceptionEventArgs e)
        {
            Debug.WriteLine("★★★★★ {0}受信:[{1}]", e.Exception.GetType().ToString(), e.Exception.Message);

            // 例外発行判定
            if (e.Exception.GetType() == typeof(TelnetClientException))
            {
                // 例外発行
                throw new TelnetException(e.Exception.Message);
            }
            else if (e.Exception.GetType() == typeof(NetworkVirtualTerminalException))
            {
                // 例外発行
                throw new TelnetException(e.Exception.Message);
            }
            else if (e.Exception.GetType() == typeof(SocketException))
            {
                // 例外発行
                throw new TelnetException(e.Exception.Message);
            }
        }
        #endregion
    }
    #endregion
}
