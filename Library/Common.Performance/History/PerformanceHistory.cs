using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace Common.Performance
{
    /// <summary>
    /// パフォーマンス履歴クラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PerformanceHistory<T>
    {
        /// <summary>
        /// 履歴許容量(実体)
        /// </summary>
        private int m_Capacity = 0;

        /// <summary>
        /// 履歴許容量
        /// </summary>
        public int Capacity
        {
            get { return this.m_Capacity; }
        }

        /// <summary>
        /// 履歴格納Queue(実体)
        /// </summary>
        Queue<T> m_History = new Queue<T>();

        /// <summary>
        /// 履歴格納Queue
        /// </summary>
        public Queue<T> Queue
        {
            get { return this.m_History; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PerformanceHistory(int pCapacity)
        {
            this.m_Capacity = pCapacity;

            Initialization();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            //----------------------------------------------------
            // チャートに表示させる値の履歴を全てデフォルト値設定
            //----------------------------------------------------
            while (this.m_History.Count <= this.Capacity)
            {
                this.Add(default(T));
            }
        }

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="pValue"></param>
        public void Add(T pValue)
        {
            this.m_History.Enqueue(pValue);
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            // 履歴クリア
            this.m_History.Clear();

            // 初期化
            this.Initialization();
        }

        /// <summary>
        /// 履歴の最大数を超えていたら、古いものを削除します
        /// </summary>
        public void RemoveOldest()
        {
            //------------------------------------------------
            // 履歴の最大数を超えていたら、古いものを削除する
            //------------------------------------------------
            while (this.m_History.Count > m_Capacity)
            {
                this.m_History.Dequeue();
            }
        }

        /// <summary>
        /// 最小値
        /// </summary>
        public T Min
        {
            get { return this.m_History.Min(); }
        }

        /// <summary>
        /// 最大値
        /// </summary>
        public T Max
        {
            get { return this.m_History.Max(); }
        }
    }
}