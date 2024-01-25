using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// ダイヤグラム列車情報
    /// </summary>
    [Serializable]
    public enum DiagramTrainInformation
    {
        /// <summary>
        /// 不明
        /// </summary>
        [StringValue("不明")]
        None = 0,

        /// <summary>
        /// 始発なら表示
        /// </summary>
        [StringValue("始発なら表示")]
        DisplayIfItIsTheFirstTrain,

        /// <summary>
        /// 常に表示
        /// </summary>
        [StringValue("常に表示")]
        AlwaysVisible,

        /// <summary>
        /// 表示しない
        /// </summary>
        [StringValue("表示しない")]
        DoNotShow,
    }
}
