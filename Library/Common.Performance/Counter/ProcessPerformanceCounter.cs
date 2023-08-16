using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Drawing;

namespace Common.Performance
{
    public class ProcessPerformanceCounter : PerformanceCounterObject
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pCounterName">カウンタ名</param>
        /// <param name="pInstanceName">インスタンス名</param>
        public ProcessPerformanceCounter(String pCounterName, String pInstanceName)
            : base("Process", pCounterName, pInstanceName, ".")
        {
            /*
            // カテゴリ名"Process"のPerformanceCounterCategoryインスタンスを作成
            var categoryProcessor = new PerformanceCounterCategory("Process");

            // インスタンス名"_Total"に対応するカウンタを取得
            foreach (var pc in categoryProcessor.GetCounters(pInstanceName))
            {
                Debug.WriteLine("{0,-15} {1,-20} {2,-10}",
                                  pc.CategoryName,
                                  pc.CounterName,
                                  pc.InstanceName);
            }
             */
        }
        /// <summary>
        /// インスタンス名作成
        /// </summary>
        /// <param name="pBaseName"></param>
        /// <param name="pNo"></param>
        /// <returns></returns>
        public static String MakeInstanceName(String pBaseName, int pNo)
        {
            String _MakeInstanceName = pBaseName;
            if (pNo > 0)
            {
                _MakeInstanceName += "#" + pNo.ToString();
            }
            return _MakeInstanceName;
        }
        public static Color GetInstanceColor(int pNo)
        {
            int _BaseColorVaue = 0x00ffffff;
            int _ColorVaue = _BaseColorVaue & (0xff << (pNo % 20));

            String _ColorFromHtml = "#" + _ColorVaue.ToString("X6");

            Color _Color = ColorTranslator.FromHtml(_ColorFromHtml);
            Debug.WriteLine(_Color.ToString());

            return _Color;
        }
    }
}