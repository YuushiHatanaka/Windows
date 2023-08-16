using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace Common.Control
{
    public class IpAddressTextbox : MaskedTextBox
    {
        /// <summary>
        /// エラー表示オブジェクト
        /// </summary>
        private ErrorProvider m_ErrorProvider = new ErrorProvider();

        /// <summary>
        /// 値
        /// </summary>
        public string Value
        {
            get
            {
                return this.Text.Replace(" ","");
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IpAddressTextbox()
            : base()
        {
            this.Mask = @"990\.990\.990\.990";
            this.PromptChar = ' ';
            this.ResetOnSpace = true;
            this.Validating += ValidatingEvent;

            // エラー設定
            this.m_ErrorProvider.SetIconAlignment(this, ErrorIconAlignment.MiddleRight);
            this.m_ErrorProvider.SetIconPadding(this, 2);
            this.m_ErrorProvider.BlinkRate = 1000;
            this.m_ErrorProvider.BlinkStyle = ErrorBlinkStyle.AlwaysBlink;
        }

        /// <summary>
        /// Validating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValidatingEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.Value == "...")
            {
                return;
            }

            Console.WriteLine(this.Text);
            Console.WriteLine(this.Value);

            IPAddress address;
            if (!IPAddress.TryParse(this.Value, out address))
            {
                this.m_ErrorProvider.SetError(this, "IPアドレスの形式が不正です");
            }
            else
            {
                this.m_ErrorProvider.SetError(this, string.Empty);
            }
        }
    }
}
