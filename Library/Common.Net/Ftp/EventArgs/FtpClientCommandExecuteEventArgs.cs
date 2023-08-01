using System.Text;

namespace Common.Net
{
    /// <summary>
    /// FtpClientCommandExecuteEventArgsクラス
    /// </summary>
    public class FtpClientCommandExecuteEventArgs : TcpClientEventArgs
    {
        #region コマンド種別
        /// <summary>
        /// コマンド種別
        /// </summary>
        public FtpCommand CommandType { get; set; } = FtpCommand.NONE;
        #endregion

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
        public object ExecuteResult = null;
        #endregion

        #region FTP応答
        /// <summary>
        /// FTP応答
        /// </summary>
        public FtpResponse Response = null;
        #endregion

        #region 受信結果
        /// <summary>
        /// 受信結果
        /// </summary>
        public FtpClientReciveStream reciveStream { get; set; } = null;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FtpClientCommandExecuteEventArgs()
            : base()
        {
        }
        #endregion
    }
}