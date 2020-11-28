using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Collections;

namespace Common.Performance
{
    public class PerformanceItemList : List<PerformanceItem>
    {
        public PerformanceItemList()
            : base()
        {
        }
        public void Add(PerformanceCounterObject pCounter, PerformanceHistory<float> pHistory)
        {
            this.Add(new PerformanceItem(pCounter, pHistory));
        }
    }
}