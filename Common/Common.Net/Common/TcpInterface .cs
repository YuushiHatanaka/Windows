using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// TCPインターフェース
    /// </summary>
    public interface TcpInterface
    {
        #region ログイン
        /// <summary>
        /// ログイン
        /// </summary>
        /// <returns></returns>
        string Login();

        /// <summary>
        /// ログアウト
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        string Login(int timeout);
        #endregion

        #region ログアウト
        /// <summary>
        /// ログアウト
        /// </summary>
        /// <returns></returns>
        void Logout();

        /// <summary>
        /// ログアウト
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        void Logout(int timeout);
        #endregion

        #region 文字列送信
        /// <summary>
        /// 文字列送信
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        void Write(string str);

        /// <summary>
        /// 文字列送信
        /// </summary>
        /// <param name="str"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        void Write(string str, int timeout);

        /// <summary>
        /// 文字列送信
        /// </summary>
        /// <param name="line"></param>
        void WriteLine(string line);

        /// <summary>
        /// 文字列送信
        /// </summary>
        /// <param name="line"></param>
        /// <param name="timeout"></param>
        void WriteLine(string line, int timeout);
        #endregion

        #region コマンド実行
        /// <summary>
        /// コマンド実行
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        string Execute(string command);

        /// <summary>
        /// コマンド実行
        /// </summary>
        /// <param name="command"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        string Execute(string command, int timeout);
        #endregion

        #region 結果待ち
        /// <summary>
        /// 結果待ち
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        string Expect(string text);

        /// <summary>
        /// 結果待ち
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        string Expect(string text, int timeout);
        #endregion
    }
}
