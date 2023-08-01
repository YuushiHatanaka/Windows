using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// FtpClientDataConnectionクラス
    /// </summary>
    public class FtpClientDataConnection : IDisposable
    {
        #region 破棄フラグ
        /// <summary>
        /// 破棄フラグ
        /// </summary>
        private bool m_Disposed = false;
        #endregion

        #region 転送モード
        /// <summary>
        /// 転送モード
        /// </summary>
        public FtpTransferMode Mode = FtpTransferMode.Active;
        #endregion

        #region IPアドレス
        /// <summary>
        /// IPアドレス
        /// </summary>
        public string IpAddress = string.Empty;
        #endregion

        #region ポート番号
        /// <summary>
        /// ポート番号
        /// </summary>
        public int Port;
        #endregion

        #region データ転送用ソケット
        /// <summary>
        /// データ転送用ソケット
        /// </summary>
        public Socket Socket = null;
        #endregion

        #region データ書込み用FileStream
        /// <summary>
        /// データ書込み用FileStream
        /// </summary>
        public FileStream FileStream = null;
        #endregion

        #region TCPリスナー
        /// <summary>
        /// TCPリスナー
        /// </summary>
        public TcpListener Listener = null;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FtpClientDataConnection()
        {
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~FtpClientDataConnection()
        {
            // 破棄
            Dispose(false);
        }
        #endregion

        #region 破棄
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 破棄
        /// </summary>
        /// <param name="isDisposing"></param>
        protected virtual void Dispose(bool isDisposing)
        {
            // 破棄しているか？
            if (!m_Disposed)
            {
                // 未実装(アンマネージドリソース解放)
                if (Socket != null && Socket.Connected)
                {
                    Socket.Dispose();
                }

                // マネージドリソース解放
                if (isDisposing)
                {
                }

                // 破棄済みを設定
                m_Disposed = true;
            }
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
            result.AppendFormat("　Mode       : {0}\n", Mode.ToString());
            result.AppendFormat("　IpAddress  : {0}\n", IpAddress);
            result.AppendFormat("　Port       : {0}\n", Port.ToString());
            result.AppendFormat("　Listener   : {0}\n", Listener);
            result.AppendFormat("　Socket     : {0}\n", Socket);
            if (Socket != null)
            {
                result.AppendFormat("　└ Connected : {0}\n", Socket.Connected);
            }

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
