using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Common.Logging
{
    /// <summary>
    /// Exceptionクラス(ロギング機能付き)
    /// </summary>
    public class LoggingException : Exception
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LoggingException()
            : base()
        {
            // ロギング
            Logger.Error(MakeExceptionMessage(this));
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="msg"></param>
        public LoggingException(string msg)
            : base(msg)
        {
            // ロギング
            Logger.Error(msg);
            Logger.Error(MakeExceptionMessage(this));
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public LoggingException(string format, params object[] arg)
            : base(string.Format(format, arg))
        {
            // ロギング
            Logger.Error(string.Format(format, arg));
            Logger.Error(MakeExceptionMessage(this));
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="innerException"></param>
        public LoggingException(string msg, Exception innerException)
            : base(msg, innerException)
        {
            // ロギング
            Logger.Error(msg);
            Logger.Error(MakeExceptionMessage(this));
            Logger.Error(MakeExceptionMessage(innerException));
        }
        #endregion

        #region 例外メッセージ作成
        /// <summary>
        /// 例外メッセージ作成
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private string MakeExceptionMessage(Exception ex)
        {
            // 例外メッセージ作成
            StringBuilder message = new StringBuilder();

            // 例外ロギング(再帰)
            message.Append(MakeExceptionMessage(0, ex));

            // 返却
            return message.ToString();
        }

        /// <summary>
        /// 例外メッセージ作成
        /// </summary>
        /// <param name="index"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private string MakeExceptionMessage(int index, Exception ex)
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

            // 返却
            return message.ToString();
        }
        #endregion
    }
}
