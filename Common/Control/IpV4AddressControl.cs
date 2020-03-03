using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.Net;

namespace Common.Windows.Forms
{
    /// <summary>
    /// IpV4入力テキストボックス
    /// </summary>
    public partial class IpV4AddressControl : UserControl
    {
        /// <summary>
        /// アドレス入力テキストボックス
        /// </summary>
        private List<TextBox> m_textBoxAddress = new List<TextBox>(4);

        /// <summary>
        /// ピリオドラベル
        /// </summary>
        private List<Label> m_labelPeriod = new List<Label>(3);

        /// <summary>
        /// 文字列返却
        /// </summary>
        public string AddresString
        {
            set
            {
                // 設定
                this.Value(value);
            }
            get
            {
                // 変換文字列を設定
                string _IPAddress =
                    this.m_textBoxAddress[0].Text +
                    this.m_labelPeriod[0].Text +
                    this.m_textBoxAddress[1].Text +
                    this.m_labelPeriod[1].Text +
                    this.m_textBoxAddress[2].Text +
                    this.m_labelPeriod[2].Text +
                    this.m_textBoxAddress[3].Text;
                return _IPAddress;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IpV4AddressControl()
        {
            // コンポーネント初期化
            InitializeComponent();

            // 初期化
            this.Initialization();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            // アドレス入力テキストボックス初期化
            for (int i = 0; i < 4; i++)
            {
                // オブジェクト生成
                TextBox _IpAddress = new TextBox();
                _IpAddress.Dock = DockStyle.Fill;
                _IpAddress.MaxLength = 3;
                _IpAddress.Multiline = false;
                _IpAddress.BorderStyle = BorderStyle.None;
                _IpAddress.TextAlign = HorizontalAlignment.Center;
                _IpAddress.KeyPress += this.OnKeyPress;

                // パネル登録
                this.tableLayoutPanel.Controls.Add(_IpAddress, i, 0);

                // オブジェクト登録
                this.m_textBoxAddress.Add(_IpAddress);
            }

            // ピリオドラベル初期化
            for (int i = 0; i < 3; i++)
            {
                // オブジェクト生成
                Label _Period = new Label();
                _Period.Text = ".";
                _Period.BackColor = this.m_textBoxAddress[0].BackColor;
                _Period.Dock = DockStyle.Fill;
                _Period.TextAlign = ContentAlignment.BottomCenter;

                // パネル登録
                this.tableLayoutPanel.Controls.Add(_Period, i + 1, 0);

                // オブジェクト登録
                this.m_labelPeriod.Add(_Period);
            }

            // コントロール調整
            this.tableLayoutPanel.BackColor = this.m_textBoxAddress[0].BackColor;
            this.tableLayoutPanel.Dock = DockStyle.Fill;
        }

        #region IPアドレス
        #region 設定
        /// <summary>
        /// 設定
        /// </summary>
        /// <param name="value"></param>
        public void Value(string value)
        {
            // 設定
            this.Value(IPAddress.Parse(value));
        }

        /// <summary>
        /// 設定
        /// </summary>
        /// <param name="value"></param>
        public void Value(IPAddress value)
        {
            // 設定
            this.Value(value.GetAddressBytes());
        }

        /// <summary>
        /// 設定
        /// </summary>
        /// <param name="value"></param>
        public void Value(byte[] value)
        {
            this.m_textBoxAddress[0].Text = value[0].ToString();
            this.m_textBoxAddress[1].Text = value[1].ToString();
            this.m_textBoxAddress[2].Text = value[2].ToString();
            this.m_textBoxAddress[3].Text = value[3].ToString();
        }
        #endregion

        #region 取得
        /// <summary>
        /// 取得
        /// </summary>
        public IPAddress Value()
        {
            return IPAddress.Parse(this.Text);
        }
        #endregion
        #endregion

        /// <summary>
        /// [Event] KeyPress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            // 入力チェック
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == '\b'))
            {
                // 入力拒否
                e.Handled = true;
            }
        }
    }
}