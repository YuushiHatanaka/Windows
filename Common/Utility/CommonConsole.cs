using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;

namespace Common.Utility
{
    /// <summary>
    /// 共通コンソール
    /// </summary>
    public class CommonConsole : TextBox
    {
        /// <summary>
        /// プロンプト
        /// </summary>
        private string m_Prompt = string.Empty;

        /// <summary>
        /// ヒストリ
        /// </summary>
        private List<string> m_History = new List<string>();

        private Point m_Point = new Point(0, 0);

        /// <summary>
        /// プロンプト
        /// </summary>
        public string Prompt
        {
            set { this.m_Prompt = value; }
            get { return this.m_Prompt; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="prompt"></param>
        public CommonConsole(string prompt)
            : base()
        {
            // 初期化
            this.Initialization(prompt);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="prompt"></param>
        private void Initialization(string prompt)
        {
            this.Multiline = true;
            this.ScrollBars = ScrollBars.Both;
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;
            this.ReadOnly = true;

            this.m_Prompt = prompt;

            this.KeyDown += this.OnKeyDown;
            this.KeyPress += this.OnKeyPress;

            this.AppendText(this.m_Prompt);

            this.m_Point.X = this.SelectionStart;
        }

/*
        /// <summary>
        /// [Override] IsInputKey
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool IsInputKey(Keys keyData)
        {
            // Altキーが押されているか確認する
            if ((keyData & Keys.Alt) != Keys.Alt)
            {
                Keys kcode = keyData & Keys.KeyCode;
                switch (kcode)
                {
                    case Keys.Up:
                        {
                            return false;
                        }
                    case Keys.Down:
                        {
                            return false;
                        }
                    case Keys.Left:
                        {
                            // 行数を判定
                            if (this.Lines.Length == 0)
                            {
                                // 1行も無ければ終了
                                return false;
                            }

                            // 文字数を判定
                            Debug.WriteLine("Left:" + this.SelectionStart);
                            if (this.m_Prompt.Length >= this.SelectionStart)
                            {
                                return false;
                            }

                            // カーソル位置を移動
                            this.SelectionStart -= 1;

                            return false;
                        }
                    case Keys.Right:
                        {
                            // 行数を判定
                            if (this.Lines.Length == 0)
                            {
                                // 1行も無ければ終了
                                return false;
                            }

                            // 最終行を取得
                            string _LastLine = this.Lines[this.Lines.Length - 1];

                            // 文字数を判定
                            Debug.WriteLine("Right:" + this.SelectionStart);
                            if (_LastLine.Length <= this.SelectionStart)
                            {
                                return false;
                            }

                            // カーソル位置を移動
                            this.SelectionStart += 1;

                            return false;
                        }
                }

            }
            return base.IsInputKey(keyData);
        }
        */
        /// <summary>
        /// [Event] KeyPress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == "\b")
            {
                Debug.WriteLine(@"OnKeyPress:\b");
                return;
            }
            else if (e.KeyChar.ToString() == "\r")
            {
                Debug.WriteLine(@"OnKeyPress:\r");
                this.Add(Environment.NewLine);
                this.Add(this.m_Prompt);
                return;
            }
            else if (e.KeyChar.ToString() == "\n")
            {
                Debug.WriteLine(@"OnKeyPress:\n");
                return;
            }
            Debug.WriteLine("OnKeyPress:" + e.KeyChar.ToString());

            // 文字追加
            //this.Add(e.KeyChar.ToString());
        }

        /// <summary>
        /// コンソール位置更新
        /// </summary>
        /// <param name="keys"></param>
        private void UpdateConsolePosition(Keys keys)
        {
            Keys kcode = keys & Keys.KeyCode;

            // コンソール位置更新
            switch (kcode)
            {
                case Keys.Return:
                    this.m_Point.X = this.m_Prompt.Length;
                    this.m_Point.Y += 1;
                    break;
                case Keys.Back:
                    this.m_Point.X -= 1;
                    if (this.m_Point.X < this.m_Prompt.Length)
                    {
                        this.m_Point.X = this.m_Prompt.Length;
                    }
                    break;
                case Keys.Up:
                    /*
                    this.m_Point.Y -= 1;
                    if (this.m_Point.Y < 0)
                    {
                        this.m_Point.Y = 0;
                    }
                    */
                    break;
                case Keys.Down:
                    /*
                    this.m_Point.Y += 1;
                    if (this.m_Point.Y > this.Lines.Length)
                    {
                        this.m_Point.Y = this.Lines.Length;
                    }
                    */
                    break;
                case Keys.Left:
                    this.m_Point.X -= 1;
                    if (this.m_Point.X < this.m_Prompt.Length)
                    {
                        this.m_Point.X = this.m_Prompt.Length;
                    }
                    break;
                case Keys.Right:
                    this.m_Point.X += 1;
                    if (this.m_Point.X > this.Lines[this.Lines.Length - 1].Length)
                    {
                        this.m_Point.X = this.Lines[this.Lines.Length - 1].Length;
                    }
                    break;
                default:
                    this.m_Point.X += 1;
                    break;
            }
            Debug.WriteLine(this.m_Point.ToString());
        }

        /// <summary>
        /// 文字列追加
        /// </summary>
        /// <param name="value"></param>
        private void Add(string value)
        {
            this.AppendText(value);
        }

        /// <summary>
        /// 文字列追加
        /// </summary>
        /// <param name="keys"></param>
        private void Add(Keys keys)
        {
        }

        /// <summary>
        /// [Event] KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // コンソール位置更新
            this.UpdateConsolePosition(e.KeyData);

            Keys kcode = e.KeyData & Keys.KeyCode;
            switch (kcode)
            {
                case Keys.Return:
                case Keys.Back:
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    break;
                default:
                    break;
            }

            /*
            switch (kcode)
            {
                case Keys.Return:
                    {
                        // プロンプト行を表示
                        this.AppendText(Environment.NewLine + this.m_Prompt);
                    }
                    break;
                case Keys.Back:
                    {
                        // 行数を判定
                        if (this.Lines.Length == 0)
                        {
                            // 1行も無ければ終了
                            break;
                        }

                        // 最終行を取得
                        string _LastLine = this.Lines[this.Lines.Length - 1];

                        // 文字数を判定
                        if (this.m_Prompt.Length >= _LastLine.Length)
                        {
                            // プロンプト行より同じ（または短い）ので、何もしない
                            break;
                        }

                        // 1文字削除させる
                        int _OldSelectionStart = this.SelectionStart;

                        // 更新
                        string _FrontString = this.Text.Substring(0, _OldSelectionStart - 1);
                        string _BackString = this.Text.Substring(_OldSelectionStart);
                        this.Text = _FrontString + _BackString;
                        //_UpdateLines[this.Lines.Length - 1] = _FrontString + _BackString;
                        //this.Lines = _UpdateLines;

                        // カーソル位置を決定
                        this.SelectionStart = _OldSelectionStart - 1;
                    }
                    break;
                default:
                    {
                    }
                    break;
            }*/
        }
    }
}
