﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace Common.Control
{
    /// <summary>
    /// 点滅ListViewItemクラス
    /// </summary>
    public class BlinkListViewItem : ListViewItem
    {
        /// <summary>
        /// インターバルタイマー
        /// </summary>
        private Timer m_IntervalTimer = new Timer();

        /// <summary>
        /// 点滅期間タイマー
        /// </summary>
        private Timer m_BetweenTimer = new Timer();

        /// <summary>
        /// タイマー間隔
        /// </summary>
        public int Interval
        {
            get
            {
                return this.m_IntervalTimer.Interval;
            }
            set
            {
                this.m_IntervalTimer.Interval = value;
            }
        }

        /// <summary>
        /// タイマーカウンタ
        /// </summary>
        private int m_TimerCount = 0;

        /// <summary>
        /// オリジナル色
        /// </summary>
        private BlinkInfomation m_OriginalInfomation = new BlinkInfomation();

        /// <summary>
        /// 点滅色リスト
        /// </summary>
        private List<BlinkInfomation> m_BlinkColors = new List<BlinkInfomation>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BlinkListViewItem()
            : base()
        {
            // 初期設定(停止時に元に戻す)
            this.m_OriginalInfomation.ForeColor = this.ForeColor;
            this.m_OriginalInfomation.BackColor = this.BackColor;

            // イベント登録
            this.m_IntervalTimer.Tick += this.OnIntervalimerTick;
            this.m_BetweenTimer.Tick += this.OnBetweenTimerTick;
        }

        /// <summary>
        /// 点滅色追加
        /// </summary>
        /// <param name="foreColor"></param>
        /// <param name="backColor"></param>
        public void BlinkColorAdd(Color foreColor, Color backColor)
        {
            // リストに追加
            this.m_BlinkColors.Add(new BlinkInfomation()
            {
                ForeColor = foreColor,
                BackColor = backColor
            });
        }
        /// <summary>
        /// 点滅色追加
        /// </summary>
        /// <param name="blinkColor"></param>
        public void BlinkColorAdd(BlinkInfomation blinkColor)
        {
            this.m_BlinkColors.Add(blinkColor);
        }

        /// <summary>
        /// 点滅開始
        /// </summary>
        /// <param name="interval">点滅間隔(ミリ秒)</param>
        public void BlinkStart(int interval)
        {
            /// 点滅開始
            this.BlinkStart(interval, 0);
        }

        /// <summary>
        /// 点滅開始
        /// </summary>
        /// <param name="interval">点滅間隔(ミリ秒)</param>
        /// <param name="between">点滅期間(秒)※0指定は無限</param>
        public void BlinkStart(int interval, int between)
        {
            // タイマー実行中か？
            if (this.m_IntervalTimer.Enabled)
            {
                // 実行中なので終了
                return;
            }

            Debug.WriteLine("タイマー開始：{0}({1}ミリ秒間隔:{2}秒間)", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), interval, between);

            // 点滅色に何も登録がない場合
            if (this.m_BlinkColors.Count <= 0)
            {
                // 点滅させない
                return;
            }

            // タイマー開始
            this.m_IntervalTimer.Interval = interval;
            this.m_IntervalTimer.Start();

            // 点滅期間を設定
            if (between > 0)
            {
                this.m_BetweenTimer.Interval = between * 1000;
                this.m_BetweenTimer.Start();
            }
        }

        /// <summary>
        /// 点滅停止
        /// </summary>
        public void BlinkStop()
        {
            // タイマー実行中か？
            if (!this.m_IntervalTimer.Enabled)
            {
                // 実行中でないので終了
                return;
            }

            Debug.WriteLine("タイマー停止：" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));

            // 期間タイマー実行中か？
            if (this.m_BetweenTimer.Enabled)
            {
                // タイマー停止
                this.m_BetweenTimer.Stop();
            }

            // タイマー停止
            this.m_IntervalTimer.Stop();
            this.m_TimerCount = 0;

            // 色を元に戻す
            this.ForeColor = this.m_OriginalInfomation.ForeColor;
            this.BackColor = this.m_OriginalInfomation.BackColor;
        }

        /// <summary>
        /// 点滅タイマーイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIntervalimerTick(object sender, EventArgs e)
        {
            // 登録色が1色の場合
            if (this.m_BlinkColors.Count == 1)
            {
                // 点滅を元の色と繰り返す
                if (this.m_TimerCount % 2 == 0)
                {
                    this.ForeColor = this.m_BlinkColors[0].ForeColor;
                    this.BackColor = this.m_BlinkColors[0].BackColor;
                }
                else
                {
                    // 色を元に戻す
                    this.ForeColor = this.m_OriginalInfomation.ForeColor;
                    this.BackColor = this.m_OriginalInfomation.BackColor;
                }
            }
            else
            {
                // 点滅色インテックス設定
                int _ColorIndex = this.m_TimerCount % this.m_BlinkColors.Count;

                // 色設定
                this.ForeColor = this.m_BlinkColors[_ColorIndex].ForeColor;
                this.BackColor = this.m_BlinkColors[_ColorIndex].BackColor;
#if DEBUG
                Debug.WriteLine("--------------------------------------------------------------------------------");
                Debug.WriteLine("ForeColor                        :{0}", this.ForeColor);
                Debug.WriteLine("BackColor                        :{0}", this.BackColor);
#endif
            }

            // カウンタ加算
            this.m_TimerCount += 1;
        }

        /// <summary>
        /// 点滅期間タイマーイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBetweenTimerTick(object sender, EventArgs e)
        {
            Debug.WriteLine("点滅期間満了：" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));

            // タイマー停止
            this.BlinkStop();
        }
    }
}
