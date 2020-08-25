using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;

namespace Common.Net
{
    /// <summary>
    /// IPアドレスクラス
    /// </summary>
    public class IpAddress
    {
        /// <summary>
        /// ホスト名
        /// </summary>
        private string m_HostName = string.Empty;

        /// <summary>
        /// IPアドレスリスト(IPv4)
        /// </summary>
        private List<IPAddress> m_IpV4 = new List<IPAddress>();

        /// <summary>
        /// IPアドレスリスト(IPv6)
        /// </summary>
        private List<IPAddress> m_IpV6 = new List<IPAddress>();

        /// <summary>
        /// ホスト名
        /// </summary>
        public string HostName
        {
            get { return this.m_HostName; }
        }

        /// <summary>
        /// IPアドレスリスト(IPv4)
        /// </summary>
        public List<IPAddress> IpV4
        {
            get { return this.m_IpV4; }
        }

        /// <summary>
        /// IPアドレスリスト(IPv6)
        /// </summary>
        public List<IPAddress> IpV6
        {
            get { return this.m_IpV6; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IpAddress()
            : this(Dns.GetHostName())
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="hostName"></param>
        public IpAddress(string hostName)
        {
            // 初期化
            this.initialization(hostName);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="hostName"></param>
        private void initialization(string hostName)
        {
            // IPv4アドレスチェック用
            Regex _IpV4Regex = new Regex(@"^(([01]?\d{1,2}|2[0-4]\d|25[0-5])\.){3}([01]?\d{1,2}|2[0-4]\d|25[0-5])$");

            // ホスト名を設定する
            this.m_HostName = hostName;

            // ホスト名からIPアドレスを取得する
            IPAddress[] _IPAddress = Dns.GetHostAddresses(this.m_HostName);
            foreach (IPAddress address in _IPAddress)
            {
                if (_IpV4Regex.IsMatch(address.ToString()))
                {
                    // 追加(IPv4)
                    this.m_IpV4.Add(address);
                }
                else
                {
                    // 追加(IPv6)
                    this.m_IpV6.Add(address);
                }
            }
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~IpAddress()
        {
            // リストクリア
            this.m_IpV4.Clear();
            this.m_IpV6.Clear();
        }
    }
}
