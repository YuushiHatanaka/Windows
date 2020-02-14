using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace Common.Data.Config
{
    /// <summary>
    /// INIファイルクラス
    /// </summary>
    public class IniFile
    {
        #region DllImport
        [DllImport("KERNEL32.DLL")]
        private static extern uint
          GetPrivateProfileString(string lpAppName,
          string lpKeyName, string lpDefault,
          StringBuilder lpReturnedString, uint nSize,
          string lpFileName);

        [DllImport("KERNEL32.DLL",
            EntryPoint = "GetPrivateProfileStringA")]
        private static extern uint
          GetPrivateProfileStringByByteArray(string lpAppName,
          string lpKeyName, string lpDefault,
          byte[] lpReturnedString, uint nSize,
          string lpFileName);

        [DllImport("KERNEL32.DLL")]
        private static extern uint
          GetPrivateProfileInt(string lpAppName,
          string lpKeyName, int nDefault, string lpFileName);

        [DllImport("KERNEL32.DLL")]
        private static extern uint WritePrivateProfileString(
          string lpAppName,
          string lpKeyName,
          string lpString,
          string lpFileName);
        #endregion

        /// <summary>
        /// ファイル名
        /// </summary>
        private string m_FileName = string.Empty;

        /// <summary>
        /// バッファサイズ
        /// </summary>
        private int m_Capacity = 1024;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileName"></param>
        public IniFile(string fileName)
        {
            // 設定
            this.m_FileName = fileName;
        }
        #endregion

        #region セクション一覧取得
        /// <summary>
        /// セクション一覧取得
        /// </summary>
        /// <returns></returns>
        public string[] GetSections()
        {
            byte[] ar2 = new byte[this.m_Capacity];

            uint resultSize2
                  = GetPrivateProfileStringByByteArray(
                        null, null, "default", ar2,
                        (uint)ar2.Length, this.m_FileName);
            string result2 = Encoding.Default.GetString(
                                    ar2, 0, (int)resultSize2 - 1);
            // セクション一覧を返却
            return result2.Split('\0');
        }
        #endregion

        #region キー一覧取得
        /// <summary>
        /// キー一覧取得
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public string[] GetKeys(string section)
        {
            byte[] ar1 = new byte[this.m_Capacity];
            uint resultSize1
                  = GetPrivateProfileStringByByteArray(
                        section, null, "default", ar1,
                        (uint)ar1.Length, this.m_FileName);
            string result1 = Encoding.Default.GetString(
                                    ar1, 0, (int)resultSize1 - 1);
            // キー一覧を返却
            return result1.Split('\0');
        }
        #endregion

        #region 追加
        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, this.m_FileName);
        }
        #endregion

        #region 文字列読み出し
        /// <summary>
        /// 文字列読み出し
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetStringValue(string section, string key)
        {
            StringBuilder sb = new StringBuilder(this.m_Capacity);
            GetPrivateProfileString(section, key,
                "default", sb, (uint)sb.Capacity, this.m_FileName);
            return sb.ToString();
        }
        #endregion

        #region 数値読み出し
        /// <summary>
        /// 数値読み出し
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public uint GetIntValue(string section, string key)
        {
            return GetPrivateProfileInt(section, key, 0, this.m_FileName);
        }
        #endregion

        #region 削除
        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="section"></param>
        public void Remove(string section)
        {
            // 指定セクション内の全てのキーと値のペアを削除する
            WritePrivateProfileString(section, null, null, this.m_FileName);
        }

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        public void Remove(string section, string key)
        {
            // 1つのキーと値のペアを削除する
            WritePrivateProfileString(section, key, null, this.m_FileName);
        }
        #endregion
    }
}
