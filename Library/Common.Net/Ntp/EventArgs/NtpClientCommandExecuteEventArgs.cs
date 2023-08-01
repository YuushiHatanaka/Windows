namespace Common.Net
{
    /// <summary>
    /// NtpClientCommandExecuteEventArgsクラス
    /// </summary>
    public class NtpClientCommandExecuteEventArgs : UdpClientEventArgs
    {
        #region コマンド実行結果
        /// <summary>
        /// コマンド実行結果
        /// </summary>
        public NtpPacket ExecuteResult = null;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NtpClientCommandExecuteEventArgs()
        {
        }
        #endregion
    }
}