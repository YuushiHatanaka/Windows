using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.Config
{
    /// <summary>
    /// INIファイルクラス(高速化版)
    /// </summary>
    public class IniFileSpeedingUp : IniFile
    {
        /// <summary>
        /// 内部データ
        /// </summary>
        private Dictionary<string, Dictionary<string, string>> m_Data = new Dictionary<string, Dictionary<string, string>>();

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileName"></param>
        public IniFileSpeedingUp(string fileName)
            : base(fileName)
        {
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~IniFileSpeedingUp()
        {
            // クリア
            this.Clear();
        }
        #endregion

        #region クリア
        /// <summary>
        /// クリア
        /// </summary>
        private void Clear()
        {
            // セッション数分繰返し
            foreach(string _session in this.m_Data.Keys)
            {
                // データクリア
                this.Clear(_session);
            }

            // 全体クリア
            this.m_Data.Clear();
        }

        /// <summary>
        /// クリア
        /// </summary>
        /// <param name="session"></param>
        private void Clear(string session)
        {
            // クリア
            this.m_Data[session].Clear();
        }
        #endregion

        #region 読込
        /// <summary>
        /// 読込
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            // クリア
            this.Clear();

            // セッション数分繰返し
            foreach (string _session in this.GetSections())
            {
                // セッション作成
                this.m_Data[_session] = new Dictionary<string, string>();

                // セッション内キーを取得
                foreach (string _key in this.GetKeys(_session))
                {
                    this.m_Data[_session][_key] = this.GetStringValue(_session, _key);
                }
            }

            // 正常終了
            return true;
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            // セッション数分繰返し
            foreach (string _session in this.m_Data.Keys)
            {
                // セッション内キーを取得
                foreach (string _key in this.m_Data[_session].Keys)
                {
                    // 書込
                    this.Write(_session, _key, this.m_Data[_session][_key]);
                }
            }

            // 正常終了
            return true;
        }
        #endregion

        #region 文字列化
        /// <summary>
        /// 文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder _ToString = new StringBuilder();

            // セッション数分繰返し
            foreach (string _session in this.m_Data.Keys)
            {
                // セッション作成
                _ToString.AppendFormat("[{0}]\n", _session);

                // セッション内キーを取得
                foreach (string _key in this.m_Data[_session].Keys)
                {
                    // キー、値作成
                    _ToString.AppendFormat("{0}={1}\n", _key, this.m_Data[_session][_key]);
                }
            }

            // 文字列を返却
            return _ToString.ToString();
        }
        #endregion
    }
}
