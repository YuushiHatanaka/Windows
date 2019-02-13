using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Common.Logging
{
    /// <summary>
    /// ログListener
    /// </summary>
    public class LoggingListener : TraceListener
    {
        private FileStream m_FileStream = null;
        private StreamWriter m_StreamWriter = null;
        private Encoding m_Encoding = Encoding.Default;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="p_LogFilePath"></param>
        public LoggingListener(string logFilePath)
        {
            try
            {
                if (this.m_FileStream == null)
                {
                    this.m_FileStream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    this.m_StreamWriter = new StreamWriter(this.m_FileStream, this.m_Encoding);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                this.m_FileStream = null;
                this.m_StreamWriter = null;
            }

        }

        /// <summary>
        /// 出力
        /// </summary>
        /// <param name="message"></param>
        public override void Write(string message)
        {
            if (this.m_StreamWriter == null)
            {
                return;
            }
            try
            {
                /*
                lock (_StreamWriter)
                {
                    _StreamWriter.Write(message);
                    _StreamWriter.Flush();
                }
                */
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                System.Diagnostics.Debug.Print(message);
            }
        }

        /// <summary>
        /// 行出力
        /// </summary>
        /// <param name="message"></param>
        public override void WriteLine(string message)
        {
            if (this.m_StreamWriter == null)
            {
                System.Diagnostics.Debug.Print(message);
                return;
            }
            try
            {
                lock (this.m_StreamWriter)
                {
                    this.m_StreamWriter.WriteLine(message);
                    this.m_StreamWriter.Flush();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                System.Diagnostics.Debug.Print(message);
            }
        }

        /// <summary>
        /// バッファ書き込み
        /// </summary>
        public override void Flush()
        {
            if (this.m_StreamWriter == null)
            {
                return;
            }
            try
            {
                lock (this.m_StreamWriter)
                {
                    this.m_StreamWriter.Flush();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
            }
        }

        /// <summary>
        /// リソース破棄
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (this.m_StreamWriter == null)
            {
                return;
            }
            try
            {
                base.Dispose(disposing);
                this.m_StreamWriter.Dispose();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
            }
        }
    }

    /// <summary>
    /// ロガークラス
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// トレース識別子
        /// </summary>
        protected static TraceSource m_TraceSource = null;

        /// <summary>
        /// トレース識別子インスタンス生成
        /// </summary>
        /// <param name="m_InstanceName"></param>
        public static void SetTraceSourceInstance(string m_InstanceName)
        {
            m_TraceSource = new TraceSource(m_InstanceName);
        }

        /// <summary>
        /// 日時取得
        /// </summary>
        /// <returns>デバッグ用日時文字列</returns>
        private static string GetDateTime()
        {
            // 日時文字列を返却する
            return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
        }

        /// <summary>
        /// フォーマット文字列作成
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        /// <returns>出力文字列</returns>
        private static string GetString(string format, params object[] args)
        {
            // 引数(args)がnullの場合、例外となる(変換不可)のでガード追加
            if (args == null)
            {
                format = Regex.Replace(format, @"\{[0-9]*\}", "{null}");
                return format;
            }
            for (int i = 0; i < args.Length; ++i)
            {
                if (args[i] == null)
                {
                    format = format.Replace("{" + i.ToString() + "}", "null");
                }
                else
                {
                    format = format.Replace("{" + i.ToString() + "}", args[i].ToString());
                }
            }
            return format;
        }

        /// <summary>
        /// デバッグ出力 - 致命的なエラーまたはアプリケーションのクラッシュ
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Critical(string format, params object[] args)
        {
            Critical(0, format, args);
        }

        /// <summary>
        /// デバッグ出力 - 致命的なエラーまたはアプリケーションのクラッシュ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Critical(int id, string format, params object[] args)
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.TraceEvent(TraceEventType.Critical, 0, GetStackFrame() + " " + GetDateTime() + " " + string.Format(" [{0,-8}][{1,5}] ", "Critical", id) + GetString(format, args));
            m_TraceSource.Flush();
        }

        /// <summary>
        /// デバッグ出力 - 回復可能なエラー
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Error(string format, params object[] args)
        {
            Error(0, format, args);
        }

        /// <summary>
        /// デバッグ出力 - 回復可能なエラー
        /// </summary>
        /// <param name="id"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Error(int id, string format, params object[] args)
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.TraceEvent(TraceEventType.Error, 0, GetStackFrame() + " " + GetDateTime() + " " + string.Format(" [{0,-8}][{1,5}] ", "Error", id) + GetString(format, args));
            m_TraceSource.Flush();
        }

        /// <summary>
        /// デバッグ出力 - 情報メッセージ
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Information(string format, params object[] args)
        {
            Information(0, format, args);
        }

        /// <summary>
        /// デバッグ出力 - 情報メッセージ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Information(int id, string format, params object[] args)
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.TraceEvent(TraceEventType.Information, 0, GetStackFrame() + " " + GetDateTime() + " " + string.Format(" [{0,-8}][{1,5}] ", "Info", id) + GetString(format, args));
        }

        /// <summary>
        /// デバッグ出力 - 重大でない問題
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Warning(string format, params object[] args)
        {
            Warning(0, format, args);
        }

        /// <summary>
        /// デバッグ出力 - 重大でない問題
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Warning(int id, string format, params object[] args)
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.TraceEvent(TraceEventType.Warning, 0, GetStackFrame() + " " + GetDateTime() + " " + string.Format(" [{0,-8}][{1,5}] ", "Warning", id) + GetString(format, args));
        }

        /// <summary>
        /// デバッグ出力 - トレースのデバッグ
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Verbose(string format, params object[] args)
        {
            Verbose(0, format, args);
        }

        /// <summary>
        /// デバッグ出力 - トレースのデバッグ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Verbose(int id, string format, params object[] args)
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.TraceEvent(TraceEventType.Verbose, 0, GetStackFrame() + " " + GetDateTime() + " " + string.Format(" [{0,-8}][{1,5}] ", "Verbose", id) + GetString(format, args));
        }

        /// <summary>
        /// デバッグ出力 - 論理的な操作の開始
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Start(string format, params object[] args)
        {
            Start(0, format, args);
        }

        /// <summary>
        /// デバッグ出力 - 論理的な操作の開始
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Start(int id, string format, params object[] args)
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.TraceEvent(TraceEventType.Start, 0, GetStackFrame() + " " + GetDateTime() + " " + string.Format(" [{0,-8}][{1,5}] =>>>>", "Start", id) + GetString(format, args));
            m_TraceSource.Flush();
        }

        /// <summary>
        /// デバッグ出力 - 論理的な操作の中断
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Suspend(string format, params object[] args)
        {
            Suspend(0, format, args);
        }

        /// <summary>
        /// デバッグ出力 - 論理的な操作の中断
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Suspend(int id, string format, params object[] args)
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.TraceEvent(TraceEventType.Suspend, 0, GetStackFrame() + " " + GetDateTime() + " " + string.Format(" [{0,-8}][{1,5}] ", "Suspend", id) + GetString(format, args));
        }
        
        /// <summary>
        /// デバッグ出力 - 論理的な操作の再開
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Resume(string format, params object[] args)
        {
            Resume(0, format, args);
        }

        /// <summary>
        /// デバッグ出力 - 論理的な操作の再開
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Resume(int id, string format, params object[] args)
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.TraceEvent(TraceEventType.Resume, 0, GetStackFrame() + " " + GetDateTime() + " " + string.Format(" [{0,-8}][{1,5}] ", "Resume", id) + GetString(format, args));
        }

        /// <summary>
        /// デバッグ出力 - 論理的な操作の停止
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Stop(string format, params object[] args)
        {
            Stop(0, format, args);
        }

        /// <summary>
        /// デバッグ出力 - 論理的な操作の停止
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Stop(int id, string format, params object[] args)
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.TraceEvent(TraceEventType.Stop, 0, GetStackFrame() + " " + GetDateTime() + " " + string.Format(" [{0,-8}][{1,5}] <<<<=", "Stop", id) + GetString(format, args));
            m_TraceSource.Flush();
        }

        /// <summary>
        /// デバッグ出力 - 相関 ID の変更
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Transfer(string format, params object[] args)
        {
            Transfer(0, format, args);
        }

        /// <summary>
        /// デバッグ出力 - 相関 ID の変更
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Transfer(int id, string format, params object[] args)
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.TraceEvent(TraceEventType.Transfer, 0, GetStackFrame() + " " + GetDateTime() + " " + string.Format(" [{0,-8}][{1,5}] ", "Transfer", id) + GetString(format, args));
        }

        /// <summary>
        /// デバッグ出力 - データダンプ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="addr"></param>
        public static void Dump(string message, byte[] addr)
        {
            Dump(0, message, addr);
        }

        /// <summary>
        /// デバッグ出力 - データダンプ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <param name="addr"></param>
        public static void Dump(int id, string message, byte[] addr)
        { 
            if (m_TraceSource == null)
            {
                return;
            }

            StringBuilder _message = new StringBuilder(); // ログメッセージ
            StringBuilder _dump = new StringBuilder();
            StringBuilder _text = new StringBuilder(16);
            int i = 0;

            // TODO:検討中
            while (i < addr.Length)
            {
                // オフセット出力
                if ((i % 16) == 0)
                {
                    string _offset = string.Format("0x{0, 8:X8}: ", i).ToLower();
                    _message.Append(_offset);
                }

                // 16進表示データ作成
                string _data = string.Format("{0} ", BitConverter.ToString(addr, i, 1)).ToLower();
                _dump.Append(_data);

                if (addr[i] > 0x80)
                {
                    _text.Append(".");
                }
                else
                {
                    UnicodeCategory[] nonRenderingCategories = new UnicodeCategory[] {
                                                                UnicodeCategory.Control,
                                                                UnicodeCategory.OtherNotAssigned,
                                                                UnicodeCategory.Surrogate };
                    bool isPrintable = char.IsWhiteSpace((char)addr[i]) || !nonRenderingCategories.Contains(char.GetUnicodeCategory((char)addr[i]));
                    if (isPrintable)
                    {
                        _text.AppendFormat("{0}", Encoding.ASCII.GetString(new byte[] { addr[i] }));
                    }
                    else
                    {
                        _text.Append(".");
                    }
                }
                i++;

                if ((i % 16) == 0)
                {
                    _message.AppendFormat(": {0,-48}", _dump.ToString());
                    _message.AppendFormat(": {0,-16}", _text.ToString());
                    _message.Append(Environment.NewLine);
                    _dump.Clear();
                    _text.Clear();
                }
            }
            if ((i % 16) != 0)
            {
                _message.AppendFormat(": {0,-48}", _dump.ToString());
                _message.AppendFormat(": {0,-16}", _text.ToString());
                _message.Append(Environment.NewLine);
                _dump.Clear();
                _text.Clear();
            }
            m_TraceSource.TraceEvent(TraceEventType.Information, id, GetStackFrame() + " " + GetDateTime() + " " + string.Format(" [{0,-8}][{1,5}] ", "Dump", id) + message + Environment.NewLine + _message.ToString().TrimEnd('\r', '\n'));
            m_TraceSource.Flush();
        }

        /// <summary>
        /// バッファフラッシュ
        /// </summary>
        public static void Flush()
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.Flush();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        public static string GetStackFrame()
        {
            StackFrame CallStack = new StackFrame(3, true);

            string SourceFile = CallStack.GetFileName();
            int SourceLine = CallStack.GetFileLineNumber();

            string _result = SourceFile + " : " + SourceLine.ToString();

            return _result;
        }
    }
}
