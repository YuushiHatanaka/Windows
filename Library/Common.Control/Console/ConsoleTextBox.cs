using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Common.Control
{
    /// <summary>
    /// ConsoleTextBoxクラス
    /// </summary>
    public class ConsoleTextBox : TextBox
    {
        /// <summary>
        /// コマンド検出 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void CommandDetectionEventHandler(object sender, CommandDetectionEventArgs e);

        /// <summary>
        /// コマンド検出 event
        /// </summary>
        public event CommandDetectionEventHandler OnCommandDetection = delegate { };

        /// <summary>
        /// プロンプト
        /// </summary>
        public string Prompt { get; set; } = string.Empty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConsoleTextBox()
        {
            // コントロールの背景色を設定
            ForeColor = Color.White;
            BackColor = Color.Black;

            Multiline = true;
            ScrollBars = ScrollBars.Both;
            WordWrap = false;
            ReadOnly = true;

            // フォントを設定
            Font = new Font("Consolas", 10f);

            // キーボード入力を受け取る
            KeyDown += ConsoleTextBox_KeyDown;
            KeyPress += ConsoleTextBox_KeyPress;
        }

        /// <summary>
        /// ConsoleTextBox_KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConsoleTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // CTRL+Cが押されているか？
            if (e.KeyData == (Keys.Control | Keys.C))
            {
                ;
            }
            else
            {
                // 押されたキーを取得して、コンソールに出力する
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        // TODO:ヒストリー処理
                        e.Handled = true;
                        break;
                    case Keys.Down:
                        // TODO:ヒストリー処理
                        e.Handled = true;
                        break;
                    case Keys.Left:
                        e.Handled = true;
                        break;
                    case Keys.Right:
                        e.Handled = true;
                        break;
                    case Keys.Tab:
                        e.Handled = true;
                        break;
                    case Keys.Back:
                        e.Handled = true;
                        break;
                    case Keys.Return:
                        {
                            CommandDetectionEventArgs eventArgs = new CommandDetectionEventArgs();
                            eventArgs.Command = Regex.Replace(Lines[Lines.Length - 1], "^" + Regex.Escape(Prompt), "");
                            if (eventArgs.Command.Length > 0)
                            {
                                OnCommandDetection(this, eventArgs);
                            }
                        }
                        AppendLine();
                        Append(Prompt);
                        break;
                }
            }
        }

        /// <summary>
        /// ConsoleTextBox_KeyPress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConsoleTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 押されたキーを取得して、コンソールに出力する
            switch (e.KeyChar)
            {
                case '\r':
                case '\n':
                    e.Handled = true;
                    break;
                case '\b':
                    if (Lines.Length > 0 && Lines[Lines.Length - 1].Length > Prompt.Length)
                    {
                        List<string> lines = new List<string>(Lines);
                        lines[lines.Count - 1] = lines[lines.Count - 1].Substring(0, lines[lines.Count - 1].Length - 1);
                        StringBuilder sb = new StringBuilder();
                        foreach (var s in lines)
                        {
                            sb.AppendLine(s);
                        }
                        ResetText();
                        Append(sb.ToString().TrimEnd('\r', '\n'));
                    }
                    e.Handled = true;
                    break;
                default:
                    Append(e.KeyChar.ToString());
                    break;
            }
        }

        public void Append(string format, params object[] args)
        {
            AppendText(string.Format(format, args));
        }

        public void AppendLine()
        {
            AppendText(Environment.NewLine);
        }

        public void AppendLine(string format, params object[] args)
        {
            AppendText(string.Format(format, args) + Environment.NewLine);
        }
    }
}
