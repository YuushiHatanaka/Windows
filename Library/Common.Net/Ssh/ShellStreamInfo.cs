using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// ShellStreamInfoクラス
    /// </summary>
    public class ShellStreamInfo
    {
        #region カラム数
        /// <summary>
        /// カラム数
        /// </summary>
        public uint Columns { get; set; } = 80;
        #endregion

        #region 行数
        /// <summary>
        /// 行数
        /// </summary>
        public uint Rows { get; set; } = 24;
        #endregion

        #region 幅
        /// <summary>
        /// 幅
        /// </summary>
        public uint Witdh { get; set; } = 800;
        #endregion

        #region 高さ
        /// <summary>
        /// 高さ
        /// </summary>
        public uint Hight { get; set; } = 600;
        #endregion

        #region バッファサイズ
        /// <summary>
        /// バッファサイズ
        /// </summary>
        public int BufferSize { get; set; } = 8192;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ShellStreamInfo()
        {
        }
        #endregion
    }
}
