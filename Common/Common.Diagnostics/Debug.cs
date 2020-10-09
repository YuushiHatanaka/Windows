using System;
using System.IO;
using System.Text;

namespace Common.Diagnostics
{
    /// <summary>
    /// デバッグクラス
    /// </summary>
    public class Debug
    {
        #region Dump
        /// <summary>
        /// Dump
        /// </summary>
        /// <param name="stream"></param>
        public static string Dump(MemoryStream stream)
        {
            // Dump
            return Debug.Dump(0, stream.ToArray(), stream.Length);
        }

        /// <summary>
        /// Dump
        /// </summary>
        /// <param name="indent"></param>
        /// <param name="stream"></param>
        public static string Dump(int indent, MemoryStream stream)
        {
            // Dump
            return Debug.Dump(indent, stream.ToArray(), stream.Length);
        }

        /// <summary>
        /// Dump
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Dump(byte[] value)
        {
            // Dump
            return Debug.Dump(0, value, value.Length);
        }

        /// <summary>
        /// Dump
        /// </summary>
        /// <param name="indent"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Dump(int indent, byte[] value)
        {
            // Dump
            return Debug.Dump(indent, value, value.Length);
        }
        
        /// <summary>
        /// Dump
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Dump(byte[] value, long length)
        {
            // Dump
            return Debug.Dump(0, value, length);
        }

        /// <summary>
        /// Dump
        /// </summary>
        /// <param name="indent"></param>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Dump(int indent, byte[] value, long length)
        {
            // ダンプイメージ返却用オブジェクト
            StringBuilder _logmsg = new StringBuilder();

            StringBuilder text = new StringBuilder();
            int i = 0;
            while (i < length)
            {
                // アドレス出力
                if ((i % 16) == 0)
                {
                    // アドレス文字列設定
                    string repeatedString = new string(' ', indent);
                    _logmsg.Append(repeatedString);
                    _logmsg.Append(string.Format("{0:x8} ", i));
                    text.Length = 0;
                    text.Clear();
                }
                string c = System.Text.Encoding.ASCII.GetString(value, i, 1);
                char[] charArray = c.ToCharArray();
                if (value[i] < 0x20 || value[i] > 0x7f)
                {
                    text.Append(".");
                }
                else
                {
                    text.Append(string.Format("{0}", c));
                }
                _logmsg.Append(string.Format("{0:x2} ", value[i]));
                i++;
                // テキスト部分出力
                if ((i % 16) == 0)
                {
                    _logmsg.AppendLine(string.Format(" : {0}", text.ToString()));
                }
            }
            if ((i % 16) != 0)
            {
                string repeatedString = new string(' ', (16 - (i % 16)) * 3 + 1);
                _logmsg.Append(repeatedString);
                _logmsg.Append(string.Format(": {0}", text.ToString()));
            }

            // ダンプイメージ返却
            return _logmsg.ToString();
        }
        #endregion
    }
}
