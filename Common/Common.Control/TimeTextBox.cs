using System;
using System.Globalization;
using System.Windows.Forms;

namespace Common.Control
{
    public class TimeTextBox : MaskedTextBox
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
                string hh = this.Text.Substring(0, 2).Replace(" ","");
                string mm = this.Text.Substring(2).Replace(" ", "");
                return hh + ":" + mm;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TimeTextBox()
            : base()
        {
            this.Mask = @"9000";
            this.PromptChar = ' ';
            this.ResetOnSpace = true;
            this.ValidatingType = typeof(DateTime);
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
            if (this.Text == string.Empty)
            {
                return;
            }

            Console.WriteLine(this.Text);
            Console.WriteLine(this.Value);

            string[] format = { "H:m", "H:mm", "HH:m", "HH:mm" };
            CultureInfo ci = CultureInfo.CurrentCulture;
            DateTimeStyles dts = DateTimeStyles.None;

            DateTime dateTime;
            if (!DateTime.TryParseExact(this.Value, format, ci, dts, out dateTime))
            {
                this.m_ErrorProvider.SetError(this, "時間の形式が不正です");
            }
            else
            {
                this.m_ErrorProvider.SetError(this, string.Empty);
            }
        }
    }
}
