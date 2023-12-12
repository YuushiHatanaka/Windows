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
using TrainTimeTable.Common;
using TrainTimeTable.Property;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// PictureBoxHoursクラス
    /// </summary>
    public class PictureBoxHours : PictureBoxDiagramCore
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="property"></param>
        public PictureBoxHours(FormRoute owner, RouteFileProperty property)
            : base(owner, property)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxHours::PictureBoxHours(FormRoute, RouteFileProperty)");
            Logger.DebugFormat("owner   :[{0}]", owner);
            Logger.DebugFormat("property:[{0}]", property);

            // ロギング
            Logger.Debug("<<<<= PictureBoxHours::PictureBoxHours(FormRoute, RouteFileProperty)");
        }
        #endregion

        #region 描画更新
        /// <summary>
        /// 描画更新
        /// </summary>
        /// <param name="property"></param>
        public override void UpdateDraw(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxHours::UpdateDraw(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 初期設定
            Property = property;
            Image = null;
            BackColor = SystemColors.Window;
            Width = GetTotalWidth();
            Height = 40;

            // DiagramDrawLibraryオブジェクト生成
            using (DiagramDrawLibrary drawLibrary = new DiagramDrawLibrary(Width, WidthPercent, Height, HeightPercent))
            {
                // 初期化
                drawLibrary.Initialization(property);

                // 全体塗りつぶし
                drawLibrary.DrawFillInTheWhole();

                // 大外枠描画
                drawLibrary.DrawLargeOuterFrame();

                // 時グリッド描画
                drawLibrary.DrawHoursGrids(Font, Brushes.Black);

                // イメージ設定
                Image = drawLibrary.Bitmap;
            }

            // ロギング
            Logger.Debug("<<<<= PictureBoxHours::UpdateDraw(RouteFileProperty)");
        }

        /// <summary>
        /// 描画更新
        /// </summary>
        /// <param name="property"></param>
        /// <param name="widthPercent"></param>
        /// <param name="heightPercent"></param>
        public void UpdateDraw(RouteFileProperty property, float widthPercent, float heightPercent)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxHours::UpdateDraw(RouteFileProperty, float, float)");
            Logger.DebugFormat("property:[{0}]", property);

            // 設定
            WidthPercent = widthPercent;
            HeightPercent = heightPercent;

            // 更新
            UpdateDraw(property);

            // ロギング
            Logger.Debug("<<<<= PictureBoxHours::UpdateDraw(RouteFileProperty, float, float)");
        }

        #endregion
    }
}
