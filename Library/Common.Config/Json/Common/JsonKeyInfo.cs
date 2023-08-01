using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Config
{
    /// <summary>
    /// Jsonキー情報クラス
    /// </summary>
    public class JsonKeyInfo
    {
        /// <summary>
        /// キー開始位置
        /// </summary>
        private uint m_StartColum = 0;

        /// <summary>
        /// キー値
        /// </summary>
        private string m_Value = string.Empty;

        /// <summary>
        /// キー開始位置
        /// </summary>
        public uint StartColum
        {
            get
            {
                return m_StartColum;
            }
            set
            {
                m_StartColum = value;
            }
        }

        /// <summary>
        /// キー値
        /// </summary>
        public string Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;
            }
        }
    }
}
