using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Common.Control
{
    /// <summary>
    /// GridTableLayoutPanelクラス
    /// </summary>
    public class GridTableLayoutPanel : TableLayoutPanel
    {
        /// <summary>
        /// 境界線色
        /// </summary>
        public Color BorderColor = Color.Black;

        /// <summary>
        /// 境界線サイズ
        /// </summary>
        public int LineWidth = 1;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GridTableLayoutPanel()
            : base()
        {
        }

        #region 境界線設定
        /// <summary>
        /// 境界線設定
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        public void SetBorder(int column, int row)
        {
            // 境界線設定
            SetBorder(column, row, BorderColor);
        }

        /// <summary>
        /// 境界線設定
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="borderColor"></param>
        public void SetBorder(int column, int row, Color borderColor)
        {
            // 境界線設定
            SetBorder(column, row, borderColor, LineWidth);
        }

        /// <summary>
        /// 境界線設定
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="borderColor"></param>
        /// <param name="lineWidth"></param>
        public void SetBorder(int column, int row, Color borderColor, int lineWidth)
        {
            // 境界線設定
            SetBorder(column, 1, row, 1, borderColor, lineWidth);
        }

        /// <summary>
        /// 境界線設定
        /// </summary>
        /// <param name="column"></param>
        /// <param name="columSpan"></param>
        /// <param name="row"></param>
        /// <param name="rowSpan"></param>
        public void SetBorder(int column, int columSpan, int row, int rowSpan)
        {
            // 境界線設定
            SetBorder(column, columSpan, row, rowSpan, BorderColor, LineWidth);
        }

        /// <summary>
        /// 境界線設定
        /// </summary>
        /// <param name="column"></param>
        /// <param name="columSpan"></param>
        /// <param name="row"></param>
        /// <param name="rowSpan"></param>
        /// <param name="borderColor"></param>
        public void SetBorder(int column, int columSpan, int row, int rowSpan, Color borderColor)
        {
            // 境界線設定
            SetBorder(column, columSpan, row, rowSpan, borderColor, LineWidth);
        }

        /// <summary>
        /// 境界線設定
        /// </summary>
        /// <param name="column"></param>
        /// <param name="columSpan"></param>
        /// <param name="row"></param>
        /// <param name="rowSpan"></param>
        /// <param name="borderColor"></param>
        /// <param name="lineWidth"></param>
        public void SetBorder(int column, int columSpan, int row, int rowSpan, Color borderColor, int lineWidth)
        {
            // 設定
            BorderColor = borderColor;
            LineWidth = lineWidth;

            CellPaint += (sender, e) =>
            {
                int endRow = row + rowSpan - 1;
                int endColumn = column + columSpan - 1;

                if (e.Row >= row && e.Row <= endRow && e.Column >= column && e.Column <= endColumn)
                {
                    using (Pen pen = new Pen(borderColor, lineWidth))
                    {
                        // 上辺を描画
                        if (e.Row == row)
                        {
                            e.Graphics.DrawLine(pen, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Right, e.CellBounds.Top);
                        }

                        // 下辺を描画
                        if (e.Row == endRow)
                        {
                            e.Graphics.DrawLine(pen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
                        }

                        // 左辺を描画
                        if (e.Column == column)
                        {
                            e.Graphics.DrawLine(pen, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
                        }

                        // 右辺を描画
                        if (e.Column == endColumn)
                        {
                            e.Graphics.DrawLine(pen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);
                        }
                    }
                }
            };
        }
        #endregion
    }
}
