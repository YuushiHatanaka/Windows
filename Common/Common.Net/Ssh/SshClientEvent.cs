using Renci.SshNet;
using System;
using System.IO;
using System.Net;

namespace Common.Net
{
    #region イベント処理用デリゲート
    /// <summary>
    /// 接続イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SshClientConnectedEventHandler(object sender, SshClientConnectedEventArgs e);

    /// <summary>
    /// 受信イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SshClientReciveEventHandler(object sender, SshClientReciveEventArgs e);

    /// <summary>
    /// 送信イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SshClientSendEventHandler(object sender, SshClientSendEventArgs e);

    /// <summary>
    /// 切断イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SshClientDisonnectedEventHandler(object sender, SshClientDisconnectedEventArgs e);
    #endregion

    #region イベントパラメータ
    /// <summary>
    /// 接続イベントパラメータ
    /// </summary>
    public class SshClientConnectedEventArgs : EventArgs
    {
        /// <summary>
        /// 接続情報
        /// </summary>
        public ConnectionInfo ConnectionInfo = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SshClientConnectedEventArgs()
            : base()
        {
        }
    }

    /// <summary>
    /// 受信イベントパラメータ
    /// </summary>
    public class SshClientReciveEventArgs : EventArgs
    {
        /// <summary>
        /// 接続情報
        /// </summary>
        public ConnectionInfo ConnectionInfo = null;

        /// <summary>
        /// 受信サイズ
        /// </summary>
        public int Size = 0;

        /// <summary>
        /// 受信Stream
        /// </summary>
        public MemoryStream Stream = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SshClientReciveEventArgs()
            : base()
        {
        }
    }

    /// <summary>
    /// 送信イベントパラメータ
    /// </summary>
    public class SshClientSendEventArgs : EventArgs
    {
        /// <summary>
        /// 接続情報
        /// </summary>
        public ConnectionInfo ConnectionInfo = null;

        /// <summary>
        /// 送信サイズ
        /// </summary>
        public int Size = 0;

        /// <summary>
        /// 送信Stream
        /// </summary>
        public MemoryStream Stream = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SshClientSendEventArgs()
            : base()
        {
        }
    }

    /// <summary>
    /// 切断イベントパラメータ
    /// </summary>
    public class SshClientDisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// 接続情報
        /// </summary>
        public ConnectionInfo ConnectionInfo = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SshClientDisconnectedEventArgs()
            : base()
        {
        }
    }
    #endregion
}
