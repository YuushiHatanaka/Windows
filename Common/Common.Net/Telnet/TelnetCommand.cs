namespace Common.Net
{
    #region Telnetコマンド
    /// <summary>
    /// Telnetコマンド
    /// </summary>
    public enum TelnetCommand : byte
    {
        /// <summary>
        /// 不明
        /// </summary>
        UN = 0x00,

        /// <summary>
        /// SE
        /// 副交渉の終わり 
        /// </summary>
        SE = 0xf0,

        /// <summary>
        /// NOP
        /// 無操作(Synch のデータストリーム部分)
        /// </summary>
        NOP = 0xf1,

        /// <summary>
        /// Data Mark
        /// (これは常に TCP Urgent 通知を伴う べきである)
        /// </summary>
        DM = 0xf2,

        /// <summary>
        /// Break
        /// NVT 文字 BRK 
        /// </summary>
        BRK = 0xf3,

        /// <summary>
        /// Interrupt Process
        /// IP 機能 
        /// </summary>
        IP = 0xf4,

        /// <summary>
        /// Abort output
        /// AO 機能
        /// </summary>
        AO = 0xf5,

        /// <summary>
        /// Are You There
        /// AYT 機能 
        /// </summary>
        AYT = 0xf6,

        /// <summary>
        /// Erase character
        /// EC 機能 
        /// </summary>
        EC = 0xf7,

        /// <summary>
        /// Erase Line
        /// EL 機能
        /// </summary>
        EL = 0xf8,

        /// <summary>
        /// Go ahead
        /// GA シグナル
        /// </summary>
        GA = 0xf9,

        /// <summary>
        /// SB
        /// (後に続くのが示されたオプションの副 交渉であることを表す)
        /// </summary>
        SB = 0xfa,

        /// <summary>
        /// WILL (オプションコード)
        /// (示されたオプションの実行開始、 または実行中かどうかの確認を望 むことを表す)
        /// </summary>
        WILL = 0xfb,

        /// <summary>
        /// WON'T (オプションコード)
        /// (示されたオプションの実行拒否ま たは継続実行拒否を表す)
        /// </summary>
        WONT = 0xfc,

        /// <summary>
        /// DO (オプションコード)
        /// (示されたオプションを実行すると いう相手側の要求、またはあなた がそれを実行することを期待して いるという確認を表す)
        /// </summary>
        DO = 0xfd,

        /// <summary>
        /// DO (オプションコード)
        /// (示されたオプションを停止すると いう相手側の要求、またはあなた がそれを実行することをもはや期 待しないという確認を表す )
        /// </summary>
        DONT = 0xfe,

        /// <summary>
        /// IAC
        /// (データバイト)
        /// </summary>
        IAC = 0xff,
    };
    #endregion
}
