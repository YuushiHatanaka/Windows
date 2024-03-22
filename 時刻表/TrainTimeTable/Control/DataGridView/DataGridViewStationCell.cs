using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Common;
using TrainTimeTable.Property;
using static System.Net.Mime.MediaTypeNames;

namespace TrainTimeTable.Control
{
    public class DataGridViewStationCell : TableLayoutPanel
    {
        /// <summary>
        /// 方向種別
        /// </summary>
        private DirectionType m_DirectionType = DirectionType.None;

        /// <summary>
        /// RouteFilePropertyオブジェクト
        /// </summary>
        private RouteFileProperty m_RouteFileProperty = null;

        private StationProperty m_StationProperty = null;

        private Label m_StationNameLabel = null;

        public DataGridViewStationCell(DirectionType type, RouteFileProperty routeFileProperty, StationProperty property)
        {
            m_DirectionType = type;
            m_RouteFileProperty = routeFileProperty;
            m_StationProperty = property;
        }

        public void Draw()
        {
            // 全角空白のサイズを取得
            SizeF sizef = new SizeF();
            using (var g = this.CreateGraphics())
            {
                sizef = g.MeasureString("あ", this.Font);
            }

            ColumnStyles.Add(new ColumnStyle() { SizeType = SizeType.Absolute, Width = 20.0f });
            ColumnStyles.Add(new ColumnStyle() { SizeType = SizeType.Absolute, Width = 64.0f });
            ColumnStyles.Add(new ColumnStyle() { SizeType = SizeType.Absolute, Width = 32.0f });
            ColumnStyles.Add(new ColumnStyle() { SizeType = SizeType.Absolute, Width = ((float)m_RouteFileProperty.Route.WidthOfStationNameField * 2.0f) * sizef.Width });
            ColumnStyles.Add(new ColumnStyle() { SizeType = SizeType.Absolute, Width = 32.0f });

            Controls.Add(new Label() { Text = "xxxxx", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, 0, 0);
            Controls.Add(new Label() { Text = "0000.0", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, 1, 0);
            Controls.Add(new Label() { Text = "xxxxx", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, 2, 0);

            m_StationNameLabel = new Label();
            m_StationNameLabel.Text = m_StationProperty.Name;
            m_StationNameLabel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
            m_StationNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            Controls.Add(m_StationNameLabel, 3, 0);

            // 方向種別で分岐
            int columns = 4;
            switch (m_DirectionType)
            {
                case DirectionType.Outbound:
                    // 時刻形式で分岐
                    switch (m_StationProperty.TimeFormat)
                    {
                        case TimeFormat.DepartureTime:
                            // 設定
                            RowStyles.Add(new RowStyle() { SizeType = SizeType.Percent, Height = 50.0f });
                            Controls.Add(new Label() { Text = "発", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleCenter }, columns, 0);
                            SetRowSpan(m_StationNameLabel, 1);
                            Height = Font.Height;
                            break;
                        case TimeFormat.DepartureAndArrival:
                            // 設定
                            RowStyles.Add(new RowStyle() { SizeType = SizeType.Percent, Height = 50.0f });
                            RowStyles.Add(new RowStyle() { SizeType = SizeType.Percent, Height = 50.0f });
                            Controls.Add(new Label() { Text = "着", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleCenter }, columns, 0);
                            Controls.Add(new Label() { Text = "発", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleCenter }, columns, 1);
                            SetRowSpan(m_StationNameLabel, 2);
                            Height = Font.Height * 2;
                            break;
                        case TimeFormat.OutboundArrivalTime:
                            // 設定
                            RowStyles.Add(new RowStyle() { SizeType = SizeType.Percent, Height = 50.0f });
                            Controls.Add(new Label() { Text = "着", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleCenter }, columns, 0);
                            SetRowSpan(m_StationNameLabel, 1);
                            Height = Font.Height;
                            break;
                        case TimeFormat.InboundArrivalTime:
                            // 設定
                            RowStyles.Add(new RowStyle() { SizeType = SizeType.Percent, Height = 50.0f });
                            Controls.Add(new Label() { Text = "発", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleCenter }, columns, 0);
                            SetRowSpan(m_StationNameLabel, 1);
                            Height = Font.Height;
                            break;
                        case TimeFormat.OutboundArrivalAndDeparture:
                            // 設定
                            RowStyles.Add(new RowStyle() { SizeType = SizeType.Percent, Height = 50.0f });
                            RowStyles.Add(new RowStyle() { SizeType = SizeType.Percent, Height = 50.0f });
                            Controls.Add(new Label() { Text = "着", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleCenter },             columns, 0);
                            Controls.Add(new Label() { Text = "発", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleCenter }, columns, 1);
                            SetRowSpan(m_StationNameLabel, 2);
                            Height = Font.Height * 2;
                            break;
                        case TimeFormat.InboundDepartureAndArrival:
                            // 設定
                            RowStyles.Add(new RowStyle() { SizeType = SizeType.Percent, Height = 50.0f });
                            RowStyles.Add(new RowStyle() { SizeType = SizeType.Percent, Height = 50.0f });
                            Controls.Add(new Label() { Text = "着", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleCenter }, columns, 0);
                            Controls.Add(new Label() { Text = "発", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleCenter }, columns, 1);
                            SetRowSpan(m_StationNameLabel, 2);
                            Height = Font.Height * 2;
                            break;
                        default:
                            throw new AggregateException(string.Format("時刻形式の異常を検出しました:[{0}]", m_StationProperty.TimeFormat));
                    }
                    break;
                case DirectionType.Inbound:
                    // 時刻形式で分岐
                    switch (m_StationProperty.TimeFormat)
                    {
                        case TimeFormat.DepartureTime:
                            // 設定
                            RowStyles.Add(new RowStyle() { SizeType = SizeType.Percent, Height = 50.0f });
                            Controls.Add(new Label() { Text = "発", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleCenter }, columns, 0);
                            SetRowSpan(m_StationNameLabel, 1);
                            Height = Font.Height;
                            break;
                        case TimeFormat.DepartureAndArrival:
                            // 設定
                            RowStyles.Add(new RowStyle() { SizeType = SizeType.Percent, Height = 50.0f });
                            RowStyles.Add(new RowStyle() { SizeType = SizeType.Percent, Height = 50.0f });
                            Controls.Add(new Label() { Text = "着", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleCenter }, columns, 0);
                            Controls.Add(new Label() { Text = "発", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleCenter }, columns, 1);
                            SetRowSpan(m_StationNameLabel, 2);
                            Height = Font.Height * 2;
                            break;
                        case TimeFormat.OutboundArrivalTime:
                            // 設定
                            RowStyles.Add(new RowStyle() { SizeType = SizeType.Percent, Height = 50.0f });
                            Controls.Add(new Label() { Text = "発", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleCenter }, columns, 0);
                            SetRowSpan(m_StationNameLabel, 1);
                            Height = Font.Height;
                            break;
                        case TimeFormat.InboundArrivalTime:
                            // 設定
                            RowStyles.Add(new RowStyle() { SizeType = SizeType.Percent, Height = 50.0f });
                            Controls.Add(new Label() { Text = "着", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleCenter }, columns, 0);
                            SetRowSpan(m_StationNameLabel, 1);
                            Height = Font.Height;
                            break;
                        case TimeFormat.OutboundArrivalAndDeparture:
                            // 設定
                            RowStyles.Add(new RowStyle() { SizeType = SizeType.Percent, Height = 50.0f });
                            RowStyles.Add(new RowStyle() { SizeType = SizeType.Percent, Height = 50.0f });
                            Controls.Add(new Label() { Text = "着", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleCenter }, columns, 0);
                            Controls.Add(new Label() { Text = "発", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleCenter }, columns, 1);
                            SetRowSpan(m_StationNameLabel, 2);
                            Height = Font.Height * 2;
                            break;
                        case TimeFormat.InboundDepartureAndArrival:
                            // 設定
                            RowStyles.Add(new RowStyle() { SizeType = SizeType.Percent, Height = 50.0f });
                            RowStyles.Add(new RowStyle() { SizeType = SizeType.Percent, Height = 50.0f });
                            Controls.Add(new Label() { Text = "着", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleCenter }, columns, 0);
                            Controls.Add(new Label() { Text = "発", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleCenter }, columns, 1);
                            SetRowSpan(m_StationNameLabel, 2);
                            Height = Font.Height * 2;
                            break;
                        default:
                            throw new AggregateException(string.Format("時刻形式の異常を検出しました:[{0}]", m_StationProperty.TimeFormat));
                    }
                    break;
                default:
                    throw new AggregateException(string.Format("方向種別の異常を検出しました:[{0}]", m_DirectionType));
            }
        }
    }
}
