using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Net;

namespace Common.Net
{
    #region イベント処理用デリゲート
    /// <summary>
    /// 接続イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void FtpClientConnectedEventHandler(object sender, FtpClientConnectedEventArgs e);

    /// <summary>
    /// Uploadイベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void FtpClientUploadEventHandler(object sender, FtpClientUploadEventArgs e);

    /// <summary>
    /// Downloadイベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void FtpClientDownloadEventHandler(object sender, FtpClientDownloadEventArgs e);

    /// <summary>
    /// 切断イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void FtpClientDisonnectedEventHandler(object sender, FtpClientDisconnectedEventArgs e);
    #endregion

    /// <summary>
    /// 接続イベントパラメータ
    /// </summary>
    public class FtpClientConnectedEventArgs : EventArgs
    {
        /// <summary>
        /// 接続元エンドポイント
        /// </summary>
        public EndPoint LocalEndPoint = null;

        /// <summary>
        /// 接続先エンドポイント
        /// </summary>
        public EndPoint RemoteEndPoint = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FtpClientConnectedEventArgs()
            : base()
        {
        }
    }

    /// <summary>
    /// Uploadイベントパラメータ
    /// </summary>
    public class FtpClientUploadEventArgs : EventArgs
    {
        /// <summary>
        /// アップロードサイズ
        /// </summary>
        public int Size;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FtpClientUploadEventArgs()
            : base()
        {
        }
    }

    /// <summary>
    /// Downloadイベントパラメータ
    /// </summary>
    public class FtpClientDownloadEventArgs : EventArgs
    {
        /// <summary>
        /// ダウンロードサイズ
        /// </summary>
        public int Size;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FtpClientDownloadEventArgs()
            : base()
        {
        }
    }

    /// <summary>
    /// 切断イベントパラメータ
    /// </summary>
    public class FtpClientDisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// 接続先エンドポイント
        /// </summary>
        public IPEndPoint RemoteEndPoint = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FtpClientDisconnectedEventArgs()
            : base()
        {
        }
    }
}
