using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Data;
using System.Management;
using System.Collections;

namespace Common.Performance
{
    class NetworkPerformanceChart : PerformanceChartList
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pCounterName">カウンタ名</param>
        /// <param name="pCapacity">履歴最大量</param>
        public NetworkPerformanceChart(String pCounterName, String pInstanceName, int pCapacity)
            : base(new NetworkInterfacePerformanceCounter(pCounterName, pInstanceName), pCapacity)
        {
        }
        public NetworkPerformanceChart(PerformanceCounterObject pPerformanceCounterObject, int pCapacity)
            : base(pPerformanceCounterObject, pCapacity)
        {

        }
        public NetworkPerformanceChart(PerformanceCounterObject[] pPerformanceCounterObject, int pCapacity)
            : base(pPerformanceCounterObject, pCapacity)
        {

        }
        /// <summary>
        /// 追加
        /// </summary>
        public override void Add()
        {
            if (!Running)
            {
                return;
            }
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
        public static String GetNetworkAdapterName(string networkName)
        {
            string adapter = null;
            try
            {
                ObjectQuery oq = new ObjectQuery("select * from Win32_NetworkAdapter");
                ManagementObjectSearcher mos = new ManagementObjectSearcher(oq);
                foreach (ManagementObject mo in mos.Get())
                {
                    string id = (String)mo.Properties["NetConnectionID"].Value;
                    if (id == networkName)
                    {
                        adapter = (String)mo.Properties["Description"].Value;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            /*
            adapter = adapter.Replace('(', '['); // 暫定
            adapter = adapter.Replace(')', ']'); // 暫定
             */
            return adapter;
        }
    }
}