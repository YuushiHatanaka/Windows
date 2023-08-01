using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// UserInfoクラス
    /// </summary>
    public class UserInfo
    {
        #region ユーザ名
        /// <summary>
        /// ユーザ名
        /// </summary>
        public string Name { get; set; } = string.Empty;
        #endregion

        #region パスワード(平分)
        /// <summary>
        /// パスワード(平分)
        /// </summary>
        public string Password { get; set; } = string.Empty;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UserInfo()
        {
        }
        #endregion
    }
}
