using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace Common.Performance
{
    public class ProcessorPerformanceChart : PerformanceChartList
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pCounterName">カウンタ名</param>
        /// <param name="pInstanceName">インスタンス名</param>
        /// <param name="pCapacity">履歴最大量</param>
        public ProcessorPerformanceChart(String pCounterName, String pInstanceName, int pCapacity)
            : base(new ProcessorPerformanceCounter(pCounterName, pInstanceName), pCapacity)
        {
        }
        public ProcessorPerformanceChart(PerformanceCounterObject pPerformanceCounterObject, int pCapacity)
            : base(pPerformanceCounterObject, pCapacity)
        {
        }
        public ProcessorPerformanceChart(PerformanceCounterObject[] pPerformanceCounterObject, int pCapacity)
            : base(pPerformanceCounterObject, pCapacity)
        {
        }
    }
}