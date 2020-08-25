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
using System.Runtime.InteropServices;

namespace Common.Utility
{
    /// <summary>
    /// コンソールヒストリクラス
    /// </summary>
    public class ConsoleHistory : List<string>
    {

    }

    /// <summary>
    /// 共通コンソール
    /// </summary>
    public class CommonConsole : TextBox
    {
        const int EM_LINEINDEX = 0xBB;
        const int EM_LINEFORMCHAR = 0xC9;

        [DllImport("User32.Dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        /// <summary>
        /// プロンプト
        /// </summary>
        private string m_Prompt = string.Empty;

        /// <summary>
        /// プロンプト
        /// </summary>
        public string Prompt
        {
            set
            {
                // プロンプト更新
                this.Write(Environment.NewLine);
                this.Write(value);
                this.m_Prompt = value;
            }
            get { return this.m_Prompt; }
        }

        /// <summary>
        /// カーソル位置(保存用)
        /// </summary>
        private int m_CursolPosition = 0;

        #region delegate
        /// <summary>
        /// [delegate] 文字列入力イベントdelegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="strings"></param>
        /// <returns></returns>
        public delegate void EventHandlerInputString(object sender, string strings);
        #endregion

        #region イベントハンドラ
        /// <summary>
        /// 文字列入力イベント通知
        /// </summary>
        public EventHandlerInputString OnInputString = null;
        #endregion

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
        /// 文字列書込
        /// </summary>
        /// <param name="line"></param>
        private void Write(string line)
        {
            this.AppendText(line);
            this.m_CursolPosition = this.SelectionStart;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="prompt"></param>
        private void Initialization(string prompt)
        {
            // 各設定
            this.m_Prompt = prompt;

            // プロパティ設定
            this.Multiline = true;
            this.ScrollBars = ScrollBars.Both;
            this.WordWrap = false;
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;
            this.ImeMode = ImeMode.NoControl;

            // イベントハンドラ設定
            this.KeyPress += this.OnKeyPress;
            this.MouseClick += this.OnMouseClick;

            // プロンプト追加
            this.Write(this.m_Prompt);
        }

        /// <summary>
        /// 入力キー判定
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool IsInputKey(Keys keyData)
        {
            // カーソル位置更新
            this.UpdateCursolPosition();

            Keys kcode = keyData & Keys.KeyCode;
            switch (kcode)
            {
                case Keys.Up:
                    // TODO:ヒストリ表示
                    return false;
                case Keys.Down:
                    // TODO:ヒストリ表示
                    return false;
                case Keys.Left:
                    {
                        // 現在位置取得
                        Point _Position = this.GetCursolPosition();

                        // プロンプト位置以前になるか？
                        if (this.m_Prompt.Length >= _Position.X - 1)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                case Keys.Right:
                    {
                        return true;
                    }
            }
            return base.IsInputKey(keyData);
        }

        /// <summary>
        /// 現在カーソル位置取得
        /// </summary>
        /// <returns></returns>
        private Point GetCursolPosition()
        {
            Point _Point = new Point
            {
                Y = SendMessage(this.Handle, EM_LINEFORMCHAR, -1, 0) + 1
            };
            int _lineIndex = SendMessage(this.Handle, EM_LINEINDEX, -1, 0);
            _Point.X = this.SelectionStart - _lineIndex + 1;
            Debug.WriteLine(_Point.ToString());

            return _Point;
        }

        /// <summary>
        /// カーソル位置更新
        /// </summary>
        private void UpdateCursolPosition()
        {
            this.m_CursolPosition = this.SelectionStart;
            Debug.WriteLine("UpdateCursolPosition:" + this.m_CursolPosition);
        }

        /// <summary>
        /// カーソル位置復旧
        /// </summary>
        private void RestoreCursolPosition()
        {
            this.SelectionStart = this.m_CursolPosition;
            Debug.WriteLine("RestoreCursolPosition:" + this.m_CursolPosition);
        }

        /// <summary>
        /// 行取得
        /// </summary>
        /// <returns></returns>
        private string GetLine()
        {
            // 行を返却
            return this.GetLine(this.GetCursolPosition());
        }

        /// <summary>
        /// 行取得
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private string GetLine(Point position)
        {
            // 現在行を返却
            return this.Lines[position.Y - 1];
        }

        /// <summary>
        /// 文字列取得
        /// </summary>
        /// <returns></returns>
        /// <remarks>プロンプト文字列を除く文字を取得</remarks>
        private string GetString()
        {
            // 文字列取得
            return this.GetString(this.GetCursolPosition());
        }

        /// <summary>
        /// 文字列取得
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        /// <remarks>プロンプト文字列を除く文字を取得</remarks>
        private string GetString(Point position)
        {
            // 行を取得
            string _CurentLine = this.GetLine(position);

            // プロンプトを除く文字を返却
            return _CurentLine.Substring(this.m_Prompt.Length);
        }

        /// <summary>
        /// 入力文字列取得
        /// </summary>
        /// <returns></returns>
        private string GetInputString()
        {
            // 現在位置取得
            Point _Position = this.GetCursolPosition();

            // 入力された1行前の文字を取得
            if (_Position.Y > 1)
            {
                _Position.Y -= 1;
                return this.GetString(_Position);
            }

            // 前行がなければ空白
            return string.Empty;
        }

        /// <summary>
        /// 文字入力イベント通知
        /// </summary>
        /// <param name="input_string"></param>
        private void OnInputStringNotyfy(string input_string)
        {
            // イベント通知
            if (this.OnInputString != null)
            {
                this.OnInputString(this, input_string);
            }
        }

        /// <summary>
        /// [Event] MouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            // カーソル位置復旧
            this.RestoreCursolPosition();
        }

        /// <summary>
        /// [Event] KeyPress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            // カーソル位置更新
            this.UpdateCursolPosition();

            if (e.KeyChar.ToString() == "\b")
            {
                // 現在位置取得
                Point _Position = this.GetCursolPosition();

                // プロンプト位置以前になるか？
                if (this.m_Prompt.Length >= _Position.X - 1)
                {
                    // イベントキャンセル
                    e.Handled = true;
                }
                return;
            }
            else if (e.KeyChar.ToString() == "\r" || e.KeyChar.ToString() == "\n")
            {
                // 改行、プロンプト表示
                this.Write(Environment.NewLine);
                this.Write(this.m_Prompt);

                // 入力文字列取得
                string _InputString = this.GetInputString();

                // TODO:入力があったらヒストリ―に追加
                if (_InputString != string.Empty)
                {

                }

                // 入力イベント通知
                this.OnInputStringNotyfy(_InputString);

                // イベントキャンセル
                e.Handled = true;
                return;
            }
        }
    }
}
