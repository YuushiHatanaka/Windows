using System.IO;
using System.Text;

namespace Common.Net
{
    /// <summary>
    /// UdpClientSendEventArgsクラス
    /// </summary>
    public class UdpClientSendEventArgs : UdpClientEventArgs
    {
        /// <summary>
        /// 送信MemoryStream
        /// </summary>
        public MemoryStream Stream = new MemoryStream();

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UdpClientSendEventArgs()
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
            result.AppendFormat("└ Stream : {0}\n", Stream.Length);

            // 返却
            return result.ToString();
        }
        #endregion
    }
}