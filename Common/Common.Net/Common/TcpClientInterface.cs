using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Common.Net
{
    /// <summary>
    /// TCPクライアントインタフェース
    /// </summary>
    public interface TcpClientInterface
    {
        #region 接続
        /// <summary>
        /// 接続
        /// </summary>
        void Connect();

        /// <summary>
        /// 接続
        /// </summary>
        /// <param name="timeout"></param>
        void Connect(int timeout);
        #endregion

        #region 切断
        /// <summary>
        /// 切断
        /// </summary>
        void Disconnect();

        /// <summary>
        /// 切断
        /// </summary>
        /// <param name="timeout"></param>
        void Disconnect(int timeout);
        #endregion

        #region 送信
        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="data"></param>
        void Send(byte data);

        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="data"></param>
        void Send(byte[] data);

        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="stream"></param>
        void Send(MemoryStream stream);

        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        void Send(byte data, int timeout);

        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        void Send(byte[] data, int timeout);

        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="timeout"></param>
        void Send(MemoryStream stream, int timeout);
        #endregion

        #region 受信
        /// <summary>
        /// 受信
        /// </summary>
        /// <returns></returns>
        MemoryStream Recive();

        /// <summary>
        /// 受信
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        MemoryStream Recive(int timeout);
        #endregion

        #region 非同期送信
        /// <summary>
        /// 非同期送信
        /// </summary>
        /// <param name="data"></param>
        void SendAsync(byte data);

        /// <summary>
        /// 非同期送信
        /// </summary>
        /// <param name="data"></param>
        void SendAsync(byte[] data);

        /// <summary>
        /// 非同期送信
        /// </summary>
        /// <param name="stream"></param>
        void SendAsync(MemoryStream stream);
        #endregion

        #region 非同期受信
        /// <summary>
        /// 受信
        /// </summary>
        /// <returns></returns>
        void ReciveAsync();

        /// <summary>
        /// 受信
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        void ReciveAsync(int size);
        #endregion
    }
}
