using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace Common.Performance.Task
{
    public class PerformanceChartListTask : PerformanceChartTask
    {
        private PerformanceItemList m_Items = new PerformanceItemList();
        public PerformanceItemList Items
        {
            get { return m_Items; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pPerformanceCounterObject"></param>
        /// <param name="pCapacity"></param>
        public PerformanceChartListTask(PerformanceCounterObject pPerformanceCounterObject, int pCapacity)
            : base()
        {
            m_Items.Add(new PerformanceItem(pPerformanceCounterObject, new PerformanceHistory<float>(pCapacity)));

            Initialization();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pPerformanceCounterObject"></param>
        /// <param name="pCapacity"></param>
        public PerformanceChartListTask(PerformanceCounterObject[] pPerformanceCounterObject, int pCapacity)
            : base()
        {
            for (int i = 0; i < pPerformanceCounterObject.Length; i++)
            {
                m_Items.Add(new PerformanceItem(pPerformanceCounterObject[i], new PerformanceHistory<float>(pCapacity)));
            }

            Initialization();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            // チャート線の属性を設定
            foreach (PerformanceItem _PerformanceItem in Items)
            {
                Series _Series = new Series();

                // 折れ線グラフとして表示
                _Series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;

                // 線の色を指定
                _Series.Color = _PerformanceItem.Counter.SeriesColor;

                // 各値に数値の非表示を設定
                _Series.IsValueShownAsLabel = false;

                // 凡例の表示・非表示を設定
                _Series.IsVisibleInLegend = false;
                if (_PerformanceItem.Counter.Legend != String.Empty)
                {
                    _Series.IsVisibleInLegend = true;
                    _Series.LegendText = _PerformanceItem.Counter.Legend;
                    Legend _Legend = this.Legends.Add(_Series.LegendText);
                    _Legend.Docking = Docking.Bottom;
                }

                this.Series.Add(_Series);
            }
        }

        /// <summary>
        /// 表示
        /// </summary>
        public override void ShowChart()
        {
            //-----------------------
            // チャートに値をセット
            //-----------------------
            for (int i = 0; i < Items.Count; i++)
            {
                this.Series[i].Points.Clear();
                PerformanceHistory<float> _PerformanceHistory = Items[i].History;
                foreach (float value in _PerformanceHistory.Queue)
                {
                    // データをチャートに追加
                    DataPoint _DataPoint = new DataPoint(0, value);
                    this.Series[i].Points.Add(_DataPoint);
                }
            }
        }

        /// <summary>
        /// 追加
        /// </summary>
        public override void Add()
        {
            // 実行判定
            if (!this.Running)
            {
                // 実行中ではないため何もしない
                return;
            }

            //------------------------
            // 値を取得し、履歴に登録
            //------------------------
            ArrayList _ValueList = new ArrayList();
            for (int i = 0; i < Items.Count; i++)
            {
                PerformanceCounterObject _PerformanceCounterObject = Items[i].Counter;
                PerformanceHistory<float> _PerformanceHistory = Items[i].History;
                float value = _PerformanceCounterObject.NextValue();
                _PerformanceHistory.Add(value);
                _ValueList.Add(value);
            }

            // ログ出力
            PrintLog(_ValueList);
        }

        /// <summary>
        /// 削除
        /// </summary>
        public override void Remove()
        {
            // 実行判定
            if (!this.Running)
            {
                // 実行中ではないため何もしない
                return;
            }

            //------------------------------------------------
            // 履歴の最大数を超えていたら、古い履歴を削除する
            //------------------------------------------------
            for (int i = 0; i < Items.Count; i++)
            {
                PerformanceHistory<float> _PerformanceHistory = Items[i].History;
                _PerformanceHistory.RemoveOldest();
            }
        }

        /// <summary>
        /// クリア
        /// </summary>
        public override void Clear()
        {
            //------------------------------------------------
            // 全ての履歴を削除する
            //------------------------------------------------
            for (int i = 0; i < Items.Count; i++)
            {
                PerformanceHistory<float> _PerformanceHistory = Items[i].History;
                _PerformanceHistory.Clear();
            }

            // 描画
            this.Drawing();
        }
    }
}

