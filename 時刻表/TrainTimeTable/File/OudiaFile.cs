using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TrainTimeTable.Common;
using TrainTimeTable.Property;
using TrainTimeTable.File.Core;

namespace TrainTimeTable.File
{
    /// <summary>
    /// OudiaFileクラス
    /// </summary>
    public class OudiaFile : FileCore
    {
        #region フォントインデックス
        /// <summary>
        /// フォントインデックス
        /// </summary>
        protected int m_FontIndex = 1;
        #endregion

        #region 方向種別
        /// <summary>
        /// 方向種別
        /// </summary>
        protected DirectionType m_CurrentDirectionType = DirectionType.None;
        #endregion

        #region ダイアグラムインデックス
        /// <summary>
        /// ダイアグラムID
        /// </summary>
        protected int m_DiagramId = -1;

        /// <summary>
        /// ダイアグラムシーケンスID
        /// </summary>
        protected int m_DiagramSequenceId = -1;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileName"></param>
        public OudiaFile(string fileName)
            : base(fileName)
        {
            // ロギング
            Logger.Debug("=>>>> OudiaFile::OudiaFile(string)");
            Logger.DebugFormat("fileName:[{0}]", fileName);

            // ロギング
            Logger.Debug("<<<<= OudiaFile::OudiaFile(string)");
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
            Logger.Debug("=>>>> OudiaFile::ProcessOneLineAtTime(string)");
            Logger.DebugFormat("line:[{0}]", line);

            // 行で分岐
            switch (line)
            {
                case ".":
                    // セクション初期化
                    m_CurrentSection = string.Empty;

                    // ロギング
                    Logger.Debug("<<<<= OudFile::ProcessOneLineAtTime(string)");

                    // 正常終了
                    return true;
                case "Rosen.":
                case "Eki.":
                case "Ressyasyubetsu.":
                case "Dia.":
                case "Kudari.":
                case "Nobori.":
                case "Ressya.":
                case "DispProp.":
                    // セクション登録
                    m_CurrentSection = line;

                    // セクション分岐
                    BranchBySection(line);

                    // ロギング
                    Logger.Debug("<<<<= OudFile::ProcessOneLineAtTime(string)");

                    // 正常終了
                    return true;
                default:
                    break;
            }

            // セクション分岐
            BranchBySection(line);

            // ロギング
            Logger.Debug("<<<<= OudiaFile::ProcessOneLineAtTime(string)");

            // 正常終了
            return true;
        }
        #endregion

        #region セクション毎処理
        /// <summary>
        /// セクション分岐
        /// </summary>
        /// <param name="line"></param>
        protected virtual void BranchBySection(string line)
        {
            // ロギング
            Logger.Debug("=>>>> OudFile::BranchBySection(string)");
            Logger.DebugFormat("line:[{0}]", line);

            // セクション毎に分岐
            switch (m_CurrentSection)
            {
                case "Rosen.":
                    // 設定
                    SetRosenSection(line);
                    break;
                case "Eki.":
                    // 設定
                    SetEkiSection(line);
                    break;
                case "Ressyasyubetsu.":
                    // 設定
                    SetRessyasyubetsuSection(line);
                    break;
                case "Dia.":
                    // 設定
                    SetDiaSection(line);
                    break;
                case "Kudari.":
                    // 設定
                    SetKudariSection(line);
                    break;
                case "Nobori.":
                    // 設定
                    SetNoboriSection(line);
                    break;
                case "Ressya.":
                    // 設定
                    SetRessyaSection(line);
                    break;
                case "DispProp.":
                    // 設定
                    SetDispPropSection(line);
                    break;
                default:
                    // 設定
                    SetDefaultSection(line);
                    break;
            }

            // ロギング
            Logger.Debug("<<<<= OudFile::BranchBySection(string)");
        }

        /// <summary>
        /// SetRosenSection
        /// </summary>
        /// <param name="line"></param>
        protected virtual void SetRosenSection(string line)
        {
            // ロギング
            Logger.Debug("=>>>> OudFile::SetRosenSection(string)");
            Logger.DebugFormat("line:[{0}]", line);

            // '='で分割
            string[] keyValue = line.Split('=');

            // 分割サイズ判定
            if (keyValue.Length != 2)
            {
                // ロギング
                Logger.Debug("<<<<= OudFile::SetRosenSection(string)");
                return;
            }

            // キー分岐
            switch (keyValue[0])
            {
                case "Rosenmei":
                    // 「路線名」設定
                    m_RouteFileProperty.Route.Name = keyValue[1];
                    m_RouteFileProperty.FileInfo.RouteName = keyValue[1];
                    break;
                default:
                    break;
            }

            // ロギング
            Logger.Debug("<<<<= OudFile::SetRosenSection(string)");
        }

        /// <summary>
        /// SetEkiSection
        /// </summary>
        /// <param name="line"></param>
        protected virtual void SetEkiSection(string line)
        {
            // ロギング
            Logger.Debug("=>>>> OudFile::SetEkiSection(string)");
            Logger.DebugFormat("line:[{0}]", line);

            // 行で分岐
            switch (line)
            {
                case "Eki.":
                    // StationSequencePropertyオブジェクト生成
                    StationSequenceProperty stationSequenceProperty = new StationSequenceProperty();
                    stationSequenceProperty.Seq = m_RouteFileProperty.StationSequences.Count + 1;
                    // StationPropertyオブジェクト生成
                    StationProperty stationProperty = new StationProperty();
                    stationProperty.Seq = m_RouteFileProperty.Stations.Count + 1;
                    stationProperty.NextStations.Add(new NextStationProperty() { NextStationSeq = stationProperty.Seq + 1, Direction = DirectionType.Outbound });
                    // 仮登録
                    m_RouteFileProperty.Stations.Add(stationProperty);
                    m_RouteFileProperty.StationSequences.Add(stationSequenceProperty);
                    // ロギング
                    Logger.Debug("<<<<= OudFile::SetEkiSection(string)");
                    return;
                case ".":
                    // ロギング
                    Logger.Debug("<<<<= OudFile::SetEkiSection(string)");
                    return;
                default:
                    break;
            }

            // '='で分割
            string[] keyValue = line.Split('=');

            // 分割サイズ判定
            if (keyValue.Length != 2)
            {
                // ロギング
                Logger.WarnFormat("駅行不正:[{0}]", line);
                Logger.Debug("<<<<= OudFile::SetEkiSection(string)");
                return;
            }

            // 配列インデックス取得
            int stationsArrayIndex = m_RouteFileProperty.Stations.Count - 1;
            int stationSequenceArrayIndex = m_RouteFileProperty.StationSequences.Count - 1;

            // キー分岐
            switch (keyValue[0])
            {
                case "Ekimei":
                    // 設定
                    m_RouteFileProperty.Stations[stationsArrayIndex].Name = keyValue[1];
                    m_RouteFileProperty.Stations[stationsArrayIndex].NextStations[0].Name = m_RouteFileProperty.Stations[stationsArrayIndex].Name;
                    m_RouteFileProperty.StationSequences[stationSequenceArrayIndex].Name = keyValue[1];
                    break;
                case "Ekijikokukeisiki":
                    switch (keyValue[1])
                    {
                        case "Jikokukeisiki_KudariChaku":
                            m_RouteFileProperty.Stations[stationsArrayIndex].TimeFormat = TimeFormat.OutboundArrivalTime;
                            break;
                        case "Jikokukeisiki_NoboriChaku":
                            m_RouteFileProperty.Stations[stationsArrayIndex].TimeFormat = TimeFormat.InboundArrivalTime;
                            break;
                        case "Jikokukeisiki_Hatsu":
                            m_RouteFileProperty.Stations[stationsArrayIndex].TimeFormat = TimeFormat.DepartureTime;
                            break;
                        case "Jikokukeisiki_Hatsuchaku":
                            m_RouteFileProperty.Stations[stationsArrayIndex].TimeFormat = TimeFormat.DepartureAndArrival;
                            break;
                        default:
                            // フォーマット異常
                            throw new FormatException(string.Format("OudiaファイルのEkijikokukeisikiに異常を検出しました:[{0}][{1}]", keyValue[0], keyValue[1]));
                    }
                    break;
                case "Ekikibo":
                    switch (keyValue[1])
                    {
                        case "Ekikibo_Ippan":
                            m_RouteFileProperty.Stations[stationsArrayIndex].StationScale = StationScale.GeneralStation;
                            break;
                        case "Ekikibo_Syuyou":
                            m_RouteFileProperty.Stations[stationsArrayIndex].StationScale = StationScale.MainStation;
                            break;
                        default:
                            // フォーマット異常
                            throw new FormatException(string.Format("OudiaファイルのEkikiboに異常を検出しました:[{0}][{1}]", keyValue[0], keyValue[1]));
                    }
                    break;
                case "Kyoukaisen":
                    if (keyValue[1] == "1")
                    {
                        m_RouteFileProperty.Stations[stationsArrayIndex].Border = true;
                    }
                    break;
                case "DiagramRessyajouhouHyoujiKudari":
                    switch (keyValue[1])
                    {
                        case "DiagramRessyajouhouHyouji_Anytime":
                            m_RouteFileProperty.Stations[stationsArrayIndex].DiagramTrainInformations[DirectionType.Outbound] = DiagramTrainInformation.AlwaysVisible;
                            break;
                        case "DiagramRessyajouhouHyouji_Not":
                            m_RouteFileProperty.Stations[stationsArrayIndex].DiagramTrainInformations[DirectionType.Outbound] = DiagramTrainInformation.DoNotShow;
                            break;
                        default:
                            m_RouteFileProperty.Stations[stationsArrayIndex].DiagramTrainInformations[DirectionType.Outbound] = DiagramTrainInformation.DisplayIfItIsTheFirstTrain;
                            break;
                    }
                    break;
                case "DiagramRessyajouhouHyoujiNobori":
                    switch (keyValue[1])
                    {
                        case "DiagramRessyajouhouHyouji_Anytime":
                            m_RouteFileProperty.Stations[stationsArrayIndex].DiagramTrainInformations[DirectionType.Inbound] = DiagramTrainInformation.AlwaysVisible;
                            break;
                        case "DiagramRessyajouhouHyouji_Not":
                            m_RouteFileProperty.Stations[stationsArrayIndex].DiagramTrainInformations[DirectionType.Inbound] = DiagramTrainInformation.DoNotShow;
                            break;
                        default:
                            m_RouteFileProperty.Stations[stationsArrayIndex].DiagramTrainInformations[DirectionType.Inbound] = DiagramTrainInformation.DisplayIfItIsTheFirstTrain;
                            break;
                    }
                    break;
                default:
                    // フォーマット異常
                    throw new FormatException(string.Format("OudiaファイルのEkiセクションに異常を検出しました:[{0}][{1}]", keyValue[0], keyValue[1]));
            }

            // ロギング
            Logger.Debug("<<<<= OudFile::SetEkiSection(string)");
        }

        /// <summary>
        /// SetRessyasyubetsuSection
        /// </summary>
        /// <param name="line"></param>
        protected virtual void SetRessyasyubetsuSection(string line)
        {
            // ロギング
            Logger.Debug("=>>>> OudFile::SetRessyasyubetsuSection(string)");
            Logger.DebugFormat("line:[{0}]", line);

            // 行で分岐
            switch (line)
            {
                case "Ressyasyubetsu.":
                    // TrainTypePropertyオブジェクト生成
                    TrainTypeProperty trainTypeProperty = new TrainTypeProperty();
                    trainTypeProperty.Seq = m_RouteFileProperty.TrainTypes.Count + 1;
                    // TrainTypeSequencePropertyオブジェクト生成
                    TrainTypeSequenceProperty trainTypeSequenceProperty = new TrainTypeSequenceProperty();
                    trainTypeSequenceProperty.Seq = m_RouteFileProperty.TrainTypeSequences.Count + 1;
                    // 仮登録
                    m_RouteFileProperty.TrainTypes.Add(trainTypeProperty);
                    m_RouteFileProperty.TrainTypeSequences.Add(trainTypeSequenceProperty);
                    // ロギング
                    Logger.Debug("<<<<= SetRessyasyubetsuSection::SetEkiSection(string)");
                    return;
                case ".":
                    // ロギング
                    Logger.Debug("<<<<= SetRessyasyubetsuSection::SetEkiSection(string)");
                    return;
                default:
                    break;
            }

            // '='で分割
            string[] keyValue = line.Split('=');

            // 分割サイズ判定
            if (keyValue.Length != 2)
            {
                // フォーマット異常
                throw new FormatException(string.Format("Oudiaファイルの列車種別行に異常を検出しました:[{0}]", line));
            }

            // 配列インデックス取得
            int trainTypesArrayIndex = m_RouteFileProperty.TrainTypes.Count - 1;
            int trainTypeSequencesArrayIndex = m_RouteFileProperty.TrainTypeSequences.Count - 1;

            // キー分岐
            switch (keyValue[0])
            {
                // 種別名
                case "Syubetsumei":
                    {
                        m_RouteFileProperty.TrainTypes[trainTypesArrayIndex].Name = keyValue[1];
                        m_RouteFileProperty.TrainTypeSequences[trainTypeSequencesArrayIndex].Name = keyValue[1];
                    }
                    break;
                // 略称
                case "Ryakusyou":
                    {
                        m_RouteFileProperty.TrainTypes[trainTypesArrayIndex].Abbreviation = keyValue[1];
                    }
                    break;
                // 時刻表文字色
                case "JikokuhyouMojiColor":
                    {
                        m_RouteFileProperty.TrainTypes[trainTypesArrayIndex].StringsColor = GetTimetableColor(keyValue).Value;
                    }
                    break;
                // 時刻表Fontインデックス
                case "JikokuhyouFontIndex":
                    {
                        m_RouteFileProperty.TrainTypes[trainTypesArrayIndex].TimetableFontIndex = int.Parse(keyValue[1]);
                    }
                    break;
                // ダイヤグラム線色
                case "DiagramSenColor":
                    {
                        m_RouteFileProperty.TrainTypes[trainTypesArrayIndex].DiagramLineColor = GetTimetableColor(keyValue).Value;
                    }
                    break;
                // ダイヤグラム線スタイル
                case "DiagramSenStyle":
                    {
                        // 線種文字で分岐
                        switch (keyValue[1])
                        {
                            case "SenStyle_Jissen":
                                m_RouteFileProperty.TrainTypes[trainTypesArrayIndex].DiagramLineStyle = DashStyle.Solid;
                                break;
                            case "SenStyle_Hasen":
                                m_RouteFileProperty.TrainTypes[trainTypesArrayIndex].DiagramLineStyle = DashStyle.Dash;
                                break;
                            case "SenStyle_Tensen":
                                m_RouteFileProperty.TrainTypes[trainTypesArrayIndex].DiagramLineStyle = DashStyle.Dot;
                                break;
                            case "SenStyle_Ittensasen":
                                m_RouteFileProperty.TrainTypes[trainTypesArrayIndex].DiagramLineStyle = DashStyle.DashDot;
                                break;
                            default:
                                // フォーマット異常
                                throw new FormatException(string.Format("OudiaファイルのDiagramSenStyleに異常を検出しました:[{0}][{1}]", keyValue[0], keyValue[1]));
                        }
                    }
                    break;
                // ダイヤグラム線スタイル(太線)
                case "DiagramSenIsBold":
                    if (keyValue[1] == "1")
                    {
                        m_RouteFileProperty.TrainTypes[trainTypesArrayIndex].DiagramLineBold = true;
                    }
                    break;
                // 停車駅明示
                case "StopMarkDrawType":
                    {
                        // 停車駅明示文字で分岐
                        switch (keyValue[1])
                        {
                            case "EStopMarkDrawType_Nothing":
                                m_RouteFileProperty.TrainTypes[trainTypesArrayIndex].StopStationClearlyIndicated = StopMarkDrawType.Nothing;
                                break;
                            case "EStopMarkDrawType_DrawOnStop":
                                m_RouteFileProperty.TrainTypes[trainTypesArrayIndex].StopStationClearlyIndicated = StopMarkDrawType.DrawOnStop;
                                break;
                            default:
                                // フォーマット異常
                                throw new FormatException(string.Format("OudiaファイルのStopMarkDrawTypeに異常を検出しました:[{0}][{1}]", keyValue[0], keyValue[1]));
                        }
                    }
                    break;
                default:
                    // フォーマット異常
                    throw new FormatException(string.Format("OudiaファイルのRessyasyubetsuセクションに異常を検出しました:[{0}][{1}]", keyValue[0], keyValue[1]));
            }

            // ロギング
            Logger.Debug("<<<<= OudFile::SetRessyasyubetsuSection(string)");
        }

        /// <summary>
        /// SetDiaSection
        /// </summary>
        /// <param name="line"></param>
        protected virtual void SetDiaSection(string line)
        {
            // ロギング
            Logger.Debug("=>>>> OudFile::SetDiaSection(string)");
            Logger.DebugFormat("line:[{0}]", line);

            // 行で分岐
            switch (line)
            {
                case "Dia.":
                    // DiagramPropertyオブジェクト生成
                    DiagramProperty diagramProperty = new DiagramProperty() { Name = "", Seq = m_RouteFileProperty.Diagrams.Count + 1 };
                    // DiagramSequencePropertyオブジェクト生成
                    DiagramSequenceProperty diagramSequenceProperty = new DiagramSequenceProperty() { Name = "", Seq = m_RouteFileProperty.DiagramSequences.Count + 1 };
                    // 仮登録
                    m_RouteFileProperty.Diagrams.Add(diagramProperty);
                    m_RouteFileProperty.DiagramSequences.Add(diagramSequenceProperty);
                    return;
                default:
                    break;
            }

            // '='で分割
            string[] keyValue = line.Split('=');

            // 分割サイズ判定
            if (keyValue.Length != 2)
            {
                // フォーマット異常
                throw new FormatException(string.Format("Oudiaファイルのダイヤグラム行に異常を検出しました:[{0}]", line));
            }

            // 配列インデックス取得
            m_DiagramId = m_RouteFileProperty.Diagrams.Count - 1;
            m_DiagramSequenceId = m_RouteFileProperty.DiagramSequences.Count - 1;

            // キー分岐
            switch (keyValue[0])
            {
                // ダイヤ名
                case "DiaName":
                    {
                        m_RouteFileProperty.Diagrams[m_DiagramId].Name = keyValue[1];
                        m_RouteFileProperty.DiagramSequences[m_DiagramId].Name = keyValue[1];
                    }
                    break;
                default:
                    // フォーマット異常
                    throw new FormatException(string.Format("OudiaファイルのDiaセクションに異常を検出しました:[{0}][{1}]", keyValue[0], keyValue[1]));
            }

            // ロギング
            Logger.Debug("<<<<= OudFile::SetDiaSection(string)");
        }

        /// <summary>
        /// SetKudariSection
        /// </summary>
        /// <param name="line"></param>
        protected virtual void SetKudariSection(string line)
        {
            // ロギング
            Logger.Debug("=>>>> OudFile::SetKudariSection(string)");
            Logger.DebugFormat("line:[{0}]", line);

            // 方向種別設定
            m_CurrentDirectionType = DirectionType.Outbound;

            // ロギング
            Logger.Debug("<<<<= OudFile::SetKudariSection(string)");
        }

        /// <summary>
        /// SetNoboriSection
        /// </summary>
        /// <param name="line"></param>
        protected virtual void SetNoboriSection(string line)
        {
            // ロギング
            Logger.Debug("=>>>> OudFile::SetNoboriSection(string)");
            Logger.DebugFormat("line:[{0}]", line);

            // 方向種別設定
            m_CurrentDirectionType = DirectionType.Inbound;

            // ロギング
            Logger.Debug("<<<<= OudFile::SetNoboriSection(string)");
        }

        /// <summary>
        /// SetRessyaSection
        /// </summary>
        /// <param name="line"></param>
        protected virtual void SetRessyaSection(string line)
        {
            // ロギング
            Logger.Debug("=>>>> OudFile::SetRessyaSection(string)");
            Logger.DebugFormat("line:[{0}]", line);

            // 配列インデックス取得
            int diagramsArrayIndex = m_RouteFileProperty.Diagrams.Count - 1;

            // 行で分岐
            switch (line)
            {
                case "Ressya.":
                    // TrainPropertyオブジェクト生成
                    TrainProperty trainProperty = new TrainProperty(m_RouteFileProperty.Stations);
                    trainProperty.DiagramId = diagramsArrayIndex;
                    trainProperty.Id = m_RouteFileProperty.Diagrams[diagramsArrayIndex].Trains[m_CurrentDirectionType].Count + 1;
                    trainProperty.Seq = m_RouteFileProperty.Diagrams[diagramsArrayIndex].Trains[m_CurrentDirectionType].Count + 1;
                    // TrainPropertyオブジェクト生成
                    TrainSequenceProperty trainSequenceProperty = new TrainSequenceProperty();
                    trainSequenceProperty.DiagramId = diagramsArrayIndex;
                    trainSequenceProperty.Id = m_RouteFileProperty.Diagrams[diagramsArrayIndex].TrainSequence[m_CurrentDirectionType].GetNewId();
                    trainSequenceProperty.Seq = m_RouteFileProperty.Diagrams[diagramsArrayIndex].TrainSequence[m_CurrentDirectionType].Count + 1;
                    // 仮登録
                    m_RouteFileProperty.Diagrams[diagramsArrayIndex].Trains[m_CurrentDirectionType].Add(trainProperty);
                    m_RouteFileProperty.Diagrams[diagramsArrayIndex].TrainSequence[m_CurrentDirectionType].Add(trainSequenceProperty);
                    return;
                default:
                    break;
            }

            // '='で分割
            string[] keyValue = line.Split('=');

            // 分割サイズ判定
            if (keyValue.Length != 2)
            {
                // フォーマット異常
                throw new FormatException(string.Format("Oudiaファイルの列車行に異常を検出しました:[{0}]", line));
            }

            // 配列インデックス取得
            int trainsArrayIndex = m_RouteFileProperty.Diagrams[diagramsArrayIndex].Trains[m_CurrentDirectionType].Count - 1;

            // キー分岐
            switch (keyValue[0])
            {
                // 方向
                case "Houkou":
                    {
                        // 値分岐
                        switch (keyValue[1])
                        {
                            case "Kudari":
                                m_RouteFileProperty.Diagrams[diagramsArrayIndex].Trains[m_CurrentDirectionType][trainsArrayIndex].Direction = DirectionType.Outbound;
                                m_RouteFileProperty.Diagrams[diagramsArrayIndex].TrainSequence[m_CurrentDirectionType][trainsArrayIndex].Direction = DirectionType.Outbound;
                                break;
                            case "Nobori":
                                m_RouteFileProperty.Diagrams[diagramsArrayIndex].Trains[m_CurrentDirectionType][trainsArrayIndex].Direction = DirectionType.Inbound;
                                m_RouteFileProperty.Diagrams[diagramsArrayIndex].TrainSequence[m_CurrentDirectionType][trainsArrayIndex].Direction = DirectionType.Inbound;
                                break;
                            default:
                                // フォーマット異常
                                throw new FormatException(string.Format("OudiaファイルのHoukouに異常を検出しました:[{0}][{1}]", keyValue[0], keyValue[1]));
                        }
                    }
                    break;
                // 列車種別
                case "Syubetsu":
                    {
                        m_RouteFileProperty.Diagrams[diagramsArrayIndex].Trains[m_CurrentDirectionType][trainsArrayIndex].TrainType = int.Parse(keyValue[1]);
                    }
                    break;
                // 列車番号
                case "Ressyabangou":
                    {
                        m_RouteFileProperty.Diagrams[diagramsArrayIndex].Trains[m_CurrentDirectionType][trainsArrayIndex].No = keyValue[1];
                    }
                    break;
                // 列車名
                case "Ressyamei":
                    {
                        m_RouteFileProperty.Diagrams[diagramsArrayIndex].Trains[m_CurrentDirectionType][trainsArrayIndex].Name = keyValue[1];
                    }
                    break;
                // 列車号数
                case "Gousuu":
                    {
                        m_RouteFileProperty.Diagrams[diagramsArrayIndex].Trains[m_CurrentDirectionType][trainsArrayIndex].Number = keyValue[1];
                    }
                    break;
                // 駅時刻
                case "EkiJikoku":
                    {
                        // 駅時刻設定
                        SetStationTime(
                            m_RouteFileProperty.Diagrams[diagramsArrayIndex].Trains[m_CurrentDirectionType][trainsArrayIndex].Direction,
                            trainsArrayIndex,
                            m_RouteFileProperty.Stations,
                            m_RouteFileProperty.Diagrams[diagramsArrayIndex].Trains[m_CurrentDirectionType][trainsArrayIndex].StationTimes,
                            keyValue[1]);
                    }
                    break;
                // 備考
                case "Bikou":
                    {
                        m_RouteFileProperty.Diagrams[diagramsArrayIndex].Trains[m_CurrentDirectionType][trainsArrayIndex].Remarks = keyValue[1];
                    }
                    break;
                default:
                    // フォーマット異常
                    throw new FormatException(string.Format("OudiaファイルのRessyaセクションに異常を検出しました:[{0}][{1}]", keyValue[0], keyValue[1]));
            }

            // ロギング
            Logger.Debug("<<<<= OudFile::SetRessyaSection(string)");
        }

        /// <summary>
        /// SetStationTime
        /// </summary>
        /// <param name="trainIndex"></param>
        /// <param name="stationProperties"></param>
        /// <param name="stationTimes"></param>
        /// <param name="value"></param>
        protected virtual void SetStationTime(DirectionType direction, int trainIndex, StationProperties stationProperties, StationTimeProperties stationTimes, string value)
        {
            // ロギング
            Logger.Debug("=>>>> OudFile::SetStationTime(DirectionType, int, StationTimeProperties, string)");
            Logger.DebugFormat("direction        :[{0}]", direction);
            Logger.DebugFormat("trainIndex       :[{0}]", trainIndex);
            Logger.DebugFormat("stationProperties:[{0}]", stationProperties);
            Logger.DebugFormat("stationTimes     :[{0}]", stationTimes);
            Logger.DebugFormat("value            :[{0}]", value);

            switch (direction)
            {
                case DirectionType.Outbound:
                    SetStationTimeOutbound(trainIndex, stationProperties, stationTimes, value);
                    break;
                case DirectionType.Inbound:
                    SetStationTimeInbound(trainIndex, stationProperties, stationTimes, value);
                    break;
                default:
                    // フォーマット異常
                    throw new FormatException(string.Format("Oudiaファイルの方向種別に異常を検出しました:[{0}]", direction));
            }

            // ロギング
            Logger.Debug("<<<<= OudFile::SetStationTime(DirectionType, int, StationTimeProperties, string)");
        }

        /// <summary>
        /// SetStationTimeOutbound
        /// </summary>
        /// <param name="trainId"></param>
        /// <param name="stationProperties"></param>
        /// <param name="stationTimes"></param>
        /// <param name="value"></param>
        private void SetStationTimeOutbound(int trainId, StationProperties stationProperties, StationTimeProperties stationTimes, string value)
        {
            // ロギング
            Logger.Debug("=>>>> OudFile::SetStationTimeOutbound(int, StationTimeProperties, string)");
            Logger.DebugFormat("trainId          :[{0}]", trainId);
            Logger.DebugFormat("stationProperties:[{0}]", stationProperties);
            Logger.DebugFormat("stationTimes     :[{0}]", stationTimes);
            Logger.DebugFormat("value            :[{0}]", value);

            // ','で分割
            string[] timetables = value.Split(',');

            // 分割文字列を繰り返す
            int index = 0;
            foreach (var timetable in timetables)
            {
                // StationTimePropertyオブジェクト生成
                StationTimeProperty property = new StationTimeProperty();
                property.DiagramId = m_DiagramId;
                property.TrainId = trainId;
                property.Direction = m_CurrentDirectionType;
                property.Seq = index + 1;
                property.StationName = stationProperties[index].Name;
                property.StationTreatment = StationTreatment.NoService;

                // 文字列判定
                if (timetable == string.Empty)
                {
                    // 登録
                    stationTimes[index++].Copy(property);

                    // 次の処理へ
                    continue;
                }

                // ';'で分割
                string[] recoads = timetable.Split(';');

                // 駅扱い設定
                SetStationTreatment(recoads[0], property);

                // 分割数判定
                if (recoads.Length == 1)
                {
                    // 登録
                    stationTimes[index++].Copy(property);

                    // 次の処理へ
                    continue;
                }

                // 発着時刻設定
                SetDepartureArrivalTime(recoads[1], property);

                // 登録
                stationTimes[index++].Copy(property);
            }

            // 後処理
            SetStationTimePostProcessing(trainId, stationTimes);

            // ロギング
            Logger.Debug("<<<<= OudFile::SetStationTimeOutbound(int, StationTimeProperties, string)");
        }

        /// <summary>
        /// SetStationTimeInbound
        /// </summary>
        /// <param name="trainId"></param>
        /// <param name="stationProperties"></param>
        /// <param name="stationTimes"></param>
        /// <param name="value"></param>
        private void SetStationTimeInbound(int trainId, StationProperties stationProperties, StationTimeProperties stationTimes, string value)
        {
            // ロギング
            Logger.Debug("=>>>> OudFile::SetStationTimeInbound(int, StationTimeProperties, string)");
            Logger.DebugFormat("trainId          :[{0}]", trainId);
            Logger.DebugFormat("stationProperties:[{0}]", stationProperties);
            Logger.DebugFormat("stationTimes     :[{0}]", stationTimes);
            Logger.DebugFormat("value            :[{0}]", value);

            // ','で分割
            string[] timetables = value.Split(',');

            // 分割文字列を繰り返す
            int index = stationTimes.Count - 1;
            foreach (var timetable in timetables)
            {
                // StationTimePropertyオブジェクト生成
                StationTimeProperty property = new StationTimeProperty();
                property.DiagramId = m_DiagramId;
                property.TrainId = trainId;
                property.Direction = m_CurrentDirectionType;
                property.Seq = index + 1;
                property.StationName = stationProperties[index].Name;
                property.StationTreatment = StationTreatment.NoService;

                // 文字列判定
                if (timetable == string.Empty)
                {
                    // 登録
                    stationTimes[index--].Copy(property);

                    // 次の処理へ
                    continue;
                }

                // ';'で分割
                string[] recoads = timetable.Split(';');

                // 駅扱い設定
                SetStationTreatment(recoads[0], property);

                // 分割数判定
                if (recoads.Length == 1)
                {
                    // 登録
                    stationTimes[index--].Copy(property);

                    // 次の処理へ
                    continue;
                }

                // 発着時刻設定
                SetDepartureArrivalTime(recoads[1], property);

                // 登録
                stationTimes[index--].Copy(property);
            }

            // 後処理
            SetStationTimePostProcessing(trainId, stationTimes);

            // ロギング
            Logger.Debug("<<<<= OudFile::SetStationTimeInbound(int, StationTimeProperties, string)");
        }

        /// <summary>
        /// SetStationTreatment
        /// </summary>
        /// <param name="value"></param>
        /// <param name="property"></param>
        /// <exception cref="FormatException"></exception>
        private void SetStationTreatment(string value, StationTimeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> OudFile::SetStationTreatment(string, StationTimeProperty)");
            Logger.DebugFormat("value   :[{0}]", value);
            Logger.DebugFormat("property:[{0}]", property);

            // 駅扱い設定
            switch (value)
            {
                case "0":
                    property.StationTreatment = StationTreatment.NoService;
                    break;
                case "1":
                    property.StationTreatment = StationTreatment.Stop;
                    break;
                case "2":
                    property.StationTreatment = StationTreatment.Passing;
                    break;
                case "3":
                    property.StationTreatment = StationTreatment.NoRoute;
                    break;
                default:
                    // フォーマット異常
                    throw new FormatException(string.Format("Oudiaファイルの駅扱いに異常を検出しました:[{0}]", value));
            }

            // ロギング
            Logger.Debug("<<<<= OudFile::SetStationTreatment(string, StationTimeProperty)");
        }

        /// <summary>
        /// SetDepartureArrivalTime
        /// </summary>
        /// <param name="value"></param>
        /// <param name="property"></param>
        /// <exception cref="FormatException"></exception>
        private void SetDepartureArrivalTime(string value, StationTimeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> OudFile::SetDepartureArrivalTime(string, StationTimeProperty)");
            Logger.DebugFormat("value   :[{0}]", value);
            Logger.DebugFormat("property:[{0}]", property);

            // 時刻判定
            if (value.IndexOf('/') >= 0)
            {
                // 時刻分解
                string[] stationTime = value.Split('/');

                // 発着刻設定
                property.ArrivalTime = DateTimeLibrary.GetTimeString(stationTime[0]);
                property.DepartureTime = DateTimeLibrary.GetTimeString(stationTime[1]);
            }
            else
            {
                // 発時刻のみ設定
                property.DepartureTime = DateTimeLibrary.GetTimeString(value);
            }

            // ロギング
            Logger.Debug("<<<<= OudFile::SetDepartureArrivalTime(string, StationTimeProperty)");
        }

        /// <summary>
        /// SetStationTimePostProcessing
        /// </summary>
        /// <param name="trainId"></param>
        /// <param name="stationTimes"></param>
        private void SetStationTimePostProcessing(int trainId, StationTimeProperties stationTimes)
        {
            // ロギング
            Logger.Debug("=>>>> OudFile::SetStationTimePostProcessing(int, StationTimeProperties)");
            Logger.DebugFormat("trainId     :[{0}]", trainId);
            Logger.DebugFormat("stationTimes:[{0}]", stationTimes);

            // 後処理
            foreach (var property in stationTimes)
            {
                // 列車インデックスを判定
                if (property.TrainId == -1)
                {
                    // 更新
                    property.DiagramId = m_DiagramId;
                    property.TrainId = trainId;
                    property.Direction = m_CurrentDirectionType;
                    property.StationTreatment = StationTreatment.NoService;
                }
            }

            // ロギング
            Logger.Debug("<<<<= OudFile::SetStationTimePostProcessing(int, StationTimeProperties)");
        }

        /// <summary>
        /// SetDispPropSection
        /// </summary>
        /// <param name="line"></param>
        protected virtual void SetDispPropSection(string line)
        {
            // ロギング
            Logger.Debug("=>>>> OudFile::SetDispPropSection(string)");
            Logger.DebugFormat("line:[{0}]", line);

            // 行で分岐
            switch (line)
            {
                case "DispProp.":
                    return;
                default:
                    break;
            }

            // '='で分割
            string[] keyValue = line.Split(new char[] { '=', ';' });

            // 分割サイズ判定
            if (keyValue.Length < 2)
            {
                // フォーマット異常
                throw new FormatException(string.Format("Oudiaファイルの表示設定行に異常を検出しました:[{0}]", line));
            }

            // キー分岐
            switch (keyValue[0])
            {
                case "JikokuhyouFont":
                    {
                        // FontPropertyオブジェクト取得
                        FontProperty fontProperty = GetTimetableFont(keyValue);

                        // 時刻表フォント辞書登録
                        m_RouteFileProperty.Fonts.Add(string.Format("時刻表ビュー {0}", m_FontIndex++), fontProperty);
                    }
                    break;
                case "JikokuhyouVFont":
                    {
                        // FontPropertyオブジェクト取得
                        FontProperty fontProperty = GetTimetableFont(keyValue);

                        // 時刻表フォント辞書登録
                        m_RouteFileProperty.Fonts.Add("時刻表ビュー 縦書き", fontProperty);
                    }
                    break;
                case "DiaEkimeiFont":
                    {
                        // FontPropertyオブジェクト取得
                        FontProperty fontProperty = GetTimetableFont(keyValue);

                        // 時刻表フォント辞書登録
                        m_RouteFileProperty.Fonts.Add("ダイヤグラムビュー 駅名", fontProperty);
                    }
                    break;
                case "DiaJikokuFont":
                    {
                        // FontPropertyオブジェクト取得
                        FontProperty fontProperty = GetTimetableFont(keyValue);

                        // 時刻表フォント辞書登録
                        m_RouteFileProperty.Fonts.Add("ダイヤグラムビュー 時刻", fontProperty);
                    }
                    break;
                case "DiaRessyaFont":
                    {
                        // FontPropertyオブジェクト取得
                        FontProperty fontProperty = GetTimetableFont(keyValue);

                        // 時刻表フォント辞書登録
                        m_RouteFileProperty.Fonts.Add("ダイヤグラムビュー 列車", fontProperty);
                    }
                    break;
                case "CommentFont":
                    {
                        // FontPropertyオブジェクト取得
                        FontProperty fontProperty = GetTimetableFont(keyValue);

                        // 時刻表フォント辞書登録
                        m_RouteFileProperty.Fonts.Add("コメント", fontProperty);
                    }
                    break;
                case "DiaMojiColor":
                    {
                        // 時刻表色取得
                        ColorProperty colorProperty = GetTimetableColor(keyValue);

                        // 時刻表色辞書登録
                        m_RouteFileProperty.Colors.Add("ダイヤ画面文字色", colorProperty);
                    }
                    break;
                case "DiaHaikeiColor":
                    // TODO:未実装
                    break;
                case "DiaRessyaColor":
                    // TODO:未実装
                    break;
                case "DiaJikuColor":
                    {
                        // 時刻表色取得
                        ColorProperty colorProperty = GetTimetableColor(keyValue);

                        // 時刻表色辞書登録
                        m_RouteFileProperty.Colors.Add("ダイヤ画面縦横軸色", colorProperty);
                    }
                    break;
                case "EkimeiLength":
                    // 「駅名欄の幅」設定
                    m_RouteFileProperty.Route.WidthOfStationNameField = int.Parse(keyValue[1]);
                    break;
                case "JikokuhyouRessyaWidth":
                    // 「時刻表の列車の幅」設定
                    m_RouteFileProperty.Route.TimetableTrainWidth = int.Parse(keyValue[1]);
                    break;
                default:
                    // フォーマット異常
                    throw new FormatException(string.Format("OudiaファイルのDispPropセクションに異常を検出しました:[{0}][{1}]", keyValue[0], keyValue[1]));
            }

            // ロギング
            Logger.Debug("<<<<= OudFile::SetDispPropSection(string)");
        }

        /// <summary>
        /// SetDefaultSection
        /// </summary>
        /// <param name="line"></param>
        protected virtual void SetDefaultSection(string line)
        {
            // ロギング
            Logger.Debug("=>>>> OudFile::SetDefaultSection(string)");
            Logger.DebugFormat("line:[{0}]", line);

            // '='で分割
            string[] keyValue = line.Split(new char[] { '=', ';' });

            // 分割サイズ判定
            if (keyValue.Length < 2)
            {
                // フォーマット異常
                throw new FormatException(string.Format("Oudiaファイルのデフォルト行に異常を検出しました:[{0}]", line));
            }

            // キー分岐
            switch (keyValue[0])
            {
                case "KitenJikoku":
                    {
                        // 文字補正
                        if (keyValue[1].Length == 3)
                        {
                            keyValue[1] = "0" + keyValue[1];
                        }

                        // 「ダイヤグラム起点時刻」設定
                        m_RouteFileProperty.DiagramScreen.DiagramDtartingTime = DateTime.ParseExact(keyValue[1], "HHmm", null);
                    }
                    break;
                case "DiagramDgrYZahyouKyoriDefault":
                    {
                        // 「ダイヤグラムの規定の駅間幅」設定
                        m_RouteFileProperty.DiagramScreen.StandardWidthBetweenStationsInTheDiagram = int.Parse(keyValue[1]);
                    }
                    break;
                case "Comment":
                    {
                        // コメント設定
                        m_RouteFileProperty.Comment.Append(keyValue[1]);
                    }
                    break;
                case "FileType":
                    {
                        // 何もしない
                    }
                    break;
                case "FileTypeAppComment":
                    {
                        // 設定
                        m_RouteFileProperty.FileInfo.ImportFileType = keyValue[1];
                    }
                    break;
                default:
                    // フォーマット異常
                    throw new FormatException(string.Format("OudiaファイルのDefaultセクションに異常を検出しました:[{0}][{1}]", keyValue[0], keyValue[1]));
            }

            // ロギング
            Logger.Debug("<<<<= OudFile::SetDefaultSection(string)");
        }
        #endregion

        #region protected virtualメソッド
        #region 時刻表フォント取得
        /// <summary>
        /// 時刻表フォント取得
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        protected virtual FontProperty GetTimetableFont(string[] keyValue)
        {
            // ロギング
            Logger.Debug("=>>>> OudFile::GetTimetableFont(string[])");
            Logger.DebugFormat("keyValue:[{0}]", keyValue);

            // 分割サイズ判定
            if (!(keyValue.Length >= 5 && (keyValue.Length <= 9)))
            {
                // ロギング
                Logger.WarnFormat("フォント行不正:[{0}]", keyValue);
                Logger.Debug("<<<<= OudFile::GetTimetableFont(string)");

                // デフォルトFontPropertyオブジェクト返却
                return new FontProperty();
            }

            // キー判定
            if (keyValue[1] != "PointTextHeight")
            {
                // ロギング
                Logger.WarnFormat("フォントサイズキー不正:[{0}]", keyValue[1]);
                Logger.Debug("<<<<= OudFile::GetTimetableFont(string)");

                // デフォルトFontPropertyオブジェクト返却
                return new FontProperty();
            }

            // フォントサイズ変換
            float fontsize = 0f;
            if (!float.TryParse(keyValue[2], out fontsize))
            {
                // ロギング
                Logger.WarnFormat("フォントサイズ値不正:[{0}]", keyValue[2]);
                Logger.Debug("<<<<= OudFile::GetTimetableFont(string)");

                // デフォルトFontPropertyオブジェクト返却
                return new FontProperty();
            }

            // キー判定
            if (keyValue[3] != "Facename")
            {
                // ロギング
                Logger.WarnFormat("フォント名キー不正:[{0}]", keyValue[3]);
                Logger.Debug("<<<<= OudFile::GetTimetableFont(string)");

                // デフォルトFontPropertyオブジェクト返却
                return new FontProperty();
            }

            // フォント
            if (!FontLibrary.Usability(keyValue[4]))
            {
                // ロギング
                Logger.WarnFormat("フォント名不正:[{0}]", keyValue[4]);
                Logger.Debug("<<<<= OudFile::GetTimetableFont(string)");

                // デフォルトFontPropertyオブジェクト返却
                return new FontProperty();
            }

            // FontStyleオブジェクト生成
            FontStyle fontStyle = FontStyle.Regular;

            // FontStyleオブジェクト設定
            for (int i = 5; i < keyValue.Length; i += 2)
            {
                // FontStyleオブジェクト設定
                fontStyle |= GetTimetableFontStyle(keyValue[i], keyValue[i + 1]);
            }

            // フォントオブジェクト生成
            FontProperty result = new FontProperty(keyValue[4], fontsize, fontStyle);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= OudFile::GetTimetableFont(string)");

            // 返却
            return result;
        }

        /// <summary>
        /// FontStyleオブジェクト設定
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual FontStyle GetTimetableFontStyle(string type, string value)
        {
            // ロギング
            Logger.Debug("=>>>> OudFile::GetTimetableFontStyle(string, string)");
            Logger.DebugFormat("type:[{0}]", type);
            Logger.DebugFormat("value:[{0}]", value);

            // FontStyleオブジェクト生成
            FontStyle result = FontStyle.Regular;


            // フォントスタイル種別で分岐
            switch (type)
            {
                case "Bold":
                    if (value == "1")
                    {
                        result = FontStyle.Bold;
                    }
                    break;
                case "Itaric":
                    if (value == "1")
                    {
                        result = FontStyle.Italic;
                    }
                    break;
                default:
                    // ロギング
                    Logger.WarnFormat("フォントスタイル不正:[{0}]", type);
                    break;
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= OudFile::GetTimetableFontStyle(string, string)");

            // 返却
            return result;
        }
        #endregion

        #region 時刻表色取得
        /// <summary>
        /// 時刻表色取得
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        protected virtual ColorProperty GetTimetableColor(string[] keyValue)
        {
            // ロギング
            Logger.Debug("=>>>> OudFile::GetTimetableColor(string[])");
            Logger.DebugFormat("keyValue:[{0}]", keyValue);

            // 分割サイズ判定
            if (keyValue.Length != 2)
            {
                // ロギング
                Logger.WarnFormat("時刻表色行不正:[{0}]", keyValue);
                Logger.Debug("<<<<= OudFile::GetTimetableFont(string)");

                // デフォルトColorPropertyオブジェクト返却
                return new ColorProperty(Color.Black);
            }

            // 色文字列分解
            int Alpha = int.Parse(keyValue[1].Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int B = int.Parse(keyValue[1].Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int G = int.Parse(keyValue[1].Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            int R = int.Parse(keyValue[1].Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

            // Colorオブジェクト取得
            ColorProperty result = new ColorProperty(Alpha, R, G, B);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= OudFile::GetTimetableColor(string[])");

            // 返却
            return result;
        }
        #endregion
        #endregion
    }
}
