using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// PanelLineクラス
    /// </summary>
    public class PanelLine : Panel
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public DashStyle DashStyle { get; set; }

        public bool Bold { get; set; }

        public PanelLine()
        {
            Paint += PanelLine_Paint;
        }

        private void PanelLine_Paint(object sender, PaintEventArgs e)
        {
            // 背景を塗りつぶす色を指定
            e.Graphics.FillRectangle(Brushes.White, e.ClipRectangle);

            // 太線判定
            float width = 1;
            if (Bold)
            {
                width = 2;
            }

            // 線を描画するためのペンを作成
            using (Pen pen = new Pen(ForeColor, width))
            {
                // 線スタイルを設定
                pen.DashStyle = DashStyle;

                // 描画
                e.Graphics.DrawLine(
                    pen,
                    e.ClipRectangle.Right - e.ClipRectangle.Width + 8,
                    e.ClipRectangle.Top + (e.ClipRectangle.Height / 2),
                    e.ClipRectangle.Right - 8,
                    e.ClipRectangle.Top + (e.ClipRectangle.Height / 2));
            }

            // 四角形を描画するためのペンを作成
            using (Pen pen = new Pen(Brushes.Black))
            {
                // 四角形を描画
                e.Graphics.DrawRectangle(pen, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
            }
        }

        public Color GetColor()
        {
            return ForeColor;
        }

        public void SetColor(Color color)
        {
            ForeColor = color;
        }
    }
}
