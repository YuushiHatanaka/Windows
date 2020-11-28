using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Common.Performance.Task
{
    public class LogicalDiskPerformanceChartTask : PerformanceChartListTask
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pCounterName">カウンタ名</param>
        /// <param name="pInstanceName">インスタンス名</param>
        /// <param name="pCapacity">履歴最大量</param>
        public LogicalDiskPerformanceChartTask(String pCounterName, String pInstanceName, int pCapacity)
            : base(new LogicalDiskPerformanceCounter(pCounterName, pInstanceName), pCapacity)
        {
        }
        public LogicalDiskPerformanceChartTask(PerformanceCounterObject pPerformanceCounterObject, int pCapacity)
            : base(pPerformanceCounterObject, pCapacity)
        {

        }
        public LogicalDiskPerformanceChartTask(PerformanceCounterObject[] pPerformanceCounterObject, int pCapacity)
            : base(pPerformanceCounterObject, pCapacity)
        {

        }
        /// <summary>
        /// 追加
        /// </summary>
        public override void Add()
        {
            //------------------------
            // 値を取得し、履歴に登録
            //------------------------
            ArrayList _ValueList = new ArrayList();
            float _MaxValue = 10.0F;
            for (int i = 0; i < Items.Count; i++)
            {
                PerformanceCounterObject _PerformanceCounterObject = Items[i].Counter;
                PerformanceHistory<float> _PerformanceHistory = Items[i].History;
                float value = _PerformanceCounterObject.NextValue();
                _PerformanceHistory.Add(value);
                _ValueList.Add(value);

                if (_PerformanceHistory.Max > _MaxValue)
                {
                    _MaxValue = _PerformanceHistory.Max;
                }
            }
            this.ChartAreas[0].AxisY.Maximum = _MaxValue;
            this.ChartAreas[0].AxisY.Interval = (int)(_MaxValue / 10);

            // ログ出力
            PrintLog(_ValueList);
        }
    }
}
