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
    }
}
