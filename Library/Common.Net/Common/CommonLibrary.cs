using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Common.Net
{
    /// <summary>
    /// CommonLibraryクラス
    /// </summary>
    public class CommonLibrary
    {
        #region ローカルアドレス取得(IPv4)
        /// <summary>
        /// ローカルアドレス取得(IPv4)
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        public static IPAddress GetLocalIPAddress(AddressFamily family)
        {
            IPAddress[] _IPAddress = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in _IPAddress)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    return address;
                }
            }
            return null;
        }
        #endregion

        #region 文字コード関連
        /// <summary>
        /// 文字コード変換
        /// </summary>
        /// <param name="from">変換元Encodingオブジェクト</param>
        /// <param name="to">変換先Encodingオブジェクト</param>
        /// <param name="str">変換対象文字列</param>
        /// <returns>変換文字列</returns>
        public static string EncordingString(Encoding from, Encoding to, string str)
        {
            // byte配列取得
            byte[] fromBytesTmp = from.GetBytes(str);

            // NULLスキップ
            byte[] fromBytes = ByteNullSkip(fromBytesTmp);

            // 文字列取得
            string toString = to.GetString(fromBytes);

            // 返却
            return toString;
        }

        /// <summary>
        /// NULLスキップ
        /// </summary>
        /// <param name="bytes">変換対象byte[]オブジェクト</param>
        /// <returns>変換byte[]オブジェクト</returns>
        public static byte[] ByteNullSkip(byte[] bytes)
        {
            // 初期化
            List<byte> result = new List<byte>();

            // バイトオブジェクトサイズ分繰り返す
            foreach (var b in bytes)
            {
                // \0判定
                if (b == 0x00)
                {
                    // スキップ
                    continue;
                }

                // リスト追加
                result.Add(b);
            }

            // 返却
            return result.ToArray();
        }

        /// <summary>
        /// 改行変換
        /// </summary>
        /// <param name="str">変換対象文字列</param>
        /// <returns>変換文字列</returns>
        public static string ConvertReturnCode(string str)
        {
            // 変換
            string result = Regex.Replace(str, "[\\r]+", "\r", RegexOptions.Multiline);

            // 返却
            return result;
        }
        #endregion
    }
}
