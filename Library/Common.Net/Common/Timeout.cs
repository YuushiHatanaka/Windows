using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// Timeoutクラス
    /// </summary>
    public class Timeout
    {
        #region タイムアウト値
        #region 接続
        /// <summary>
        /// 接続
        /// </summary>
        public TimeSpan Connect { get; set; } = new TimeSpan(0, 0, 0, 10, 0);
        #endregion

        #region 切断
        /// <summary>
        /// 切断
        /// </summary>
        public TimeSpan Disconnect { get; set; } = new TimeSpan(0, 0, 0, 10, 0);
        #endregion

        #region 送信
        /// <summary>
        /// 送信
        /// </summary>
        public TimeSpan Send { get; set; } = new TimeSpan(0, 0, 0, 10, 0);
        #endregion

        #region 受信
        /// <summary>
        /// 受信
        /// </summary>
        public TimeSpan Recive { get; set; } = new TimeSpan(0, 0, 0, 10, 0);
        #endregion

        #region キャンセル
        /// <summary>
        /// キャンセル
        /// </summary>
        public TimeSpan Cancel { get; set; } = new TimeSpan(0, 0, 0, 10, 0);
        #endregion
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Timeout()
        {
        }
        #endregion
    }
}
