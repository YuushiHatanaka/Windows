using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;
using TrainTimeTable.Common;
using TrainTimeTable.Property;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// PictureBoxDiagramCoreクラス
    /// </summary>
    public class PictureBoxDiagramCore : PictureBox
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// FormRouteオブジェクト
        /// </summary>
        protected FormRoute Owner;

        /// <summary>
        /// RouteFilePropertyオブジェクト
        /// </summary>
        protected RouteFileProperty Property;

        /// <summary>
        /// 横幅描画割合
        /// </summary>
        protected float WidthPercent { get; set; } = 100.0f;

        /// <summary>
        /// 縦幅描画割合
        /// </summary>
        protected float HeightPercent { get; set; } = 100.0f;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="property"></param>
        public PictureBoxDiagramCore(FormRoute owner, RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagramCore::PictureBoxDiagramCore(FormRoute, RouteFileProperty)");
            Logger.DebugFormat("owner   :[{0}]", owner);
            Logger.DebugFormat("property:[{0}]", property);

            // 設定
            Owner = owner;
            Property = property;

            // ロギング
            Logger.Debug("<<<<= PictureBoxDiagramCore::PictureBoxDiagramCore(FormRoute, RouteFileProperty)");
        }
        #endregion

        #region 描画更新
        /// <summary>
        /// 描画更新
        /// </summary>
        public virtual void UpdateDraw()
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagramCore::UpdateDraw()");

            // 描画更新
            UpdateDraw(Property);

            // ロギング
            Logger.Debug("<<<<= PictureBoxDiagramCore::UpdateDraw()");
        }

        /// <summary>
        /// 描画更新
        /// </summary>
        /// <param name="property"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void UpdateDraw(RouteFileProperty property)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 描画幅取得
        /// <summary>
        /// 描画幅取得
        /// </summary>
        /// <returns></returns>
        protected int GetTotalWidth()
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagramCore::GetTotalWidth()");

            // 結果設定
            int result = GetTotalWidth(WidthPercent);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= PictureBoxDiagramCore::GetTotalWidth()");

            // 返却
            return result;
        }

        /// <summary>
        /// 描画幅取得
        /// </summary>
        /// <param name="percent"></param>
        /// <returns></returns>
        protected int GetTotalWidth(float percent)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagramCore::GetTotalWidth(float)");
            Logger.DebugFormat("percent:[{0}]", percent);

            // 結果設定
            int result = (int)((24 * 60 * DiagramDrawLibrary.MinuteUnitWidth) * (percent / 100));

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= PictureBoxDiagramCore::GetTotalWidth(float)");

            // 返却
            return result;
        }
        #endregion

        #region 描画高取得
        /// <summary>
        /// 描画高取得
        /// </summary>
        /// <returns></returns>
        protected int GetTotalHeight()
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagramCore::GetTotalHeight()");

            // 結果設定
            int result = GetTotalHeight(Property.Stations.Count);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= PictureBoxDiagramCore::GetTotalHeight()");

            // 返却
            return result;
        }

        /// <summary>
        /// 描画高取得
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        protected int GetTotalHeight(int station)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagramCore::GetTotalHeight(int)");
            Logger.DebugFormat("station:[{0}]", station);

            // 結果設定
            int result = GetTotalHeight(station, HeightPercent);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= PictureBoxDiagramCore::GetTotalHeight(int)");

            // 返却
            return result;
        }

        /// <summary>
        /// 描画高取得
        /// </summary>
        /// <param name="station"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        protected int GetTotalHeight(int station, float percent)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagramCore::GetTotalHeight(int, float)");
            Logger.DebugFormat("station:[{0}]", station);
            Logger.DebugFormat("percent:[{0}]", percent);

            // 結果設定
            int result = (int)(((station + 1) * DiagramDrawLibrary.StationUnitHeight) * (percent / 100));

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= PictureBoxDiagramCore::GetTotalHeight(int, float)");

            // 返却
            return result;
        }
        #endregion

        #region 描画サイズ取得
        /// <summary>
        /// 描画サイズ取得
        /// </summary>
        /// <param name="percent"></param>
        /// <returns></returns>
        public SizeF GetDrawSize(float percent)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagramCore::GetDrawSize(float)");
            Logger.DebugFormat("percent:[{0}]", percent);

            // 結果設定
            SizeF result = new SizeF(Image.Width * (percent / 100), Image.Height * (percent / 100));

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= PictureBoxDiagramCore::GetDrawSize(float)");

            // 返却
            return result;
        }
        #endregion

        #region 描画イメージ取得
        /// <summary>
        /// 描画イメージ取得
        /// </summary>
        /// <param name="page"></param>
        /// <param name="percent"></param>
        /// <param name="scaleSize"></param>
        /// <param name="pageRectangle"></param>
        /// <param name="drawRectangle"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public Image GetDrawImage(int page, float percent, SizeF scaleSize, RectangleF pageRectangle, RectangleF drawRectangle, ref bool next)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagramCore::GetTotalHeight(int, float, SizeF, RectangleF, RectangleF, ref bool)");
            Logger.DebugFormat("page         :[{0}]", page);
            Logger.DebugFormat("percent      :[{0}]", percent);
            Logger.DebugFormat("scaleSize    :[{0}]", scaleSize);
            Logger.DebugFormat("pageRectangle:[{0}]", pageRectangle);
            Logger.DebugFormat("drawRectangle:[{0}]", drawRectangle);
            Logger.DebugFormat("next         :[{0}]", next);

            // トータル横幅を計算
            //int totalWidth = GetTotalWidth(percent);
            float totalWidth = Image.Width * (percent / 100);

            // 単位時間横幅を計算
            float unitWidth = totalWidth / 24;

            // 縮小サイズを取得
            Image scaleSizeImage = ResizeImage(scaleSize);

            // 1ページ当たりの描画できる時間を計算
            int unitHours = 0;
            for (float i = 0; i < pageRectangle.Width; i += unitWidth)
            {
                if (i + unitWidth > pageRectangle.Width)
                {
                    break;
                }

                // 時間数を加算
                unitHours++;
            }

            // 切り出し位置計算
            float cuttingPosition_X = (page - 1) * (unitHours * unitWidth);
            float cuttingPosition_Y = 0;
            float cuttingPosition_Witdh = unitHours * unitWidth;
            float cuttingPosition_Height = drawRectangle.Height;

            if (page * unitHours * unitWidth > totalWidth)
            {
                next = false;
            }

            // ロギング
            Logger.DebugFormat("≪取得イメージ情報≫");
            Logger.DebugFormat("トータル横幅   :{0}", totalWidth);
            Logger.DebugFormat("単位時間横幅   :{0}", unitWidth);
            Logger.DebugFormat("縮小画像       :{0}", scaleSizeImage.Size);
            Logger.DebugFormat("描画可能時間   :{0}", unitHours);
            Logger.DebugFormat("切り出し位置(X):{0}", cuttingPosition_X);
            Logger.DebugFormat("切り出し位置(Y):{0}", cuttingPosition_Y);
            Logger.DebugFormat("切り出し幅     :{0}", cuttingPosition_Witdh);
            Logger.DebugFormat("切り出し高     :{0}", cuttingPosition_Height);

            // 結果設定
            Image result = CropImage(scaleSizeImage, cuttingPosition_X, cuttingPosition_Y, cuttingPosition_Witdh, cuttingPosition_Height);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= PictureBoxDiagramCore::GetTotalHeight(int, float, SizeF, RectangleF, RectangleF, ref bool)");

            // 返却
            return result;
        }
        #endregion

        #region 描画イメージ(Resize)取得
        /// <summary>
        /// 描画イメージ(Resize)取得
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public Image ResizeImage(SizeF size)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagramCore::ResizeImage(SizeF)");
            Logger.DebugFormat("size:[{0}]", size);

            // 結果設定
            Image result = ResizeImage(Image, size.Width, size.Height);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= PictureBoxDiagramCore::ResizeImage(SizeF)");

            // 返却
            return result;
        }

        /// <summary>
        /// 描画イメージ(Resize)取得
        /// </summary>
        /// <param name="image"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Image ResizeImage(Image image, SizeF size)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagramCore::ResizeImage(Image, SizeF)");
            Logger.DebugFormat("image:[{0}]", image);
            Logger.DebugFormat("size :[{0}]", size);

            // 結果設定
            Image result = ResizeImage(image, size.Width, size.Height);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= PictureBoxDiagramCore::ResizeImage(Image, SizeF)");

            // 返却
            return result;
        }

        /// <summary>
        /// 描画イメージ(Resize)取得
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Image ResizeImage(Image image, float width, float height)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagramCore::ResizeImage(Image, float, float)");
            Logger.DebugFormat("image :[{0}]", image);
            Logger.DebugFormat("width :[{0}]", width);
            Logger.DebugFormat("height:[{0}]", height);

            // 新しいBitmapを作成
            Bitmap result = new Bitmap((int)width, (int)height);

            // Graphicsオブジェクトを取得
            using (Graphics g = Graphics.FromImage(result))
            {
                // 補間モードを指定して画像を縮小して描画
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, width, height);
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= PictureBoxDiagramCore::ResizeImage(Image, float, float)");

            // 返却
            return result;
        }
        #endregion

        #region 描画イメージ(切り出し)取得
        /// <summary>
        /// 描画イメージ(切り出し)取得
        /// </summary>
        /// <param name="point"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Image CropImage(PointF point, SizeF size)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagramCore::CropImage(PointF, SizeF)");
            Logger.DebugFormat("point:[{0}]", point);
            Logger.DebugFormat("size :[{0}]", size);

            // 結果設定
            Image result = CropImage(point.X, point.Y, size.Width, size.Height);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= PictureBoxDiagramCore::CropImage(PointF, SizeF)");

            // 返却
            return result;
        }

        /// <summary>
        /// 描画イメージ(切り出し)取得
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Image CropImage(float x, float y, SizeF size)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagramCore::CropImage(float, float, SizeF)");
            Logger.DebugFormat("x   :[{0}]", x);
            Logger.DebugFormat("y   :[{0}]", y);
            Logger.DebugFormat("size:[{0}]", size);

            // 結果設定
            Image result = CropImage(x, y, size.Width, size.Height);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= PictureBoxDiagramCore::CropImage(float, float, SizeF)");

            // 返却
            return result;
        }

        /// <summary>
        /// 描画イメージ(切り出し)取得
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Image CropImage(float x, float y, float width, float height)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagramCore::CropImage(float, float, float, float)");
            Logger.DebugFormat("x     :[{0}]", x);
            Logger.DebugFormat("y     :[{0}]", y);
            Logger.DebugFormat("width :[{0}]", width);
            Logger.DebugFormat("height:[{0}]", height);

            // 結果設定
            Image result = CropImage(Image, x, y, width, height);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= PictureBoxDiagramCore::CropImage(float, float, float, float)");

            // 返却
            return result;
        }

        /// <summary>
        /// 描画イメージ(切り出し)取得
        /// </summary>
        /// <param name="image"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Image CropImage(Image image, float x, float y, float width, float height)
        {
            // ロギング
            Logger.Debug("=>>>> PictureBoxDiagramCore::CropImage(Image, float, float, float, float)");
            Logger.DebugFormat("image :[{0}]", image);
            Logger.DebugFormat("x     :[{0}]", x);
            Logger.DebugFormat("y     :[{0}]", y);
            Logger.DebugFormat("width :[{0}]", width);
            Logger.DebugFormat("height:[{0}]", height);

            // 切り取る部分の範囲を決定する。
            System.Drawing.Rectangle srcRect = new System.Drawing.Rectangle((int)x, (int)y, (int)width, (int)height);

            // 描画する部分の範囲を決定する。
            System.Drawing.Rectangle desRect = new System.Drawing.Rectangle(0, 0, srcRect.Width, srcRect.Height);

            // 新しいBitmapを作成
            Bitmap result = new Bitmap((int)width, (int)height);

            // Graphicsオブジェクトを取得
            using (Graphics g = Graphics.FromImage(result))
            {
                //画像の一部を描画する
                g.DrawImage(image, desRect, srcRect, GraphicsUnit.Pixel);
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= PictureBoxDiagramCore::CropImage(Image, float, float, float, float)");

            // 返却
            return result;
        }
        #endregion
    }
}
