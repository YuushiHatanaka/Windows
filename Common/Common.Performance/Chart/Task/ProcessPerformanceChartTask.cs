using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;

namespace Common.Performance.Task
{
    public class ProcessPerformanceChartTask : PerformanceChartStackTask
    {
        /// <summary>
        /// 代表インスタンス名
        /// </summary>
        private string m_InstanceName = string.Empty;

        /// <summary>
        /// インスタンス名
        /// </summary>
        public string InstanceName
        {
            set
            {
                this.m_InstanceName = value;
            }
            get
            {
                return this.m_InstanceName;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pCounterName">カウンタ名</param>
        /// <param name="pInstanceName">インスタンス名</param>
        /// <param name="pCapacity">履歴最大量</param>
        public ProcessPerformanceChartTask(string pCounterName, string pInstanceName, int pCapacity)
            : base(new ProcessPerformanceCounter(pCounterName, pInstanceName), pCapacity)
        {
            m_InstanceName = pInstanceName;
        }
        public ProcessPerformanceChartTask(PerformanceCounterObject pPerformanceCounterObject, int pCapacity)
            : base(pPerformanceCounterObject, pCapacity)
        {

        }
        public ProcessPerformanceChartTask(PerformanceCounterObject[] pPerformanceCounterObject, int pCapacity)
            : base(pPerformanceCounterObject, pCapacity)
        {

        }
        /// <summary>
        /// 追加
        /// </summary>
        public override void Add()
        {
            Debug.WriteLine("m_PerformanceList.Count = " + Items.Count.ToString());

            Process[] _Process = Process.GetProcessesByName(m_InstanceName);
            Debug.WriteLine("_Process.Length         = " + _Process.Length.ToString());

            if (!Running)
            {
                return;
            }
            //------------------------
            // 値を取得し、履歴に登録
            //------------------------
            ArrayList _ValueList = new ArrayList();
            for (int i = 0; i < Items.Count; i++)
            {
                PerformanceCounterObject _PerformanceCounterObject = Items.GetItem(i).Counter;
                PerformanceHistory<float> _PerformanceHistory = Items.GetItem(i).History;
                float value = _PerformanceCounterObject.NextValue();
                _PerformanceHistory.Add(value);
                _ValueList.Add(value);
            }

            // ログ出力
            PrintLog(_ValueList);

        }
        /// <summary>
        /// 実行
        /// </summary>
        public override void Run()
        {
            //------------------------
            // 値を取得し、履歴に登録
            //------------------------
            this.Add();

            //------------------------------------------------
            // 履歴の最大数を超えていたら、古いものを削除する
            //------------------------------------------------
            this.Remove();

            //--------------------
            // グラフを再描画する
            //--------------------
            this.ShowChart();
        }
    }
}
