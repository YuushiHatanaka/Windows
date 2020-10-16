using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace Common.Net
{
    /// <summary>
    /// Telnetクラス
    /// </summary>
    public class Telnet : NetworkVirtualTerminal
    {
        #region ログイン情報
        #region ユーザ名
        /// <summary>
        /// ユーザ名
        /// </summary>
        private string m_UserName = string.Empty;

        /// <summary>
        /// ユーザ名
        /// </summary>
        public string UserName { get { return this.m_UserName; } set { this.m_UserName = value; } }
        #endregion

        #region ユーザパスワード
        /// <summary>
        /// ユーザパスワード
        /// </summary>
        private string m_UserPassword = string.Empty;

        /// <summary>
        /// ユーザパスワード
        /// </summary>
        public string UserPassword { get { return this.m_UserPassword; } set { this.m_UserPassword = value; } }
        #endregion

        #region ログインプロンプト
        /// <summary>
        /// ログインプロンプト
        /// </summary>
        private string m_LoginPrompt = @"^login: ";

        /// <summary>
        /// ログインプロンプト
        /// </summary>
        public string LoginPrompt { get { return this.m_LoginPrompt; } set { this.m_LoginPrompt = value; } }
        #endregion

        #region パスワードプロンプト
        /// <summary>
        /// パスワードプロンプト
        /// </summary>
        private string m_PasswordPrompt = @"^Password:";

        /// <summary>
        /// パスワードプロンプト
        /// </summary>
        public string PasswordPrompt { get { return this.m_PasswordPrompt; } set { this.m_PasswordPrompt = value; } }
        #endregion

        #region コマンドプロンプト
        /// <summary>
        /// コマンドプロンプト
        /// </summary>
        private string m_CommandPrompt = @"\$ $";

        /// <summary>
        /// コマンドプロンプト
        /// </summary>
        public string CommandPrompt { get { return this.m_CommandPrompt; } set { this.m_CommandPrompt = value; } }
        #endregion
        #endregion

        #region ログアウト情報
        #region exitコマンド
        /// <summary>
        /// exitコマンド
        /// </summary>
        private string m_ExitCommand = "exit";

        /// <summary>
        /// ユーザ名
        /// </summary>
        public string ExitCommand { get { return this.m_ExitCommand; } set { this.m_ExitCommand = value; } }
        #endregion

        #region logoutメッセージ
        /// <summary>
        /// logoutメッセージ
        /// </summary>
        private string m_LogoutMsg = "exit";

        /// <summary>
        /// ユーザ名
        /// </summary>
        public string LogoutMsg { get { return this.m_LogoutMsg; } set { this.m_LogoutMsg = value; } }
        #endregion
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPasword"></param>
        public Telnet(string userName, string userPasword)
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
        public Telnet(string host, string userName, string userPasword)
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
        public Telnet(string host, int port, string userName, string userPasword)
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
        ~Telnet()
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
            StringBuilder read = null;                  // 読込用
            StringBuilder result = new StringBuilder(); // 結果用

            // 接続
            this.Connect();

            // ログインプロンプト受信待ち
            read = this.Read(this.m_LoginPrompt);
            if (read == null)
            {
                // 例外
                throw new TelnetException("ログインに失敗しました:[ログインプロンプト受信待ち]");
            }

            // 結果追加
            result.Append(read.ToString());

            // ユーザ名送信
            this.WriteLine(this.m_UserName);

            // パスワードプロンプト受信待ち
            read = this.Read(this.m_PasswordPrompt);
            if (read == null)
            {
                // 例外
                throw new TelnetException("ログインに失敗しました:[パスワードプロンプト受信待ち]");
            }

            // 結果追加
            result.Append(read.ToString());

            // パスワード送信
            this.WriteLine(this.m_UserPassword);

            // コマンドプロンプト待ち
            read = this.Read(this.m_CommandPrompt);
            if (read == null)
            {
                // 例外
                throw new TelnetException("ログインに失敗しました:[コマンドプロンプト待ち]");
            }

            // 結果追加
            result.Append(read.ToString());

            // 結果返却
            return result.ToString();
        }
        #endregion

        #region ログアウト
        /// <summary>
        /// ログアウト
        /// </summary>
        public string Logout()
        {
            StringBuilder read = null;                  // 読込用
            StringBuilder result = new StringBuilder(); // 結果用

            // exitコマンド送信
            this.WriteLine(this.m_ExitCommand);

            // 受信待ち
            read = this.Read(this.m_LogoutMsg, 100);
            if (read != null)
            {
                // 結果追加
                result.Append(read.ToString());
            }

            // 切断
            this.DisConnect();

            // 結果返却
            return result.ToString();
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
        #endregion

        #region 例外イベントハンドラ
        /// <summary>
        /// 例外イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExceptionEventHandler(object sender, NetworkVirtualTerminalExceptionEventArgs e)
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
}
