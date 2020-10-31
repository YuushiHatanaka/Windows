using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
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

    /// <summary>
    /// TELNET仮想端末クラス
    /// </summary>
    public class TelnetNetworkVirtualTerminal : TcpNetworkVirtualTerminal
    {
        #region 端末速度
        /// <summary>
        /// 端末速度
        /// </summary>
        private TerminalSpeed m_TerminalSpeed = new TerminalSpeed() { Input = 38400, Output = 38400 };
        #endregion

        #region 端末速度
        /// <summary>
        /// バイナリ送信モード
        /// </summary>
        private bool m_TelBinSend = false;
        #endregion

        #region バイナリ受信モード
        /// <summary>
        /// バイナリ受信モード
        /// </summary>
        private bool m_TelBinRecv = false;
        #endregion

        #region 行送信モード
        /// <summary>
        /// 行送信モード
        /// </summary>
        private bool m_TelLineMode = false;
        #endregion

        #region エコーモード
        /// <summary>
        /// エコーモード
        /// </summary>
        private bool m_TelEcho = false;
        #endregion

        #region ローカルエコー
        /// <summary>
        /// ローカルエコー
        /// </summary>
        private int m_LocalEcho = 0;
        #endregion

        #region オプション定義
        /// <summary>
        /// オプション定義(Local)
        /// </summary>
        private Dictionary<TelnetOption, TelnetNegotiationStatus> m_Local = new Dictionary<TelnetOption, TelnetNegotiationStatus>();

        /// <summary>
        /// オプション定義(Remote)
        /// </summary>
        private Dictionary<TelnetOption, TelnetNegotiationStatus> m_Remote = new Dictionary<TelnetOption, TelnetNegotiationStatus>();
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TelnetNetworkVirtualTerminal()
        {
            // 初期化
            this.Initialization();
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~TelnetNetworkVirtualTerminal()
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
                this.m_Local[opt] = new TelnetNegotiationStatus();
                this.m_Remote[opt] = new TelnetNegotiationStatus();
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
        }
        #endregion

        #region ネゴシエーション
        /// <summary>
        /// ネゴシエーション
        /// </summary>
        /// <param name="recive"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public List<TelnetNegotiationInfomation> Negotiation(MemoryStream recive, MemoryStream output)
        {
            // 解析
            return this.Parse(recive.ToArray(), output);
        }
        #endregion

        #region 解析(Parse)
        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="parseData"></param>
        /// <returns></returns>
        private List<TelnetNegotiationInfomation> Parse(byte[] parseData)
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
        private List<TelnetNegotiationInfomation> Parse(byte[] parseData, MemoryStream stream)
        {
            // 返却オブジェクト生成
            List<TelnetNegotiationInfomation> negotiationInfomations = new List<TelnetNegotiationInfomation>();

            // 解析データ分繰返し
            for (int i = 0; i < parseData.Length;)
            {
                // データがIAC？
                if (parseData[i] == (byte)TelnetCommand.IAC)
                {
                    // IAC解析
                    TelnetNegotiationInfomation negotiationInfomation = this.ParseIAC(parseData, ref i);
                    if (negotiationInfomation != null)
                    {
                        // リスト追加
                        negotiationInfomations.Add(negotiationInfomation);
                    }
                    else
                    {
                        // 例外
                        throw new TelnetNetworkVirtualTerminalException("IAC解析に失敗しました");
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
        private TelnetNegotiationInfomation ParseIAC(byte[] parseData, ref int index)
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
                    return null;
            }
        }

        /// <summary>
        /// 解析(DO)
        /// </summary>
        /// <param name="parseData"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private TelnetNegotiationInfomation ParseDO(byte[] parseData, ref int index)
        {
            // 返却オブジェクト生成
            TelnetNegotiationInfomation negotiationInfomation = new TelnetNegotiationInfomation() { Command = TelnetCommand.DO };

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
        private TelnetNegotiationInfomation ParseDONT(byte[] parseData, ref int index)
        {
            // 返却オブジェクト生成
            TelnetNegotiationInfomation negotiationInfomation = new TelnetNegotiationInfomation() { Command = TelnetCommand.DONT };

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
        private TelnetNegotiationInfomation ParseWILL(byte[] parseData, ref int index)
        {
            // 返却オブジェクト生成
            TelnetNegotiationInfomation negotiationInfomation = new TelnetNegotiationInfomation() { Command = TelnetCommand.WILL };

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
        private TelnetNegotiationInfomation ParseWONT(byte[] parseData, ref int index)
        {
            // 返却オブジェクト生成
            TelnetNegotiationInfomation negotiationInfomation = new TelnetNegotiationInfomation() { Command = TelnetCommand.WONT };

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
        private TelnetNegotiationInfomation ParseSB(byte[] parseData, ref int index)
        {
            // 返却オブジェクト生成
            TelnetNegotiationInfomation negotiationInfomation = new TelnetNegotiationInfomation() { Command = TelnetCommand.SB };

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
        private TelnetNegotiationInfomation ParseSE(byte[] parseData, ref int index)
        {
            // 返却オブジェクト生成
            TelnetNegotiationInfomation negotiationInfomation = new TelnetNegotiationInfomation() { Command = TelnetCommand.SE };

            // インデックス更新
            index++;

            // 返却
            return negotiationInfomation;
        }
        #endregion

        #region 応答送信(Response)
        /// <summary>
        /// ネゴシエーション応答送信
        /// </summary>
        /// <param name="client"></param>
        /// <param name="infoList"></param>
        /// <returns></returns>
        public void NegotiationResponse(Common.Net.TelnetClient client, List<TelnetNegotiationInfomation> infoList)
        {
            // リスト数分繰返し
            foreach (TelnetNegotiationInfomation info in infoList)
            {
                // コマンド毎に分岐
                switch (info.Command)
                {
                    case TelnetCommand.DO:
                        // 応答送信(DO)
                        this.ResponseDO(client, info);
                        break;
                    case TelnetCommand.DONT:
                        // 応答送信(DONT)
                        this.ResponseDONT(client, info);
                        break;
                    case TelnetCommand.WILL:
                        // 応答送信(WILL)
                        this.ResponseWILL(client, info);
                        break;
                    case TelnetCommand.WONT:
                        // 応答送信(WONT)
                        this.ResponseWONT(client, info);
                        break;
                    case TelnetCommand.SB:
                        // 応答送信(SB)
                        this.ResponseSB(client, info);
                        break;
                    case TelnetCommand.SE:
                        // 応答送信(SE)
                        this.ResponseSE(client, info);
                        break;
                    default:
                        // 異常終了
                        break;
                }
            }
        }

        /// <summary>
        /// 応答送信
        /// </summary>
        /// <param name="command"></param>
        /// <param name="option"></param>
        private MemoryStream SendBack(TelnetCommand command, TelnetOption option)
        {
            // 送信メモリオブジェクト生成
            MemoryStream sendStream = new MemoryStream();
            sendStream.WriteByte((byte)TelnetCommand.IAC);
            sendStream.WriteByte((byte)command);
            sendStream.WriteByte((byte)option);
            return sendStream;
        }

        /// <summary>
        /// ウィンドウサイズ送信
        /// </summary>
        private MemoryStream SendWinSize()
        {
            // 送信メモリオブジェクト生成
            MemoryStream sendStream = new MemoryStream();
            sendStream.WriteByte((byte)TelnetCommand.IAC);
            sendStream.WriteByte((byte)TelnetCommand.SB);
            sendStream.WriteByte((byte)TelnetOption.window_size);

            byte[] width = BitConverter.GetBytes(this.m_Size.Width);
            byte[] height = BitConverter.GetBytes(this.m_Size.Height);
            sendStream.WriteByte(width[1]);
            sendStream.WriteByte(width[0]);
            sendStream.WriteByte(height[1]);
            sendStream.WriteByte(height[0]);

            sendStream.WriteByte((byte)TelnetCommand.IAC);
            sendStream.WriteByte((byte)TelnetCommand.SE);
            return sendStream;
        }

        /// <summary>
        /// 端末種別送信
        /// </summary>
        private MemoryStream SendTerminalType()
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

            return sendStream;
        }

        /// <summary>
        /// 端末速度送信
        /// </summary>
        private MemoryStream SendTerminalSpeed()
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

            return sendStream;
        }

        /// <summary>
        /// 応答送信(DO)
        /// </summary>
        /// <param name="client"></param>
        /// <param name="info"></param>
        private void ResponseDO(Common.Net.TelnetClient client, TelnetNegotiationInfomation info)
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
                            client.SendAsync(SendBack(TelnetCommand.WILL, info.Option));
                        }
                        else
                        {
                            client.SendAsync(SendBack(TelnetCommand.WONT, info.Option));
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
                                client.SendAsync(SendBack(TelnetCommand.WONT, info.Option));
                                break;
                        }
                        break;
                }
            }
            else
            {
                client.SendAsync(SendBack(TelnetCommand.WONT, info.Option));
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
                        client.SendAsync(SendWinSize());
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
        /// <param name="client"></param>
        /// <param name="info"></param>
        private void ResponseDONT(Common.Net.TelnetClient client, TelnetNegotiationInfomation info)
        {
            if (info.Option <= TelnetOption.max)
            {
                switch (this.m_Local[info.Option].Status)
                {
                    case TelnetOptionStatus.Yes:
                        this.m_Local[info.Option].Status = TelnetOptionStatus.No;
                        client.SendAsync(SendBack(TelnetCommand.WONT, info.Option));
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
                                client.SendAsync(SendBack(TelnetCommand.WILL, info.Option));
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
                client.SendAsync(SendBack(TelnetCommand.WONT, info.Option));
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
        /// <param name="client"></param>
        /// <param name="info"></param>
        private void ResponseWILL(Common.Net.TelnetClient client, TelnetNegotiationInfomation info)
        {
            if (info.Option <= TelnetOption.max)
            {
                switch (this.m_Remote[info.Option].Status)
                {
                    case TelnetOptionStatus.No:
                        if (this.m_Remote[info.Option].Accept)
                        {
                            client.SendAsync(SendBack(TelnetCommand.DO, info.Option));
                            this.m_Remote[info.Option].Status = TelnetOptionStatus.Yes;
                        }
                        else
                        {
                            client.SendAsync(SendBack(TelnetCommand.DONT, info.Option));
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
                                client.SendAsync(SendBack(TelnetCommand.DONT, info.Option));
                                break;
                        }
                        break;
                }
            }
            else
            {
                client.SendAsync(SendBack(TelnetCommand.DONT, info.Option));
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
        /// <param name="client"></param>
        /// <param name="info"></param>
        private void ResponseWONT(Common.Net.TelnetClient client, TelnetNegotiationInfomation info)
        {
            if (info.Option <= TelnetOption.max)
            {
                switch (this.m_Remote[info.Option].Status)
                {
                    case TelnetOptionStatus.Yes:
                        this.m_Remote[info.Option].Status = TelnetOptionStatus.No;
                        client.SendAsync(SendBack(TelnetCommand.DONT, info.Option));
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
                                client.SendAsync(SendBack(TelnetCommand.DO, info.Option));
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
                client.SendAsync(SendBack(TelnetCommand.DONT, info.Option));
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
        /// <param name="client"></param>
        /// <param name="info"></param>
        private void ResponseSB(Common.Net.TelnetClient client, TelnetNegotiationInfomation info)
        {
            // オプション毎に分岐
            switch (info.Option)
            {
                case TelnetOption.terminal_type:
                    // TODO:未実装
                    if (info.Stream.Length > 0 && info.Stream.ToArray()[0] == 0x01)
                    {
                        client.SendAsync(SendTerminalType());
                    }
                    break;
                case TelnetOption.window_size:
                    // TODO:未実装
                    client.SendAsync(SendWinSize());
                    break;
                case TelnetOption.terminal_speed:
                    // TODO:未実装
                    if (info.Stream.Length > 0 && info.Stream.ToArray()[0] == 0x01)
                    {
                        client.SendAsync(SendTerminalSpeed());
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 応答送信(SE)
        /// </summary>
        /// <param name="client"></param>
        /// <param name="info"></param>
        private void ResponseSE(Common.Net.TelnetClient client, TelnetNegotiationInfomation info)
        {
        }
        #endregion
    }
}
