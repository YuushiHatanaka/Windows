using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Performance.Task
{
    public class ProcessorPerformanceChartTask : PerformanceChartListTask
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pCounterName">カウンタ名</param>
        /// <param name="pInstanceName">インスタンス名</param>
        /// <param name="pCapacity">履歴最大量</param>
        public ProcessorPerformanceChartTask(String pCounterName, String pInstanceName, int pCapacity)
            : base(new ProcessorPerformanceCounter(pCounterName, pInstanceName), pCapacity)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pPerformanceCounterObject"></param>
        /// <param name="pCapacity"></param>
        public ProcessorPerformanceChartTask(PerformanceCounterObject pPerformanceCounterObject, int pCapacity)
            : base(pPerformanceCounterObject, pCapacity)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pPerformanceCounterObject"></param>
        /// <param name="pCapacity"></param>
        public ProcessorPerformanceChartTask(PerformanceCounterObject[] pPerformanceCounterObject, int pCapacity)
            : base(pPerformanceCounterObject, pCapacity)
        {
        }
    }
}
