using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;

namespace Common.Control
{
    public class NetworkAdapterListView : ListView
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NetworkAdapterListView()
            : base()
        {
            // 初期化
            Initialization();
        }
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            // 2次バッファ設定
            this.DoubleBuffered = true;

            // 列ヘッダ初期化
            ColumnHeaderInitialization();

            // 属性初期化
            AttributeInitialization();
        }
        /// <summary>
        /// 列ヘッダ初期化
        /// </summary>
        private void ColumnHeaderInitialization()
        {
            // 列ヘッダ初期設定
            ColumnHeader[] _ColumnHeader = new ColumnHeader[]
                {
                new ColumnHeader() { Text = "名称", Width = 144, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "説明", Width = 144, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "I/F種類", Width = 64, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "最大速度", Width = 64, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "MACアドレス", Width = 128, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "ユニキャストアドレス", Width = 96, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "IPv4マスク", Width = 96, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "DNS", Width = 96, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "DHCP", Width = 96, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "Gateway", Width = 96, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "WINS", Width = 96, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "受信バイト数", Width = 96, TextAlign = HorizontalAlignment.Right },
                new ColumnHeader() { Text = "送信バイト数", Width = 96, TextAlign = HorizontalAlignment.Right },
                new ColumnHeader() { Text = "受信したユニキャストパケット数", Width = 96, TextAlign = HorizontalAlignment.Right },
                new ColumnHeader() { Text = "送信したユニキャストパケット数", Width = 96, TextAlign = HorizontalAlignment.Right },
                };

            // 列ヘッダをリストに追加
            this.Columns.AddRange(_ColumnHeader);
        }
        /// <summary>
        /// 属性初期化
        /// </summary>
        private void AttributeInitialization()
        {
            // 初期設定
            FullRowSelect = true;                           // 全ての行を選択状態
            GridLines = true;                               // グリッド線を表示
            HeaderStyle = ColumnHeaderStyle.Nonclickable;   // ヘッダはクリック不可
            MultiSelect = false;                            // 複数行選択不可
            View = View.Details;                            // 詳細ビュー
        }
        /// <summary>
        /// リスト初期化
        /// </summary>
        public void AllClearList()
        {
            // 更新開始
            this.BeginUpdate();

            // 項目クリア
            this.Items.Clear();

            // 更新終了
            this.EndUpdate();
        }
        /// <summary>
        /// 現在のリストにあって、最新のリストにないもを削除する
        /// </summary>
        private void Delete()
        {
            // すべてのネットワークインターフェイスを取得する
            NetworkInterface[] _NetworkInterfaceList = NetworkInterface.GetAllNetworkInterfaces();

            // 現在のリストにあって、最新のリストにないものは削除
            foreach (ListViewItem _ListViewItem in this.Items)
            {
                // すべてのネットワークインターフェイス分繰り返し
                foreach (NetworkInterface _NetworkInterface in _NetworkInterfaceList)
                {
                    if (_ListViewItem.Text == _NetworkInterface.Name)
                    {
                        // 互いのリストに存在するので削除しない
                        return;
                    }
                }

                // 最新リストにないので削除
                this.Items.Remove(_ListViewItem);
            }
        }
        /// <summary>
        /// リスト更新
        /// </summary>
        public void UpdateList()
        {
            // 更新開始
            this.BeginUpdate();

            // リスト削除
            Delete();

            // すべてのネットワークインターフェイスを取得する
            NetworkInterface[] _NetworkInterfaceList = NetworkInterface.GetAllNetworkInterfaces();

            // すべてのネットワークインターフェイス分繰り返し
            foreach (NetworkInterface _NetworkInterface in _NetworkInterfaceList)
            {
                Debug.WriteLine("----------------------------------------");
                Debug.WriteLine("ネットワークインターフェイス情報");
                Debug.WriteLine("----------------------------------------");
                Debug.WriteLine("名称       : " + _NetworkInterface.Name);
                Debug.WriteLine("説明       : " + _NetworkInterface.Description);
                Debug.WriteLine("I/F種類    : " + _NetworkInterface.NetworkInterfaceType.ToString());
                Debug.WriteLine("最大速度   : " + (_NetworkInterface.Speed / 1000000).ToString() + "Mbps");
                Debug.WriteLine("macアドレス: " + _NetworkInterface.GetPhysicalAddress().ToString());
                Debug.WriteLine("送信byte数 : " + _NetworkInterface.GetIPv4Statistics().BytesSent.ToString());
                Debug.WriteLine("受信byte数 : " + _NetworkInterface.GetIPv4Statistics().BytesReceived.ToString());

                // 登録がなければリストに追加
                ListViewItem _ListViewItem = this.FindItemWithText(_NetworkInterface.Name);
                if (_ListViewItem == null)
                {
                    _ListViewItem = this.Items.Add(_NetworkInterface.Name);
                    for (int i = 1; i < this.Columns.Count; i++)
                    {
                        _ListViewItem.SubItems.Add("");
                    }
                }
                else
                {
                    for (int i = 1; i < this.Columns.Count; i++)
                    {
                        _ListViewItem.SubItems[i].Text = "";
                    }
                }

                int _SubItemsIndex = 1;
                _ListViewItem.SubItems[_SubItemsIndex].Text += _NetworkInterface.Description;
                _ListViewItem.SubItems[_SubItemsIndex + 1].Text += _NetworkInterface.NetworkInterfaceType.ToString();
                _ListViewItem.SubItems[_SubItemsIndex + 2].Text += (_NetworkInterface.Speed / 1000000).ToString() + "Mbps";
                _ListViewItem.SubItems[_SubItemsIndex + 3].Text += _NetworkInterface.GetPhysicalAddress().ToString();
                _SubItemsIndex += 4;

                //ネットワーク接続しているか調べる
                if (_NetworkInterface.OperationalStatus == OperationalStatus.Up &&
                    _NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    _NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                {

                    // 接続している場合
                    // 構成情報、アドレス情報を取得する
                    IPInterfaceProperties _IPInterfaceProperties = _NetworkInterface.GetIPProperties();
                    if (_IPInterfaceProperties != null)
                    {
                        foreach (UnicastIPAddressInformation _UnicastIPAddressInformation in _IPInterfaceProperties.UnicastAddresses)
                        {
                            _ListViewItem.SubItems[_SubItemsIndex].Text += _UnicastIPAddressInformation.Address + " ";
                            _ListViewItem.SubItems[_SubItemsIndex + 1].Text += _UnicastIPAddressInformation.IPv4Mask + " ";
                            Debug.WriteLine("ユニキャストアドレス:{0}", _UnicastIPAddressInformation.Address);
                            Debug.WriteLine("IPv4マスク:{0}", _UnicastIPAddressInformation.IPv4Mask);
                        }
                        _SubItemsIndex += 2;

                        foreach (IPAddress _IPAddress in _IPInterfaceProperties.DnsAddresses)
                        {
                            _ListViewItem.SubItems[_SubItemsIndex].Text += _IPAddress.ToString() + " ";
                            Debug.WriteLine("DNS:" + _IPAddress.ToString());
                        }
                        _SubItemsIndex += 1;

                        foreach (IPAddress _IPAddress in _IPInterfaceProperties.DhcpServerAddresses)
                        {
                            _ListViewItem.SubItems[_SubItemsIndex].Text += _IPAddress.ToString() + " ";
                            Debug.WriteLine("DHCP:" + _IPAddress.ToString());
                        }
                        _SubItemsIndex += 1;

                        foreach (GatewayIPAddressInformation _IPAddress in _IPInterfaceProperties.GatewayAddresses)
                        {
                            _ListViewItem.SubItems[_SubItemsIndex].Text += _IPAddress.Address.ToString() + " ";
                            Debug.WriteLine("Gateway:" + _IPAddress.Address.ToString());
                        }
                        _SubItemsIndex += 1;

                        foreach (System.Net.IPAddress _IPAddress in _IPInterfaceProperties.WinsServersAddresses)
                        {
                            _ListViewItem.SubItems[_SubItemsIndex].Text += _IPAddress.ToString() + " ";
                            Debug.WriteLine("WINS:" + _IPAddress.ToString());
                        }
                        _SubItemsIndex += 1;
                    }
                    // IPv4の統計情報を表示する
                    if (_NetworkInterface.Supports(NetworkInterfaceComponent.IPv4))
                    {
                        IPv4InterfaceStatistics _IPv4InterfaceStatistics = _NetworkInterface.GetIPv4Statistics();
                        _ListViewItem.SubItems[_SubItemsIndex].Text += _IPv4InterfaceStatistics.BytesReceived.ToString("#,0") + " ";
                        _ListViewItem.SubItems[_SubItemsIndex + 1].Text += _IPv4InterfaceStatistics.BytesSent.ToString("#,0") + " ";
                        _ListViewItem.SubItems[_SubItemsIndex + 1].Text += _IPv4InterfaceStatistics.BytesReceived.ToString("#,0") + " ";
                        _ListViewItem.SubItems[_SubItemsIndex + 1].Text += _IPv4InterfaceStatistics.BytesSent.ToString("#,0") + " ";
                        Debug.WriteLine("受信バイト数:" + _IPv4InterfaceStatistics.BytesReceived);
                        Debug.WriteLine("送信バイト数:" + _IPv4InterfaceStatistics.BytesSent);
                        Debug.WriteLine("受信したユニキャストパケット数:" + _IPv4InterfaceStatistics.UnicastPacketsReceived);
                        Debug.WriteLine("送信したユニキャストパケット数:" + _IPv4InterfaceStatistics.UnicastPacketsSent);
                    }
                    Debug.WriteLine("");
                }
                else
                {
                    // 接続していない場合
                }
            }

            // 更新終了
            this.EndUpdate();
        }
    }
}