using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Collections;

namespace Common.Performance.Task
{
    public class MemoryPerformanceChartTask: PerformanceChartListTask
    {
        ManagementClass m_ManagementClass = null;
        ManagementObjectCollection m_ManagementObjectCollection = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pCounterName">カウンタ名</param>
        /// <param name="pCapacity">履歴最大量</param>
        public MemoryPerformanceChartTask(String pCounterName, int pCapacity)
            : base(new MemoryPerformanceCounter(pCounterName, String.Empty), pCapacity)
        {
            m_ManagementClass = new System.Management.ManagementClass("Win32_OperatingSystem");
            m_ManagementObjectCollection = m_ManagementClass.GetInstances();

            // 初期化
            Initialization();
        }
        public MemoryPerformanceChartTask(PerformanceCounterObject pPerformanceCounterObject, int pCapacity)
            : base(pPerformanceCounterObject, pCapacity)
        {

        }
        public MemoryPerformanceChartTask(PerformanceCounterObject[] pPerformanceCounterObject, int pCapacity)
            : base(pPerformanceCounterObject, pCapacity)
        {

        }
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~MemoryPerformanceChartTask()
        {
            m_ManagementClass.Dispose();
            m_ManagementObjectCollection.Dispose();
        }
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            /*
            float _TotalVisibleMemorySize = 0;//合計物理メモリ
            float _FreePhysicalMemory = 0;//利用可能物理メモリ
            float _UsagePhysicalMemory = 0;//使用中物理メモリ

            ManagementClass mc = new System.Management.ManagementClass("Win32_OperatingSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                //合計物理メモリ
                Debug.WriteLine("合計物理メモリ:{0}KB", mo["TotalVisibleMemorySize"]);
                _TotalVisibleMemorySize += (float.Parse(mo["TotalVisibleMemorySize"].ToString()) / 1000.0F);
                //利用可能な物理メモリ
                Debug.WriteLine("利用可能物理メモリ:{0}KB", mo["FreePhysicalMemory"]);
                _FreePhysicalMemory += (float.Parse(mo["FreePhysicalMemory"].ToString()) / 1000.0F);
                //合計仮想メモリ
                Debug.WriteLine("合計仮想メモリ:{0}KB", mo["TotalVirtualMemorySize"]);
                //利用可能な仮想メモリ
                Debug.WriteLine("利用可能仮想メモリ:{0}KB", mo["FreeVirtualMemory"]);

                //他のページをスワップアウトせずにページングファイルにマップできるサイズ
                Debug.WriteLine("FreeSpaceInPagingFiles:{0}KB", mo["FreeSpaceInPagingFiles"]);
                //ページングファイルに保存できる合計サイズ
                Debug.WriteLine("SizeStoredInPagingFiles:{0}KB", mo["SizeStoredInPagingFiles"]);
                //スワップスペースの合計サイズ
                //スワップスペースとページングファイルが区別されていなければ、NULL
                Debug.WriteLine("TotalSwapSpaceSize:{0}KB", mo["TotalSwapSpaceSize"]);

                mo.Dispose();
            }
            _UsagePhysicalMemory = _TotalVisibleMemorySize - _FreePhysicalMemory;
            Debug.WriteLine("_TotalVisibleMemorySize:{0}MB", _TotalVisibleMemorySize);
            Debug.WriteLine("_FreePhysicalMemory:{0}MB", _FreePhysicalMemory);
            Debug.WriteLine("_UsagePhysicalMemory:{0}MB", _UsagePhysicalMemory);

            moc.Dispose();
            mc.Dispose();
            */
            // 初期値変更
            this.ChartAreas[0].AxisY.Minimum = 0;  // 縦軸の最小値を0にする
            this.ChartAreas[0].AxisY.Maximum = this.TotalVisibleMemorySize;
            this.ChartAreas[0].AxisY.Interval = 1000;
        }
        private float FreePhysicalMemory
        {
            get
            {
                float _FreePhysicalMemory = 0;//利用可能物理メモリ
                foreach (System.Management.ManagementObject mo in m_ManagementObjectCollection)
                {
                    _FreePhysicalMemory += (float.Parse(mo["FreePhysicalMemory"].ToString()) / 1000.0F);
                }
                return _FreePhysicalMemory;
            }
        }
        private float TotalVisibleMemorySize
        {
            get
            {
                float _TotalVisibleMemorySize = 0;//合計物理メモリ
                foreach (System.Management.ManagementObject mo in m_ManagementObjectCollection)
                {
                    _TotalVisibleMemorySize += (float.Parse(mo["TotalVisibleMemorySize"].ToString()) / 1000.0F);
                }
                return _TotalVisibleMemorySize;
            }
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
            for (int i = 0; i < Items.Count; i++)
            {
                PerformanceCounterObject _PerformanceCounterObject = Items[i].Counter;
                PerformanceHistory<float> _PerformanceHistory = Items[i].History;
                float value = this.TotalVisibleMemorySize - _PerformanceCounterObject.NextValue();
                _PerformanceHistory.Add(value);
                _ValueList.Add(value);
            }

            // ログ出力
            PrintLog(_ValueList);
        }
    }
}
