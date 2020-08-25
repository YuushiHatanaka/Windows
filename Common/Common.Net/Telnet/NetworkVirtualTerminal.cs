using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Common.Net
{
    /// <summary>
    /// 仮想端末クラス
    /// </summary>
    public class NetworkVirtualTerminal
    {
        #region 送信文字コード
        /// <summary>
        /// 送信文字コード
        /// </summary>
        private Encoding m_SendEncoding;

        /// <summary>
        /// 送信文字コード
        /// </summary>
        public Encoding SendEncoging { get { return this.m_SendEncoding; } set { this.m_SendEncoding = value; } }
        #endregion

        #region 受信文字コード
        /// <summary>
        /// 受信文字コード
        /// </summary>
        private Encoding m_RecvEncoding;
 
        /// <summary>
        /// 受信文字コード
        /// </summary>
        public Encoding RecvEncodin { get { return this.m_SendEncoding; } set { this.m_RecvEncoding = value; } }
        #endregion

        #region 端末サイズ
        /// <summary>
        /// 端末サイズ
        /// </summary>
        private Size m_TerminalSize;

        /// <summary>
        /// 端末サイズ
        /// </summary>
        public Size TerminalSize { get { return this.TerminalSize; } set { this.m_TerminalSize = value; } }
        #endregion

        #region ローカルエコー
        /// <summary>
        /// ローカルエコー
        /// </summary>
        private bool m_LocalEcho;

        /// <summary>
        /// ローカルエコー
        /// </summary>
        public bool LocalEcho { get { return this.m_LocalEcho; } set { this.m_LocalEcho = value; } }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NetworkVirtualTerminal()
        {
            // デフォルト設定
            this.m_SendEncoding = Encoding.GetEncoding("UTF-8");
            this.m_RecvEncoding = Encoding.GetEncoding("UTF-8");
            this.m_TerminalSize.Width = 80;
            this.m_TerminalSize.Height = 32;
            this.m_LocalEcho = false;
        }

        #region ネゴシエーション
        /// <summary>
        /// ネゴシエーション
        /// </summary>
        public byte[] Negotiation()
        {
            // ネゴシエーション用MemoryStream
            MemoryStream _NegotiationMemoryStream = new MemoryStream();

            // MemorStreamを返却する
            return _NegotiationMemoryStream.ToArray();
        }
        #endregion
    }
}
