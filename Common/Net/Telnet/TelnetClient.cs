using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Net;

namespace Common.Net
{
    /// <summary>
    /// Telnetクライアントクラス
    /// </summary>
    public class TelnetClient
    {
        /// <summary>
        /// サーバエンドポイント
        /// </summary>
        public IPEndPoint m_IPEndPoint = null;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="hostName"></param>
        public TelnetClient(string hostName)
        {
            IPAddress hostAddress = IPAddress.Parse(hostName);
            this.m_IPEndPoint = new IPEndPoint(hostAddress, 23);
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~TelnetClient()
        {
            // 破棄
            this.Dispose(false);
        }
        #endregion

        #region 破棄
        /// <summary>
        /// 破棄フラグ
        /// </summary>
        private bool m_Disposed = false;

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 破棄
        /// </summary>
        /// <param name="isDisposing"></param>
        protected virtual void Dispose(bool isDisposing)
        {
            // 破棄しているか？
            if (!this.m_Disposed)
            {
                // TODO:未実装(アンマネージドリソース解放)

                // TODO:未実装(マネージドリソース解放)
                if (isDisposing)
                {
                }

                // 破棄済みを設定
                this.m_Disposed = true;
            }
        }
        #endregion
    }
}
