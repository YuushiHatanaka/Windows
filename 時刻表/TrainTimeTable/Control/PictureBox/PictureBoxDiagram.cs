using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using TrainTimeTable.Common;
using TrainTimeTable.Property;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// PictureBoxStationsクラス
    /// </summary>
    public class PictureBoxDiagram : PictureBoxDiagramCore
    {
        /// <summary>
        /// 列車番号表示
        /// </summary>
        public bool TrainNumberDisplay { get; set; } = true;

        /// <summary>
        /// 列車名表示
        /// </summary>
        public bool TrainNameDisplay { get; set; } = true;

        /// <summary>
        /// 列車時刻表示
        /// </summary>
        public bool TrainTimeDisplay { get; set; } = false;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="daiagramName"></param>
        /// <param name="property"></param>
        public PictureBoxDiagram(FormRoute owner, string daiagramName, RouteFileProperty property)
            : base(owner, property)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagram::PictureBoxDiagram(FormRoute, string, RouteFileProperty)");
            Logger.DebugFormat("owner       :[{0}]", owner);
            Logger.DebugFormat("daiagramName:[{0}]", daiagramName);
            Logger.DebugFormat("property    :[{0}]", property);

            // 設定
            Text = daiagramName;

            // ロギング
            Logger.Debug("<<<<= PictureBoxDiagram::PictureBoxDiagram(FormRoute, string, RouteFileProperty)");
        }
        #endregion

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="property"></param>
        private void Initialization(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagram::Initialization(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 初期設定
            Property = property;
            Image = null;
            BackColor = SystemColors.Window;
            Width = GetTotalWidth();
            Height = GetTotalHeight();

            // ロギング
            Logger.Debug("<<<<= PictureBoxDiagram::Initialization(RouteFileProperty)");
        }

        #region 描画更新
        /// <summary>
        /// 描画更新
        /// </summary>
        /// <param name="property"></param>
        /// <param name="trainNumberDisplay"></param>
        /// <param name="trainNameDisplay"></param>
        /// <param name="trainTimeDisplay"></param>
        public void UpdateDraw(RouteFileProperty property, bool trainNumberDisplay, bool trainNameDisplay, bool trainTimeDisplay)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagram::UpdateDraw(RouteFileProperty, bool, bool, bool)");
            Logger.DebugFormat("trainNumberDisplay:[{0}]", trainNumberDisplay);
            Logger.DebugFormat("trainNameDisplay  :[{0}]", trainNameDisplay);
            Logger.DebugFormat("trainTimeDisplay  :[{0}]", trainTimeDisplay);

            // 設定
            TrainNumberDisplay = trainNumberDisplay;
            TrainNameDisplay = trainNameDisplay;
            TrainTimeDisplay = trainTimeDisplay;

            // 初期設定
            Initialization(property);

            // DiagramDrawLibraryオブジェクト生成
            using (DiagramDrawLibrary drawLibrary = new DiagramDrawLibrary(Width, WidthPercent, Height, HeightPercent))
            {
                // 表示設定
                drawLibrary.TrainNumberDisplay = TrainNumberDisplay;
                drawLibrary.TrainNameDisplay = TrainNameDisplay;
                drawLibrary.TrainTimeDisplay = TrainTimeDisplay;

                // 描画更新
                UpdateDraw(drawLibrary);
            }

            // ロギング
            Logger.Debug("<<<<= PictureBoxDiagram::UpdateDraw(RouteFileProperty, bool, bool, bool)");
        }

        /// <summary>
        /// 描画更新
        /// </summary>
        /// <param name="property"></param>
        public override void UpdateDraw(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagram::UpdateDraw(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 初期設定
            Initialization(property);

            // DiagramDrawLibraryオブジェクト生成
            using (DiagramDrawLibrary drawLibrary = new DiagramDrawLibrary(Width, WidthPercent, Height, HeightPercent))
            {
                // 表示設定
                drawLibrary.TrainNumberDisplay = TrainNumberDisplay;
                drawLibrary.TrainNameDisplay = TrainNameDisplay;
                drawLibrary.TrainTimeDisplay = TrainTimeDisplay;

                // 描画更新
                UpdateDraw(drawLibrary);
            }

            // ロギング
            Logger.Debug("<<<<= PictureBoxDiagram::UpdateDraw(RouteFileProperty)");
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
            Logger.Debug("=>>>> PictureBoxDiagram::UpdateDraw(RouteFileProperty, float, float)");
            Logger.DebugFormat("property:[{0}]", property);

            // 設定
            WidthPercent = widthPercent;
            HeightPercent = heightPercent;

            // 更新
            UpdateDraw(property);

            // ロギング
            Logger.Debug("<<<<= PictureBoxDiagram::UpdateDraw(RouteFileProperty, float, float)");
        }

        /// <summary>
        /// 描画更新
        /// </summary>
        /// <param name="drawLibrary"></param>
        private void UpdateDraw(DiagramDrawLibrary drawLibrary)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagram::UpdateDraw(DiagramDrawLibrary)");
            Logger.DebugFormat("library:[{0}]", drawLibrary);

            // 初期化
            drawLibrary.Initialization(Property);

            // 全体塗りつぶし
            drawLibrary.DrawFillInTheWhole();

            // 大外枠描画
            drawLibrary.DrawLargeOuterFrame();

            // 時グリッド描画
            drawLibrary.DrawHoursGrids();

            // 駅グリッド描画
            drawLibrary.DrawStationsGrids();

            // 列車描画
            drawLibrary.DrawTrainsLine(Text);

            // イメージ設定
            Image = drawLibrary.Bitmap;

            // ロギング
            Logger.Debug("<<<<= PictureBoxDiagram::UpdateDraw(DiagramDrawLibrary)");
        }
        #endregion
    }
}
