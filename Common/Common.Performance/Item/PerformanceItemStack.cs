using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Collections;

namespace Common.Performance
{
    public class PerformanceItemStack : Stack<PerformanceItem>
    {
        public PerformanceItemStack()
            : base()
        {
        }
        public void Add(PerformanceCounterObject pCounter, PerformanceHistory<float> pHistory)
        {
            this.Push(new PerformanceItem(pCounter, pHistory));
        }
        public PerformanceItem GetItem(int pIndex)
        {
            PerformanceItem[] _PerformanceItem = this.ToArray();
            return _PerformanceItem[pIndex];
        }
    }
}