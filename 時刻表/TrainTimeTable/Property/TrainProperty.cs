using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Common;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// TrainPropertyクラス
    /// </summary>
    [Serializable]
    public class TrainProperty
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// ダイヤグラムID
        /// </summary>
        public int DiagramId { get; set; } = -1;

        /// <summary>
        /// 方向種別
        /// </summary>
        public DirectionType Direction { get; set; } = DirectionType.None;

        /// <summary>
        /// 列車ID
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// シーケンス番号
        /// </summary>
        [Obsolete("TrainPropertyクラスのシーケンス番号は今後使用不可となる予定です", false)]
        public int Seq { get; set; } = 0;

        /// <summary>
        /// 列車種別番号
        /// </summary>
        public int TrainType { get; set; } = 0;

        /// <summary>
        /// 列車番号
        /// </summary>
        public string No { get; set; } = string.Empty;

        /// <summary>
        /// 列車名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 列車号数
        /// </summary>
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// 列車記号
        /// </summary>
        public TrainMarkProperties Marks { get; set; } = new TrainMarkProperties();

        /// <summary>
        /// 列車記号シーケンス
        /// </summary>
        public TrainMarkSequenceProperties MarkSequences { get; set; } = new TrainMarkSequenceProperties();

        /// <summary>
        /// 発駅
        /// </summary>
        public string DepartingStation { get; set; } = string.Empty;

        /// <summary>
        /// 着駅
        /// </summary>
        public string DestinationStation { get; set; } = string.Empty;

        /// <summary>
        /// 駅時刻
        /// </summary>
        public StationTimeProperties StationTimes { get; set; } = new StationTimeProperties();

        /// <summary>
        /// 備考
        /// </summary>
        public string Remarks { get; set; } = string.Empty;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TrainProperty()
        {
            // ロギング
            Logger.Debug("=>>>> TrainProperty::TrainProperty()");

            // ロギング
            Logger.Debug("<<<<= TrainProperty::TrainProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public TrainProperty(TrainProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainProperty::TrainProperty(TrainProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            Copy(property);

            // ロギング
            Logger.Debug("<<<<= TrainProperty::TrainProperty(TrainProperty)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public TrainProperty(StationProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> TrainProperty::TrainProperty(StationProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 駅時刻仮登録
            for (int i = 0; i < properties.Count; i++)
            {
                // StationTimePropertyオブジェクト生成
                StationTimeProperty property = new StationTimeProperty();
                property.StationName = properties[i].Name;
                property.Seq = i + 1;

                // 登録
                StationTimes.Add(property);
            }

            // ロギング
            Logger.Debug("<<<<= TrainProperty::TrainProperty(StationProperties)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="property"></param>
        public void Copy(TrainProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainProperty::Copy(TrainProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this ,property))
            {
                // コピー
                DiagramId = property.DiagramId;
                Direction = property.Direction;
                Id = property.Id;
                Seq = property.Seq;
                TrainType = property.TrainType;
                No = property.No;
                Name = property.Name;
                DepartingStation = property.DepartingStation;
                DestinationStation = property.DestinationStation;
                Number = property.Number;
                StationTimes.Copy(property.StationTimes);
                Remarks = property.Remarks;
            }

            // ロギング
            Logger.Debug("<<<<= TrainProperty::Copy(TrainProperty)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(TrainProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainProperty::Compare(TrainProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 比較
            if (DiagramId != property.DiagramId)
            {
                // ロギング
                Logger.DebugFormat("DiagramId:[不一致][{0}][{1}]", DiagramId, property.DiagramId);
                Logger.Debug("<<<<= TrainProperty::Compare(TrainProperty)");

                // 不一致
                return false;
            }
            if (Direction != property.Direction)
            {
                // ロギング
                Logger.DebugFormat("Direction:[不一致][{0}][{1}]", Direction, property.Direction);
                Logger.Debug("<<<<= TrainProperty::Compare(TrainProperty)");

                // 不一致
                return false;
            }
            if (Id != property.Id)
            {
                // ロギング
                Logger.DebugFormat("Id:[不一致][{0}][{1}]", Id, property.Id);
                Logger.Debug("<<<<= TrainProperty::Compare(TrainProperty)");

                // 不一致
                return false;
            }
            if (Seq != property.Seq)
            {
                // ロギング
                Logger.DebugFormat("Seq:[不一致][{0}][{1}]", Seq, property.Seq);
                Logger.Debug("<<<<= TrainProperty::Compare(TrainProperty)");

                // 不一致
                return false;
            }
            if (TrainType != property.TrainType)
            {
                // ロギング
                Logger.DebugFormat("TrainType:[不一致][{0}][{1}]", TrainType, property.TrainType);
                Logger.Debug("<<<<= TrainProperty::Compare(TrainProperty)");

                // 不一致
                return false;
            }
            if (No != property.No)
            {
                // ロギング
                Logger.DebugFormat("No:[不一致][{0}][{1}]", No, property.No);
                Logger.Debug("<<<<= TrainProperty::Compare(TrainProperty)");

                // 不一致
                return false;
            }
            if (Name != property.Name)
            {
                // ロギング
                Logger.DebugFormat("Name:[不一致][{0}][{1}]", Name, property.Name);
                Logger.Debug("<<<<= TrainProperty::Compare(TrainProperty)");

                // 不一致
                return false;
            }
            if (Number != property.Number)
            {
                // ロギング
                Logger.DebugFormat("Number:[不一致][{0}][{1}]", Number, property.Number);
                Logger.Debug("<<<<= TrainProperty::Compare(TrainProperty)");

                // 不一致
                return false;
            }
            if (!StationTimes.Compare(property.StationTimes))
            {
                // ロギング
                Logger.Debug("<<<<= TrainProperty::Compare(TrainProperty)");

                // 不一致
                return false;
            }
            if (DepartingStation != property.DepartingStation)
            {
                // ロギング
                Logger.DebugFormat("DepartingStation:[不一致][{0}][{1}]", DepartingStation, property.DepartingStation);
                Logger.Debug("<<<<= TrainProperty::Compare(TrainProperty)");

                // 不一致
                return false;
            }
            if (DestinationStation != property.DestinationStation)
            {
                // ロギング
                Logger.DebugFormat("DestinationStation:[不一致][{0}][{1}]", DestinationStation, property.DestinationStation);
                Logger.Debug("<<<<= TrainProperty::Compare(TrainProperty)");

                // 不一致
                return false;
            }
            if (Remarks != property.Remarks)
            {
                // ロギング
                Logger.DebugFormat("Remarks:[不一致][{0}][{1}]", Remarks, property.Remarks);
                Logger.Debug("<<<<= TrainProperty::Compare(TrainProperty)");

                // 不一致
                return false;
            }

            // ロギング
            Logger.DebugFormat("result:[一致]");
            Logger.Debug("<<<<= TrainProperty::Compare(TrainProperty)");

            // 一致
            return true;
        }
        #endregion

        #region 発着駅設定
        /// <summary>
        /// 発着駅設定
        /// </summary>
        public void DepartureArrivalStationSetting()
        {
            // ロギング
            Logger.Debug("=>>>> TrainProperty::DepartureArrivalStationSetting()");

            // 方向種別で分岐
            switch(Direction)
            {
                case DirectionType.Outbound:
                    DepartureArrivalStationOutboundSetting();
                    break;
                case DirectionType.Inbound:
                    DepartureArrivalStationInboundSetting();
                    break;
                default:
                    break;
            }

            // ロギング
            Logger.DebugFormat("DepartingStation  :[{0}]", DepartingStation);
            Logger.DebugFormat("DestinationStation:[{0}]", DestinationStation);
            Logger.Debug("<<<<= TrainProperty::DepartureArrivalStationSetting()");
        }

        /// <summary>
        /// DepartureArrivalStationOutboundSetting
        /// </summary>
        private void DepartureArrivalStationOutboundSetting()
        {
            // ロギング
            Logger.Debug("=>>>> TrainProperty::DepartureArrivalStationOutboundSetting()");

            // 駅数分繰り返す
            int stop = 0;
            for (int i = 0; i < StationTimes.Count; i++)
            {
                // 発駅が設定されてなく、かつ発時刻がある場合
                if (DepartingStation == string.Empty && StationTimes[i].DepartureTime != string.Empty)
                {
                    DepartingStation = StationTimes[i].StationName;
                }

                // 駅停車か？
                if (StationTimes[i].StationTreatment == StationTreatment.Stop)
                {
                    // 停車駅を記憶
                    stop = i;
                }
            }

            // 着駅設定
            DestinationStation = StationTimes[stop].StationName;

            // ロギング
            Logger.Debug("<<<<= TrainProperty::DepartureArrivalStationOutboundSetting()");
        }

        /// <summary>
        /// DepartureArrivalStationInboundSetting
        /// </summary>
        private void DepartureArrivalStationInboundSetting()
        {
            // ロギング
            Logger.Debug("=>>>> TrainProperty::DepartureArrivalStationInboundSetting()");

            // 駅数分繰り返す
            int stop = 0;
            for (int i = StationTimes.Count - 1; i >= 0; i--)
            {
                // 発駅が設定されてなく、かつ発時刻がある場合
                if (DepartingStation == string.Empty && StationTimes[i].DepartureTime != string.Empty)
                {
                    DepartingStation = StationTimes[i].StationName;
                }

                // 駅停車か？
                if (StationTimes[i].StationTreatment == StationTreatment.Stop)
                {
                    // 停車駅を記憶
                    stop = i;
                }
            }

            // 着駅設定
            DestinationStation = StationTimes[stop].StationName;

            // ロギング
            Logger.Debug("<<<<= TrainProperty::DepartureArrivalStationInboundSetting()");
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
            result.AppendLine(indentstr + string.Format("＜列車情報＞"));
            result.AppendLine(indentstr + string.Format("　ダイヤグラムID:[{0}] ", DiagramId));
            result.AppendLine(indentstr + string.Format("　方向種別      :[{0}] ", Direction.GetStringValue()));
            result.AppendLine(indentstr + string.Format("　ID            :[{0}] ", Id));
            result.AppendLine(indentstr + string.Format("　シーケンス番号:[{0}] ", Seq));
            result.AppendLine(indentstr + string.Format("　列車種別番号  :[{0}] ", TrainType));
            result.AppendLine(indentstr + string.Format("　列車番号      :[{0}] ", No));
            result.AppendLine(indentstr + string.Format("　列車名        :[{0}] ", Name));
            result.AppendLine(indentstr + string.Format("　列車号数      :[{0}] ", Number));
            result.AppendLine(indentstr + string.Format("　発駅          :[{0}] ", DepartingStation));
            result.AppendLine(indentstr + string.Format("　着駅          :[{0}] ", DestinationStation));
            result.AppendLine(indentstr + string.Format("　駅時刻        :[{0}] ", StationTimes));
            result.AppendLine(indentstr + string.Format("　備考          :[{0}] ", Remarks));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
