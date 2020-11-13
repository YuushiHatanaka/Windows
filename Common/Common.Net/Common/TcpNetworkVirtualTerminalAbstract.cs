using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// TCP仮想端末クラス
    /// </summary>
    public class TcpNetworkVirtualTerminal
    {
        #region 文字コード(Local)
        /// <summary>
        /// 文字コード(Local)
        /// </summary>
        protected Encoding m_LocalEncoding;

        /// <summary>
        /// 文字コード(Local)
        /// </summary>
        public Encoding LocalEncoding { get { return this.m_LocalEncoding; } set { this.m_LocalEncoding = value; } }
        #endregion

        #region 文字コード(Remote)
        /// <summary>
        /// 文字コード(Remote)
        /// </summary>
        protected Encoding m_RemoteEncoding;

        /// <summary>
        /// 文字コード(Remote)
        /// </summary>
        public Encoding RemoteEncoding { get { return this.m_RemoteEncoding; } set { this.m_RemoteEncoding = value; } }
        #endregion

        #region 改行コード(送信)
        /// <summary>
        /// 改行コード(送信)
        /// </summary>
        protected string m_WriteNewLine = string.Empty;

        /// <summary>
        /// 改行コード(送信)
        /// </summary>
        public string WriteNewLine { get { return this.m_WriteNewLine; } set { this.m_WriteNewLine = value; } }
        #endregion

        #region 改行コード(受信)
        /// <summary>
        /// 改行コード(受信)
        /// </summary>
        protected string m_ReadNewLine = string.Empty;

        /// <summary>
        /// 改行コード(受信)
        /// </summary>
        public string ReadNewLine { get { return this.m_ReadNewLine; } set { this.m_ReadNewLine = value; } }
        #endregion

        #region 端末種別
        /// <summary>
        /// 端末種別
        /// </summary>
        protected string m_TerminalType = "vt100";

        /// <summary>
        /// 端末種別
        /// </summary>
        public string TerminalType { get { return this.m_TerminalType; } set { this.m_TerminalType = value; } }
        #endregion

        #region 端末サイズ
        /// <summary>
        /// 端末サイズ
        /// </summary>
        protected Size m_Size = new Size() { Width = 80, Height = 40 };

        /// <summary>
        /// 端末サイズ
        /// </summary>
        public Size Size { get { return this.m_Size; } set { this.m_Size = value; } }
        #endregion
    }
}
