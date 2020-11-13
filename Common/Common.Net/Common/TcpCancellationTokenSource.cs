using System.Threading;

namespace Common.Net
{
    /// <summary>
    /// TCPタスクキャンセルトークンクラス
    /// </summary>
    public class TcpCancellationTokenSource
    {
        /// <summary>
        /// 接続
        /// </summary>
        public CancellationTokenSource Connect = null;

        /// <summary>
        /// 受信
        /// </summary>
        public CancellationTokenSource Recv = null;

        /// <summary>
        /// 送信
        /// </summary>
        public CancellationTokenSource Send = null;

        /// <summary>
        /// 切断
        /// </summary>
        public CancellationTokenSource Disconnect = null;
    }
}
