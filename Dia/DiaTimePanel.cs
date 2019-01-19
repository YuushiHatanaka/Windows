using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using Common;

namespace Dia.Control
{
    public class DiaTimePanel : Panel
    {
        #region プライベートメソッド
        /// <summary>
        /// 時間
        /// </summary>
        private DateTime m_Time = new DateTime();
        
        /// <summary>
        /// 時間フォーマット
        /// </summary>
        private string m_TimeFormat = "HH:mm";

        /// <summary>
        /// 無印フォーマット
        /// </summary>
        private string m_NonFormat = "‥";

        /// <summary>
        /// 種別
        /// </summary>
        private DiaPanelType m_Type = DiaPanelType.Non;
        #endregion





        /// <summary>
        /// 時間
        /// </summary>
        [Localizable(true)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DateTime Time
        {
            get
            {
                return this.m_Time;
            }
            set
            {
                this.m_Time = value;
                this.UpdatePaint();
            }
        }

        /// <summary>
        /// 時間フォーマット
        /// </summary>
        [Localizable(true)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string TimeFormat
        {
            get
            {
                return this.m_TimeFormat;
            }
            set
            {
                this.m_TimeFormat = value;
                this.UpdatePaint();
            }
        }

        /// <summary>
        /// 無印フォーマット
        /// </summary>
        [Localizable(true)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string NonFormat
        {
            get
            {
                return this.m_NonFormat;
            }
            set
            {
                this.m_NonFormat = value;
                this.UpdatePaint();
            }
        }

        /// <summary>
        /// 種別
        /// </summary>
        [Localizable(true)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DiaPanelType Type
        {
            get
            {
                return this.m_Type;
            }
            set
            {
                this.m_Type = value;
                this.UpdatePaint();
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DiaTimePanel()
            : base()
        {
            // フォントオブジェクトの作成
            //this.Font = new Font("MS UI Gothic", 20);

            // イベント登録
            this.Paint += this.OnPaint;
            this.FontChanged += OnFontChanged;
        }

        private void OnFontChanged(object sender, EventArgs e)
        {
            this.UpdatePaint();
        }

        private void UpdatePaint()
        {
            Graphics graphics = this.CreateGraphics();
            Rectangle rectangle = this.ClientRectangle;

            this.UpdatePaint(graphics, rectangle);
        }

        private void UpdateString(Graphics graphics, Rectangle rectangle, string value)
        {
            this.SuspendLayout();

            StringFormat stringFormat = new StringFormat();
            stringFormat.LineAlignment = StringAlignment.Center;
            stringFormat.Alignment = StringAlignment.Center;

            graphics.DrawString(
                value,
                this.Font,
                Brushes.Black,
                rectangle,
                stringFormat);

            this.ResumeLayout();
        }

        private void UpdatePassingThrough(Graphics graphics, Rectangle rectangle)
        {
            // TODO:未実装
        }
        private void UpdateViaAnotherLineSection(Graphics graphics, Rectangle rectangle)
        {
            graphics.DrawLine(Pens.Black, (rectangle.Width / 2) - 1, rectangle.Top + 1, (rectangle.Width / 2) - 1, rectangle.Bottom - 1);
            graphics.DrawLine(Pens.Black, (rectangle.Width / 2) + 1, rectangle.Top + 1, (rectangle.Width / 2) + 1, rectangle.Bottom - 1);
        }
        private void UpdateThisStationStppage(Graphics graphics, Rectangle rectangle)
        {
            graphics.DrawLine(Pens.Black, rectangle.Left, rectangle.Bottom - 3, rectangle.Right, rectangle.Bottom - 3);
            graphics.DrawLine(Pens.Black, rectangle.Left, rectangle.Bottom - 1, rectangle.Right, rectangle.Bottom - 1);
        }

        private void UpdatePaint(Graphics graphics, Rectangle rectangle)
        {
            // なし
            if (this.m_Type == DiaPanelType.Non)
            {
                // 更新
                this.UpdateString(graphics, rectangle, this.m_NonFormat);
                return;
            }
            // 通常
            else if (this.m_Type.HasFlag(DiaPanelType.Normal))
            {
                // 時刻を表示
                this.UpdateString(graphics, rectangle, this.m_Time.ToString(this.m_TimeFormat));
            }
            // 通過
            else if (this.m_Type.HasFlag(DiaPanelType.PassingThrough))
            {
                // 更新
                this.UpdatePassingThrough(graphics, rectangle);
                return;
            }
            // 他線区経由
            else if (this.m_Type.HasFlag(DiaPanelType.ViaAnotherLineSection))
            {
                // 更新
                this.UpdateViaAnotherLineSection(graphics, rectangle);
                return;
            }
            // この駅止まり
            else if (this.m_Type.HasFlag(DiaPanelType.ThisStationStppage))
            {
                // 時刻を表示
                this.UpdateString(graphics, rectangle, this.m_Time.ToString(this.m_TimeFormat));

                // 更新
                this.UpdateThisStationStppage(graphics, rectangle);
            }
        }

        public virtual void OnPaint(object sender, PaintEventArgs e)
        {
            this.UpdatePaint(e.Graphics, e.ClipRectangle);
        }
    }
}
