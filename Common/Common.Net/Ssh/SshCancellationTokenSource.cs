using System.Threading;

namespace Common.Net
{
    /// <summary>
    /// SSHタスクキャンセルトークンクラス
    /// </summary>
    public class SshCancellationTokenSource
    {
        /// <summary>
        /// ログイン
        /// </summary>
        public CancellationTokenSource Login = null;

        /// <summary>
        /// ログアウト
        /// </summary>
        public CancellationTokenSource Logout = null;

        /// <summary>
        /// 文字列送信
        /// </summary>
        public CancellationTokenSource WriteLine = null;

        /// <summary>
        /// コマンド実行
        /// </summary>
        public CancellationTokenSource Execute = null;

        /// <summary>
        /// 結果待ち
        /// </summary>
        public CancellationTokenSource Expect = null;
    }
}
