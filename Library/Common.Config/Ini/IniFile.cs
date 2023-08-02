using log4net;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Common.Config
{
    /// <summary>
    /// IniFileクラス
    /// </summary>
    public class IniFile : IDisposable
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

        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Disposeフラグ
        /// <summary>
        /// Disposeフラグ
        /// </summary>
        protected bool m_Disposed = false;
        #endregion

        #region 定義ファイル名
        /// <summary>
        /// 定義ファイル名
        /// </summary>
        protected string FileName { get; set; } = string.Empty;
        #endregion

        #region バッファサイズ
        /// <summary>
        /// バッファサイズ
        /// </summary>
        public int Capacity { get; protected set; } = 0;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileName"></param>
        public IniFile(string fileName)
            : this(fileName, 1024)
        {
            // ロギング
            Logger.Debug("=>>>> IniFile::IniFile(string)");
            Logger.DebugFormat("fileName:{0}", fileName);

            // ロギング
            Logger.Debug("<<<<= IniFile::IniFile(string)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="capacity"></param>
        public IniFile(string fileName, int capacity)
        {
            // ロギング
            Logger.Debug("=>>>> IniFile::IniFile(string)");
            Logger.DebugFormat("fileName:{0}", fileName);
            Logger.DebugFormat("capacity:{0}", capacity);

            // 設定
            FileName = fileName;
            Capacity = capacity;

            // ロギング
            Logger.Debug("<<<<= IniFile::IniFile(string)");
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~IniFile()
        {
            // ロギング
            Logger.Debug("=>>>> IniFile::~IniFile()");

            // リソース破棄
            Dispose(false);

            // ロギング
            Logger.Debug("<<<<= IniFile::~IniFile()");
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // ロギング
            Logger.Debug("=>>>> IniFile::Dispose()");

            // リソース破棄
            Dispose(true);

            // ガベージコレクション
            GC.SuppressFinalize(this);

            // ロギング
            Logger.Debug("<<<<= IniFile::Dispose()");
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            // ロギング
            Logger.Debug("=>>>> IniFile::Dispose(bool)");

            if (!m_Disposed)
            {
                if (disposing)
                {
                    // TODO: Dispose managed resources here.
                }

                // TODO: Free unmanaged resources here.

                // Note disposing has been done.
                m_Disposed = true;
            }

            // ロギング
            Logger.Debug("<<<<= IniFile::Dispose(bool)");
        }
        #endregion

        #region 存在判定
        /// <summary>
        /// 存在判定
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            // ロギング
            Logger.Debug("=>>>> IniFile::IsExist()");

            // ファイル存在判定
            bool result = File.Exists(FileName);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= IniFile::IsExist()");

            // 返却
            return result;
        }
        #endregion

        #region セクション一覧取得
        /// <summary>
        /// セクション一覧取得
        /// </summary>
        /// <returns></returns>
        public string[] GetSections()
        {
            // ロギング
            Logger.Debug("=>>>> IniFile::GetSections()");

            byte[] ar2 = new byte[Capacity];

            uint resultSize2
                  = GetPrivateProfileStringByByteArray(
                        null, null, "default", ar2,
                        (uint)ar2.Length, FileName);
            string result2 = Encoding.Default.GetString(
                                    ar2, 0, (int)resultSize2 - 1);

            // セクション一覧取得
            string[] result = result2.Split('\0');

            // ロギング
            Logger.DebugFormat("result:{0}", result.Length);
            Logger.Debug("<<<<= IniFile::GetSections()");

            // 結果返却
            return result;
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
            // ロギング
            Logger.Debug("=>>>> IniFile::GetKeys()");
            Logger.DebugFormat("section:{0}", section);

            byte[] ar1 = new byte[Capacity];
            uint resultSize1
                  = GetPrivateProfileStringByByteArray(
                        section, null, "default", ar1,
                        (uint)ar1.Length, FileName);
            string result1 = Encoding.Default.GetString(
                                    ar1, 0, (int)resultSize1 - 1);

            // キー一覧取得
            string[] result = result1.Split('\0');

            // ロギング
            Logger.DebugFormat("result:{0}", result.Length);
            Logger.Debug("<<<<= IniFile::GetKeys()");

            // 結果返却
            return result;
        }
        #endregion

        #region 書込
        /// <summary>
        /// 書込
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Write(string section, string key, string value)
        {
            // ロギング
            Logger.Debug("=>>>> IniFile::Write(string, string, string)");
            Logger.DebugFormat("section:{0}", section);
            Logger.DebugFormat("key    :{0}", key);
            Logger.DebugFormat("value  :{0}", value);

            // 書込
            WritePrivateProfileString(section, key, value, FileName);

            // ロギング
            Logger.Debug("<<<<= IniFile::Write(string, string, string)");
        }

        /// <summary>
        /// 書込
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Write(string section, string key, bool value)
        {
            // ロギング
            Logger.Debug("=>>>> IniFile::Write(string, string, bool)");
            Logger.DebugFormat("section:{0}", section);
            Logger.DebugFormat("key    :{0}", key);
            Logger.DebugFormat("value  :{0}", value);

            // 書込
            WritePrivateProfileString(section, key, value.ToString(), FileName);

            // ロギング
            Logger.Debug("<<<<= IniFile::Write(string, string, bool)");
        }

        /// <summary>
        /// 書込
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Write(string section, string key, uint value)
        {
            // ロギング
            Logger.Debug("=>>>> IniFile::Write(string, string, uint)");
            Logger.DebugFormat("section:{0}", section);
            Logger.DebugFormat("key    :{0}", key);
            Logger.DebugFormat("value  :{0}", value);

            // 書込
            WritePrivateProfileString(section, key, value.ToString(), FileName);

            // ロギング
            Logger.Debug("<<<<= IniFile::Write(string, string, uint)");
        }
        #endregion

        #region 読み出し
        #region 文字列読み出し
        /// <summary>
        /// 文字列読み出し
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetStringValue(string section, string key)
        {
            // ロギング
            Logger.Debug("=>>>> IniFile::GetStringValue(string, string)");
            Logger.DebugFormat("section:{0}", section);
            Logger.DebugFormat("key    :{0}", key);

            StringBuilder sb = new StringBuilder(Capacity);
            GetPrivateProfileString(section, key, "", sb, (uint)sb.Capacity, FileName);

            // 文字取得
            string result = sb.ToString();

            // ロギング
            Logger.DebugFormat("result:{0}", result);
            Logger.Debug("<<<<= IniFile::GetStringValue(string, string)");

            // 結果返却
            return result;
        }
        #endregion

        #region Bool読み出し
        /// <summary>
        /// Bool読み出し
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetBoolValue(string section, string key)
        {
            // ロギング
            Logger.Debug("=>>>> IniFile::GetBoolValue(string, string)");
            Logger.DebugFormat("section:{0}", section);
            Logger.DebugFormat("key    :{0}", key);

            StringBuilder sb = new StringBuilder(Capacity);
            GetPrivateProfileString(section, key, "", sb, (uint)sb.Capacity, FileName);

            // 文字取得
            string result = sb.ToString();

            // ロギング
            Logger.DebugFormat("result:{0}", result);
            Logger.Debug("<<<<= IniFile::GetBoolValue(string, string)");

            // 結果返却
            return bool.Parse(result);
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
            // ロギング
            Logger.Debug("=>>>> IniFile::GetIntValue(string, string)");
            Logger.DebugFormat("section:{0}", section);
            Logger.DebugFormat("key    :{0}", key);

            // 数値取得
            uint result = GetPrivateProfileInt(section, key, 0, FileName);

            // ロギング
            Logger.DebugFormat("result:{0}", result);
            Logger.Debug("<<<<= IniFile::GetIntValue(string, string)");

            // 結果返却
            return result;
        }
        #endregion
        #endregion

        #region 削除
        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="section"></param>
        public void Remove(string section)
        {
            // ロギング
            Logger.Debug("=>>>> IniFile::Remove(string)");
            Logger.DebugFormat("section:{0}", section);

            // 指定セクション内の全てのキーと値のペアを削除する
            uint result = WritePrivateProfileString(section, null, null, FileName);

            // ロギング
            Logger.DebugFormat("result:{0}", result);
            Logger.Debug("<<<<= IniFile::Remove(string)");
        }

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        public void Remove(string section, string key)
        {
            // ロギング
            Logger.Debug("=>>>> IniFile::Remove(string, string)");
            Logger.DebugFormat("section:{0}", section);
            Logger.DebugFormat("key    :{0}", key);

            // 1つのキーと値のペアを削除する
            uint result = WritePrivateProfileString(section, key, null, FileName);

            // ロギング
            Logger.DebugFormat("result:{0}", result);
            Logger.Debug("<<<<= IniFile::Remove(string, string)");
        }
        #endregion
    }
}
