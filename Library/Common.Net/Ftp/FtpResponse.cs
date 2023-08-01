using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// FtpResponseクラス
    /// </summary>
    public class FtpResponse
    {
        #region 実行結果
        /// <summary>
        /// 実行結果
        /// </summary>
        public bool Result = true;
        #endregion

        #region ステータスコード
        /// <summary>
        /// ステータスコード
        /// </summary>
        public int StatusCode = 0;
        #endregion

        #region ステータス詳細
        /// <summary>
        /// ステータス詳細
        /// </summary>
        public string StatusDetail = string.Empty;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FtpResponse()
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
            // 返却
            return string.Format("{0} {1}", StatusCode, StatusDetail);
        }
        #endregion
    }
}
