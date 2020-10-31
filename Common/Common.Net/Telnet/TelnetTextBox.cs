using System;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace Common.Net
{
    /// <summary>
    /// Telnetテキストボックスクラス
    /// </summary>
    public class TelnetTextBox : RichTextBox
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TelnetTextBox()
            : base()
        {
            // 初期設定
            this.Multiline = true;
            this.ScrollBars = RichTextBoxScrollBars.Both;
        }

        /// <summary>
        /// テキスト追加
        /// </summary>
        /// <param name="text"></param>
        public void AddText(string text)
        {

            // TODO:エスケープ文字をどうするか？
            // 文字列化オブジェクト
            StringBuilder stringBuilder = new StringBuilder();

            // 文字列をbyte配列へ変換
            byte[] textByte = System.Text.Encoding.ASCII.GetBytes(text);

            // byte配列分繰り返す
            for (int i = 0; i < textByte.Length; i++)
            {
                Debug.WriteLine(string.Format("　・0x{0:x02} = [{1}]", textByte[i], text.ToCharArray()[i]));
                // ANSIエスケープコード判定
                if (textByte[i] == 0x1b)
                {
                    Debug.WriteLine("ANSIエスケープコード:[受信]");
                    for (; i < textByte.Length; i++)
                    {
                        Debug.Write(string.Format("0x{0:x02} ", textByte[i]));
                        // 'm'
                        if (textByte[i] == 0x6d)
                        {
                            break;
                        }
                    }
                    Debug.WriteLine("");
                }
                else
                {
                    // TODO:0x0dが連続する場合がるので暫定対処
                    if (i > 0)
                    {
                        if (text.ToCharArray()[i - 1] != '\r')
                        {
                            stringBuilder.Append(text.ToCharArray()[i]);
                        }
                    }
                    else
                    {
                        stringBuilder.Append(text.ToCharArray()[i]);
                    }
                }
                /*
                // ANSIエスケープコード判定
                if (textByte[i] == 0x1b)
                {
                    for (; i < textByte.Length; i++)
                    {
                        Debug.Write(string.Format("0x{0:x02} ", textByte[i]));
                        // 'A'～'H'、'J'、'K'、'S'、'T'、'f'、'm'、';'
                        if ((textByte[i] >= 0x41 && textByte[i] <= 0x48) || textByte[i] == 0x4a || textByte[i] == 0x4b ||
                             textByte[i] == 0x66 || textByte[i] == 0x6d ||
                             textByte[i] == 0x3b)
                        {
                            break;
                        }
                    }
                    Debug.WriteLine("");
                }
                else
                {
                    stringBuilder.Append(text.ToCharArray()[i]);
                }*/
            }

            // テキスト追加
            this.AppendText(stringBuilder.ToString());
        }
    }
}
