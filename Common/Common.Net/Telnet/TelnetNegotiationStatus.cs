namespace Common.Net
{
    #region TELNETネゴシエーション状態Templateクラス
    /// <summary>
    /// TELNETネゴシエーション状態クラス
    /// </summary>
    public class TelnetNegotiationStatus
    {
        /// <summary>
        /// アクセス開始
        /// </summary>
        public bool Accept = false;

        /// <summary>
        /// オプション状態
        /// </summary>
        public TelnetOptionStatus Status = new TelnetOptionStatus();

        /// <summary>
        /// オプションQueue
        /// </summary>
        public TelnetOptionQueue Queue = new TelnetOptionQueue();

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TelnetNegotiationStatus()
        {
            this.Status = TelnetOptionStatus.No;
            this.Queue = TelnetOptionQueue.Empty;
        }
        #endregion
    };
    #endregion
}
