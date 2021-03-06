﻿using System.Net;
using System.Net.Sockets;

namespace Common.Net
{
    /// <summary>
    /// Ntpクライアントクラス
    /// </summary>
    public class NtpClient
    {
        private UdpClient _connection = new UdpClient();

        public IPAddress IP { get { return _receiveEndPoint.Address; } }
        private IPEndPoint _receiveEndPoint = new IPEndPoint(System.Net.IPAddress.Any, 0);

        public string Host { get; private set; }

        public int Port { get; private set; }

        public int SendTimeout { get { return _connection.Client.SendTimeout; } set { _connection.Client.SendTimeout = value; } }

        public int ReceiveTimeout { get { return _connection.Client.ReceiveTimeout; } set { _connection.Client.ReceiveTimeout = value; } }

        public void Connect(string host)
        {
            Connect(host, 123);
        }

        public void Connect(string host, int port)
        {
            this.Host = host;
            this.Port = port;
            _connection.Connect(this.Host, this.Port);
        }

        public NtpPacket GetData()
        {
            Send();
            return Receive();
        }

        private void Send()
        {
            NtpPacket packet = NtpPacket.CreateSendPacket();
            _connection.Send(packet.PacketData, packet.PacketData.GetLength(0));
        }

        private NtpPacket Receive()
        {
            byte[] receiveData = _connection.Receive(ref _receiveEndPoint);
            return new NtpPacket(receiveData);
        }
    }
}