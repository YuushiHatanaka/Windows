namespace Common.Net
{
    #region Telnetオプション
    /// <summary>
    /// Telnetオプション
    /// </summary>
    public enum TelnetOption : byte
    {
        /// <summary>
        /// 通常の7ビットデータではなく、8ビットバイナリとしてデータを受信する
        /// </summary>
        binary = 0x00,

        /// <summary>
        /// エコーバックを行う
        /// </summary>
        echo = 0x01,

        /// <summary>
        /// 送受信を切り替えるGO AHEADコマンドの送信を抑制する
        /// </summary>
        suppress_go_ahead = 0x03,

        /// <summary>
        /// Telnetオプション状態を送信する
        /// </summary>
        status = 0x05,

        /// <summary>
        /// コネクションの双方の同期を取る際に使用される
        /// </summary>
        timing_mark = 0x06,

        /// <summary>
        /// 端末タイプを送信する
        /// （クライアント側のみに対して有効）
        /// </summary>
        terminal_type = 0x18,

        /// <summary>
        /// 端末ウィンドウの行と列の数を送る
        /// （クライアント側のみに対して有効）
        /// </summary>
        window_size = 0x1f,

        /// <summary>
        /// 端末の送信速度と受信速度を送る
        /// （クライアント側のみに対して有効）
        /// </summary>
        terminal_speed = 0x20,

        /// <summary>
        /// フロー制御を行う
        /// </summary>
        remote_flow_control = 0x21,

        /// <summary>
        /// リアルラインモードにてデータを行単位で送る
        /// </summary>
        linemode = 0x22,

        /// <summary>
        /// 
        /// </summary>
        display_location = 0x23,

        /// <summary>
        /// 
        /// </summary>
        environment_variables = 0x24,

        /// <summary>
        /// 
        /// </summary>
        environment_option = 0x27,

        /// <summary>
        /// 最大値
        /// </summary>
        max = 0x28,
    };
    #endregion
}