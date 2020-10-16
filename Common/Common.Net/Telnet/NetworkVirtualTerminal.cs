using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Net
{
    #region 端末速度
    /// <summary>
    /// 端末速度
    /// </summary>
    public class TerminalSpeed
    {
        public int Input;
        public int Output;
    };
    #endregion

    #region イベントパラメータ
    /// <summary>
    /// 受信イベントパラメータ
    /// </summary>
    public class NetworkVirtualTerminalReadEventArgs : EventArgs
    {
        /// <summary>
        /// バッファ
        /// </summary>
        public StringBuilder StringBuilder = new StringBuilder();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NetworkVirtualTerminalReadEventArgs()
            : base()
        {
        }
    }

    /// <summary>
    /// 送信イベントパラメータ
    /// </summary>
    public class NetworkVirtualTerminalWriteEventArgs : EventArgs
    {
        /// <summary>
        /// バッファ
        /// </summary>
        public StringBuilder StringBuilder = new StringBuilder();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NetworkVirtualTerminalWriteEventArgs()
            : base()
        {
        }
    }

    /// <summary>
    /// 例外イベントパラメータ
    /// </summary>
    public class NetworkVirtualTerminalExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// 例外
        /// </summary>
        public Exception Exception = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NetworkVirtualTerminalExceptionEventArgs()
            : base()
        {
        }
    }
    #endregion

    #region イベントdelega
    /// <summary>
    /// 受信イベントdelegate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void NetworkVirtualTerminalReadEventHandler(object sender, NetworkVirtualTerminalReadEventArgs e);

    /// <summary>
    /// 送信イベントdelegate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void NetworkVirtualTerminalWriteEventHandler(object sender, NetworkVirtualTerminalWriteEventArgs e);

    /// <summary>
    /// 例外イベントdelegate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void NetworkVirtualTerminalExceptionEventHandler(object sender, NetworkVirtualTerminalExceptionEventArgs e);
    #endregion

    #region 仮想ネットワーク端末クラス
    /// <summary>
    /// 仮想ネットワーク端末クラス
    /// </summary>
    public class NetworkVirtualTerminal : TelnetClient
    {
        #region イベントハンドラ
        /// <summary>
        /// 読込イベントハンドラ
        /// </summary>
        public NetworkVirtualTerminalReadEventHandler OnRead;

        /// <summary>
        /// 書込イベントハンドラ
        /// </summary>
        public NetworkVirtualTerminalWriteEventHandler OnWrite;

        /// <summary>
        /// 例外イベントハンドラ
        /// </summary>
        public NetworkVirtualTerminalExceptionEventHandler OnException;
        #endregion

        #region 受信タスク
        /// <summary>
        /// 受信タスクオブジェクト
        /// </summary>
        private Task m_ReciveTask = null;

        /// <summary>
        /// 受信タスクトークン
        /// </summary>
        private CancellationTokenSource m_CancellationTokenSource = null;
        #endregion

        /// <summary>
        /// 受信イベント通知
        /// </summary>
        private ManualResetEvent OnReadNotify = new ManualResetEvent(false);

        /// <summary>
        /// 受信バッファ
        /// </summary>
        private StringBuilder m_ReadString = new StringBuilder();

        #region 端末情報
        #region 文字コード(Local)
        /// <summary>
        /// 文字コード(Local)
        /// </summary>
        private Encoding m_LocalEncoding;

        /// <summary>
        /// 文字コード(Local)
        /// </summary>
        public Encoding LocalEncoding { get { return this.m_LocalEncoding; } set { this.m_LocalEncoding = value; } }
        #endregion

        #region 文字コード(Remote)
        /// <summary>
        /// 文字コード(Remote)
        /// </summary>
        private Encoding m_RemoteEncoding;

        /// <summary>
        /// 文字コード(Remote)
        /// </summary>
        public Encoding RemoteEncoding { get { return this.m_RemoteEncoding; } set { this.m_RemoteEncoding = value; } }
        #endregion

        #region 改行コード(送信)
        /// <summary>
        /// 改行コード(送信)
        /// </summary>
        private string m_WriteNewLine = string.Empty;

        /// <summary>
        /// 改行コード(送信)
        /// </summary>
        public string WriteNewLine { get { return this.m_WriteNewLine; } set { this.m_WriteNewLine = value; } }
        #endregion

        #region 改行コード(受信)
        /// <summary>
        /// 改行コード(受信)
        /// </summary>
        private string m_ReadNewLine = string.Empty;

        /// <summary>
        /// 改行コード(受信)
        /// </summary>
        public string ReadNewLine { get { return this.m_ReadNewLine; } set { this.m_ReadNewLine = value; } }
        #endregion

        /// <summary>
        /// エコーモード
        /// </summary>
        private bool m_TelEcho = false;

        /// <summary>
        /// ローカルエコー
        /// </summary>
        private int m_LocalEcho = 0;

        #region 端末種別
        /// <summary>
        /// 端末種別
        /// </summary>
        private string m_TerminalType = "xterm";
        #endregion

        #region 端末サイズ
        /// <summary>
        /// 端末サイズ
        /// </summary>
        private Size m_WindowSize = new Size() { Width = 80, Height = 40 };
        #endregion

        #region 端末速度
        /// <summary>
        /// 端末速度
        /// </summary>
        private TerminalSpeed m_TerminalSpeed = new TerminalSpeed() { Input = 38400, Output = 38400 };
        #endregion

        /// <summary>
        /// バイナリ送信モード
        /// </summary>
        private bool m_TelBinSend = false;

        /// <summary>
        /// バイナリ受信モード
        /// </summary>
        private bool m_TelBinRecv = false;

        /// <summary>
        /// 行送信モード
        /// </summary>
        private bool m_TelLineMode = false;

        /// <summary>
        /// オプション定義(Local)
        /// </summary>
        private Dictionary<TelnetOption, NegotiationStatus> m_Local = new Dictionary<TelnetOption, NegotiationStatus>();

        /// <summary>
        /// オプション定義(Remote)
        /// </summary>
        private Dictionary<TelnetOption, NegotiationStatus> m_Remote = new Dictionary<TelnetOption, NegotiationStatus>();
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NetworkVirtualTerminal()
            : base("localhost")
        {
            // 初期化
            this.Initialization();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        public NetworkVirtualTerminal(string host)
            : base(host)
        {
            // 初期化
            this.Initialization();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public NetworkVirtualTerminal(string host, int port)
            : base(host, port)
        {
            // 初期化
            this.Initialization();
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~NetworkVirtualTerminal()
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

            // Telnetオプション状態初期化
            for (TelnetOption opt = TelnetOption.binary; opt < TelnetOption.max; opt++)
            {
                this.m_Local[opt] = new NegotiationStatus();
                this.m_Remote[opt] = new NegotiationStatus();
            }

            // 各Telnetオプション初期設定
            this.m_Local[TelnetOption.binary].Accept = true;
            this.m_Remote[TelnetOption.binary].Accept = true;
            this.m_Local[TelnetOption.suppress_go_ahead].Accept = true;
            this.m_Remote[TelnetOption.suppress_go_ahead].Accept = true;
            this.m_Remote[TelnetOption.echo].Accept = true;
            this.m_Local[TelnetOption.terminal_type].Accept = true;
            this.m_Local[TelnetOption.terminal_speed].Accept = true;
            this.m_Local[TelnetOption.window_size].Accept = true;
            this.m_Remote[TelnetOption.window_size].Accept = true;

            // イベントハンドラ設定
            this.OnConnected += this.ConnectedEvent;
            this.OnSend += this.SendEvent;
            this.OnRecive += this.ReciveEvent;
            this.OnDisconnected += this.DisonnectedEvent;
        }
        #endregion

        #region 受信タスク
        /// <summary>
        /// 受信タスク
        /// </summary>
        private void ReciveTask()
        {
            // 無限ループ
            while (true)
            {
                // Taskキャンセル判定
                if (this.m_CancellationTokenSource.IsCancellationRequested)
                {
                    // キャンセルされたらTaskを終了する.
                    break;
                }

                try
                {
                    // 受信
                    this.Recive();
                }
                catch (Exception ex)
                {
                    // イベント呼出し判定
                    if (this.OnException != null)
                    {
                        // イベントパラメータ生成
                        NetworkVirtualTerminalExceptionEventArgs eventArgs = new NetworkVirtualTerminalExceptionEventArgs()
                        {
                            Exception = ex
                        };

                        // イベント呼出し
                        this.OnException(this, eventArgs);
                    }
                }
            }

            // 切断
            this.DisConnect();
        }
        #endregion

        #region 非同期関連コールバックメソッド
        /// <summary>
        /// 非同期接続のコールバックメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectedEvent(object sender, TelnetClientConnectedEventArgs e)
        {
            // 受信タスクトークン生成
            this.m_CancellationTokenSource = new CancellationTokenSource();

            // 受信タスク開始
            this.m_ReciveTask = Task.Factory.StartNew(() =>
            {
                // 受信タスク実行
                this.ReciveTask();
            }, this.m_CancellationTokenSource.Token);
        }

        /// <summary>
        /// 非同期切断のコールバックメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisonnectedEvent(object sender, TelnetClientDisconnectedEventArgs e)
        {
            // 受信タスクトークン破棄
            if (this.m_CancellationTokenSource != null)
            {
                this.m_CancellationTokenSource.Cancel();
            }

            try
            {
                // 受信タスク終了待ち
                if (this.m_ReciveTask != null)
                {
                    this.m_ReciveTask.Wait(this.Timeout.Disconnect, this.m_CancellationTokenSource.Token);
                }
            }
            catch (OperationCanceledException)
            {
                // ロギング
                this.Logger.WarnFormat("受信タスク終了：[OperationCanceledException]");
            }
            catch (AggregateException)
            {
                // ロギング
                this.Logger.WarnFormat("受信タスク終了：[AggregateException]");
            }

            // 受信タスクトークン破棄
            if (this.m_CancellationTokenSource != null)
            {
                this.m_CancellationTokenSource.Dispose();
                this.m_CancellationTokenSource = null;
            }
        }

        /// <summary>
        /// 非同期送信のコールバックメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendEvent(object sender, TelnetClientSendEventArgs e)
        {
            // 送信通知
            if (this.OnWrite != null)
            {
                // パラメータ生成
                NetworkVirtualTerminalWriteEventArgs args = new NetworkVirtualTerminalWriteEventArgs();
                args.StringBuilder.Append(this.LocalEncoding.GetString(e.Stream.ToArray()));

                // イベント呼出し
                this.OnWrite(this, args);
            }
        }

        /// <summary>
        /// 非同期受信のコールバックメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReciveEvent(object sender, TelnetClientReciveEventArgs e)
        {
            // 受信判定
            if (e.Size > 0)
            {
                // 受信データオブジェクト
                MemoryStream memoryStream = new MemoryStream();

                // ネゴシエーション
                this.Negotiation(e.Stream, memoryStream);

                // MemorySteramのサイズ判定
                if (memoryStream.Length > 0)
                {
                    // ロギング
                    this.Logger.Info(this.RemoteEncoding.GetString(memoryStream.ToArray()));

                    // 受信通知
                    if (this.OnRead != null)
                    {
                        // パラメータ生成
                        NetworkVirtualTerminalReadEventArgs args = new NetworkVirtualTerminalReadEventArgs();
                        args.StringBuilder.Append(this.RemoteEncoding.GetString(memoryStream.ToArray()));

                        // イベント呼出し
                        this.OnRead(this, args);
                    }
                }
            }
        }
        #endregion

        #region ネゴシエーション
        /// <summary>
        /// ネゴシエーション
        /// </summary>
        /// <param name="recive"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        private void Negotiation(MemoryStream recive, MemoryStream output)
        {
            // ロギング
            this.Logger.Info("＜受信データ＞");

            // 解析
            List<NegotiationInfomation> parseResult = this.Parse(recive.ToArray(), output);

            // 応答作成
            if (parseResult.Count > 0)
            {
                // ロギング
                this.Logger.Info("＜送信データ＞");

                // 応答送信
                this.Response(parseResult);
            }
        }
        #endregion

        #region 解析(Parse)
        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="parseData"></param>
        /// <returns></returns>
        private List<NegotiationInfomation> Parse(byte[] parseData)
        {
            // 解析
            return this.Parse(parseData, new MemoryStream());
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="parseData"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        private List<NegotiationInfomation> Parse(byte[] parseData, MemoryStream stream)
        {
            // 返却オブジェクト生成
            List<NegotiationInfomation> negotiationInfomations = new List<NegotiationInfomation>();

            // 解析データ分繰返し
            for (int i = 0; i < parseData.Length;)
            {
                // データがIAC？
                if (parseData[i] == (byte)TelnetCommand.IAC)
                {
                    // IAC解析
                    NegotiationInfomation negotiationInfomation = this.ParseIAC(parseData, ref i);
                    if (negotiationInfomation != null)
                    {
                        // リスト追加
                        this.Logger.Info("　" + negotiationInfomation.ToString());
                        negotiationInfomations.Add(negotiationInfomation);
                    }
                    else
                    {
                        // 例外
                        throw new NetworkVirtualTerminalException("IAC解析に失敗しました");
                    }
                }
                else
                {
                    // バッファに書込み
                    stream.WriteByte(parseData[i++]);
                }
            }

            // 返却
            return negotiationInfomations;
        }

        /// <summary>
        /// 解析(IAC)
        /// </summary>
        /// <param name="parseData"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private NegotiationInfomation ParseIAC(byte[] parseData, ref int index)
        {
            // インデックス更新
            index++;

            // Telnetコマンド毎に分岐
            switch (parseData[index])
            {
                case (byte)TelnetCommand.DO:
                    // 解析(DO)
                    return this.ParseDO(parseData, ref index);
                case (byte)TelnetCommand.DONT:
                    // 解析(DONT)
                    return this.ParseDONT(parseData, ref index);
                case (byte)TelnetCommand.WILL:
                    // 解析(WILL)
                    return this.ParseWILL(parseData, ref index);
                case (byte)TelnetCommand.WONT:
                    // 解析(WONT)
                    return this.ParseWONT(parseData, ref index);
                case (byte)TelnetCommand.SB:
                    // 解析(SB)
                    return this.ParseSB(parseData, ref index);
                case (byte)TelnetCommand.SE:
                    // 解析(SB)
                    return this.ParseSE(parseData, ref index);
                default:
                    // TODO:異常終了
                    this.Logger.Error("[ParseIAC] UNKNOWN Telnet Command(0x" + string.Format("{0,2:x2}", parseData[index]) + ").");
                    return null;
            }
        }

        /// <summary>
        /// 解析(DO)
        /// </summary>
        /// <param name="parseData"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private NegotiationInfomation ParseDO(byte[] parseData, ref int index)
        {
            // 返却オブジェクト生成
            NegotiationInfomation negotiationInfomation = new NegotiationInfomation() { Command = TelnetCommand.DO };

            // インデックス更新
            index++;

            // Telnetオプション設定
            negotiationInfomation.Option = (TelnetOption)parseData[index++];

            // 返却
            return negotiationInfomation;
        }

        /// <summary>
        /// 解析(DONT)
        /// </summary>
        /// <param name="parseData"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private NegotiationInfomation ParseDONT(byte[] parseData, ref int index)
        {
            // 返却オブジェクト生成
            NegotiationInfomation negotiationInfomation = new NegotiationInfomation() { Command = TelnetCommand.DONT };

            // インデックス更新
            index++;

            // Telnetオプション設定
            negotiationInfomation.Option = (TelnetOption)parseData[index++];

            // 返却
            return negotiationInfomation;
        }

        /// <summary>
        /// 解析(WILL)
        /// </summary>
        /// <param name="parseData"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private NegotiationInfomation ParseWILL(byte[] parseData, ref int index)
        {
            // 返却オブジェクト生成
            NegotiationInfomation negotiationInfomation = new NegotiationInfomation() { Command = TelnetCommand.WILL };

            // インデックス更新
            index++;

            // Telnetオプション設定
            negotiationInfomation.Option = (TelnetOption)parseData[index++];

            // 返却
            return negotiationInfomation;
        }

        /// <summary>
        /// 解析(WONT)
        /// </summary>
        /// <param name="parseData"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private NegotiationInfomation ParseWONT(byte[] parseData, ref int index)
        {
            // 返却オブジェクト生成
            NegotiationInfomation negotiationInfomation = new NegotiationInfomation() { Command = TelnetCommand.WONT };

            // インデックス更新
            index++;

            // Telnetオプション設定
            negotiationInfomation.Option = (TelnetOption)parseData[index++];

            // 返却
            return negotiationInfomation;
        }

        /// <summary>
        /// 解析(SB)
        /// </summary>
        /// <param name="parseData"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private NegotiationInfomation ParseSB(byte[] parseData, ref int index)
        {
            // 返却オブジェクト生成
            NegotiationInfomation negotiationInfomation = new NegotiationInfomation() { Command = TelnetCommand.SB };

            // インデックス更新
            index++;

            // Telnetオプション設定
            negotiationInfomation.Option = (TelnetOption)parseData[index++];

            // インデックス繰り返し
            for (; index < parseData.Length; index++)
            {
                // 副交渉の終わり？
                if (parseData[index] == (byte)TelnetCommand.IAC && parseData[index + 1] == (byte)TelnetCommand.SE)
                {
                    // 副交渉の終わりだった場合は繰り返し終了
                    break;
                }

                // データ部格納
                if (negotiationInfomation.Stream == null)
                {
                    negotiationInfomation.Stream = new MemoryStream();
                }
                negotiationInfomation.Stream.WriteByte(parseData[index]);
            }

            // 返却
            return negotiationInfomation;
        }

        /// <summary>
        /// 解析(SE)
        /// </summary>
        /// <param name="parseData"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private NegotiationInfomation ParseSE(byte[] parseData, ref int index)
        {
            // 返却オブジェクト生成
            NegotiationInfomation negotiationInfomation = new NegotiationInfomation() { Command = TelnetCommand.SE };

            // インデックス更新
            index++;

            // 返却
            return negotiationInfomation;
        }
        #endregion

        #region 応答送信(Response)
        /// <summary>
        /// 応答送信
        /// </summary>
        /// <param name="infoList"></param>
        /// <returns></returns>
        private void Response(List<NegotiationInfomation> infoList)
        {
            // リスト数分繰返し
            foreach (NegotiationInfomation info in infoList)
            {
                // コマンド毎に分岐
                switch (info.Command)
                {
                    case TelnetCommand.DO:
                        // 応答送信(DO)
                        this.ResponseDO(info);
                        break;
                    case TelnetCommand.DONT:
                        // 応答送信(DONT)
                        this.ResponseDONT(info);
                        break;
                    case TelnetCommand.WILL:
                        // 応答送信(WILL)
                        this.ResponseWILL(info);
                        break;
                    case TelnetCommand.WONT:
                        // 応答送信(WONT)
                        this.ResponseWONT(info);
                        break;
                    case TelnetCommand.SB:
                        // 応答送信(SB)
                        this.ResponseSB(info);
                        break;
                    case TelnetCommand.SE:
                        // 応答送信(SE)
                        this.ResponseSE(info);
                        break;
                    default:
                        // 異常終了
                        this.Logger.Error("[Response] UNKNOWN Telnet Command(" + info.Command.ToString() + ").");
                        break;
                }
            }
        }

        /// <summary>
        /// 応答送信
        /// </summary>
        /// <param name="command"></param>
        /// <param name="option"></param>
        private void SendBack(TelnetCommand command, TelnetOption option)
        {
            // 送信メモリオブジェクト生成
            MemoryStream sendStream = new MemoryStream();
            sendStream.WriteByte((byte)TelnetCommand.IAC);
            sendStream.WriteByte((byte)command);
            sendStream.WriteByte((byte)option);

            // 送信
            this.Parse(sendStream.ToArray());
            this.Send(sendStream);
        }

        /// <summary>
        /// ウィンドウサイズ送信
        /// </summary>
        private void SendWinSize()
        {
            // 送信メモリオブジェクト生成
            MemoryStream sendStream = new MemoryStream();
            sendStream.WriteByte((byte)TelnetCommand.IAC);
            sendStream.WriteByte((byte)TelnetCommand.SB);
            sendStream.WriteByte((byte)TelnetOption.window_size);

            byte[] width = BitConverter.GetBytes(this.m_WindowSize.Width);
            byte[] height = BitConverter.GetBytes(this.m_WindowSize.Height);
            sendStream.WriteByte(width[1]);
            sendStream.WriteByte(width[0]);
            sendStream.WriteByte(height[1]);
            sendStream.WriteByte(height[0]);

            sendStream.WriteByte((byte)TelnetCommand.IAC);
            sendStream.WriteByte((byte)TelnetCommand.SE);

            // 送信
            this.Parse(sendStream.ToArray());
            this.Send(sendStream);
        }

        /// <summary>
        /// 端末種別送信
        /// </summary>
        private void SendTerminalType()
        {
            // 送信メモリオブジェクト生成
            MemoryStream sendStream = new MemoryStream();

            sendStream.WriteByte((byte)TelnetCommand.IAC);
            sendStream.WriteByte((byte)TelnetCommand.SB);
            sendStream.WriteByte((byte)TelnetOption.terminal_type);

            // ASCII エンコード
            byte[] byteData = System.Text.Encoding.ASCII.GetBytes(this.m_TerminalType);
            sendStream.WriteByte(0x00);
            sendStream.Write(byteData, 0, byteData.Length);

            sendStream.WriteByte((byte)TelnetCommand.IAC);
            sendStream.WriteByte((byte)TelnetCommand.SE);

            // 送信
            this.Parse(sendStream.ToArray());
            this.Send(sendStream);
        }

        /// <summary>
        /// 端末速度送信
        /// </summary>
        private void SendTerminalSpeed()
        {
            // 送信メモリオブジェクト生成
            MemoryStream sendStream = new MemoryStream();

            sendStream.WriteByte((byte)TelnetCommand.IAC);
            sendStream.WriteByte((byte)TelnetCommand.SB);
            sendStream.WriteByte((byte)TelnetOption.terminal_speed);

            // ASCII エンコード
            byte[] byteInputData = System.Text.Encoding.ASCII.GetBytes(this.m_TerminalSpeed.Input.ToString());
            byte[] byteOutputData = System.Text.Encoding.ASCII.GetBytes(this.m_TerminalSpeed.Output.ToString());

            sendStream.WriteByte(0x00);
            sendStream.Write(byteInputData, 0, byteInputData.Length);
            sendStream.WriteByte(Convert.ToByte(','));
            sendStream.Write(byteOutputData, 0, byteOutputData.Length);

            sendStream.WriteByte((byte)TelnetCommand.IAC);
            sendStream.WriteByte((byte)TelnetCommand.SE);

            // 送信
            this.Parse(sendStream.ToArray());
            this.Send(sendStream);
        }

        /// <summary>
        /// 応答送信(DO)
        /// </summary>
        /// <param name="info"></param>
        private void ResponseDO(NegotiationInfomation info)
        {
            if (info.Option <= TelnetOption.max)
            {
                // オプション状態で分岐
                switch (this.m_Local[info.Option].Status)
                {
                    case TelnetOptionStatus.No:
                        if (this.m_Local[info.Option].Accept)
                        {
                            this.m_Local[info.Option].Status = TelnetOptionStatus.Yes;
                            SendBack(TelnetCommand.WILL, info.Option);
                        }
                        else
                        {
                            SendBack(TelnetCommand.WONT, info.Option);
                        }
                        break;
                    case TelnetOptionStatus.WantNo:
                        switch (this.m_Local[info.Option].Queue)
                        {
                            case TelnetOptionQueue.Empty:
                                this.m_Local[info.Option].Status = TelnetOptionStatus.No;
                                break;
                            case TelnetOptionQueue.Opposite:
                                this.m_Local[info.Option].Status = TelnetOptionStatus.Yes;
                                break;
                        }
                        break;
                    case TelnetOptionStatus.WantYes:
                        switch (this.m_Local[info.Option].Queue)
                        {
                            case TelnetOptionQueue.Empty:
                                this.m_Local[info.Option].Status = TelnetOptionStatus.Yes;
                                break;
                            case TelnetOptionQueue.Opposite:
                                this.m_Local[info.Option].Status = TelnetOptionStatus.WantNo;
                                this.m_Local[info.Option].Queue = TelnetOptionQueue.Empty;
                                SendBack(TelnetCommand.WONT, info.Option);
                                break;
                        }
                        break;
                }
            }
            else
            {
                SendBack(TelnetCommand.WONT, info.Option);
            }

            switch (info.Option)
            {
                case TelnetOption.binary:
                    switch (this.m_Local[TelnetOption.binary].Status)
                    {
                        case TelnetOptionStatus.Yes:
                            this.m_TelBinSend = true;
                            break;
                        case TelnetOptionStatus.No:
                            this.m_TelBinSend = false;
                            break;
                    }
                    break;

                case TelnetOption.window_size:
                    if (this.m_Local[TelnetOption.window_size].Status == TelnetOptionStatus.Yes)
                    {
                        SendWinSize();
                    }
                    break;

                case TelnetOption.suppress_go_ahead:
                    if (this.m_Local[TelnetOption.suppress_go_ahead].Status == TelnetOptionStatus.Yes)
                    {
                        this.m_TelLineMode = false;
                    }
                    break;
            }
        }

        /// <summary>
        /// 応答送信(DONT)
        /// </summary>
        /// <param name="info"></param>
        private void ResponseDONT(NegotiationInfomation info)
        {
            if (info.Option <= TelnetOption.max)
            {
                switch (this.m_Local[info.Option].Status)
                {
                    case TelnetOptionStatus.Yes:
                        this.m_Local[info.Option].Status = TelnetOptionStatus.No;
                        SendBack(TelnetCommand.WONT, info.Option);
                        break;

                    case TelnetOptionStatus.WantNo:
                        switch (this.m_Local[info.Option].Queue)
                        {
                            case TelnetOptionQueue.Empty:
                                this.m_Local[info.Option].Status = TelnetOptionStatus.No;
                                break;
                            case TelnetOptionQueue.Opposite:
                                this.m_Local[info.Option].Status = TelnetOptionStatus.WantYes;
                                this.m_Local[info.Option].Queue = TelnetOptionQueue.Empty;
                                SendBack(TelnetCommand.WILL, info.Option);
                                break;
                        }
                        break;

                    case TelnetOptionStatus.WantYes:
                        switch (this.m_Local[info.Option].Queue)
                        {
                            case TelnetOptionQueue.Empty:
                                this.m_Local[info.Option].Status = TelnetOptionStatus.No;
                                break;
                            case TelnetOptionQueue.Opposite:
                                this.m_Local[info.Option].Status = TelnetOptionStatus.No;
                                this.m_Local[info.Option].Queue = TelnetOptionQueue.Empty;
                                break;
                        }
                        break;
                }
            }
            else
            {
                SendBack(TelnetCommand.WONT, info.Option);
            }

            switch (info.Option)
            {
                case TelnetOption.binary:
                    switch (this.m_Local[TelnetOption.binary].Status)
                    {
                        case TelnetOptionStatus.Yes:
                            this.m_TelBinSend = true;
                            break;
                        case TelnetOptionStatus.No:
                            this.m_TelBinSend = false;
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// 応答送信(WILL)
        /// </summary>
        /// <param name="info"></param>
        private void ResponseWILL(NegotiationInfomation info)
        {
            if (info.Option <= TelnetOption.max)
            {
                switch (this.m_Remote[info.Option].Status)
                {
                    case TelnetOptionStatus.No:
                        if (this.m_Remote[info.Option].Accept)
                        {
                            SendBack(TelnetCommand.DO, info.Option);
                            this.m_Remote[info.Option].Status = TelnetOptionStatus.Yes;
                        }
                        else
                        {
                            SendBack(TelnetCommand.DONT, info.Option);
                        }
                        break;

                    case TelnetOptionStatus.WantNo:
                        switch (this.m_Remote[info.Option].Queue)
                        {
                            case TelnetOptionQueue.Empty:
                                this.m_Remote[info.Option].Status = TelnetOptionStatus.No;
                                break;
                            case TelnetOptionQueue.Opposite:
                                this.m_Remote[info.Option].Status = TelnetOptionStatus.Yes;
                                break;
                        }
                        break;

                    case TelnetOptionStatus.WantYes:
                        switch (this.m_Remote[info.Option].Queue)
                        {
                            case TelnetOptionQueue.Empty:
                                this.m_Remote[info.Option].Status = TelnetOptionStatus.Yes;
                                break;
                            case TelnetOptionQueue.Opposite:
                                this.m_Remote[info.Option].Status = TelnetOptionStatus.WantNo;
                                this.m_Remote[info.Option].Queue = TelnetOptionQueue.Empty;
                                SendBack(TelnetCommand.DONT, info.Option);
                                break;
                        }
                        break;
                }
            }
            else
            {
                SendBack(TelnetCommand.DONT, info.Option);
            }

            switch (info.Option)
            {
                case TelnetOption.echo:
                    if (this.m_TelEcho)
                    {
                        switch (this.m_Remote[TelnetOption.echo].Status)
                        {
                            case TelnetOptionStatus.Yes:
                                this.m_LocalEcho = 0;
                                break;
                            case TelnetOptionStatus.No:
                                this.m_LocalEcho = 1;
                                break;
                        }
                    }
                    if (this.m_Remote[TelnetOption.echo].Status == TelnetOptionStatus.Yes)
                    {
                        this.m_TelLineMode = false;
                    }
                    break;

                case TelnetOption.suppress_go_ahead:
                    if (this.m_Remote[TelnetOption.suppress_go_ahead].Status == TelnetOptionStatus.Yes)
                    {
                        this.m_TelLineMode = false;
                    }
                    break;

                case TelnetOption.binary:
                    switch (this.m_Remote[TelnetOption.binary].Status)
                    {
                        case TelnetOptionStatus.Yes:
                            this.m_TelBinRecv = true;
                            break;
                        case TelnetOptionStatus.No:
                            this.m_TelBinRecv = true;
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// 応答送信(WONT)
        /// </summary>
        /// <param name="info"></param>
        private void ResponseWONT(NegotiationInfomation info)
        {
            if (info.Option <= TelnetOption.max)
            {
                switch (this.m_Remote[info.Option].Status)
                {
                    case TelnetOptionStatus.Yes:
                        this.m_Remote[info.Option].Status = TelnetOptionStatus.No;
                        SendBack(TelnetCommand.DONT, info.Option);
                        break;

                    case TelnetOptionStatus.WantNo:
                        switch (this.m_Remote[info.Option].Queue)
                        {
                            case TelnetOptionQueue.Empty:
                                this.m_Remote[info.Option].Status = TelnetOptionStatus.No;
                                break;
                            case TelnetOptionQueue.Opposite:
                                this.m_Remote[info.Option].Status = TelnetOptionStatus.WantYes;
                                this.m_Remote[info.Option].Queue = TelnetOptionQueue.Empty;
                                SendBack(TelnetCommand.DO, info.Option);
                                break;
                        }
                        break;

                    case TelnetOptionStatus.WantYes:
                        switch (this.m_Remote[info.Option].Queue)
                        {
                            case TelnetOptionQueue.Empty:
                                this.m_Remote[info.Option].Status = TelnetOptionStatus.No;
                                break;
                            case TelnetOptionQueue.Opposite:
                                this.m_Remote[info.Option].Status = TelnetOptionStatus.No;
                                this.m_Remote[info.Option].Queue = TelnetOptionQueue.Empty;
                                break;
                        }
                        break;
                }
            }
            else
            {
                SendBack(TelnetCommand.DONT, info.Option);
            }

            switch (info.Option)
            {
                case TelnetOption.echo:
                    if (this.m_TelEcho)
                    {
                        switch (this.m_Remote[TelnetOption.echo].Status)
                        {
                            case TelnetOptionStatus.Yes:
                                this.m_LocalEcho = 0;
                                break;
                            case TelnetOptionStatus.No:
                                this.m_LocalEcho = 1;
                                break;
                        }
                    }
                    if (this.m_Remote[TelnetOption.echo].Status == TelnetOptionStatus.Yes)
                    {
                        this.m_TelLineMode = false;
                    }
                    break;

                case TelnetOption.binary:
                    switch (this.m_Remote[TelnetOption.binary].Status)
                    {
                        case TelnetOptionStatus.Yes:
                            this.m_TelBinRecv = true;
                            break;
                        case TelnetOptionStatus.No:
                            this.m_TelBinRecv = false;
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// 応答送信(SB)
        /// </summary>
        /// <param name="info"></param>
        private void ResponseSB(NegotiationInfomation info)
        {
            // オプション毎に分岐
            switch (info.Option)
            {
                case TelnetOption.terminal_type:
                    // TODO:未実装
                    if (info.Stream.Length > 0 && info.Stream.ToArray()[0] == 0x01)
                    {
                        this.SendTerminalType();
                    }
                    break;
                case TelnetOption.window_size:
                    // TODO:未実装
                    this.SendWinSize();
                    break;
                case TelnetOption.terminal_speed:
                    // TODO:未実装
                    if (info.Stream.Length > 0 && info.Stream.ToArray()[0] == 0x01)
                    {
                        this.SendTerminalSpeed();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 応答送信(SE)
        /// </summary>
        /// <param name="info"></param>
        private void ResponseSE(NegotiationInfomation info)
        {
        }
        #endregion

        #region 書込
        /// <summary>
        /// 書込
        /// </summary>
        /// <param name="str"></param>
        public void Write(string str)
        {
            // MemoryStreamオブジェクト生成
            MemoryStream sendStream = new MemoryStream();

            // エンコード
            byte[] data = this.RemoteEncoding.GetBytes(str);

            // MemoryStreamオブジェクト書込
            sendStream.Write(data, 0, data.Length);

            // 送信
            this.Send(sendStream);
        }

        /// <summary>
        /// 書込
        /// </summary>
        /// <param name="str"></param>
        public void WriteLine(string str)
        {
            // 書込(文字列+改行)
            this.Write(str + this.m_WriteNewLine);
        }
        #endregion

        #region 読込
        /// <summary>
        /// 読込
        /// </summary>
        /// <returns></returns>
        public StringBuilder Read()
        {
            // 結果オブジェクト生成
            StringBuilder result = new StringBuilder();

            //　イベント登録
            this.OnRead += this.ReadEventHandler;

            // 受信イベント待ち
            if (!this.OnReadNotify.WaitOne())
            {
                // 通知解除
                this.OnReadNotify.Reset();

                // TODO:例外
                return null;
            }

            // 通知解除
            this.OnReadNotify.Reset();

            // 結果登録
            result.Append(this.m_ReadString);

            // クリア
            lock (this.m_ReadString)
            {
                this.m_ReadString.Length = 0;
                this.m_ReadString.Clear();
            }

            //　イベント解除
            this.OnRead -= this.ReadEventHandler;

            // 結果返却
            return result;
        }

        /// <summary>
        /// 読込
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public StringBuilder Read(string str)
        {
            // 結果オブジェクト生成
            StringBuilder result = new StringBuilder();

            // 文字列受信まで繰り返す
            while (true)
            {
                // 読込
                StringBuilder readString = this.Read();

                // 結果登録
                result.Append(readString.ToString());

                // 文字列比較
                Regex regex = new Regex(str, RegexOptions.Compiled | RegexOptions.Multiline);
                if (regex.IsMatch(readString.ToString()))
                {
                    break;
                }
            }

            // 結果返却
            return result;
        }

        /// <summary>
        /// 読込
        /// </summary>
        /// <param name="str"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public StringBuilder Read(string str, int timeout)
        {
            // 結果オブジェクト生成
            StringBuilder result = new StringBuilder();

            // Taskオブジェクト生成
            using (CancellationTokenSource source = new CancellationTokenSource())
            {
                // タイムアウト設定
                source.CancelAfter(timeout);

                // Task開始
                Task task = Task.Factory.StartNew(() =>
                {
                    // 読込
                    StringBuilder readString = this.Read(str);

                    // 結果登録
                    result.Append(readString.ToString());
                }, source.Token);

                try
                {
                    // タスク待ち
                    task.Wait(source.Token);
                    Console.WriteLine("＜タスク終了＞");
                    return result;
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("OperationCanceledExceptionが発生しました。");
                    return null;
                }
                catch (AggregateException)
                {
                    Console.WriteLine("AggregateExceptionが発生しました。");
                    return null;
                }
            }
        }

        /// <summary>
        /// 読込
        /// </summary>
        /// <returns></returns>
        public StringBuilder ReadLine()
        {
            // TODO:未実装
            return null;
        }

        /// <summary>
        /// 読込イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadEventHandler(object sender, NetworkVirtualTerminalReadEventArgs e)
        {
            // 受信文字列追加
            lock (this.m_ReadString)
            {
                // 受信文字列追加
                this.m_ReadString.Append(e.StringBuilder.ToString());

                // 受信通知
                this.OnReadNotify.Set();
            }
        }
        #endregion
    }
    #endregion
}
