using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;

namespace Common.Control
{
    /// <summary>
    /// コンソールクラス
    /// </summary>
    public partial class TextBoxConsole : TextBox
    {
        /// <summary>
        /// ロガーインスタンス
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 起動メッセージ
        /// </summary>
        public StringBuilder StartUpMessage = new StringBuilder();

        /// <summary>
        /// プロンプト
        /// </summary>
        public string Prompt { get; set; } = "]$ ";

        /// <summary>
        /// 改行文字列
        /// </summary>
        public string NewLine { get; set; } = Environment.NewLine;

        /// <summary>
        /// コマンド保存用
        /// </summary>
        //private StringBuilder m_CurrentCommand = new StringBuilder();

        /// <summary>
        /// コマンド履歴
        /// </summary>
        private CommandHistory m_CommandHistory = new CommandHistory();

        /// <summary>
        /// コマンド検出 Delegate
        /// </summary>
        public delegate void CommandDetectionEventHandler(object sender, CommandDetectionEventArgs args);

        /// <summary>
        /// コマンド検出 Event
        /// </summary>
        public event CommandDetectionEventHandler OnCommandDetection = delegate { };

        /// <summary>
        /// コマンド(Control+C)検出 Delegate
        /// </summary>
        public delegate void ControlCEventHandler(object sender, EventArgs args);

        /// <summary>
        /// コマンド(Control+C)検出 Event
        /// </summary>
        public event ControlCEventHandler OnControlC = delegate { };

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TextBoxConsole()
            : base()
        {
            // コンポーネント初期化
            InitializeComponent();
        }

        /// <summary>
        /// TextBoxConsole_KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxConsole_KeyDown(object sender, KeyEventArgs e)
        {
            // CTRL+Cが押されているか？
            if (e.KeyData == (Keys.Control | Keys.C))
            {
                // // イベント発行
                OnControlC(this, new EventArgs());

                // イベント処理済み設定
                e.Handled = true;
            }
            else
            {
                // キーコードで分岐する
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        // 改行追加
                        AppendNewLine();
                        // コマンド履歴登録判定
                        SetCommand();
                        // プロンプト追加
                        AppendPrompt();
                        // イベント処理済み設定
                        e.Handled = true;
                        break;
                    case Keys.Up:
                        // コマンド履歴処理
                        SetCommandHistory(Keys.Up);
                        // イベント処理済み設定
                        e.Handled = true;
                        break;
                    case Keys.Down:
                        // コマンド履歴処理
                        SetCommandHistory(Keys.Down);
                        // イベント処理済み設定
                        e.Handled = true;
                        break;
                    case Keys.Delete:
                        // 1文字削除
                        DeleteCharacter();
                        // イベント処理済み設定
                        e.Handled = true;
                        break;
                    case Keys.Left:
                        // カーソル位置設定
                        CursorPositionSetting(Keys.Left);
                        // イベント処理済み設定
                        e.Handled = true;
                        break;
                    case Keys.Right:
                        // カーソル位置設定
                        CursorPositionSetting(Keys.Right);
                        // イベント処理済み設定
                        e.Handled = true;
                        break;
                    default:
                        // イベント処理済み設定
                        e.Handled = true;
                        break;
                }
            }
        }

        /// <summary>
        /// TextBoxConsole_KeyPress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxConsole_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 文字で分岐する
            switch (e.KeyChar)
            {
                case '\r':
                case '\n':
                    // イベント処理済み設定
                    e.Handled = true;
                    break;
                case '\u0003': // CTRL+C
                    // イベント処理済み設定
                    e.Handled = true;
                    break;
                case '\b':
                    // 1文字削除
                    DeleteCharacter();
                    // イベント処理済み設定
                    e.Handled = true;
                    break;
                default:
                    // 文字挿入
                    Insert(e.KeyChar.ToString(), SelectionStart);
                    // イベント処理済み設定
                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// カーソル位置設定
        /// </summary>
        /// <param name="position"></param>
        private void SetCursorPosition(int position)
        {
            // カーソルを末尾に配置する
            Select(position, 0);

            // カーソル位置
            ScrollToCaret();
        }

        /// <summary>
        /// コマンド設定
        /// </summary>
        private void SetCommand()
        {
            // コマンド取得
            string command = GetLastLineWithotPromt().Trim();

            // コマンド長判定
            if (command.Length != 0)
            {
                // コマンド履歴に登録
                m_CommandHistory.Add(command);

                // CommandDetectionEventArgsオブジェクト生成
                CommandDetectionEventArgs eventArgs = new CommandDetectionEventArgs() { Command = command };

                // イベント発行
                OnCommandDetection(this, eventArgs);
            }
        }

        /// <summary>
        /// コマンド履歴設定
        /// </summary>
        /// <param name="key"></param>
        private void SetCommandHistory(Keys key)
        {
            // コマンド履歴文字列取得
            string history = m_CommandHistory.Get(key);
            if (history != string.Empty)
            {
                // 最新行更新
                UpdateLine(Prompt + history);
            }
            else
            {
                // 最新行更新
                UpdateLine(Prompt);
            }

            // カーソル位置設定
            SetCursorPosition(Text.Length);
        }

        /// <summary>
        /// カーソル位置設定
        /// </summary>
        /// <param name="key"></param>
        private void CursorPositionSetting(Keys key)
        {
            // 行全体を取得する
            List<string> lines = Lines.ToList();

            // 現在の行番号取得
            int currentLine = GetLineFromCharIndex(SelectionStart);

            // 現在行文字列を取得する
            string line = lines[currentLine];

            // 現在行の先頭文字インデックスを取得
            int startIndexOfCurrentLine = GetFirstCharIndexFromLine(currentLine);

            // 戻りインデックス最大位置(先頭からプロンプト手前まで)を設定
            int returnIndexMaximumPosition = startIndexOfCurrentLine + Prompt.Length;

            // インデックス最終位置
            int iindexFinalPosition = startIndexOfCurrentLine + line.Length;

            // キー種別で分岐
            switch (key)
            {
                case Keys.Left:
                    if (SelectionStart > returnIndexMaximumPosition)
                    {
                        // カーソル位置を設定する
                        SetCursorPosition(SelectionStart - 1);
                    }

                    break;
                case Keys.Right:
                    if (SelectionStart < iindexFinalPosition)
                    {
                        // カーソル位置を設定する
                        SetCursorPosition(SelectionStart + 1);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 開始
        /// </summary>
        public void Start()
        {
            // クリア
            base.Clear();

            // 起動メッセージ表示
            AppendText(StartUpMessage.ToString());

            // プロンプト追加
            AppendPrompt();
        }

        /// <summary>
        /// クリア
        /// </summary>
        public new void Clear()
        {
            // クリア
            base.Clear();

            // プロンプト追加
            AppendPrompt();
        }

        /// <summary>
        /// 最終行更新
        /// </summary>
        /// <param name="line"></param>
        public void UpdateLine(string line)
        {
            UpdateLine(Lines.Count() - 1, line);
        }

        /// <summary>
        /// 行更新
        /// </summary>
        /// <param name="index"></param>
        /// <param name="newLine"></param>
        public void UpdateLine(int index, string newLine)
        {
            // 行抽出
            List<string> lines = Lines.ToList();

            // 削除行判定
            if (index > lines.Count)
            {
                // TODO:例外
                throw new IndexOutOfRangeException();
            }

            // 更新
            lines[index] = newLine;

            // 復旧
            Text = string.Join(NewLine, lines);
        }

        /// <summary>
        /// 行追加
        /// </summary>
        /// <param name="line"></param>
        public void Append(string line)
        {
            // テキスト追加
            AppendText(line);
        }

        /// <summary>
        /// 行追加
        /// </summary>
        public void AppendLine()
        {
            // 行追加
            AppendText(NewLine);
        }

        /// <summary>
        /// 行追加
        /// </summary>
        /// <param name="line"></param>
        public void AppendLine(string line)
        {
            // 行追加
            AppendLine(new StringBuilder(line));
        }

        /// <summary>
        /// 行追加
        /// </summary>
        /// <param name="lines"></param>
        public void AppendLine(StringBuilder lines)
        {
            // 行追加
            AppendText(lines.ToString());

            // 改行追加
            AppendNewLine();
        }

        /// <summary>
        /// 行追加
        /// </summary>
        /// <param name="line"></param>
        public void AppendLine(string format, params object[] args)
        {
            // 行追加
            AppendLine(string.Format(format, args));
        }

        /// <summary>
        /// プロンプト追加
        /// </summary>
        public void AppendPrompt()
        {
            // テキスト追加
            AppendText(Prompt);
        }

        /// <summary>
        /// 改行追加
        /// </summary>
        public void AppendNewLine()
        {
            AppendText(NewLine);
        }

        /// <summary>
        /// 最終行取得(コマンドプロンプトなし)
        /// </summary>
        /// <returns></returns>
        public string GetLastLineWithotPromt()
        {
            List<string> lines = new List<string>(Lines);
            string lastLine = lines[lines.Count - 1].Substring(Prompt.Length);
            return lastLine;
        }

        /// <summary>
        /// 1文字削除
        /// </summary>
        private void DeleteCharacter()
        {
            // カーソル位置の1つ前の文字を削除する
            if (SelectionStart > 0)
            {
                // 行全体を取得する
                List<string> lines = Lines.ToList();

                // カーソルの位置から行番号を取得する
                int cursorLine = GetLineFromCharIndex(SelectionStart);

                // 選択行を取得する
                string selectLine = lines[cursorLine];

                // プロンプトサイズ判定
                if(selectLine.Length > Prompt.Length)
                {
                    // 大きい場合は文字削除
                    lines[cursorLine] = selectLine.Substring(0, selectLine.Length - 1);

                    // 復旧
                    Text = string.Join(NewLine, lines);
                }

                // カーソル位置を設定する
                SetCursorPosition(Text.Length);
            }
        }

        /// <summary>
        /// 文字挿入
        /// </summary>
        /// <param name="text"></param>
        /// <param name="index"></param>
        private void Insert(string text, int index)
        {
            // テキストボックスのカーソル位置にテキストを挿入
            Text = Text.Insert(index, text);

            // カーソル位置を設定する
            SetCursorPosition(index + text.Length);
        }

        /// <summary>
        /// 指定行削除
        /// </summary>
        /// <param name="index"></param>
        private void RemoveAt(int index)
        {
            // 行抽出
            List<string> lines = Lines.ToList();

            // 削除行判定
            if (index > lines.Count)
            {
                // TODO:例外
                throw new IndexOutOfRangeException();
            }

            // 行削除
            lines.RemoveAt(index);

            // 復旧
            Text = string.Join(NewLine, lines);
        }
    }
}
