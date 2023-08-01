using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// ServerInfoクラス
    /// </summary>
    public class ServerInfo
    {
        #region ログインプロンプト
        /// <summary>
        /// ログインプロンプト
        /// </summary>
        public string LoginPrompt = "login: ";
        #endregion

        #region パスワードプロンプト
        /// <summary>
        /// パスワードプロンプト
        /// </summary>
        public string PasswordPrompt = "Password: ";
        #endregion

        #region プロンプト
        /// <summary>
        /// プロンプト
        /// </summary>
        public string Prompt = "$ ";
        #endregion

        #region 改行コード
        /// <summary>
        /// 改行コード
        /// </summary>
        public string NewLine = "\r\n";
        #endregion

        #region 文字エンコーディング
        /// <summary>
        /// 文字エンコーディング
        /// </summary>
        public Encoding Encoding = Encoding.GetEncoding("UTF-8");
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ServerInfo()
        {
        }
        #endregion
    }
}
