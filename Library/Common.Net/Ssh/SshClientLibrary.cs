using Common.Diagnostics;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Common.Net
{
    /// <summary>
    /// SshClientLibraryクラス
    /// </summary>
    public partial class SshClientLibrary : TcpClientLibrary, InterfaceTcpClientLibrary
    {
        #region ログイン状態
        /// <summary>
        /// ログイン状態
        /// </summary>
        public bool IsLogin { get; protected set; }
        #endregion

        #region 接続情報
        /// <summary>
        /// 接続情報
        /// </summary>
        private ConnectionInfo m_ConnectionInfo = null;
        #endregion

        #region SSHクライアント
        /// <summary>
        /// SSHクライアント
        /// </summary>
        private SshClient m_Client = null;
        #endregion

        #region ShellStreamオブジェクト
        /// <summary>
        /// ShellStreamオブジェクト
        /// </summary>
        private ShellStream m_ShellStream = null;
        #endregion

        #region 認証情報リスト
        /// <summary>
        /// 認証情報リスト
        /// </summary>
        private List<AuthenticationMethod> m_AuthenticationMethodList = new List<AuthenticationMethod>();
        #endregion

        #region ユーザ情報
        /// <summary>
        /// ユーザ情報
        /// </summary>
        public UserInfo UserInfo { get; set; } = new UserInfo();
        #endregion

        #region サーバ情報
        /// <summary>
        /// サーバ情報
        /// </summary>
        public ServerInfo ServerInfo { get; set; } = new ServerInfo();
        #endregion

        #region シェルストリーム情報
        /// <summary>
        /// シェルストリーム情報
        /// </summary>
        public ShellStreamInfo ShellStreamInfo { get; set; }=new ShellStreamInfo();
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        public SshClientLibrary(string host)
            : base(host, 22)
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibrary::SshClientLibrary(string)");
            Logger.DebugFormat("host:{0}", host);

            // ロギング
            Logger.Debug("<<<<= SshClientLibrary::SshClientLibrary(string)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public SshClientLibrary(string host, int port)
            : base(host, port)
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibrary::SshClientLibrary(string, int)");
            Logger.DebugFormat("host:{0}", host);
            Logger.DebugFormat("port:{0}", port);

            // ロギング
            Logger.Debug("<<<<= SshClientLibrary::SshClientLibrary(string, int)");
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibrary::Dispose(bool)");

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
            Logger.Debug("<<<<= SshClientLibrary::Dispose(bool)");
        }
        #endregion  

        #region 接続(同期)
        /// <summary>
        /// 接続(同期)
        /// </summary>
        public override bool Connect()
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibrary::Connect()");

            // パスワード(プレーン)認証メソッドオブジェクト生成
            PasswordAuthenticationMethod passwordAuthenticationMethod =
                new PasswordAuthenticationMethod(UserInfo.Name, UserInfo.Password);

            // キーボードインタラクティブ認証メソッドオブジェクト生成
            KeyboardInteractiveAuthenticationMethod keyboardInteractiveAuthenticationMethod =
                new KeyboardInteractiveAuthenticationMethod(UserInfo.Name);

            // 認証メソッドオブジェクト登録
            m_AuthenticationMethodList.Add(passwordAuthenticationMethod);
            m_AuthenticationMethodList.Add(keyboardInteractiveAuthenticationMethod);

            // キーボードインタラクティブ認証プロンプト取得イベント設定
            keyboardInteractiveAuthenticationMethod.AuthenticationPrompt += delegate (object sender, Renci.SshNet.Common.AuthenticationPromptEventArgs e)
            {
                // プロンプト分繰り返す
                foreach (var prompt in e.Prompts)
                {
                    // パスワードプロンプトが含まれているか？
                    if (prompt.Request.IndexOf(ServerInfo.PasswordPrompt) != -1)
                    {
                        // 含まれている場合、応答にパスワードを設定する
                        prompt.Response = UserInfo.Password;
                    }
                }
            };

            // 接続情報生成
            m_ConnectionInfo = new ConnectionInfo(
                m_HostInfo.Host,
                m_HostInfo.Port,
                UserInfo.Name,
                m_AuthenticationMethodList.ToArray()
            );

            // タイムアウト設定
            m_ConnectionInfo.Timeout = Timeout.Connect;

            // エンコード設定
            m_ConnectionInfo.Encoding = ServerInfo.Encoding;

            // SSHクライアントオブジェクトを生成
            m_Client = new SshClient(m_ConnectionInfo);

            // ロギング
            Logger.Debug("<<<<= SshClientLibrary::Connect()");

            // 正常終了
            return true;
        }
        #endregion

        #region 切断(同期)
        /// <summary>
        /// 切断(同期)
        /// </summary>
        public override bool Disconnect()
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibrary::Disconnect()");

            // 切断
            m_Client?.Disconnect();
            m_Client?.Dispose();
            m_Client = null;

            // ロギング
            Logger.Debug("<<<<= SshClientLibrary::Disconnect()");

            // 正常終了
            return true;
        }
        #endregion

        #region ログイン(同期)
        /// <summary>
        /// ログイン(同期)
        /// </summary>
        /// <returns></returns>
        public bool Login()
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibrary::Login()");

            // 接続
            m_Client.Connect();

            // 接続状態判定
            if (!m_Client.IsConnected)
            {
                // ロギング
                Logger.ErrorFormat("接続に失敗しました:[{0}:{1}]", m_HostInfo.Host, m_HostInfo.Port);

                // ログイン状態設定
                IsLogin = false;

                // 異常終了
                return false;
            }

            // TODO:ShellStreamオブジェクト取得
            m_ShellStream = m_Client.CreateShellStream(
                                        m_ConnectionInfo.Host,
                                        ShellStreamInfo.Columns,
                                        ShellStreamInfo.Rows,
                                        ShellStreamInfo.Witdh,
                                        ShellStreamInfo.Hight,
                                        ShellStreamInfo.BufferSize);

            // プロンプト受信待ち
            Expect(ServerInfo.Prompt);

            // ログイン状態設定
            IsLogin = true;

            // ロギング
            Logger.Debug("<<<<= SshClientLibrary::Login()");

            // 正常終了
            return true;
        }
        #endregion

        #region ログアウト(同期)
        /// <summary>
        /// ログアウト(同期)
        /// </summary>
        /// <returns></returns>
        public bool Logout()
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibrary::Logout()");

            // 切断
            Disconnect();

            // ロギング
            Logger.Debug("<<<<= SshClientLibrary::Logout()");

            // 正常終了
            return true;
        }
        #endregion

        #region 読込(同期)
        /// <summary>
        /// 読込(同期)
        /// </summary>
        /// <returns></returns>
        public override string Read()
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibrary::Read()");

            // 受信データ待ち
            if (ReadWait() < 0)
            {
                // ロギング
                Logger.Warn("読込失敗(キャンセル受信):[SshClientLibrary::Read()]");
                Logger.Debug("<<<<= SshClientLibrary::Read()");

                // 返却
                return string.Empty;
            }

            // 文字列読込
            string readStream = ReadStream();

           // ロギング
            Logger.Debug("<<<<= SshClientLibrary::Read()");

            // 返却
            return readStream;
        }

        #region 受信待ち
        /// <summary>
        /// 受信待ち
        /// </summary>
        /// <returns>-1:キャンセル受信</returns>
        private long ReadWait()
        {
            // 読込待ち
            do
            {
                // キャンセル判定
                if (m_CancellationTokenSource.IsCancellationRequested)
                {
                    // 返却
                    return -1;
                }

                // Wait(100msec)
                Task.Delay(100, m_CancellationTokenSource.Token);
            }
            while (m_ShellStream.Length == 0);

            // 返却
            return m_ShellStream.Length;
        }
        #endregion

        #region Stream読込
        /// <summary>
        /// Stream読込
        /// </summary>
        /// <returns></returns>
        private string ReadStream()
        {
            // 文字列読込
            string str = m_ShellStream.Read();

            // 文字列判定
            if (str == null || str.Length == 0)
            {
                // 返却
                return string.Empty;
            }

            // 受信内容表示
            Logger.InfoFormat("受信内容：【{0}:{1}】[受信サイズ:{2}]\r\n{3}",
                m_HostInfo.Host,
                m_HostInfo.Port,
                str.Length,
                Dump.ToString(str, Encoding));

            // 文字コード変換
            string encordingString = CommonLibrary.EncordingString(ServerInfo.Encoding, ServerInfo.Encoding, str);

            // 改行コード変換
            string result = CommonLibrary.ConvertReturnCode(encordingString);

            // 返却
            return result;
        }
        #endregion
        #endregion

        #region 改行付与書込(同期)
        /// <summary>
        /// 書込(同期)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public override bool Write(string str)
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibrary::Write(string)");
            Logger.DebugFormat("str :{0}", str);

            // 書込
            m_ShellStream.Write(str);

            // ShellStreamフラッシュ
            m_ShellStream.Flush();

            // 送信内容表示
            Logger.InfoFormat("送信内容：【{0}:{1}】\r\n{2}",
                m_HostInfo.Host,
                m_HostInfo.Port,
                Dump.ToString(str, Encoding));

            // ロギング
            Logger.Debug("<<<<= SshClientLibrary::Write(string)");

            // 正常終了
            return true;
        }
        #endregion

        #region 改行付与書込(同期)
        /// <summary>
        /// 改行付与書込(同期)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public override bool WriteLine(string str)
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibrary::WriteLine(string)");
            Logger.DebugFormat("str :{0}", str);

            // 書込
            m_ShellStream.WriteLine(str);

            // ShellStreamフラッシュ
            m_ShellStream.Flush();

            // 送信内容表示
            Logger.InfoFormat("送信内容：【{0}:{1}】\r\n{2}",
                m_HostInfo.Host,
                m_HostInfo.Port,
                Dump.ToString(str, Encoding));

            // ロギング
            Logger.Debug("<<<<= SshClientLibrary::WriteLine(string)");

            // 正常終了
            return true;
        }
        #endregion

        #region 結果待ち(同期)
        /// <summary>
        /// 結果待ち(同期)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public StringBuilder Expect(string text)
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibrary::Expect(string)");
            Logger.DebugFormat("text:{0}", text);

            // 結果オブジェクト
            StringBuilder result = new StringBuilder();

            // 一致するまで繰り返し
            while (true)
            {
                // 読込
                string resultRecive = Read();

                // 結果格納
                result.Append(resultRecive);

                // 文字列比較
                Regex regex = new Regex(text, RegexOptions.Compiled | RegexOptions.Multiline);
                if (regex.IsMatch(resultRecive.ToString()))
                {
                    // 一致したので終了
                    break;
                }
            }

            // ロギング
            Logger.Debug(result.ToString());
            Logger.Debug("<<<<= SshClientLibrary::Expect(string)");

            // 結果返却
            return result;
        }
        #endregion

        #region コマンド実行
        /// <summary>
        /// コマンド実行
        /// </summary>
        /// <param name="command"></param>
        public StringBuilder Excexute(string command)
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibrary::Excexute(string)");
            Logger.DebugFormat("command:{0}", command);

            // コマンド送信
            WriteLine(command);

            // 結果待ち
            StringBuilder result = new StringBuilder();

            // コマンド
            string tmpStr = Expect(ServerInfo.Prompt).ToString();

            // コマンドライン正規表現置換オブジェクト生成
            var regexCmdLineReplaceStr = "^.*" + Regex.Escape(command) + @"[\r\n]*";
            var regexCmdLineReplace = new Regex(regexCmdLineReplaceStr, RegexOptions.Multiline);

            // コマンドライン正規表現置換
            tmpStr = regexCmdLineReplace.Replace(tmpStr, "");

            // プロンプト正規表現置換オブジェクト生成
            var regexPromptReplaceStr = "^.*" + Regex.Escape(ServerInfo.Prompt) + @"[\r\n]*$";
            var regexPromptReplace = new Regex(regexPromptReplaceStr, RegexOptions.Multiline);

            // プロンプト正規表現置換
            tmpStr = regexPromptReplace.Replace(tmpStr, "");

            // 結果登録
            result.Append(tmpStr);

            // ロギング
            Logger.Debug(result.ToString());
            Logger.Debug("<<<<= SshClientLibrary::Excexute(string)");

            // 結果返却
            return result;
        }
        #endregion
    }
}
