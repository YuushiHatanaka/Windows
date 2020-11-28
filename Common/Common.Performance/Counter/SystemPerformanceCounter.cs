﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace Common.Performance
{
    public class SystemPerformanceCounter : PerformanceCounterObject
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pCounterName">カウンタ名</param>
        /// <param name="pInstanceName">インスタンス名</param>
        public SystemPerformanceCounter(String pCounterName, String pInstanceName)
            : base("System", pCounterName, pInstanceName, ".")
        {
            // カテゴリ名"System"のPerformanceCounterCategoryインスタンスを作成
            var categoryProcessor = new PerformanceCounterCategory("System");

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