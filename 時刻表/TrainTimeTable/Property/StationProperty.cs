using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Common;
using TrainTimeTable.Component;
using TrainTimeTable.Property;
using static System.Net.Mime.MediaTypeNames;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// StationPropertyクラス
    /// </summary>
    [Serializable]
    public class StationProperty
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// 駅名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 起点駅
        /// </summary>
        public bool StartingStation { get; set; } = false;

        /// <summary>
        /// 終点駅
        /// </summary>
        public bool TerminalStation { get; set; } = false;

        /// <summary>
        /// 駅マーク(駅弁、みどりの窓口など)
        /// </summary>
        public StationMarks Marks {  get; set; }= new StationMarks();

        /// <summary>
        /// 時刻形式
        /// </summary>
        public TimeFormat TimeFormat { get; set; } = TimeFormat.None;

        /// <summary>
        /// 駅規模
        /// </summary>
        public StationScale StationScale { get; set; } = StationScale.None;

        /// <summary>
        /// 基準駅からの駅距離
        /// </summary>
        public DictionaryStationDistanceFromReferenceStation StationDistanceFromReferenceStations { get; set; } = new DictionaryStationDistanceFromReferenceStation()
        {
            { DirectionType.Outbound, 0.0f },
            { DirectionType.Inbound, 0.0f }
        };

        /// <summary>
        /// 境界線
        /// </summary>
        public bool Border { get; set; } = false;

        /// <summary>
        /// 次駅プロパティリスト
        /// </summary>
        public NextStationProperties NextStations { get; set; } = new NextStationProperties();

        /// <summary>
        /// ダイヤグラム列車情報
        /// </summary>
        public DictionaryDiagramTrainInformation DiagramTrainInformations { get; set; } = new DictionaryDiagramTrainInformation()
        {
            { DirectionType.Outbound, DiagramTrainInformation.DisplayIfItIsTheFirstTrain },
            { DirectionType.Inbound, DiagramTrainInformation.DisplayIfItIsTheFirstTrain }
        };

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StationProperty()
        {
            // ロギング
            Logger.Debug("=>>>> StationProperty::StationProperty()");

            // ロギング
            Logger.Debug("<<<<= StationProperty::StationProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public StationProperty(StationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationProperty::StationProperty(StationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            Copy(property);

            // ロギング
            Logger.Debug("<<<<= StationProperty::StationProperty(StationProperty)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="property"></param>
        public void Copy(StationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationProperty::Copy(StationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this ,property))
            {
                // コピー
                Name = property.Name;
                StartingStation = property.StartingStation;
                TerminalStation = property.TerminalStation;
                Marks.Clear();
                Marks.AddRange(property.Marks);
                TimeFormat = property.TimeFormat;
                StationScale = property.StationScale;
                StationDistanceFromReferenceStations.Copy(property.StationDistanceFromReferenceStations);
                Border = property.Border;
                NextStations.Clear();
                NextStations.AddRange(property.NextStations);
                DiagramTrainInformations.Copy(property.DiagramTrainInformations);
            }

            // ロギング
            Logger.Debug("<<<<= StationProperty::Copy(StationProperty)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(StationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationProperty::Compare(StationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 比較
            if (Name != property.Name)
            {
                // ロギング
                Logger.DebugFormat("Name:[不一致][{0}][{1}]", Name, property.Name);
                Logger.Debug("<<<<= StationProperty::Compare(StationProperty)");

                // 不一致
                return false;
            }
            if (StartingStation != property.StartingStation)
            {
                // ロギング
                Logger.DebugFormat("StartingStation:[不一致][{0}][{1}]", StartingStation, property.StartingStation);
                Logger.Debug("<<<<= StationProperty::Compare(StationProperty)");

                // 不一致
                return false;
            }
            if (TerminalStation != property.TerminalStation)
            {
                // ロギング
                Logger.DebugFormat("TerminalStation:[不一致][{0}][{1}]", TerminalStation, property.TerminalStation);
                Logger.Debug("<<<<= StationProperty::Compare(StationProperty)");

                // 不一致
                return false;
            }
            if (!Marks.Compare(property.Marks))
            {
                // ロギング
                Logger.DebugFormat("Marks:[不一致][{0}][{1}]", Marks, property.Marks);
                Logger.Debug("<<<<= StationProperty::Compare(StationProperty)");

                // 不一致
                return false;
            }
            if (TimeFormat != property.TimeFormat)
            {
                // ロギング
                Logger.DebugFormat("TimeFormat:[不一致][{0}][{1}]", TimeFormat, property.TimeFormat);
                Logger.Debug("<<<<= StationProperty::Compare(StationProperty)");

                // 不一致
                return false;
            }
            if (StationScale != property.StationScale)
            {
                // ロギング
                Logger.DebugFormat("StationScale:[不一致][{0}][{1}]", StationScale, property.StationScale);
                Logger.Debug("<<<<= StationProperty::Compare(StationProperty)");

                // 不一致
                return false;
            }
            if (!StationDistanceFromReferenceStations.Compare(property.StationDistanceFromReferenceStations))
            {
                // ロギング
                Logger.DebugFormat("StationDistanceFromReferenceStations:[不一致][{0}][{1}]", StationDistanceFromReferenceStations, property.StationDistanceFromReferenceStations);
                Logger.Debug("<<<<= StationProperty::Compare(StationProperty)");

                // 不一致
                return false;
            }
            if (Border != property.Border)
            {
                // ロギング
                Logger.DebugFormat("Border:[不一致][{0}][{1}]", Border, property.Border);
                Logger.Debug("<<<<= StationProperty::Compare(StationProperty)");

                // 不一致
                return false;
            }
            if (!NextStations.Compare(property.NextStations))
            {
                // ロギング
                Logger.DebugFormat("NextStations:[不一致][{0}][{1}]", NextStations, property.NextStations);
                Logger.Debug("<<<<= StationProperty::Compare(StationProperty)");

                // 不一致
                return false;
            }
            if (!DiagramTrainInformations.Compare(property.DiagramTrainInformations))
            {
                // ロギング
                Logger.DebugFormat("DiagramTrainInformations:[不一致][{0}][{1}]", DiagramTrainInformations, property.DiagramTrainInformations);
                Logger.Debug("<<<<= StationProperty::Compare(StationProperty)");

                // 不一致
                return false;
            }

            // ロギング
            Logger.DebugFormat("result:[一致]");
            Logger.Debug("<<<<= StationProperty::Compare(StationProperty)");

            // 一致
            return true;
        }
        #endregion

        #region 文字列化
        /// <summary>
        /// 文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // 文字列化返却
            return ToString(0);
        }

        /// <summary>
        /// 文字列化
        /// </summary>
        /// <param name="indent"></param>
        /// <returns></returns>
        public string ToString(int indent)
        {
            // 結果オブジェクト生成
            StringBuilder result = new StringBuilder();

            // インデント生成
            string indentstr = new string('　', indent);

            // 文字列追加
            result.AppendLine(indentstr + string.Format("＜駅情報＞"));
            result.AppendLine(indentstr + string.Format("　駅名    :[{0}]", Name));
            result.AppendLine(indentstr + string.Format("　起点駅  :[{0}]", StartingStation));
            result.AppendLine(indentstr + string.Format("　終点駅  :[{0}]", TerminalStation));
            result.Append(Marks.ToString(indent + 1));
            result.AppendLine(indentstr + string.Format("　時刻形式:[{0}]", TimeFormat.GetStringValue()));
            result.AppendLine(indentstr + string.Format("　駅規模  :[{0}]", StationScale.GetStringValue()));
            result.Append(StationDistanceFromReferenceStations.ToString(indent + 1));
            result.AppendLine(indentstr + string.Format("　境界線  :[{0}]", Border));
            result.Append(NextStations.ToString(indent + 1));
            result.Append(DiagramTrainInformations.ToString(indent + 1));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
