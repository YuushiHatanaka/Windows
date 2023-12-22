using log4net;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrainTimeTable.Common;
using TrainTimeTable.Property;
using TrainTimeTable.File.Core;
using System.Security.Cryptography;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Shapes;

namespace TrainTimeTable.File
{
    /// <summary>
    /// WinDiaFileクラス
    /// </summary>
    public class WinDiaFile : FileCore
    {
        /// <summary>
        /// WinDIAColor
        /// </summary>
        private List<Color> m_WinDIAColor = new List<Color>()
        {
            Color.FromArgb(  0,  0,255) ,	//  0:青
	        Color.FromArgb(  0,255,  0) ,	//  1:緑
	        Color.FromArgb(  0,  0,132) ,	//  2:暗い青
	        Color.FromArgb(  0,130,  0) ,	//  3:暗い緑
	        Color.FromArgb(  0,255,255) ,	//  4:
	        Color.FromArgb(255,  0,  0) ,	//  5:赤
	        Color.FromArgb(  0,130,132) ,	//  6:
	        Color.FromArgb(132,  0,  0) ,	//  7:
	        Color.FromArgb(255,  0,255) ,	//  8:
	        Color.FromArgb(255,255,  0) ,	//  9:黄
	        Color.FromArgb(132,  0,132) ,	// 10:
	        Color.FromArgb(132,130,  0) ,	// 11:
	        Color.FromArgb(  0,  0,  0) ,	// 12:黒
	        Color.FromArgb(132,130,132) ,	// 13:
	        Color.FromArgb(198,195,198) ,	// 14:
	        Color.FromArgb(255,255,255)  	// 15:白
        };

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileName"></param>
        public WinDiaFile(string fileName)
            : base(fileName)
        {
            // ロギング
            Logger.Debug("=>>>> WinDiaFile::WinDiaFile(string)");
            Logger.DebugFormat("fileName:[{0}]", fileName);

            // 列車種別デフォルト設定
            m_RouteFileProperty.TrainTypes = new TrainTypeProperties()
            {
                { new TrainTypeProperty(){ Seq =  1, Name = "普通"    , Abbreviation = ""    , StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = false }},
                { new TrainTypeProperty(){ Seq =  2, Name = "快速"    , Abbreviation = "快速", StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = false }},
                { new TrainTypeProperty(){ Seq =  3, Name = "特別快速", Abbreviation = "特快", StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = false }},
                { new TrainTypeProperty(){ Seq =  4, Name = "新快速"  , Abbreviation = "新快", StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = false }},
                { new TrainTypeProperty(){ Seq =  5, Name = "通勤快速", Abbreviation = "通快", StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = false }},
                { new TrainTypeProperty(){ Seq =  6, Name = "準急"    , Abbreviation = "準急", StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = true  }},
                { new TrainTypeProperty(){ Seq =  7, Name = "急行"    , Abbreviation = "急行", StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = true  }},
                { new TrainTypeProperty(){ Seq =  8, Name = "快速急行", Abbreviation = "快急", StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = true  }},
                { new TrainTypeProperty(){ Seq =  9, Name = "特急"    , Abbreviation = "特急", StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = true  }},
                { new TrainTypeProperty(){ Seq = 10, Name = "快速特急", Abbreviation = "快特", StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = true  }},
                { new TrainTypeProperty(){ Seq = 11, Name = "通勤準急", Abbreviation = "通準", StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = true  }},
                { new TrainTypeProperty(){ Seq = 12, Name = "通勤急行", Abbreviation = "通急", StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = true  }},
                { new TrainTypeProperty(){ Seq = 13, Name = "区間快速", Abbreviation = "区快", StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = false }},
                { new TrainTypeProperty(){ Seq = 14, Name = "区間急行", Abbreviation = "区急", StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = true  }},
                { new TrainTypeProperty(){ Seq = 15, Name = "回送"    , Abbreviation = "回送", StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = false }},
                { new TrainTypeProperty(){ Seq = 16, Name = "貨物"    , Abbreviation = "貨物", StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Dash ,  DiagramLineBold = false }},
                { new TrainTypeProperty(){ Seq = 17, Name = "急行貨物", Abbreviation = "急貨", StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Dash ,  DiagramLineBold = true  }},
                { new TrainTypeProperty(){ Seq = 18, Name = ""        , Abbreviation = ""    , StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = false }},
                { new TrainTypeProperty(){ Seq = 19, Name = ""        , Abbreviation = ""    , StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = false }},
                { new TrainTypeProperty(){ Seq = 20, Name = ""        , Abbreviation = ""    , StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = false }},
                { new TrainTypeProperty(){ Seq = 21, Name = ""        , Abbreviation = ""    , StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = false }},
                { new TrainTypeProperty(){ Seq = 22, Name = ""        , Abbreviation = ""    , StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = false }},
                { new TrainTypeProperty(){ Seq = 23, Name = ""        , Abbreviation = ""    , StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = false }},
                { new TrainTypeProperty(){ Seq = 24, Name = ""        , Abbreviation = ""    , StringsColor = m_WinDIAColor[12], DiagramLineColor = m_WinDIAColor[12], DiagramLineStyle = DashStyle.Solid,  DiagramLineBold = false }},
            };

            // 列車種別シーケンスデフォルト設定
            int seq = 1;
            foreach (var trainType in m_RouteFileProperty.TrainTypes)
            {
                m_RouteFileProperty.TrainTypeSequences.Add(new TrainTypeSequenceProperty() { Name = trainType.Name, Seq = seq++ });
            }

            // DiagramPropertyオブジェクト生成
            DiagramProperty diagramProperty = new DiagramProperty() { Name = "", Seq = 1 };

            // DiagramSequencePropertyオブジェクト生成
            DiagramSequenceProperty diagramSequenceProperty = new DiagramSequenceProperty() { Name = "", Seq = 1 };

            // 1件分登録(WinDIAは1件のみなので固定)
            m_RouteFileProperty.Diagrams.Add(diagramProperty);
            m_RouteFileProperty.DiagramSequences.Add(diagramSequenceProperty);

            // インポートファイル種別設定
            m_RouteFileProperty.FileInfo.ImportFileType = "WinDIA";

            // ロギング
            Logger.Debug("<<<<= WinDiaFile::WinDiaFile(string)");
        }
        #endregion

        #region 1行処理
        /// <summary>
        /// 1行処理
        /// </summary>
        /// <param name="line"></param>
        protected override bool ProcessOneLineAtTime(string line)
        {
            // ロギング
            Logger.Debug("=>>>> WinDiaFile::ProcessOneLineAtTime(string)");
            Logger.DebugFormat("line:[{0}]", line);

            // 先頭文字判定
            if (line.Substring(0, 1) == "[")
            {
                // セクション登録
                m_CurrentSection = line;

                // ロギング
                Logger.DebugFormat("m_CurrentSection:[{0}]", m_CurrentSection);
                Logger.Debug("<<<<= WinDiaFile::ProcessOneLineAtTime(string)");

                // 正常終了
                return true;
            }

            // セクション毎に分岐
            switch (m_CurrentSection)
            {
                case "[WinDIA]":
                    // 設定
                    SetWinDIASection(line);
                    break;
                case "[駅]":
                    // 設定
                    SetStationSection(line);
                    break;
                case "[線種]":
                    // 設定
                    SetLineTypeSection(line);
                    break;
                case "[下り]":
                    // 設定
                    SetOutboundSection(line);
                    break;
                case "[上り]":
                    // 設定
                    SetInboundSection(line);
                    break;
                default:
                    // フォーマット異常
                    throw new FormatException(string.Format("WinDIAファイルのセクション種別異常を検出しました:[{0}]", m_CurrentSection));
            }

            // ロギング
            Logger.Debug("<<<<= WinDiaFile::ProcessOneLineAtTime(string)");

            // 正常終了
            return true;
        }
        #endregion

        #region セクション毎処理
        /// <summary>
        /// SetWinDIASection
        /// </summary>
        /// <param name="line"></param>
        private void SetWinDIASection(string line)
        {
            // ロギング
            Logger.Debug("=>>>> WinDiaFile::SetWinDIASection(string)");
            Logger.DebugFormat("line:[{0}]", line);

            // 設定
            m_RouteFileProperty.FileInfo.RouteName = line;
            m_RouteFileProperty.Route.Name = line;
            m_RouteFileProperty.Diagrams[0].Name = line;
            m_RouteFileProperty.DiagramSequences[0].Name = line;

            // ロギング
            Logger.Debug("<<<<= WinDiaFile::SetWinDIASection(string)");
        }

        /// <summary>
        /// SetStationSection
        /// </summary>
        /// <param name="line"></param>
        private void SetStationSection(string line)
        {
            // ロギング
            Logger.Debug("=>>>> WinDiaFile::SetStationSection(string)");
            Logger.DebugFormat("line:[{0}]", line);

            // TextReaderオブジェクト生成
            TextReader textReader = new StringReader(line);

            // TextFieldParserオブジェクト生成
            using (TextFieldParser textFieldParser = new TextFieldParser(textReader))
            {
                // CSV形式解析
                textFieldParser.TextFieldType = FieldType.Delimited;
                textFieldParser.SetDelimiters(",");

                // 要素毎に繰り返す
                while (!textFieldParser.EndOfData)
                {
                    // StationSequencePropertyオブジェクト生成
                    StationSequenceProperty stationSequenceProperty = new StationSequenceProperty();
                    stationSequenceProperty.Seq = m_RouteFileProperty.StationSequences.Count + 1;

                    // StationPropertyオブジェクト生成
                    StationProperty stationProperty = new StationProperty();
                    stationProperty.Seq = m_RouteFileProperty.Stations.Count + 1;
                    stationProperty.StationScale = StationScale.GeneralStation;
                    stationProperty.TimeFormat = TimeFormat.DepartureTime;
                    stationProperty.NextStations.Add(new NextStationProperty() { NextStationSeq = stationProperty.Seq + 1, Direction = DirectionType.Outbound });

                    // カラム取得
                    string[] columns = textFieldParser.ReadFields();

                    // カラム数判定
                    if (columns.Length != 2)
                    {
                        // フォーマット異常
                        throw new FormatException(string.Format("WinDiaファイルの駅フォーマットに異常を検出しました:[{0}]", columns[1]));
                    }

                    // 属性設定
                    if (columns[0].Contains("p"))
                    {
                        stationProperty.StationScale = StationScale.MainStation;
                    }
                    if (columns[0].Contains("b"))
                    {
                        stationProperty.TimeFormat = TimeFormat.DepartureAndArrival;
                    }
                    else if (columns[0].Contains("d"))
                    {
                        stationProperty.TimeFormat = TimeFormat.OutboundArrivalTime;
                    }
                    else if (columns[0].Contains("u"))
                    {
                        stationProperty.TimeFormat = TimeFormat.InboundArrivalTime;
                    }

                    // 駅名抽出
                    Regex reg = new Regex("(?<distance>[0-9\\. ]*)(?<name>.*)");
                    Match match = reg.Match(columns[1]);

                    // 駅名取得成功？
                    if (match.Success == true)
                    {
                        // 駅名設定
                        stationProperty.Name = Regex.Replace(match.Groups["name"].Value.Trim(), "[ |　]*", "");
                        stationProperty.NextStations[stationProperty.NextStations.Count - 1].Name = stationProperty.Name;
                        stationSequenceProperty.Name = stationProperty.Name;

                        // 距離判定
                        float distance = 0.0f;
                        if (float.TryParse(match.Groups["distance"].Value.Trim(), out distance))
                        {
                            // 距離設定
                            stationProperty.StationDistanceFromReferenceStations[DirectionType.Outbound] = distance;
                        }
                        else
                        {
                            // 距離設定
                            stationProperty.StationDistanceFromReferenceStations[DirectionType.Outbound] = 0.0f;
                        }

                        // 登録
                        m_RouteFileProperty.Stations.Add(stationProperty);
                        m_RouteFileProperty.StationSequences.Add(stationSequenceProperty);
                    }
                    else
                    {
                        // フォーマット異常
                        throw new FormatException(string.Format("WinDiaファイルの駅名フォーマットに異常を検出しました:[{0}]", columns[1]));
                    }
                }
            }

            // ロギング
            Logger.Debug("<<<<= WinDiaFile::SetStationSection(string)");
        }

        /// <summary>
        /// SetLineTypeSection
        /// </summary>
        /// <param name="line"></param>
        private void SetLineTypeSection(string line)
        {
            // ロギング
            Logger.Debug("=>>>> WinDiaFile::SetLineTypeSection(string)");
            Logger.DebugFormat("line:[{0}]", line);

            // 行解析
            Regex reg = new Regex("(?<key>^.*)=(?<value>.*)");
            Match match = reg.Match(line);

            // 一致結果判定
            if (match.Success == true)
            {
                // 分解
                string key = match.Groups["key"].Value;
                string value = match.Groups["value"].Value;

                // キーで分岐
                switch (key)
                {
                    case "LINES":
                        SetLineTypeLinesSection(key, value);
                        break;
                    default:
                        SetLineTypeTrainSection(key, value);
                        break;
                }
            }
            else
            {
                // フォーマット異常
                throw new FormatException(string.Format("WinDiaファイルのLINESフォーマットに異常を検出しました:[{0}]", line));
            }

            // ロギング
            Logger.Debug("<<<<= WinDiaFile::SetLineTypeSection(string)");
        }

        /// <summary>
        /// SetLineTypeLinesSection
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void SetLineTypeLinesSection(string key, string value)
        {
            // ロギング
            Logger.Debug("=>>>> WinDiaFile::SetLineTypeLinesSection(string, string)");
            Logger.DebugFormat("key  :[{0}]", key);
            Logger.DebugFormat("value:[{0}]", value);

            // TextReaderオブジェクト生成
            TextReader textReader = new StringReader(value);

            // TextFieldParserオブジェクト生成
            using (TextFieldParser textFieldParser = new TextFieldParser(textReader))
            {
                // CSV形式解析
                textFieldParser.TextFieldType = FieldType.Delimited;
                textFieldParser.SetDelimiters(",");

                // 要素毎に繰り返す
                while (!textFieldParser.EndOfData)
                {
                    // カラム取得
                    string[] columns = textFieldParser.ReadFields();

                    // カラムを繰り返す
                    int i = 0;
                    foreach (var column in columns)
                    {
                        // TrainTypePropertyオブジェクト生成
                        TrainTypeProperty trainTypeProperty = new TrainTypeProperty();
                        trainTypeProperty.TimetableFontName = "時刻表ビュー 1";
                        trainTypeProperty.TimetableFontIndex = 0;

                        // 数値変換
                        uint iValue = uint.Parse(column);

                        // 線種判定
                        switch ((iValue >> 0) & 0x3)
                        {
                            case 0:
                                trainTypeProperty.DiagramLineStyle = DashStyle.Solid;
                                break;
                            case 1:
                                trainTypeProperty.DiagramLineStyle = DashStyle.Dash;
                                break;
                            case 2:
                                trainTypeProperty.DiagramLineStyle = DashStyle.Dot;
                                break;
                            default:
                                trainTypeProperty.DiagramLineStyle = DashStyle.DashDot;
                                break;
                        }

                        // ダイヤグラム線スタイル(太線)
                        if (((iValue >> 7) & 0x1) > 0)
                        {
                            trainTypeProperty.DiagramLineBold = true;
                        }

                        if (((iValue >> 6) & 0x1) > 0)
                        {
                            uint iColorNum = (iValue >> 2) & 0x0f;
                            trainTypeProperty.StringsColor = m_WinDIAColor[(int)iColorNum];
                            trainTypeProperty.DiagramLineColor = m_WinDIAColor[(int)iColorNum];
                        }

                        // 更新
                        m_RouteFileProperty.TrainTypes[i].DiagramLineStyle = trainTypeProperty.DiagramLineStyle;
                        m_RouteFileProperty.TrainTypes[i].DiagramLineBold = trainTypeProperty.DiagramLineBold;
                        m_RouteFileProperty.TrainTypes[i].StringsColor = trainTypeProperty.StringsColor;
                        m_RouteFileProperty.TrainTypes[i].DiagramLineColor = trainTypeProperty.DiagramLineColor;
                        i++;
                    }
                }
            }

            // ロギング
            Logger.Debug("<<<<= WinDiaFile::SetLineTypeLinesSection(string, string)");
        }

        /// <summary>
        /// SetLineTypeTrainSection
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void SetLineTypeTrainSection(string key, string value)
        {
            // ロギング
            Logger.Debug("=>>>> WinDiaFile::SetLineTypeLinesSection(string, string)");
            Logger.DebugFormat("key  :[{0}]", key);
            Logger.DebugFormat("value:[{0}]", value);

            // 行解析
            Regex reg = new Regex("(?<key>^.*)(?<value>[0-9][0-9])");
            Match match = reg.Match(key);

            // 一致結果判定
            if (match.Success == true)
            {
                // 分解
                int index = int.Parse(match.Groups["value"].Value);

                // TextReaderオブジェクト生成
                TextReader textReader = new StringReader(value);

                // TextFieldParserオブジェクト生成
                using (TextFieldParser textFieldParser = new TextFieldParser(textReader))
                {
                    // CSV形式解析
                    textFieldParser.TextFieldType = FieldType.Delimited;
                    textFieldParser.SetDelimiters(",");

                    // 要素毎に繰り返す
                    while (!textFieldParser.EndOfData)
                    {
                        // カラム取得
                        string[] columns = textFieldParser.ReadFields();

                        // 同一名称がいるか？
                        List<TrainTypeProperty> trainTypePropertyFindAll = m_RouteFileProperty.TrainTypes.FindAll(x => x.Name == columns[0] && x.Seq != index + 1);
                        if (trainTypePropertyFindAll.Count > 0)
                        {
                            // 同一種別がいたらクリアする
                            foreach (var item in trainTypePropertyFindAll)
                            {
                                item.Name = string.Empty;
                            }
                        }

                        // 設定
                        m_RouteFileProperty.TrainTypes[index].Seq = index + 1;
                        m_RouteFileProperty.TrainTypes[index].Name = columns[0];
                        m_RouteFileProperty.TrainTypes[index].Abbreviation = columns[1];
                        m_RouteFileProperty.TrainTypeSequences[index].Name = columns[0];
                    }
                }
            }
            else
            {
                // フォーマット異常
                throw new FormatException(string.Format("WinDiaファイルのTrainフォーマットに異常を検出しました:[{0}]", key));
            }

            // ロギング
            Logger.Debug("<<<<= WinDiaFile::SetLineTypeLinesSection(string, string)");
        }

        /// <summary>
        /// SetOutboundSection
        /// </summary>
        /// <param name="line"></param>
        private void SetOutboundSection(string line)
        {
            // ロギング
            Logger.Debug("=>>>> WinDiaFile::SetOutboundSection(string)");
            Logger.DebugFormat("line:[{0}]", line);

            // TrainPropertyオブジェクト生成
            TrainProperty trainProperty = new TrainProperty();
            trainProperty.DiagramId = 0;
            trainProperty.Id = m_RouteFileProperty.Diagrams[0].Trains[DirectionType.Outbound].Count + 1;
            trainProperty.Seq = m_RouteFileProperty.Diagrams[0].Trains[DirectionType.Outbound].Count + 1;
            trainProperty.Direction = DirectionType.Outbound;

            // TrainPropertyオブジェクト生成
            TrainSequenceProperty trainSequenceProperty = new TrainSequenceProperty();
            trainSequenceProperty.DiagramId = 0;
            trainSequenceProperty.Id = m_RouteFileProperty.Diagrams[0].TrainSequence[DirectionType.Outbound].GetNewId();
            trainSequenceProperty.Seq = m_RouteFileProperty.Diagrams[0].Trains[DirectionType.Outbound].Count + 1;
            trainSequenceProperty.Direction = DirectionType.Outbound;

            // 仮登録
            m_RouteFileProperty.Diagrams[0].Trains[DirectionType.Outbound].Add(trainProperty);
            m_RouteFileProperty.Diagrams[0].TrainSequence[DirectionType.Outbound].Add(trainSequenceProperty);

            // 設定
            SetTrainSection(m_RouteFileProperty.Stations, trainProperty, m_RouteFileProperty.Diagrams[0].Trains[DirectionType.Outbound].Count, line);

            // ロギング
            Logger.Debug("<<<<= WinDiaFile::SetOutboundSection(string)");
        }

        /// <summary>
        /// SetInboundSection
        /// </summary>
        /// <param name="line"></param>
        private void SetInboundSection(string line)
        {
            // ロギング
            Logger.Debug("=>>>> WinDiaFile::SetInboundSection(string)");
            Logger.DebugFormat("line:[{0}]", line);

            // TrainPropertyオブジェクト生成
            TrainProperty trainProperty = new TrainProperty();
            trainProperty.DiagramId = 0;
            trainProperty.Id = m_RouteFileProperty.Diagrams[0].Trains[DirectionType.Inbound].Count + 1;
            trainProperty.Seq = m_RouteFileProperty.Diagrams[0].Trains[DirectionType.Inbound].Count + 1;
            trainProperty.Direction = DirectionType.Inbound;

            // TrainPropertyオブジェクト生成
            TrainSequenceProperty trainSequenceProperty = new TrainSequenceProperty();
            trainSequenceProperty.DiagramId = 0;
            trainSequenceProperty.Id = m_RouteFileProperty.Diagrams[0].TrainSequence[DirectionType.Inbound].GetNewId();
            trainSequenceProperty.Seq = m_RouteFileProperty.Diagrams[0].Trains[DirectionType.Inbound].Count + 1;
            trainSequenceProperty.Direction = DirectionType.Inbound;

            // 仮登録
            m_RouteFileProperty.Diagrams[0].Trains[DirectionType.Inbound].Add(trainProperty);
            m_RouteFileProperty.Diagrams[0].TrainSequence[DirectionType.Inbound].Add(trainSequenceProperty);

            // 設定
            SetTrainSection(m_RouteFileProperty.Stations, trainProperty, m_RouteFileProperty.Diagrams[0].Trains[DirectionType.Inbound].Count, line);

            // ロギング
            Logger.Debug("<<<<= WinDiaFile::SetInboundSection(string)");
        }

        /// <summary>
        /// SetTrainSection
        /// </summary>
        /// <param name="stationProperties"></param>
        /// <param name="trainProperty"></param>
        /// <param name="trainId"></param>
        /// <param name="line"></param>
        private void SetTrainSection(StationProperties stationProperties, TrainProperty trainProperty, int trainId, string line)
        {
            // ロギング
            Logger.Debug("=>>>> WinDiaFile::SetInboundSection(StationProperties, TrainProperty, int, string)");
            Logger.DebugFormat("stationProperties:[{0}]", stationProperties);
            Logger.DebugFormat("trainProperty    :[{0}]", trainProperty);
            Logger.DebugFormat("trainId          :[{0}]", trainId);
            Logger.DebugFormat("line             :[{0}]", line);

            // TextReaderオブジェクト生成
            TextReader textReader = new StringReader(line);

            // TextFieldParserオブジェクト生成
            using (TextFieldParser textFieldParser = new TextFieldParser(textReader))
            {
                // CSV形式解析
                textFieldParser.TextFieldType = FieldType.Delimited;
                textFieldParser.SetDelimiters(",");

                // 要素毎に繰り返す
                while (!textFieldParser.EndOfData)
                {
                    // カラム取得
                    string[] columns = textFieldParser.ReadFields();

                    // カラム数分繰り返す
                    for (int i = 0; i < columns.Length; i++)
                    {
                        // カラム毎に分岐
                        switch (i)
                        {
                            // 列車種別番号
                            case 0:
                                {
                                    // ()付き番号は削除
                                    Regex reg = new Regex("\\(.*\\)");
                                    string trainType = reg.Replace(columns[i], "");
                                    if (trainType != string.Empty)
                                    {
                                        trainProperty.TrainType = int.Parse(trainType);
                                    }
                                }
                                break;
                            // 列車番号
                            case 1:
                                {
                                    trainProperty.No = columns[i];
                                }
                                break;
                            // 列車名
                            case 2:
                                {
                                    trainProperty.Name = columns[i];
                                }
                                break;
                            case 3:
                                // 列車号数
                                {
                                    trainProperty.Number = columns[i];
                                }
                                break;
                            default:
                                {
                                    // 行解析
                                    Regex reg = new Regex("(?<value>^.*)%(?<remarks>.*)");
                                    Match match = reg.Match(columns[i]);

                                    // 初期化
                                    string value = columns[i];
                                    string departureTime = string.Empty;
                                    string arrivalTime = string.Empty;

                                    // 一致結果判定
                                    if (match.Success == true)
                                    {
                                        trainProperty.Remarks = match.Groups["remarks"].Value;
                                        continue;
                                    }

                                    // StationTimePropertyオブジェクト生成
                                    StationTimeProperty stationTimeProperty = new StationTimeProperty();
                                    stationTimeProperty.DiagramId = 0;
                                    stationTimeProperty.TrainId = trainId;
                                    stationTimeProperty.Direction = trainProperty.Direction;
                                    stationTimeProperty.Seq = trainProperty.StationTimes.Count + 1;
                                    stationTimeProperty.StationName = stationProperties[trainProperty.StationTimes.Count].Name;
                                    stationTimeProperty.StationTreatment = StationTreatment.NoService;

                                    // 時刻判定
                                    if (value.IndexOf('/') >= 0)
                                    {
                                        // 時刻分解
                                        string[] stationTime = value.Split('/');

                                        // 発着刻設定
                                        arrivalTime = DateTimeLibrary.GetTimeString(stationTime[0]);
                                        departureTime = DateTimeLibrary.GetTimeString(stationTime[1]);
                                    }
                                    else
                                    {
                                        // 発時刻のみ設定
                                        departureTime = DateTimeLibrary.GetTimeString(value);
                                    }

                                    // 推定時刻か？
                                    if (departureTime.IndexOf("?") >= 0)
                                    {
                                        departureTime = departureTime.Replace("?", "");
                                        stationTimeProperty.EstimatedDepartureTime = true;
                                        stationTimeProperty.StationTreatment = StationTreatment.Stop;
                                    }
                                    if (arrivalTime.IndexOf("?") >= 0)
                                    {
                                        arrivalTime = arrivalTime.Replace("?", "");
                                        stationTimeProperty.EstimatedArrivalTime = true;
                                        stationTimeProperty.StationTreatment = StationTreatment.Stop;
                                    }

                                    // 発着時刻判定
                                    if (arrivalTime == "-" || departureTime == "-")
                                    {
                                        // 通過設定
                                        stationTimeProperty.StationTreatment = StationTreatment.Passing;
                                    }
                                    else if (arrivalTime != string.Empty || departureTime != string.Empty)
                                    {
                                        // 停車設定
                                        stationTimeProperty.StationTreatment = StationTreatment.Stop;
                                    }

                                    // 時刻設定
                                    stationTimeProperty.ArrivalTime = DateTimeLibrary.GetTimeString(arrivalTime);
                                    stationTimeProperty.DepartureTime = DateTimeLibrary.GetTimeString(departureTime);

                                    // 駅時刻登録
                                    trainProperty.StationTimes.Add(stationTimeProperty);
                                }
                                break;
                        }
                    }
                }
            }

            // ロギング
            Logger.Debug("<<<<= WinDiaFile::SetInboundSection(StationProperties, TrainProperty, int, string)");
        }
        #endregion
    }
}
