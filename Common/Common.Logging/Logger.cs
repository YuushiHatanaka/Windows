using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Logging
{
    /// <summary>
    /// 
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
        public static void SetTraceSourceInstance(String m_InstanceName)
        {
            m_TraceSource = new TraceSource(m_InstanceName);
        }

        /// <summary>
        /// フォーマット文字列作成
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        /// <returns>出力文字列</returns>
        private static String GetString(String format, params object[] args)
        {
            // 引数(args)がnullの場合、例外となる(変換不可)のでガード追加
            if (args == null)
            {
#if DEBUG
                m_TraceSource.TraceEvent(TraceEventType.Critical, 0, "CsDebug::GetString ... params object[] args = null[" + format + "]");
                m_TraceSource.Flush();
#endif
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
        public static void Critical(String format, params object[] args)
        {
            Critical(0, format, args);
        }

        /// <summary>
        /// デバッグ出力 - 致命的なエラーまたはアプリケーションのクラッシュ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Critical(int id, String format, params object[] args)
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.TraceEvent(TraceEventType.Critical, 0, String.Format(" [{0,-8}][{1,5}] ", "Critical", id) + GetString(format, args));
            m_TraceSource.Flush();
        }

        /// <summary>
        /// デバッグ出力 - 回復可能なエラー
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Error(String format, params object[] args)
        {
            Error(0, format, args);
        }

        /// <summary>
        /// デバッグ出力 - 回復可能なエラー
        /// </summary>
        /// <param name="id"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Error(int id, String format, params object[] args)
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.TraceEvent(TraceEventType.Error, 0, String.Format(" [{0,-8}][{1,5}] ", "Error", id) + GetString(format, args));
            m_TraceSource.Flush();
        }

        /// <summary>
        /// デバッグ出力 - 情報メッセージ
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Information(String format, params object[] args)
        {
            Information(0, format, args);
        }

        /// <summary>
        /// デバッグ出力 - 情報メッセージ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Information(int id, String format, params object[] args)
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.TraceEvent(TraceEventType.Information, 0, String.Format(" [{0,-8}][{1,5}] ", "Info", id) + GetString(format, args));
        }

        /// <summary>
        /// デバッグ出力 - 重大でない問題
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Warning(String format, params object[] args)
        {
            Warning(0, format, args);
        }

        /// <summary>
        /// デバッグ出力 - 重大でない問題
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Warning(int id, String format, params object[] args)
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.TraceEvent(TraceEventType.Warning, 0, String.Format(" [{0,-8}][{1,5}] ", "Warning", id) + GetString(format, args));
            m_TraceSource.Flush();
        }

        /// <summary>
        /// デバッグ出力 - トレースのデバッグ
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Verbose(String format, params object[] args)
        {
            Verbose(0, format, args);
        }

        /// <summary>
        /// デバッグ出力 - トレースのデバッグ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Verbose(int id, String format, params object[] args)
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.TraceEvent(TraceEventType.Verbose, 0, String.Format(" [{0,-8}][{1,5}] ", "Verbose", id) + GetString(format, args));
        }

        /// <summary>
        /// デバッグ出力 - 論理的な操作の開始
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Start(String format, params object[] args)
        {
            Start(0, format, args);
        }

        /// <summary>
        /// デバッグ出力 - 論理的な操作の開始
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Start(int id, String format, params object[] args)
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.TraceEvent(TraceEventType.Start, 0, String.Format(" [{0,-8}][{1,5}] ", "Start", id) + GetString(format, args));
        }

        /// <summary>
        /// デバッグ出力 - 論理的な操作の中断
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Suspend(String format, params object[] args)
        {
            Suspend(0, format, args);
        }

        /// <summary>
        /// デバッグ出力 - 論理的な操作の中断
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Suspend(int id, String format, params object[] args)
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.TraceEvent(TraceEventType.Suspend, 0, String.Format(" [{0,-8}][{1,5}] ", "Suspend", id) + GetString(format, args));
            m_TraceSource.Flush();
        }

        /// <summary>
        /// デバッグ出力 - 論理的な操作の再開
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Resume(String format, params object[] args)
        {
            Resume(0, format, args);
        }

        /// <summary>
        /// デバッグ出力 - 論理的な操作の再開
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Resume(int id, String format, params object[] args)
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.TraceEvent(TraceEventType.Resume, 0, String.Format(" [{0,-8}][{1,5}] ", "Resume", id) + GetString(format, args));
            m_TraceSource.Flush();
        }

        /// <summary>
        /// デバッグ出力 - 論理的な操作の停止
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Stop(String format, params object[] args)
        {
            Stop(0, format, args);
        }

        /// <summary>
        /// デバッグ出力 - 論理的な操作の停止
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Stop(int id, String format, params object[] args)
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.TraceEvent(TraceEventType.Stop, 0, String.Format(" [{0,-8}][{1,5}] ", "Stop", id) + GetString(format, args));
            m_TraceSource.Flush();
        }

        /// <summary>
        /// デバッグ出力 - 相関 ID の変更
        /// </summary>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Transfer(String format, params object[] args)
        {
            Transfer(0, format, args);
        }

        /// <summary>
        /// デバッグ出力 - 相関 ID の変更
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="format">フォーマット文字列</param>
        /// <param name="args">出力オブジェクト</param>
        public static void Transfer(int id, String format, params object[] args)
        {
            if (m_TraceSource == null)
            {
                return;
            }
            m_TraceSource.TraceEvent(TraceEventType.Transfer, 0, String.Format(" [{0,-8}][{1,5}] ", "Transfer", id) + GetString(format, args));
        }

        /// <summary>
        /// デバッグ用メッセージボックス
        /// </summary>
        /// <param name="text">メッセージ</param>
        /// <param name="caption">タイトル</param>
        /// <param name="button">ボタン種別</param>
        /// <param name="icon">アイコン種別</param>
        /// <returns>MessageBox.Showの結果</returns>
        public static DialogResult MessageBox_Show(String text, String caption, MessageBoxButtons button, MessageBoxIcon icon)
        {
#if DEBUG
            return MessageBox.Show(text, caption, button, icon);
#else
            return DialogResult.None;
#endif
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
    }
}
