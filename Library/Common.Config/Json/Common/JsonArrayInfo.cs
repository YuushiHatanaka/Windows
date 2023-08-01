using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Config
{
    /// <summary>
    /// JsonArray情報クラス
    /// </summary>
    public class JsonArrayInfo
    {
        /// <summary>
        /// インデックス値
        /// </summary>
        private int m_Value = 0;

        /// <summary>
        /// キー値
        /// </summary>
        private string m_Key = string.Empty;

        /// <summary>
        /// インデックス値
        /// </summary>
        public int Value
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

        /// <summary>
        /// キー値
        /// </summary>
        public string Key
        {
            get
            {
                return m_Key;
            }
            set
            {
                m_Key = value;
            }
        }
    }
}
