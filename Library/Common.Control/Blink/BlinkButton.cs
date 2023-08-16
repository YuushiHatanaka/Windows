using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Common.Control
{
    #region イベントdelegate
    /// <summary>
    /// 開始イベントdelegate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void OnStartEventHandler(object sender, EventArgs e);

    /// <summary>
    /// 停止イベントdelegate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void OnStopEventHandler(object sender, EventArgs e);

    /// <summary>
    /// インターバルタイマーイベントdelegate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void OnIntervalimerEventHandler(object sender, EventArgs e);

    /// <summary>
    /// 点滅期間タイマーイベントdelegate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void OnBetweenTimerEventHandler(object sender, EventArgs e);
    #endregion

    /// <summary>
    /// 点滅Buttonクラス
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [Designer("System.Windows.Forms.Design.UserControlDocumentDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IRootDesigner))]
    [Designer("System.Windows.Forms.Design.ControlDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [DesignerCategory("BlinkControl")]
    public class BlinkButton : Button
    {
        #region プロパティ実態
        /// <summary>
        /// インターバルタイマー
        /// </summary>
        private Timer m_IntervalTimer = new Timer();

        /// <summary>
        /// 点滅期間タイマー
        /// </summary>
        private Timer m_BetweenTimer = new Timer();

        /// <summary>
        /// タイマーカウンタ
        /// </summary>
        private int m_TimerCount = 0;

        /// <summary>
        /// オリジナル色
        /// </summary>
        private ButtonBlinkInfomation m_OriginalInfomation = new ButtonBlinkInfomation();

        /// <summary>
        /// 点滅色リスト
        /// </summary>
        private List<ButtonBlinkInfomation> m_BlinkColors = new List<ButtonBlinkInfomation>();
        #endregion

        #region プロパティ
        /// <summary>
        /// インターバルタイマー
        /// </summary>
        [Localizable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public System.Windows.Forms.Timer IntervalTimer
        {
            get
            {
                return this.m_IntervalTimer;
            }
            set
            {
                this.m_IntervalTimer = value;
            }
        }

        /// <summary>
        /// 点滅期間タイマー
        /// </summary>
        [Localizable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public System.Windows.Forms.Timer BetweenTimer
        {
            get
            {
                return this.m_BetweenTimer;
            }
            set
            {
                this.m_BetweenTimer = value;
            }
        }

        /// <summary>
        /// タイマーカウンタ
        /// </summary>
        public int TimerCount
        {
            get
            {
                return this.m_TimerCount;
            }
            private set
            {
                this.m_TimerCount = value;
            }
        }
        #endregion

        #region イベントハンドラ
        /// <summary>
        /// 開始イベントハンドラ
        /// </summary>
        public OnStartEventHandler OnStart;

        /// <summary>
        /// 終了イベントハンドラ
        /// </summary>
        public OnStopEventHandler OnStop;

        /// <summary>
        /// インターバルタイマーイベントハンドラ
        /// </summary>
        public OnIntervalimerEventHandler OnIntervalimer;

        /// <summary>
        /// 点滅期間タイマーイベントハンドラ
        /// </summary>
        public OnBetweenTimerEventHandler OnBetweenTimer;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BlinkButton()
        {
            // 初期設定(停止時に元に戻す)
            this.m_OriginalInfomation.ForeColor = this.ForeColor;
            this.m_OriginalInfomation.BackColor = this.BackColor;
            this.m_OriginalInfomation.FlatStyle = this.FlatStyle;
            this.m_OriginalInfomation.FlatAppearance.Set(this.FlatAppearance);
            this.m_OriginalInfomation.UseVisualStyleBackColor = this.UseVisualStyleBackColor;

            // イベント登録
            this.m_IntervalTimer.Tick += this.OnIntervalimerTick;
            this.m_BetweenTimer.Tick += this.OnBetweenTimerTick;
        }
        #endregion

        #region 点滅色追加
        /// <summary>
        /// 点滅色追加
        /// </summary>
        /// <param name="foreColor"></param>
        /// <param name="backColor"></param>
        /// <param name="flatAppearance"></param>
        /// <param name="flatStyle"></param>
        /// <param name="useVisualStyleBackColor"></param>
        public void ColorAdd(Color foreColor, Color backColor, BlinkAppearance flatAppearance, FlatStyle flatStyle, bool useVisualStyleBackColor)
        {
            // リストに追加
            this.m_BlinkColors.Add(new ButtonBlinkInfomation() {
                ForeColor = foreColor,
                BackColor = backColor,
                FlatAppearance = flatAppearance,
                FlatStyle = flatStyle,
                UseVisualStyleBackColor = useVisualStyleBackColor
            });
        }

        /// <summary>
        /// 点滅色追加
        /// </summary>
        /// <param name="blinkColor"></param>
        public void ColorAdd(ButtonBlinkInfomation blinkColor)
        {
            // リストに追加
            this.m_BlinkColors.Add(blinkColor);
        }
        #endregion

        #region 開始
        /// <summary>
        /// 開始
        /// </summary>
        /// <param name="interval">点滅間隔(ミリ秒)</param>
        public void Start(int interval)
        {
            // 開始
            this.Start(interval, 0);
        }

        /// <summary>
        /// 開始
        /// </summary>
        /// <param name="interval">点滅間隔(ミリ秒)</param>
        /// <param name="between">点滅期間(秒)※0指定は無限</param>
        public void Start(int interval, int between)
        {
            // タイマー実行中か？
            if (this.m_IntervalTimer.Enabled)
            {
                // 実行中なので終了
                return;
            }

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

            // イベント呼出し
            if (this.OnStart != null)
            {
                this.OnStart(this, new EventArgs() { });
            }
        }
        #endregion

        #region 停止
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            // タイマー実行中か？
            if (!this.m_IntervalTimer.Enabled)
            {
                // 実行中でないので終了
                return;
            }

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
            this.SetStyle(this.m_OriginalInfomation);

            // イベント呼出し
            if (this.OnStop != null)
            {
                this.OnStop(this, new EventArgs() { });
            }
        }
        #endregion

        #region タイマーイベント
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
                    // 初期設定の色を設定
                    this.SetStyle(this.m_BlinkColors[0]);
                }
                else
                {
                    // 色を元に戻す
                    this.SetStyle(this.m_OriginalInfomation);
                }
            }
            else
            {
                // 点滅色インテックス設定
                int _ColorIndex = this.m_TimerCount % this.m_BlinkColors.Count;

                // 色設定
                this.SetStyle(this.m_BlinkColors[_ColorIndex]);
#if __DEBUG__
                Debug.WriteLine("--------------------------------------------------------------------------------");
                Debug.WriteLine("ForeColor                        :{0}", this.ForeColor);
                Debug.WriteLine("BackColor                        :{0}", this.BackColor);
                Debug.WriteLine("FlatAppearance.BorderColor       :{0}", this.FlatAppearance.BorderColor);
                Debug.WriteLine("FlatAppearance.BorderSize        :{0}", this.FlatAppearance.BorderSize);
                Debug.WriteLine("FlatAppearance.CheckedBackColor  :{0}", this.FlatAppearance.CheckedBackColor);
                Debug.WriteLine("FlatAppearance.MouseDownBackColor:{0}", this.FlatAppearance.MouseDownBackColor);
                Debug.WriteLine("FlatAppearance.MouseOverBackColor:{0}", this.FlatAppearance.MouseOverBackColor);
                Debug.WriteLine("FlatStyle                        :{0}", this.FlatStyle);
                Debug.WriteLine("UseVisualStyleBackColor          :{0}", this.UseVisualStyleBackColor);
#endif
            }

            // カウンタ加算
            this.m_TimerCount += 1;

            // イベント呼出し
            if (this.OnIntervalimer != null)
            {
                this.OnIntervalimer(this, new EventArgs() { });
            }
        }

        /// <summary>
        /// 点滅期間タイマーイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBetweenTimerTick(object sender, EventArgs e)
        {
            // タイマー停止
            this.Stop();

            // イベント呼出し
            if (this.OnBetweenTimer != null)
            {
                this.OnBetweenTimer(this, new EventArgs() { });
            }
        }
        #endregion

        #region スタイル設定
        /// <summary>
        /// スタイル設定
        /// </summary>
        /// <param name="style"></param>
        private void SetStyle(ButtonBlinkInfomation style)
        {
            this.ForeColor = style.ForeColor;
            this.BackColor = style.BackColor;
            this.FlatAppearance.BorderColor = style.FlatAppearance.BorderColor;
            this.FlatAppearance.BorderSize = style.FlatAppearance.BorderSize;
            this.FlatAppearance.CheckedBackColor = style.FlatAppearance.CheckedBackColor;
            this.FlatAppearance.MouseDownBackColor = style.FlatAppearance.MouseDownBackColor;
            this.FlatAppearance.MouseOverBackColor = style.FlatAppearance.MouseOverBackColor;
            this.FlatStyle = style.FlatStyle;
            this.UseVisualStyleBackColor = style.UseVisualStyleBackColor;
        }
        #endregion
    }
}
