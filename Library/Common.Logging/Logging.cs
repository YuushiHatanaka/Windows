using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Common.Logging
{
    /// <summary>
    /// ロギングクラス
    /// </summary>
    public class Logging
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Fatal
        /// <summary>
        /// ロギング(Fatal)
        /// </summary>
        /// <param name="message"></param>
        public static void Fatal(string message)
        {
            // ロギング
            Logger.Fatal(message);
#if DEBUG
            Console.WriteLine("*** {0} {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), message);
#endif
        }

        /// <summary>
        /// ロギング(Fatal)
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public static void FatalFormat(string format, params object[] arg)
        {
            // 表示メッセージ作成
            string message = string.Format(format, arg);

            // ロギング
            Logger.Fatal(message);
#if DEBUG
            Console.WriteLine("*** {0} {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), message);
#endif
        }

        /// <summary>
        /// 例外ロギング(Fatal)
        /// </summary>
        /// <param name="ex"></param>
        public static void FatalException(Exception ex)
        {
            // 例外メッセージ取得
            string exceptionMessage = MakeExceptionMessage(ex);

            // ロギング
            Logger.Fatal(exceptionMessage);
#if DEBUG
            Console.WriteLine("*** {0} {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), ex.Message);
#endif
        }

        /// <summary>
        /// 例外ロギング(Fatal)
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public static void FatalException(Exception ex, string format, params object[] arg)
        {
            // 表示メッセージ作成
            string message = string.Format(format, arg);

            // 例外メッセージ取得
            string exceptionMessage = MakeExceptionMessage(ex);

            // ロギング
            Logger.Fatal(message);
            Logger.Fatal(exceptionMessage);
#if DEBUG
            Console.WriteLine("*** {0} {1}:[{2}]", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), message, ex.Message);
#endif
        }
        #endregion

        #region Error
        /// <summary>
        /// ロギング(Error)
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            // ロギング
            Logger.Error(message);
#if DEBUG
            Console.WriteLine("**. {0} {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), message);
#endif
        }

        /// <summary>
        /// ロギング(Error)
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public static void ErrorFormat(string format, params object[] arg)
        {
            // 表示メッセージ作成
            string message = string.Format(format, arg);

            // ロギング
            Logger.Error(message);
#if DEBUG
            Console.WriteLine("**. {0} {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), message);
#endif
        }

        /// <summary>
        /// 例外ロギング(Error)
        /// </summary>
        /// <param name="ex"></param>
        public static void ErrorException(Exception ex)
        {
            // 例外メッセージ取得
            string exceptionMessage = MakeExceptionMessage(ex);

            // ロギング
            Logger.Error(exceptionMessage);
#if DEBUG
            Console.WriteLine("**. {0} {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), ex.Message);
#endif
        }

        /// <summary>
        /// 例外ロギング(Error)
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public static void ErrorException(Exception ex, string format, params object[] arg)
        {
            // 表示メッセージ作成
            string message = string.Format(format, arg);

            // 例外メッセージ取得
            string exceptionMessage = MakeExceptionMessage(ex);

            // ロギング
            Logger.Error(message);
            Logger.Error(exceptionMessage);
#if DEBUG
            Console.WriteLine("**. {0} {1}:[{2}]", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), message, ex.Message);
#endif
        }
        #endregion

        #region Warn
        /// <summary>
        /// ロギング(Warn)
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message)
        {
            // ロギング
            Logger.Warn(message);
#if DEBUG
            Console.WriteLine("*.. {0} {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), message);
#endif
        }

        /// <summary>
        /// ロギング(Warn)
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public static void WarnFormat(string format, params object[] arg)
        {
            // 表示メッセージ作成
            string message = string.Format(format, arg);

            // ロギング
            Logger.Warn(message);
#if DEBUG
            Console.WriteLine("*.. {0} {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), message);
#endif
        }

        /// <summary>
        /// 例外ロギング(Warn)
        /// </summary>
        /// <param name="ex"></param>
        public static void WarnException(Exception ex)
        {
            // 例外メッセージ取得
            string exceptionMessage = MakeExceptionMessage(ex);

            // ロギング
            Logger.Warn(exceptionMessage);
#if DEBUG
            Console.WriteLine("*.. {0} {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), ex.Message);
#endif
        }

        /// <summary>
        /// 例外ロギング(Warn)
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public static void WarnException(Exception ex, string format, params object[] arg)
        {
            // 表示メッセージ作成
            string message = string.Format(format, arg);

            // 例外メッセージ取得
            string exceptionMessage = MakeExceptionMessage(ex);

            // ロギング
            Logger.Warn(message);
            Logger.Warn(exceptionMessage);
#if DEBUG
            Console.WriteLine("*.. {0} {1}:[{2}]", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), message, ex.Message);
#endif
        }
        #endregion

        #region Info
        /// <summary>
        /// ロギング(Info)
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            // ロギング
            Logger.Info(message);
#if DEBUG
            Console.WriteLine("#.. {0} {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), message);
#endif
        }

        /// <summary>
        /// ロギング(Info)
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public static void InfoFormat(string format, params object[] arg)
        {
            // 表示メッセージ作成
            string message = string.Format(format, arg);

            // ロギング
            Logger.Info(message);
#if DEBUG
            Console.WriteLine("#.. {0} {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), message);
#endif
        }

        /// <summary>
        /// 例外ロギング(Info)
        /// </summary>
        /// <param name="ex"></param>
        public static void InfoException(Exception ex)
        {
            // 例外メッセージ取得
            string exceptionMessage = MakeExceptionMessage(ex);

            // ロギング
            Logger.Info(exceptionMessage);
#if DEBUG
            Console.WriteLine("#.. {0} {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), ex.Message);
#endif
        }

        /// <summary>
        /// 例外ロギング(Info)
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public static void InfoException(Exception ex, string format, params object[] arg)
        {
            // 表示メッセージ作成
            string message = string.Format(format, arg);

            // 例外メッセージ取得
            string exceptionMessage = MakeExceptionMessage(ex);

            // ロギング
            Logger.Info(message);
            Logger.Info(exceptionMessage);
#if DEBUG
            Console.WriteLine("#.. {0} {1}:[{2}]", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), message, ex.Message);
#endif
        }
        #endregion

        #region Debug
        /// <summary>
        /// ロギング(Debug)
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            // ロギング
            Logger.Debug(message);
#if DEBUG
            Console.WriteLine("... {0} {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), message);
#endif
        }

        /// <summary>
        /// ロギング(Debug)
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public static void DebugFormat(string format, params object[] arg)
        {
            // 表示メッセージ作成
            string message = string.Format(format, arg);

            // ロギング
            Logger.Debug(message);
#if DEBUG
            Console.WriteLine("... {0} {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), message);
#endif
        }

        /// <summary>
        /// 例外ロギング(Debug)
        /// </summary>
        /// <param name="ex"></param>
        public static void DebugException(Exception ex)
        {
            // 例外メッセージ取得
            string exceptionMessage = MakeExceptionMessage(ex);

            // ロギング
            Logger.Debug(exceptionMessage);
        }

        /// <summary>
        /// 例外ロギング(Debug)
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public static void DebugException(Exception ex, string format, params object[] arg)
        {
            // 表示メッセージ作成
            string message = string.Format(format, arg);

            // 例外メッセージ取得
            string exceptionMessage = MakeExceptionMessage(ex);

            // ロギング
            Logger.Debug(message);
            Logger.Debug(exceptionMessage);
        }
        #endregion

        #region Console
        /// <summary>
        /// ロギング(Console)
        /// </summary>
        /// <param name="message"></param>
        public static void WriteLine(string message)
        {
#if DEBUG
            Console.WriteLine("### {0} {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), message);
#endif
        }

        /// <summary>
        /// ロギング(Console)
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public static void WriteLine(string format, params object[] arg)
        {
#if DEBUG
            // 表示メッセージ作成
            string message = string.Format(format, arg);

            Console.WriteLine("### {0} {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), message);
#endif
        }

        /// <summary>
        /// 例外ロギング(Console)
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteLine(Exception ex)
        {
#if DEBUG
            Console.WriteLine("### " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
            Console.WriteLine(ex.Message);
#endif
        }

        /// <summary>
        /// 例外ロギング(Console)
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public static void WriteLine(Exception ex, string format, params object[] arg)
        {
#if DEBUG
            // 表示メッセージ作成
            string message = string.Format(format, arg);

            // 例外メッセージ取得
            string exceptionMessage = MakeExceptionMessage(ex);

            Console.WriteLine("### " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
            Console.WriteLine(message);
            Console.WriteLine(ex.Message);
#endif
        }
        #endregion

        #region 例外メッセージ作成
        /// <summary>
        /// 例外メッセージ作成
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static string MakeExceptionMessage(Exception ex)
        {
            // オブジェクト判定
            if (ex == null)
            {
                //　処理終了
                return string.Empty;
            }

            // 例外メッセージ作成
            StringBuilder message = new StringBuilder();
            message.AppendLine("[0] --------------------");
            message.AppendLine("【例外情報】");
            message.AppendLine(string.Format("Type       ：{0}", ex.GetType().ToString()));
            message.AppendLine(string.Format("Message    ：{0}", ex.Message));
            message.AppendLine(string.Format("Source     ：{0}", ex.Source));
            message.AppendLine(string.Format("StackTrace ："));
            message.AppendLine(string.Format("{0}", ex.StackTrace));

            // 例外ロギング(再帰)
            message.Append(MakeExceptionMessage(1, ex.InnerException));
            return message.ToString();
        }

        /// <summary>
        /// 例外メッセージ作成
        /// </summary>
        /// <param name="index"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static string MakeExceptionMessage(int index, Exception ex)
        {
            // オブジェクト判定
            if (ex == null)
            {
                //　処理終了
                return string.Empty;
            }

            // 例外メッセージ作成
            StringBuilder message = new StringBuilder();
            message.AppendLine(string.Format("[{0}] --------------------", index));
            message.AppendLine("【例外情報】");
            message.AppendLine(string.Format("Type       ：{0}", ex.GetType().ToString()));
            message.AppendLine(string.Format("Message    ：{0}", ex.Message));
            message.AppendLine(string.Format("Source     ：{0}", ex.Source));
            message.AppendLine(string.Format("StackTrace ："));
            message.AppendLine(string.Format("{0}", ex.StackTrace));

            // 例外ロギング(再帰)
            message.Append(MakeExceptionMessage(index + 1, ex.InnerException));
            return message.ToString();
        }
        #endregion
    }
}
