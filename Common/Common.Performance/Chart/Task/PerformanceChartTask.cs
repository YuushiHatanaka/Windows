using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.Collections;

namespace Common.Performance.Task
{
    /// <summary>
    /// パフォーマンスチャートTaskクラス
    /// </summary>
    public class PerformanceChartTask : Chart, IDisposable
    {
        /// <summary>
        /// Disposed
        /// </summary>
        private bool m_Disposed = false;

        /// <summary>
        /// 実行タイマー
        /// </summary>
        private System.Windows.Forms.Timer m_ExecuteTimer = new System.Windows.Forms.Timer();

        /// <summary>
        /// ログ出力スレッド
        /// </summary>
        private Thread m_LogThread = null;

        /// <summary>
        /// ログ出力オブジェクト
        /// </summary>
        private PerformanceLog m_PerformanceLog = null;

        /// <summary>
        /// インターバルタイマー
        /// </summary>
        public int Interval
        {
            set
            {
                this.m_ExecuteTimer.Interval = value;
            }
            get
            {
                return this.m_ExecuteTimer.Interval;
            }
        }

        /// <summary>
        /// 実行中フラグ
        /// </summary>
        public bool Running
        {
            set
            {
                this.m_ExecuteTimer.Enabled = value;
            }
            get
            {
                return this.m_ExecuteTimer.Enabled;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PerformanceChartTask()
        {
            // 初期化
            this.Initialization();
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~PerformanceChartTask()
        {
            // マネージドオブジェクトは破棄しない
            this.Dispose(false);
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
            this.ChartAreas.Add(_ChartArea);

            // タイマー設定(1秒)
            this.m_ExecuteTimer.Interval = 1000;
            this.m_ExecuteTimer.Tick += ExecuteTimer_Tick;
        }

        #region Dispose
        /// <summary>
        /// Dispose
        /// </summary>
        public new void Dispose()
        {
            // マネージドオブジェクト破棄
            this.Dispose(true);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected new void Dispose(bool disposing)
        {
            // 親オブジェクトDispose
            base.Dispose(disposing);

            // 破棄済み？
            if (!this.m_Disposed)
            {
                // マネージドオブジェクト破棄するか？
                if (disposing)
                {
                    // 破棄
                    this.Destroy();
                }

                // 破棄済み設定
                this.m_Disposed = true;
            }
        }
        #endregion

        /// <summary>
        /// 破棄
        /// </summary>
        private void Destroy()
        {
            // タイマー破棄
            if (this.m_ExecuteTimer != null)
            {
                this.m_ExecuteTimer.Dispose();
                this.m_ExecuteTimer = null;
            }

            // ログ出力オブジェクト破棄
            if (this.m_PerformanceLog != null)
            {
                this.m_PerformanceLog.Stop();
                this.m_PerformanceLog = null;
            }
        }

        /// <summary>
        /// 追加
        /// </summary>
        public virtual void Add()
        {
        }

        /// <summary>
        /// 削除
        /// </summary>
        public virtual void Remove()
        {
        }

        /// <summary>
        /// 表示
        /// </summary>
        public virtual void ShowChart()
        {
        }

        /// <summary>
        /// 実行
        /// </summary>
        public virtual void Run()
        {
            // 追加
            this.Add();

            // 削除
            this.Remove();

            // 表示
            this.ShowChart();
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExecuteTimer_Tick(object sender, EventArgs e)
        {
            // 実行
            this.Run();
        }

        /// <summary>
        /// ログ開始
        /// </summary>
        /// <param name="path"></param>
        /// <param name="basename"></param>
        /// <param name="append"></param>
        public void StartLog(string path, string basename, bool append)
        {
            // ログスレッド生成
            this.m_PerformanceLog = new PerformanceLog(PerformanceLog.GetInstance(path, basename, append), 100, 1000);

            // ログスレッド開始
            this.m_LogThread = new Thread(m_PerformanceLog.DoWork);
            this.m_LogThread.Start();
        }

        /// <summary>
        /// ログ出力
        /// </summary>
        /// <param name="pValueList"></param>
        public virtual void PrintLog(ArrayList pValueList)
        {
            // 実行中か？
            if (!this.Running)
            {
                return;
            }

            // ログ出力オブジェクトが未生成か？
            if (this.m_PerformanceLog == null)
            {
                return;
            }

            // ログ登録
            this.m_PerformanceLog.Add(pValueList);
        }
    }
}
