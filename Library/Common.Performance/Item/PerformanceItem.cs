using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Performance
{
    public class PerformanceItem
    {
        private PerformanceCounterObject m_Counter = null;
        public PerformanceCounterObject Counter
        {
            get { return m_Counter; }
        }
        private PerformanceHistory<float> m_History = null;
        public PerformanceHistory<float> History
        {
            get { return m_History; }
        }
        public PerformanceItem(PerformanceCounterObject pCounter, PerformanceHistory<float> pHistory)
        {
            m_Counter = pCounter;
            m_History = pHistory;
        }
    }
}