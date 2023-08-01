using System.Text;

namespace Common.Net
{
    /// <summary>
    /// SshClientCommandExecuteEventArgsクラス
    /// </summary>
    public class SshClientCommandExecuteEventArgs : TcpClientEventArgs
    {
        #region コマンド文字列
        /// <summary>
        /// コマンド文字列
        /// </summary>
        public string Command { get; set; } = string.Empty;
        #endregion

        #region コマンド実行結果
        /// <summary>
        /// コマンド実行結果
        /// </summary>
        public StringBuilder ExecuteResult = null;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SshClientCommandExecuteEventArgs()
            : base()
        {
        }
        #endregion

        #region 文字列化
        /// <summary>
        /// 文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // 結果オブジェクト生成
            StringBuilder result = new StringBuilder();

            // 文字列作成
            result.AppendFormat(base.ToString());
            result.AppendFormat("└ Command      : {0}\n", Command);
            if (ExecuteResult != null)
            {
                result.AppendFormat("└ ExecuteResult:\n{0}\n", ExecuteResult.ToString());
            }
            else
            {
                result.AppendFormat("└ ExecuteResult:[なし]\n");
            }

            // 返却
            return result.ToString();
        }
        #endregion
    }
}