﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Net.Sockets;
using System.IO;

namespace Common.Net
{
    /// <summary>
    /// データコネクションクラス
    /// </summary>
    public class FtpClientDataConnection : IDisposable
    {
        /// <summary>
        /// 転送モード
        /// </summary>
        public FtpTransferMode Mode = FtpTransferMode.Active;

        /// <summary>
        /// IPアドレス
        /// </summary>
        public string IpAddress = string.Empty;

        /// <summary>
        /// ポート番号
        /// </summary>
        public int Port;

        /// <summary>
        /// データ転送用ソケット
        /// </summary>
        public Socket Socket = null;

        /// <summary>
        /// データ書込み用FileStream
        /// </summary>
        public FileStream FileStream = null;

        /// <summary>
        /// TCPリスナー
        /// </summary>
        public TcpListener Listener = null;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FtpClientDataConnection()
        {
            Trace.WriteLine("FtpClientDataConnection::FtpClientDataConnection()");
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~FtpClientDataConnection()
        {
            Trace.WriteLine("FtpClientDataConnection::~FtpClientDataConnection()");

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
            Trace.WriteLine("FtpClientDataConnection::Dispose()");

            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 破棄
        /// </summary>
        /// <param name="isDisposing"></param>
        protected virtual void Dispose(bool isDisposing)
        {
            Trace.WriteLine("FtpClientDataConnection::Dispose(bool)");

            // 破棄しているか？
            if (!this.m_Disposed)
            {
                // 未実装(アンマネージドリソース解放)
                if (this.Socket != null && this.Socket.Connected)
                {
                    this.Socket.Dispose();
                }

                // マネージドリソース解放
                if (isDisposing)
                {
                }

                // 破棄済みを設定
                this.m_Disposed = true;
            }
        }
        #endregion

        /// <summary>
        /// 文字列変換
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder _StringBuilder = new StringBuilder();

            // 文字列作成
            _StringBuilder.AppendFormat("　Mode       : {0}\n", Mode.ToString());
            _StringBuilder.AppendFormat("　IpAddress  : {0}\n", IpAddress);
            _StringBuilder.AppendFormat("　Port       : {0}\n", Port.ToString());
            _StringBuilder.AppendFormat("　Listener   : {0}\n", Listener);
            _StringBuilder.AppendFormat("　Socket     : {0}\n", Socket);
            if (Socket != null)
            {
                _StringBuilder.AppendFormat("　└ Connected : {0}\n", Socket.Connected);
            }

            return _StringBuilder.ToString();
        }
    }
}
