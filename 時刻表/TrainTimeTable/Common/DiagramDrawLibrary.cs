using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Controls.Primitives;
using TrainTimeTable.Control;
using TrainTimeTable.Property;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// DiagramDrawLibraryクラス
    /// </summary>
    public class DiagramDrawLibrary : IDisposable
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Disposed
        /// <summary>
        /// Track whether Dispose has been called.
        /// </summary>
        private bool m_Disposed = false;
        #endregion

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

        /// <summary>
        /// RouteFilePropertyオブジェクト
        /// </summary>
        private RouteFileProperty m_RouteFileProperty { get; set; } = null;

        /// <summary>
        /// 起点時間
        /// </summary>
        private int m_StartHours { get; set; } = 0;

        /// <summary>
        /// 終点時間
        /// </summary>
        private int m_EndHours { get; set; } = 23;

        /// <summary>
        /// 分ステップ数
        /// </summary>
        public static int MinuteStep { get; set; } = 2;

        /// <summary>
        /// 分単位横幅
        /// </summary>
        public static int MinuteUnitWidth { get; set; } = 4;

        /// <summary>
        /// 駅単位縦幅
        /// </summary>
        public static int StationUnitHeight { get; set; } = 32;

        /// <summary>
        /// 描画横幅
        /// </summary>
        private int Width { get; set; } = 0;

        /// <summary>
        /// 描画縦幅
        /// </summary>
        private int Height { get; set; } = 0;

        /// <summary>
        /// 横幅描画割合
        /// </summary>
        protected float WidthPercent { get; set; } = 100.0f;

        /// <summary>
        /// 縦幅描画割合
        /// </summary>
        protected float HeightPercent { get; set; } = 100.0f;

        /// <summary>
        /// Bitmapオブジェクト
        /// </summary>
        public Bitmap Bitmap { get; private set; } = null;

        /// <summary>
        /// Graphicsオブジェクト
        /// </summary>
        private Graphics m_Graphics { get; set; } = null;

        /// <summary>
        /// DiagramDrawPropertiesオブジェクト
        /// </summary>
        private DiagramDrawProperties m_DiagramDrawProperties { get; set; } = new DiagramDrawProperties();

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="width"></param>
        /// <param name="widthPercent"></param>
        /// <param name="height"></param>
        /// <param name="heightPercent"></param>
        public DiagramDrawLibrary(int width, float widthPercent, int height, float heightPercent)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DiagramDrawLibrary(int, float, int, float)");
            Logger.DebugFormat("width        :[{0}]", width);
            Logger.DebugFormat("widthPercent :[{0}]", widthPercent);
            Logger.DebugFormat("height       :[{0}]", height);
            Logger.DebugFormat("heightPercent:[{0}]", heightPercent);

            // 設定
            Width = width;
            WidthPercent = widthPercent;
            Height = height;
            HeightPercent = heightPercent;

            // Bitmapオブジェクト生成
            Bitmap = new Bitmap(Width, Height);

            // Graphicsオブジェクト生成
            m_Graphics = Graphics.FromImage(Bitmap);

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DiagramDrawLibrary(int, float, int, float)");
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~DiagramDrawLibrary()
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::~DiagramDrawLibrary()");

            // リソース破棄
            Dispose(false);

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::~DiagramDrawLibrary()");
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::Dispose()");

            // リソース破棄
            Dispose(true);

            // ガベージコレクション
            GC.SuppressFinalize(this);

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::Dispose()");
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::Dispose(bool)");
            Logger.DebugFormat("disposing:[{0}]", disposing);

            if (!m_Disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here.
                    m_Graphics?.Dispose();
                    m_Graphics = null;
                }

                // TODO: Free unmanaged resources here.

                // Note disposing has been done.
                m_Disposed = true;
            }

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::Dispose(bool)");
        }
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="property"></param>
        public void Initialization(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::Initialization(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 設定
            m_RouteFileProperty = property;

            // 起点時間設定
            m_StartHours = m_RouteFileProperty.DiagramScreen.DiagramDtartingTime.Hour;

            // 終点時間設定
            m_EndHours = m_StartHours + 23;

            // 初期化(駅描画情報)
            InitializationDrawStation();

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::Initialization(RouteFileProperty)");
        }

        /// <summary>
        /// 初期化(駅描画情報)
        /// </summary>
        private void InitializationDrawStation()
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::InitializationDrawStation()");

            // リストクリア
            m_DiagramDrawProperties.Clear();

            // 駅シーケンスリスト取得(昇順)
            var stationSequences = m_RouteFileProperty.StationSequences.OrderBy(t => t.Seq);

            // 駅を繰り返す
            int stationIndex = 0;
            foreach (var stationSequence in stationSequences)
            {
                // 駅情報取得
                StationProperty station = m_RouteFileProperty.Stations.Find(t => t.Name == stationSequence.Name);

                // DiagramDrawPropertyオブジェクト生成
                DiagramDrawProperty diagramDrawProperty = new DiagramDrawProperty();
                diagramDrawProperty.Station = new StationProperty(station);
                diagramDrawProperty.DrawHeight = GetStationPosition(stationIndex++);
                diagramDrawProperty.PenSize = GetStationPenSize(station.StationScale, 2, 1);
                diagramDrawProperty.Font = GetStationFont(m_RouteFileProperty.Fonts, station.StationScale);

                // 登録
                m_DiagramDrawProperties.Add(diagramDrawProperty);
            }

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::InitializationDrawStation()");
        }
        #endregion

        #region 全体塗りつぶし
        /// <summary>
        /// 全体塗りつぶし
        /// </summary>
        public void DrawFillInTheWhole()
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawFillInTheWhole()");

            // 全体塗りつぶし
            DrawFillInTheWhole(Brushes.White);

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawFillInTheWhole()");
        }

        /// <summary>
        /// 全体塗りつぶし
        /// </summary>
        /// <param name="brush"></param>
        public void DrawFillInTheWhole(Brush brush)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawFillInTheWhole(Brush)");
            Logger.DebugFormat("brush:[{0}]", brush);

            // 全体を塗りつぶす
            m_Graphics.FillRectangle(brush, m_Graphics.VisibleClipBounds);

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawFillInTheWhole(Brush)");
        }
        #endregion

        #region 大外枠描画
        /// <summary>
        /// 大外枠描画
        /// </summary>
        public void DrawLargeOuterFrame()
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawLargeOuterFrame()");

            // 大外枠描画
            DrawLargeOuterFrame(Width, Height, Color.DarkGray, 4);

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawLargeOuterFrame()");
        }

        /// <summary>
        /// 大外枠描画
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        /// <param name="size"></param>
        public void DrawLargeOuterFrame(int width, int height, Color color, float size)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawLargeOuterFrame(int, int, Color, float)");
            Logger.DebugFormat("width :[{0}]", width);
            Logger.DebugFormat("height:[{0}]", height);
            Logger.DebugFormat("color :[{0}]", color);
            Logger.DebugFormat("size  :[{0}]", size);

            // セル内のグリッド線を描画するためのペンを作成
            using (Pen pen = new Pen(color, size))
            {
                // セル内に線を描画
                m_Graphics.DrawRectangle(pen, 0, 0, width, height);
            }

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawLargeOuterFrame(int, int, Color, float)");
        }
        #endregion

        #region 時グリッド描画
        /// <summary>
        /// 時グリッド描画
        /// </summary>
        public void DrawHoursGrids()
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawHoursGrids()");

            // 時グリッド描画
            DrawHoursGrids(Color.DarkGray, 2, 1, DashStyle.Dash);

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawHoursGrids()");
        }

        /// <summary>
        /// 時グリッド描画
        /// </summary>
        /// <param name="font"></param>
        /// <param name="brash"></param>
        public void DrawHoursGrids(Font font, Brush brash)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawHoursGrids(Font, Brush)");
            Logger.DebugFormat("font :[{0}]", font);
            Logger.DebugFormat("brash:[{0}]", brash);

            // 時グリッド描画
            DrawHoursGrids(font, brash, Color.DarkGray, 2, 1, DashStyle.Dash);

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawHoursGrids(Font, Brush)");
        }

        /// <summary>
        /// 時グリッド描画
        /// </summary>
        /// <param name="color"></param>
        /// <param name="boldSize"></param>
        /// <param name="solidSize"></param>
        /// <param name="dash"></param>
        public void DrawHoursGrids(Color color, int boldSize, int solidSize, DashStyle unitStyle)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawLargeOuterFrame(Color, int, int, DashStyle)");
            Logger.DebugFormat("color    :[{0}]", color);
            Logger.DebugFormat("boldSize :[{0}]", boldSize);
            Logger.DebugFormat("solidSize:[{0}]", solidSize);
            Logger.DebugFormat("unitStyle:[{0}]", unitStyle);

            // セル内のグリッド線を描画するためのペンを作成
            using (Pen hoursPen = new Pen(color, boldSize))
            {
                // 24時間繰り返す
                for (int hours = 0; hours < 24; hours++)
                {
                    // 時間描画横位置取得
                    int hoursPosition = GetHoursLineDrawPosition(hours);

                    // 60分繰り返す(0分は時で実施済み、2分単位)
                    for (int minutes = 2; minutes < 60; minutes += DiagramDrawLibrary.MinuteStep)
                    {
                        // 時間(分)位置取得
                        int minutesPosition = GetMinutesLineDrawPosition(hoursPosition, minutes);

                        // 分単位Penオブジェクト取得
                        using (Pen minutesPen = GetMinutesPen(color, solidSize, minutes, unitStyle))
                        {
                            // 分単位線描画
                            m_Graphics.DrawLine(minutesPen, minutesPosition, 0, minutesPosition, Height);
                        }
                    }

                    // 時単位線描画
                    m_Graphics.DrawLine(hoursPen, hoursPosition, 0, hoursPosition, Height);
                }
            }

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawLargeOuterFrame(Color, int, int, DashStyle)");
        }

        /// <summary>
        /// 時グリッド描画
        /// </summary>
        /// <param name="font"></param>
        /// <param name="brash"></param>
        /// <param name="color"></param>
        /// <param name="boldSize"></param>
        /// <param name="solidSize"></param>
        /// <param name="unitStype"></param>
        public void DrawHoursGrids(Font font, Brush brash, Color color, int boldSize, int solidSize, DashStyle unitStyle)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawLargeOuterFrame(Font, Brush, Color, int, int, DashStyle)");
            Logger.DebugFormat("font     :[{0}]", font);
            Logger.DebugFormat("brash    :[{0}]", brash);
            Logger.DebugFormat("color    :[{0}]", color);
            Logger.DebugFormat("boldSize :[{0}]", boldSize);
            Logger.DebugFormat("solidSize:[{0}]", solidSize);
            Logger.DebugFormat("unitStyle:[{0}]", unitStyle);

            // 起点時間
            int startHours = m_RouteFileProperty.DiagramScreen.DiagramDtartingTime.Hour;

            // セル内のグリッド線を描画するためのペンを作成
            using (Pen hoursPen = new Pen(color, boldSize))
            {
                // 24時間繰り返す
                for (int hours = 0; hours < 24; hours++)
                {
                    // 時間描画横位置取得
                    int hoursPosition = GetHoursLineDrawPosition(hours);

                    // 描画時間設定
                    int drawTime = hours + startHours;
                    if (drawTime >= 24)
                    {
                        drawTime -= 24;
                    }

                    // 時間文字列描画
                    m_Graphics.DrawString(drawTime.ToString(), font, brash, hoursPosition, 0);

                    // 時単位線描画
                    m_Graphics.DrawLine(hoursPen, hoursPosition, 0, hoursPosition, Height);
                }
            }

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawLargeOuterFrame(Font, Brush, Color, int, int, DashStyle)");
        }

        /// <summary>
        /// 時間(時)線描画位置取得
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        private int GetHoursLineDrawPosition(int hour)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::GetHoursLineDrawPosition(int)");
            Logger.DebugFormat("hour:[{0}]", hour);

            // 結果設定
            int result = (int)(hour * 60 * (MinuteUnitWidth * (WidthPercent / 100)));

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DiagramDrawLibrary::GetHoursLineDrawPosition(int)");

            // 返却
            return result;
        }

        /// <summary>
        /// 時間(分)線描画位置取得
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        private int GetMinutesLineDrawPosition(int hour, int minutes)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::GetMinutesLineDrawPosition(int, int)");
            Logger.DebugFormat("hour   :[{0}]", hour);
            Logger.DebugFormat("minutes:[{0}]", minutes);

            // 結果設定
            int result = (int)(hour + (minutes * (MinuteUnitWidth * (WidthPercent / 100))));

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DiagramDrawLibrary::GetMinutesLineDrawPosition(int, int)");

            // 返却
            return result;
        }

        /// <summary>
        /// 分単位Penオブジェクト取得
        /// </summary>
        /// <param name="color"></param>
        /// <param name="size"></param>
        /// <param name="minutes"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        private Pen GetMinutesPen(Color color, int size, int minutes, DashStyle style)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::GetMinutesPen(Color, int, int, DashStyle)");
            Logger.DebugFormat("color  :[{0}]", color);
            Logger.DebugFormat("size   :[{0}]", size);
            Logger.DebugFormat("minutes:[{0}]", minutes);
            Logger.DebugFormat("style  :[{0}]", style);

            // 結果オブジェクト生成
            Pen result = new Pen(color, size);

            // 10分毎か？
            if (minutes % 10 != 0)
            {
                // スタイル設定
                result.DashStyle = style;
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DiagramDrawLibrary::GetMinutesPen(Color, int, int, DashStyle)");

            // 返却
            return result;
        }
        #endregion

        #region 駅グリッド描画
        /// <summary>
        /// 駅グリッド描画
        /// </summary>
        /// <param name="property"></param>
        /// <param name="boldSize"></param>
        /// <param name="solidSize"></param>
        public void DrawStationsGrids()
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawStationsGrids()");

            // 駅シーケンスリスト取得(昇順)
            var stationSequences = m_RouteFileProperty.StationSequences.OrderBy(t => t.Seq);

            // 駅を繰り返す
            foreach (var stationSequence in stationSequences)
            {
                // 駅情報取得
                StationProperty station = m_RouteFileProperty.Stations.Find(t => t.Name == stationSequence.Name);

                // DiagramDrawPropertyオブジェクト取得
                DiagramDrawProperty diagramDrawProperty = m_DiagramDrawProperties.Find(t => t.Station.Compare(station));

                // 駅Penオブジェクト取得
                using (Pen stationPen = new Pen(Color.DarkGray, diagramDrawProperty.PenSize))
                {
                    // 駅単位線描画
                    m_Graphics.DrawLine(stationPen, 0, diagramDrawProperty.DrawHeight, Width, diagramDrawProperty.DrawHeight);
                }
            }

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawStationsGrids()");
        }

        /// <summary>
        /// 駅グリッド描画
        /// </summary>
        /// <param name="brash"></param>
        public void DrawStationsGrids(Brush brash)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawStationsGrids(Brush)");
            Logger.DebugFormat("brash:[{0}]", brash);

            // 駅シーケンスリスト取得(昇順)
            var stationSequences = m_RouteFileProperty.StationSequences.OrderBy(t => t.Seq);

            // 駅を繰り返す
            foreach (var stationSequence in stationSequences)
            {
                // 駅情報取得
                StationProperty station = m_RouteFileProperty.Stations.Find(t => t.Name == stationSequence.Name);

                // DiagramDrawPropertyオブジェクト取得
                DiagramDrawProperty diagramDrawProperty = m_DiagramDrawProperties.Find(t => t.Station.Compare(station));

                // 駅Penオブジェクト取得
                using (Pen stationPen = new Pen(Color.DarkGray, diagramDrawProperty.PenSize))
                {
                    // 駅文字列描画
                    m_Graphics.DrawString(station.Name, diagramDrawProperty.Font, brash, 0, diagramDrawProperty.DrawHeight - diagramDrawProperty.Font.GetHeight());

                    // 駅単位線描画
                    m_Graphics.DrawLine(stationPen, 0, diagramDrawProperty.DrawHeight, Width, diagramDrawProperty.DrawHeight);
                }
            }

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawStationsGrids(Brush)");
        }

        /// <summary>
        /// 駅位置取得
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        private int GetStationPosition(int station)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::GetStationPosition(int)");
            Logger.DebugFormat("station:[{0}]", station);

            // 結果設定
            int result = (int)((station + 1) * StationUnitHeight * (HeightPercent / 100));

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DiagramDrawLibrary::GetStationPosition(int)");

            // 返却
            return result;
        }

        /// <summary>
        /// 駅線描画サイズ取得
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="boldSize"></param>
        /// <param name="solidSize"></param>
        /// <returns></returns>
        private int GetStationPenSize(StationScale scale, int boldSize, int solidSize)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::GetStationPenSize(StationScale, int, int)");
            Logger.DebugFormat("scale    :[{0}]", scale);
            Logger.DebugFormat("boldSize :[{0}]", boldSize);
            Logger.DebugFormat("solidSize:[{0}]", solidSize);

            // 結果設定
            int result = solidSize;

            // 駅規模判定
            if (scale == StationScale.MainStation)
            {
                // 太線サイズを設定
                result = boldSize;
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DiagramDrawLibrary::GetStationPenSize(StationScale, int, int)");

            // 返却
            return result;
        }

        /// <summary>
        /// 駅Fontオブジェクト取得
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        private Font GetStationFont(FontProperties properties, StationScale scale)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::GetStationFont(FontProperties, StationScale)");
            Logger.DebugFormat("properties:[{0}]", properties);
            Logger.DebugFormat("scale     :[{0}]", scale);

            // フォント名初期化
            string fontName = "一般駅";

            // 駅規模判定
            if (scale == StationScale.MainStation)
            {
                // フォント名変更
                fontName = "主要駅";
            }

            // 結果設定
            Font result = properties.GetFont(fontName);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DiagramDrawLibrary::GetStationFont(FontProperties, StationScale)");

            // 返却
            return result;
        }
        #endregion

        #region 列車描画
        /// <summary>
        /// 列車描画
        /// </summary>
        /// <param name="daiagramName"></param>
        public void DrawTrainsLine(string daiagramName)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawTrainsLine(string)");
            Logger.DebugFormat("daiagramName:[{0}]", daiagramName);

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = m_RouteFileProperty.Diagrams.Find(diagram => diagram.Name == daiagramName);

            // 結果判定
            if (diagramProperty == null)
            {
                // 例外
                throw new KeyNotFoundException(string.Format("ダイアグラムは登録されていません:[0]", daiagramName));
            }

            // 下り列車を描画
            DrawTrainsLineOutbound(diagramProperty.Trains[DirectionType.Outbound]);

            // 上り列車を描画
            DrawTrainsLineInbound(diagramProperty.Trains[DirectionType.Inbound]);

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawTrainsLine(string)");
        }

        /// <summary>
        /// 下り列車描画
        /// </summary>
        /// <param name="properties"></param>
        private void DrawTrainsLineOutbound(TrainProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawTrainsLineOutbound(TrainProperties)");
            Logger.DebugFormat("property:[{0}]", properties);

            // 列車を繰り返す
            foreach (var train in properties)
            {
                // TrainTypePropertyオブジェクト取得
                TrainTypeProperty trainTypeProperty = m_RouteFileProperty.TrainTypes.Find(t => t.Name == train.TrainTypeName);

                // 列車描画用Penオブジェクト生成
                using (Pen trainPen = trainTypeProperty.GetDiagramLinePen())
                {
                    // 始発駅、終着駅StationTimePropertyを取得
                    StationTimeProperty startTimes = train.StationTimes.Find(t => t.StationTreatment == StationTreatment.Stop);
                    StationTimeProperty endTimes = train.StationTimes.FindLast(t => t.StationTreatment == StationTreatment.Stop);

                    // 見つからなかった場合
                    if (startTimes == null || endTimes == null)
                    {
                        // ロギング
                        Logger.WarnFormat("以下の列車のダイヤグラム描画ができませんでした。\r\n{0}", train.ToString());
                        continue;
                    }

                    // 始発駅から次の停車駅を求める
                    StationTimeProperty startNextTimes = train.StationTimes.GetNext(startTimes, StationTreatment.Stop);

                    // 次の停車駅が見つからなかった場合
                    if (startNextTimes == null)
                    {
                        // 終着駅を次の停車駅とする
                        startNextTimes = new StationTimeProperty(endTimes);
                    }

                    // 描画開始位置取得
                    Point currentPoint = GetDrawingPosition(startTimes);
                    Point nextPoint = GetDrawingPosition(startNextTimes);
                    Point beforePoint = new Point(currentPoint.X, currentPoint.Y);

                    // ロギング
                    Logger.DebugFormat("【下り】{0}:{1}→{2}[{3}]", train.No, startTimes.StationName, endTimes.StationName, beforePoint);

                    // 駅時刻を繰り返す
                    for (int i = startTimes.Seq - 1; i <= endTimes.Seq - 1;)
                    {
                        // 対象駅、次停車駅、隣接駅のStationTimePropertyを取得
                        StationTimeProperty current = train.StationTimes[i];
                        StationTimeProperty next = train.StationTimes.GetNext(current, StationTreatment.Stop);
                        StationTimeProperty adjacentFrontStation = train.StationTimes.Find(t => t.Seq == current.Seq - 1);
                        StationTimeProperty adjacentNextStation = train.StationTimes.Find(t => t.Seq == current.Seq + 1);

                        // ロギング
                        Logger.DebugFormat("[{0}]⇒[{1}][{2},{3}]", current.StationName, next?.StationName, adjacentFrontStation?.StationName, adjacentNextStation?.StationName);

                        // 次駅がなかったら
                        if (next == null)
                        {
                            // 終了
                            break;
                        }

                        // 隣接駅判定
                        if (adjacentNextStation?.StationTreatment == StationTreatment.Stop || adjacentNextStation?.StationTreatment == StationTreatment.Passing)
                        {
                            // 列車描画
                            DrawTrainsLine(train, trainPen, current, next);
                        }
                        if (!(adjacentFrontStation?.StationTreatment == StationTreatment.Stop || adjacentFrontStation?.StationTreatment == StationTreatment.Passing))
                        {
                            // 列車番号表示
                            currentPoint = GetDrawingPosition(current);
                            nextPoint = GetDrawingPosition(next);
                            beforePoint = new Point(currentPoint.X, currentPoint.Y);

                            // 列車番号表示
                            DrawTrainNoName(beforePoint, currentPoint, nextPoint, train);
                        }

                        // 変数更新
                        i = next.Seq - 1;
                    }
                }
            }

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawTrainsLineOutbound(TrainProperties)");
        }

        /// <summary>
        /// 上り列車描画
        /// </summary>
        /// <param name="properties"></param>
        private void DrawTrainsLineInbound(TrainProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawTrainsLineInbound(TrainProperties)");
            Logger.DebugFormat("property:[{0}]", properties);

            // 列車を繰り返す
            foreach (var train in properties)
            {
                // TrainTypePropertyオブジェクト取得
                TrainTypeProperty trainTypeProperty = m_RouteFileProperty.TrainTypes.Find(t => t.Name == train.TrainTypeName);

                // 列車描画用Penオブジェクト生成
                using (Pen trainPen = trainTypeProperty.GetDiagramLinePen())
                {
                    // 始発駅、終着駅StationTimePropertyを取得
                    StationTimeProperty startTimes = train.StationTimes.FindLast(t => t.StationTreatment == StationTreatment.Stop);
                    StationTimeProperty endTimes = train.StationTimes.Find(t => t.StationTreatment == StationTreatment.Stop);

                    // 見つからなかった場合
                    if (startTimes == null || endTimes == null)
                    {
                        // ロギング
                        Logger.WarnFormat("以下の列車のダイヤグラム描画ができませんでした。\r\n{0}", train.ToString());
                        continue;
                    }

                    // 始発駅から次の停車駅を求める
                    StationTimeProperty startNextTimes = train.StationTimes.GetBefore(startTimes, StationTreatment.Stop);

                    // 次の停車駅が見つからなかった場合
                    if (startNextTimes == null)
                    {
                        // 終着駅を次の停車駅とする
                        startNextTimes = new StationTimeProperty(endTimes);
                    }

                    // 描画開始位置取得
                    Point currentPoint = GetDrawingPosition(startTimes);
                    Point nextPoint = GetDrawingPosition(startNextTimes);
                    Point beforePoint = new Point(currentPoint.X, currentPoint.Y);

                    // ロギング
                    Logger.DebugFormat("【上り】{0}:{1}→{2}[{3}]", train.No, startTimes.StationName, endTimes.StationName, beforePoint);

                    // 駅時刻を繰り返す
                    for (int i = startTimes.Seq - 1; i >= endTimes.Seq - 1;)
                    {
                        // 対象駅、次停車駅、隣接駅のStationTimePropertyを取得
                        StationTimeProperty current = train.StationTimes[i];
                        StationTimeProperty next = train.StationTimes.GetBefore(current, StationTreatment.Stop);
                        StationTimeProperty adjacentFrontStation = train.StationTimes.Find(t => t.Seq == current.Seq - 1);
                        StationTimeProperty adjacentNextStation = train.StationTimes.Find(t => t.Seq == current.Seq + 1);

                        // ロギング
                        Logger.DebugFormat("[{0}]⇒[{1}][{2},{3}]", current.StationName, next?.StationName, adjacentFrontStation?.StationName, adjacentNextStation?.StationName);

                        // 次駅がなかったら
                        if (next == null)
                        {
                            // 終了
                            break;
                        }

                        // 隣接駅判定
                        if (adjacentFrontStation?.StationTreatment == StationTreatment.Stop || adjacentFrontStation?.StationTreatment == StationTreatment.Passing)
                        {
                            // 列車描画
                            DrawTrainsLine(train, trainPen, current, next);
                        }
                        if (!(adjacentNextStation?.StationTreatment == StationTreatment.Stop || adjacentNextStation?.StationTreatment == StationTreatment.Passing))
                        {
                            // 列車番号表示
                            currentPoint = GetDrawingPosition(current);
                            nextPoint = GetDrawingPosition(next);
                            beforePoint = new Point(currentPoint.X, currentPoint.Y);

                            // 列車番号表示
                            DrawTrainNoName(beforePoint, currentPoint, nextPoint, train);
                        }

                        // 変数更新
                        i = next.Seq - 1;
                    }
                }
            }

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawTrainsLineInbound(TrainProperties)");
        }

        /// <summary>
        /// 描画位置取得
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private Point GetDrawingPosition(StationTimeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::GetDrawingPosition(StationTimeProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 結果オブジェクト生成
            Point result = new Point();

            // 着時刻が設定されているか？
            if (property.ArrivalTime != string.Empty)
            {
                // 着時刻を開始位置に設定
                result = new Point(GetTimePosition(property.GetArrivalTimeValue()), GetStationPosition(property.Seq - 1));
            }
            // 発時刻が設定されているか？
            else if (property.DepartureTime != string.Empty)
            {
                // 発時刻を開始位置に設定
                result = new Point(GetTimePosition(property.GetDepartureTimeValue()), GetStationPosition(property.Seq - 1));
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DiagramDrawLibrary::GetDrawingPosition(StationTimeProperty)");

            // 返却
            return result;
        }

        #region 時刻位置取得
        /// <summary>
        /// 時刻位置取得
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private int GetTimePosition(DateTime dateTime)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::GetTimePosition(DateTime)");
            Logger.DebugFormat("dateTime:[{0}]", dateTime);

            // 結果オブジェクト設定
            int result = GetTimePosition(dateTime.Hour, dateTime.Minute);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DiagramDrawLibrary::GetTimePosition(DateTime)");

            // 返却
            return result;
        }

        /// <summary>
        /// 時刻位置取得
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        private int GetTimePosition(int hour, int minute)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::GetTimePosition(int, int)");
            Logger.DebugFormat("hour  :[{0}]", hour);
            Logger.DebugFormat("minute:[{0}]", minute);

            // 描画時間取得
            hour = GetDrawHour(hour);

            // 時間跨ぎ時間取得
            hour = GetTimeSpanningTimeAcquisition(hour);

            // 結果オブジェクト設定
            int result = CalculationTimePosition(hour, minute);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DiagramDrawLibrary::GetTimePosition(int, int)");

            // 返却
            return result;
        }

        /// <summary>
        /// 時間跨ぎ時間取得
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        private int GetTimeSpanningTimeAcquisition(int hour)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::GetTimeSpanningTimeAcquisition(int)");
            Logger.DebugFormat("hour:[{0}]", hour);

            // 描画時間調整
            if (m_StartHours > 0)
            {
                hour = hour - m_StartHours;
            }

            // ロギング
            Logger.DebugFormat("hour:[{0}]", hour);
            Logger.Debug("<<<<= DiagramDrawLibrary::GetTimeSpanningTimeAcquisition(int)");

            // 返却
            return hour;
        }

        /// <summary>
        /// 時刻位置計算
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minutes"></param>
        private int CalculationTimePosition(int hour, int minutes)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::CalculationTimePosition(int, int)");
            Logger.DebugFormat("hour   :[{0}]", hour);
            Logger.DebugFormat("minutes:[{0}]", minutes);

            // 結果オブジェクト設定(時)
            int result = (int)(hour * 60 * (MinuteUnitWidth * (WidthPercent / 100)));

            // 結果オブジェクト設定(分)
            result = (int)(result + (minutes * (MinuteUnitWidth * (WidthPercent / 100))));

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DiagramDrawLibrary::CalculationTimePosition(int, int)");

            // 返却
            return result;
        }
        #endregion

        /// <summary>
        /// 描画時間取得
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        private int GetDrawHour(int hour)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::GetDrawHour(int)");
            Logger.DebugFormat("hour:[{0}]", hour);

            // 描画時間調整
            if (m_StartHours > 0 && hour < m_StartHours)
            {
                hour += 24;
            }

            // ロギング
            Logger.DebugFormat("hour:[{0}]", hour);
            Logger.Debug("<<<<= DiagramDrawLibrary::GetDrawHour(int)");

            // 返却
            return hour;
        }
        #endregion

        #region  列車番号、列車名描画
        /// <summary>
        /// 列車番号、列車名描画
        /// </summary>
        /// <param name="drawPoint"></param>
        /// <param name="beforePoint"></param>
        /// <param name="currentPoint"></param>
        /// <param name="property"></param>
        private void DrawTrainNoName(Point drawPoint, Point beforePoint, Point currentPoint, TrainProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawTrainNoName(Point, Point, Point, TrainProperty)");
            Logger.DebugFormat("drawPoint   :[{0}]", drawPoint);
            Logger.DebugFormat("beforePoint :[{0}]", beforePoint);
            Logger.DebugFormat("currentPoint:[{0}]", currentPoint);
            Logger.DebugFormat("property    :[{0}]", property);

            // 表示文字を設定
            string displayStrings = string.Empty;

            // 列車番号表示判定
            if (TrainNumberDisplay == true && TrainNameDisplay == true)
            {
                // 列車番号、列車名設定
                displayStrings = string.Format("{0} {1}{2}", property.No, property.Name, property.Number);
            }
            else if (TrainNumberDisplay == true)
            {
                // 列車番号設定
                displayStrings = string.Format("{0}", property.No);
            }
            else if (TrainNameDisplay == true)
            {
                // 列車名設定
                displayStrings = string.Format("{0}", property.Name);
            }

            // 表示文字を判定
            if (displayStrings == string.Empty)
            {
                // ロギング
                Logger.Debug("<<<<= DiagramDrawLibrary::DrawTrainNoName(Point, Point, Point, TrainProperty)");

                // 終了
                return;
            }

            // 傾きを求める
            double angle = MathLibrary.CalculateAngle(beforePoint, currentPoint);

            // ロギング
            Logger.DebugFormat("角度:{0,3} ", Math.Round(angle));

            // 大きさを調べるためのダミーのBitmapオブジェクトの作成
            Bitmap img0 = new Bitmap(1, 1);

            // imgのGraphicsオブジェクトを取得
            Graphics bg0 = Graphics.FromImage(img0);

            // 使用するFontオブジェクトを作成
            Font fnt = m_RouteFileProperty.Fonts.GetFont("列車番号");

            // 文字列を描画したときの大きさを計算する
            int w = (int)bg0.MeasureString(displayStrings, fnt).Width;
            int h = (int)fnt.GetHeight(bg0);

            // Bitmapオブジェクトを作り直す
            Bitmap img = new Bitmap(w, h);

            // ColorからSolidBrushに変換
            SolidBrush brush = new SolidBrush(m_RouteFileProperty.TrainTypes.Find(t => t.Name == property.TrainTypeName).DiagramLineColor);

            //imgに文字列を描画する
            Graphics bg = Graphics.FromImage(img);
            bg.DrawString(displayStrings, fnt, brush, 0, 0);

            // 回転するための座標を計算(ラジアン単位に変換)
            double d = angle / (180 / Math.PI);

            // 新しい座標位置を計算する
            float x1 = drawPoint.X + img.Width * (float)Math.Cos(d);
            float y1 = drawPoint.Y + img.Width * (float)Math.Sin(d);
            float x2 = drawPoint.X - img.Height * (float)Math.Sin(d);
            float y2 = drawPoint.Y + img.Height * (float)Math.Cos(d);

            // PointF配列を作成
            PointF[] destinationPoints =
            {
                new PointF(drawPoint.X, drawPoint.Y),
                new PointF(x1, y1),
                new PointF(x2, y2)
            };

            // 画像を描画
            m_Graphics.DrawImage(img, destinationPoints);

            // リソースを解放する
            bg0.Dispose();
            img0.Dispose();
            img.Dispose();
            bg.Dispose();

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawTrainNoName(Point, Point, Point, TrainProperty)");
        }
        #endregion

        #region 列車時刻描画
        /// <summary>
        /// 列車時刻描画
        /// </summary>
        /// <param name="train"></param>
        /// <param name="trainPen"></param>
        /// <param name="showDateTime"></param>
        /// <param name="drawDateTime"></param>
        /// <param name="height"></param>
        private void DrawTrainTime(TrainProperty train, Pen trainPen, string showDateTime, DateTime drawDateTime, int height)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawTrainTime(TrainProperty, Pen, string, DateTime, int)");
            Logger.DebugFormat("train       :[{0}]", train);
            Logger.DebugFormat("trainPen    :[{0}]", trainPen);
            Logger.DebugFormat("showDateTime:[{0}]", showDateTime);
            Logger.DebugFormat("drawDateTime:[{0}]", drawDateTime);
            Logger.DebugFormat("height      :[{0}]", height);

            // 描画判定
            if (!TrainTimeDisplay)
            {
                // 終了
                return;
            }

            // 初期化
            using (Font stationTimeFont = new Font("メイリオ", 6))
            {
                // ColorからSolidBrushに変換
                using (SolidBrush brush = new SolidBrush(trainPen.Color))
                {
                    // 時刻描画
                    m_Graphics.DrawString(showDateTime, stationTimeFont, brush, (PointF)new Point(GetTimePosition(drawDateTime), height));
                }
            }

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawTrainTime(TrainProperty, Pen, string, DateTime, int)");
        }
        #endregion

        #region 列車線描画
        /// <summary>
        /// 列車線描画
        /// </summary>
        /// <param name="train"></param>
        /// <param name="trainPen"></param>
        /// <param name="current"></param>
        /// <param name="next"></param>
        private void DrawTrainsLine(TrainProperty train, Pen trainPen, StationTimeProperty current, StationTimeProperty next)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawTrainsLine(TrainProperty, Pen, StationTimeProperty, StationTimeProperty)");
            Logger.DebugFormat("train   :[{0}]", train);
            Logger.DebugFormat("trainPen:[{0}]", trainPen);
            Logger.DebugFormat("current :[{0}]", current);
            Logger.DebugFormat("next    :[{0}]", next);

            // 駅扱いで分岐
            switch (current.StationTreatment)
            {
                case StationTreatment.Stop:
                    // ロギング
                    Logger.Debug("[停車]");
                    // 描画
                    DrawTrainsStationTreatmentStopLine(train, trainPen, current, next);
                    break;
                case StationTreatment.Passing:
                    // ロギング
                    Logger.Debug("[通過]");
                    // 描画
                    DrawTrainsStationTreatmentPassingLine(train, trainPen, current, next);
                    break;
                case StationTreatment.NoRoute:
                    // ロギング
                    Logger.Debug("[経由なし]");
                    // 描画
                    DrawTrainsStationTreatmentNoRouteLine(train, trainPen, current, next);
                    break;
                case StationTreatment.NoService:
                    // ロギング
                    Logger.Debug("[運行なし]");
                    // 描画
                    DrawTrainsStationTreatmentNoServiceLine(train, trainPen, current, next);
                    break;
                default:
                    // 例外
                    throw new AggregateException(string.Format("駅扱いの異常を検出しました:[{0}]", current.StationTreatment));
            }

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawTrainsLine(TrainProperty, Pen, StationTimeProperty, StationTimeProperty)");
        }

        /// <summary>
        /// 列車線描画(停車)
        /// </summary>
        /// <param name="train"></param>
        /// <param name="trainPen"></param>
        /// <param name="current"></param>
        /// <param name="next"></param>
        private void DrawTrainsStationTreatmentStopLine(TrainProperty train, Pen trainPen, StationTimeProperty current, StationTimeProperty next)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawTrainsStationTreatmentStopLine(TrainProperty, Pen, StationTimeProperty, StationTimeProperty)");
            Logger.DebugFormat("train   :[{0}]", train);
            Logger.DebugFormat("trainPen:[{0}]", trainPen);
            Logger.DebugFormat("current :[{0}]", current);
            Logger.DebugFormat("next    :[{0}]", next);

            // 次駅判定
            if (next == null)
            {
                // 着時刻が設定されているか？
                if (current.ArrivalTime != string.Empty)
                {
                    // 列車時刻描画
                    DrawTrainTime(train, trainPen, current.ArrivalTime, current.GetArrivalTimeValue(), m_DiagramDrawProperties[current.Seq - 1].DrawHeight);
                }
                // 発時刻が設定されているか？
                else if (current.DepartureTime != string.Empty)
                {
                    // 列車時刻描画
                    DrawTrainTime(train, trainPen, current.DepartureTime, current.GetDepartureTimeValue(), m_DiagramDrawProperties[current.Seq - 1].DrawHeight);
                }

                // ロギング
                Logger.Debug("<<<<= DiagramDrawLibrary::DrawTrainsStationTreatmentStopLine(TrainProperty, Pen, StationTimeProperty, StationTimeProperty)");

                // 終了
                return;
            }

            // 初期化
            DateTime currentDateTime = default(DateTime);
            DateTime nextDateTime = default(DateTime);
            Point currentPoint = default(Point);
            Point nextPoint = default(Point);

            // 着、発時刻両方が設定されている場合
            if ((current.ArrivalTime != string.Empty) && (current.DepartureTime != string.Empty))
            {
                // DateTimeオブジェクト取得
                currentDateTime = current.GetArrivalTimeValue();

                // DateTimeオブジェクト取得
                nextDateTime = current.GetDepartureTimeValue();

                // 次位置まで線を描画
                DrawLineNextPoint(train, trainPen, current, currentDateTime, current, nextDateTime);

                // 列車時刻描画
                DrawTrainTime(train, trainPen, current.ArrivalTime, current.GetArrivalTimeValue(), m_DiagramDrawProperties[current.Seq - 1].DrawHeight);

                // カレント位置更新
                currentPoint.X = nextPoint.X;
                currentPoint.Y = nextPoint.Y;
                currentDateTime = nextDateTime;
            }
            // 着時刻のみ設定されている場合
            else if ((current.ArrivalTime != string.Empty) && (current.DepartureTime == string.Empty))
            {
                // DateTimeオブジェクト取得
                currentDateTime = current.GetArrivalTimeValue();

                // 描画開始位置取得
                currentPoint = new Point(GetTimePosition(currentDateTime), m_DiagramDrawProperties[current.Seq - 1].DrawHeight);
            }
            // 発時刻のみ設定されている場合
            else if ((current.ArrivalTime == string.Empty) && (current.DepartureTime != string.Empty))
            {
                // DateTimeオブジェクト取得
                currentDateTime = current.GetDepartureTimeValue();

                // 描画開始位置取得
                currentPoint = new Point(GetTimePosition(currentDateTime), m_DiagramDrawProperties[current.Seq - 1].DrawHeight);
            }
            // 着、発時刻両方が設定されていない場合
            else
            {
                // ロギング
                Logger.WarnFormat("着、発時刻両方が設定されていません:[current][{0}][{1}][{2}]", train.DiagramName, train.Direction.GetStringValue(), train.Id);
                return;
            }

            // 着、発時刻両方が設定されている場合
            if ((next.ArrivalTime != string.Empty) && (next.DepartureTime != string.Empty))
            {
                // DateTimeオブジェクト取得
                nextDateTime = next.GetArrivalTimeValue();
            }
            // 着時刻のみ設定されている場合
            else if ((next.ArrivalTime != string.Empty) && (next.DepartureTime == string.Empty))
            {
                // DateTimeオブジェクト取得
                nextDateTime = next.GetArrivalTimeValue();
            }
            // 発時刻のみ設定されている場合
            else if ((next.ArrivalTime == string.Empty) && (next.DepartureTime != string.Empty))
            {
                // DateTimeオブジェクト取得
                nextDateTime = next.GetDepartureTimeValue();
            }
            // 着、発時刻両方が設定されていない場合
            else
            {
                // ロギング
                Logger.WarnFormat("着、発時刻両方が設定されていません:[next][{0}][{1}][{2}]", train.DiagramName, train.Direction.GetStringValue(), train.Id);
                return;
            }

            // 列車時刻描画
            DrawTrainTime(train, trainPen, currentDateTime.ToString("HHmm"), currentDateTime, m_DiagramDrawProperties[current.Seq - 1].DrawHeight);

            // 次位置まで線を描画
            DrawLineNextPoint(train, trainPen, current, currentDateTime, next, nextDateTime);

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawTrainsStationTreatmentStopLine(TrainProperty, Pen, StationTimeProperty, StationTimeProperty)");
        }

        /// <summary>
        /// 列車線描画(通過)
        /// </summary>
        /// <param name="train"></param>
        /// <param name="trainPen"></param>
        /// <param name="current"></param>
        /// <param name="next"></param>
        private void DrawTrainsStationTreatmentPassingLine(TrainProperty train, Pen trainPen, StationTimeProperty current, StationTimeProperty next)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawTrainsStationTreatmentPassingLine(TrainProperty, Pen, StationTimeProperty, StationTimeProperty)");
            Logger.DebugFormat("train   :[{0}]", train);
            Logger.DebugFormat("trainPen:[{0}]", trainPen);
            Logger.DebugFormat("current :[{0}]", current);
            Logger.DebugFormat("next    :[{0}]", next);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawTrainsStationTreatmentPassingLine(TrainProperty, Pen, StationTimeProperty, StationTimeProperty)");
        }

        /// <summary>
        /// 列車線描画(経由なし)
        /// </summary>
        /// <param name="train"></param>
        /// <param name="trainPen"></param>
        /// <param name="current"></param>
        /// <param name="next"></param>
        private void DrawTrainsStationTreatmentNoRouteLine(TrainProperty train, Pen trainPen, StationTimeProperty current, StationTimeProperty next)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawTrainsStationTreatmentNoRouteLine(TrainProperty, Pen, StationTimeProperty, StationTimeProperty)");
            Logger.DebugFormat("train   :[{0}]", train);
            Logger.DebugFormat("trainPen:[{0}]", trainPen);
            Logger.DebugFormat("current :[{0}]", current);
            Logger.DebugFormat("next    :[{0}]", next);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawTrainsStationTreatmentNoRouteLine(TrainProperty, Pen, StationTimeProperty, StationTimeProperty)");
        }

        /// <summary>
        /// 列車線描画(運行なし)
        /// </summary>
        /// <param name="train"></param>
        /// <param name="trainPen"></param>
        /// <param name="current"></param>
        /// <param name="next"></param>
        private void DrawTrainsStationTreatmentNoServiceLine(TrainProperty train, Pen trainPen, StationTimeProperty current, StationTimeProperty next)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawTrainsStationTreatmentNoServiceLine(TrainProperty, Pen, StationTimeProperty, StationTimeProperty)");
            Logger.DebugFormat("train   :[{0}]", train);
            Logger.DebugFormat("trainPen:[{0}]", trainPen);
            Logger.DebugFormat("current :[{0}]", current);
            Logger.DebugFormat("next    :[{0}]", next);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawTrainsStationTreatmentNoServiceLine(TrainProperty, Pen, StationTimeProperty, StationTimeProperty)");
        }
        #endregion

        #region 列車線描画
        /// <summary>
        /// 列車線描画
        /// </summary>
        /// <param name="train"></param>
        /// <param name="trainPen"></param>
        /// <param name="current"></param>
        /// <param name="currentDateTime"></param>
        /// <param name="next"></param>
        /// <param name="nextDateTime"></param>
        private void DrawLineNextPoint(TrainProperty train, Pen trainPen, StationTimeProperty current, DateTime currentDateTime, StationTimeProperty next, DateTime nextDateTime)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DrawLineNextPoint(TrainProperty, Pen, StationTimeProperty, StationTimeProperty)");
            Logger.DebugFormat("train          :[{0}]", train);
            Logger.DebugFormat("trainPen       :[{0}]", trainPen);
            Logger.DebugFormat("current        :[{0}]", current);
            Logger.DebugFormat("currentDateTime:[{0}]", currentDateTime);
            Logger.DebugFormat("next           :[{0}]", next);
            Logger.DebugFormat("nextDateTime   :[{0}]", nextDateTime);

            // 設定
            int currentDateHour = GetDrawHour(currentDateTime.Hour);
            int nextDateHour = GetDrawHour(nextDateTime.Hour);

            // 跨り判定
            if (currentDateHour > nextDateHour)
            {
                // 描画開始位置取得
                Point currentPoint = new Point(GetTimePosition(currentDateTime), m_DiagramDrawProperties[current.Seq - 1].DrawHeight);

                // 次駅（跨り先)の時間を設定
                nextDateHour = nextDateTime.Hour + 24 - m_StartHours;

                // 次位置設定
                Point drawPoint = new Point(CalculationTimePosition(nextDateHour, nextDateTime.Minute), m_DiagramDrawProperties[next.Seq - 1].DrawHeight);

                // 時単位線描画
                m_Graphics.DrawLine(trainPen, currentPoint, drawPoint);

                // 前駅（跨り先)の時間を設定
                DateTime baseDateTime = DateTime.Today.AddHours(m_StartHours);
                DateTime targetDateTime = DateTime.Today.AddHours(currentDateTime.Hour);
                if (currentDateTime.Hour > m_StartHours)
                {
                    targetDateTime = targetDateTime.AddDays(-1);
                }
                TimeSpan differTimeSpan = targetDateTime - baseDateTime;
                currentDateHour = (int)differTimeSpan.TotalHours;

                // 跨り先
                currentPoint = new Point(CalculationTimePosition(currentDateHour, currentDateTime.Minute), m_DiagramDrawProperties[current.Seq - 1].DrawHeight);

                // 次位置設定
                drawPoint = new Point(GetTimePosition(nextDateTime), m_DiagramDrawProperties[next.Seq - 1].DrawHeight);

                // 列車名表示
                DrawTrainNoName(drawPoint, currentPoint, drawPoint, train);

                // 時単位線描画
                m_Graphics.DrawLine(trainPen, currentPoint, drawPoint);
            }
            else
            {
                // 描画開始位置取得
                Point currentPoint = new Point(GetTimePosition(currentDateTime), m_DiagramDrawProperties[current.Seq - 1].DrawHeight);

                // 次位置設定
                Point drawPoint = new Point(GetTimePosition(nextDateTime), m_DiagramDrawProperties[next.Seq - 1].DrawHeight);

                // 時単位線描画
                m_Graphics.DrawLine(trainPen, currentPoint, drawPoint);
            }

            // ロギング
            Logger.Debug("<<<<= DiagramDrawLibrary::DrawLineNextPoint(TrainProperty, Pen, StationTimeProperty, StationTimeProperty)");
        }
        #endregion

        #region 縮尺決定
        /// <summary>
        /// 縮尺決定
        /// </summary>
        /// <param name="baseHeight"></param>
        /// <param name="height"></param>
        /// <returns></returns>

        public static float DetermineScale(float baseHeight, float height)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramDrawLibrary::DetermineScale(float, float)");
            Logger.DebugFormat("baseHeight:[{0}]", baseHeight);
            Logger.DebugFormat("height    :[{0}]", height);

            // 結果設定
            float result = (baseHeight / height) * 100;

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DiagramDrawLibrary::DetermineScale(float, float)");

            // 返却
            return result;
        }
        #endregion
    }
}
