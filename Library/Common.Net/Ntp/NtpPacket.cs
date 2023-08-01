using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    public class NtpPacket
    {
        const long COMPENSATING_RATE_32 = 0x100000000L;
        const double COMPENSATING_RATE_16 = 0x10000d;

        static readonly DateTime COMPENSATING_DATETIME = new DateTime(1900, 1, 1);
        static readonly DateTime PASSED_COMPENSATING_DATETIME = COMPENSATING_DATETIME.AddSeconds(uint.MaxValue);

        const int CLIENT_VERSION = 3;

        public byte[] PacketData { get; private set; }

        public DateTime NtpPacketCreatedTime { get; private set; }

        public TimeSpan DifferentTimeSpan
        {
            get
            {
                long offsetTick = ((ReceiveTimestamp - OriginateTimestamp) + (TransmitTimestamp - NtpPacketCreatedTime)).Ticks / 2;
                return new TimeSpan(offsetTick);
            }
        }

        public TimeSpan NetworkDelay
        {
            get { return ((NtpPacketCreatedTime - OriginateTimestamp) + (TransmitTimestamp - ReceiveTimestamp)); }
        }

        public NtpPacket(byte[] packetData)
        {
            PacketData = packetData;
            NtpPacketCreatedTime = DateTime.Now;
        }

        static DateTime GetCompensatingDatetime(uint seconds)
        {
            return (seconds & 0x80000000) == 0 ? PASSED_COMPENSATING_DATETIME : COMPENSATING_DATETIME;
        }

        static DateTime GetCompensatingDatetime(DateTime dateTime)
        {
            return PASSED_COMPENSATING_DATETIME <= dateTime ? PASSED_COMPENSATING_DATETIME : COMPENSATING_DATETIME;
        }

        static public NtpPacket CreateSendPacket()
        {
            byte[] packet = new byte[48];
            FillHeader(packet);
            FillTransmitTimestamp(packet);
            return new NtpPacket(packet);
        }

        static private void FillHeader(byte[] ntpPacket)
        {
            const byte li = 0x00;
            const byte mode = 0x03;

            ntpPacket[0] = (byte)(li | CLIENT_VERSION << 3 | mode);
        }

        static private void FillTransmitTimestamp(byte[] ntpPacket)
        {
            byte[] time = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(DateTimeToNtpTimeStamp(DateTime.UtcNow)));
            Array.Copy(time, 0, ntpPacket, 40, 8);
        }

        static private double SignedFixedPointToDouble(int signedFixedPoint)
        {
            short number = (short)(signedFixedPoint >> 16);
            ushort fraction = (ushort)(signedFixedPoint & short.MaxValue);

            return number + (double)fraction / COMPENSATING_RATE_16;
        }

        static private DateTime NtpTimeStampToDateTime(long ntpTimeStamp)
        {
            uint seconds = (uint)(ntpTimeStamp >> 32);
            uint secondsFraction = (uint)(ntpTimeStamp & uint.MaxValue);

            long milliseconds = (long)seconds * 1000 + (secondsFraction * 1000) / COMPENSATING_RATE_32;
            return GetCompensatingDatetime(seconds) + TimeSpan.FromMilliseconds(milliseconds);
        }

        static private long DateTimeToNtpTimeStamp(DateTime dateTime)
        {
            DateTime compensatingDatetime = GetCompensatingDatetime(dateTime);
            double ntpStandardTick = (dateTime - compensatingDatetime).TotalMilliseconds;

            uint seconds = (uint)((dateTime - compensatingDatetime).TotalSeconds);
            uint secondsFraction = (uint)((ntpStandardTick % 1000) * COMPENSATING_RATE_32 / 1000);

            return (long)(((ulong)seconds << 32) | secondsFraction);
        }

        public int LeapIndicator
        {
            get { return PacketData[0] >> 6 & 0x03; }
        }

        public int Version
        {
            get { return PacketData[0] >> 3 & 0x03; }
        }

        public int Mode
        {
            get { return PacketData[0] & 0x03; }
        }

        public int Stratum
        {
            get { return PacketData[1]; }
        }

        public int PollInterval
        {
            get
            {
                int interval = (SByte)PacketData[2];
                switch (interval)
                {
                    case (0): return 0;
                    case (1): return 1;
                    default: return (int)Math.Pow(2, interval);
                }
            }
        }

        public double Precision
        {
            get { return Math.Pow(2, (SByte)PacketData[3]); }
        }

        public double RootDelay
        {
            get { return SignedFixedPointToDouble(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(PacketData, 4))); }
        }

        public double RootDispersion
        {
            get { return SignedFixedPointToDouble(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(PacketData, 8))); }
        }

        public string ReferenceIdentifier
        {
            get
            {
                if (Stratum <= 1)
                {
                    return Encoding.ASCII.GetString(PacketData, 12, 4).TrimEnd(new char());
                }
                else
                {
                    IPAddress ip = new IPAddress(new byte[] { PacketData[12], PacketData[13], PacketData[14], PacketData[15] });
                    return ip.ToString();
                }
            }
        }

        public DateTime ReferenceTimestamp
        {
            get { return NtpTimeStampToDateTime(IPAddress.NetworkToHostOrder(BitConverter.ToInt64(PacketData, 16))).ToLocalTime(); }
        }

        public DateTime OriginateTimestamp
        {
            get { return NtpTimeStampToDateTime(IPAddress.NetworkToHostOrder(BitConverter.ToInt64(PacketData, 24))).ToLocalTime(); }
        }

        public DateTime ReceiveTimestamp
        {
            get { return NtpTimeStampToDateTime(IPAddress.NetworkToHostOrder(BitConverter.ToInt64(PacketData, 32))).ToLocalTime(); }
        }

        public DateTime TransmitTimestamp
        {
            get { return NtpTimeStampToDateTime(IPAddress.NetworkToHostOrder(BitConverter.ToInt64(PacketData, 40))).ToLocalTime(); }
        }

        public string KeyIdentifier
        {
            get
            {
                if (PacketData.Length <= 48) { return string.Empty; }
                return Encoding.ASCII.GetString(PacketData, 48, 4).TrimEnd(new char());
            }
        }

        public string MessageDigest
        {
            get
            {
                if (PacketData.Length <= 52) { return string.Empty; }
                return Encoding.ASCII.GetString(PacketData, 52, 16).TrimEnd(new char()) ?? string.Empty;
            }
        }

        #region 文字列化
        /// <summary>
        /// 文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // 結果オブジェクト生成
            StringBuilder result = new StringBuilder();

            // 文字列作成
            result.AppendFormat("LeapIndicator      : {0:X}\n", LeapIndicator);
            result.AppendFormat("Version            : {0:X}\n", Version);
            result.AppendFormat("Mode               : {0:X}\n", Mode);
            result.AppendFormat("Stratum            : {0:X}\n", Stratum);
            result.AppendFormat("PollInterval       : {0:X}\n", PollInterval);
            result.AppendFormat("Precision          : {0:0.00}\n", Precision);
            result.AppendFormat("RootDelay          : {0:0.00}\n", RootDelay);
            result.AppendFormat("RootDispersion     : {0:0.00}\n", RootDispersion);
            result.AppendFormat("ReferenceIdentifier: {0:X}\n", ReferenceIdentifier);
            result.AppendFormat("ReferenceTimestamp : {0}\n", ReferenceTimestamp);
            result.AppendFormat("OriginateTimestamp : {0}\n", OriginateTimestamp);
            result.AppendFormat("ReceiveTimestamp   : {0}\n", ReceiveTimestamp);
            result.AppendFormat("TransmitTimestamp  : {0}\n", TransmitTimestamp);
            result.AppendFormat("KeyIdentifier      : {0}\n", KeyIdentifier);
            result.AppendFormat("MessageDigest      : {0}\n", MessageDigest);

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
