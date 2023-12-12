using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TrainTimeTable.Common;
using TrainTimeTable.Property;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// StationPropertiesクラス
    /// </summary>
    [Serializable]
    public class StationProperties : List<StationProperty>
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
        public StationProperties()
        {
            // ロギング
            Logger.Debug("=>>>> StationProperties::StationProperties()");

            // ロギング
            Logger.Debug("<<<<= StationProperties::StationProperties()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public StationProperties(StationProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> StationProperties::StationProperties(StationProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // コピー
            Copy(properties);

            // ロギング
            Logger.Debug("<<<<= StationProperties::StationProperties(StationProperties)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="properties"></param>
        public void Copy(StationProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> StationProperties::Copy(StationProperties)");
            Logger.DebugFormat("property:[{0}]", properties);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this ,properties))
            {
                // クリア
                Clear();

                // 要素を繰り返す
                foreach (var property in properties)
                {
                    // 登録
                    Add(new StationProperty(property));
                }
            }

            // ロギング
            Logger.Debug("<<<<= StationProperties::Copy(StationProperties)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool Compare(StationProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> StationProperties::Compare(StationProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 要素数判定
            if (Count != properties.Count)
            {
                // ロギング
                Logger.DebugFormat("Count:[{0}][{1}][不一致]", Count, properties.Count);
                Logger.Debug("<<<<= StationProperties::Compare(StationProperties)");

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
                    Logger.Debug("<<<<= StationProperties::Compare(StationProperties)");

                    // 不一致
                    return false;
                }

                // 要素インクリメント
                i++;
            }

            // ロギング
            Logger.Debug("result:[一致]");
            Logger.Debug("<<<<= StationProperties::Compare(StationProperties)");

            // 一致
            return true;
        }
        #endregion

        #region 次駅設定
        /// <summary>
        /// 次駅設定
        /// </summary>
        public void SetNextStation()
        {
            // ロギング
            Logger.Debug("=>>>> StationProperties::SetNextStation()");

            // 駅数判定
            if (Count == 0)
            {
                // ロギング
                Logger.Debug("<<<<= StationProperties::SetNextStation()");

                // 何もしない
                return;
            }

            // 駅登録分繰り返す(昇順)
            for (int i = 0; i < Count; i++)
            {
                // StationProperty設定
                StationProperty property = this[i];

                // 念のため上り情報を一時削除
                property.NextStations.RemoveAll(station => station.Direction == DirectionType.Inbound);

                // 次駅リストを繰り返す
                foreach (var nextStation in property.NextStations)
                {
                    // 次駅のシーケンス番号を駅リストから検索
                    var findStations = FindAll(station => station.Seq == nextStation.NextStationSeq);

                    // 見つからなかった場合
                    if (findStations.Count == 0)
                    {
                        // 次駅設定
                        nextStation.NextStationSeq = 0;
                        continue;
                    }
                    // 1件見つかった場合
                    else if (findStations.Count == 1)
                    {
                        // TODO:未実装
                    }
                    // 複数見つかった場合
                    else
                    {
                        // TODO:未実装
                    }
                }
            }

            // 終着駅距離
            float terminalStationDistance = this[Count - 1].StationDistanceFromReferenceStations[DirectionType.Outbound];

            // 駅登録分繰り返す(降順)
            for (int i = Count - 1; i >= 0; i--)
            {
                // StationProperty設定
                StationProperty stationProperty = this[i];

                // 駅距離計算
                stationProperty.StationDistanceFromReferenceStations[DirectionType.Inbound] = terminalStationDistance - stationProperty.StationDistanceFromReferenceStations[DirectionType.Outbound]; ;

                // NextStationPropertyオブジェクト生成
                NextStationProperty nextStation = new NextStationProperty()
                {
                    Name = stationProperty.Name,
                    NextStationSeq = stationProperty.Seq - 1,
                    Direction = DirectionType.Inbound
                };

                // 次駅のシーケンス番号を駅リストから検索
                var findStations = FindAll(station => station.Seq == nextStation.NextStationSeq);

                // 見つからなかった場合
                if (findStations.Count == 0)
                {
                    // 次駅設定
                    nextStation.NextStationSeq = 0;

                    // 登録
                    stationProperty.NextStations.Add(nextStation);
                }
                // 1件見つかった場合
                else if (findStations.Count == 1)
                {
                    // 登録
                    stationProperty.NextStations.Add(nextStation);
                }
                // 複数見つかった場合
                else
                {
                    // TODO:未実装
                }
            }

            // ロギング
            Logger.Debug("<<<<= StationProperties::SetNextStation()");
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
