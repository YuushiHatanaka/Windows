﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace Common.Performance
{
    public class MemoryPerformanceCounter : PerformanceCounterObject
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pCounterName">カウンタ名</param>
        /// <param name="pInstanceName">インスタンス名</param>
        public MemoryPerformanceCounter(String pCounterName, String pInstanceName)
            : base("Memory", pCounterName, pInstanceName, ".")
        {
            // カテゴリ名"Memory"のPerformanceCounterCategoryインスタンスを作成
            var categoryProcessor = new PerformanceCounterCategory("Memory");

            // インスタンス名"_Total"に対応するカウンタを取得
            foreach (var pc in categoryProcessor.GetCounters(pInstanceName))
            {
                Debug.WriteLine("{0,-15} {1,-20} {2,-10}",
                                  pc.CategoryName,
                                  pc.CounterName,
                                  pc.InstanceName);
            }
        }
    }
}