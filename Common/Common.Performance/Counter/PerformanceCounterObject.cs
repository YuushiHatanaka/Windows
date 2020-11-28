using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Drawing;

namespace Common.Performance
{
    public class PerformanceCounterObject
    {
        /// <summary>
        /// パフォーマンスカウンタオブジェクト
        /// </summary>
        private PerformanceCounter m_PerformanceCounter = null;
        /// <summary>
        /// インスタンス名
        /// </summary>
        public String InstanceName
        {
            get { return m_PerformanceCounter.InstanceName; }
        }
        /// <summary>
        /// チャート線の色
        /// </summary>
        public Color SeriesColor = ColorTranslator.FromHtml("#00FF00");
        /// <summary>
        /// 凡例
        /// </summary>
        public String Legend = String.Empty;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pCategoryName">カテゴリ名</param>
        /// <param name="pCounterName">カウンタ名</param>
        /// <param name="pInstanceName">インスタンス名</param>
        /// <param name="pMachineName">コンピュータ名</param>
        public PerformanceCounterObject(String pCategoryName, String pCounterName, String pInstanceName, String pMachineName)
        {
            // カテゴリ存在判定
            if (!IsCategory(pCategoryName, pMachineName))
            {
                throw new PerformanceCounterException("登録されていないカテゴリです。 : " + pCategoryName);
            }
            // カウンタ存在判定
            if (!IsCounter(pCounterName, pCategoryName, pMachineName))
            {
                throw new PerformanceCounterException("登録されていないカウンタです。 :  " + pCounterName);
            }
            /*
            // インスタンス存在判定
            if (pInstanceName != String.Empty)
            {
                if (!IsInstance(pInstanceName, pCategoryName, pMachineName))
                {
                    throw new PerformanceCounterException("登録されていないインスタンスです。 :  " + pInstanceName);
                }
            }
             */
            // パフォーマンスカウンタオブジェクト生成
            m_PerformanceCounter = new PerformanceCounter(pCategoryName, pCounterName, pInstanceName, pMachineName);
        }
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~PerformanceCounterObject()
        {
            // パフォーマンスカウンタオブジェクト破棄
            if (m_PerformanceCounter != null)
            {
                m_PerformanceCounter.Dispose();
            }
        }
        /// <summary>
        /// カテゴリ存在判定
        /// </summary>
        /// <param name="pCategoryName">カテゴリ名</param>
        /// <param name="pMachineName">コンピュータ名</param>
        /// <returns></returns>
        private bool IsCategory(String pCategoryName, String pMachineName)
        {
            return PerformanceCounterCategory.Exists(pCategoryName, pMachineName);
        }
        /// <summary>
        /// カウンタ存在判定
        /// </summary>
        /// <param name="pCounterName">カウンタ名</param>
        /// <param name="pCategoryName">カテゴリ名</param>
        /// <param name="pMachineName">コンピュータ名</param>
        /// <returns></returns>
        private bool IsCounter(String pCounterName, String pCategoryName, String pMachineName)
        {
            return PerformanceCounterCategory.CounterExists(pCounterName, pCategoryName, pMachineName);
        }
        /// <summary>
        /// インスタンス存在判定
        /// </summary>
        /// <param name="pInstanceName">インスタンス名</param>
        /// <param name="pCategoryName">カテゴリ名</param>
        /// <param name="pMachineName">コンピュータ名</param>
        /// <returns></returns>
        private bool IsInstance(String pInstanceName, String pCategoryName, String pMachineName)
        {
            return PerformanceCounterCategory.InstanceExists(pInstanceName, pCategoryName, pMachineName);
        }
        /// <summary>
        /// カウンター サンプルを取得し、計算される値を返します。
        /// </summary>
        /// <returns>このカウンターのためにシステムで取得された計算される値の次の値。</returns>
        public float NextValue()
        {
            try
            {
                return m_PerformanceCounter.NextValue();
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("m_PerformanceCounter.CategoryName:" + m_PerformanceCounter.CategoryName);
                Debug.WriteLine("m_PerformanceCounter.CounterName :" + m_PerformanceCounter.CounterName);
                Debug.WriteLine("m_PerformanceCounter.InstanceName:" + m_PerformanceCounter.InstanceName);
                Debug.WriteLine("m_PerformanceCounter.MachineName :" + m_PerformanceCounter.MachineName);
                return -1;
            }
        }
    }
}