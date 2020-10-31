using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Renci.SshNet;

namespace Common.Net
{
    /// <summary>
    /// 送信Streamクラス
    /// </summary>
    public class SshClientSendStream
    {
        /// <summary>
        /// ConnectionInfoオブジェクト
        /// </summary>
        public Renci.SshNet.ConnectionInfo ConnectionInfo = null;

        /// <summary>
        /// SshClientオブジェクト
        /// </summary>
        public Renci.SshNet.SshClient SshClient = null;

        /// <summary>
        /// ShellStreamオブジェクト
        /// </summary>
        public Renci.SshNet.ShellStream ShellStream = null;

        /// <summary>
        /// IPEndPointオブジェクト
        /// </summary>
        public IPEndPoint IPEndPoint = null;

        /// <summary>
        /// 送信サイズ
        /// </summary>
        public int Size = 0;

        /// <summary>
        /// 送信Stream
        /// </summary>
        public MemoryStream Stream = null;
    }

    /// <summary>
    /// 受信Streamクラス
    /// </summary>
    public class SshClientReciveStream
    {
        /// <summary>
        /// ConnectionInfoオブジェクト
        /// </summary>
        public Renci.SshNet.ConnectionInfo ConnectionInfo = null;

        /// <summary>
        /// SshClientオブジェクト
        /// </summary>
        public Renci.SshNet.SshClient SshClient = null;

        /// <summary>
        /// ShellStreamオブジェクト
        /// </summary>
        public Renci.SshNet.ShellStream ShellStream = null;

        /// <summary>
        /// IPEndPointオブジェクト
        /// </summary>
        public IPEndPoint IPEndPoint = null;

        /// <summary>
        /// データ転送用バッファ
        /// </summary>
        public byte[] Buffer = null;

        /// <summary>
        /// データ保持用Stream
        /// </summary>
        public MemoryStream Stream = null;
    }
}
