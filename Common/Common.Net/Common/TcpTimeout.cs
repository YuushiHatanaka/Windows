using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// TCPタイムアウトクラス
    /// </summary>
    public class TcpTimeout
    {
        #region 接続
        /// <summary>
        /// 接続
        /// </summary>
        private int m_Connect = 10000;

        /// <summary>
        /// 接続
        /// </summary>
        public int Connect { get { return this.m_Connect; } set { this.m_Connect = value; } }
        #endregion

        #region 受信
        /// <summary>
        /// 受信
        /// </summary>
        private int m_Recv = 10000;

        /// <summary>
        /// 受信タイムアウト
        /// </summary>
        public int Recv { get { return this.m_Recv; } set { this.m_Recv = value; } }
        #endregion

        #region 送信
        /// <summary>
        /// 送信
        /// </summary>
        private int m_Send = 10000;

        /// <summary>
        /// 送信
        /// </summary>
        public int Send { get { return this.m_Send; } set { this.m_Send = value; } }
        #endregion

        #region 切断
        /// <summary>
        /// 切断
        /// </summary>
        private int m_Disconnect = 10000;

        /// <summary>
        /// 切断
        /// </summary>
        public int Disconnect { get { return this.m_Disconnect; } set { this.m_Disconnect = value; } }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TcpTimeout()
        {
        }
        #endregion
    }
}
