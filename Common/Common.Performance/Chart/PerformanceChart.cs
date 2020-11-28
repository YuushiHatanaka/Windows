using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Collections;
using System.Threading;

namespace Common.Performance
{
    public class PerformanceChart : Chart
    {
        private bool m_Running = true;
        public bool Running
        {
            get { return m_Running; }
            set { m_Running = value; }
        }

        /// <summary>
        /// ログ出力スレッド
        /// </summary>
        Thread m_LogThread = null;
        PerformanceLog m_PerformanceLog = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PerformanceChart()
        {
            // 初期化
            Initialization();
        }
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~PerformanceChart()
        {
            // 破棄
            Destruction();
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Destruction()
        {
            Running = false;
            if (m_PerformanceLog != null)
            {
                m_PerformanceLog.Stop();
            }
        }
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            // チャート全体の背景色を設定
            this.BackColor = Color.Black;
            this.AntiAliasing = AntiAliasingStyles.None;

            // チャートエリアを設定(チャートエリアは１面のみ)
            ChartArea _ChartArea = new ChartArea();
            _ChartArea.BackColor = Color.Transparent;

            // チャート表示エリア周囲の余白をカットする
            _ChartArea.InnerPlotPosition.Auto = true;
            _ChartArea.InnerPlotPosition.Width = 100; // 100%
            _ChartArea.InnerPlotPosition.Height = 90;  // 90%(横軸のメモリラベル印字分の余裕を設ける)
            _ChartArea.InnerPlotPosition.X = 8;
            _ChartArea.InnerPlotPosition.Y = 0;

            _ChartArea.AxisX.MinorGrid.Enabled = true;
            _ChartArea.AxisX.Interval = 60;
            _ChartArea.AxisY.Minimum = 0;  // 縦軸の最小値を0にする
            _ChartArea.AxisY.Maximum = 100; // 縦軸の最大値を100にする
            _ChartArea.AxisY.Interval = 10;

            // X,Y軸情報のセット関数を定義
            Action<Axis> setAxis = (axisInfo) =>
            {
            // 軸のメモリラベルのフォントサイズ上限値を制限
            axisInfo.LabelAutoFitMaxFontSize = 8;

            // 軸のメモリラベルの文字色をセット
            axisInfo.LabelStyle.ForeColor = Color.White;

            // 軸タイトルの文字色をセット(今回はTitle未使用なので関係ないが...)
            axisInfo.TitleForeColor = Color.White;

            // 軸の色をセット
            axisInfo.MajorGrid.Enabled = true;
                axisInfo.MajorGrid.LineColor = ColorTranslator.FromHtml("#008242");
            };

            // X,Y軸の表示方法を定義
            setAxis(_ChartArea.AxisY);
            setAxis(_ChartArea.AxisX);

            // チャートエリアを追加
            ChartAreas.Add(_ChartArea);

            Running = true;
        }
        /// <summary>
        /// 表示
        /// </summary>
        public virtual void ShowChart()
        {
        }
        /// <summary>
        /// 追加
        /// </summary>
        public virtual void Add()
        {
        }
        /// <summary>
        /// ログ出力
        /// </summary>
        /// <param name="pValueList"></param>
        public virtual void PrintLog(ArrayList pValueList)
        {
            if (!Running)
            {
                return;
            }
            if (m_PerformanceLog == null)
            {
                return;
            }
            m_PerformanceLog.Add(pValueList);
        }
        /// <summary>
        /// 削除
        /// </summary>
        public virtual void Remove()
        {
        }
        /// <summary>
        /// 実行
        /// </summary>
        public virtual void Run()
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
        /// <summary>
        /// ログ開始
        /// </summary>
        /// <param name="path"></param>
        /// <param name="basename"></param>
        /// <param name="append"></param>
        public void StartLog(String path, String basename, bool append)
        {
            // ログスレッド生成
            m_PerformanceLog = new PerformanceLog(PerformanceLog.GetInstance(path, basename, append), 100, 1000);
            m_LogThread = new Thread(m_PerformanceLog.DoWork);
            m_LogThread.Start();
        }
    }
}