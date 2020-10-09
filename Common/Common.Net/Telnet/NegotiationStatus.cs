namespace Common.Net
{
    #region ネゴシエーション状態Templateクラス
    /// <summary>
    /// ネゴシエーション状態クラス
    /// </summary>
    public class NegotiationStatus
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
        public NegotiationStatus()
        {
            this.Status = TelnetOptionStatus.No;
            this.Queue = TelnetOptionQueue.Empty;
        }
        #endregion
    };
    #endregion
}
