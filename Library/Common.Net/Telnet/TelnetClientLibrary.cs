using Common.Diagnostics;
using PrimS.Telnet;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Common.Net
{
    /// <summary>
    /// TelnetClientLibraryクラス
    /// </summary>
    public partial class TelnetClientLibrary : TcpClientLibrary, InterfaceTcpClientLibrary
    {
        #region ログイン状態
        /// <summary>
        /// ログイン状態
        /// </summary>
        public bool IsLogin { get; protected set; }
        #endregion

        #region クライアントオブジェクト
        /// <summary>
        /// クライアントオブジェクト
        /// </summary>
        private Client m_Client = null;
        #endregion

        #region ユーザ情報
        /// <summary>
        /// ユーザ情報
        /// </summary>
        public UserInfo UserInfo { get; set; } = new UserInfo();
        #endregion

        #region ユーザ情報
        /// <summary>
        /// ユーザ情報
        /// </summary>
        public ServerInfo ServerInfo { get; set; } = new ServerInfo();
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        public TelnetClientLibrary(string host)
            : base(host, 23)
        {
            // ロギング
            Logger.Debug("=>>>> TelnetClientLibrary::TelnetClientLibrary(string)");
            Logger.DebugFormat("host:{0}", host);

            // ロギング
            Logger.Debug("<<<<= TelnetClientLibrary::TelnetClientLibrary(string)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public TelnetClientLibrary(string host, int port)
            : base(host, port)
        {
            // ロギング
            Logger.Debug("=>>>> TelnetClientLibrary::TelnetClientLibrary(string, int)");
            Logger.DebugFormat("host:{0}", host);
            Logger.DebugFormat("port:{0}", port);

            // ロギング
            Logger.Debug("<<<<= TelnetClientLibrary::TelnetClientLibrary(string, int)");
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
            Logger.Debug("=>>>> TelnetClientLibrary::Dispose(bool)");

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
            Logger.Debug("<<<<= TelnetClientLibrary::Dispose(bool)");
        }
        #endregion

        #region 接続(同期)
        /// <summary>
        /// 接続(同期)
        /// </summary>
        public override bool Connect()
        {
            // ロギング
            Logger.Debug("=>>>> TelnetClientLibrary::Connect()");

            // Telnetクライアントオブジェクトを生成
            m_Client = new Client(m_HostInfo.Host, m_HostInfo.Port, new CancellationToken());

            // 接続状態判定
            if (!m_Client.IsConnected)
            {
                // ロギング
                Logger.ErrorFormat("接続に失敗しました:[{0}]", m_HostInfo.ToString());

                // 異常終了
                return false;
            }

            // ロギング
            Logger.Debug("<<<<= TelnetClientLibrary::Connect()");

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
            Logger.Debug("=>>>> TelnetClientLibrary::Disconnect()");

            // 切断
            m_Client?.Dispose();
            m_Client = null;

            // ロギング
            Logger.Debug("<<<<= TelnetClientLibrary::Disconnect()");

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
            Logger.Debug("=>>>> TelnetClientLibrary::Login()");

            // ログインプロンプト結果待ち
            Expect(ServerInfo.LoginPrompt);

            // ログインユーザ名を送信
            WriteLine(UserInfo.Name);

            // パスワードプロンプト結果待ち
            Expect(ServerInfo.PasswordPrompt);

            // ログインパスワードを送信
            WriteLine(UserInfo.Password);

            // コマンドラインプロンプト結果待ち
            Expect(ServerInfo.Prompt);

            // ログイン状態設定
            IsLogin = true;

            // ロギング
            Logger.Debug("<<<<= TelnetClientLibrary::Login()");

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
            Logger.Debug("=>>>> TelnetClientLibrary::Logout()");

            // 切断
            Disconnect();

            // ロギング
            Logger.Debug("<<<<= TelnetClientLibrary::Logout()");

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
            Logger.Debug("=>>>> TelnetClientLibrary::Read()");

            // 受信
            string readStream = ReadStream();

            // 受信データ待ち
            if (readStream == null)
            {
                // ロギング
                Logger.Warn("読込失敗(キャンセル受信):[TelnetClientLibrary::Read()]");
                Logger.Debug("<<<<= TelnetClientLibrary::Read()");

                // 返却
                return string.Empty;
            }

            // 受信内容表示
            Logger.InfoFormat("受信内容：【{0}:{1}】[受信サイズ:{2}]\r\n{3}",
                m_HostInfo.Host,
                m_HostInfo.Port,
                readStream.Length,
                Dump.ToString(readStream, Encoding));

            // 文字コード変換
            string encordingString = CommonLibrary.EncordingString(ServerInfo.Encoding, ServerInfo.Encoding, readStream);

            // 改行コード変換
            string result = CommonLibrary.ConvertReturnCode(encordingString);

            // 返却
            return result;
        }

        #region Stream読込
        /// <summary>
        /// Stream読込
        /// </summary>
        /// <returns></returns>
        private string ReadStream()
        {
            Task<string> task = null;

            // 読込待ち
            do
            {
                // キャンセル判定
                if (m_CancellationTokenSource.IsCancellationRequested)
                {
                    // 返却
                    return null;
                }

                // 受信
                task = m_Client.ReadAsync();

                // Wait(100msec)
                Task.Delay(100, m_CancellationTokenSource.Token);
            }
            while (task.Result == null || task.Result.Length == 0);

            // 返却
            return task.Result;
        }
        #endregion
        #endregion

        #region 書込(同期)
        /// <summary>
        /// 書込(同期)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public override bool Write(string str)
        {
            // ロギング
            Logger.Debug("=>>>> TelnetClientLibrary::Write(string)");
            Logger.DebugFormat("str :{0}", str);

            // 送信
            Task task = m_Client.WriteAsync(str);

            // Task待ち合わせ
            task.Wait();

            // 送信内容表示
            Logger.InfoFormat("送信内容：【{0}:{1}】\r\n{2}",
                m_HostInfo.Host,
                m_HostInfo.Port,
                Dump.ToString(str, Encoding));

            // ロギング
            Logger.Debug("<<<<= TelnetClientLibrary::Write(string)");

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
            Logger.Debug("=>>>> TelnetClientLibrary::WriteLine(string)");
            Logger.DebugFormat("str :{0}", str);

            // 送信
            Task task = m_Client.WriteLineAsync(str);

            // Task待ち合わせ
            task.Wait();

            // 送信内容表示
            Logger.InfoFormat("送信内容：【{0}:{1}】\r\n{2}",
                m_HostInfo.Host,
                m_HostInfo.Port,
                Dump.ToString(str, Encoding));

            // ロギング
            Logger.Debug("<<<<= TelnetClientLibrary::WriteLine(string)");

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
            Logger.Debug("=>>>> TelnetClientLibrary::Expect(string)");
            Logger.DebugFormat("text:{0}", text);

            // 結果オブジェクト
            StringBuilder result = new StringBuilder();

            // 一致するまで繰り返し
            while (true)
            {
                // 文字コード変換
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
            Logger.Debug("<<<<= TelnetClientLibrary::Expect(string)");

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
            Logger.Debug("=>>>> TelnetClientLibrary::Excexute(string)");
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
            Logger.Debug("<<<<= TelnetClientLibrary::Excexute(string)");

            // 結果返却
            return result;
        }
        #endregion
    }
}
