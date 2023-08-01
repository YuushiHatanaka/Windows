using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// InterfaceTcpClientLibraryインタフェース
    /// </summary>
    public interface InterfaceTcpClientLibrary
    {
        #region ログイン
        #region ログイン(同期)
        /// <summary>
        /// ログイン(同期)
        /// </summary>
        /// <returns></returns>
        bool Login();
        #endregion
        #endregion

        #region ログアウト
        #region ログアウト(同期)
        /// <summary>
        /// ログアウト(同期)
        /// </summary>
        /// <returns></returns>
        bool Logout();
        #endregion
        #endregion
    }
}
