using System.IO;
using System.Text;

namespace Common.Net
{
    /// <summary>
    /// TcpClientSendEventArgsクラス
    /// </summary>
    public class TcpClientSendEventArgs : TcpClientEventArgs
    {
        /// <summary>
        /// 送信StringBuilder
        /// </summary>
        public StringBuilder Strings = new StringBuilder();

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TcpClientSendEventArgs()
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
            result.AppendFormat("└ Strings:\n{0}\n", Strings.ToString());

            // 返却
            return result.ToString();
        }
        #endregion
    }
}