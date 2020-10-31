using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// SSH仮想端末クラス
    /// </summary>
    public class SshNetworkVirtualTerminal : TcpNetworkVirtualTerminal
    {
        #region サイズ(Pixel)
        /// <summary>
        /// サイズ(Pixel)
        /// </summary>
        protected Size m_Pixel = new Size() { Width = 800, Height = 400 };

        /// <summary>
        /// サイズ(Pixel)
        /// </summary>
        public Size Pixel { get { return this.m_Pixel; } set { this.m_Pixel = value; } }
        #endregion

        #region サイズ(Pixel)
        /// <summary>
        /// ShellStreamバッファサイズ
        /// </summary>
        protected int m_ShellStreamBufferSize = 4096;

        /// <summary>
        /// ShellStreamバッファサイズ
        /// </summary>
        public int ShellStreamBufferSize { get { return this.m_ShellStreamBufferSize; } set { this.m_ShellStreamBufferSize = value; } }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SshNetworkVirtualTerminal()
        {
            // 初期化
            this.Initialization();
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~SshNetworkVirtualTerminal()
        {
        }
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            // デフォルト設定
            this.m_LocalEncoding = Encoding.GetEncoding("UTF-8");
            this.m_RemoteEncoding = Encoding.GetEncoding("UTF-8");
            this.m_ReadNewLine = "\n";
            this.m_WriteNewLine = "\n";
        }
        #endregion
    }
}
