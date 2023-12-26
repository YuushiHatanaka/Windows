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
using System.Xml.Linq;

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

        /// <summary>
        /// 線スタイル
        /// </summary>
        public DashStyle DashStyle { get; set; } = DashStyle.Solid;

        /// <summary>
        /// ボールドフラグ
        /// </summary>
        public bool Bold { get; set; } = false;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PanelLine()
        {
            // ロギング
            Logger.Debug("=>>>> PanelLine::PanelLine()");

            // 設定
            Paint += PanelLine_Paint;

            // ロギング
            Logger.Debug("<<<<= PanelLine::PanelLine()");
        }
        #endregion

        #region イベント
        /// <summary>
        /// PanelLine_Paint
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelLine_Paint(object sender, PaintEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> PanelLine::PanelLine_Paint(object, PaintEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

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

            // ロギング
            Logger.Debug("<<<<= PanelLine::PanelLine_Paint(object, PaintEventArgs)");
        }
        #endregion

        #region publicメソッド
        /// <summary>
        /// 線色取得
        /// </summary>
        /// <returns></returns>
        public Color GetColor()
        {
            // ロギング
            Logger.Debug("=>>>> PanelLine::GetColor()");

            // 結果設定
            Color result = ForeColor;

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= PanelLine::GetColor()");

            // 返却
            return result;
        }

        /// <summary>
        /// 線色設定
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            // ロギング
            Logger.Debug("=>>>> PanelLine::SetColor(Color)");
            Logger.DebugFormat("color:[{0}]", color);

            // 設定
            ForeColor = color;

            // ロギング
            Logger.Debug("<<<<= PanelLine::SetColor(Color)");
        }
        #endregion
    }
}
