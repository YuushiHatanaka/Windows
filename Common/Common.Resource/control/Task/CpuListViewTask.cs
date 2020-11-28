using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Resource;

namespace Common.Resource.Task
{
    public class CpuListViewTask : CpuListView
    {
        /// <summary>
        /// 実行タイマー
        /// </summary>
        private System.Windows.Forms.Timer m_ExecuteTimer = new System.Windows.Forms.Timer();

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

        /// コンストラクタ
        /// </summary>
        public CpuListViewTask()
            : base()
        {
            // タイマー設定(1秒)
            this.m_ExecuteTimer.Interval = 1000;
            this.m_ExecuteTimer.Tick += ExecuteTimer_Tick;
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExecuteTimer_Tick(object sender, EventArgs e)
        {
            // リスト更新
            this.UpdateList();
        }
    }
}
