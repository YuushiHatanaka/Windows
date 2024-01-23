using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Util;
using System.Windows.Forms;
using TrainTimeTable.Common;
using TrainTimeTable.Component;
using TrainTimeTable.Property;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// DiagramPropertiesクラス
    /// </summary>
    [Serializable]
    public class DiagramProperties : List<DiagramProperty>
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DiagramProperties()
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::DiagramProperties()");

            // ロギング
            Logger.Debug("<<<<= DiagramProperties::DiagramProperties()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public DiagramProperties(DiagramProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::DiagramProperties(DiagramProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // コピー
            Copy(properties);

            // ロギング
            Logger.Debug("<<<<= DiagramProperties::DiagramProperties(DiagramProperties)");
        }
        #endregion

        #region インデックス取得
        /// <summary>
        /// インデックス取得
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        public int GetIndex(string name)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::string(name)");
            Logger.DebugFormat("name:[{0}]", name);

            // 要素を繰り返す
            for (int i = 0; i < Count; i++)
            {
                // 名称判定
                if (this[i].Name == name)
                {
                    // ロギング
                    Logger.DebugFormat("i:[{0}]", i);
                    Logger.Debug("<<<<= DiagramProperties::string(name)");

                    // 返却
                    return i;
                }
            }

            // ロギング
            Logger.DebugFormat("i:[該当データなし]");
            Logger.Debug("<<<<= DiagramProperties::string(name)");

            // 返却
            return -1;
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="properties"></param>
        public void Copy(DiagramProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::Copy(DiagramProperties)");
            Logger.DebugFormat("property:[{0}]", properties);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this, properties))
            {
                // クリア
                Clear();

                // 要素を繰り返す
                foreach (var property in properties)
                {
                    // 登録
                    Add(new DiagramProperty(property));
                }

            }

            // ロギング
            Logger.Debug("<<<<= DiagramProperties::Copy(DiagramProperties)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool Compare(DiagramProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::Compare(DiagramProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 要素数判定
            if (Count != properties.Count)
            {
                // ロギング
                Logger.DebugFormat("Count:[{0}][{1}][不一致]", Count, properties.Count);
                Logger.Debug("<<<<= DiagramProperties::Compare(DiagramProperties)");

                // 不一致
                return false;
            }

            // 要素を繰り返す
            int i = 0;
            foreach (var property in properties)
            {
                // 各要素比較
                if (!this[i].Compare(property))
                {
                    // ロギング
                    Logger.DebugFormat("Property:[不一致][{0}][{1}]", this[i].ToString(), property.ToString());
                    Logger.Debug("<<<<= DiagramProperties::Compare(DiagramProperties)");

                    // 不一致
                    return false;
                }

                // 要素インクリメント
                i++;
            }

            // ロギング
            Logger.Debug("result:[一致]");
            Logger.Debug("<<<<= DiagramProperties::Compare(DiagramProperties)");

            // 一致
            return true;
        }
        #endregion

        #region 駅関連
        #region 発着駅設定
        /// <summary>
        /// 発着駅設定
        /// </summary>
        public void DepartureArrivalStationSetting()
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::DepartureArrivalStationSetting()");

            // リストを繰り返す
            foreach (var property in this)
            {
                // 発着駅設定(方向)
                property.Trains.DepartureArrivalStationSetting();
            }

            // ロギング
            Logger.Debug("<<<<= DiagramProperties::DepartureArrivalStationSetting()");
        }
        #endregion

        #region 駅追加
        /// <summary>
        /// 駅追加
        /// </summary>
        /// <param name="addProperty"></param>
        public void AddStation(StationProperty addProperty)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::AddStation(StationProperty)");
            Logger.DebugFormat("addProperty:[{0}]", addProperty);

            // ダイヤ数分繰り返す
            foreach (var property in this)
            {
                // 列車下り上り分繰り返す
                foreach (var direction in property.Trains.Values)
                {
                    // 列車分繰り返す
                    foreach (var train in direction)
                    {
                        // StationTimePropertオブジェクト生成
                        StationTimeProperty stationTimeProperty = new StationTimeProperty()
                        {
                            DiagramName = property.Name,
                            Direction = train.Direction,
                            StationTreatment = StationTreatment.NoService,
                            TrainId = train.Id,
                            StationName = addProperty.Name,
                        };

                        // 登録
                        train.StationTimes.Add(stationTimeProperty);
                    }
                }
            }

            // ロギング
            Logger.Debug("<<<<= DiagramProperties::AddStation(StationProperty)");
        }
        #endregion

        #region 駅挿入
        /// <summary>
        /// 駅挿入
        /// </summary>
        /// <param name="index"></param>
        /// <param name="insertProperty"></param>
        public void InsertStation(int index, StationProperty insertProperty)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::InsertStation(int, StationProperty)");
            Logger.DebugFormat("index         :[{0}]", index);
            Logger.DebugFormat("insertProperty:[{0}]", insertProperty);

            // ダイヤ数分繰り返す
            foreach (var property in this)
            {
                // 列車下り上り分繰り返す
                foreach (var direction in property.Trains.Values)
                {
                    // 列車分繰り返す
                    foreach (var train in direction)
                    {
                        // StationTimePropertオブジェクト生成
                        StationTimeProperty stationTimeProperty = new StationTimeProperty()
                        {
                            DiagramName = property.Name,
                            Direction = train.Direction,
                            StationTreatment = StationTreatment.NoService,
                            TrainId = train.Id,
                            StationName = insertProperty.Name,
                        };

                        // 登録
                        train.StationTimes.Insert(index, stationTimeProperty);
                    }
                }
            }

            // ロギング
            Logger.Debug("<<<<= DiagramProperties::InsertStation(int, StationProperty)");
        }
        #endregion

        #region 駅削除
        /// <summary>
        /// 駅削除
        /// </summary>
        /// <param name="removeProperty"></param>
        public void RemoveStation(StationProperty removeProperty)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::RemoveStation(StationProperty)");
            Logger.DebugFormat("removeProperty:[{0}]", removeProperty);

            // ダイヤ数分繰り返す
            foreach (var property in this)
            {
                // 列車下り上り分繰り返す
                foreach (var direction in property.Trains.Values)
                {
                    // 列車分繰り返す
                    foreach (var train in direction)
                    {
                        // 削除オブジェクト取得
                        StationTimeProperty removeStationTimeProperty = train.StationTimes.Find(s => s.StationName == removeProperty.Name);

                        // 削除
                        train.StationTimes.Remove(removeStationTimeProperty);
                    }
                }
            }

            // ロギング
            Logger.Debug("<<<<= DiagramProperties::RemoveStation(StationProperty)");
        }
        #endregion

        #region 駅名変更
        /// <summary>
        /// 駅名変更
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public void ChangeStationName(string oldName, string newName)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::ChangeStationName(string, string)");
            Logger.DebugFormat("oldName:[{0}]", oldName);
            Logger.DebugFormat("newName:[{0}]", newName);

            // ダイヤ数分繰り返す
            foreach (var property in this)
            {
                // 列車下り上り分繰り返す
                foreach (var direction in property.Trains.Values)
                {
                    // 列車分繰り返す
                    foreach (var train in direction)
                    {
                        // 発駅
                        if (train.DepartingStation == oldName)
                        {
                            // 旧駅名⇒新駅名変換
                            train.DepartingStation = newName;
                        }

                        // 着駅
                        if (train.DestinationStation == oldName)
                        {
                            // 旧駅名⇒新駅名変換
                            train.DestinationStation = newName;
                        }

                        // 旧駅名⇒新駅名変換
                        train.StationTimes.FindAll(t => t.StationName == oldName).ForEach(t => t.StationName = newName);
                    }
                }
            }

            // ロギング
            Logger.Debug("<<<<= DiagramProperties::ChangeStationName(string, string)");
        }
        #endregion
        #endregion

        #region 列車種別関連
        #region 列車種別変更
        /// <summary>
        /// 列車種別変更
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public void ChangeTrainType(string oldName, string newName)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::ChangeTrainType(string, string)");
            Logger.DebugFormat("oldName:[{0}]", oldName);
            Logger.DebugFormat("newName:[{0}]", newName);

            // ダイヤ数分繰り返す
            foreach (var property in this)
            {
                // 列車下り上り分繰り返す
                foreach (var direction in property.Trains.Values)
                {
                    // 列車種別が一致する列車を全て検索して変換
                    direction.FindAll(t => t.TrainTypeName == oldName).ForEach(t => t.TrainTypeName = newName);
                }
            }

            // ロギング
            Logger.Debug("<<<<= DiagramProperties::ChangeTrainType(string, string)");
        }
        #endregion

        #region 列車種別削除
        /// <summary>
        /// 列車種別削除
        /// </summary>
        /// <param name="removeProperty"></param>
        public void RemoveTrainType(TrainTypeProperty removeProperty)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::RemoveTrainType(StationProperty)");
            Logger.DebugFormat("removeProperty:[{0}]", removeProperty);

            // ダイヤ数分繰り返す
            foreach (var property in this)
            {
                // 列車下り上り分繰り返す
                foreach (var direction in property.Trains.Values)
                {
                    // 列車種別が一致する列車を全て検索
                    List<TrainProperty> targetTrain = direction.FindAll(t => t.TrainTypeName == removeProperty.Name);

                    // 検索結果を繰り返す
                    foreach (TrainProperty train in targetTrain)
                    {
                        // 列車種別名をクリア
                        train.TrainTypeName = string.Empty;
                    }
                }
            }

            // ロギング
            Logger.Debug("<<<<= DiagramProperties::RemoveTrainType(StationProperty)");
        }
        #endregion
        #endregion

        #region 列車関連
        #region 列車追加
        /// <summary>
        /// 列車追加
        /// </summary>
        /// <param name="type"></param>
        /// <param name="property"></param>
        public void AddTrain(DirectionType type, TrainProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::AddTrain(DirectionType, TrainProperty)");
            Logger.DebugFormat("type    :[{0}]", type.GetStringValue());
            Logger.DebugFormat("property:[{0}]", property);

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = Find(t => t.Name == property.DiagramName);

            // TrainPropertiesオブジェクト取得
            TrainProperties trainProperties = diagramProperty.Trains[type];

            // TrainSequencePropertiesオブジェクト取得
            TrainSequenceProperties trainSequenceProperties = diagramProperty.TrainSequence[type];

            // 登録新IDを取得
            int newId = trainSequenceProperties.GetNewId();

            // ID設定
            property.Id = newId;

            // 列車挿入
            trainProperties.Add(property);
            trainSequenceProperties.Add(new TrainSequenceProperty(property));

            // シーケンス番号再構築
            trainSequenceProperties.SequenceNumberReconstruction();

            // ロギング
            Logger.Debug("<<<<= DiagramProperties::AddTrain(DirectionType, TrainProperty)");
        }
        #endregion

        #region 列車挿入
        /// <summary>
        /// 列車挿入
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <param name="property"></param>
        public void InsertTrain(DirectionType type, int index, TrainProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::InsertTrain(DirectionType, int, TrainProperty)");
            Logger.DebugFormat("type    :[{0}]", type.GetStringValue());
            Logger.DebugFormat("index   :[{0}]", index);
            Logger.DebugFormat("property:[{0}]", property);

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = Find(t => t.Name == property.DiagramName);

            // TrainPropertiesオブジェクト取得
            TrainProperties trainProperties = diagramProperty.Trains[type];

            // TrainSequencePropertiesオブジェクト取得
            TrainSequenceProperties trainSequenceProperties = diagramProperty.TrainSequence[type];

            // 登録新IDを取得
            int newId = trainSequenceProperties.GetNewId();

            // ID設定
            property.Id = newId;

            // 列車挿入
            trainProperties.Insert(index, property);
            trainSequenceProperties.Insert(index, new TrainSequenceProperty(property));

            // シーケンス番号再構築
            trainSequenceProperties.SequenceNumberReconstruction();

            // ロギング
            Logger.Debug("<<<<= DiagramProperties::InsertTrain(DirectionType, int, TrainProperty)");
        }
        #endregion

        #region 列車削除
        /// <summary>
        /// 列車削除
        /// </summary>
        /// <param name="type"></param>
        /// <param name="property"></param>
        public void RemoveTrain(DirectionType type, TrainProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::RemoveTrain(DirectionType, TrainProperty)");
            Logger.DebugFormat("type    :[{0}]", type.GetStringValue());
            Logger.DebugFormat("property:[{0}]", property);

            // DiagramPropertyオブジェクト取得
            DiagramProperty diagramProperty = Find(t => t.Name == property.DiagramName);

            // TrainPropertiesオブジェクト取得
            TrainProperties trainProperties = diagramProperty.Trains[type];

            // TrainSequencePropertiesオブジェクト取得
            TrainSequenceProperties trainSequenceProperties = diagramProperty.TrainSequence[type];

            // TrainPropertyオブジェクト取得
            TrainProperty trainProperty = diagramProperty.Trains[type].Find(t=>t.Id == property.Id);

            // TrainSequencePropertyオブジェクト取得
            TrainSequenceProperty trainSequenceProperty = diagramProperty.TrainSequence[type].Find(t => t.Id == property.Id);

            // 列車挿入
            trainProperties.Remove(trainProperty);
            trainSequenceProperties.Remove(trainSequenceProperty);

            // シーケンス番号再構築
            trainSequenceProperties.SequenceNumberReconstruction();

            // ロギング
            Logger.Debug("<<<<= DiagramProperties::RemoveTrain(DirectionType, TrainProperty)");
        }
        #endregion
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

            // リストを繰り返す
            int i = 0;
            foreach (var property in this)
            {
                // 文字列追加
                result.AppendLine(indentstr + string.Format("《配列番号:[{0}]》", i++));
                result.Append(property.ToString(indent + 1));
            }

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
