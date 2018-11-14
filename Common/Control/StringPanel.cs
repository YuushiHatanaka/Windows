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

namespace Common.Control
{
    public partial class StringPanel : UserControl
    {
        #region 自動サイズ調整
        /// <summary>
        /// 自動サイズ調整
        /// </summary>
        private bool m_AutoFontSize = false;

        /// <summary>
        /// 自動サイズ調整
        /// </summary>
        public bool AutoFontSize
        {
            get
            {
                return this.m_AutoFontSize;
            }
            set
            {
                this.m_AutoFontSize = value;
                this.Refresh();
            }
        }
        #endregion

        #region Text表示位置
        /// <summary>
        /// Text表示位置
        /// </summary>
        private ContentAlignment m_TextAlign = ContentAlignment.TopLeft;

        /// <summary>
        /// Text表示位置
        /// </summary>
        public ContentAlignment TextAlign
        {
            get
            {
                return this.m_TextAlign;
            }
            set
            {
                this.m_TextAlign = value;
                this.Refresh();
            }
        }
        #endregion

        #region 表示文字列
        /// <summary>
        /// 表示文字列
        /// </summary>
        private string m_Value;

        /// <summary>
        /// 表示文字列
        /// </summary>
        public string Value
        {
            get
            {
                return this.m_Value;
            }
            set
            {
                this.m_Value = value;
                this.Refresh();
            }
        }
        #endregion

        #region 境界線
        #region 境界線描画
        /// <summary>
        /// 境界線描画
        /// </summary>
        private bool m_BorderDrawing = false;

        /// <summary>
        /// 境界線描画
        /// </summary>
        public bool BorderDrawing
        {
            get
            {
                return this.m_BorderDrawing;
            }
            set
            {
                this.m_BorderDrawing = value;
                this.Refresh();
            }
        }
        #endregion

        #region 境界線色
        /// <summary>
        /// 境界線色
        /// </summary>
        private Color m_BorderColor = Color.Black;

        /// <summary>
        /// 境界線色
        /// </summary>
        public Color BorderColor
        {
            get
            {
                return this.m_BorderColor;
            }
            set
            {
                this.m_BorderColor = value;
                this.Refresh();
            }
        }
        #endregion

        #region 境界線色
        /// <summary>
        /// 境界線サイズ
        /// </summary>
        private float m_BorderSize = 1;

        /// <summary>
        /// 境界線サイズ
        /// </summary>
        public float BorderSize
        {
            get
            {
                return this.m_BorderSize;
            }
            set
            {
                this.m_BorderSize = value;
                this.Refresh();
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StringPanel()
        {
            // コンポーネント初期化
            InitializeComponent();

            // 初期化
            this.Initialization();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            // 再描画
            this.Refresh();
        }

        /// <summary>
        /// OnPaint
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="pevent"></param>
        private void OnPaint(object sender, PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            Font realFont = this.Font;

            // 境界線描画
            if (this.m_BorderDrawing)
            {
                this.DrawBorder(g, pevent.ClipRectangle, this.m_BorderColor, this.m_BorderSize);
            }

            // 文字列表示位置取得
            TextFormatFlags flags = this.GetTextAlignment();

            // フォントサイズ決定
            if (this.m_AutoFontSize)
            {
                SizeF size = this.DeterminationFontSize(this.m_Value, g, this.Width, this.Height, ref realFont);
            }

            // テキスト描画
            TextRenderer.DrawText(g, this.m_Value, realFont, new Rectangle(0, 0, this.Width, this.Height), this.ForeColor, flags);
        }

        /// <summary>
        /// 境界線描画
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="rectangle"></param>
        /// <param name="drawingColor"></param>
        /// <param name="width"></param>
        private void DrawBorder(Graphics graphics, Rectangle rectangle, Color drawingColor, float width)
        {
            // Penオブジェクトの作成
            Pen drawingPen = new Pen(drawingColor, width);

            // 長方形を描く
            rectangle.Width = rectangle.Width - 1;
            rectangle.Height = rectangle.Height - 1;
            graphics.DrawRectangle(drawingPen, rectangle);

            //リソースを解放する
            drawingPen.Dispose();
        }

        /// <summary>
        /// フォントサイズ決定
        /// </summary>
        /// <param name="text"></param>
        /// <param name="graphics"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="drawFont"></param>
        /// <returns></returns>
        private SizeF DeterminationFontSize(string text, Graphics graphics, int width, int height, ref Font drawFont)
        {
            // テキスト描画のサイズを取得
            SizeF _SizeF = graphics.MeasureString(text, drawFont);

            // サイズにより分岐
            if (_SizeF.Width >= width || _SizeF.Height >= height)
            {
                while (_SizeF.Width >= width || _SizeF.Height >= height)
                {
                    if (drawFont.Size <= 1)
                        break;
                    FontStyle _FontStype = this.GetFontStyle(drawFont);
                    drawFont = new Font(drawFont.FontFamily, drawFont.Size - 1, _FontStype);
                    _SizeF = graphics.MeasureString(text, drawFont);
                }
            }
            else
            {
                while (_SizeF.Width < width && _SizeF.Height <= height)
                {
                    FontStyle _FontStype = this.GetFontStyle(drawFont);
                    drawFont = new Font(drawFont.FontFamily, drawFont.Size + 1, _FontStype);
                    _SizeF = graphics.MeasureString(text, drawFont);
                }
            }

            // テキスト描画のサイズを返却
            return _SizeF;
        }

        /// <summary>
        /// フォントスタイル取得
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        private FontStyle GetFontStyle(Font font)
        {
            FontStyle _FontStyle = FontStyle.Regular;   // フォントスタイル

            // Bold判定
            if (font.Bold)
            {
                _FontStyle |= FontStyle.Bold;
            }
            // Italic判定
            if (font.Italic)
            {
                _FontStyle |= FontStyle.Italic;
            }
            // Underline判定
            if (font.Underline)
            {
                _FontStyle |= FontStyle.Underline;
            }
            // Underline判定
            if (font.Strikeout)
            {
                _FontStyle |= FontStyle.Strikeout;
            }

            // フォントスタイルを返却
            return _FontStyle;
        }

        /// <summary>
        /// 文字列表示位置取得
        /// </summary>
        /// <returns></returns>
        private TextFormatFlags GetTextAlignment()
        {
            // 表示位置設定
            switch (this.m_TextAlign)
            {
                case ContentAlignment.TopLeft:      // 上端左寄せ
                    return TextFormatFlags.Left | TextFormatFlags.Top;
                case ContentAlignment.TopCenter:    // 上端中央
                    return TextFormatFlags.HorizontalCenter | TextFormatFlags.Top;
                case ContentAlignment.TopRight:     // 上端右寄せ
                    return TextFormatFlags.Right | TextFormatFlags.Top;
                case ContentAlignment.MiddleLeft:   // 中段左寄せ
                    return TextFormatFlags.Left | TextFormatFlags.VerticalCenter;
                case ContentAlignment.MiddleCenter: // 中段中央
                    return TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                case ContentAlignment.MiddleRight:  // 中段右寄せ
                    return TextFormatFlags.Right | TextFormatFlags.VerticalCenter;
                case ContentAlignment.BottomLeft:   // 下段左寄せ
                    return TextFormatFlags.Left | TextFormatFlags.Bottom;
                case ContentAlignment.BottomCenter: // 下段中央
                    return TextFormatFlags.HorizontalCenter | TextFormatFlags.Bottom;
                case ContentAlignment.BottomRight:  // 下段右寄せ
                    return TextFormatFlags.Right | TextFormatFlags.Bottom;
                default:
                    return TextFormatFlags.Default;
            }
        }
    }
}
