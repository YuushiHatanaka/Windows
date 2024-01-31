using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        private readonly static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// ダイヤグラム名
        /// </summary>
        public string DiagramName { get; set; } = string.Empty;

        /// <summary>
        /// 方向種別
        /// </summary>
        public DirectionType Direction { get; set; } = DirectionType.None;

        /// <summary>
        /// 列車ID
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// 列車種別名
        /// </summary>
        public string TrainTypeName { get; set; } = string.Empty;

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
        /// <param name="name"></param>
        /// <param name="properties"></param>
        public TrainProperty(string name, StationProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> TrainProperty::TrainProperty(string, StationProperties)");
            Logger.DebugFormat("name      :[{0}]", name);
            Logger.DebugFormat("properties:[{0}]", properties);

            // ダイヤグラム名設定
            DiagramName = name;

            // 駅時刻仮登録
            for (int i = 0; i < properties.Count; i++)
            {
                // StationTimePropertyオブジェクト生成
                StationTimeProperty property = new StationTimeProperty()
                {
                    DiagramName = name,
                    StationName = properties[i].Name,
                };

                // 登録
                StationTimes.Add(property);
            }

            // ロギング
            Logger.Debug("<<<<= TrainProperty::TrainProperty(string, StationProperties)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="properties"></param>
        public TrainProperty(string name, DirectionType type, StationProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> TrainProperty::TrainProperty(string, DirectionType, StationProperties)");
            Logger.DebugFormat("name      :[{0}]", name);
            Logger.DebugFormat("type      :[{0}]", type.GetStringValue());
            Logger.DebugFormat("properties:[{0}]", properties);

            // 設定
            Direction = type;
            DiagramName = name;

            // 駅時刻仮登録
            for (int i = 0; i < properties.Count; i++)
            {
                // StationTimePropertyオブジェクト生成
                StationTimeProperty property = new StationTimeProperty()
                {
                    DiagramName = name,
                    Direction = type,
                    StationName = properties[i].Name,
                    StationTreatment = StationTreatment.NoService,
                };

                // 登録
                StationTimes.Add(property);
            }

            // ロギング
            Logger.Debug("<<<<= TrainProperty::TrainProperty(string, DirectionType, StationProperties)");
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
                DiagramName = property.DiagramName;
                Direction = property.Direction;
                Id = property.Id;
                TrainTypeName = property.TrainTypeName;
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
            if (DiagramName != property.DiagramName)
            {
                // ロギング
                Logger.DebugFormat("DiagramName:[不一致][{0}][{1}]", DiagramName, property.DiagramName);
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
            if (TrainTypeName != property.TrainTypeName)
            {
                // ロギング
                Logger.DebugFormat("TrainTypeName:[不一致][{0}][{1}]", TrainTypeName, property.TrainTypeName);
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

        #region 分割
        /// <summary>
        /// 分割判定
        /// </summary>
        /// <param name="stations"></param>
        /// <param name="sequences"></param>
        /// <param name="stationName"></param>
        /// <returns></returns>
        public bool IsDivisible(StationProperties stations, StationSequenceProperties sequences, string stationName)
        {
            // ロギング
            Logger.Debug("=>>>> TrainProperty::IsDivisible(StationProperties, StationSequenceProperties, string)");
            Logger.DebugFormat("stations   :[{0}]", stations);
            Logger.DebugFormat("sequences  :[{0}]", sequences);
            Logger.DebugFormat("stationName:[{0}]", stationName);

            // 結果設定
            bool result = true;

            #region 選択駅情報取得
            // StationSequencePropertyオブジェクト取得
            StationSequenceProperty stationSequenceProperty = sequences[stationName];

            // StationPropertyオブジェクト取得
            StationProperty stationProperty = stations[stationName];

            // StationTimePropertyオブジェクト取得
            StationTimeProperty stationTimeProperty = StationTimes[stationName];
            #endregion

            #region 次駅情報取得
            // 次駅
            StationSequenceProperty nextStationSequenceProperty = sequences.Next(Direction, 1, stationName);

            // StationPropertyオブジェクト取得
            StationProperty nextStationProperty = stations[nextStationSequenceProperty?.Name];

            // StationTimePropertyオブジェクト取得
            StationTimeProperty nextStationTimeProperty = StationTimes[nextStationSequenceProperty?.Name];
            #endregion

            do
            {
                // 停車判定
                if (!(stationTimeProperty.StationTreatment == StationTreatment.Stop || stationTimeProperty.StationTreatment == StationTreatment.Passing))
                {
                    // 結果設定
                    result = false;

                    // 終了
                    break;
                }

                // 次駅存在判定
                if (nextStationSequenceProperty == null)
                {
                    // 結果設定
                    result = false;

                    // 終了
                    break;
                }

                // 停車判定
                if (!(nextStationTimeProperty.StationTreatment == StationTreatment.Stop || nextStationTimeProperty.StationTreatment == StationTreatment.Passing))
                {
                    // 結果設定
                    result = false;

                    // 終了
                    break;
                }

                // 終了
                break;
            } while (true);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= TrainProperty::IsDivisible(StationProperties, StationSequenceProperties, string)");

            // 返却
            return result;
        }

        /// <summary>
        /// 分割
        /// </summary>
        /// <param name="sequences"></param>
        /// <param name="stationName"></param>
        /// <returns></returns>
        public TrainProperties Divide(StationProperties stations, StationSequenceProperties sequences, string stationName)
        {
            // ロギング
            Logger.Debug("=>>>> TrainProperty::Divide(StationProperties, StationSequenceProperties, string)");
            Logger.DebugFormat("stations   :[{0}]", stations);
            Logger.DebugFormat("sequences  :[{0}]", sequences);
            Logger.DebugFormat("stationName:[{0}]", stationName);

            // 結果オブジェクト生成
            TrainProperties result = new TrainProperties()
            {
                new TrainProperty(this),
                new TrainProperty(this),
            };

            // 設定
            result[0].StopStationSetting(Direction, sequences, stationName);
            result[1].StartStationSetting(Direction, sequences, stationName);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= TrainProperty::Divide(StationProperties, StationSequenceProperties, string)");

            // 返却
            return result;
        }
        #endregion

        #region 結合
        /// <summary>
        /// 結合判定
        /// </summary>
        /// <param name="stations"></param>
        /// <param name="sequences"></param>
        /// <param name="stationName"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool IsJoin(StationProperties stations, StationSequenceProperties sequences, string stationName, TrainProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainProperty::IsJoin(StationProperties, StationSequenceProperties, string, TrainProperty)");
            Logger.DebugFormat("stations   :[{0}]", stations);
            Logger.DebugFormat("sequences  :[{0}]", sequences);
            Logger.DebugFormat("stationName:[{0}]", stationName);
            Logger.DebugFormat("property   :[{0}]", property);

            // 結果設定
            bool result = true;

            #region 選択駅情報取得
            // StationSequencePropertyオブジェクト取得
            StationSequenceProperty stationSequenceProperty = sequences[stationName];

            // StationPropertyオブジェクト取得
            StationProperty stationProperty = stations[stationName];

            // StationTimePropertyオブジェクト取得
            StationTimeProperty stationTimeProperty = StationTimes[stationName];
            #endregion

            #region 次駅情報取得
            // 次駅
            StationSequenceProperty nextStationSequenceProperty = sequences.Next(Direction, 1, stationName);

            // StationPropertyオブジェクト取得
            StationProperty nextStationProperty = stations[nextStationSequenceProperty?.Name];

            // StationTimePropertyオブジェクト取得
            StationTimeProperty nextStationTimeProperty = StationTimes[nextStationSequenceProperty?.Name];
            #endregion

            do
            {
                // 結合対象TrainProperty判定
                if (property == null)
                {
                    // 結果設定
                    result = false;

                    // 終了
                    break;
                }

                // 列車番号判定
                if (No != property.No)
                {
                    // 結果設定
                    result = false;

                    // 終了
                    break;
                }

                // 終了
                break;
            } while (true);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= TrainProperty::IsJoin(StationProperties, StationSequenceProperties, string, TrainProperty)");

            // 返却
            return result;
        }

        /// <summary>
        /// 結合
        /// </summary>
        /// <param name="stations"></param>
        /// <param name="sequences"></param>
        /// <param name="stationName"></param>
        /// <param name="property"></param>
        public void Join(StationProperties stations, StationSequenceProperties sequences, string stationName, TrainProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainProperty::Join(StationProperties, StationSequenceProperties, string, TrainProperty)");
            Logger.DebugFormat("stations   :[{0}]", stations);
            Logger.DebugFormat("sequences  :[{0}]", sequences);
            Logger.DebugFormat("stationName:[{0}]", stationName);
            Logger.DebugFormat("property   :[{0}]", property);

            // StationSequencePropertyオブジェクト取得
            StationSequenceProperty targetStationSequence = sequences.Find(s => s.Name == stationName);

            // StationTimePropertiesオブジェクト生成
            StationTimeProperties stationTimeProperties = new StationTimeProperties();

            // StationTimePropertyオブジェクト
            StationTimeProperty stationTimeProperty = null;

            // 方向種別で分岐
            switch (Direction)
            {
                case DirectionType.Outbound:
                    // シーケンス番号を繰り返す
                    foreach (var sequence in sequences.OrderBy(s => s.Seq))
                    {
                        // シーケンス番号判定
                        if (sequence.Seq <= targetStationSequence.Seq)
                        {
                            // StationTimePropertyオブジェクト取得
                            stationTimeProperty = StationTimes.Find(s => s.StationName == sequence.Name);
                        }
                        else
                        {
                            // StationTimePropertyオブジェクト取得
                            stationTimeProperty = property.StationTimes.Find(s => s.StationName == sequence.Name);
                        }

                        // ID更新
                        stationTimeProperty.TrainId = Id;

                        // 登録
                        stationTimeProperties.Add(stationTimeProperty);
                    }
                    break;
                case DirectionType.Inbound:
                    // シーケンス番号を繰り返す
                    foreach (var sequence in sequences.OrderByDescending(s => s.Seq))
                    {
                        // シーケンス番号判定
                        if (sequence.Seq <= targetStationSequence.Seq)
                        {
                            // StationTimePropertyオブジェクト取得
                            stationTimeProperty = property.StationTimes.Find(s => s.StationName == sequence.Name);
                        }
                        else
                        {
                            // StationTimePropertyオブジェクト取得
                            stationTimeProperty = StationTimes.Find(s => s.StationName == sequence.Name);
                        }

                        // ID更新
                        stationTimeProperty.TrainId = Id;

                        // 登録
                        stationTimeProperties.Add(stationTimeProperty);
                    }
                    break;
                default:
                    throw new AggregateException(string.Format("方向種別の異常を検出しました:[{0}]", Direction));
            }

            // データ登録
            StationTimes.Copy(stationTimeProperties);

            // 発着駅設定
            DepartureArrivalStationOutboundSetting();

            // ロギング
            Logger.Debug("<<<<= TrainProperty::Join(StationProperties, StationSequenceProperties, string, TrainProperty)");
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

        #region 当駅始発設定
        /// <summary>
        /// 当駅始発設定
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sequences"></param>
        /// <param name="stationName"></param>
        public void StartStationSetting(DirectionType type, StationSequenceProperties sequences, string stationName)
        {
            // ロギング
            Logger.Debug("=>>>> TrainProperty::StartStationSetting(DirectionType, StationSequenceProperties, string)");
            Logger.DebugFormat("type       :[{0}]", type.GetStringValue());
            Logger.DebugFormat("sequences  :[{0}]", sequences);
            Logger.DebugFormat("stationName:[{0}]", stationName);

            // 設定
            DepartingStation = stationName;

            // シーケンスを取得
            IOrderedEnumerable<StationSequenceProperty> orderedSequence = null;
            switch (type)
            {
                case DirectionType.Outbound:
                    orderedSequence = sequences.OrderBy(s => s.Seq);
                    break;
                case DirectionType.Inbound:
                    orderedSequence = sequences.OrderByDescending(s => s.Seq);
                    break;
                default:
                    throw new AggregateException(string.Format("方向種別の異常を検出しました:[{0}]", type));
            }

            // シーケンスを繰り返す
            foreach (var sequence in orderedSequence)
            {
                // 始発駅に一致？
                if (sequence.Name == stationName)
                {
                    // 処理を終了
                    break;
                }

                // StationTimePropertyオブジェクト取得
                StationTimeProperty stationTime = StationTimes.Find(s => s.StationName == sequence.Name);

                // 時刻消去
                stationTime.EraseTime();
            }

            // ロギング
            Logger.Debug("<<<<= TrainProperty::StartStationSetting(DirectionType, StationSequenceProperties, string)");
        }
        #endregion

        #region 当駅止り設定
        /// <summary>
        /// 当駅止り設定
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sequences"></param>
        /// <param name="stationName"></param>
        public void StopStationSetting(DirectionType type, StationSequenceProperties sequences, string stationName)
        {
            // ロギング
            Logger.Debug("=>>>> TrainProperty::StopStationSetting(DirectionType, StationSequenceProperties, string)");
            Logger.DebugFormat("type       :[{0}]", type.GetStringValue());
            Logger.DebugFormat("sequences  :[{0}]", sequences);
            Logger.DebugFormat("stationName:[{0}]", stationName);

            // 設定
            DestinationStation = stationName;

            // シーケンスを取得
            IOrderedEnumerable<StationSequenceProperty> orderedSequence = null;
            switch (type)
            {
                case DirectionType.Outbound:
                    orderedSequence = sequences.OrderByDescending(s => s.Seq);
                    break;
                case DirectionType.Inbound:
                    orderedSequence = sequences.OrderBy(s => s.Seq);
                    break;
                default:
                    throw new AggregateException(string.Format("方向種別の異常を検出しました:[{0}]", type));
            }

            // シーケンスを繰り返す
            foreach (var sequence in orderedSequence)
            {
                // 始発駅に一致？
                if (sequence.Name == stationName)
                {
                    // 処理を終了
                    break;
                }

                // StationTimePropertyオブジェクト取得
                StationTimeProperty stationTime = StationTimes.Find(s => s.StationName == sequence.Name);

                // 時刻消去
                stationTime.EraseTime();
            }

            // ロギング
            Logger.Debug("<<<<= TrainProperty::StopStationSetting(DirectionType, StationSequenceProperties, string)");
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
            result.AppendLine(indentstr + string.Format("　ダイヤグラム名:[{0}] ", DiagramName));
            result.AppendLine(indentstr + string.Format("　方向種別      :[{0}] ", Direction.GetStringValue()));
            result.AppendLine(indentstr + string.Format("　ID            :[{0}] ", Id));
            result.AppendLine(indentstr + string.Format("　列車種別名    :[{0}] ", TrainTypeName));
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
