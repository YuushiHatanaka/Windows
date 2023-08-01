using System;
using System.Text;

namespace Common.Net
{
    /// <summary>
    /// FtpClientLoginEventArgsクラス
    /// </summary>
    public class FtpClientLoginEventArgs : TcpClientEventArgs
    {
        #region ユーザ名
        /// <summary>
        /// ユーザ名
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FtpClientLoginEventArgs()
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
            result.AppendFormat("└ UserName: {0}\n", UserName);

            // 返却
            return result.ToString();
        }
        #endregion
    }
}