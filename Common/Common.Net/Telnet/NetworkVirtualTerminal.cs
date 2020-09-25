using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Common.Net
{
    /// <summary>
    /// Telnetコマンド
    /// </summary>
    public enum TelnetCommand : byte
    {
        /// <summary>
        /// 不明
        /// </summary>
        UN = 0x00,

        /// <summary>
        /// SE
        /// 副交渉の終わり 
        /// </summary>
        SE = 0xf0,

        /// <summary>
        /// NOP
        /// 無操作(Synch のデータストリーム部分)
        /// </summary>
        NOP = 0xf1,

        /// <summary>
        /// Data Mark
        /// (これは常に TCP Urgent 通知を伴う べきである)
        /// </summary>
        DM = 0xf2,

        /// <summary>
        /// Break
        /// NVT 文字 BRK 
        /// </summary>
        BRK = 0xf3,

        /// <summary>
        /// Interrupt Process
        /// IP 機能 
        /// </summary>
        IP = 0xf4,

        /// <summary>
        /// Abort output
        /// AO 機能
        /// </summary>
        AO = 0xf5,

        /// <summary>
        /// Are You There
        /// AYT 機能 
        /// </summary>
        AYT = 0xf6,

        /// <summary>
        /// Erase character
        /// EC 機能 
        /// </summary>
        EC = 0xf7,

        /// <summary>
        /// Erase Line
        /// EL 機能
        /// </summary>
        EL = 0xf8,

        /// <summary>
        /// Go ahead
        /// GA シグナル
        /// </summary>
        GA = 0xf9,

        /// <summary>
        /// SB
        /// (後に続くのが示されたオプションの副 交渉であることを表す)
        /// </summary>
        SB = 0xfa,

        /// <summary>
        /// WILL (オプションコード)
        /// (示されたオプションの実行開始、 または実行中かどうかの確認を望 むことを表す)
        /// </summary>
        WILL = 0xfb,

        /// <summary>
        /// WON'T (オプションコード)
        /// (示されたオプションの実行拒否ま たは継続実行拒否を表す)
        /// </summary>
        WONT = 0xfc,

        /// <summary>
        /// DO (オプションコード)
        /// (示されたオプションを実行すると いう相手側の要求、またはあなた がそれを実行することを期待して いるという確認を表す)
        /// </summary>
        DO = 0xfd,

        /// <summary>
        /// DO (オプションコード)
        /// (示されたオプションを停止すると いう相手側の要求、またはあなた がそれを実行することをもはや期 待しないという確認を表す )
        /// </summary>
        DONT = 0xfe,

        /// <summary>
        /// IAC
        /// (データバイト)
        /// </summary>
        IAC = 0xff,
    };

    /// <summary>
    /// Telnetオプション
    /// </summary>
    public enum TelnetOption : byte
    {
        /// <summary>
        /// 通常の7ビットデータではなく、8ビットバイナリとしてデータを受信する
        /// </summary>
        binary = 0x00,

        /// <summary>
        /// エコーバックを行う
        /// </summary>
        echo = 0x01,

        /// <summary>
        /// 送受信を切り替えるGO AHEADコマンドの送信を抑制する
        /// </summary>
        suppress_go_ahead = 0x03,

        /// <summary>
        /// Telnetオプション状態を送信する
        /// </summary>
        status = 0x05,

        /// <summary>
        /// コネクションの双方の同期を取る際に使用される
        /// </summary>
        timing_mark = 0x06,

        /// <summary>
        /// 端末タイプを送信する
        /// （クライアント側のみに対して有効）
        /// </summary>
        terminal_type = 0x18,

        /// <summary>
        /// 端末ウィンドウの行と列の数を送る
        /// （クライアント側のみに対して有効）
        /// </summary>
        window_size = 0x1f,

        /// <summary>
        /// 端末の送信速度と受信速度を送る
        /// （クライアント側のみに対して有効）
        /// </summary>
        terminal_speed = 0x20,

        /// <summary>
        /// フロー制御を行う
        /// </summary>
        remote_flow_control = 0x21,

        /// <summary>
        /// リアルラインモードにてデータを行単位で送る
        /// </summary>
        linemode = 0x22,

        /// <summary>
        /// 
        /// </summary>
        display_location = 0x23,

        /// <summary>
        /// 
        /// </summary>
        environment_variables = 0x24,

        /// <summary>
        /// 
        /// </summary>
        environment_option = 0x27,

        /// <summary>
        /// 未使用(または不明)
        /// </summary>
        unkown = 0xff,
    };

    /// <summary>
    /// ネゴシエーション情報クラス
    /// </summary>
    public class NegotiationInfomation
    {
        public TelnetCommand IAC = TelnetCommand.IAC;
        public TelnetCommand Command = TelnetCommand.UN;
        public TelnetOption Option = TelnetOption.unkown;
        public MemoryStream Stream = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NegotiationInfomation()
        {
        }

        /// <summary>
        /// コピーコンストラクタ
        /// </summary>
        /// <param name="info"></param>
        public NegotiationInfomation(NegotiationInfomation info)
        {
            this.IAC = info.IAC;
            this.Command = info.Command;
            this.Option = info.Option;
            this.Stream = info.Stream;
        }


        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("IAC ");
            stringBuilder.Append(this.Command.ToString() + " ");
            if (this.Option != TelnetOption.unkown)
            {
                stringBuilder.Append(this.Option.ToString() + " ");
            }
            if (this.Stream != null)
            {
                foreach (byte data in this.Stream.ToArray())
                {
                    stringBuilder.Append(string.Format("{0,2:x2} ", data));
                }
            }

            return stringBuilder.ToString();
        }
    };

    public class NegotiationStatus
    {
        public bool DO = false;
        public bool DONT = false;
        public bool WILL = false;
        public bool WONT = false;
        public bool SB = false;
        public bool SE = false;
    };

    public class TemplateStatus<T>
    {
        public NegotiationStatus Send = new NegotiationStatus();
        public NegotiationStatus Recv = new NegotiationStatus();
        public bool Negotiated = false;
        public T Value;

        public TemplateStatus()
        {
            this.Value = default(T);
        }
        public TemplateStatus(T value)
        {
            this.Value = value;
        }
    };

    /// <summary>
    /// 仮想端末クラス
    /// </summary>
    public class NetworkVirtualTerminal
    {
        #region 送信文字コード
        /// <summary>
        /// 送信文字コード
        /// </summary>
        private Encoding m_SendEncoding;

        /// <summary>
        /// 送信文字コード
        /// </summary>
        public Encoding SendEncoging { get { return this.m_SendEncoding; } set { this.m_SendEncoding = value; } }
        #endregion

        #region 受信文字コード
        /// <summary>
        /// 受信文字コード
        /// </summary>
        private Encoding m_RecvEncoding;

        /// <summary>
        /// 受信文字コード
        /// </summary>
        public Encoding RecvEncodin { get { return this.m_SendEncoding; } set { this.m_RecvEncoding = value; } }
        #endregion

        #region 端末サイズ
        /// <summary>
        /// 端末サイズ
        /// </summary>
        private TemplateStatus<Size> TerminalSize = new TemplateStatus<Size>(new Size() { Width = 80, Height = 40 });
        #endregion

        #region エコー
        /// <summary>
        /// エコー
        /// </summary>
        private TemplateStatus<bool> ServerEcho = new TemplateStatus<bool>(false);
        /// <summary>
        /// エコー
        /// </summary>
        private TemplateStatus<bool> LocalEcho = new TemplateStatus<bool>(false);
        #endregion

        #region go_ahead
        private TemplateStatus<bool> GoAhead = new TemplateStatus<bool>(false);
        #endregion

        #region terminal_type
        private TemplateStatus<string> TerminalType = new TemplateStatus<string>("xterm");
        #endregion

        #region terminal_speed
        private TemplateStatus<Dictionary<string,int>> TerminalSpeed = new TemplateStatus<Dictionary<string, int>>(new Dictionary<string, int>());
        #endregion

        #region display_location
        private TemplateStatus<Object> DisplayLocation = new TemplateStatus<Object>();
        #endregion

        #region environment_option
        private TemplateStatus<Object> EnvironmentOption = new TemplateStatus<Object>();
        #endregion

        #region status
        private TemplateStatus<bool> Status = new TemplateStatus<bool>(false);
        #endregion

        #region remote_flow_control
        private TemplateStatus<bool> RemoteFlowControl = new TemplateStatus<bool>(false);
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NetworkVirtualTerminal()
        {
            // デフォルト設定
            this.m_SendEncoding = Encoding.GetEncoding("UTF-8");
            this.m_RecvEncoding = Encoding.GetEncoding("UTF-8");
            this.TerminalSize.Value.Width = 124;
            this.TerminalSize.Value.Height = 32;
            this.ServerEcho.Value = true;
            this.LocalEcho.Value = false;
            this.GoAhead.Value = false;
            this.TerminalSpeed.Value["min"] = 38400;
            this.TerminalSpeed.Value["max"] = 38400;
        }

        #region ネゴシエーション
        /// <summary>
        /// ネゴシエーション
        /// </summary>
        /// <param name="recive"></param>
        /// <returns></returns>
        public MemoryStream Negotiation(MemoryStream recive)
        {
            //Debug.WriteLine(this.ToString(recive.ToArray(), (int)recive.Length));

            // 解析
            Debug.WriteLine("受信データ：");
            List<NegotiationInfomation> parseResult = this.Parse(recive.ToArray());

            // 応答作成
            MemoryStream _NegotiationMemoryStream = this.Response(parseResult);

            // MemorStreamを返却する
            return _NegotiationMemoryStream;
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
            // 返却オブジェクト生成
            List<NegotiationInfomation> negotiationInfomations = new List<NegotiationInfomation>();

            for (int i = 0; i < parseData.Length;)
            {
                //Debug.WriteLine("解析データ：0x" + string.Format("{0,2:x2}", parseData[i]));
                if (parseData[i] == (byte)TelnetCommand.IAC)
                {
                    // 解析
                    NegotiationInfomation negotiationInfomation = this.ParseIAC(parseData, ref i);
                    if (negotiationInfomation != null)
                    {
                        // リスト追加
                        Debug.WriteLine("　" + negotiationInfomation.ToString());
                        negotiationInfomations.Add(negotiationInfomation);
                    }
                    else
                    {
                        // TODO:例外
                        break;
                    }
                }
                else
                {
                    // TODO:例外
                    Debug.Fail("Illical Telnet Command(Not IAC).");
                    break;
                }
            }

            // 返却
            return negotiationInfomations;
        }

        private NegotiationInfomation ParseIAC(byte[] parseData, ref int index)
        {
            // インデックス更新
            index++;

            // Telnetコマンド毎に分岐
            switch (parseData[index])
            {
                case (byte)TelnetCommand.DO:
                    return this.ParseDO(parseData, ref index);
                case (byte)TelnetCommand.DONT:
                    return this.ParseDONT(parseData, ref index);
                case (byte)TelnetCommand.WILL:
                    return this.ParseWILL(parseData, ref index);
                case (byte)TelnetCommand.WONT:
                    return this.ParseWONT(parseData, ref index);
                case (byte)TelnetCommand.SB:
                    return this.ParseSB(parseData, ref index);
                case (byte)TelnetCommand.SE:
                    return this.ParseSE(parseData, ref index);
                default:
                    // TODO:異常終了
                    Debug.WriteLine("[ParseIAC] UNKNOWN Telnet Command(0x" + string.Format("{0,2:x2}", parseData[index]) + ").");
                    return null;
            }
        }

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

        private NegotiationInfomation ParseSB(byte[] parseData, ref int index)
        {
            // 返却オブジェクト生成
            NegotiationInfomation negotiationInfomation = new NegotiationInfomation() { Command = TelnetCommand.SB };

            // インデックス更新
            index++;

            // Telnetオプション設定
            negotiationInfomation.Option = (TelnetOption)parseData[index++];

            // データ部格納
            for (; ; index++)
            {
                if (parseData[index] == 0xff && parseData[index + 1] == 0xf0)
                {
                    break;
                }
                if (negotiationInfomation.Stream == null)
                {
                    negotiationInfomation.Stream = new MemoryStream();
                }
                negotiationInfomation.Stream.WriteByte(parseData[index]);
            }

            // 返却
            return negotiationInfomation;
        }

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

        #region 応答(Response)
        /// <summary>
        /// 応答
        /// </summary>
        /// <param name="infoList"></param>
        /// <returns></returns>
        private MemoryStream Response(List<NegotiationInfomation> infoList)
        {
            // 返却オブジェクト生成
            MemoryStream memoryStream = new MemoryStream();

            this.ResponseServerEcho(memoryStream);
            this.ResponseGoAhead(memoryStream);

            // リスト数分繰返し
            foreach (NegotiationInfomation info in infoList)
            {
                // コマンド毎に分岐
                switch(info.Command)
                {
                    case TelnetCommand.DO:
                        this.ResponseDO(info, memoryStream);
                        break;
                    case TelnetCommand.DONT:
                        this.ResponseDONT(info, memoryStream);
                        break;
                    case TelnetCommand.WILL:
                        this.ResponseWILL(info, memoryStream);
                        break;
                    case TelnetCommand.WONT:
                        this.ResponseWONT(info, memoryStream);
                        break;
                    case TelnetCommand.SB:
                        this.ResponseSB(info, memoryStream);
                        break;
                    case TelnetCommand.SE:
                        this.ResponseSE(info, memoryStream);
                        break;
                    default:
                        // 異常終了
                        Debug.WriteLine("[Response] UNKNOWN Telnet Command(" + info.Command.ToString() + ").");
                        return null;
                }
            }

            // 返却
            Debug.WriteLine("応答データ：");
            this.Parse(memoryStream.ToArray());
            return memoryStream;
        }
        private void ResponseServerEcho(MemoryStream stream)
        {
            if (this.ServerEcho.Send.DO == false)
            {
                stream.WriteByte((byte)TelnetCommand.IAC);
                if (this.ServerEcho.Value)
                {
                    stream.WriteByte((byte)TelnetCommand.DO);
                }
                else
                {
                    stream.WriteByte((byte)TelnetCommand.DONT);
                }
                stream.WriteByte((byte)TelnetOption.echo);
                this.ServerEcho.Send.DO = true;
            }
        }
        private void ResponseGoAhead(MemoryStream stream)
        {
            if (this.GoAhead.Send.DO == false)
            {
                if (!this.GoAhead.Value)
                {
                    stream.WriteByte((byte)TelnetCommand.IAC);
                    stream.WriteByte((byte)TelnetCommand.DO);
                    stream.WriteByte((byte)TelnetOption.suppress_go_ahead);
                }
                this.GoAhead.Send.DO = true;
            }
            if (this.GoAhead.Send.WILL == false)
            {
                if (!this.GoAhead.Value)
                {
                    stream.WriteByte((byte)TelnetCommand.IAC);
                    stream.WriteByte((byte)TelnetCommand.WILL);
                    stream.WriteByte((byte)TelnetOption.suppress_go_ahead);
                }
                this.GoAhead.Send.WILL = true;
            }
        }

        #region ResponseDO
        private void ResponseDO(NegotiationInfomation info, MemoryStream stream)
        {
            // オプション毎に分岐
            switch(info.Option)
            {
                case TelnetOption.echo:
                    this.ResponseDO_echo(info, stream);
                    break;
                case TelnetOption.terminal_type:
                    this.ResponseDO_terminal_type(info, stream);
                    break;
                case TelnetOption.terminal_speed:
                    this.ResponseDO_terminal_speed(info, stream);
                    break;
                case TelnetOption.window_size:
                    this.ResponseDO_window_size(info, stream);
                    break;
                case TelnetOption.display_location:
                    this.ResponseDO_display_location(info, stream);
                    break;
                case TelnetOption.environment_option:
                    this.ResponseDO_environment_option(info, stream);
                    break;
                case TelnetOption.suppress_go_ahead:
                    this.ResponseDO_suppress_go_ahead(info, stream);
                    break;
                case TelnetOption.remote_flow_control:
                    this.ResponseDO_remote_flow_control(info, stream);
                    break;
                default:
                    // TODO:異常終了
                    Debug.WriteLine("Response [DO] UNKNOWN Telnet Option(" + info.Option.ToString() + ").");
                    break;
            }
        }
        private void ResponseDO_echo(NegotiationInfomation info, MemoryStream stream)
        {
            if (!this.LocalEcho.Recv.DO)
            {
                if (this.LocalEcho.Value)
                {
                    stream.WriteByte((byte)TelnetCommand.IAC);
                    stream.WriteByte((byte)TelnetCommand.WILL);
                    stream.WriteByte((byte)TelnetOption.echo);
                    this.LocalEcho.Recv.DO = true;
                    this.LocalEcho.Send.WILL = true;
                }
                else
                {
                    stream.WriteByte((byte)TelnetCommand.IAC);
                    stream.WriteByte((byte)TelnetCommand.WONT);
                    stream.WriteByte((byte)TelnetOption.echo);
                    this.LocalEcho.Recv.DO = true;
                    this.LocalEcho.Send.WONT = true;
                }
            }
            else
            {
                // TODO:既に受信していた場合どうする？
            }
        }
        private void ResponseDO_terminal_type(NegotiationInfomation info, MemoryStream stream)
        {
            if (!this.TerminalType.Recv.DO)
            {
                stream.WriteByte((byte)TelnetCommand.IAC);
                stream.WriteByte((byte)TelnetCommand.WILL);
                stream.WriteByte((byte)TelnetOption.terminal_type);
                this.TerminalType.Recv.DO = true;
                this.TerminalType.Send.WILL = true;
            }
            else
            {
                // TODO:既に受信していた場合どうする？
            }
        }
        private void ResponseDO_terminal_speed(NegotiationInfomation info, MemoryStream stream)
        {
            if (!this.TerminalSpeed.Recv.DO)
            {
                stream.WriteByte((byte)TelnetCommand.IAC);
                stream.WriteByte((byte)TelnetCommand.WILL);
                stream.WriteByte((byte)TelnetOption.terminal_speed);
                this.TerminalSpeed.Recv.DO = true;
                this.TerminalSpeed.Send.WILL = true;
            }
            else
            {
                // TODO:既に受信していた場合どうする？
            }
        }
        private void ResponseDO_window_size(NegotiationInfomation info, MemoryStream stream)
        {
            if (!this.TerminalSize.Recv.DO)
            {
                stream.WriteByte((byte)TelnetCommand.IAC);
                stream.WriteByte((byte)TelnetCommand.SB);
                stream.WriteByte((byte)TelnetOption.window_size);

                byte[] width = BitConverter.GetBytes(this.TerminalSize.Value.Width);
                byte[] height = BitConverter.GetBytes(this.TerminalSize.Value.Height);
                stream.WriteByte(width[1]);
                stream.WriteByte(width[0]);
                stream.WriteByte(height[1]);
                stream.WriteByte(height[0]);

                stream.WriteByte((byte)TelnetCommand.IAC);
                stream.WriteByte((byte)TelnetCommand.SE);

                this.TerminalSize.Recv.DO = true;
                this.TerminalSize.Send.SB = true;
                this.TerminalSize.Send.SE = true;
            }
            else
            {
                // TODO:既に受信していた場合どうする？
            }
        }
        private void ResponseDO_display_location(NegotiationInfomation info, MemoryStream stream)
        {
            if (!this.DisplayLocation.Recv.DO)
            {
                this.DisplayLocation.Recv.DO = true;

                if (this.DisplayLocation.Value == null)
                {
                    stream.WriteByte((byte)TelnetCommand.IAC);
                    stream.WriteByte((byte)TelnetCommand.WONT);
                    stream.WriteByte((byte)TelnetOption.display_location);
                    this.DisplayLocation.Send.WONT = true;
                }
                else
                {
                    stream.WriteByte((byte)TelnetCommand.IAC);
                    stream.WriteByte((byte)TelnetCommand.WILL);
                    stream.WriteByte((byte)TelnetOption.display_location);
                    this.DisplayLocation.Send.WILL = true;
                }
            }
            else
            {
                // TODO:既に受信していた場合どうする？
            }
        }
        private void ResponseDO_environment_option(NegotiationInfomation info, MemoryStream stream)
        {
            if (!this.EnvironmentOption.Recv.DO)
            {
                this.EnvironmentOption.Recv.DO = true;

                if (this.EnvironmentOption.Value == null)
                {
                    stream.WriteByte((byte)TelnetCommand.IAC);
                    stream.WriteByte((byte)TelnetCommand.WONT);
                    stream.WriteByte((byte)TelnetOption.environment_option);
                    this.EnvironmentOption.Send.WONT = true;
                }
                else
                {
                    stream.WriteByte((byte)TelnetCommand.IAC);
                    stream.WriteByte((byte)TelnetCommand.WILL);
                    stream.WriteByte((byte)TelnetOption.environment_option);
                    this.EnvironmentOption.Send.WILL = true;
                }
            }
            else
            {
                // TODO:既に受信していた場合どうする？
            }
        }
        private void ResponseDO_suppress_go_ahead(NegotiationInfomation info, MemoryStream stream)
        {
            this.ResponseGoAhead(stream);
        }
        private void ResponseDO_remote_flow_control(NegotiationInfomation info, MemoryStream stream)
        {
            if (!this.RemoteFlowControl.Recv.DO)
            {
                if (this.RemoteFlowControl.Value)
                {
                    stream.WriteByte((byte)TelnetCommand.IAC);
                    stream.WriteByte((byte)TelnetCommand.WILL);
                    stream.WriteByte((byte)TelnetOption.remote_flow_control);
                    this.RemoteFlowControl.Recv.DO = true;
                    this.RemoteFlowControl.Send.WILL = true;
                }
                else
                {
                    stream.WriteByte((byte)TelnetCommand.IAC);
                    stream.WriteByte((byte)TelnetCommand.WONT);
                    stream.WriteByte((byte)TelnetOption.remote_flow_control);
                    this.RemoteFlowControl.Recv.DO = true;
                    this.RemoteFlowControl.Send.WONT = true;
                }
            }
            else
            {
                // TODO:既に受信していた場合どうする？
            }
        }
        #endregion

        #region ResponseDONT
        private void ResponseDONT(NegotiationInfomation info, MemoryStream stream)
        {
            // オプション毎に分岐
            switch (info.Option)
            {
                default:
                    // TODO:異常終了
                    Debug.WriteLine("Response [DONT] UNKNOWN Telnet Option(" + info.Option.ToString() + ").");
                    break;
            }
        }
        #endregion

        #region ResponseWILL
        private void ResponseWILL(NegotiationInfomation info, MemoryStream stream)
        {
            // オプション毎に分岐
            switch (info.Option)
            {
                case TelnetOption.echo:
                    this.ResponseWILL_server_echo(info, stream);
                    break;
                case TelnetOption.suppress_go_ahead:
                    this.ResponseWILL_suppress_go_ahead(info, stream);
                    break;
                case TelnetOption.status:
                    this.ResponseWILL_status(info, stream);
                    break;
                default:
                    // TODO:異常終了
                    Debug.WriteLine("Response [WILL] UNKNOWN Telnet Option(" + info.Option.ToString() + ").");
                    break;
            }
        }
        private void ResponseWILL_server_echo(NegotiationInfomation info, MemoryStream stream)
        {
            if (this.ServerEcho.Send.DO)
            {
                this.ServerEcho.Recv.WILL = true;
                this.ServerEcho.Negotiated = true;
            }
            else
            {
                // TODO:異常終了?
                Debug.Fail("ネゴシエーション失敗(echo)");
            }
        }
        private void ResponseWILL_suppress_go_ahead(NegotiationInfomation info, MemoryStream stream)
        {
            if (this.GoAhead.Send.DO)
            {
                this.GoAhead.Recv.WILL = true;
                this.GoAhead.Negotiated = true;
            }
            else
            {
                // TODO:異常終了?
                Debug.Fail("ネゴシエーション失敗(go ahead)");
            }
        }
        private void ResponseWILL_status(NegotiationInfomation info, MemoryStream stream)
        {
            if (!this.Status.Recv.WILL)
            {
                this.Status.Recv.WILL = true;

                if (this.Status.Value)
                {
                    stream.WriteByte((byte)TelnetCommand.IAC);
                    stream.WriteByte((byte)TelnetCommand.DO);
                    stream.WriteByte((byte)TelnetOption.status);
                    this.Status.Send.DO = true;
                }
                else
                {
                    stream.WriteByte((byte)TelnetCommand.IAC);
                    stream.WriteByte((byte)TelnetCommand.DONT);
                    stream.WriteByte((byte)TelnetOption.status);
                    this.Status.Send.DONT = true;
                }
            }
            else
            {
                // WILL受信済みならどうするか？
            }
        }
        #endregion

        #region ResponseWONT
        private void ResponseWONT(NegotiationInfomation info, MemoryStream stream)
        {
            // オプション毎に分岐
            switch (info.Option)
            {
                default:
                    // TODO:異常終了
                    Debug.WriteLine("Response [WONT] UNKNOWN Telnet Option(" + info.Option.ToString() + ").");
                    break;
            }
        }
        #endregion

        #region ResponseSB
        private void ResponseSB(NegotiationInfomation info, MemoryStream stream)
        {
            // オプション毎に分岐
            switch (info.Option)
            {
                case TelnetOption.terminal_type:
                    this.ResponseSB_terminal_type(info, stream);
                    break;
                case TelnetOption.terminal_speed:
                    this.ResponseSB_terminal_speed(info, stream);
                    break;
                default:
                    // TODO:異常終了
                    Debug.WriteLine("Response [DO] UNKNOWN Telnet Option(" + info.Option.ToString() + ").");
                    break;
            }
        }
        private void ResponseSB_terminal_type(NegotiationInfomation info, MemoryStream stream)
        {
            stream.WriteByte((byte)TelnetCommand.IAC);
            stream.WriteByte((byte)TelnetCommand.SB);
            stream.WriteByte((byte)TelnetOption.terminal_type);

            if (info.Stream != null && info.Stream.ToArray()[0] == 0x01)
            {
                // ASCII エンコード
                byte[] byteData = System.Text.Encoding.ASCII.GetBytes(this.TerminalType.Value);
                stream.WriteByte(0x00);
                stream.Write(byteData, 0, byteData.Length);
            }
            else
            {
                Debug.Fail("ResponseSB_terminal_type");
            }

            stream.WriteByte((byte)TelnetCommand.IAC);
            stream.WriteByte((byte)TelnetCommand.SE);
        }
        private void ResponseSB_terminal_speed(NegotiationInfomation info, MemoryStream stream)
        {
            stream.WriteByte((byte)TelnetCommand.IAC);
            stream.WriteByte((byte)TelnetCommand.SB);
            stream.WriteByte((byte)TelnetOption.terminal_speed);

            if (info.Stream != null && info.Stream.ToArray()[0] == 0x01)
            {
                // ASCII エンコード
                byte[] byteMinData = System.Text.Encoding.ASCII.GetBytes(this.TerminalSpeed.Value["min"].ToString());
                byte[] byteMaxData = System.Text.Encoding.ASCII.GetBytes(this.TerminalSpeed.Value["max"].ToString());

                stream.WriteByte(0x00);
                stream.Write(byteMinData, 0, byteMinData.Length);
                stream.WriteByte(Convert.ToByte(','));
                stream.Write(byteMaxData, 0, byteMaxData.Length);
            }
            else
            {
                Debug.Fail("ResponseSB_terminal_speed");
            }

            stream.WriteByte((byte)TelnetCommand.IAC);
            stream.WriteByte((byte)TelnetCommand.SE);
        }
        #endregion

        #region ResponseSE
        private void ResponseSE(NegotiationInfomation info, MemoryStream stream)
        {
            // 終端なので何もしない
        }
        #endregion

        #endregion
    }
}
