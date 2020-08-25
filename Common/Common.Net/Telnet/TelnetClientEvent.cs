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
    public delegate void TelnetClientConnectedEventHandler(object sender, TelnetClientConnectedEventArgs e);

    /// <summary>
    /// 受信イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TelnetClientReciveEventHandler(object sender, TelnetClientReciveEventArgs e);

    /// <summary>
    /// 送信イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TelnetClientSendEventHandler(object sender, TelnetClientSendEventArgs e);

    /// <summary>
    /// 切断イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TelnetClientDisonnectedEventHandler(object sender, TelnetClientDisconnectedEventArgs e);
    #endregion

    /// <summary>
    /// 接続イベントパラメータ
    /// </summary>
    public class TelnetClientConnectedEventArgs : EventArgs
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
        public TelnetClientConnectedEventArgs()
            : base()
        {
        }
    }

    /// <summary>
    /// 受信イベントパラメータ
    /// </summary>
    public class TelnetClientReciveEventArgs : EventArgs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TelnetClientReciveEventArgs()
            : base()
        {
        }
    }

    /// <summary>
    /// 送信イベントパラメータ
    /// </summary>
    public class TelnetClientSendEventArgs : EventArgs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TelnetClientSendEventArgs()
            : base()
        {
        }
    }

    /// <summary>
    /// 切断イベントパラメータ
    /// </summary>
    public class TelnetClientDisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// 接続先エンドポイント
        /// </summary>
        public IPEndPoint RemoteEndPoint = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TelnetClientDisconnectedEventArgs()
            : base()
        {
        }
    }
}
