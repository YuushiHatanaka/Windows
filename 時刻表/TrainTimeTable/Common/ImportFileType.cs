using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// ImportFileType定数(enum)
    /// </summary>
    public enum ImportFileType
    {
        /// <summary>
        /// 不明
        /// </summary>
        [StringValue("不明")]
        None,

        /// <summary>
        /// WinDIA形式
        /// </summary>
        [StringValue("WinDIA形式")]
        WinDIA,

        /// <summary>
        /// OuDia形式
        /// </summary>
        [StringValue("OuDia形式")]
        OuDia,

        /// <summary>
        /// OuDia2形式
        /// </summary>
        [StringValue("OuDia2形式")]
        OuDia2
    }
}
